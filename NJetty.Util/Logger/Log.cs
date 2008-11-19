#region Apache License
//
// Licensed to the Apache Software Foundation (ASF) under one or more 
// contributor license agreements. See the NOTICE file distributed with
// this work for additional information regarding copyright ownership. 
// The ASF licenses this file to you under the Apache License, Version 2.0
// (the "License"); you may not use this file except in compliance with 
// the License. You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
#endregion


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Configuration;


namespace NJetty.Util.Logger
{
    /// <summary>
    ///  This class provides a static logging interface.  If an instance of the 
    ///  NLog Assembly is found on the classpath, the static log methods
    ///  are directed to a NLog logger for "NJetty.Util.Log".   Otherwise the logs
    ///  are directed to Console.
    ///  
    ///  If the system property VERBOSE is set, then ignored exceptions are logged in detail.
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// November 2008
    /// </date>
    public class Log
    {
        static readonly string[] __nestedEx = new string[] { "TargetException", "TargetError", "Exception", "RootCause" };
        static readonly Type[] __noArgs = new Type[0];

        public static string EXCEPTION = "EXCEPTION ";
        public static string IGNORED = "IGNORED ";
        public static string IGNORED_FMT = "IGNORED: {0} ";
        public static string NOT_IMPLEMENTED = "NOT IMPLEMENTED ";
        
        static string __logType = ConfigurationManager.AppSettings["NJetty.Log"] ?? "NJetty.Util.Logger.NLogLog";
        static bool __verbose =  
            !string.IsNullOrEmpty(ConfigurationManager.AppSettings["VERBOSE"]) 
            ? "true".Equals(ConfigurationManager.AppSettings["VERBOSE"], StringComparison.OrdinalIgnoreCase) 
            : false;

        static bool __ignored =   
            !string.IsNullOrEmpty(ConfigurationManager.AppSettings["IGNORED"]) 
            ? "true".Equals(ConfigurationManager.AppSettings["IGNORED"], StringComparison.OrdinalIgnoreCase) 
            : false;

        static ILogger __log;

        static bool _initialized;
        static object staticThreadLock = new object();

        public static bool Initialized()
        {
            if (__log != null)
                return true;

            lock (staticThreadLock)
            {
                if (_initialized)
                    return __log != null;
                _initialized = true;
            }

            Type log_type = null;
            try
            {
                log_type = Type.GetType(__logType, true);
                if (__log == null || __log.GetType() != log_type)
                {
                    __log = (ILogger)Activator.CreateInstance(log_type);
                    __log.Info("Logging to {0} via {1}", __log, log_type.ToString());
                }
            }
            catch (Exception e)
            {
                if (__log == null)
                {
                    log_type = typeof(StdErrLog);

                    __log = new StdErrLog();
                    __log.Info("Logging to {0} via {1}", __log, log_type.ToString());
                    if (__verbose)
                    {
                        Console.Error.WriteLine(e.StackTrace);
                    }
                }
            }

            return __log != null;
        }


        public static ILogger Logger
        {
            set { Log.__log = value; }
            get { Initialized(); return __log; }
        }


       

        // TODO: change comment to fit for NJetty
        /// <summary>
        /// Set Log to parent Logger.
        /// <p>
        /// If there is a different Log class available,
        /// call GetLogger(String) on it and construct a LoggerLog instance
        /// as this Log's Logger, so that logging is delegated to the parent Log.
        /// <p>
        /// This should be used if a webapp is using Log, but wishes the logging to be 
        /// directed to the containers log.
        /// <p>
        /// If there is not parent Log, then this call is equivalent to
        /// <pre>
        /// Log.Logger = Log.GetLogger(name);
        /// </pre>
        /// 
        /// </summary>
        /// <param name="name">Logger Name</param>

         


        public static void SetLogToParent(string name)
        {

            

            //ClassLoader loader = Log.class.getClassLoader();
            //if (loader.getParent()!=null)
            //{
                try
                {
                    //TODO: this is not complete




                    //Class<?> uberlog = loader.getParent().loadClass("org.mortbay.log.Log");
                    //Method getLogger=uberlog.getMethod("getLogger",new Class[]{String.class});
                    //Object logger = getLogger.invoke(null,name);
                    //setLog(new LoggerLog(logger));
                    //return;

                    //object logger = null;
                    // get the logger objecthere by calling TheLog.GetLogger(name)

                    Log.Logger = new LoggerLog(Logger);
                    return;
                    

                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(e.StackTrace);
                }     
            //}

            Logger = GetLogger(name);

            
        }

        public static void Debug(Exception exception)
        {
            if (!IsDebugEnabled)
            {
                return;
            }

            __log.Debug(EXCEPTION, exception);
            Unwind(exception);
        }

        public static void Debug(string msg, params object[] args)
        {
            if (!Initialized())
                return;
            __log.Debug(msg, args);
        }


        
         
        /// <summary>
        /// Ignore an exception unless trace is enabled.
        /// This works around the problem that log4j does not support the trace level.
        /// </summary>
        /// <param name="exception"></param>
        public static void Ignore(Exception exception)
        {
            if (!Initialized())
                return;
            if (__ignored)
            {
                __log.Warn(IGNORED, exception);
                Unwind(exception);
            }
            else if (__verbose)
            {
                __log.Warn(IGNORED, exception);
                Unwind(exception);
            }
        }

        public static void Info(string msg, params object[] args)
        {
            if (!Initialized())
                return;
            __log.Info(msg, args);
        }

        public static bool IsDebugEnabled
        {
            get
            {
                if (!Initialized())
                    return false;
                return __log.IsDebugEnabled;
            }
        }

        public static void Warn(string msg, params object[] args)
        {
            if (!Initialized())
                return;
            __log.Warn(msg, args);
        }

        public static void Warn(string msg, Exception exception)
        {
            if (!IsDebugEnabled)
            {
                return;
            }

            __log.Debug(msg, exception);
            Unwind(exception);
        }

        public static void Warn(Exception exception)
        {
            if (!IsDebugEnabled)
            {
                return;
            }

            __log.Debug(EXCEPTION, exception);
            Unwind(exception);
        }

        
        /// <summary>
        /// Obtain a named Logger.
        /// Obtain a named Logger or the default Logger if null is passed.
        /// </summary>
        /// <param name="name">Log Name</param>
        /// <returns>The Currently Used Log</returns>
        public static ILogger GetLogger(string name)
        {
            if (!Initialized())
                return null;

            if (string.IsNullOrEmpty(name))
                return __log;

            return __log.GetLogger(name);
        }

        private static void Unwind(Exception exception)
        {
            if (exception == null)
            {
                return;
            }


            // TODO: more research on custom Exceptions of jetty
            // use the Dotnet Exception Style if this is not applicable
            //get property values for "TargetException", "TargetError", "Exception", "RootCause"
            foreach (string nestedEx in __nestedEx)
            {
                try
                {
                    PropertyInfo propTarget = exception.GetType().GetProperty(nestedEx);
                    Exception ex = (Exception)propTarget.GetValue(exception, null);
                    if (ex != null && ex != exception)
                    {
                        Warn("Nested in " + exception + ":", ex);
                    }
                    
                }
                catch{ }
            }
            
        }
    }
}

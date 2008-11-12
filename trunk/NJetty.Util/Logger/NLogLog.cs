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



namespace NJetty.Util.Logger
{

    /// <summary>
    /// TODO: Class/Interface Information here
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// November 2008
    /// </date>
    public class NLogLog : ILogger
    {
        #region NLogger Reflection Items

        const string LOGGER_ASSEMBLY = "NLog";
        const string LOGGER = "NLog.Logger";
        const string LOGGER_FACTORY = "NLog.LogManager";
        const string DEFAULT_LOG_NAME = "NJetty.Util.Log";


        readonly static object[] NO_ARGS = new object[] { };
        readonly static Type[] MSG_LOG_ARGS = new Type[] { typeof(string), typeof(object[]) };
        readonly static Type[] MSG_EXCEPTION_LOG_ARGS = new Type[] { typeof(string), typeof(Exception) };

        Assembly nlogAssembly;

        MethodInfo info;
        MethodInfo debug;
        MethodInfo debugException;
        PropertyInfo debugEnabled;
        MethodInfo warn;
        MethodInfo warnException;
        MethodInfo errorException;
        PropertyInfo loggerName;
        MethodInfo getLogger;

        // the NLog.Logger object
        object logger;



        void InitializeReflectionMethods()
        {
            Type nLog = nlogAssembly.GetType(LOGGER, true, true);
            Type nLogF = nlogAssembly.GetType(LOGGER_FACTORY, true, true);

            // Methods Reflected

            info = nLog.GetMethod("Info", MSG_LOG_ARGS);
            debug = nLog.GetMethod("Debug", MSG_LOG_ARGS);
            debugException = nLog.GetMethod("DebugException", MSG_EXCEPTION_LOG_ARGS);
            debugEnabled = nLog.GetProperty("IsDebugEnabled");
            warn = nLog.GetMethod("Warn", MSG_LOG_ARGS); ;
            warnException = nLog.GetMethod("WarnException", MSG_EXCEPTION_LOG_ARGS);
            errorException = nLog.GetMethod("ErrorException", MSG_EXCEPTION_LOG_ARGS);
            loggerName = nLog.GetProperty("Name");


            getLogger = nLogF.GetMethod("GetLogger", new Type[] { typeof(string) });
            
        }



        #endregion

        #region Constructors

        public NLogLog()
            : this(DEFAULT_LOG_NAME)
        {

        }

        public NLogLog(string name)
        {   
            try
            {
                nlogAssembly = Assembly.Load(LOGGER_ASSEMBLY);
            }
            catch(Exception e)
            {
                throw new Exception("NLog Assembly Not Found", e);
            }
            InitializeReflectionMethods();
            logger = getLogger.Invoke(null, new object[] { name });
        }

        #endregion

        #region ILogger Members

        public bool IsDebugEnabled
        {
            get
            {
                try
                {
                    return (bool)debugEnabled.GetValue(logger, null);
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(e.StackTrace);
                    return true;
                }
            }
            set
            {
                Warn("Setting of Property IsDebugEnabled not implemented");
            }
        }

        public void Info(string msg, params object[] args)
        {
            try
            {
                info.Invoke(logger, new object[] { msg, args });
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.StackTrace);
            }
        }

        public void Debug(string msg, Exception exception)
        {
            try
            {
                debugException.Invoke(logger, new object[] { msg, exception });
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.StackTrace);
            }
        }

        public void Debug(string msg, params object[] args)
        {
            try
            {
                debug.Invoke(logger, new object[] { msg, args });
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.StackTrace);
            }
        }

        public void Warn(string msg, params object[] args)
        {
            try
            {
                warn.Invoke(logger, new object[] { msg, args });
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.StackTrace);
            }
        }

        public void Warn(string msg, Exception exception)
        {
            try
            {
                if (exception is System.SystemException)
                {
                    errorException.Invoke(logger, new object[] { msg, exception });
                }
                else
                {
                    warnException.Invoke(logger, new object[] { msg, exception });
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.StackTrace);
            }
        }

        public ILogger GetLogger(string name)
        {
            try
            {
                return new NLogLog(name);
            }
            catch (Exception e)
            {
                Log.Warn(e);
                return this;
            }
        }

        #endregion


        public override string ToString()
        {
            return (string)loggerName.GetValue(logger, null);
        }
    }
}

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
    public class LoggerLog : ILogger
    {

        #region LoggerLog Reflection Items



        readonly static object[] NO_ARGS = new object[] { };
        readonly static Type[] MSG_LOG_ARGS = new Type[] { typeof(string), typeof(object[]) };
        readonly static Type[] MSG_EXCEPTION_LOG_ARGS = new Type[] { typeof(string), typeof(Exception) };

        
        MethodInfo info;
        MethodInfo debug;
        MethodInfo debugException;
        PropertyInfo debugEnabled;
        MethodInfo warn;
        MethodInfo warnException;
        MethodInfo errorException;
        PropertyInfo loggerName;
        MethodInfo getLogger;
        
        // the Logger object
        
        object logger;
        object loggerFactory;





        #endregion

        
        #region Constructors


        public LoggerLog(object logger)
        {
            this.logger = logger;

            Type logType = logger.GetType();

            // Methods Reflected

            info = logType.GetMethod("Info", MSG_LOG_ARGS);
            debug = logType.GetMethod("Debug", MSG_LOG_ARGS);
            debugException = logType.GetMethod("DebugException", MSG_EXCEPTION_LOG_ARGS);
            debugEnabled = logType.GetProperty("IsDebugEnabled");
            warn = logType.GetMethod("Warn", MSG_LOG_ARGS); ;
            warnException = logType.GetMethod("WarnException", MSG_EXCEPTION_LOG_ARGS);
            errorException = logType.GetMethod("ErrorException", MSG_EXCEPTION_LOG_ARGS);
            loggerName = logType.GetProperty("Name");

            PropertyInfo lfPropInfo = logType.GetProperty("Factory");
            loggerFactory = lfPropInfo.GetValue(logger, null);

            getLogger = lfPropInfo.PropertyType.GetMethod("GetLogger", new Type[] { typeof(string) });
            
            
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
                try
                {
                    debugEnabled.SetValue(logger, value, null);
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(e.StackTrace);
                }
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
                object logger = getLogger.Invoke(loggerFactory, new object[] { name });
                return new LoggerLog(logger);
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

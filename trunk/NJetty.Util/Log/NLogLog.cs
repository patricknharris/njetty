using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reflection;

namespace NJetty.Util.Log
{
    public class NLogLog : ILogger
    {
        #region NLogger Reflection Items

        const string LOGGER_ASSEMBLY = "NLog";
        const string LOGGER = "NLog.Logger";
        const string LOGGER_FACTORY = "NLog.LogManager";
        const string DEFAULT_LOG_NAME = "NJetty.Util.Log";


        readonly object[] NO_ARGS = new object[] { };

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

            info = nLog.GetMethod("Info", new Type[] { typeof(string), typeof(object[]) });
            debug = nLog.GetMethod("Debug", new Type[] { typeof(string), typeof(object[]) });
            debugException = nLog.GetMethod("DebugException", new Type[] { typeof(string), typeof(Exception) });
            debugEnabled = nLog.GetProperty("IsDebugEnabled");
            warn = nLog.GetMethod("Warn", new Type[] { typeof(string), typeof(object[]) }); ;
            warnException = nLog.GetMethod("WarnException", new Type[] { typeof(string), typeof(Exception) });
            errorException = nLog.GetMethod("ErrorException", new Type[] { typeof(string), typeof(Exception) });
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
                    return (bool)debugEnabled.GetValue(logger, new object[] { });
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
                    errorException.Invoke(logger, new object[] { msg, exception });
                else
                    warnException.Invoke(logger, new object[] { msg, exception });
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
                Log.warn(e);
                return this;
            }
        }

        #endregion


        public override string ToString()
        {
            return (string)loggerName.GetValue(logger, new object[] { });
        }
    }
}

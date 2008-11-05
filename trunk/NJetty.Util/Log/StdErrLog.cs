using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NJetty.Util.Log
{
    public class StdErrLog : ILogger
    {

        #region Static members


        //private static DateCache _dateCache; TODO date cache implementation
        static bool _debug = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DEBUG")); //System.getProperty("DEBUG",null)!=null;
        string _name;
        bool _hideStacks=false;
    
        static StdErrLog()
        {
            try
            {
                // TODO: use datecache
                //_dateCache=new DateCache("yyyy-MM-dd HH:mm:ss");
            }
            catch(Exception e)
            {
                Console.Error.WriteLine(e.StackTrace);
            }
            
        }


        #endregion


        #region Constructors

        public StdErrLog() : this(null)
        {
            
        }

        public StdErrLog(String name)
        {
            this._name = name ?? string.Empty;
        }

        #endregion


        public bool HideStacks
        {
            get { return _hideStacks; }
            set { _hideStacks = value; }
        }


        #region Helper Methods

        private string getLogPrefix(string logType)
        {
            // TODO: use date time cache
            return string.Format("{0:ddd MMM dd HH:mm:ss zzz yyyy}:{1}:{2}", DateTime.Now, _name, logType);

        }

        #endregion


        #region ILogger Members

        public bool IsDebugEnabled
        {
            get{ return _debug; }
            set{ _debug = value; }
        }

        public void Info(string msg, params object[] args)
        {
            string log = string.Format(msg, args);
            Console.Error.WriteLine(string.Format("{0} : {1}", getLogPrefix("INFO"), log));
        }

        public void Debug(string msg, Exception exception)
        {
            if (_debug)
            {
                Console.Error.WriteLine(string.Format("{0} : {1}", getLogPrefix("DEBUG"), msg));
                if (exception != null)
                {
                    if (_hideStacks)
                        Console.Error.WriteLine(exception.Message);
                    else
                        Console.Error.WriteLine(exception.StackTrace);
                }
            }
        }

        public void Debug(string msg, params object[] args)
        {
            if (_debug)
            {
                string log = string.Format(msg, args);
                Console.Error.WriteLine(string.Format("{0} : {1}", getLogPrefix("DEBUG"), log));
            }
        }

        public void Warn(string msg, params object[] args)
        {
            string log = string.Format(msg, args);
            Console.Error.WriteLine(string.Format("{0} : {1}", getLogPrefix("WARN"), log));
        }

        public void Warn(string msg, Exception exception)
        {

            Console.Error.WriteLine(string.Format("{0} : {1}", getLogPrefix("WARN"), msg));
            if (exception != null)
            {
                if (_hideStacks)
                    Console.Error.WriteLine(exception.Message);
                else
                    Console.Error.WriteLine(exception.StackTrace);
            }

        }

        public ILogger GetLogger(string name)
        {
            if ((name == null && this._name == null) ||
            (name != null && name.Equals(this._name)))
                return this;
            return new StdErrLog(name);
        }

        #endregion


        public override string ToString()
        {
            return "STDERR" + _name;
        }
    }
}

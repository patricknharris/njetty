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
using System.Configuration;
using NJetty.Util.Util;



namespace NJetty.Util.Logging
{

    /// <summary>
    /// StdErr Logging.
    /// This implementation of the Logging facade sends all logs to StdErr with minimal formatting.
    /// 
    /// If the system property DEBUG is set, then debug logs are printed if stderr is being used.
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// November 2008
    /// </date>
    public class StdErrLog : ILogger
    {

        #region Static members


        static DateCache _dateCache; 
        static bool _debug = 
            !string.IsNullOrEmpty(ConfigurationManager.AppSettings["DEBUG"]) 
            ? "true".Equals(ConfigurationManager.AppSettings["DEBUG"], StringComparison.OrdinalIgnoreCase) 
            : false; 


        string _name;
        bool _hideStacks=false;
    
        static StdErrLog()
        {
            try
            {
                _dateCache=new DateCache("yyyy-MM-dd HH:mm:ss");
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

        public StdErrLog(string name)
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
            return string.Format("{0}:{1}:{2}", _dateCache.Now, _name, logType);
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

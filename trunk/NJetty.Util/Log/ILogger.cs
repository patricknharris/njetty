using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NJetty.Util.Log
{
    public interface ILogger
    {
        bool IsDebugEnabled
        {
            get;
            set;
        }

        void Info(string msg, params object[] args);
        void Debug(string msg, Exception exeption);
        void Debug(string msg, params object[] args);
        void Warn(string msg, params object[] args);
        void Warn(string msg, Exception exception);
        ILogger GetLogger(string name);
    }
}

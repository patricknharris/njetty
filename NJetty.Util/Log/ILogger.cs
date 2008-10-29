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

        void info(string msg, object arg0, object arg1);
        void debug(string msg, Exception th);
        void debug(string msg, object arg0, object arg1);
        void warn(string msg, object arg0, object arg1);
        void warn(string msg, Exception th);
        ILogger getLogger(string name);
    }
}

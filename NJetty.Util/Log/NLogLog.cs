using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NJetty.Util.Log
{
    public class NLogLog : ILogger
    {
        #region ILogger Members

        // TODO implementation here, this will act as the slf4j log


        public bool IsDebugEnabled
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void info(string msg, object arg0, object arg1)
        {
            throw new NotImplementedException();
        }

        public void debug(string msg, Exception th)
        {
            throw new NotImplementedException();
        }

        public void debug(string msg, object arg0, object arg1)
        {
            throw new NotImplementedException();
        }

        public void warn(string msg, object arg0, object arg1)
        {
            throw new NotImplementedException();
        }

        public void warn(string msg, Exception th)
        {
            throw new NotImplementedException();
        }

        public ILogger getLogger(string name)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

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
using NUnit.Framework;
using NJetty.Util.Logging;


namespace NJetty.Util.Test.Logging
{
    /// <summary>
    /// Test For Logs
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// November 2008
    /// </date>
    

    [TestFixture]
    public class LogTest
    {
        [Test]
        public void TestLoggerLog()
        {
            // TODO: LoggerLog Test Implementation
            
            //Logger log = new LoggerLog(Log.getLogger("test"));
            //log.setDebugEnabled(true);
            //log.debug("testing {} {}", "LoggerLog", "debug");
            //log.info("testing {} {}", "LoggerLog", "info");
            //log.warn("testing {} {}", "LoggerLog", "warn");
            //log.setDebugEnabled(false);
            //log.debug("YOU SHOULD NOT SEE THIS!", null, null);

            //log = log.getLogger("next");
            //log.info("testing {} {}", "LoggerLog", "info");
        }

        [Test]
        public void TestLog()
        {
            Log.Debug("testing {0} {1}", "LoggerLog", "debug");
            Log.Info("testing {0} {1}", "LoggerLog", "info");
            Log.Warn("testing {0} {1}", "LoggerLog", "warn");

        }
    }
}

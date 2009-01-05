#region Apache License
//
// Licensed to the Apache Software Foundation (ASF) under one or more 
// contributor license agreements. See the NOTICE file distributed with
// this work for Additional information regarding copyright ownership. 
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
using NJetty.Util.Util;
using System.Globalization;
using System.Threading;
using NJetty.Util.Logging;

namespace NJetty.Util.Test.Util
{
    /// <summary>
    /// DateCacheTest
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// January 2008
    /// </date>

    [TestFixture]
    public class DateCacheTest
    {
        [Test]
        public void TestDateCache()
        {
            DateCache dc = new DateCache("ddd, dd MMM yyyy HH:mm:ss zzz ZZZ",
                                         CultureInfo.CurrentCulture);
            dc.TimeZone = TimeZoneInfo.Utc;
            string last=dc.Format(DateTime.Now.Ticks);
            bool change=false;
            for (int i=0;i<15;i++)
            {
                Thread.Sleep(100);
                string date = dc.Format(DateTime.Now.Ticks);
                
                Assert.AreEqual(last.Substring(0,17),
                              date.Substring(0, 17), "Same Date");


                if (!last.Substring(17).Equals(date.Substring(17)))
                    change=true;
                else
                {
                    int lh=int.Parse(last.Substring(17,2));
                    int dh=int.Parse(date.Substring(17,2));
                    int lm=int.Parse(last.Substring(20,2));
                    int dm=int.Parse(date.Substring(20,2));
                    int ls=int.Parse(last.Substring(23,2));
                    int ds=int.Parse(date.Substring(23,2));

                    // This won't work at midnight!
                    change|= ds!=ls || dm!=lm || dh!=lh;
                }
                last=date;
            }
            Assert.IsTrue(change, "time changed");


            // Test string is cached
            dc = new DateCache();
            string s1=dc.Format(DateTime.Now.Ticks);
            dc.Format(1);
            string s2 = dc.Format(DateTime.Now.Ticks);
            dc.Format(DateTime.Now.Ticks + (10 * 60 * 60 * 1000) );
            string s3 = dc.Format(DateTime.Now.Ticks);
            Assert.IsTrue(s1==s2 || s2==s3);
        }
    }
}

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
using NJetty.Util.Util;


namespace NJetty.Util.Test.Util
{

    /// <summary>
    /// Test for UtfStringBuilder
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// November 2008
    /// </date>

    [TestFixture]
    public class Utf8StringBuilderTest
    {
        [Test]
        public void TestUtfStringBuilder1()
        {
            string source = "abcd012345\n\r\u0000\u00a4\u10fb\ufffdnjetty";

            for (int i = 0; i < 100; i++)
            {
                source += "abcd012345\n\r\u0000\u00a4\u10fb\ufffdnjetty";
            }

            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(source);
            Utf8StringBuilder buffer = new Utf8StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                buffer.Append(bytes[i]);
            }


            Assert.AreEqual(source, buffer.ToString());
            Assert.IsTrue(buffer.ToString().EndsWith("njetty")); 
        }

        [Test]
        public void TestUtfStringBuilder2()
        {
            string source = "abcd012345\n\r\u0000\u00a4\u10fb\ufffdnjetty";
            Utf8StringBuilder buffer = new Utf8StringBuilder();
            buffer.Append(source);

            for (int i = 0; i < 100; i++)
            {
                source += "abcd012345\n\r\u0000\u00a4\u10fb\ufffdnjetty";
                buffer.Append("abcd012345\n\r\u0000\u00a4\u10fb\ufffdnjetty");
            }




            Assert.AreEqual(source, buffer.ToString());
            Assert.IsTrue(buffer.ToString().EndsWith("njetty"));
        }
    }
}

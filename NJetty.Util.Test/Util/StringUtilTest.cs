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

namespace NJetty.Util.Test.Util
{
    /// <summary>
    /// StringUtilTest
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// November 2008
    /// </date>

    [TestFixture]
    public class StringUtilTest
    {
        [Test]
        public void TestAsciiToLowerCase()
        {
            string lc="\u0690bc def 1\u06903";
            Assert.AreEqual(StringUtil.AsciiToLowerCase("\u0690Bc DeF 1\u06903"), lc);
            Assert.IsTrue(StringUtil.AsciiToLowerCase(lc)==lc);
        }

        [Test]
        public void TestStartsWithIgnoreCase()
        {

            Assert.IsTrue(StringUtil.StartsWithIgnoreCase("\u0690b\u0690defg", "\u0690b\u0690"));
            Assert.IsTrue(StringUtil.StartsWithIgnoreCase("\u0690bcdefg", "\u0690bc"));
            Assert.IsTrue(StringUtil.StartsWithIgnoreCase("\u0690bcdefg", "\u0690Bc"));
            Assert.IsTrue(StringUtil.StartsWithIgnoreCase("\u0690Bcdefg", "\u0690bc"));
            Assert.IsTrue(StringUtil.StartsWithIgnoreCase("\u0690Bcdefg", "\u0690Bc"));
            Assert.IsTrue(StringUtil.StartsWithIgnoreCase("\u0690bcdefg", ""));
            Assert.IsTrue(StringUtil.StartsWithIgnoreCase("\u0690bcdefg", null));
            Assert.IsTrue(StringUtil.StartsWithIgnoreCase("\u0690bcdefg", "\u0690bcdefg"));

            Assert.IsFalse(StringUtil.StartsWithIgnoreCase(null, "xyz"));
            Assert.IsFalse(StringUtil.StartsWithIgnoreCase("\u0690bcdefg", "xyz"));
            Assert.IsFalse(StringUtil.StartsWithIgnoreCase("\u0690", "xyz"));
        }

        public void testEndsWithIgnoreCase()
        {
            Assert.IsTrue(StringUtil.EndsWithIgnoreCase("\u0690bcd\u0690f\u0690", "\u0690f\u0690"));
            Assert.IsTrue(StringUtil.EndsWithIgnoreCase("\u0690bcdefg", "efg"));
            Assert.IsTrue(StringUtil.EndsWithIgnoreCase("\u0690bcdefg", "eFg"));
            Assert.IsTrue(StringUtil.EndsWithIgnoreCase("\u0690bcdeFg", "efg"));
            Assert.IsTrue(StringUtil.EndsWithIgnoreCase("\u0690bcdeFg", "eFg"));
            Assert.IsTrue(StringUtil.EndsWithIgnoreCase("\u0690bcdefg", ""));
            Assert.IsTrue(StringUtil.EndsWithIgnoreCase("\u0690bcdefg", null));
            Assert.IsTrue(StringUtil.EndsWithIgnoreCase("\u0690bcdefg", "\u0690bcdefg"));

            Assert.IsFalse(StringUtil.EndsWithIgnoreCase(null, "xyz"));
            Assert.IsFalse(StringUtil.EndsWithIgnoreCase("\u0690bcdefg", "xyz"));
            Assert.IsFalse(StringUtil.EndsWithIgnoreCase("\u0690", "xyz"));
        }

        [Test]
        public void TestIndexFrom()
        {
            Assert.AreEqual(StringUtil.IndexFrom("\u0690bcd", "xyz"), -1);
            Assert.AreEqual(StringUtil.IndexFrom("\u0690bcd", "\u0690bcz"), 0);
            Assert.AreEqual(StringUtil.IndexFrom("\u0690bcd", "bcz"), 1);
            Assert.AreEqual(StringUtil.IndexFrom("\u0690bcd", "dxy"), 3);
        }

        [Test]
        public void TestReplace()
        {
            string s = "\u0690bc \u0690bc \u0690bc";
            Assert.AreEqual(StringUtil.Replace(s, "\u0690bc", "xyz"), "xyz xyz xyz");
            Assert.IsTrue(StringUtil.Replace(s, "xyz", "pqy") == s);

            s = " \u0690bc ";
            Assert.AreEqual(StringUtil.Replace(s, "\u0690bc", "xyz"), " xyz ");

        }

        [Test]
        public void TestUnquote()
        {
            string uq = " not quoted ";
            Assert.IsTrue(StringUtil.Unquote(uq) == uq);
            Assert.AreEqual(StringUtil.Unquote("' quoted string '"), " quoted string ");
            Assert.AreEqual(StringUtil.Unquote("\" quoted string \""), " quoted string ");
            Assert.AreEqual(StringUtil.Unquote("' quoted\"string '"), " quoted\"string ");
            Assert.AreEqual(StringUtil.Unquote("\" quoted'string \""), " quoted'string ");
        }

        [Test]
        public void TestNonNull()
        {
            string nn = "";
            Assert.IsTrue(nn == StringUtil.NonNull(nn));
            Assert.AreEqual("", StringUtil.NonNull(null));
        }

        /// <summary>
        /// Test for bool Equals(string, char[], int, int)
        /// </summary>
        public void testEqualsStringcharArrayintint()
        {
            Assert.IsTrue(StringUtil.Equals("\u0690bc", new char[] { 'x', '\u0690', 'b', 'c', 'z' }, 1, 3));
            Assert.IsFalse(StringUtil.Equals("axc", new char[] { 'x', 'a', 'b', 'c', 'z' }, 1, 3));
        }

        [Test]
        public void TestAppend()
        {
            StringBuilder buf = new StringBuilder();
            buf.Append('a');
            StringUtil.Append(buf, "abc", 1, 1);
            StringUtil.Append(buf, (byte)12, 16);
            StringUtil.Append(buf, (byte)16, 16);
            StringUtil.Append(buf, unchecked((byte)-1), 16);
            StringUtil.Append(buf, unchecked((byte)-16), 16);
            Assert.AreEqual("ab0c10fff0", buf.ToString());

        }
        
    }
}

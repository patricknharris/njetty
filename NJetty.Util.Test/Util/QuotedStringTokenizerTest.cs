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
    /// QuotedStringTokenizerTest
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// November 2008
    /// </date>
    
    [TestFixture]
    public class QuotedStringTokenizerTest
    {



        private void CheckTok(QuotedStringTokenizer tok, bool delim, bool quotes)
        {
            Assert.IsTrue(tok.HasMoreElements());
            Assert.IsTrue(tok.hasMoreTokens());
            Assert.AreEqual("abc", tok.NextToken());
            if (delim) Assert.AreEqual(",", tok.NextToken());
            if (delim) Assert.AreEqual(" ", tok.NextToken());

            Assert.AreEqual(quotes ? "\"d\\\"'\"" : "d\"'", tok.NextElement());
            if (delim) Assert.AreEqual(",", tok.NextToken());
            Assert.AreEqual(quotes ? "'p\\',y'" : "p',y", tok.NextToken());
            if (delim) Assert.AreEqual(" ", tok.NextToken());
            Assert.AreEqual("z", tok.NextToken());
            Assert.IsFalse(tok.hasMoreTokens());
        }

        [Test]
        public void TestNextToken()
        {
            QuotedStringTokenizer tok =
                new QuotedStringTokenizer("abc\n\"d\\\"'\"\n'p\\',y'\nz");
            CheckTok(tok, false, false);
        }

        [Test]
        public void TestNextToken1()
        {
            QuotedStringTokenizer tok =
                new QuotedStringTokenizer("abc, \"d\\\"'\",'p\\',y' z",
                                          " ,");
            CheckTok(tok, false, false);
        }

        [Test]
        public void TestNextToken2()
        {
            QuotedStringTokenizer tok =
                new QuotedStringTokenizer("abc, \"d\\\"'\",'p\\',y' z", " ,",
                false);
            CheckTok(tok, false, false);

            tok = new QuotedStringTokenizer("abc, \"d\\\"'\",'p\\',y' z", " ,",
                                            true);
            CheckTok(tok, true, false);
        }

        [Test]
        public void TestNextToken3()
        {
            QuotedStringTokenizer tok;

            tok = new QuotedStringTokenizer("abc, \"d\\\"'\",'p\\',y' z", " ,",
                                            false, false);
            CheckTok(tok, false, false);

            tok = new QuotedStringTokenizer("abc, \"d\\\"'\",'p\\',y' z", " ,",
                                            false, true);
            CheckTok(tok, false, true);

            tok = new QuotedStringTokenizer("abc, \"d\\\"'\",'p\\',y' z", " ,",
                                            true, false);
            CheckTok(tok, true, false);

            tok = new QuotedStringTokenizer("abc, \"d\\\"'\",'p\\',y' z", " ,",
                                            true, true);
            CheckTok(tok, true, true);
        }

        [Test]
        public void TestQuote()
        {
            StringBuilder buf = new StringBuilder();

            buf.Length = 0;
            QuotedStringTokenizer.Quote(buf, "abc \n efg");
            Assert.AreEqual("\"abc \\n efg\"", buf.ToString());

            buf.Length = 0;
            QuotedStringTokenizer.Quote(buf, "abcefg");
            Assert.AreEqual("\"abcefg\"", buf.ToString());

            buf.Length = 0;
            QuotedStringTokenizer.Quote(buf, "abcefg\"");
            Assert.AreEqual("\"abcefg\\\"\"", buf.ToString());

            buf.Length = 0;
            QuotedStringTokenizer.QuoteIfNeeded(buf, "abc \n efg");
            Assert.AreEqual("\"abc \\n efg\"", buf.ToString());

            buf.Length = 0;
            QuotedStringTokenizer.QuoteIfNeeded(buf, "abcefg");
            Assert.AreEqual("abcefg", buf.ToString());

        }

        [Test]
        public void TestTokenizer4()
        {
            QuotedStringTokenizer tok = new QuotedStringTokenizer("abc'def,ghi'jkl", ",");
            tok.SingleQuotes = false;
            Assert.AreEqual("abc'def", tok.NextToken());
            Assert.AreEqual("ghi'jkl", tok.NextToken());
            tok = new QuotedStringTokenizer("abc'def,ghi'jkl", ",");
            tok.SingleQuotes = true;
            Assert.AreEqual("abcdef,ghijkl", tok.NextToken());
        }

        [Test]
        public void TestQuoteString()
        {
            Assert.AreEqual("abc", QuotedStringTokenizer.Quote("abc", " ,"));
            Assert.AreEqual("\"a c\"", QuotedStringTokenizer.Quote("a c", " ,"));
            Assert.AreEqual("\"a'c\"", QuotedStringTokenizer.Quote("a'c", " ,"));
            Assert.AreEqual("\"a\\n\\r\\t\"", QuotedStringTokenizer.Quote("a\n\r\t"));
        }

        [Test]
        public void TestUnquote()
        {
            Assert.AreEqual("abc", QuotedStringTokenizer.Unquote("abc"));
            Assert.AreEqual("a\"c", QuotedStringTokenizer.Unquote("\"a\\\"c\""));
            Assert.AreEqual("a'c", QuotedStringTokenizer.Unquote("\"a'c\""));
            Assert.AreEqual("a\n\r\t", QuotedStringTokenizer.Unquote("\"a\\n\\r\\t\""));
        }
    }
}

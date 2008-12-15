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
using System.IO;
using NJetty.Util.Logging;

namespace NJetty.Util.Test.Util
{
    /// <summary>
    /// URLEncodedTest
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// December 2008
    /// </date>

    [TestFixture]
    public class URLEncodedTest
    {

        [Test]
        public void TestUrlEncoded()
        {

            UrlEncoded url_encoded = new UrlEncoded();
            Assert.AreEqual(0, url_encoded.Count, "Empty");

            url_encoded.Clear();
            url_encoded.Decode("Name1=Value1");
            Assert.AreEqual(1, url_encoded.Count, "simple param size");
            Assert.AreEqual("Name1=Value1", url_encoded.Encode(), "simple encode");
            Assert.AreEqual("Value1", url_encoded.GetString("Name1"), "simple get");

            url_encoded.Clear();
            url_encoded.Decode("Name2=");
            Assert.AreEqual(1, url_encoded.Count, "dangling param size");
            Assert.AreEqual("Name2", url_encoded.Encode(), "dangling encode");
            Assert.AreEqual("", url_encoded.GetString("Name2"), "dangling get");

            url_encoded.Clear();
            url_encoded.Decode("Name3");
            Assert.AreEqual(1, url_encoded.Count, "noValue param size");
            Assert.AreEqual("Name3", url_encoded.Encode(), "noValue encode");
            Assert.AreEqual("", url_encoded.GetString("Name3"), "noValue get");

            url_encoded.Clear();
            url_encoded.Decode("Name4=V\u0629lue+4%21");
            Assert.AreEqual(1, url_encoded.Count, "encoded param size");
            Assert.AreEqual("Name4=V%D8%A9lue+4%21", url_encoded.Encode(), "encoded encode");
            Assert.AreEqual("V\u0629lue 4!", url_encoded.GetString("Name4"), "encoded get");

            url_encoded.Clear();
            url_encoded.Decode("Name4=Value%2B4%21");
            Assert.AreEqual(1, url_encoded.Count, "encoded param size");
            Assert.AreEqual("Name4=Value%2B4%21", url_encoded.Encode(), "encoded encode");
            Assert.AreEqual("Value+4!", url_encoded.GetString("Name4"), "encoded get");

            url_encoded.Clear();
            url_encoded.Decode("Name4=Value+4%21%20%214");
            Assert.AreEqual(1, url_encoded.Count, "encoded param size");
            Assert.AreEqual("Name4=Value+4%21+%214", url_encoded.Encode(), "encoded encode");
            Assert.AreEqual("Value 4! !4", url_encoded.GetString("Name4"), "encoded get");


            url_encoded.Clear();
            url_encoded.Decode("Name5=aaa&Name6=bbb");
            Assert.AreEqual(2, url_encoded.Count, "multi param size");
            string str = url_encoded.Encode();
            Assert.IsTrue(
                       str.Equals("Name5=aaa&Name6=bbb") ||
                       str.Equals("Name6=bbb&Name5=aaa"),
                       "multi encode " + url_encoded.Encode()
                       );
            Assert.AreEqual("aaa", url_encoded.GetString("Name5"), "multi get");
            Assert.AreEqual("bbb", url_encoded.GetString("Name6"), "multi get");

            url_encoded.Clear();
            url_encoded.Decode("Name7=aaa&Name7=b%2Cb&Name7=ccc");
            Assert.AreEqual("Name7=aaa&Name7=b%2Cb&Name7=ccc", url_encoded.Encode(), "multi encode");
            Assert.AreEqual(url_encoded.GetString("Name7"), "aaa,b,b,ccc", "list get all");
            Assert.AreEqual("aaa", url_encoded.GetValues("Name7")[0], "list get");
            Assert.AreEqual(url_encoded.GetValues("Name7")[1], "b,b", "list get");
            Assert.AreEqual("ccc", url_encoded.GetValues("Name7")[2], "list get");

            url_encoded.Clear();
            url_encoded.Decode("Name8=xx%2C++yy++%2Czz");
            Assert.AreEqual(1, url_encoded.Count, "encoded param size");
            Assert.AreEqual("Name8=xx%2C++yy++%2Czz", url_encoded.Encode(), "encoded encode");
            Assert.AreEqual(url_encoded.GetString("Name8"), "xx,  yy  ,zz", "encoded get");

             
            url_encoded.Clear();
            url_encoded.Decode("Name11=xxVerdi+%C6+og+2zz", "ISO-8859-1");
            Assert.AreEqual(1, url_encoded.Count, "encoded param size");
            Assert.AreEqual(url_encoded.GetString("Name11"),"xxVerdi \u00c6 og 2zz", "encoded get");
            
            url_encoded.Clear();
            url_encoded.Decode("Name12=xxVerdi+%2F+og+2zz", "UTF-8");
            Assert.AreEqual(1, url_encoded.Count, "encoded param size");
            Assert.AreEqual(url_encoded.GetString("Name12"), "xxVerdi / og 2zz", "encoded get");

            try
            {
                Encoding.GetEncoding("SJIS");
                url_encoded.Clear();
                url_encoded.Decode("Name9=%83e%83X%83g", "SJIS"); // "Test" in Japanese Katakana
                Assert.AreEqual(1, url_encoded.Count, "encoded param size");
                Assert.AreEqual("\u30c6\u30b9\u30c8", url_encoded.GetString("Name9"), "encoded get");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true, "Charset SJIS not supported by CLR");
            }
        }

        [Test]
        public void TestUrlEncodedStream()
        {
            String [][] charsets = new String[][]
            {
               new string[] {StringUtil.__ISO_8859_1,null},
               new string[] {StringUtil.__ISO_8859_1,StringUtil.__ISO_8859_1},
               new string[] {StringUtil.__UTF8,StringUtil.__UTF8},
               new string[] {StringUtil.__UTF16,StringUtil.__UTF16},
            };
            
            for (int i=0;i<charsets.Length;i++)
            {
                MemoryStream input = new MemoryStream(Encoding.GetEncoding(charsets[i][0]).GetBytes("name\n=value+%30&name1=&name2&n\u00e3me3=value+3"));
                MultiMap<string> m = new MultiMap<string>();
                UrlEncoded.DecodeTo(input, m, charsets[i][1], -1);
                Log.Info(m.ToString());
                Assert.AreEqual(4, m.Count, i + " stream length");
                Assert.AreEqual("value 0", m.GetString("name\n"), i + " stream name\\n");
                Assert.AreEqual("", m.GetString("name1"), i + " stream name1");
                Assert.AreEqual("", m.GetString("name2"), i + " stream name2");
                Assert.AreEqual("value 3", m.GetString("n\u00e3me3"), i + " stream n\u00e3me3");
            }

            try
            {
                byte[] bytes = Encoding.GetEncoding("Shift_JIS").GetBytes("name=%83e%83X%83g");
                MemoryStream in2 = new MemoryStream();
                in2.Write(bytes, 0, bytes.Length);
                MultiMap<string> m2 = new MultiMap<string>();
                UrlEncoded.DecodeTo(in2, m2, "Shift_JIS", -1);
                Assert.AreEqual(1, m2.Count, "stream length");
                Assert.AreEqual("\u30c6\u30b9\u30c8", m2.GetString("name"), "stream name");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true, "Charset Shift_JIS not supported by CLR");
            }
        }



    }
}

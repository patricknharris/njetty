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
            Assert.IsTrue(
                       url_encoded.Encode().Equals("Name5=aaa&Name6=bbb") ||
                       url_encoded.Encode().Equals("Name6=bbb&Name5=aaa"),
                       "multi encode " + url_encoded.Encode()
                       );
            Assert.AreEqual("multi get", "aaa", url_encoded.GetString("Name5"));
            Assert.AreEqual("multi get", "bbb", url_encoded.GetString("Name6"));

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

            /* Not every jvm supports this encoding */

            //if (java.nio.charset.Charset.isSupported("SJIS"))
            //{
            //    url_encoded.clear();
            //    url_encoded.decode("Name9=%83e%83X%83g", "SJIS"); // "Test" in Japanese Katakana
            //    Assert.AreEqual("encoded param size", 1, url_encoded.Count);
            //    Assert.AreEqual("encoded get", "\u30c6\u30b9\u30c8", url_encoded.getString("Name9"));
            //}
            //else
            //    Assert.IsTrue("Charset SJIS not supported by jvm", true);
        }

        //[Test]
        //public void TestUrlEncodedStream()
        //{
        //    String [][] charsets = new String[][]
        //    {
        //       {StringUtil.__ISO_8859_1,null},
        //       {StringUtil.__ISO_8859_1,StringUtil.__ISO_8859_1},
        //       {StringUtil.__UTF8,StringUtil.__UTF8},
        //       {StringUtil.__UTF16,StringUtil.__UTF16},
        //    };
            
        //    for (int i=0;i<charsets.length;i++)
        //    {
        //        ByteArrayInputStream in = new ByteArrayInputStream("name\n=value+%30&name1=&name2&n\u00e3me3=value+3".getBytes(charsets[i][0]));
        //        MultiMap m = new MultiMap();
        //        UrlEncoded.decodeTo(in, m, charsets[i][1], -1);
        //        System.err.println(m);
        //        Assert.AreEqual(i+" stream length",4,m.Count);
        //        Assert.AreEqual(i+" stream name\\n","value 0",m.getString("name\n"));
        //        Assert.AreEqual(i+" stream name1","",m.getString("name1"));
        //        Assert.AreEqual(i+" stream name2","",m.getString("name2"));
        //        Assert.AreEqual(i+" stream n\u00e3me3","value 3",m.getString("n\u00e3me3"));
        //    }
            
            
        //    if (java.nio.charset.Charset.isSupported("Shift_JIS"))
        //    {
        //        ByteArrayInputStream in2 = new ByteArrayInputStream ("name=%83e%83X%83g".getBytes());
        //        MultiMap m2 = new MultiMap();
        //        UrlEncoded.decodeTo(in2, m2, "Shift_JIS", -1);
        //        Assert.AreEqual("stream length",1,m2.Count);
        //        Assert.AreEqual("stream name","\u30c6\u30b9\u30c8",m2.getString("name"));
        //    }
        //    else
        //        Assert.IsTrue("Charset Shift_JIS not supported by jvm", true);
        //}



    }
}

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
using System.Collections;

namespace NJetty.Util.Test.Util
{
    /// <summary>
    /// StringMapTest
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// December 2008
    /// </date>

    [TestFixture]
    public class StringMapTest
    {
        StringMap m0;
        StringMap m1;
        StringMap m5;
        StringMap m5i;

        [TestFixtureSetUp]
        protected void SetUp()
        {
            m0=new StringMap();
            m1=new StringMap(false);
            m1.Add("abc", "0");
            
            m5=new StringMap(false);
            m5.Add("a", "0");
            m5.Add("ab", "1");
            m5.Add("abc", "2");
            m5.Add("abb", "3");
            m5.Add("bbb", "4");
            
            m5i=new StringMap(true); 
            m5i.Add(null, "0");
            m5i.Add("ab", "1");
            m5i.Add("abc", "2");
            m5i.Add("abb", "3");
            m5i.Add("bbb", null);
        }

        [Test]
        public void TestSize()
        {
            Assert.AreEqual(0, m0.Count);
            Assert.AreEqual(1, m1.Count);
            Assert.AreEqual(5, m5.Count);
            Assert.AreEqual(5, m5i.Count);
            
            m1.Remove("abc");
            m5.Remove("abc");
            m5.Add("bbb","x");
            m5i.Add("ABC", "x");
            Assert.AreEqual(0, m0.Count);
            Assert.AreEqual(0, m1.Count);
            Assert.AreEqual(4, m5.Count);
            Assert.AreEqual(5, m5i.Count);
        }

        [Test]
        public void TestIsEmpty()
        {
            Assert.IsTrue(m0.IsEmpty);
            Assert.IsFalse(m1.IsEmpty);
            Assert.IsFalse(m5.IsEmpty);
            Assert.IsFalse(m5i.IsEmpty);
        }

        [Test]
        public void TestClear()
        {
            m0.Clear();
            m1.Clear();
            m5.Clear();
            m5i.Clear();
            Assert.IsTrue(m0.IsEmpty);
            Assert.IsTrue(m1.IsEmpty);
            Assert.IsTrue(m5.IsEmpty);
            Assert.IsTrue(m5i.IsEmpty);
            Assert.AreEqual(null,m1["abc"]);
            Assert.AreEqual(null,m5["abc"]);
            Assert.AreEqual(null,m5i["abc"]);
        }


        [Test]
        public void TestPutGet()
        {
            Assert.AreEqual("2",m5["abc"]);
            Assert.AreEqual(null,m5["aBc"]);
            Assert.AreEqual("2",m5i["abc"]);
            Assert.AreEqual("2",m5i["aBc"]);
            
            m5.Add(null,"x");
            m5.Add("aBc", "x");
            m5i.Add("AbC", "x");

            StringBuilder buffer=new StringBuilder();
            buffer.Append("aBc");
            Assert.AreEqual("2",m5["abc"]);
            Assert.AreEqual("x",m5[buffer]);
            Assert.AreEqual("x",m5i[(object)"abc"]);
            Assert.AreEqual("x",m5i["aBc"]);
            
            Assert.AreEqual("x",m5[null]);
            Assert.AreEqual("0",m5i[null]);
            
        }



        /// <summary>
        /// Test for Entry GetEntry(string, int, int)
        /// </summary>
        [Test]
        public void TestGetEntryStringintint()
        {
            StringMap.Entry entry;
            
            entry=m5.GetEntry("xabcyz",1,3);
            Assert.IsTrue(entry!=null);
            Assert.AreEqual("abc",entry.Key);
            Assert.AreEqual("2",entry.Value);
            
            entry=m5.GetBestEntry("xabcyz".GetBytes(),1,5);
            Assert.IsTrue(entry!=null);
            Assert.AreEqual("abc",entry.Key);
            Assert.AreEqual("2",entry.Value);
            
            entry=m5.GetEntry("xaBcyz",1,3);
            Assert.IsTrue(entry==null);
            
            entry=m5i.GetEntry("xaBcyz",1,3);
            Assert.IsTrue(entry!=null);
            Assert.AreEqual("abc",entry.Key);
            Assert.AreEqual("2",entry.Value);
            entry.Value = "x";
            Assert.AreEqual("{[c:abc=x]}",entry.ToString());
            
            entry=m5i.GetEntry((string)null,0,0);
            Assert.IsTrue(entry!=null);
            Assert.AreEqual(null,entry.Key);
            Assert.AreEqual("0",entry.Value);
            entry.Value = "x";
            Assert.AreEqual("[:null=x]",entry.ToString());

        }

        /// <summary>
        /// Test for Entry GetEntry(char[], int, int) 
        /// </summary>
        [Test]
        public void TestGetEntrycharArrayintint()
        {
            char[] xabcyz = {'x','a','b','c','y','z'};
            char[] xaBcyz = {'x','a','B','c','y','z'};
            StringMap.Entry entry;
            
            entry=m5.GetEntry(xabcyz,1,3);
            Assert.IsTrue(entry!=null);
            Assert.AreEqual("abc",entry.Key);
            Assert.AreEqual("2",entry.Value);
            
            entry=m5.GetEntry(xaBcyz,1,3);
            Assert.IsTrue(entry==null);
            
            entry=m5i.GetEntry(xaBcyz,1,3);
            Assert.IsTrue(entry!=null);
            Assert.AreEqual("abc",entry.Key);
            Assert.AreEqual("2",entry.Value);
        }

        /// <summary>
        /// Test for object Remove(object)
        /// </summary>
        [Test]
        public void TestRemove()
        {
            m0.Remove("abc");
            m1.Remove("abc");
            m5.Remove("aBc");
            m5.Remove("bbb");
            m5i.Remove("aBc");
            m5i.Remove(null);

            Assert.AreEqual(0, m0.Count);
            Assert.AreEqual(0, m1.Count);
            Assert.AreEqual(4, m5.Count);
            Assert.AreEqual(3, m5i.Count);

            Assert.AreEqual("2",m5["abc"]);
            Assert.AreEqual(null,m5["bbb"]);
            Assert.AreEqual(null,m5i["AbC"]);
            Assert.AreEqual(null,m5i[null]);
        }


        /// <summary>
        /// TODO: implementation
        /// Test for Set EntrySet()
        /// </summary>
        [Test]
        public void TestEntrySet()
        {
            ICollection es0 = m0.EntrySet;
            ICollection es1 = m1.EntrySet;
            ICollection es5 = m5.EntrySet;
            Assert.AreEqual(0, es0.Count);
            Assert.AreEqual(1, es1.Count);
            Assert.AreEqual(5, es5.Count);
        }

        /// <summary>
        /// Test for boolean ContainsKey(object)
        /// </summary>
        [Test]
        public void TestContainsKey()
        {
            Assert.IsTrue(m5.ContainsKey("abc"));
            Assert.IsTrue(!m5.ContainsKey("aBc"));
            Assert.IsTrue(m5.ContainsKey("bbb"));
            Assert.IsTrue(!m5.ContainsKey("xyz"));
            
            Assert.IsTrue(m5i.ContainsKey(null));
            Assert.IsTrue(m5i.ContainsKey("abc"));
            Assert.IsTrue(m5i.ContainsKey("aBc"));
            Assert.IsTrue(m5i.ContainsKey("ABC"));
        }

        public void TestWriteExternal()
        {
            //ByteArrayOutputStream bout= new ByteArrayOutputStream();
            //ObjectOutputStream oo=new ObjectOutputStream(bout);
            //ObjectInputStream oi;
            
            //oo.writeObject(m0);
            //oo.writeObject(m1);
            //oo.writeObject(m5);
            //oo.writeObject(m5i);
            
            //oi=new ObjectInputStream(new ByteArrayInputStream(bout.toByteArray()));
            //m0=(StringMap)oi.readObject();
            //m1=(StringMap)oi.readObject();
            //m5=(StringMap)oi.readObject();
            //m5i=(StringMap)oi.readObject();
            //testSize();
            
            //oi=new ObjectInputStream(new ByteArrayInputStream(bout.toByteArray()));
            //m0=(StringMap)oi.readObject();
            //m1=(StringMap)oi.readObject();
            //m5=(StringMap)oi.readObject();
            //m5i=(StringMap)oi.readObject();
            //testPutGet();
            
        }
        
        [Test]
        public void TestToString()
        {
            Assert.AreEqual("{}",m0.ToString());
            Assert.AreEqual("{abc=0}",m1.ToString());
            Assert.IsTrue(m5.ToString().IndexOf("abc=2")>0);
        }

    }
}

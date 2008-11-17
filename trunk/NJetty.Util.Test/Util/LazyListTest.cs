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
    /// Mock Listener
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// November 2008
    /// </date>

    [TestFixture]
    public class LazyListTest
    {



        [Test]
        public void TestAddobjectobject()
        {
            object list=null;
            Assert.AreEqual(0,LazyList.Size(list));
            
            list=LazyList.Add(list, "a");
            Assert.AreEqual(1,LazyList.Size(list));
            Assert.AreEqual("a",LazyList.Get(list,0));

            list = LazyList.Add(list, "b");
            Assert.AreEqual(2,LazyList.Size(list));
            Assert.AreEqual("a",LazyList.Get(list,0));
            Assert.AreEqual("b",LazyList.Get(list,1));

            list=null;
            list=LazyList.Add(list, null);
            Assert.AreEqual(1,LazyList.Size(list));
            Assert.AreEqual(null,LazyList.Get(list,0));
            
            list="a";
            list=LazyList.Add(list, null);
            Assert.AreEqual(2,LazyList.Size(list));
            Assert.AreEqual("a", LazyList.Get(list, 0));
            Assert.AreEqual(null, LazyList.Get(list, 1));

            list = LazyList.Add(list, null);
            Assert.AreEqual(3,LazyList.Size(list));
            Assert.AreEqual("a", LazyList.Get(list, 0));
            Assert.AreEqual(null, LazyList.Get(list, 1));
            Assert.AreEqual(null, LazyList.Get(list, 2));

            list = LazyList.Add(null, list);
            Assert.AreEqual(1,LazyList.Size(list));
            object l = LazyList.Get(list, 0);
            Assert.IsTrue(l is IList);
        }

        
        /// <summary>
        /// Test for object Add(object, int, object)
        /// </summary>
        [Test]
        public void TestAddobjectintobject()
        {
            object list=null;
            list=LazyList.Add(list,0,"c");
            list=LazyList.Add(list,0,"a");
            list=LazyList.Add(list,1,"b");
            list=LazyList.Add(list,3,"d");
            
            Assert.AreEqual(4,LazyList.Size(list));
            Assert.AreEqual("a",LazyList.Get(list,0));
            Assert.AreEqual("b",LazyList.Get(list,1));
            Assert.AreEqual("c",LazyList.Get(list,2));
            Assert.AreEqual("d",LazyList.Get(list,3));
            
            list=LazyList.Add(null, 0, null);
            Assert.IsTrue(list is IList);
            
            list=LazyList.Add(null, 0, new List<object>());
            Assert.IsTrue(list is IList);
        }

        [Test]
        public void TestAddCollection()
        {
            List<object> l = new List<object>();
            l.Add("a");
            l.Add("b");

            object list = null;
            list = LazyList.AddCollection(list, l);
            list = LazyList.AddCollection(list, l);

            Assert.AreEqual(4, LazyList.Size(list));
            Assert.AreEqual("a", LazyList.Get(list, 0));
            Assert.AreEqual("b", LazyList.Get(list, 1));
            Assert.AreEqual("a", LazyList.Get(list, 2));
            Assert.AreEqual("b", LazyList.Get(list, 3));
        }

        [Test]
        public void TestEnsureSize()
        {
            Assert.IsTrue(LazyList.EnsureSize(null,10)!=null);
            
            Assert.IsTrue(LazyList.EnsureSize("a",10) is List<object>);

            List<object> l=new List<object>();
            l.Add("a");
            l.Add("b");
            Assert.IsTrue(LazyList.EnsureSize(l,10)!=l);
            Assert.IsTrue(LazyList.EnsureSize(l,1)==l);   
        }

        /// <summary>
        /// Test for object Remove(object, object)
        /// </summary>
        [Test]
        public void TestRemoveobjectobject()
        {
            object list = null;

            Assert.IsTrue(LazyList.Remove(null, "a") == null);

            list = LazyList.Add(list, "a");
            Assert.AreEqual("a", LazyList.Remove(list, "z"));
            Assert.IsTrue(LazyList.Remove(list, "a") == null);

            list = LazyList.Add(list, "b");
            list = LazyList.Remove(list, "b");
            list = LazyList.Add(list, "b");
            list = LazyList.Add(list, "c");
            list = LazyList.Add(list, "d");
            list = LazyList.Add(list, "e");
            list = LazyList.Remove(list, "a");
            list = LazyList.Remove(list, "d");
            list = LazyList.Remove(list, "e");

            Assert.AreEqual(2, LazyList.Size(list));
            Assert.AreEqual("b", LazyList.Get(list, 0));
            Assert.AreEqual("c", LazyList.Get(list, 1));

            list = LazyList.Remove(list, "b");
            list = LazyList.Remove(list, "c");
            Assert.AreEqual(null, list);
        }

        /// <summary>
        /// Test for object Remove(object, int)
        /// </summary>
        [Test]
        public void TestRemoveobjectint()
        {
            object list = null;
            Assert.IsTrue(LazyList.Remove(list, 0) == null);

            list = LazyList.Add(list, "a");
            Assert.AreEqual("a", LazyList.Remove(list, 1));
            Assert.IsTrue(LazyList.Remove(list, 0) == null);

            list = LazyList.Add(list, "b");
            list = LazyList.Remove(list, 1);
            list = LazyList.Add(list, "b");
            list = LazyList.Add(list, "c");
            list = LazyList.Add(list, "d");
            list = LazyList.Add(list, "e");
            list = LazyList.Remove(list, 0);
            list = LazyList.Remove(list, 2);
            list = LazyList.Remove(list, 2);

            Assert.AreEqual(2, LazyList.Size(list));
            Assert.AreEqual("b", LazyList.Get(list, 0));
            Assert.AreEqual("c", LazyList.Get(list, 1));

            list = LazyList.Remove(list, 0);
            list = LazyList.Remove(list, 0);
            Assert.AreEqual(null, list);
        }

        /// <summary>
        /// Test for List GetList(object)
        /// </summary>
        [Test]
        public void TestGetListobject()
        {
            Assert.AreEqual(0, LazyList.GetList(null).Count);
            Assert.AreEqual(1, LazyList.GetList("a").Count);

            List<object> l = new List<object>();
            l.Add("a");
            l.Add("b");
            Assert.AreEqual(2, LazyList.GetList(l).Count);



            foreach (var item in LazyList.GetList("a"))
            {
                Assert.AreEqual("a", item);
            }

            foreach (var item in LazyList.GetList(null))
            {
                Assert.IsTrue(false, "this should not run!!!");
            }

        }

        /// <summary>
        /// Test for List GetList(object, bool)
        /// </summary>
        [Test]
        public void TestGetListobjectboolean()
        {
            Assert.AreEqual(0, LazyList.GetList(null, false).Count);
            Assert.AreEqual(null, LazyList.GetList(null, true));
        }

        [Test]
        public void TestToStringArray()
        {
            Assert.AreEqual(0, LazyList.ToStringArray(null).Length);

            Assert.AreEqual(1, LazyList.ToStringArray("a").Length);
            Assert.AreEqual("a", LazyList.ToStringArray("a")[0]);

            List<object> l = new List<object>();
            l.Add("a");
            l.Add(null);
            l.Add(2);
            string[] a = LazyList.ToStringArray(l);

            Assert.AreEqual(3, a.Length);
            Assert.AreEqual("a", a[0]);
            Assert.AreEqual(null, a[1]);
            Assert.AreEqual("2", a[2]);

        }

        [Test]
        public void TestSize()
        {
            List<object> l = new List<object>();
            l.Add("a");
            l.Add("b");

            Assert.AreEqual(0, LazyList.Size(null));
            Assert.AreEqual(0, LazyList.Size(new List<object>()));
            Assert.AreEqual(1, LazyList.Size("a"));
            Assert.AreEqual(2, LazyList.Size(l));
        }

        [Test]
        public void TestGet()
        {
            TestAddobjectobject();

            Assert.AreEqual("a", LazyList.Get("a", 0));

            try
            {
                LazyList.Get(null, 0);
                Assert.IsTrue(false);
            }
            catch (IndexOutOfRangeException e)
            {
                Assert.IsTrue(true);
            }

            try
            {
                LazyList.Get("a", 1);
                Assert.IsTrue(false);
            }
            catch (IndexOutOfRangeException e)
            {
                Assert.IsTrue(true);
            }
        }

        [Test]
        public void TestContains()
        {
            List<object> l = new List<object>();
            l.Add("a");
            l.Add("b");

            Assert.IsFalse(LazyList.Contains(null, "z"));
            Assert.IsFalse(LazyList.Contains("a", "z"));
            Assert.IsFalse(LazyList.Contains(l, "z"));

            Assert.IsTrue(LazyList.Contains("a", "a"));
            Assert.IsTrue(LazyList.Contains(l, "b"));

        }


        [Test]
        public void TestEnumerator()
        {
            List<object> l = new List<object>();
            l.Add("a");
            l.Add("b");

            Assert.IsFalse(LazyList.GetEnumerator(null).MoveNext());

            IEnumerator i = LazyList.GetEnumerator("a");
            Assert.IsTrue(i.MoveNext());
            Assert.AreEqual("a", i.Current);
            Assert.IsFalse(i.MoveNext());

            i = LazyList.GetEnumerator(l);
            Assert.IsTrue(i.MoveNext());
            Assert.AreEqual("a", i.Current);
            Assert.IsTrue(i.MoveNext());
            Assert.AreEqual("b", i.Current);
            Assert.IsFalse(i.MoveNext());


        }



        // TODO: may be this one is no longer applicable
        //[Test]
        //public void TestListIterator()
        //{
        //    List<object> l = new List<object>();
        //    l.Add("a");
        //    l.Add("b");

        //    Assert.IsFalse(LazyList.listIterator(null).hasNext());

        //    ListIterator i = LazyList.listIterator("a");
        //    Assert.IsTrue(i.hasNext());
        //    Assert.IsFalse(i.hasPrevious());
        //    Assert.AreEqual("a", i.next());
        //    Assert.IsFalse(i.hasNext());
        //    Assert.IsTrue(i.hasPrevious());
        //    Assert.AreEqual("a", i.previous());

        //    i = LazyList.listIterator(l);
        //    Assert.IsTrue(i.hasNext());
        //    Assert.IsFalse(i.hasPrevious());
        //    Assert.AreEqual("a", i.next());
        //    Assert.IsTrue(i.hasNext());
        //    Assert.IsTrue(i.hasPrevious());
        //    Assert.AreEqual("b", i.next());
        //    Assert.IsFalse(i.hasNext());
        //    Assert.IsTrue(i.hasPrevious());
        //    Assert.AreEqual("b", i.previous());
        //    Assert.AreEqual("a", i.previous());
        //}

        [Test]
        public void TestCloneToString()
        {
            List<object> l = new List<object>();
            l.Add("a");
            l.Add("b");

            Assert.AreEqual("[]", LazyList.ToString(LazyList.Clone(null)));
            Assert.AreEqual("[a]", LazyList.ToString(LazyList.Clone("a")));
            Assert.AreEqual("[a, b]", LazyList.ToString(LazyList.Clone(l)));
        }

    }
}

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
using System.Reflection;
using NJetty.Util.Util;

namespace NJetty.Util.Test.Util
{

    /// <summary>
    /// TestIntrospectionUtil
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// December 2008
    /// </date>

    [TestFixture]
    public class TestIntrospectionUtil
    {
        const BindingFlags _BINDINGFLAGS = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static;


        public readonly Type[] __INTEGER_ARG = new Type[] { typeof(int) };
        FieldInfo privateAField;
        FieldInfo protectedAField;
        FieldInfo publicAField;
        FieldInfo defaultAField;
        FieldInfo privateBField;
        FieldInfo protectedBField;
        FieldInfo publicBField;
        FieldInfo defaultBField;
        MethodInfo privateCMethod;
        MethodInfo protectedCMethod;
        MethodInfo publicCMethod;
        MethodInfo defaultCMethod;
        MethodInfo privateDMethod;
        MethodInfo protectedDMethod;
        MethodInfo publicDMethod;
        MethodInfo defaultDMethod;


        public class ClassFixtureA
        {
            private int privateA;
            protected int protectedA;
            internal int internalA;
            public int publicA;
            private static int bong;
        }

        public class ClassFixtureB : ClassFixtureA
        {
            private string privateB;
            protected string protectedB;
            public string publicB;
            internal string internalB;
        }

        public class ClassFixtureC
        {
            private void setPrivateC(int c) { }
            protected void setProtectedC(int c) { }
            public void setPublicC(int c) { }
            internal void setInternalC(int c) { }
        }

        public class ClassFixtureD : ClassFixtureC
        {
            private void setPrivateD(int d) { }
            protected void setProtectedD(int d) { }
            public void setPublicD(int d) { }
            internal void setInternalD(int d) { }
        }

        
        [TestFixtureSetUp]
        public void SetUp()
        {
            privateAField = typeof(ClassFixtureA).GetField("privateA", _BINDINGFLAGS);
            protectedAField = typeof(ClassFixtureA).GetField("protectedA", _BINDINGFLAGS);
            publicAField = typeof(ClassFixtureA).GetField("publicA", _BINDINGFLAGS);
            defaultAField = typeof(ClassFixtureA).GetField("internalA", _BINDINGFLAGS);
            privateBField = typeof(ClassFixtureB).GetField("privateB", _BINDINGFLAGS);
            protectedBField = typeof(ClassFixtureB).GetField("protectedB", _BINDINGFLAGS);
            publicBField = typeof(ClassFixtureB).GetField("publicB", _BINDINGFLAGS);
            defaultBField = typeof(ClassFixtureB).GetField("internalB", _BINDINGFLAGS);
            privateCMethod = typeof(ClassFixtureC).GetMethod("setPrivateC", _BINDINGFLAGS, null, __INTEGER_ARG, null);
            protectedCMethod = typeof(ClassFixtureC).GetMethod("setProtectedC", _BINDINGFLAGS, null, __INTEGER_ARG, null);
            publicCMethod = typeof(ClassFixtureC).GetMethod("setPublicC", _BINDINGFLAGS, null, __INTEGER_ARG, null);
            defaultCMethod = typeof(ClassFixtureC).GetMethod("setInternalC", _BINDINGFLAGS, null, __INTEGER_ARG, null);
            privateDMethod = typeof(ClassFixtureD).GetMethod("setPrivateD", _BINDINGFLAGS, null, __INTEGER_ARG, null);
            protectedDMethod = typeof(ClassFixtureD).GetMethod("setProtectedD", _BINDINGFLAGS, null, __INTEGER_ARG, null);
            publicDMethod = typeof(ClassFixtureD).GetMethod("setPublicD", _BINDINGFLAGS, null, __INTEGER_ARG, null);
            defaultDMethod = typeof(ClassFixtureD).GetMethod("setInternalD", _BINDINGFLAGS, null, __INTEGER_ARG, null);
        }


        [Test]
        public void TestFieldPrivate ()
        {
            //direct
            FieldInfo f = IntrospectionUtil.FindField(typeof(ClassFixtureA), "privateA", typeof(int), true, false);
            Assert.AreEqual(privateAField, f);

            //inheritance
            Assert.IsNull(IntrospectionUtil.FindField(typeof(ClassFixtureB), "privateB", typeof(int), true, false), "Private fields should not be inherited");
            
        }

        [Test]
        public void TestFieldProtected()    
        {
            //direct
            FieldInfo f = IntrospectionUtil.FindField(typeof(ClassFixtureA), "protectedA", typeof(int), true, false);
            Assert.AreEqual(f, protectedAField);

            //inheritance
            f = IntrospectionUtil.FindField(typeof(ClassFixtureB), "protectedA", typeof(int), true, false);
            Assert.AreEqual(f, protectedAField);
        }

        [Test]
        public void TestFieldPublic()
        {
            //direct
            FieldInfo f = IntrospectionUtil.FindField(typeof(ClassFixtureA), "publicA", typeof(int), true, false);
            Assert.AreEqual(f, publicAField);

            //inheritance
            f = IntrospectionUtil.FindField(typeof(ClassFixtureB), "publicA", typeof(int), true, false);
            Assert.AreEqual(f, publicAField);
        }

        [Test]
        public void TestFieldDefault()
        {
            //direct
            FieldInfo f = IntrospectionUtil.FindField(typeof(ClassFixtureA), "internalA", typeof(int), true, false);
            Assert.AreEqual(f, defaultAField);

            //inheritance
            f = IntrospectionUtil.FindField(typeof(ClassFixtureB), "internalA", typeof(int), true, false);
            Assert.AreEqual(f, defaultAField);
        }

        [Test]
        public void TestMethodPrivate ()
        {
            //direct
            MethodInfo m = IntrospectionUtil.FindMethod(typeof(ClassFixtureC), "setPrivateC", __INTEGER_ARG, true, false);
            Assert.AreEqual(m, privateCMethod);

            Assert.IsNull(IntrospectionUtil.FindMethod(typeof(ClassFixtureD), "setPrivateC", __INTEGER_ARG, true, false), "Should Be Null");
      
        }

        [Test]
        public void TestMethodProtected ()
        {
            // direct
            MethodInfo m = IntrospectionUtil.FindMethod(typeof(ClassFixtureC), "setProtectedC", __INTEGER_ARG, true, false);
            Assert.AreEqual(m, protectedCMethod);

            //inherited
            m = IntrospectionUtil.FindMethod(typeof(ClassFixtureD), "setProtectedC", __INTEGER_ARG, true, false);
            Assert.AreEqual(m, protectedCMethod);
        }

        [Test]
        public void TestMethodPublic()
        {
            // direct
            MethodInfo m = IntrospectionUtil.FindMethod(typeof(ClassFixtureC), "setPublicC", __INTEGER_ARG, true, false);
            Assert.AreEqual(m, publicCMethod);

            //inherited
            m = IntrospectionUtil.FindMethod(typeof(ClassFixtureD), "setPublicC", __INTEGER_ARG, true, false);
            Assert.AreEqual(m, publicCMethod);
        }

        [Test]
        public void TestMethodDefault()
        {
            // direct
            MethodInfo m = IntrospectionUtil.FindMethod(typeof(ClassFixtureC), "setInternalC", __INTEGER_ARG, true, false);
            Assert.AreEqual(m, defaultCMethod);

            //inherited
            m = IntrospectionUtil.FindMethod(typeof(ClassFixtureD), "setInternalC", __INTEGER_ARG, true, false);
            Assert.AreEqual(m, defaultCMethod);
        }



    }
}

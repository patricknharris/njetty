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
    /// URITest
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// November 2008
    /// </date>

    [TestFixture]
    public class URITest
    {


        [Test]
        public void TestEncodePath()
        {
            // test basic encode/decode
            StringBuilder buf = new StringBuilder();


            buf.Length = 0;
            URIUtil.EncodePath(buf, "/foo%23+;,:=/b a r/?info ");
            Assert.AreEqual("/foo%2523+%3B,:=/b%20a%20r/%3Finfo%20", buf.ToString());

            Assert.AreEqual("/foo%2523+%3B,:=/b%20a%20r/%3Finfo%20", URIUtil.EncodePath("/foo%23+;,:=/b a r/?info "));

            buf.Length = 0;
            URIUtil.EncodeString(buf, "foo%23;,:=b a r", ";,= ");
            Assert.AreEqual("foo%2523%3b%2c:%3db%20a%20r", buf.ToString());

        }

        [Test]
        public void TestDecodePath()
        {
            Assert.AreEqual("foo%23;,:=b a r", URIUtil.DecodePath("foo%2523%3b%2c:%3db%20a%20r"));
            Assert.AreEqual("foo%23;,:=b a r=", URIUtil.DecodePath("xxxfoo%2523%3b%2c:%3db%20a%20r%3Dxxx".GetBytes(), 3, 30));
            Assert.AreEqual("fää%23;,:=b a r=", URIUtil.DecodePath("fää%2523%3b%2c:%3db%20a%20r%3D"));
            Assert.AreEqual("f\u0629\u0629%23;,:=b a r", URIUtil.DecodePath("f%d8%a9%d8%a9%2523%3b%2c:%3db%20a%20r"));
        }

        [Test]
        public void TestAddPaths()
        {
            Assert.AreEqual(URIUtil.AddPaths(null, null), null, "null+null");
            Assert.AreEqual(URIUtil.AddPaths(null, ""), "", "null+");
            Assert.AreEqual(URIUtil.AddPaths(null, "bbb"), "bbb", "null+bbb");
            Assert.AreEqual(URIUtil.AddPaths(null, "/"), "/", "null+/");
            Assert.AreEqual(URIUtil.AddPaths(null, "/bbb"), "/bbb", "null+/bbb");

            Assert.AreEqual(URIUtil.AddPaths("", null), "", "+null");
            Assert.AreEqual(URIUtil.AddPaths("", ""), "", "+");
            Assert.AreEqual(URIUtil.AddPaths("", "bbb"), "bbb", "+bbb");
            Assert.AreEqual(URIUtil.AddPaths("", "/"), "/", "+/");
            Assert.AreEqual(URIUtil.AddPaths("", "/bbb"), "/bbb", "+/bbb");

            Assert.AreEqual(URIUtil.AddPaths("aaa", null), "aaa", "aaa+null");
            Assert.AreEqual(URIUtil.AddPaths("aaa", ""), "aaa", "aaa+");
            Assert.AreEqual(URIUtil.AddPaths("aaa", "bbb"), "aaa/bbb", "aaa+bbb");
            Assert.AreEqual(URIUtil.AddPaths("aaa", "/"), "aaa/", "aaa+/");
            Assert.AreEqual(URIUtil.AddPaths("aaa", "/bbb"), "aaa/bbb", "aaa+/bbb");

            Assert.AreEqual(URIUtil.AddPaths("/", null), "/", "/+null");
            Assert.AreEqual(URIUtil.AddPaths("/", ""), "/", "/+");
            Assert.AreEqual(URIUtil.AddPaths("/", "bbb"), "/bbb", "/+bbb");
            Assert.AreEqual(URIUtil.AddPaths("/", "/"), "/", "/+/");
            Assert.AreEqual(URIUtil.AddPaths("/", "/bbb"), "/bbb", "/+/bbb");

            Assert.AreEqual(URIUtil.AddPaths("aaa/", null), "aaa/", "aaa/+null");
            Assert.AreEqual(URIUtil.AddPaths("aaa/", ""), "aaa/", "aaa/+");
            Assert.AreEqual(URIUtil.AddPaths("aaa/", "bbb"), "aaa/bbb", "aaa/+bbb");
            Assert.AreEqual(URIUtil.AddPaths("aaa/", "/"), "aaa/", "aaa/+/");
            Assert.AreEqual(URIUtil.AddPaths("aaa/", "/bbb"), "aaa/bbb", "aaa/+/bbb");

            Assert.AreEqual(URIUtil.AddPaths(";JS", null), ";JS", ";JS+null");
            Assert.AreEqual(URIUtil.AddPaths(";JS", ""), ";JS", ";JS+");
            Assert.AreEqual(URIUtil.AddPaths(";JS", "bbb"), "bbb;JS", ";JS+bbb");
            Assert.AreEqual(URIUtil.AddPaths(";JS", "/"), "/;JS", ";JS+/");
            Assert.AreEqual(URIUtil.AddPaths(";JS", "/bbb"), "/bbb;JS", ";JS+/bbb");

            Assert.AreEqual(URIUtil.AddPaths("aaa;JS", null), "aaa;JS", "aaa;JS+null");
            Assert.AreEqual(URIUtil.AddPaths("aaa;JS", ""), "aaa;JS", "aaa;JS+");
            Assert.AreEqual(URIUtil.AddPaths("aaa;JS", "bbb"), "aaa/bbb;JS", "aaa;JS+bbb");
            Assert.AreEqual(URIUtil.AddPaths("aaa;JS", "/"), "aaa/;JS", "aaa;JS+/");
            Assert.AreEqual(URIUtil.AddPaths("aaa;JS", "/bbb"), "aaa/bbb;JS", "aaa;JS+/bbb");

            Assert.AreEqual(URIUtil.AddPaths("aaa/;JS", null), "aaa/;JS", "aaa;JS+null");
            Assert.AreEqual(URIUtil.AddPaths("aaa/;JS", ""), "aaa/;JS", "aaa;JS+");
            Assert.AreEqual(URIUtil.AddPaths("aaa/;JS", "bbb"), "aaa/bbb;JS", "aaa;JS+bbb");
            Assert.AreEqual(URIUtil.AddPaths("aaa/;JS", "/"), "aaa/;JS", "aaa;JS+/");
            Assert.AreEqual(URIUtil.AddPaths("aaa/;JS", "/bbb"), "aaa/bbb;JS", "aaa;JS+/bbb");

            Assert.AreEqual(URIUtil.AddPaths("?A=1", null), "?A=1", "?A=1+null");
            Assert.AreEqual(URIUtil.AddPaths("?A=1", ""), "?A=1", "?A=1+");
            Assert.AreEqual(URIUtil.AddPaths("?A=1", "bbb"), "bbb?A=1", "?A=1+bbb");
            Assert.AreEqual(URIUtil.AddPaths("?A=1", "/"), "/?A=1", "?A=1+/");
            Assert.AreEqual(URIUtil.AddPaths("?A=1", "/bbb"), "/bbb?A=1", "?A=1+/bbb");

            Assert.AreEqual(URIUtil.AddPaths("aaa?A=1", null), "aaa?A=1", "aaa?A=1+null");
            Assert.AreEqual(URIUtil.AddPaths("aaa?A=1", ""), "aaa?A=1", "aaa?A=1+");
            Assert.AreEqual(URIUtil.AddPaths("aaa?A=1", "bbb"), "aaa/bbb?A=1", "aaa?A=1+bbb");
            Assert.AreEqual(URIUtil.AddPaths("aaa?A=1", "/"), "aaa/?A=1", "aaa?A=1+/");
            Assert.AreEqual(URIUtil.AddPaths("aaa?A=1", "/bbb"), "aaa/bbb?A=1", "aaa?A=1+/bbb");

            Assert.AreEqual(URIUtil.AddPaths("aaa/?A=1", null), "aaa/?A=1", "aaa?A=1+null");
            Assert.AreEqual(URIUtil.AddPaths("aaa/?A=1", ""), "aaa/?A=1", "aaa?A=1+");
            Assert.AreEqual(URIUtil.AddPaths("aaa/?A=1", "bbb"), "aaa/bbb?A=1", "aaa?A=1+bbb");
            Assert.AreEqual(URIUtil.AddPaths("aaa/?A=1", "/"), "aaa/?A=1", "aaa?A=1+/");
            Assert.AreEqual(URIUtil.AddPaths("aaa/?A=1", "/bbb"), "aaa/bbb?A=1", "aaa?A=1+/bbb");

            Assert.AreEqual(URIUtil.AddPaths(";JS?A=1", null), ";JS?A=1", ";JS?A=1+null");
            Assert.AreEqual(URIUtil.AddPaths(";JS?A=1", ""), ";JS?A=1", ";JS?A=1+");
            Assert.AreEqual(URIUtil.AddPaths(";JS?A=1", "bbb"), "bbb;JS?A=1", ";JS?A=1+bbb");
            Assert.AreEqual(URIUtil.AddPaths(";JS?A=1", "/"), "/;JS?A=1", ";JS?A=1+/");
            Assert.AreEqual(URIUtil.AddPaths(";JS?A=1", "/bbb"), "/bbb;JS?A=1", ";JS?A=1+/bbb");

            Assert.AreEqual(URIUtil.AddPaths("aaa;JS?A=1", null), "aaa;JS?A=1", "aaa;JS?A=1+null");
            Assert.AreEqual(URIUtil.AddPaths("aaa;JS?A=1", ""), "aaa;JS?A=1", "aaa;JS?A=1+");
            Assert.AreEqual(URIUtil.AddPaths("aaa;JS?A=1", "bbb"), "aaa/bbb;JS?A=1", "aaa;JS?A=1+bbb");
            Assert.AreEqual(URIUtil.AddPaths("aaa;JS?A=1", "/"), "aaa/;JS?A=1", "aaa;JS?A=1+/");
            Assert.AreEqual(URIUtil.AddPaths("aaa;JS?A=1", "/bbb"), "aaa/bbb;JS?A=1", "aaa;JS?A=1+/bbb");

            Assert.AreEqual(URIUtil.AddPaths("aaa/;JS?A=1", null), "aaa/;JS?A=1", "aaa;JS?A=1+null");
            Assert.AreEqual(URIUtil.AddPaths("aaa/;JS?A=1", ""), "aaa/;JS?A=1", "aaa;JS?A=1+");
            Assert.AreEqual(URIUtil.AddPaths("aaa/;JS?A=1", "bbb"), "aaa/bbb;JS?A=1", "aaa;JS?A=1+bbb");
            Assert.AreEqual(URIUtil.AddPaths("aaa/;JS?A=1", "/"), "aaa/;JS?A=1", "aaa;JS?A=1+/");
            Assert.AreEqual(URIUtil.AddPaths("aaa/;JS?A=1", "/bbb"), "aaa/bbb;JS?A=1", "aaa;JS?A=1+/bbb");

        }

        [Test]
        public void TestCompactPath()
        {
            Assert.AreEqual("/foo/bar", URIUtil.CompactPath("/foo/bar"));
            Assert.AreEqual("/foo/bar?a=b//c", URIUtil.CompactPath("/foo/bar?a=b//c"));

            Assert.AreEqual("/foo/bar", URIUtil.CompactPath("//foo//bar"));
            Assert.AreEqual("/foo/bar?a=b//c", URIUtil.CompactPath("//foo//bar?a=b//c"));

            Assert.AreEqual("/foo/bar", URIUtil.CompactPath("/foo///bar"));
            Assert.AreEqual("/foo/bar?a=b//c", URIUtil.CompactPath("/foo///bar?a=b//c"));
        }

        [Test]
        public void TestParentPath()
        {
            Assert.AreEqual("/aaa/", URIUtil.ParentPath("/aaa/bbb/"), "parent /aaa/bbb/");
            Assert.AreEqual("/aaa/", URIUtil.ParentPath("/aaa/bbb"), "parent /aaa/bbb");
            Assert.AreEqual("/", URIUtil.ParentPath("/aaa/"), "parent /aaa/");
            Assert.AreEqual("/", URIUtil.ParentPath("/aaa"), "parent /aaa");
            Assert.AreEqual(null, URIUtil.ParentPath("/"), "parent /");
            Assert.AreEqual(null, URIUtil.ParentPath(null), "parent null");

        }

        [Test]
        public void TestCanonicalPath()
        {
            String[][] canonical = 
            {
                new string[]{"/aaa/bbb/","/aaa/bbb/"},
                new string[]{"/aaa//bbb/","/aaa//bbb/"},
                new string[]{"/aaa///bbb/","/aaa///bbb/"},
                new string[]{"/aaa/./bbb/","/aaa/bbb/"},
                new string[]{"/aaa/../bbb/","/bbb/"},
                new string[]{"/aaa/./../bbb/","/bbb/"},
                new string[]{"/aaa/bbb/ccc/../../ddd/","/aaa/ddd/"},
                new string[]{"./bbb/","bbb/"},
                new string[]{"./aaa/../bbb/","bbb/"},
                new string[]{"./",""},
                new string[]{".//",".//"},
                new string[]{".///",".///"},
                new string[]{"/.","/"},
                new string[]{"//.","//"},
                new string[]{"///.","///"},
                new string[]{"/","/"},
                new string[]{"aaa/bbb","aaa/bbb"},
                new string[]{"aaa/","aaa/"},
                new string[]{"aaa","aaa"},
                new string[]{"/aaa/bbb","/aaa/bbb"},
                new string[]{"/aaa//bbb","/aaa//bbb"},
                new string[]{"/aaa/./bbb","/aaa/bbb"},
                new string[]{"/aaa/../bbb","/bbb"},
                new string[]{"/aaa/./../bbb","/bbb"},
                new string[]{"./bbb","bbb"},
                new string[]{"./aaa/../bbb","bbb"},
                new string[]{"aaa/bbb/..","aaa/"},
                new string[]{"aaa/bbb/../","aaa/"},
                new string[]{"/aaa//../bbb","/aaa/bbb"},
                new string[]{"/aaa/./../bbb","/bbb"},
                new string[]{"./",""},
                new string[]{".",""},
                new string[]{"",""},
                new string[]{"..",null},
                new string[]{"./..",null},
                new string[]{"aaa/../..",null},
                new string[]{"/foo/bar/../../..",null},
                new string[]{"/../foo",null},
                new string[]{"/foo/.","/foo/"},
                new string[]{"a","a"},
                new string[]{"a/","a/"},
                new string[]{"a/.","a/"},
                new string[]{"a/..",""},
                new string[]{"a/../..",null},
                new string[]{"/foo/../bar//","/bar//"},
            };

            for (int t = 0; t < canonical.Length; t++)
            {
                Assert.AreEqual(canonical[t][1],
                              URIUtil.CanonicalPath(canonical[t][0]),
                              "canonical " + canonical[t][0]

                              );
            }

        }


    }
}

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

namespace NJetty.Util.Util
{

    /// <summary>
    /// string class extensions, used to add some missing methods found in java
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// November 2008
    /// </date>

    public static class StringExtensions
    {
        public static byte[] GetBytes(this string str)
        {
            return Encoding.ASCII.GetBytes(str);
        }

        public static byte[] GetBytes(this string str, string encoding)
        {
            return Encoding.GetEncoding(encoding).GetBytes(str);
        }

        public static int CodePointAt(this string str, int index)
        {
            if ((index < 0) || (index >= str.Length))
            {
                throw new IndexOutOfRangeException(str.ToString());
            }

            return CharExtensions.CodePointAtImpl(str.ToCharArray(), index, str.Length);
        }
    }
}

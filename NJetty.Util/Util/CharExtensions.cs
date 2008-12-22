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
    /// char class extensions, used to add some missing methods found in java
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// November 2008
    /// </date>
    
    public static class CharExtensions
    {
        public const char MIN_HIGH_SURROGATE = '\uD800';
        public const char MAX_HIGH_SURROGATE = '\uDBFF';
        public const char MIN_LOW_SURROGATE = '\uDC00';
        public const char MAX_LOW_SURROGATE = '\uDFFF';
        public const int MIN_SUPPLEMENTARY_CODE_POINT = 0x010000;

        


        /// <summary>
        ///  Determines the number of bytes values needed to
        /// represent the specified character (Unicode code point). If the
        /// specified character is equal to or greater than 0x10000, then
        /// the method returns 2. Otherwise, the method returns 1.
        /// </summary>
        /// <param name="codePoint"></param>
        /// <returns></returns>
        public static int CharCount(int codePoint)
        {
            return codePoint >= 0x010000 ? 2 : 1;
        }



        public static int CodePointAtImpl(char[] a, int index, int limit)
        {
            char c1 = a[index++];
            if (Char.IsHighSurrogate(c1))
            {
                if (index < limit)
                {
                    char c2 = a[index];
                    if (Char.IsLowSurrogate(c2))
                    {
                        return ToCodePoint(c1, c2);
                    }
                }
            }
            return c1;
        }
        
        public static int ToCodePoint(char high, char low)
        {
            return ((high - MIN_HIGH_SURROGATE) << 10)
                + (low - MIN_LOW_SURROGATE) + MIN_SUPPLEMENTARY_CODE_POINT;
        }

    }
}

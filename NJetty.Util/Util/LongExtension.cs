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
    /// long class extensions, used to add some missing methods found in java
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// December 2008
    /// </date>

    public static class LongExtension
    {



        static readonly char[] int_DigitTens = new char[]
        {
	    '0', '0', '0', '0', '0', '0', '0', '0', '0', '0',
	    '1', '1', '1', '1', '1', '1', '1', '1', '1', '1',
	    '2', '2', '2', '2', '2', '2', '2', '2', '2', '2',
	    '3', '3', '3', '3', '3', '3', '3', '3', '3', '3',
	    '4', '4', '4', '4', '4', '4', '4', '4', '4', '4',
	    '5', '5', '5', '5', '5', '5', '5', '5', '5', '5',
	    '6', '6', '6', '6', '6', '6', '6', '6', '6', '6',
	    '7', '7', '7', '7', '7', '7', '7', '7', '7', '7',
	    '8', '8', '8', '8', '8', '8', '8', '8', '8', '8',
	    '9', '9', '9', '9', '9', '9', '9', '9', '9', '9',
	    };

        static readonly char[] int_digits = new char[]
        {
	    '0' , '1' , '2' , '3' , '4' , '5' ,
	    '6' , '7' , '8' , '9' , 'a' , 'b' ,
	    'c' , 'd' , 'e' , 'f' , 'g' , 'h' ,
	    'i' , 'j' , 'k' , 'l' , 'm' , 'n' ,
	    'o' , 'p' , 'q' , 'r' , 's' , 't' ,
	    'u' , 'v' , 'w' , 'x' , 'y' , 'z'
        };


        static readonly char[] int_DigitOnes = new char[]
        { 
	    '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
	    '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
	    '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
	    '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
	    '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
	    '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
	    '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
	    '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
	    '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
	    '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
	    };

        public static string ToString(this long i, int radix)
        {
            if (radix < 2 || radix > 36)
                radix = 10;
            if (radix == 10)
                return ToString(i);
            char[] buf = new char[65];
            int charPos = 64;
            bool negative = (i < 0);

            if (!negative)
            {
                i = -i;
            }

            while (i <= -radix)
            {
                buf[charPos--] = int_digits[(int)(-(i % radix))];
                i = i / radix;
            }
            buf[charPos] = int_digits[(int)(-i)];

            if (negative)
            {
                buf[--charPos] = '-';
            }

            return new String(buf, charPos, (65 - charPos));
        }


        public static string ToString(this long i)
        {
            if (i == long.MinValue)
                return "-9223372036854775808";
            int size = (i < 0) ? stringSize(-i) + 1 : stringSize(i);
            char[] buf = new char[size];
            getChars(i, size, buf);
            return new string(buf, 0, size);
        }


        static void getChars(long i, int index, char[] buf)
        {
            long q;
            int r;
            int charPos = index;
            char sign = (char)0;

            if (i < 0)
            {
                sign = '-';
                i = -i;
            }

            // Get 2 digits/iteration using longs until quotient fits into an int
            while (i > int.MaxValue)
            {
                q = i / 100;
                // really: r = i - (q * 100);
                r = (int)(i - ((q << 6) + (q << 5) + (q << 2)));
                i = q;
                buf[--charPos] = int_DigitOnes[r];
                buf[--charPos] = int_DigitTens[r];
            }

            // Get 2 digits/iteration using ints
            int q2;
            int i2 = (int)i;
            while (i2 >= 65536)
            {
                q2 = i2 / 100;
                // really: r = i2 - (q * 100);
                r = i2 - ((q2 << 6) + (q2 << 5) + (q2 << 2));
                i2 = q2;
                buf[--charPos] = int_DigitOnes[r];
                buf[--charPos] = int_DigitTens[r];
            }

            // Fall thru to fast mode for smaller numbers
            // assert(i2 <= 65536, i2);
            for (; ; )
            {
                q2 = (int)(uint)(i2 * 52429) >> (16 + 3);
                r = i2 - ((q2 << 3) + (q2 << 1));  // r = i2-(q2*10) ...
                buf[--charPos] = int_digits[r];
                i2 = q2;
                if (i2 == 0) break;
            }
            if (sign != 0)
            {
                buf[--charPos] = sign;
            }
        }


        // Requires positive x
        static int stringSize(long x)
        {
            long p = 10;
            for (int i = 1; i < 19; i++)
            {
                if (x < p)
                    return i;
                p = 10 * p;
            }
            return 19;
        }
    }
}

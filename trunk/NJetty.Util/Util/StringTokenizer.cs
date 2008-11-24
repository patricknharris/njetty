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
    /// StringTokenizer class allows an application to break a string into tokens
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// November 2008
    /// </date>
    
    public class StringTokenizer
    {
        int currentPosition;
        int newPosition;
        int maxPosition;
        string str;
        string delimiters;
        bool retDelims;
        bool delimsChanged;

        int maxDelimCodePoint;

        bool hasSurrogates = false;

        int[] delimiterCodePoints;


        #region Constructors

        public StringTokenizer(string str, string delim, bool returnDelims)
        {
            currentPosition = 0;
            newPosition = -1;
            delimsChanged = false;
            this.str = str;
            maxPosition = str.Length;
            delimiters = delim;
            retDelims = returnDelims;
            SetMaxDelimCodePoint();
        }

        
        public StringTokenizer(string str, string delim)
            : this(str, delim, false)
        {
            
        }

        public StringTokenizer(string str)
            : this(str, " \t\n\r\f", false)
        {

        }

        #endregion



        /// <summary>
        /// Set MaxDelimCodePoint to the highest char in the delimiter set.
        /// </summary>
        private void SetMaxDelimCodePoint()
        {
            if (delimiters == null)
            {
                maxDelimCodePoint = 0;
                return;
            }

            int m = 0;
            int c;
            int count = 0;
            for (int i = 0; i < delimiters.Length; i += CharExtensions.CharCount(c))
            {
                c = delimiters[i];
                if (c >= CharExtensions.MIN_HIGH_SURROGATE && c <= CharExtensions.MAX_LOW_SURROGATE)
                {
                    c = delimiters.CodePointAt(i);
                    hasSurrogates = true;
                }
                if (m < c)
                    m = c;
                count++;
            }
            maxDelimCodePoint = m;

            if (hasSurrogates)
            {
                delimiterCodePoints = new int[count];
                for (int i = 0, j = 0; i < count; i++, j += CharExtensions.CharCount(c))
                {
                    c = delimiters.CodePointAt(j);
                    delimiterCodePoints[i] = c;
                }
            }
        }

        
        /// <summary>
        /// Skips delimiters starting from the specified position. If retDelims
        /// is false, returns the index of the first non-delimiter character at or
        /// after startPos. If retDelims is true, startPos is returned.
        /// </summary>
        /// <param name="startPos"></param>
        /// <returns></returns>
        private int SkipDelimiters(int startPos)
        {
            if (delimiters == null)
                throw new NullReferenceException();

            int position = startPos;
            while (!retDelims && position < maxPosition)
            {
                if (!hasSurrogates)
                {
                    char c = str[position];
                    if ((c > maxDelimCodePoint) || (delimiters.IndexOf(c) < 0))
                        break;
                    position++;
                }
                else
                {
                    int c = str.CodePointAt(position);
                    if ((c > maxDelimCodePoint) || !IsDelimiter(c))
                    {
                        break;
                    }
                    position += CharExtensions.CharCount(c);
                }
            }
            return position;
        }

        /**
         * Skips ahead from startPos and returns the index of the next delimiter
         * character encountered, or maxPosition if no such delimiter is found.
         */
        private int ScanToken(int startPos)
        {
            int position = startPos;
            while (position < maxPosition)
            {
                if (!hasSurrogates)
                {
                    char c = str[position];
                    if ((c <= maxDelimCodePoint) && (delimiters.IndexOf(c) >= 0))
                        break;
                    position++;
                }
                else
                {
                    int c = str.CodePointAt(position);
                    if ((c <= maxDelimCodePoint) && IsDelimiter(c))
                        break;
                    position += CharExtensions.CharCount(c);
                }
            }
            if (retDelims && (startPos == position))
            {
                if (!hasSurrogates)
                {
                    char c = str[position];
                    if ((c <= maxDelimCodePoint) && (delimiters.IndexOf(c) >= 0))
                        position++;
                }
                else
                {
                    int c = str.CodePointAt(position);
                    if ((c <= maxDelimCodePoint) && IsDelimiter(c))
                        position += CharExtensions.CharCount(c);
                }
            }
            return position;
        }

        private bool IsDelimiter(int codePoint)
        {
            for (int i = 0; i < delimiterCodePoints.Length; i++)
            {
                if (delimiterCodePoints[i] == codePoint)
                {
                    return true;
                }
            }
            return false;
        }

        
        public bool HasMoreTokens()
        {
            /*
             * Temporarily store this position and use it in the following
             * nextToken() method only if the delimiters haven't been changed in
             * that nextToken() invocation.
             */
            newPosition = SkipDelimiters(currentPosition);
            return (newPosition < maxPosition);
        }

        /**
         * Returns the next token from this string tokenizer.
         *
         * @return     the next token from this string tokenizer.
         * @exception  NoSuchElementException  if there are no more tokens in this
         *               tokenizer's string.
         */
        public string NextToken()
        {
            /*
             * If next position already computed in hasMoreElements() and
             * delimiters have changed between the computation and this invocation,
             * then use the computed value.
             */

            currentPosition = (newPosition >= 0 && !delimsChanged) ?
                newPosition : SkipDelimiters(currentPosition);

            /* Reset these anyway */
            delimsChanged = false;
            newPosition = -1;

            if (currentPosition >= maxPosition)
                throw new NoSuchElementException();
            int start = currentPosition;
            currentPosition = ScanToken(currentPosition);
            return str.Substring(start, currentPosition);
        }

        /**
         * Returns the next token in this string tokenizer's string. First,
         * the set of characters considered to be delimiters by this
         * <tt>StringTokenizer</tt> object is changed to be the characters in
         * the string <tt>delim</tt>. Then the next token in the string
         * after the current position is returned. The current position is
         * advanced beyond the recognized token.  The new delimiter set
         * remains the default after this call.
         *
         * @param      delim   the new delimiters.
         * @return     the next token, after switching to the new delimiter set.
         * @exception  NoSuchElementException  if there are no more tokens in this
         *               tokenizer's string.
         * @exception NullPointerException if delim is <CODE>null</CODE>
         */
        public string NextToken(string delim)
        {
            delimiters = delim;

            /* delimiter string specified, so set the appropriate flag. */
            delimsChanged = true;

            SetMaxDelimCodePoint();
            return NextToken();
        }

        /**
         * Returns the same value as the <code>hasMoreTokens</code>
         * method. It exists so that this class can implement the
         * <code>Enumeration</code> interface.
         *
         * @return  <code>true</code> if there are more tokens;
         *          <code>false</code> otherwise.
         * @see     java.util.Enumeration
         * @see     java.util.StringTokenizer#hasMoreTokens()
         */
        public bool HasMoreElements()
        {
            return HasMoreTokens();
        }

        /**
         * Returns the same value as the <code>nextToken</code> method,
         * except that its declared return value is <code>object</code> rather than
         * <code>string</code>. It exists so that this class can implement the
         * <code>Enumeration</code> interface.
         *
         * @return     the next token in the string.
         * @exception  NoSuchElementException  if there are no more tokens in this
         *               tokenizer's string.
         * @see        java.util.Enumeration
         * @see        java.util.StringTokenizer#nextToken()
         */
        public object NextElement()
        {
            return NextToken();
        }

        /**
         * Calculates the number of times that this tokenizer's
         * <code>nextToken</code> method can be called before it generates an
         * exception. The current position is not advanced.
         *
         * @return  the number of tokens remaining in the string using the current
         *          delimiter set.
         * @see     java.util.StringTokenizer#nextToken()
         */
        public int CountTokens()
        {
            int count = 0;
            int currpos = currentPosition;
            while (currpos < maxPosition)
            {
                currpos = SkipDelimiters(currpos);
                if (currpos >= maxPosition)
                    break;
                currpos = ScanToken(currpos);
                count++;
            }
            return count;
        }



    }
}

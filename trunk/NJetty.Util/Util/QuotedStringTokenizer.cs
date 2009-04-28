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
using NJetty.Commons.Util;
using NJetty.Commons;

namespace NJetty.Util.Util
{

    /// <summary>
    /// TODO: Class/Interface Information here
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// November 2008
    /// </date>
    public class QuotedStringTokenizer : StringTokenizer
    {

        const string __delim = "\t\n\r";
        string _string;
        string _delim = __delim;
        bool _returnQuotes = false;
        bool _returnDelimiters = false;
        StringBuilder _token;
        bool _hasToken = false;
        int _i = 0;
        int _lastStart = 0;
        bool _double = true;
        bool _single = true;

        public QuotedStringTokenizer(string str,
                                     string delim,
                                     bool returnDelimiters,
                                     bool returnQuotes)
            : base("")
        {
            _string = str;
            if (delim != null)
                _delim = delim;
            _returnDelimiters = returnDelimiters;
            _returnQuotes = returnQuotes;

            if (_delim.IndexOf('\'') >= 0 ||
                _delim.IndexOf('"') >= 0)
                throw new SystemException("Can't use quotes as delimiters: " + _delim);

            _token = new StringBuilder(_string.Length > 1024 ? 512 : _string.Length / 2);
        }

        public QuotedStringTokenizer(string str,
                                     string delim,
                                     bool returnDelimiters)
            : this(str, delim, returnDelimiters, false)
        {

        }

        public QuotedStringTokenizer(string str,
                                     string delim)
            : this(str, delim, false, false)
        {

        }

        public QuotedStringTokenizer(string str)
            : this(str, null, false, false)
        {

        }

        /* ------------------------------------------------------------ */
        public bool hasMoreTokens()
        {
            // Already found a token
            if (_hasToken)
                return true;

            _lastStart = _i;

            int state = 0;
            bool escape = false;
            while (_i < _string.Length)
            {
                char c = _string[_i++];

                switch (state)
                {
                    case 0: // Start
                        if (_delim.IndexOf(c) >= 0)
                        {
                            if (_returnDelimiters)
                            {
                                _token.Append(c);
                                return _hasToken = true;
                            }
                        }
                        else if (c == '\'' && _single)
                        {
                            if (_returnQuotes)
                                _token.Append(c);
                            state = 2;
                        }
                        else if (c == '\"' && _double)
                        {
                            if (_returnQuotes)
                                _token.Append(c);
                            state = 3;
                        }
                        else
                        {
                            _token.Append(c);
                            _hasToken = true;
                            state = 1;
                        }
                        continue;

                    case 1: // Token
                        _hasToken = true;
                        if (_delim.IndexOf(c) >= 0)
                        {
                            if (_returnDelimiters)
                                _i--;
                            return _hasToken;
                        }
                        else if (c == '\'' && _single)
                        {
                            if (_returnQuotes)
                                _token.Append(c);
                            state = 2;
                        }
                        else if (c == '\"' && _double)
                        {
                            if (_returnQuotes)
                                _token.Append(c);
                            state = 3;
                        }
                        else
                            _token.Append(c);
                        continue;


                    case 2: // Single Quote
                        _hasToken = true;
                        if (escape)
                        {
                            escape = false;
                            _token.Append(c);
                        }
                        else if (c == '\'')
                        {
                            if (_returnQuotes)
                                _token.Append(c);
                            state = 1;
                        }
                        else if (c == '\\')
                        {
                            if (_returnQuotes)
                                _token.Append(c);
                            escape = true;
                        }
                        else
                            _token.Append(c);
                        continue;


                    case 3: // Double Quote
                        _hasToken = true;
                        if (escape)
                        {
                            escape = false;
                            _token.Append(c);
                        }
                        else if (c == '\"')
                        {
                            if (_returnQuotes)
                                _token.Append(c);
                            state = 1;
                        }
                        else if (c == '\\')
                        {
                            if (_returnQuotes)
                                _token.Append(c);
                            escape = true;
                        }
                        else
                            _token.Append(c);
                        continue;
                }
            }

            return _hasToken;
        }

        public new string NextToken()
        {
            if (!hasMoreTokens() || _token == null)
                throw new NoSuchElementException();
            string t = _token.ToString();
            _token.Length = 0;
            _hasToken = false;
            return t;
        }

        /* ------------------------------------------------------------ */
        public new string NextToken(string delim)
        {
            _delim = delim;
            _i = _lastStart;
            _token.Length = 0;
            _hasToken = false;
            return NextToken();
        }

        /* ------------------------------------------------------------ */
        public new bool HasMoreElements()
        {
            return hasMoreTokens();
        }

        /* ------------------------------------------------------------ */
        public new object NextElement()
        {
            return NextToken();
        }

        /* ------------------------------------------------------------ */
        /** Not implemented.
         */
        public new int CountTokens()
        {
            return -1;
        }


        /* ------------------------------------------------------------ */
        /** Quote a string.
         * The string is quoted only if quoting is required due to
         * embeded delimiters, quote characters or the
         * empty string.
         * @param s The string to quote.
         * @return quoted string
         */
        public static string Quote(string s, string delim)
        {
            if (s == null)
                return null;
            if (s.Length == 0)
                return "\"\"";


            for (int i = 0; i < s.Length; i++)
            {
                char c = s[i];
                if (c == '\\' || c == '"' || c == '\'' || Char.IsWhiteSpace(c) || delim.IndexOf(c) >= 0)
                {
                    StringBuilder b = new StringBuilder(s.Length + 8);
                    Quote(b, s);
                    return b.ToString();
                }
            }

            return s;
        }

        /* ------------------------------------------------------------ */
        /** Quote a string.
         * The string is quoted only if quoting is required due to
         * embeded delimiters, quote characters or the
         * empty string.
         * @param s The string to quote.
         * @return quoted string
         */
        public static string Quote(string s)
        {
            if (s == null)
                return null;
            if (s.Length == 0)
                return "\"\"";

            StringBuilder b = new StringBuilder(s.Length + 8);
            Quote(b, s);
            return b.ToString();

        }


        /// <summary>
        /// Quote a string into a StringBuilder.
        /// The characters ", \, \n, \r, \t, \f and \b are escaped
        /// </summary>
        /// <param name="buf">StringBuilder Data</param>
        /// <param name="s">string to quote</param>
        public static void Quote(StringBuilder buf, string s)
        {
            lock (buf)
            {
                buf.Append('"');

                int i = 0;
                for (; i < s.Length; i++)
                {
                    char c = s[i];
                    switch (c)
                    {
                        case '"':
                            buf.Append(s, 0, i);
                            buf.Append("\\\"");
                            goto EndOfLoop;
                        case '\\':
                            buf.Append(s, 0, i);
                            buf.Append("\\\\");
                            goto EndOfLoop;
                        case '\n':
                            buf.Append(s, 0, i);
                            buf.Append("\\n");
                            goto EndOfLoop;
                        case '\r':
                            buf.Append(s, 0, i);
                            buf.Append("\\r");
                            goto EndOfLoop;
                        case '\t':
                            buf.Append(s, 0, i);
                            buf.Append("\\t");
                            goto EndOfLoop;
                        case '\f':
                            buf.Append(s, 0, i);
                            buf.Append("\\f");
                            goto EndOfLoop;
                        case '\b':
                            buf.Append(s, 0, i);
                            buf.Append("\\b");
                            goto EndOfLoop;

                        default:
                            continue;
                    }
                }
            EndOfLoop:

                if (i == s.Length)
                    buf.Append(s);
                else
                {
                    i++;
                    for (; i < s.Length; i++)
                    {
                        char c = s[i];
                        switch (c)
                        {
                            case '"':
                                buf.Append("\\\"");
                                continue;
                            case '\\':
                                buf.Append("\\\\");
                                continue;
                            case '\n':
                                buf.Append("\\n");
                                continue;
                            case '\r':
                                buf.Append("\\r");
                                continue;
                            case '\t':
                                buf.Append("\\t");
                                continue;
                            case '\f':
                                buf.Append("\\f");
                                continue;
                            case '\b':
                                buf.Append("\\b");
                                continue;

                            default:
                                buf.Append(c);
                                continue;
                        }
                    }
                }

                buf.Append('"');
            }



        }


        /// <summary>
        /// Quote a string into a StringBuilder.
        /// The characters ", \, \n, \r, \t, \f, \b are escaped.
        /// Quotes are forced if any escaped characters are present or there
        /// is a ", ', space, + or % character.
        /// </summary>
        /// <param name="buf">StringBuilder data</param>
        /// <param name="s">string to quote</param>
        public static void QuoteIfNeeded(StringBuilder buf, string s)
        {
            lock (buf)
            {
                int e = -1;

                for (int i = 0; i < s.Length; i++)
                {
                    char c = s[i];
                    switch (c)
                    {
                        case '"':
                        case '\\':
                        case '\n':
                        case '\r':
                        case '\t':
                        case '\f':
                        case '\b':
                        case '%':
                        case '+':
                        case ' ':
                            e = i;
                            buf.Append('"');
                            // TODO when 1.4 support is dropped: buf.Append(s,0,e);
                            for (int j = 0; j < e; j++)
                                buf.Append(s[j]);
                            goto search;

                        default:
                            continue;
                    }
                }

            search:

                if (e < 0)
                {
                    buf.Append(s);
                    return;
                }

                for (int i = e; i < s.Length; i++)
                {
                    char c = s[i];
                    switch (c)
                    {
                        case '"':
                            buf.Append("\\\"");
                            continue;
                        case '\\':
                            buf.Append("\\\\");
                            continue;
                        case '\n':
                            buf.Append("\\n");
                            continue;
                        case '\r':
                            buf.Append("\\r");
                            continue;
                        case '\t':
                            buf.Append("\\t");
                            continue;
                        case '\f':
                            buf.Append("\\f");
                            continue;
                        case '\b':
                            buf.Append("\\b");
                            continue;

                        default:
                            buf.Append(c);
                            continue;
                    }
                }
                buf.Append('"');
            }
        }

        /// <summary>
        /// Unquote a string.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string Unquote(string s)
        {
            if (s == null)
                return null;
            if (s.Length < 2)
                return s;

            char first = s[0];
            char last = s[s.Length - 1];
            if (first != last || (first != '"' && first != '\''))
                return s;

            StringBuilder b = new StringBuilder(s.Length - 2);
            lock (b)
            {
                bool escape = false;
                for (int i = 1; i < s.Length - 1; i++)
                {
                    char c = s[i];

                    if (escape)
                    {
                        escape = false;
                        switch (c)
                        {
                            case 'n':
                                b.Append('\n');
                                break;
                            case 'r':
                                b.Append('\r');
                                break;
                            case 't':
                                b.Append('\t');
                                break;
                            case 'f':
                                b.Append('\f');
                                break;
                            case 'b':
                                b.Append('\b');
                                break;
                            case 'u':
                                b.Append((char)(
                                        (TypeUtil.ConvertHexDigit((byte)s[i++]) << 24) +
                                        (TypeUtil.ConvertHexDigit((byte)s[i++]) << 16) +
                                        (TypeUtil.ConvertHexDigit((byte)s[i++]) << 8) +
                                        (TypeUtil.ConvertHexDigit((byte)s[i++]))
                                        )
                                );
                                break;
                            default:
                                b.Append(c);
                                break;
                        }
                    }
                    else if (c == '\\')
                    {
                        escape = true;
                        continue;
                    }
                    else
                        b.Append(c);
                }

                return b.ToString();
            }
        }

        /// <summary>
        /// Gets or Sets, handling double quotes
        /// </summary>
        public bool DoubleQuotes
        {
            get { return _double; }
            set { _double = value; }
        }

        /// <summary>
        /// Gets or Sets, handling of single quotes
        /// </summary>
        public bool SingleQuotes
        {
            get { return _single; }
            set { _single = value; }
        }
    }
}

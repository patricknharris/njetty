#region Apache License
//
// Licensed to the Apache Software Foundation (ASF) under one or more 
// contributor license agreements. See the NOTICE file distributed with
// this work for additional information regarding copyright ownership. 
// The ASF licenses this file to you under the Apache License, Version 2.0
// (the "License"); you may not use this file except input compliance with 
// the License. You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to input writing, software
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
using System.IO;

namespace NJetty.Util.Util
{

    /// <summary>
    /// Handles coding of MIME  "x-www-form-urlencoded".
    /// This class handles the encoding and decoding for either
    /// the query string of a URL or the _content of a POST HTTP request.
    /// Notes: 
    /// The hashtable either contains string single values, vectors
    /// of string or arrays of Strings.
    /// 
    /// This class is only partially synchronised.  In particular, simple
    /// get operations are not protected from concurrent updates.
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// November 2008
    /// </date>
    public class UrlEncoded : MultiMap<string>
    {


        public UrlEncoded(UrlEncoded url)
            : base(url)
        { }

        public UrlEncoded()
            : base(6)
        { }

        public UrlEncoded(string s)
            : base(6)
        {
            Decode(s, StringUtil.__UTF8);
        }

        public UrlEncoded(string s, string charset)
            : base(6)
        {
            Decode(s, charset);
        }

        public void Decode(string query)
        {
            DecodeTo(query, this, StringUtil.__UTF8);
        }

        public void Decode(string query, string charset)
        {
            DecodeTo(query, this, charset);
        }

        /// <summary>
        /// Encode Hashtable with % encoding.
        /// </summary>
        /// <returns></returns>
        public string Encode()
        {
            return Encode(StringUtil.__UTF8, false);
        }

        /// <summary>
        /// Encode Hashtable with % encoding.
        /// </summary>
        /// <param name="charset"></param>
        /// <returns></returns>
        public string Encode(string charset)
        {
            return Encode(charset, false);
        }

        /// <summary>
        /// Lock object
        /// </summary>
        object _lock = new object();

        /// <summary>
        /// Encode Hashtable with % encoding.
        /// </summary>
        /// <param name="charset">Characterset Encoding</param>
        /// <param name="equalsForNullValue">if True, then an '=' is always used, even
        /// for parameters without a value. e.g. "blah?a=&b=&c=".
        /// </param>
        /// <returns></returns>
        public string Encode(string charset, bool equalsForNullValue)
        {
            lock (_lock)
            {
                return Encode(this, charset, equalsForNullValue);
            }
        }

        /// <summary>
        /// Encode Hashtable with % encoding.
        /// </summary>
        /// <param name="map">multimap values to encode</param>
        /// <param name="charset">Characterset Encoding</param>
        /// <param name="equalsForNullValue">if True, then an '=' is always used, even
        /// for parameters without a value. e.g. "blah?a=&b=&c=".
        /// </param>
        /// <returns>Encoded String Value</returns>
        public static string Encode(MultiMap<string> map, string charset, bool equalsForNullValue)
        {
            if (charset == null)
                charset = StringUtil.__UTF8;

            StringBuilder result = new StringBuilder(128);
            
            bool first = true;
            foreach (string key in map.Keys)
            {
                if (!first)
                    result.Append('&');

                object list = map[key];
                int s = LazyList.Size(list);

                if (s == 0)
                {
                    result.Append(EncodeString(key, charset));
                    if (equalsForNullValue)
                        result.Append('=');
                }
                else
                {
                    for (int i = 0; i < s; i++)
                    {
                        if (i > 0)
                            result.Append('&');
                        object val = LazyList.Get(list, i);
                        result.Append(EncodeString(key, charset));

                        if (val != null)
                        {
                            string str = val.ToString();
                            if (str.Length > 0)
                            {
                                result.Append('=');
                                result.Append(EncodeString(str, charset));
                            }
                            else if (equalsForNullValue)
                                result.Append('=');
                        }
                        else if (equalsForNullValue)
                            result.Append('=');
                    }
                }

                first = false;

            }


            return result.ToString();
        }


        /// <summary>
        /// Decoded parameters to Map.
        /// </summary>
        /// <param name="content">the string containing the encoded parameters</param>
        /// <param name="map"></param>
        /// <param name="charset"></param>
        public static void DecodeTo(string content, MultiMap<string> map, string charset)
        {
            if (charset == null)
                charset = StringUtil.__UTF8;

            lock (map)
            {
                string key = null;
                string value = null;
                int mark = -1;
                bool encoded = false;
                for (int i = 0; i < content.Length; i++)
                {
                    char c = content[i];
                    switch (c)
                    {
                        case '&':
                            int l = i - mark - 1;
                            value = l == 0 ? "" :
                                (encoded ? DecodeString(content, mark + 1, l, charset) : content.Substring(mark + 1, i - (mark + 1)));
                            mark = i;
                            encoded = false;
                            if (key != null)
                            {
                                map.Append(key, value);
                            }
                            else if (value != null && value.Length > 0)
                            {
                                map.Append(value, "");
                            }
                            key = null;
                            value = null;
                            break;
                        case '=':
                            if (key != null)
                                break;
                            key = encoded ? DecodeString(content, mark + 1, i - mark - 1, charset) : content.Substring(mark + 1, i - (mark + 1));
                            mark = i;
                            encoded = false;
                            break;
                        case '+':
                            encoded = true;
                            break;
                        case '%':
                            encoded = true;
                            break;
                    }
                }

                if (key != null)
                {
                    int l = content.Length - mark - 1;
                    value = l == 0 ? "" : (encoded ? DecodeString(content, mark + 1, l, charset) : content.Substring(mark + 1));
                    map.Append(key, value);
                }
                else if (mark < content.Length)
                {
                    key = encoded
                        ? DecodeString(content, mark + 1, content.Length - mark - 1, charset)
                        : content.Substring(mark + 1);
                    map.Append(key, "");
                }
            }
        }

        /// <summary>
        /// Decoded parameters to Map.
        /// </summary>
        /// <param name="raw">the byte[] containing the encoded parameters</param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <param name="map"></param>
        public static void DecodeUtf8To(byte[] raw, int offset, int length, MultiMap<string> map)
        {
            DecodeUtf8To(raw, offset, length, map, new Utf8StringBuilder());
        }

        /* -------------------------------------------------------------- */
        /** Decoded parameters to Map.
         * @param data the byte[] containing the encoded parameters
         */
        public static void DecodeUtf8To(byte[] raw, int offset, int length, MultiMap<string> map, Utf8StringBuilder buffer)
        {
            lock (map)
            {
                string key = null;
                string value = null;

                // TODO cache of parameter names ???
                int end = offset + length;
                for (int i = offset; i < end; i++)
                {
                    byte b = raw[i];
                    switch ((char)(0xff & b))
                    {
                        case '&':
                            value = buffer.Length == 0 ? "" : buffer.ToString();
                            buffer.Reset();
                            if (key != null)
                            {
                                map.Append(key, value);
                            }
                            else if (value != null && value.Length > 0)
                            {
                                map.Append(value, "");
                            }
                            key = null;
                            value = null;
                            break;

                        case '=':
                            if (key != null)
                            {
                                buffer.Append(b);
                                break;
                            }
                            key = buffer.ToString();
                            buffer.Reset();
                            break;

                        case '+':
                            buffer.Append((byte)' ');
                            break;

                        case '%':
                            if (i + 2 < end)
                                buffer.Append((byte)((TypeUtil.ConvertHexDigit(raw[++i]) << 4) + TypeUtil.ConvertHexDigit(raw[++i])));
                            break;
                        default:
                            buffer.Append(b);
                            break;
                    }
                }

                if (key != null)
                {
                    value = buffer.Length == 0 ? "" : buffer.ToString();
                    buffer.Reset();
                    map.Append(key, value);
                }
                else if (buffer.Length > 0)
                {
                    map.Append(buffer.ToString(), "");
                }
            }
        }

        /// <summary>
        /// Decoded parameters to Map.
        /// </summary>
        /// <param name="input">InputSteam to read</param>
        /// <param name="map">MultiMap to Add parameters to</param>
        /// <param name="maxLength">maximum length of content to read 0r -1 for no limit</param>
        public static void Decode88591To(Stream input, MultiMap<string> map, int maxLength)
        {
            lock (map)
            {
                StringBuilder buffer = new StringBuilder();
                string key = null;
                string value = null;

                int b;

                // TODO cache of parameter names ???
                int totalLength = 0;
                while ((b = input.ReadByte()) >= 0)
                {
                    switch ((char)b)
                    {
                        case '&':
                            value = buffer.Length == 0 ? "" : buffer.ToString();
                            buffer.Length = 0;
                            if (key != null)
                            {
                                map.Append(key, value);
                            }
                            else if (value != null && value.Length > 0)
                            {
                                map.Append(value, "");
                            }
                            key = null;
                            value = null;
                            break;

                        case '=':
                            if (key != null)
                            {
                                buffer.Append((char)b);
                                break;
                            }
                            key = buffer.ToString();
                            buffer.Length = 0;
                            break;

                        case '+':
                            buffer.Append(' ');
                            break;

                        case '%':
                            int dh = input.ReadByte();
                            int dl = input.ReadByte();
                            if (dh < 0 || dl < 0)
                                break;
                            buffer.Append((char)((TypeUtil.ConvertHexDigit((byte)dh) << 4) + TypeUtil.ConvertHexDigit((byte)dl)));
                            break;
                        default:
                            buffer.Append((char)b);
                            break;
                    }
                    if (maxLength >= 0 && (++totalLength > maxLength))
                        throw new InvalidOperationException("Form too large");
                }

                if (key != null)
                {
                    value = buffer.Length == 0 ? "" : buffer.ToString();
                    buffer.Length = 0;
                    map.Append(key, value);
                }
                else if (buffer.Length > 0)
                {
                    map.Append(buffer.ToString(), "");
                }
            }
        }


        /// <summary>
        /// Decoded parameters to Map.
        /// </summary>
        /// <param name="input">InputSteam to read</param>
        /// <param name="map">MultiMap to Add parameters to</param>
        /// <param name="maxLength">maximum length of conent to read 0r -1 for no limit</param>
        public static void DecodeUtf8To(Stream input, MultiMap<string> map, int maxLength)
        {
            lock (map)
            {
                Utf8StringBuilder buffer = new Utf8StringBuilder();
                string key = null;
                string value = null;

                int b;

                // TODO cache of parameter names ???
                int totalLength = 0;
                while ((b = input.ReadByte()) >= 0)
                {
                    switch ((char)b)
                    {
                        case '&':
                            value = buffer.Length == 0 ? "" : buffer.ToString();
                            buffer.Reset();
                            if (key != null)
                            {
                                map.Append(key, value);
                            }
                            else if (value != null && value.Length > 0)
                            {
                                map.Append(value, "");
                            }
                            key = null;
                            value = null;
                            break;

                        case '=':
                            if (key != null)
                            {
                                buffer.Append((byte)b);
                                break;
                            }
                            key = buffer.ToString();
                            buffer.Reset();
                            break;

                        case '+':
                            buffer.Append((byte)' ');
                            break;

                        case '%':
                            int dh = input.ReadByte();
                            int dl = input.ReadByte();
                            if (dh < 0 || dl < 0)
                                break;
                            buffer.Append((byte)((TypeUtil.ConvertHexDigit((byte)dh) << 4) + TypeUtil.ConvertHexDigit((byte)dl)));
                            break;
                        default:
                            buffer.Append((byte)b);
                            break;
                    }
                    if (maxLength >= 0 && (++totalLength > maxLength))
                        throw new InvalidOperationException("Form too large");
                }

                if (key != null)
                {
                    value = buffer.Length == 0 ? "" : buffer.ToString();
                    buffer.Reset();
                    map.Append(key, value);
                }
                else if (buffer.Length > 0)
                {
                    map.Append(buffer.ToString(), "");
                }
            }
        }

        public static void DecodeUtf16To(Stream input, MultiMap<string> map, int maxLength)
        {
            StreamReader reader = new StreamReader(input, Encoding.GetEncoding(StringUtil.__UTF16));
            StringBuilder buf = new StringBuilder();

            

            int c;
            int length = 0;
            if (maxLength < 0)
                maxLength = int.MaxValue;
            while ((c = reader.Read()) > 0 && length++ < maxLength)
                buf.Append((char)c);
            DecodeTo(buf.ToString(), map, StringUtil.__UTF8);
        }

        
        /// <summary>
        /// Decoded parameters to Map.
        /// </summary>
        /// <param name="input">the stream containing the encoded parameters</param>
        /// <param name="map"></param>
        /// <param name="charset"></param>
        /// <param name="maxLength"></param>
        public static void DecodeTo(Stream input, MultiMap<string> map, string charset, int maxLength)
        {
            if (charset == null || StringUtil.__ISO_8859_1.Equals(charset))
            {
                Decode88591To(input, map, maxLength);
                return;
            }

            if (StringUtil.__UTF8.Equals(charset, StringComparison.OrdinalIgnoreCase))
            {
                DecodeUtf8To(input, map, maxLength);
                return;
            }

            if (StringUtil.__UTF16.Equals(charset, StringComparison.OrdinalIgnoreCase)) // Should be all 2 byte encodings
            {
                DecodeUtf16To(input, map, maxLength);
                return;
            }


            lock (map)
            {
                string key = null;
                string value = null;

                int c;
                int digit = 0;
                int digits = 0;

                int totalLength = 0;
                ByteArrayOutputStream2 output = new ByteArrayOutputStream2();
                input.Position = 0;
                long size = 0;

                while ((c = input.ReadByte()) > 0)
                {
                    switch ((char)c)
                    {
                        case '&':
                            size = output.Length;
                            value = size == 0 ? "" : Encoding.GetEncoding(charset).GetString(output.GetBuffer(), 0, (int)size);
                            output.Position = 0;
                            if (key != null)
                            {
                                map.Append(key, value);
                            }
                            else if (value != null && value.Length > 0)
                            {
                                map.Append(value, "");
                            }
                            key = null;
                            value = null;
                            break;
                        case '=':
                            if (key != null)
                            {
                                output.WriteByte(c);
                                break;
                            }
                            size = output.Length;
                            key = size == 0 ? "" : Encoding.GetEncoding(charset).GetString(output.GetBuffer(), 0, (int)size);
                            output.Position = 0;
                            break;
                        case '+':
                            output.WriteByte(' ');
                            break;
                        case '%':
                            digits = 2;
                            break;
                        default:
                            if (digits == 2)
                            {
                                digit = TypeUtil.ConvertHexDigit((byte)c);
                                digits = 1;
                            }
                            else if (digits == 1)
                            {
                                int v = (byte)(digit << 4) + TypeUtil.ConvertHexDigit((byte)c);
                                output.WriteByte(v);
                                digits = 0;
                            }
                            else
                                output.WriteByte((byte)c);
                            break;
                    }

                    totalLength++;
                    if (maxLength >= 0 && totalLength > maxLength)
                        throw new InvalidOperationException("Form too large");
                }

                size = output.Length;
                if (key != null)
                {
                    value = size == 0 ? "" : Encoding.GetEncoding(charset).GetString(output.GetBuffer(),0, (int)size);
                    output.Position = 0;
                    map.Append(key, value);
                }
                else if (size > 0)
                    map.Append(Encoding.GetEncoding(charset).GetString(output.GetBuffer(),0, (int)size), "");
            }
        }

        
        /// <summary>
        /// Decode string with % encoding.
        /// This method makes the assumption that the majority of calls
        /// will need no decoding.
        /// </summary>
        /// <param name="encoded"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <param name="charset"></param>
        /// <returns></returns>
        public static string DecodeString(string encoded, int offset, int length, string charset)
        {
            if (charset == null || StringUtil.IsUTF8(charset))
            {
                Utf8StringBuffer buffer = null;

                for (int i = 0; i < length; i++)
                {
                    char c = encoded[offset + i];
                    if (c < 0 || c > 0xff)
                    {
                        if (buffer == null)
                        {
                            buffer = new Utf8StringBuffer(length);
                            buffer.Append(encoded, offset, i+1);
                            

                        }
                        else
                            buffer.Append((byte)c);
                    }
                    else if (c == '+')
                    {
                        if (buffer == null)
                        {
                            buffer = new Utf8StringBuffer(length);
                            buffer.Append(encoded, offset, i);
                        }

                        buffer.Append((byte)' ');
                    }
                    else if (c == '%' && (i + 2) < length)
                    {
                        if (buffer == null)
                        {
                            buffer = new Utf8StringBuffer(length);
                            buffer.Append(encoded, offset, i);
                        }

                        while (c == '%' && (i + 2) < length)
                        {
                            byte b = (byte)TypeUtil.ParseInt(encoded, offset + i + 1, 2, 16);
                            buffer.Append(b);
                            i += 3;
                            if (i < length)
                                c = encoded[offset + i];
                        }
                        i--;
                    }
                    else if (buffer != null)
                        buffer.Append((byte)c);
                }

                if (buffer == null)
                {
                    if (offset == 0 && encoded.Length == length)
                        return encoded;
                    return encoded.Substring(offset, length);
                }

                return buffer.ToString();
            }
            else
            {
                StringBuilder buffer = null;

                try
                {
                    for (int i = 0; i < length; i++)
                    {
                        char c = encoded[offset + i];
                        if (c < 0 || c > 0xff)
                        {
                            if (buffer == null)
                            {
                                buffer = new StringBuilder(length);
                                buffer.Append(encoded, offset, i + 1);
                            }
                            else
                                buffer.Append(c);
                        }
                        else if (c == '+')
                        {
                            if (buffer == null)
                            {
                                buffer = new StringBuilder(length);
                                buffer.Append(encoded, offset, i);
                            }

                            buffer.Append(' ');
                        }
                        else if (c == '%' && (i + 2) < length)
                        {
                            if (buffer == null)
                            {
                                buffer = new StringBuilder(length);
                                buffer.Append(encoded, offset, i);
                            }

                            byte[] ba = new byte[length];
                            int n = 0;
                            while (c >= 0 && c <= 0xff)
                            {
                                if (c == '%')
                                {
                                    ba[n++] = (byte)TypeUtil.ParseInt(encoded, offset + i + 1, 2, 16);
                                    i += 3;
                                }
                                else if (c == '+')
                                {
                                    ba[n++] = (byte)' ';
                                    i++;
                                }
                                else
                                {
                                    ba[n++] = (byte)c;
                                    i++;
                                }

                                if (i >= length)
                                    break;
                                c = encoded[offset + i];
                            }

                            i--;
                            buffer.Append(Encoding.GetEncoding(charset).GetString(ba, 0,n));

                        }
                        else if (buffer != null)
                            buffer.Append(c);
                    }

                    if (buffer == null)
                    {
                        if (offset == 0 && encoded.Length == length)
                            return encoded;
                        return encoded.Substring(offset, length);
                    }

                    return buffer.ToString();
                }
                catch (ArgumentException e)
                {
                    throw new SystemException(e.Message, e);
                }
            }

        }

   
        /// <summary>
        /// Perform URL encoding.
        /// Assumes 8859 charset/ UTF-8 charset
        /// </summary>
        /// <param name="str">string to encode</param>
        /// <returns>encoded string.</returns>
        public static string EncodeString(string str)
        {
            return EncodeString(str, StringUtil.__UTF8);
        }

        /// <summary>
        /// Perform URL encoding.
        /// </summary>
        /// <param name="str">string to encode</param>
        /// <param name="charset">Characterset Encoding</param>
        /// <returns></returns>
        public static string EncodeString(string str, string charset)
        {
            if (charset == null)
                charset = StringUtil.__UTF8;
            byte[] bytes = null;
            try
            {
                bytes = Encoding.GetEncoding(charset).GetBytes(str);
            }
            catch (ArgumentException)
            {
                // Log.warn(LogSupport.EXCEPTION,e);
                bytes = Encoding.ASCII.GetBytes(str);
            }

            int len = bytes.Length;
            byte[] encoded = new byte[bytes.Length * 3];
            int n = 0;
            bool noEncode = true;

            for (int i = 0; i < len; i++)
            {
                byte b = bytes[i];

                if (b == ' ')
                {
                    noEncode = false;
                    encoded[n++] = (byte)'+';
                }
                else if (b >= 'a' && b <= 'z' ||
                         b >= 'A' && b <= 'Z' ||
                         b >= '0' && b <= '9')
                {
                    encoded[n++] = b;
                }
                else
                {
                    noEncode = false;
                    encoded[n++] = (byte)'%';
                    byte nibble = (byte)((b & 0xf0) >> 4);
                    if (nibble >= 10)
                        encoded[n++] = (byte)('A' + nibble - 10);
                    else
                        encoded[n++] = (byte)('0' + nibble);
                    nibble = (byte)(b & 0xf);
                    if (nibble >= 10)
                        encoded[n++] = (byte)('A' + nibble - 10);
                    else
                        encoded[n++] = (byte)('0' + nibble);
                }
            }

            if (noEncode)
                return str;

            try
            {
                return Encoding.GetEncoding(charset).GetString(encoded, 0, n);
            }
            catch (ArgumentException)
            {
                // Log.warn(LogSupport.EXCEPTION,e);
                return Encoding.ASCII.GetString(encoded, 0, n);
            }
        }


        public object Clone()
        {
            return new UrlEncoded(this);
        }

    }
}

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

        /* ----------------------------------------------------------------- */
        public void Decode(string query)
        {
            DecodeTo(query, this, StringUtil.__UTF8);
        }

        /* ----------------------------------------------------------------- */
        public void Decode(string query, string charset)
        {
            decodeTo(query, this, charset);
        }

        /* -------------------------------------------------------------- */
        /** Encode Hashtable with % encoding.
         */
        public string Encode()
        {
            return Encode(StringUtil.__UTF8, false);
        }

        /* -------------------------------------------------------------- */
        /** Encode Hashtable with % encoding.
         */
        public string Encode(string charset)
        {
            return Encode(charset, false);
        }

        /* -------------------------------------------------------------- */
        /** Encode Hashtable with % encoding.
         * @param equalsForNullValue if True, then an '=' is always used, even
         * for parameters without a value. e.g. "blah?a=&b=&c=".
         */
        object _lock = new object();

        public string Encode(string charset, bool equalsForNullValue)
        {
            lock (_lock)
            {
                return Encode(this, charset, equalsForNullValue);
            }
        }

        /* -------------------------------------------------------------- */
        /** Encode Hashtable with % encoding.
         * @param equalsForNullValue if True, then an '=' is always used, even
         * for parameters without a value. e.g. "blah?a=&b=&c=".
         */
        public static string Encode(MultiMap<string> map, string charset, bool equalsForNullValue)
        {
            if (charset == null)
                charset = StringUtil.__UTF8;

            StringBuilder result = new StringBuilder(128);

            Iterator iter = map.entrySet().iterator();
            while (iter.hasNext())
            {
                Map.Entry entry = (Map.Entry)iter.next();

                string key = entry.getKey().toString();
                object list = entry.getValue();
                int s = LazyList.size(list);

                if (s == 0)
                {
                    result.append(encodeString(key, charset));
                    if (equalsForNullValue)
                        result.append('=');
                }
                else
                {
                    for (int i = 0; i < s; i++)
                    {
                        if (i > 0)
                            result.append('&');
                        object val = LazyList.get(list, i);
                        result.append(encodeString(key, charset));

                        if (val != null)
                        {
                            string str = val.toString();
                            if (str.length() > 0)
                            {
                                result.append('=');
                                result.append(encodeString(str, charset));
                            }
                            else if (equalsForNullValue)
                                result.append('=');
                        }
                        else if (equalsForNullValue)
                            result.append('=');
                    }
                }
                if (iter.hasNext())
                    result.append('&');
            }
            return result.toString();
        }



        /* -------------------------------------------------------------- */
        /** Decoded parameters to Map.
         * @param content the string containing the encoded parameters
         */
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
                for (int i = 0; i < content.length(); i++)
                {
                    char c = content.charAt(i);
                    switch (c)
                    {
                        case '&':
                            int l = i - mark - 1;
                            value = l == 0 ? "" :
                                (encoded ? decodeString(content, mark + 1, l, charset) : content.substring(mark + 1, i));
                            mark = i;
                            encoded = false;
                            if (key != null)
                            {
                                map.add(key, value);
                            }
                            else if (value != null && value.length() > 0)
                            {
                                map.add(value, "");
                            }
                            key = null;
                            value = null;
                            break;
                        case '=':
                            if (key != null)
                                break;
                            key = encoded ? decodeString(content, mark + 1, i - mark - 1, charset) : content.substring(mark + 1, i);
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
                    int l = content.length() - mark - 1;
                    value = l == 0 ? "" : (encoded ? decodeString(content, mark + 1, l, charset) : content.substring(mark + 1));
                    map.add(key, value);
                }
                else if (mark < content.length())
                {
                    key = encoded
                        ? decodeString(content, mark + 1, content.length() - mark - 1, charset)
                        : content.substring(mark + 1);
                    map.add(key, "");
                }
            }
        }

        /* -------------------------------------------------------------- */
        /** Decoded parameters to Map.
         * @param data the byte[] containing the encoded parameters
         */
        public static void DecodeUtf8To(byte[] raw, int offset, int length, MultiMap<string> map)
        {
            decodeUtf8To(raw, offset, length, map, new Utf8StringBuilder());
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
                            value = buffer.length() == 0 ? "" : buffer.toString();
                            buffer.reset();
                            if (key != null)
                            {
                                map.add(key, value);
                            }
                            else if (value != null && value.length() > 0)
                            {
                                map.add(value, "");
                            }
                            key = null;
                            value = null;
                            break;

                        case '=':
                            if (key != null)
                            {
                                buffer.append(b);
                                break;
                            }
                            key = buffer.toString();
                            buffer.reset();
                            break;

                        case '+':
                            buffer.append((byte)' ');
                            break;

                        case '%':
                            if (i + 2 < end)
                                buffer.append((byte)((TypeUtil.convertHexDigit(raw[++i]) << 4) + TypeUtil.convertHexDigit(raw[++i])));
                            break;
                        default:
                            buffer.append(b);
                            break;
                    }
                }

                if (key != null)
                {
                    value = buffer.length() == 0 ? "" : buffer.toString();
                    buffer.reset();
                    map.add(key, value);
                }
                else if (buffer.length() > 0)
                {
                    map.add(buffer.toString(), "");
                }
            }
        }

        /* -------------------------------------------------------------- */
        /** Decoded parameters to Map.
         * @param input InputSteam to read
         * @param map MultiMap to add parameters to
         * @param maxLength maximum length of content to read 0r -1 for no limit
         */
        public static void Decode88591To(InputStream input, MultiMap<string> map, int maxLength)
        {
            lock (map)
            {
                StringBuilder buffer = new StringBuilder();
                string key = null;
                string value = null;

                int b;

                // TODO cache of parameter names ???
                int totalLength = 0;
                while ((b = input.read()) >= 0)
                {
                    switch ((char)b)
                    {
                        case '&':
                            value = buffer.length() == 0 ? "" : buffer.toString();
                            buffer.setLength(0);
                            if (key != null)
                            {
                                map.add(key, value);
                            }
                            else if (value != null && value.length() > 0)
                            {
                                map.add(value, "");
                            }
                            key = null;
                            value = null;
                            break;

                        case '=':
                            if (key != null)
                            {
                                buffer.append((char)b);
                                break;
                            }
                            key = buffer.toString();
                            buffer.setLength(0);
                            break;

                        case '+':
                            buffer.append((char)' ');
                            break;

                        case '%':
                            int dh = input.read();
                            int dl = input.read();
                            if (dh < 0 || dl < 0)
                                break;
                            buffer.append((char)((TypeUtil.convertHexDigit((byte)dh) << 4) + TypeUtil.convertHexDigit((byte)dl)));
                            break;
                        default:
                            buffer.append((char)b);
                            break;
                    }
                    if (maxLength >= 0 && (++totalLength > maxLength))
                        throw new IllegalStateException("Form too large");
                }

                if (key != null)
                {
                    value = buffer.length() == 0 ? "" : buffer.toString();
                    buffer.setLength(0);
                    map.add(key, value);
                }
                else if (buffer.length() > 0)
                {
                    map.add(buffer.toString(), "");
                }
            }
        }

        /* -------------------------------------------------------------- */
        /** Decoded parameters to Map.
         * @param input InputSteam to read
         * @param map MultiMap to add parameters to
         * @param maxLength maximum length of conent to read 0r -1 for no limit
         */
        public static void DecodeUtf8To(InputStream input, MultiMap<string> map, int maxLength)
        {
            lock (map)
            {
                Utf8StringBuilder buffer = new Utf8StringBuilder();
                string key = null;
                string value = null;

                int b;

                // TODO cache of parameter names ???
                int totalLength = 0;
                while ((b = input.read()) >= 0)
                {
                    switch ((char)b)
                    {
                        case '&':
                            value = buffer.length() == 0 ? "" : buffer.toString();
                            buffer.reset();
                            if (key != null)
                            {
                                map.add(key, value);
                            }
                            else if (value != null && value.length() > 0)
                            {
                                map.add(value, "");
                            }
                            key = null;
                            value = null;
                            break;

                        case '=':
                            if (key != null)
                            {
                                buffer.append((byte)b);
                                break;
                            }
                            key = buffer.toString();
                            buffer.reset();
                            break;

                        case '+':
                            buffer.append((byte)' ');
                            break;

                        case '%':
                            int dh = input.read();
                            int dl = input.read();
                            if (dh < 0 || dl < 0)
                                break;
                            buffer.append((byte)((TypeUtil.convertHexDigit((byte)dh) << 4) + TypeUtil.convertHexDigit((byte)dl)));
                            break;
                        default:
                            buffer.append((byte)b);
                            break;
                    }
                    if (maxLength >= 0 && (++totalLength > maxLength))
                        throw new IllegalStateException("Form too large");
                }

                if (key != null)
                {
                    value = buffer.length() == 0 ? "" : buffer.toString();
                    buffer.reset();
                    map.add(key, value);
                }
                else if (buffer.length() > 0)
                {
                    map.add(buffer.toString(), "");
                }
            }
        }

        /* -------------------------------------------------------------- */
        public static void DecodeUtf16To(InputStream input, MultiMap<string> map, int maxLength)
        {
            InputStreamReader input = new InputStreamReader(input, StringUtil.__UTF16);
            StringBuilder buf = new StringBuilder();

            int c;
            int length = 0;
            if (maxLength < 0)
                maxLength = Integer.MAX_VALUE;
            while ((c = input.read()) > 0 && length++ < maxLength)
                buf.append((char)c);
            DecodeTo(buf.toString(), map, StringUtil.__UTF8);
        }

        /* -------------------------------------------------------------- */
        /** Decoded parameters to Map.
         * @param input the stream containing the encoded parameters
         */
        public static void DecodeTo(InputStream input, MultiMap<string> map, string charset, int maxLength)
        {
            if (charset == null || StringUtil.__ISO_8859_1.equals(charset))
            {
                Decode88591To(input, map, maxLength);
                return;
            }

            if (StringUtil.__UTF8.equalsIgnoreCase(charset))
            {
                DecodeUtf8To(input, map, maxLength);
                return;
            }

            if (StringUtil.__UTF16.equalsIgnoreCase(charset)) // Should be all 2 byte encodings
            {
                decodeUtf16To(input, map, maxLength);
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

                int size = 0;

                while ((c = input.read()) > 0)
                {
                    switch ((char)c)
                    {
                        case '&':
                            size = output.size();
                            value = size == 0 ? "" : output.toString(charset);
                            output.setCount(0);
                            if (key != null)
                            {
                                map.add(key, value);
                            }
                            else if (value != null && value.length() > 0)
                            {
                                map.add(value, "");
                            }
                            key = null;
                            value = null;
                            break;
                        case '=':
                            if (key != null)
                            {
                                output.write(c);
                                break;
                            }
                            size = output.size();
                            key = size == 0 ? "" : output.toString(charset);
                            output.setCount(0);
                            break;
                        case '+':
                            output.write(' ');
                            break;
                        case '%':
                            digits = 2;
                            break;
                        default:
                            if (digits == 2)
                            {
                                digit = TypeUtil.convertHexDigit((byte)c);
                                digits = 1;
                            }
                            else if (digits == 1)
                            {
                                output.write((digit << 4) + TypeUtil.convertHexDigit((byte)c));
                                digits = 0;
                            }
                            else
                                output.write(c);
                            break;
                    }

                    totalLength++;
                    if (maxLength >= 0 && totalLength > maxLength)
                        throw new IllegalStateException("Form too large");
                }

                size = output.size();
                if (key != null)
                {
                    value = size == 0 ? "" : output.toString(charset);
                    output.setCount(0);
                    map.add(key, value);
                }
                else if (size > 0)
                    map.add(output.toString(charset), "");
            }
        }

        /* -------------------------------------------------------------- */
        /** Decode string with % encoding.
         * This method makes the assumption that the majority of calls
         * will need no decoding.
         */
        public static string DecodeString(string encoded, int offset, int length, string charset)
        {
            if (charset == null || StringUtil.isUTF8(charset))
            {
                Utf8StringBuffer buffer = null;

                for (int i = 0; i < length; i++)
                {
                    char c = encoded.charAt(offset + i);
                    if (c < 0 || c > 0xff)
                    {
                        if (buffer == null)
                        {
                            buffer = new Utf8StringBuffer(length);
                            buffer.getStringBuffer().append(encoded, offset, offset + i + 1);
                        }
                        else
                            buffer.getStringBuffer().append(c);
                    }
                    else if (c == '+')
                    {
                        if (buffer == null)
                        {
                            buffer = new Utf8StringBuffer(length);
                            buffer.getStringBuffer().append(encoded, offset, offset + i);
                        }

                        buffer.getStringBuffer().append(' ');
                    }
                    else if (c == '%' && (i + 2) < length)
                    {
                        if (buffer == null)
                        {
                            buffer = new Utf8StringBuffer(length);
                            buffer.getStringBuffer().append(encoded, offset, offset + i);
                        }

                        while (c == '%' && (i + 2) < length)
                        {
                            byte b = (byte)TypeUtil.parseInt(encoded, offset + i + 1, 2, 16);
                            buffer.append(b);
                            i += 3;
                            if (i < length)
                                c = encoded.charAt(offset + i);
                        }
                        i--;
                    }
                    else if (buffer != null)
                        buffer.getStringBuffer().append(c);
                }

                if (buffer == null)
                {
                    if (offset == 0 && encoded.length() == length)
                        return encoded;
                    return encoded.substring(offset, offset + length);
                }

                return buffer.toString();
            }
            else
            {
                StringBuilder buffer = null;

                try
                {
                    for (int i = 0; i < length; i++)
                    {
                        char c = encoded.charAt(offset + i);
                        if (c < 0 || c > 0xff)
                        {
                            if (buffer == null)
                            {
                                buffer = new StringBuilder(length);
                                buffer.append(encoded, offset, offset + i + 1);
                            }
                            else
                                buffer.append(c);
                        }
                        else if (c == '+')
                        {
                            if (buffer == null)
                            {
                                buffer = new StringBuilder(length);
                                buffer.append(encoded, offset, offset + i);
                            }

                            buffer.append(' ');
                        }
                        else if (c == '%' && (i + 2) < length)
                        {
                            if (buffer == null)
                            {
                                buffer = new StringBuilder(length);
                                buffer.append(encoded, offset, offset + i);
                            }

                            byte[] ba = new byte[length];
                            int n = 0;
                            while (c >= 0 && c <= 0xff)
                            {
                                if (c == '%')
                                {
                                    ba[n++] = (byte)TypeUtil.parseInt(encoded, offset + i + 1, 2, 16);
                                    i += 3;
                                }
                                else
                                {
                                    ba[n++] = (byte)c;
                                    i++;
                                }

                                if (i >= length)
                                    break;
                                c = encoded.charAt(offset + i);
                            }

                            i--;
                            buffer.append(new string(ba, 0, n, charset));

                        }
                        else if (buffer != null)
                            buffer.append(c);
                    }

                    if (buffer == null)
                    {
                        if (offset == 0 && encoded.length() == length)
                            return encoded;
                        return encoded.substring(offset, offset + length);
                    }

                    return buffer.toString();
                }
                catch (UnsupportedEncodingException e)
                {
                    throw new RuntimeException(e);
                }
            }

        }

        /* ------------------------------------------------------------ */
        /** Perform URL encoding.
         * Assumes 8859 charset
         * @param string 
         * @return encoded string.
         */
        public static string EncodeString(string str)
        {
            return EncodeString(str, StringUtil.__UTF8);
        }

        /* ------------------------------------------------------------ */
        /** Perform URL encoding.
         * @param string 
         * @return encoded string.
         */
        public static string EncodeString(string str, string charset)
        {
            if (charset == null)
                charset = StringUtil.__UTF8;
            byte[] bytes = null;
            try
            {
                bytes = str.getBytes(charset);
            }
            catch (UnsupportedEncodingException e)
            {
                // Log.warn(LogSupport.EXCEPTION,e);
                bytes = str.getBytes();
            }

            int len = bytes.length;
            byte[] encoded = new byte[bytes.length * 3];
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
                return new string(encoded, 0, n, charset);
            }
            catch (UnsupportedEncodingException e)
            {
                // Log.warn(LogSupport.EXCEPTION,e);
                return new string(encoded, 0, n);
            }
        }


        /* ------------------------------------------------------------ */
        /** 
         */
        public object Clone()
        {
            return new UrlEncoded(this);
        }

    }
}

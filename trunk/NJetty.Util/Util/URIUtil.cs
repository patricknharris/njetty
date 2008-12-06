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
    /// URI Utility
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// November 2008
    /// </date>
    public static class URIUtil //: ICloneable
    {


        public const string SLASH = "/";
        public const string HTTP = "http";
        public const string HTTP_COLON = "http:";
        public const string HTTPS = "https";
        public const string HTTPS_COLON = "https:";

        // Use UTF-8 as per http://www.w3.org/TR/html40/appendix/notes.html#non-ascii-chars
        public static readonly Encoding __CHARSET = System.Text.Encoding.GetEncoding(StringUtil.__UTF8); //System.getProperty("org.mortbay.util.URI.charset",StringUtil.__UTF8);

        

        /// <summary>
        /// Encode a URI path.
        /// This is the same encoding offered by URLEncoder, except that
        /// the '/' character is not encoded.
        /// </summary>
        /// <param name="path">Path To Encode</param>
        /// <returns>Encoded Path</returns>
        public static string EncodePath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return path;

            StringBuilder buf = EncodePath(null, path);
            return buf == null ? path : buf.ToString();
        }

        /// <summary>
        /// Encode a URI path.
        /// </summary>
        /// <param name="buf">StringBuilder to encode path into (or null)</param>
        /// <param name="path">Path to Encode</param>
        /// <returns>StringBuilder or null if no substitutions required.</returns>
        public static StringBuilder EncodePath(StringBuilder buf, string path)
        {
            if (buf == null)
            {

                for (int i = 0; i < path.Length; i++)
                {
                    char c = path[i];
                    switch (c)
                    {
                        case '%':
                        case '?':
                        case ';':
                        case '#':
                        case ' ':
                            buf = new StringBuilder(path.Length << 1);
                            goto Loop;
                    }
                }
            Loop:
                if (buf == null)
                    return null;
            }

            lock (buf)
            {
                for (int i = 0; i < path.Length; i++)
                {
                    char c = path[i];
                    switch (c)
                    {
                        case '%':
                            buf.Append("%25");
                            continue;
                        case '?':
                            buf.Append("%3F");
                            continue;
                        case ';':
                            buf.Append("%3B");
                            continue;
                        case '#':
                            buf.Append("%23");
                            continue;
                        case ' ':
                            buf.Append("%20");
                            continue;
                        default:
                            buf.Append(c);
                            continue;
                    }
                }
            }

            return buf;
        }

        /// <summary>
        /// Encode a URI path.
        /// </summary>
        /// <param name="buf">StringBuilder to encode path into (or null)</param>
        /// <param name="path">Path to encode</param>
        /// <param name="encode">string of characters to encode. % is always encoded.</param>
        /// <returns>StringBuilder or null if no substitutions required.</returns>
        public static StringBuilder EncodeString(StringBuilder buf,
                                                 string path,
                                                 string encode)
        {
            if (buf == null)
            {

                for (int i = 0; i < path.Length; i++)
                {
                    char c = path[i];
                    if (c == '%' || encode.IndexOf(c) >= 0)
                    {
                        buf = new StringBuilder(path.Length << 1);
                        goto Loop;
                    }
                }

            Loop:
                if (buf == null)
                    return null;
            }

            lock (buf)
            {
                for (int i = 0; i < path.Length; i++)
                {
                    char c = path[i];
                    if (c == '%' || encode.IndexOf(c) >= 0)
                    {
                        buf.Append('%');
                        StringUtil.Append(buf, (byte)(0xff & c), 16);
                    }
                    else
                        buf.Append(c);
                }
            }

            return buf;
        }

        /// <summary>
        /// Decode a URI path. 
        /// </summary>
        /// <param name="path">The Path to decode</param>
        /// <returns>StringBuilder to encode path into</returns>
        public static string DecodePath(string path)
        {
            byte[] b;

            try
            {
                b = __CHARSET.GetBytes(path);
            }
            catch (ArgumentException)
            {

                b = Encoding.ASCII.GetBytes(path);
            }
            return DecodePath(b, 0, b.Length);

        }

       
        /// <summary>
        /// Decode a URI path.
        /// </summary>
        /// <param name="buf">StringBuilder to encode path into</param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string DecodePath(byte[] buf, int offset, int length)
        {
            byte[] bytes = null;
            int n = 0;

            for (int i = 0; i < length; i++)
            {
                byte b = buf[i + offset];

                if (b == '%' && (i + 2) < length)
                {
                    b = (byte)(0xff & TypeUtil.ParseInt(buf, i + offset + 1, 2, 16));
                    i += 2;
                }
                else if (bytes == null)
                {
                    n++;
                    continue;
                }

                if (bytes == null)
                {
                    bytes = new byte[length];
                    for (int j = 0; j < n; j++)
                        bytes[j] = buf[j + offset];
                }

                bytes[n++] = b;
            }

            if (bytes == null)
                return StringUtil.ToString(buf, offset, length, __CHARSET);
            return StringUtil.ToString(bytes, 0, n, __CHARSET);
        }

        /// <summary>
        /// Add two URI path segments.
        /// Handles null and empty paths, path and query params (eg ?a=b or
        /// ;JSESSIONID=xxx) and avoids duplicate '/'
        /// </summary>
        /// <param name="p1">URI path segment</param>
        /// <param name="p2">URI path segment</param>
        /// <returns>Legally combined path segments.</returns>
        public static string AddPaths(string p1, string p2)
        {
            if (p1 == null || p1.Length == 0)
            {
                if (p1 != null && p2 == null)
                    return p1;
                return p2;
            }
            if (p2 == null || p2.Length == 0)
                return p1;

            int split = p1.IndexOf(';');
            if (split < 0)
                split = p1.IndexOf('?');
            if (split == 0)
                return p2 + p1;
            if (split < 0)
                split = p1.Length;

            StringBuilder buf = new StringBuilder(p1.Length + p2.Length + 2);
            buf.Append(p1);

            if (buf[split - 1] == '/')
            {
                if (p2.StartsWith(URIUtil.SLASH))
                {
                    buf.Remove(split - 1, 1);
                    buf.Insert(split - 1, p2);
                }
                else
                    buf.Insert(split, p2);
            }
            else
            {
                if (p2.StartsWith(URIUtil.SLASH))
                    buf.Insert(split, p2);
                else
                {
                    buf.Insert(split, '/');
                    buf.Insert(split + 1, p2);
                }
            }



            return buf.ToString();
        }

        
        /// <summary>
        /// Return the parent Path.
        /// Treat a URI like a directory path and return the parent directory.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static string ParentPath(string p)
        {
            if (p == null || URIUtil.SLASH.Equals(p))
                return null;
            int slash = p.LastIndexOf('/', p.Length - 2);
            if (slash >= 0)
                return p.Substring(0, slash + 1);
            return null;
        }

        
        /// <summary>
        /// Strip parameters from a path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns>Return path upto any semicolon parameters.</returns>
        public static string StripPath(string path)
        {
            if (path == null)
                return null;
            int semi = path.IndexOf(';');
            if (semi < 0)
                return path;
            return path.Substring(0, semi);
        }

        
        /// <summary>
        /// Convert a path to a cananonical form.
        /// All instances of "." and ".." are factored out.  Null is returned
        /// if the path tries to .. above its root.
        /// </summary>
        /// <param name="path">Path</param>
        /// <returns>path or null.</returns>
        public static string CanonicalPath(string path)
        {

            
            if (path == null || path.Length == 0)
                return path;

            int end = path.Length;
            int queryIdx = path.IndexOf('?');
            int start = path.LastIndexOf('/', (queryIdx > 0 ? queryIdx : end-1));


            while (end > 0)
            {
                switch (end - start)
                {
                    case 2: // possible single dot
                        if (path[start + 1] != '.')
                            break;
                        goto Search;
                    case 3: // possible double dot
                        if (path[start + 1] != '.' || path[start + 2] != '.')
                            break;
                        goto Search;
                }

                end = start;

                try
                {
                    start = path.LastIndexOf('/', end - 1);
                }
                catch { }
            }
        Search:

            // If we have checked the entire string
            if (start >= end)
                return path;

            StringBuilder buf = new StringBuilder(path);
            int delStart = -1;
            int delEnd = -1;
            int skip = 0;

            while (end > 0)
            {
                switch (end - start)
                {
                    case 2: // possible single dot
                        if (buf[start + 1] != '.')
                        {
                            if (skip > 0 && --skip == 0)
                            {
                                delStart = start >= 0 ? start : 0;
                                if (delStart > 0 && delEnd == buf.Length && buf[delEnd - 1] == '.')
                                    delStart++;
                            }
                            break;
                        }

                        if (start < 0 && buf.Length > 2 && buf[1] == '/' && buf[2] == '/')
                            break;

                        if (delEnd < 0)
                            delEnd = end;
                        delStart = start;
                        if (delStart < 0 || delStart == 0 && buf[delStart] == '/')
                        {
                            delStart++;
                            if (delEnd < buf.Length && buf[delEnd] == '/')
                                delEnd++;
                            break;
                        }
                        if (end == buf.Length)
                            delStart++;

                        end = start--;
                        while (start >= 0 && buf[start] != '/')
                            start--;
                        continue;

                    case 3: // possible double dot
                        if (buf[start + 1] != '.' || buf[start + 2] != '.')
                        {
                            if (skip > 0 && --skip == 0)
                            {
                                delStart = start >= 0 ? start : 0;
                                if (delStart > 0 && delEnd == buf.Length && buf[delEnd - 1] == '.')
                                    delStart++;
                            }
                            break;
                        }

                        delStart = start;
                        if (delEnd < 0)
                            delEnd = end;

                        skip++;
                        end = start--;
                        while (start >= 0 && buf[start] != '/')
                            start--;
                        continue;

                    default:
                        if (skip > 0 && --skip == 0)
                        {
                            delStart = start >= 0 ? start : 0;
                            if (delEnd == buf.Length && buf[delEnd - 1] == '.')
                                delStart++;
                        }
                        break;
                }

                // Do the delete
                if (skip <= 0 && delStart >= 0 && delStart >= 0)
                {
                    //buf.delete(delStart,delEnd);
                    buf.Remove(delStart, delEnd - delStart);
                    delStart = delEnd = -1;
                    if (skip > 0)
                        delEnd = end;
                }

                end = start--;
                while (start >= 0 && buf[start] != '/')
                    start--;
            }

            // Too many ..
            if (skip > 0)
                return null;

            // Do the delete
            if (delEnd >= 0)
            {
                //buf.delete(delStart, delEnd);
                buf.Remove(delStart, delEnd - delStart);
            }

            return buf.ToString();
        }

        /// <summary>
        /// Convert a path to a compact form.
        /// All instances of "//" and "///" etc. are factored out to single "/" 
        /// </summary>
        /// <param name="path">path to compact</param>
        /// <returns>compacted path</returns>
        public static string CompactPath(string path)
        {
            if (path == null || path.Length == 0)
                return path;

            int state = 0;
            int end = path.Length;
            int i = 0;


            while (i < end)
            {
                char c = path[i];
                switch (c)
                {
                    case '?':
                        return path;
                    case '/':
                        state++;
                        if (state == 2)
                        {
                            goto WhileLoop;
                        }
                        break;
                    default:
                        state = 0;
                        break;
                }
                i++;
            }
        WhileLoop:

            if (state < 2)
                return path;

            StringBuilder buf = new StringBuilder(path.Length);
            buf.Append(path, 0, i);


            while (i < end)
            {
                char c = path[i];
                switch (c)
                {
                    case '?':
                        buf.Append(path, i, end-i);
                        goto Loop2;
                    case '/':
                        if (state++ == 0)
                            buf.Append(c);
                        break;
                    default:
                        state = 0;
                        buf.Append(c);
                        break;
                }
                i++;
            }

        Loop2:

            return buf.ToString();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri">uri URI</param>
        /// <returns>True if the uri has a scheme</returns>
        public static bool HasScheme(string uri)
        {
            for (int i = 0; i < uri.Length; i++)
            {
                char c = uri[i];
                if (c == ':')
                    return true;
                if (!(c >= 'a' && c <= 'z' ||
                      c >= 'A' && c <= 'Z' ||
                      (i > 0 && (c >= '0' && c <= '9' ||
                              c == '.' ||
                              c == '+' ||
                              c == '-'))
                      ))
                    break;
            }
            return false;
        }

    }
}

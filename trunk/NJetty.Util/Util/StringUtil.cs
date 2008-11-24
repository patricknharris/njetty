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
using System.Configuration;
using NJetty.Util.Logging;

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
    public class StringUtil
    {
        public const string ALL_INTERFACES="0.0.0.0";
        public const string CRLF="\015\012";
        public const string __LINE_SEPARATOR= "\n"; // TODO: System.getProperty("line.separator","\n");
           
        public static readonly string __ISO_8859_1;
        static StringUtil()
        {
            string iso=ConfigurationManager.AppSettings["ISO_8859_1"];
            if (iso==null)
            {
                try{

                    Encoding latinEuropeanEncoding = Encoding.GetEncoding("iso-8859-1"); 
                    latinEuropeanEncoding.GetString(new byte[]{(byte)20});
                    iso="ISO-8859-1";
                }
                catch(Exception e)
                {
                    iso="ISO8859_1";
                }        
            }
            __ISO_8859_1=iso;
        }
        
        
        public const string __UTF8="UTF-8";
        public const string __UTF8Alt="UTF8";
        public const string __UTF16="UTF-16";
        
        
        private static char[] lowercases = new char[]{
        
            (char)0, 
            (char)1, (char)2, (char)3, (char)4, (char)5, (char)6, (char)7, (char)8, (char)9, (char)10, 
            (char)11, (char)12, (char)13, (char)14, (char)15, (char)16, (char)17, (char)18, (char)19, (char)20, 
            (char)21, (char)22, (char)23, (char)24, (char)25, (char)26, (char)27, (char)28, (char)29, (char)30, 
            (char)31, (char)32, (char)33, (char)34, (char)35, (char)36, (char)37, (char)38, (char)39, (char)40, 
            (char)41, (char)42, (char)43, (char)44, (char)45, (char)46, (char)47, (char)48, (char)49, (char)50, 
            (char)51, (char)52, (char)53, (char)54, (char)55, (char)56, (char)57, (char)58, (char)59, (char)60, 
            (char)61, (char)62, (char)63, (char)64, (char)97, (char)98, (char)99, (char)100, 
            (char)101, (char)102, (char)103, (char)104, (char)105, (char)106, (char)107, (char)108, (char)109, (char)110, 
            (char)111, (char)112, (char)113, (char)114, (char)115, (char)116, (char)117, (char)118, (char)119, (char)120, 
            (char)121, (char)122, (char)91, (char)92, (char)93, (char)94, (char)95, (char)96, (char)97, (char)98, (char)99, (char)100, 
            (char)101, (char)102, (char)103, (char)104, (char)105, (char)106, (char)107, (char)108, (char)109, (char)110, 
            (char)111, (char)112, (char)113, (char)114, (char)115, (char)116, (char)117, (char)118, (char)119, (char)120, 
            (char)121, (char)122, (char)123, (char)124, (char)125, (char)126, (char)127
        
        
        };

        /// <summary>
        /// fast lower case conversion. Only works on ascii (not unicode)
        /// </summary>
        /// <param name="s">string to convert</param>
        /// <returns>lower case version of s</returns>
        public static string AsciiToLowerCase(string s)
        {
            char[] c = null;
            int i=s.Length;

            // look for first conversion
            while (i-->0)
            {
                char c1=s[i];
                if (c1<=127)
                {
                    char c2=lowercases[c1];
                    if (c1!=c2)
                    {
                        c=s.ToCharArray();
                        c[i]=c2;
                        break;
                    }
                }
            }

            while (i-->0)
            {
                if(c[i]<=127)
                    c[i] = lowercases[c[i]];
            }
            
            return c==null?s:new string(c);
        }


        public static bool StartsWithIgnoreCase(string s,string w)
        {
            if (w==null)
                return true;
            
            if (s==null || s.Length<w.Length)
                return false;
            
            for (int i=0;i<w.Length;i++)
            {
                char c1=s[i];
                char c2=w[i];
                if (c1!=c2)
                {
                    if (c1<=127)
                        c1=lowercases[c1];
                    if (c2<=127)
                        c2=lowercases[c2];
                    if (c1!=c2)
                        return false;
                }
            }
            return true;
        }
        
        public static bool EndsWithIgnoreCase(string s,string w)
        {
            if (w==null)
                return true;

            if (s==null)
                return false;
                
            int sl=s.Length;
            int wl=w.Length;
            
            if (sl<wl)
                return false;
            
            for (int i=wl;i-->0;)
            {
                char c1=s[--sl];
                char c2=w[i];
                if (c1!=c2)
                {
                    if (c1<=127)
                        c1=lowercases[c1];
                    if (c2<=127)
                        c2=lowercases[c2];
                    if (c1!=c2)
                        return false;
                }
            }
            return true;
        }
        
        /// <summary>
        /// returns the next index of a character from the chars string
        /// </summary>
        /// <param name="s"></param>
        /// <param name="chars"></param>
        /// <returns></returns>
        public static int IndexFrom(string s,string chars)
        {
            for (int i=0;i<s.Length;i++)
               if (chars.IndexOf(s[i])>=0)
                  return i;
            return -1;
        }
        
        /// <summary>
        /// replace substrings within string.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="sub"></param>
        /// <param name="with"></param>
        /// <returns></returns>
        public static string Replace(string s, string sub, string with)
        {
            int c=0;
            int i=s.IndexOf(sub,c);
            if (i == -1)
                return s;
        
            StringBuilder buf = new StringBuilder(s.Length+with.Length);

            do
            {
                buf.Append(s.Substring(c,i));
                buf.Append(with);
                c=i+sub.Length;
            } while ((i=s.IndexOf(sub,c))!=-1);

            if (c<s.Length)
                buf.Append(s.Substring(c,s.Length));

            return buf.ToString();
            
        }


        /// <summary>
        /// Remove single or double quotes.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string Unquote(string s)
        {
            return QuotedStringTokenizer.Unquote(s);
        }

        /// <summary>
        /// Append substring to StringBuilder
        /// </summary>
        /// <param name="buf">StringBuilder to append to</param>
        /// <param name="s">string to append from</param>
        /// <param name="offset">The offset of the substring</param>
        /// <param name="length">The length of the substring</param>
        public static void Append(StringBuilder buf,
                                  string s,
                                  int offset,
                                  int length)
        {
            lock(buf)
            {
                int end=offset+length;
                for (int i=offset; i<end;i++)
                {
                    if (i>=s.Length)
                        break;
                    buf.Append(s[i]);
                }
            }
        }

        /// <summary>
        /// append hex digit 
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="b"></param>
        /// <param name="radix"></param>
        public static void Append(StringBuilder buf,byte b,int radix)
        {
            int bi=0xff&b;
            int c = '0' + (bi / radix) % radix;
            if (c>'9')
                c= 'a'+(c-'0'-10);
            buf.Append((char)c);
            c = '0' + bi % radix;
            if (c>'9')
                c= 'a'+(c-'0'-10);
            buf.Append((char)c);
        }

        public static void Append2Digits(StringBuilder buf,int i)
        {
            if (i<100)
            {
                buf.Append((char)(i/10+'0'));
                buf.Append((char)(i%10+'0'));
            }
        }

        /// <summary>
        /// Return a non null string.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string NonNull(string s)
        {
            if (s==null)
                return String.Empty;
            return s;
        }
        
        public static bool Equals(string s,char[] buf, int offset, int length)
        {
            if (s.Length!=length)
                return false;
            for (int i=0;i<length;i++)
                if (buf[offset+i]!=s[i])
                    return false;
            return true;
        }

        #region ToString Byte Encoding Converter

        public static string ToUTF8String(byte[] b, int offset, int length)
        {
            try
            {
                return Encoding.UTF8.GetString(b, offset, length);
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message, e);
            }
        }


        public static string ToString(byte[] b,int offset,int length,Encoding charset)
        {
            try
            {
                return charset.GetString(b, offset, length);
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message, e);
            }
        }

        public static string ToString(byte[] b, int offset, int length, string charset)
        {
            return ToString(b, offset, length, Encoding.GetEncoding(charset));
        }
        #endregion


        public static bool IsUTF8(string charset)
        {
            return charset == __UTF8 || __UTF8.Equals(charset, StringComparison.OrdinalIgnoreCase) || __UTF8Alt.Equals(charset, StringComparison.OrdinalIgnoreCase);
        }


        public static string Printable(string name)
        {
            if (name==null)
                return null;
            StringBuilder buf = new StringBuilder(name.Length);
            for (int i=0;i<name.Length;i++)
            {
                char c=name[i];
                if (!IsISOControl(c))
                {
                    buf.Append(c);
                }
            }
            return buf.ToString();
        }
        
        public static byte[] GetBytes(string s)
        {
            try
            {

                return Encoding.GetEncoding(__ISO_8859_1).GetBytes(s);
            }
            catch(Exception e)
            {
                Log.Warn(e);
                return Encoding.ASCII.GetBytes(s);
            }
        }
        
        public static byte[] GetBytes(string s,string charset)
        {
            try
            {
                return Encoding.GetEncoding(charset).GetBytes(s);
            }
            catch(Exception e)
            {
                Log.Warn(e);
                return Encoding.ASCII.GetBytes(s);
            }
        }


        public static bool IsISOControl(char ch)
        {
            return IsISOControl((int)ch);
        }

        public static bool IsISOControl(int codePoint)
        {
            return (codePoint >= 0x0000 && codePoint <= 0x001F) ||
                (codePoint >= 0x007F && codePoint <= 0x009F);
        }
    }
}

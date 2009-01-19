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
using System.Reflection;

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
    public class TypeUtil
    {

        public static int CR = 13;
        public static int LF = 10;

        /* ------------------------------------------------------------ */
        static readonly Dictionary<string, Type> name2Class = new Dictionary<string, Type>();
        static TypeUtil()
        {
            name2Class.Add("bool", typeof(bool));
            name2Class.Add("bool", typeof(bool));
            name2Class.Add("byte", typeof(byte));
            name2Class.Add("char", typeof(char));
            name2Class.Add("double", typeof(double));
            name2Class.Add("float", typeof(float));
            name2Class.Add("int", typeof(int));
            name2Class.Add("long", typeof(long));
            name2Class.Add("short", typeof(short));
            name2Class.Add("void", typeof(void));



            //name2Class.put("java.lang.Boolean.TYPE",java.lang.Boolean.TYPE);
            //name2Class.put("java.lang.Byte.TYPE",java.lang.Byte.TYPE);
            //name2Class.put("java.lang.Character.TYPE",java.lang.Character.TYPE);
            //name2Class.put("java.lang.Double.TYPE",java.lang.Double.TYPE);
            //name2Class.put("java.lang.Float.TYPE",java.lang.Float.TYPE);
            //name2Class.put("java.lang.Integer.TYPE",java.lang.Integer.TYPE);
            //name2Class.put("java.lang.Long.TYPE",java.lang.Long.TYPE);
            //name2Class.put("java.lang.Short.TYPE",java.lang.Short.TYPE);
            //name2Class.put("java.lang.Void.TYPE",java.lang.Void.TYPE);

            //name2Class.put("java.lang.Boolean",java.lang.Boolean.class);
            //name2Class.put("java.lang.Byte",java.lang.Byte.class);
            //name2Class.put("java.lang.Character",java.lang.Character.class);
            //name2Class.put("java.lang.Double",java.lang.Double.class);
            //name2Class.put("java.lang.Float",java.lang.Float.class);
            //name2Class.put("java.lang.Integer",java.lang.Integer.class);
            //name2Class.put("java.lang.Long",java.lang.Long.class);
            //name2Class.put("java.lang.Short",java.lang.Short.class);

            //name2Class.put("Boolean",java.lang.Boolean.class);
            //name2Class.put("Byte",java.lang.Byte.class);
            //name2Class.put("Character",java.lang.Character.class);
            //name2Class.put("Double",java.lang.Double.class);
            //name2Class.put("Float",java.lang.Float.class);
            //name2Class.put("Integer",java.lang.Integer.class);
            //name2Class.put("Long",java.lang.Long.class);
            //name2Class.put("Short",java.lang.Short.class);

            //name2Class.put(null,java.lang.Void.TYPE);
            //name2Class.put("string",java.lang.string.class);
            //name2Class.put("string",java.lang.string.class);
            //name2Class.put("java.lang.string",java.lang.string.class);



            /*---------------------------------------------------------------------*/







        }

        /* ------------------------------------------------------------ */
        //private static final HashMap class2Name=new HashMap();
        //static
        //{
        //    class2Name.put(java.lang.Boolean.TYPE,"bool");
        //    class2Name.put(java.lang.Byte.TYPE,"byte");
        //    class2Name.put(java.lang.Character.TYPE,"char");
        //    class2Name.put(java.lang.Double.TYPE,"double");
        //    class2Name.put(java.lang.Float.TYPE,"float");
        //    class2Name.put(java.lang.Integer.TYPE,"int");
        //    class2Name.put(java.lang.Long.TYPE,"long");
        //    class2Name.put(java.lang.Short.TYPE,"short");
        //    class2Name.put(java.lang.Void.TYPE,"void");

        //    class2Name.put(java.lang.Boolean.class,"java.lang.Boolean");
        //    class2Name.put(java.lang.Byte.class,"java.lang.Byte");
        //    class2Name.put(java.lang.Character.class,"java.lang.Character");
        //    class2Name.put(java.lang.Double.class,"java.lang.Double");
        //    class2Name.put(java.lang.Float.class,"java.lang.Float");
        //    class2Name.put(java.lang.Integer.class,"java.lang.Integer");
        //    class2Name.put(java.lang.Long.class,"java.lang.Long");
        //    class2Name.put(java.lang.Short.class,"java.lang.Short");

        //    class2Name.put(null,"void");
        //    class2Name.put(java.lang.string.class,"java.lang.string");
        //}

        ///* ------------------------------------------------------------ */
        //private static final HashMap class2Value=new HashMap();
        //static
        //{
        //    try
        //    {
        //        Class[] s ={java.lang.string.class};

        //        class2Value.put(java.lang.Boolean.TYPE,
        //                       java.lang.Boolean.class.getMethod("valueOf",s));
        //        class2Value.put(java.lang.Byte.TYPE,
        //                       java.lang.Byte.class.getMethod("valueOf",s));
        //        class2Value.put(java.lang.Double.TYPE,
        //                       java.lang.Double.class.getMethod("valueOf",s));
        //        class2Value.put(java.lang.Float.TYPE,
        //                       java.lang.Float.class.getMethod("valueOf",s));
        //        class2Value.put(java.lang.Integer.TYPE,
        //                       java.lang.Integer.class.getMethod("valueOf",s));
        //        class2Value.put(java.lang.Long.TYPE,
        //                       java.lang.Long.class.getMethod("valueOf",s));
        //        class2Value.put(java.lang.Short.TYPE,
        //                       java.lang.Short.class.getMethod("valueOf",s));

        //        class2Value.put(java.lang.Boolean.class,
        //                       java.lang.Boolean.class.getMethod("valueOf",s));
        //        class2Value.put(java.lang.Byte.class,
        //                       java.lang.Byte.class.getMethod("valueOf",s));
        //        class2Value.put(java.lang.Double.class,
        //                       java.lang.Double.class.getMethod("valueOf",s));
        //        class2Value.put(java.lang.Float.class,
        //                       java.lang.Float.class.getMethod("valueOf",s));
        //        class2Value.put(java.lang.Integer.class,
        //                       java.lang.Integer.class.getMethod("valueOf",s));
        //        class2Value.put(java.lang.Long.class,
        //                       java.lang.Long.class.getMethod("valueOf",s));
        //        class2Value.put(java.lang.Short.class,
        //                       java.lang.Short.class.getMethod("valueOf",s));
        //    }
        //    catch(Exception e)
        //    {
        //        e.printStackTrace();
        //    }
        //}

        private static Type[] stringArg = new Type[] { typeof(string) };


        /** Class from a canonical name for a type.
         * @param name A class or type name.
         * @return A class , which may be a primitive TYPE field..
         */
        public static Type FromName(string name)
        {
            return Type.GetType(name);
        }

        /* ------------------------------------------------------------ */
        /** Canonical name for a type.
         * @param type A class , which may be a primitive TYPE field.
         * @return Canonical name.
         */
        public static string ToName(Type type)
        {
            return (string)type.ToString();
        }

        /* ------------------------------------------------------------ */
        /** Convert string value to instance.
         * @param type The class of the instance, which may be a primitive TYPE field.
         * @param value The value as a string.
         * @return The value as an object.
         */
        public static object ValueOf(Type type, string value)
        {

            try
            {
                if (type == typeof(string))
                {
                    return value;
                }


                if (type == typeof(char))
                {
                    return value[0];
                }


                MethodInfo m = type.GetMethod("Parse", stringArg);
                if (m != null)
                {
                    return m.Invoke(null, new object[] { value });
                }

                return Activator.CreateInstance(type, new object[] { value });
            }
            catch (TargetException e)
            {
                if (e.InnerException is SystemException)
                {
                    throw e.InnerException;
                }
            }
            catch { }

            return null;
        }

        /* ------------------------------------------------------------ */
        /** Convert string value to instance.
         * @param type classname or type (eg int)
         * @param value The value as a string.
         * @return The value as an object.
         */
        public static object ValueOf(string type, string value)
        {
            return ValueOf(FromName(type), value);
        }

        [Obsolete("use intvalue.ToString instead ...")]
        public static string ToString(int i)
        {
            return i.ToString();
        }
        [Obsolete("use longvalue.ToString() instead ...")]
        public static string ToString(long i)
        {
            return i.ToString();
        }


        /* ------------------------------------------------------------ */
        /** Parse an int from a substring.
         * Negative numbers are not handled.
         * @param s string
         * @param offset Offset within string
         * @param length Length of integer or -1 for remainder of string
         * @param radix radix of the integer
         * @exception NumberFormatException 
         */
        public static int ParseInt(string s, int offset, int length, int radix)
        {
            int value = 0;

            if (length < 0)
                length = s.Length - offset;

            for (int i = 0; i < length; i++)
            {
                char c = s[offset + i];

                int digit = c - '0';
                if (digit < 0 || digit >= radix || digit >= 10)
                {
                    digit = 10 + c - 'A';
                    if (digit < 10 || digit >= radix)
                        digit = 10 + c - 'a';
                }
                if (digit < 0 || digit >= radix)
                {
                    throw new FormatException(s.Substring(offset, length));
                }

                value = value * radix + digit;
            }
            return value;
        }

        /* ------------------------------------------------------------ */
        /** Parse an int from a byte array of ascii characters.
         * Negative numbers are not handled.
         * @param b byte array
         * @param offset Offset within string
         * @param length Length of integer or -1 for remainder of string
         * @param radix radix of the integer
         * @exception NumberFormatException 
         */
        public static int ParseInt(byte[] b, int offset, int length, int radix)
        {
            int value = 0;

            if (length < 0)
                length = b.Length - offset;

            for (int i = 0; i < length; i++)
            {
                char c = (char)(0xff & b[offset + i]);

                int digit = c - '0';
                if (digit < 0 || digit >= radix || digit >= 10)
                {
                    digit = 10 + c - 'A';
                    if (digit < 10 || digit >= radix)
                        digit = 10 + c - 'a';
                }
                if (digit < 0 || digit >= radix)
                {
                    throw new FormatException(Encoding.ASCII.GetString(b, offset, length));
                }
                value = value * radix + digit;
            }
            return value;
        }

        /* ------------------------------------------------------------ */
        public static byte[] ParseBytes(string s, int radix)
        {
            byte[] bytes = new byte[s.Length / 2];
            for (int i = 0; i < s.Length; i += 2)
                bytes[i / 2] = (byte)TypeUtil.ParseInt(s, i, 2, radix);
            return bytes;
        }

        /* ------------------------------------------------------------ */
        public static string ToString(byte[] bytes, int radix)
        {
            StringBuilder buf = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                int bi = 0xff & bytes[i];
                int c = '0' + (bi / radix) % radix;
                if (c > '9')
                    c = 'a' + (c - '0' - 10);
                buf.Append((char)c);
                c = '0' + bi % radix;
                if (c > '9')
                    c = 'a' + (c - '0' - 10);
                buf.Append((char)c);
            }
            return buf.ToString();
        }

        /* ------------------------------------------------------------ */
        /** 
         * @param b An ASCII encoded character 0-9 a-f A-F
         * @return The byte value of the character 0-16.
         */
        public static byte ConvertHexDigit(byte b)
        {
            if ((b >= '0') && (b <= '9')) return (byte)(b - '0');
            if ((b >= 'a') && (b <= 'f')) return (byte)(b - 'a' + 10);
            if ((b >= 'A') && (b <= 'F')) return (byte)(b - 'A' + 10);
            return 0;
        }

        /* ------------------------------------------------------------ */
        public static string ToHexString(byte[] b)
        {
            StringBuilder buf = new StringBuilder();
            for (int i = 0; i < b.Length; i++)
            {
                int bi = 0xff & b[i];
                int c = '0' + (bi / 16) % 16;
                if (c > '9')
                    c = 'A' + (c - '0' - 10);
                buf.Append((char)c);
                c = '0' + bi % 16;
                if (c > '9')
                    c = 'a' + (c - '0' - 10);
                buf.Append((char)c);
            }
            return buf.ToString();
        }

        /* ------------------------------------------------------------ */
        public static string ToHexString(byte[] b, int offset, int length)
        {
            StringBuilder buf = new StringBuilder();
            for (int i = offset; i < offset + length; i++)
            {
                int bi = 0xff & b[i];
                int c = '0' + (bi / 16) % 16;
                if (c > '9')
                    c = 'A' + (c - '0' - 10);
                buf.Append((char)c);
                c = '0' + bi % 16;
                if (c > '9')
                    c = 'a' + (c - '0' - 10);
                buf.Append((char)c);
            }
            return buf.ToString();
        }

        /* ------------------------------------------------------------ */
        public static byte[] FromHexString(string s)
        {
            if (s.Length % 2 != 0)
                throw new ArgumentException(s);
            byte[] array = new byte[s.Length / 2];
            for (int i = 0; i < array.Length; i++)
            {
                int b = int.Parse(s.Substring(i * 2, i * 2 + 2), System.Globalization.NumberStyles.HexNumber);
                array[i] = (byte)(0xff & b);
            }
            return array;
        }


        //public static void Dump(Type c)
        //{
        //    Console.Error.WriteLine("Dump: "+c);
        //    Dump(c.Assembly);
        //}

        //public static void Dump(Assembly asm)
        //{
        //    Console.Error.WriteLine("Dump Assemblies:");
        //    Console.Error.WriteLine("  Assmbly Loader " + cl.FullName);
        //    // See java implemenatation
        //}


        /* ------------------------------------------------------------ */
        //public static byte[] readLine(InputStream in) throws IOException
        //{
        //    byte[] buf = new byte[256];

        //    int i=0;
        //    int loops=0;
        //    int ch=0;

        //    while (true)
        //    {
        //        ch=in.read();
        //        if (ch<0)
        //            break;
        //        loops++;

        //        // skip a leading LF's
        //        if (loops==1 && ch==LF)
        //            continue;

        //        if (ch==CR || ch==LF)
        //            break;

        //        if (i>=buf.length)
        //        {
        //            byte[] old_buf=buf;
        //            buf=new byte[old_buf.length+256];
        //            System.arraycopy(old_buf, 0, buf, 0, old_buf.length);
        //        }
        //        buf[i++]=(byte)ch;
        //    }

        //    if (ch==-1 && i==0)
        //        return null;

        //    // skip a trailing LF if it exists
        //    if (ch==CR && in.available()>=1 && in.markSupported())
        //    {
        //        in.mark(1);
        //        ch=in.read();
        //        if (ch!=LF)
        //            in.reset();
        //    }

        //    byte[] old_buf=buf;
        //    buf=new byte[i];
        //    System.arraycopy(old_buf, 0, buf, 0, i);

        //    return buf;
        //}

        //public static System.Uri JarFor(string className)
        //{
        //    try
        //    {
        //        className=className.replace('.','/')+".class";
        //        // hack to discover jstl libraries
        //        URL url = Loader.getResource(null,className,false);
        //        string s=url.toString();
        //        if (s.startsWith("jar:file:"))
        //            return new URL(s.substring(4,s.indexOf("!/")));
        //    }
        //    catch(Exception e)
        //    {
        //        Log.ignore(e);
        //    }
        //    return null;
        //}
    }
}

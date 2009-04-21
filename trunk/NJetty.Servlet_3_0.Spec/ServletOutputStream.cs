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
using System.IO;

namespace Javax.NServlet
{

    /// <summary>
    /// Provides an output stream for sending binary data to the
    /// client. A <code>ServletOutputStream</code> object is normally retrieved 
    /// via the {@link ServletResponse#getOutputStream} method.
    ///
    /// <p>This is an abstract class that the servlet container implements.
    /// Subclasses of this class
    /// must implement the <code>java.io.OutputStream.write(int)</code>
    /// method.
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// April 21, 2009
    /// </date>
    public abstract class ServletOutputStream : TextWriter
    {
        public override Encoding Encoding
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Writes a <code>string</code> to the client,
        /// without a carriage return-line feed (CRLF) 
        /// character at the end.
        /// </summary>
        /// <param name="s">the string to send to the client</param>
        /// <exception cref="IOException">if an input or output exception occurred</exception>
        public void Print(string s)
        {
            if (s == null) s = "null";
            int len = s.Length;
            for (int i = 0; i < len; i++)
            {
                char c = s[i];

                //
                // XXX NOTE:  This is clearly incorrect for many strings,
                // but is the only consistent approach within the current
                // servlet framework.  It must suffice until servlet output
                // streams properly encode their output.
                //
                if ((c & 0xff00) != 0)
                {	// high order byte must be zero
                    //TODO: in java: string errMsg = lStrings.getString("err.not_iso8859_1");
                    string errMsg = "Not an ISO 8859-1 character: {0}";
                    object[] errArgs = new object[1];
                    errArgs[0] = c;
                    errMsg = string.Format(errMsg, errArgs);
                    //TODO: in java:  throw new CharConversionException(errMsg);
                    throw new System.IO.IOException(errMsg);
                }
                Write(c);
            }
        }



        /// <summary>
        /// Writes a <code>bool</code> value to the client,
        /// with no carriage return-line feed (CRLF) 
        /// character at the end.
        /// </summary>
        /// <param name="b">the bool value to send to the client</param>
        /// <exception cref="IOException">if an input or output exception occurred</exception>
        public void Print(bool b)
        {
            string msg;
            if (b)
            {
                //TODO: in java: msg = lStrings.getString("value.true");
                msg = "true";
            }
            else
            {
                //TODO: in java: msg = lStrings.getString("value.false");
                msg = "false";
            }
            Print(msg);
        }



        /// <summary>
        /// Writes a character to the client,
        /// with no carriage return-line feed (CRLF) 
        /// at the end.
        /// </summary>
        /// <param name="c">the character to send to the client</param>
        /// <exception cref="IOException">if an input or output exception occurred</exception>
        public void Print(char c)
        {
            Print(c.ToString());
        }
        

        /// <summary>
        /// Writes an int to the client,
        /// with no carriage return-line feed (CRLF) 
        /// at the end.
        /// </summary>
        /// <param name="i">the int to send to the client</param>
        /// <exception cref="IOException">if an input or output exception occurred</exception>
        public void Print(int i)
        {
            Print(i.ToString());
        }


        /// <summary>
        /// Writes a long value to the client,
        /// with no carriage return-line feed (CRLF) at the end.
        /// </summary>
        /// <param name="l">the long value to send to the client</param>
        /// <exception cref="IOException">if an input or output exception occurred</exception>
        public void Print(long l)
        {
            Print(l.ToString());
        }


        /// <summary>
        /// Writes a <code>float</code> value to the client,
        /// with no carriage return-line feed (CRLF) at the end.
        /// </summary>
        /// <param name="f">the float value to send to the client</param>
        /// <exception cref="IOException">if an input or output exception occurred</exception>
        public void Print(float f)
        {
            Print(f.ToString());
        }



        
        /// <summary>
        /// Writes a <code>double</code> value to the client,
        /// with no carriage return-line feed (CRLF) at the end.
        /// </summary>
        /// <param name="d">the double value to send to the client</param>
        /// <exception cref="IOException">if an input or output exception occurred</exception>
        public void Print(double d)
        {
            Print(d.ToString());
        }



        /// <summary>
        /// Writes a carriage return-line feed (CRLF)
        /// to the client.
        /// </summary>
        /// <exception cref="IOException">if an input or output exception occurred</exception>
        public void Println()
        {
            Print("\r\n");
        }


        /// <summary>
        /// Writes a string to the client, 
        /// followed by a carriage return-line feed (CRLF).
        /// </summary>
        /// <param name="s">the string to write to the client</param>
        /// <exception cref="IOException">if an input or output exception occurred</exception>
        public void Println(string s)
        {
            Print(s);
            Println();
        }



        /// <summary>
        /// Writes a <code>bool</code> value to the client, 
        /// followed by a 
        /// carriage return-line feed (CRLF).
        /// </summary>
        /// <param name="b">the bool value to write to the client</param>
        /// <exception cref="IOException">if an input or output exception occurred</exception>
        public void Println(bool b)
        {
            Print(b);
            Println();
        }



        /// <summary>
        /// Writes a character to the client, followed by a carriage
        /// return-line feed (CRLF).
        /// </summary>
        /// <param name="c">the character to write to the client</param>
        /// <exception cref="IOException">if an input or output exception occurred</exception>
        public void Println(char c)
        {
            Print(c);
            Println();
        }



        /// <summary>
        /// Writes an int to the client, followed by a 
        /// carriage return-line feed (CRLF) character.
        /// </summary>
        /// <param name="i">the int to write to the client</param>
        /// <exception cref="IOException">if an input or output exception occurred</exception>
        public void Println(int i)
        {
            Print(i);
            Println();
        }


        /// <summary>
        /// Writes a <code>long</code> value to the client, followed by a 
        /// carriage return-line feed (CRLF).
        /// </summary>
        /// <param name="l">the long value to write to the client</param>
        /// <exception cref="IOException">if an input or output exception occurred</exception>
        public void Println(long l)
        {
            Print(l);
            Println();
        }



        /// <summary>
        /// Writes a float value to the client, 
        /// followed by a carriage return-line feed (CRLF).
        /// </summary>
        /// <param name="f">the float value to write to the client</param>
        /// <exception cref="IOException">if an input or output exception occurred</exception>
        public void Println(float f)
        {
            Print(f);
            Println();
        }



        /// <summary>
        /// Writes a double value to the client, 
        /// followed by a carriage return-line feed (CRLF).
        /// </summary>
        /// <param name="d">the double value to write to the client</param>
        /// <exception cref="IOException">if an input or output exception occurred</exception>
        public void Println(double d)
        {
            Print(d);
            Println();
        }
    }
}

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
    /// Provides an input stream for reading binary data from a client
    /// request, including an efficient <code>readLine</code> method
    /// for reading data one line at a time. With some protocols, such
    /// as HTTP POST and PUT, a <code>ServletInputStream</code>
    /// object can be used to read data sent from the client.
    ///
    /// <p>A <code>ServletInputStream</code> object is normally retrieved via
    /// the {@link ServletRequest#getInputStream} method.
    ///
    ///
    /// <p>This is an abstract class that a servlet container implements.
    /// Subclasses of this class
    /// must implement the System.IO.TextReader.Read() method.
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// April 19, 2009
    /// </date>
    public abstract class ServletInputStream : TextReader
    {

        /// <summary>
        /// Reads the input stream, one line at a time. Starting at an
        /// offset, reads bytes into an array, until it reads a certain number
        /// of bytes or reaches a newline character, which it reads into the
        /// array as well.
        ///
        /// This method returns -1 if it reaches the end of the input
        /// stream before reading the maximum number of bytes.
        /// </summary>
        /// <param name="b">an array of bytes into which data is read</param>
        /// <param name="off">an integer specifying the character at which this method begins reading</param>
        /// <param name="len">an integer specifying the maximum number of bytes to read</param>
        /// <returns>an integer specifying the actual number of bytes read, or -1 if the end of the stream is reached</returns>
        /// <exception cref="System.IO.IOException">if an input or output exception has occurred</exception>
        public int ReadLine(byte[] b, int off, int len)
        {
            if (len <= 0)
            {
                return 0;
            }
            int count = 0, c;

            while ((c = Read()) != -1)
            {
                b[off++] = (byte)c;
                count++;
                if (c == '\n' || count == len)
                {
                    break;
                }
            }
            return count > 0 ? count : -1;
        }

    }
}

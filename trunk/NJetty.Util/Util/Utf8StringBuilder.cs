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
    /// Non-Thread safe Utf8 String Generator
    /// using per byte input, 
    /// this makes use of an array of bytes that can grow
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// November 2008
    /// </date>
    public class Utf8StringBuilder
    {
        
        const int DEFAULT_CAPACITY=256;
        const int DEFAUlT_GROW_BY=256;

        readonly int _growBy = DEFAUlT_GROW_BY;
        byte[] _buffer;
        int _pointer = 0;
        

        public Utf8StringBuilder()
        {
            _buffer = new byte[DEFAULT_CAPACITY];
        }

        public Utf8StringBuilder(int capacity)
        {
            _buffer = new byte[capacity];
        }

        public Utf8StringBuilder(int capacity, int growBy)
        {
            _buffer = new byte[capacity];
            _growBy = growBy;
        }

        public void Append(byte[] b, int offset, int length)
        {
            int end = offset + length;
            for (int i = offset; i < end; i++)
                Append(b[i]);
        }

        public void Append(byte b)
        {
            if (_pointer == _buffer.Length)
            {
                byte[] buff = new byte[_pointer + _growBy];
                Buffer.BlockCopy(_buffer, 0, buff, 0, _pointer);
                _buffer = buff;
            }

            _buffer[_pointer++] = b;
        }

        public int Length
        {
            get
            {
                return _pointer;
            }
        }

        public void Reset()
        {
            _pointer = 0;
        }

        public StringBuilder StringBuilder
        {
            get
            {
                return new StringBuilder(ToString(), _buffer.Length);
            }
        }

        public override string ToString()
        {
            return System.Text.Encoding.UTF8.GetString(_buffer, 0, _pointer);
        }
    }
}

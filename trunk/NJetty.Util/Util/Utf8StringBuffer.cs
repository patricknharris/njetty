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
    /// TODO: Class/Interface Information here
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// November 2008
    /// </date>
    public class Utf8StringBuffer
    {

        Utf8StringBuilder _sb;
        object _lock = new object();
        

        public Utf8StringBuffer()
        {
            _sb = new Utf8StringBuilder();
        }

        public Utf8StringBuffer(int capacity)
        {
            _sb = new Utf8StringBuilder(capacity);
        }

        public Utf8StringBuffer(int capacity, int growBy)
        {
            _sb = new Utf8StringBuilder(capacity, growBy);
        }

        public void Append(byte[] b, int offset, int length)
        {
            lock (_lock)
            {
                _sb.Append(b, offset, length);
            }
        }

        public void Append(byte b)
        {
            lock (_lock)
            {
                _sb.Append(b);
            }
        }

        public int Length
        {
            get
            {
                lock (_lock)
                {
                    return _sb.Length;
                }
            }
        }

        public void Reset()
        {
            lock (_lock)
            {
                _sb.Reset();
            }
        }

        public StringBuilder StringBuilder
        {
            get
            {
                lock (_lock)
                {
                    return _sb.StringBuilder;
                }
            }
        }

        public override string ToString()
        {
            lock (_lock)
            {
                return _sb.ToString();
            }
        }
    }
}

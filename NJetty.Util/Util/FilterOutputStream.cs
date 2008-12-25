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

namespace NJetty.Util.Util
{

    /// <summary>
    /// Implementation of the java.io.FilterOutputStream
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// December 2008
    /// </date>
    public class FilterOutputStream : Stream
    {

        protected Stream output;


        public FilterOutputStream(Stream output)
        {
            this.output = output;
        }

        public override bool CanRead
        {
            get { return false; }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override void Flush()
        {
            output.Flush();
        }

        public override void Close()
        {

            try
            {
                Flush();
            }
            catch (Exception)
            {
            }
            output.Close();
        }

        public override long Length
        {
            get { return output.Length; }
        }

        public override long Position
        {
            get
            {
                return output.Position;
            }
            set
            {
                output.Position = value;
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            output.SetLength(value);
        }

        public void Write(byte[] buffer)
        {
            output.Write(buffer, 0, buffer.Length);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            output.Write(buffer, offset, count);
        }

        public void Write(string s)
        {
            if (s == null)
            {
                return;
            }
            byte[] bytes = s.GetBytes();
            Write(bytes, 0, bytes.Length);
        }

        public void WriteByte(int b)
        {
            output.WriteByte((byte)b);
        }


        public override void WriteByte(byte b)
        {
            output.WriteByte(b);
        }
    }
}

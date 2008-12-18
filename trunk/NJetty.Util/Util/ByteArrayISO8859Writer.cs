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
    /// This class combines the features of a OutputStreamWriter for
    /// ISO8859 encoding with that of a ByteArrayOutputStream.  It avoids
    /// many inefficiencies associated with these standard library classes.
    /// It has been optimized for standard ASCII characters.
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// December 2008
    /// </date>
    public class ByteArrayISO8859Writer : Stream
    {

        byte[] _buf;
        int _size;
        ByteArrayOutputStream2 _bout=null;
        BinaryWriter _writer = null;
        bool _fixed=false;

        object _thisLock = new object();

    
        public ByteArrayISO8859Writer()
        {
            
            _buf=new byte[2048];
        } 
    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="capacity">Buffer capacity</param>
        public ByteArrayISO8859Writer(int capacity)
        {
            _buf=new byte[capacity];
        }
    
        public ByteArrayISO8859Writer(byte[] buf)
        {
            _buf=buf;
            _fixed=true;
        }

        public object Lock
        {
            get{ return _thisLock; }
        }

        public override long Length
        {
            get{ return _size; }
        }

        public override void SetLength(long l)
        {
            _size = (int)l;
        }
    
        public int Capacity
        {
            get{ return _buf.Length; }
        }

        public long SpareCapacity
        {
            get{ return _buf.Length-_size;}
        }
    
    

        public byte[] GetBuffer()
        {
            return _buf;
        }
    
        public void WriteTo(Stream output)
        {
            output.Write(_buf, 0,_size);
        }

        public void Write(char c)
        {
            EnsureSpareCapacity(1);
            if (c>=0&&c<=0x7f)
                _buf[_size++]=(byte)c;
            else
            {
                char[] ca ={c};
                WriteEncoded(ca,0,1);
            }
        }
    
        public void Write(char[] ca)
        {
            EnsureSpareCapacity(ca.Length);
            for (int i=0;i<ca.Length;i++)
            {
                char c=ca[i];
                if (c>=0&&c<=0x7f)
                    _buf[_size++]=(byte)c;
                else
                {
                    WriteEncoded(ca,i,ca.Length-i);
                    break;
                }
            }
        }
    
        public void Write(char[] ca,int offset, int length)
        {
            EnsureSpareCapacity(length);
            for (int i=0;i<length;i++)
            {
                char c=ca[offset+i];
                if (c>=0&&c<=0x7f)
                    _buf[_size++]=(byte)c;
                else
                {
                    WriteEncoded(ca,offset+i,length-i);
                    break;
                }
            }
        }
    
        public void Write(string s)
        {
            if (s==null)
            {
                Write("null",0,4);
                return;
            }
            
            int length=s.Length;
            EnsureSpareCapacity(length);
            for (int i=0;i<length;i++)
            {
                char c=s[i];
                if (c>=0x0&&c<=0x7f)
                    _buf[_size++]=(byte)c;
                else
                {
                    WriteEncoded(s.ToCharArray(),i,length-i);
                    break;
                }
            }
        }
        
        public void Write(string s,int offset, long length)
        {
            EnsureSpareCapacity(length);
            for (int i=0;i<length;i++)
            {
                char c=s[offset+i];
                if (c>=0&&c<=0x7f)
                    _buf[_size++]=(byte)c;
                else
                {
                    WriteEncoded(s.ToCharArray(),offset+i,length-i);
                    break;
                }
            }
        }

        static Encoding __ISO_8859_1 = Encoding.GetEncoding(StringUtil.__ISO_8859_1);
        private void WriteEncoded(char[] ca,int offset, long length)
        {
            if (_bout == null)
            {
                _bout = new ByteArrayOutputStream2((int)(2 * length));
                _writer = new BinaryWriter(_bout, __ISO_8859_1);
            }
            else
            {
                _bout.Reset();
            }
            _writer.Write(ca,offset,(int)length);
            _writer.Flush();
            EnsureSpareCapacity(_bout.Length);
            Buffer.BlockCopy(_bout.GetBuffer(), 0, _buf, _size, (int)_bout.Length);
            _size+=(int)_bout.Length;
        }
    
    

        public void ResetWriter()
        {
            _size=0;
        }

        public override void Close()
        {}

        public void Destroy()
        {
            _buf=null;
        }
    
        public void EnsureSpareCapacity(long n)
        {
            if (_size+n>_buf.Length)
            {
                if (_fixed)
                {
                    throw new IOException("Buffer overflow: "+_buf.Length);
                }
                
                byte[] buf = new byte[(_buf.Length+n)*4/3];
                Buffer.BlockCopy(_buf,0,buf,0,_size);
                _buf=buf;
            }
        }


        public byte[] GetByteArray()
        {
            byte[] data=new byte[_size];
            Buffer.BlockCopy(_buf,0,data,0,_size);
            return data;
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
        {}

        public override long Position
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
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

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (_bout == null)
            {
                _bout = new ByteArrayOutputStream2((int)(2 * count));
                _writer = new BinaryWriter(_bout, __ISO_8859_1);
            }
            else
            {
                _bout.Reset();
            }
            _writer.Write(buffer, offset, count);
            _writer.Flush();
            EnsureSpareCapacity(_bout.Length);
            Buffer.BlockCopy(_bout.GetBuffer(), 0, _buf, _size, (int)_bout.Length);
            _size += (int)_bout.Length;
        }
    }
}

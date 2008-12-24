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
using NJetty.Util.Logging;

namespace NJetty.Util.Util
{

    /// <summary>
    /// Handle a multipart MIME response.
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// December 2008
    /// </date>
    public class MultiPartOutputStream : FilterOutputStream
    {
        private static byte[] __CRLF;
        private static byte[] __DASHDASH;

        public static string MULTIPART_MIXED = "multipart/mixed";
        public static string MULTIPART_X_MIXED_REPLACE = "multipart/x-mixed-replace";

        static MultiPartOutputStream()
        {
            try
            {
                __CRLF="\015\012".GetBytes(StringUtil.__ISO_8859_1);
                __DASHDASH="--".GetBytes(StringUtil.__ISO_8859_1);
            }
            catch (Exception e) 
            {
                Log.Warn(e);
                Environment.Exit(1);
            }
        }


        private string boundary;
        private byte[] boundaryBytes;

        private bool inPart=false;    
    
        public MultiPartOutputStream(Stream output) : base(output)
        {
            
            boundary = "NJetty" + this.GetHashCode() +
            ((DateTime.UtcNow.Ticks / 1000) % 10000).ToString(36);
            boundaryBytes=boundary.GetBytes(StringUtil.__ISO_8859_1);
            inPart=false;
        }

        

        /// <summary>
        /// End the current part.
        /// </summary>
        /// <exception cref="IOException"></exception>
        public override void Close()
        {
            if (inPart)
                Write(__CRLF);
            Write(__DASHDASH);
            Write(boundaryBytes);
            Write(__DASHDASH);
            Write(__CRLF);
            inPart=false;
            base.Close();
        }
        
        public string Boundary
        {
            get{return boundary;}
        }

        public Stream Output 
        {
            get { return output; }
        }
        
       
        /// <summary>
        /// Start creation of the next Content.
        /// </summary>
        /// <param name="contentType"></param>
        public void StartPart(string contentType)
        {
            if (inPart)
                Write(__CRLF);
            inPart=true;
            Write(__DASHDASH);
            Write(boundaryBytes);
            Write(__CRLF);
            Write(("Content-Type: "+contentType).GetBytes(StringUtil.__ISO_8859_1));
            Write(__CRLF);
            Write(__CRLF);
        }
            
        /// <summary>
        /// Start creation of the next Content.
        /// </summary>
        /// <param name="contentType"></param>
        /// <param name="headers"></param>
        public void StartPart(string contentType, string[] headers)
        {
            if (inPart)
                Write(__CRLF);
            inPart=true;
            Write(__DASHDASH);
            Write(boundaryBytes);
            Write(__CRLF);
            Write(("Content-Type: "+contentType).GetBytes(StringUtil.__ISO_8859_1));
            Write(__CRLF);
            for (int i=0;headers!=null && i<headers.Length;i++)
            {
                Write(headers[i].GetBytes(StringUtil.__ISO_8859_1));
                Write(__CRLF);
            }
            Write(__CRLF);
        }

    }
}

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
    /// Handle a multipart MIME response.
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// December 2008
    /// </date>
    public class MultiPartWriter : FilterWriter
    {
        private const string __CRLF="\015\012";
        private const string __DASHDASH="--";
        
        public static string MULTIPART_MIXED=MultiPartOutputStream.MULTIPART_MIXED;
        public static string MULTIPART_X_MIXED_REPLACE=MultiPartOutputStream.MULTIPART_X_MIXED_REPLACE;
        
        private string boundary;

        private bool inPart=false;    
        
        /* ------------------------------------------------------------ */
        public MultiPartWriter(Stream output) : base(output)
        {
            boundary = "NJetty"+this.GetHashCode()+
            ((DateTime.UtcNow.Ticks/1000) % 10000).ToString(36);
            
            
            
            
            inPart=false;
        }

        /* ------------------------------------------------------------ */
        /** End the current part.
         * @exception IOException IOException
         */
        public void close()
        {
            if (inPart)
                output.Write(__CRLF);
            output.Write(__DASHDASH);
            output.write(boundary);
            output.write(__DASHDASH);
            output.write(__CRLF);
            inPart=false;
            base.Close();
        }
        
        /* ------------------------------------------------------------ */
        public string getBoundary()
        {
            return boundary;
        }
        
        /* ------------------------------------------------------------ */
        /** Start creation of the next Content.
         */
        public void startPart(string contentType)
        {
            if (inPart)
                output.write(__CRLF);
            output.write(__DASHDASH);
            output.write(boundary);
            output.write(__CRLF);
            output.write("Content-Type: ");
            output.write(contentType);
            output.write(__CRLF);
            output.write(__CRLF);
            inPart=true;
        }
        
        /* ------------------------------------------------------------ */
        /** end creation of the next Content.
         */
        public void endPart()
        {
            if (inPart)
                output.write(__CRLF);
            inPart=false;
        }
            
        /* ------------------------------------------------------------ */
        /** Start creation of the next Content.
         */
        public void startPart(string contentType, string[] headers)
        {
            if (inPart)
                output.write(__CRLF);
            output.write(__DASHDASH);
            output.write(boundary);
            output.write(__CRLF);
            output.write("Content-Type: ");
            output.write(contentType);
            output.write(__CRLF);
            for (int i=0;headers!=null && i<headers.length;i++)
            {
                output.write(headers[i]);
                output.write(__CRLF);
            }
            output.write(__CRLF);
            inPart=true;
        }
    }
}

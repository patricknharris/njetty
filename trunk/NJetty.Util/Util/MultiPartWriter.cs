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
        private const string __CRLF = "\015\012";
        private const string __DASHDASH = "--";

        public static string MULTIPART_MIXED = MultiPartOutputStream.MULTIPART_MIXED;
        public static string MULTIPART_X_MIXED_REPLACE = MultiPartOutputStream.MULTIPART_X_MIXED_REPLACE;

        private string boundary;

        private bool inPart = false;

        public MultiPartWriter(Stream output)
            : base(output)
        {
            boundary = "NJetty" + this.GetHashCode() +
            ((DateTime.UtcNow.Ticks / 1000) % 10000).ToString(36);
            inPart = false;
        }

        /// <summary>
        /// End the current part.
        /// </summary>
        /// <exception cref="IOException"></exception>
        public override void Close()
        {
            if (inPart)
            {
                Write(__CRLF);
            }
            Write(__DASHDASH);
            Write(boundary);
            Write(__DASHDASH);
            Write(__CRLF);
            inPart = false;
            base.Close();
        }

        public string Boundary
        {
            get { return boundary; }
        }

        /// <summary>
        /// Start creation of the next Content.
        /// </summary>
        /// <param name="contentType"></param>
        public void StartPart(string contentType)
        {
            if (inPart)
            {
                Write(__CRLF);
            }
            Write(__DASHDASH);
            Write(boundary);
            Write(__CRLF);
            Write("Content-Type: ");
            Write(contentType);
            Write(__CRLF);
            Write(__CRLF);
            inPart = true;
        }

        /// <summary>
        /// end creation of the next Content.
        /// </summary>
        public void EndPart()
        {
            if (inPart)
            {
                Write(__CRLF);
            }
            inPart = false;
        }

        /// <summary>
        /// Start creation of the next Content.
        /// </summary>
        /// <param name="contentType"></param>
        /// <param name="headers"></param>
        public void StartPart(string contentType, string[] headers)
        {
            if (inPart)
            {
                Write(__CRLF);
            }
            Write(__DASHDASH);
            Write(boundary);
            Write(__CRLF);
            Write("Content-Type: ");
            Write(contentType);
            Write(__CRLF);
            for (int i = 0; headers != null && i < headers.Length; i++)
            {
                Write(headers[i]);
                Write(__CRLF);
            }
            Write(__CRLF);
            inPart = true;
        }
    }
}

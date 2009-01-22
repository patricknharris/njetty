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

namespace Javax.NServlet.Http
{

    /// <summary>
    /// Constants for IHttpServletRequest
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// January 2009
    /// </date>
    public static class HttpServletRequestConstants
    {
        /// <summary>
        /// string identifier for Basic authentication. Value "BASIC"
        /// </summary>
        public const string BASIC_AUTH = "BASIC";

        /// <summary>
        /// string identifier for Form authentication. Value "FORM"
        /// </summary>
        public const string FORM_AUTH = "FORM";

        /// <summary>
        /// string identifier for Client Certificate authentication. Value "CLIENT_CERT"
        /// </summary>
        public const string CLIENT_CERT_AUTH = "CLIENT_CERT";

        /// <summary>
        /// string identifier for Digest authentication. Value "DIGEST"
        /// </summary>
        public const string DIGEST_AUTH = "DIGEST";
    }
}

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
using System.Collections;

namespace Javax.NServlet.Http
{

    /// <summary>
    /// Obsolete As of Java(tm) Servlet API 2.1 for security reasons, 
    /// with no replacement. This interface will be removed in a future 
    /// version of this API.
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// April 28, 2009
    /// </date>
    [Obsolete("As of Java(tm) Servlet API 2.1 for security reasons, with no replacement. This interface will be removed in a future version of this API.")]
    public interface IHttpSessionContext
    {
      


        /// <summary>
        /// Obsolete As of Java Servlet API 2.1 with
        /// no replacement. This method must 
        /// return null and will be removed in
        /// a future version of this API.
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        [Obsolete("As of Java Servlet API 2.1 with no replacement. This method must return null and will be removed in a future version of this API.")]
        IHttpSession GetSession(String sessionId);




        /// <summary>
        /// Obsolete as of Java Servlet API 2.1 with no replacement. 
        /// This method must return an empty List and will be removed 
        /// in a future version of this API.
        /// </summary>
        [Obsolete("As of Java Servlet API 2.1 with no replacement. This method must return an empty List and will be removed in a future version of this API.")]
        IList Ids
        {
            get;
        }
    }
}

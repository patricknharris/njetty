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

namespace Javax.NServlet
{

    /// <summary>
    /// Events of this kind indicate lifecycle
    /// events for a ServletRequest.
    /// The source of the event
    /// is the ServletContext of this web application.
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// April 21, 2009
    /// </date>
    public class ServletRequestEvent //TODO: in java: extends java.util.EventObject
    {
        IServletRequest request;
        IServletContext sc;

        /// <summary>
        /// Construct a ServletRequestEvent for the given ServletContext
        /// and ServletRequest.
        /// </summary>
        /// <param name="sc">the ServletContext of the web application.</param>
        /// <param name="request">the ServletRequest that is sending the event.</param>
        public ServletRequestEvent(IServletContext sc, IServletRequest request)
        {
            this.sc = sc;
            this.request = request;
        }

        /// <summary>
        /// Gets the ServletRequest that is changing.
        /// </summary>
        public IServletRequest ServletRequest
        {
            get { return this.request; }
        }

        /// <summary>
        /// Gets the ServletContext of this web application.
        /// </summary>
        public IServletContext ServletContext
        {
            get { return (IServletContext)sc; }
        }
    }
}

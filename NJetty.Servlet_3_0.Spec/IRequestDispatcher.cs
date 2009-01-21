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
    /// Defines an object that receives requests from the client
    /// and sends them to any resource (such as a servlet,
    /// HTML file, or JSP file) on the server. The servlet
    /// container creates the <code>RequestDispatcher</code> object,
    /// which is used as a wrapper around a server resource located
    /// at a particular path or given by a particular name.
    /// 
    /// This interface is intended to wrap servlets,
    /// but a servlet container can create RequestDispatcher
    /// objects to wrap any type of resource.
    /// </summary>
    /// <see cref="IServletContext#GetRequestDispatcher(string)"/>
    /// <see cref="IServletContext#GetNamedDispatcher(string)"/>
    /// <see cref="IServletRequest#GetRequestDispatcher(string)"/>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// January 2009
    /// </date>
    public interface IRequestDispatcher
    {

        /// <summary>
        /// Forwards a request from
        /// a servlet to another resource (servlet, JSP file, or
        /// HTML file) on the server. This method allows
        /// one servlet to do preliminary processing of
        /// a request and another resource to generate
        /// the response.
        /// 
        /// For a RequestDispatcher obtained via 
        /// GetRequestDispatcher(), the ServletRequest
        /// object has its path elements and parameters adjusted to match
        /// the path of the target resource.
        /// 
        /// Forward should be called before the response has been 
        /// committed to the client (before response body output has been flushed).  
        /// If the response already has been committed, this method throws
        /// an InvalidOperationException.
        /// Uncommitted output in the response buffer is automatically cleared 
        /// before the forward.
        /// 
        /// The request and response parameters must be either the same
        /// objects as were passed to the calling servlet's service method or be
        /// subclasses of the <see cref="ServletRequestWrapper"/>  or <see cref="ServletResponseWrapper"/> classes
        /// that wrap them.
        /// </summary>
        /// <param name="request">
        ///     a IServletRequest object 
        ///     that represents the request the client
        ///     makes of the servlet
        /// </param>
        /// <param name="response">
        ///     a IServletResponse object
        ///     that represents the response the servlet
        ///     returns to the client
        /// </param>
        /// <exception cref="ServletException">if the included resource throws this exception</exception>
        /// <exception cref="System.IO.Exception">if the included resource throws this exception</exception>
        void Forward(IServletRequest request, IServletResponse response);



        /// <summary>
        /// Includes the content of a resource (servlet, JSP page,
        /// HTML file) in the response. In essence, this method enables 
        /// programmatic server-side includes.
        ///
        /// The IServletResponse object has its path elements
        /// and parameters remain unchanged from the caller's. The included
        /// servlet cannot change the response status code or set headers;
        /// any attempt to make a change is ignored.
        /// 
        /// The request and response parameters must be either the same
        /// objects as were passed to the calling servlet's service method or be
        /// subclasses of the ServletRequestWrapper or ServletResponseWrapper classes
        /// that wrap them.
        /// </summary>
        /// <param name="request">a IServletRequest object that contains the client's request</param>
        /// <param name="response">a IServletResponse object that contains the servlet's response</param>
        /// <exception cref="ServletException">if the included resource throws this exception</exception>
        /// <exception cref="System.IO.Exception">if the included resource throws this exception</exception>
        void Include(IServletRequest request, IServletResponse response);
    }
}

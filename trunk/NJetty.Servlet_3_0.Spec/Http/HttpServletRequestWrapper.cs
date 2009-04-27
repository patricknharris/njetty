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
    /// Provides a convenient implementation of the HttpServletRequest interface that
    /// can be subclassed by developers wishing to adapt the request to a Servlet.
    /// This class implements the Wrapper or Decorator pattern. Methods default to
    /// calling through to the wrapped request object.
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// April 27, 2009
    /// </date>
    public class HttpServletRequestWrapper : ServletRequestWrapper, IHttpServletRequest
    {
       
        /// <summary>
        /// Constructs a request object wrapping the given request.
        /// </summary>
        /// <param name="request"></param>
        /// <exception cref="ArgumentException">if the request is null</exception>
        public HttpServletRequestWrapper(IHttpServletRequest request) : base(request)
        {
            
        }



        private IHttpServletRequest HttpServletRequest
        {
            get
            {
                return (IHttpServletRequest)base.Request;
            }
        }

        /// <summary>
        /// The default behavior of this property is to return get AuthType
        /// on the wrapped request object.
        /// </summary>
        public string AuthType
        {
            get { return HttpServletRequest.AuthType; }
        }

        /// <summary>
        /// The default behavior of this property is to return get Cookies
        /// on the wrapped request object.
        /// </summary>
        public Cookie[] Cookies
        {
            get { return HttpServletRequest.Cookies; }
        }

        /// <summary>
        ///  The default behavior of this method is to return GetDateHeader(string name)
        ///  on the wrapped request object.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public long GetDateHeader(string name)
        {
            return HttpServletRequest.GetDateHeader(name);
        }

        /// <summary>
        /// The default behavior of this method is to return GetHeader(string name)
        /// on the wrapped request object.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetHeader(string name)
        {
            return HttpServletRequest.GetHeader(name);
        }

        /// <summary>
        /// The default behavior of this method is to return GetHeaders(string name)
        /// on the wrapped request object.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public System.Collections.IList GetHeaders(string name)
        {
            return HttpServletRequest.GetHeaders(name);
        }

        /// <summary>
        /// The default behavior of this property is Get the HeaderNames
        /// on the wrapped request object.
        /// </summary>
        public System.Collections.IList HeaderNames
        {
            get { return HttpServletRequest.HeaderNames; }
        }

        /// <summary>
        /// The default behavior of this method is to return GetIntHeader(String name)
        /// on the wrapped request object.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int GetIntHeader(string name)
        {
            return HttpServletRequest.GetIntHeader(name);
        }

        /// <summary>
        /// The default behavior of this property is to Get Method
        /// on the wrapped request object.
        /// </summary>
        public string Method
        {
            get { return HttpServletRequest.Method; }
        }

        /// <summary>
        /// The default behavior of this property is to Get PathInfo
        /// on the wrapped request object.
        /// </summary>
        public string PathInfo
        {
            get { return HttpServletRequest.PathInfo; }
        }

        /// <summary>
        /// The default behavior of this property is to Get PathTranslated
        /// on the wrapped request object.
        /// </summary>
        public string PathTranslated
        {
            get { return HttpServletRequest.PathTranslated; }
        }

        /// <summary>
        /// The default behavior of this property is to Get ContextPath
        /// on the wrapped request object.
        /// </summary>
        public string ContextPath
        {
            get { return HttpServletRequest.ContextPath; }
        }

        /// <summary>
        /// The default behavior of this property is to Get QueryString
        /// on the wrapped request object.
        /// </summary>
        public string QueryString
        {
            get { return HttpServletRequest.QueryString; }
        }

        /// <summary>
        /// The default behavior of this property is to Get RemoteUser
        /// on the wrapped request object.
        /// </summary>
        public string RemoteUser
        {
            get { return HttpServletRequest.RemoteUser; }
        }


        /// <summary>
        /// The default behavior of this method is to return IsUserInRole(string role)
        /// on the wrapped request object.
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public bool IsUserInRole(string role)
        {
            return HttpServletRequest.IsUserInRole(role);
        }


        /// <summary>
        /// The default behavior of this property is to Get UserPrincipal
        /// on the wrapped request object.
        /// </summary>
        public System.Security.Principal.IPrincipal UserPrincipal
        {
            get { return HttpServletRequest.UserPrincipal; }
        }

        /// <summary>
        /// The default behavior of this property is to Get RequestedSessionId
        /// on the wrapped request object.
        /// </summary>
        public string RequestedSessionId
        {
            get { return HttpServletRequest.RequestedSessionId; }
        }

        /// <summary>
        /// The default behavior of this property is to Get RequestURI
        /// on the wrapped request object.
        /// </summary>
        public string RequestURI
        {
            get { return HttpServletRequest.RequestURI; }
        }


        /// <summary>
        /// The default behavior of this property is to Get RequestURL
        /// on the wrapped request object.
        /// </summary>
        public StringBuilder RequestURL
        {
            get { return HttpServletRequest.RequestURL; }
        }


        /// <summary>
        /// The default behavior of this property is to Get ServletPath
        /// on the wrapped request object.
        /// </summary>
        public string ServletPath
        {
            get { return HttpServletRequest.ServletPath; }
        }


        /// <summary>
        /// The default behavior of this method is to return GetSession(bool create)
        /// on the wrapped request object.
        /// </summary>
        /// <param name="create"></param>
        /// <returns></returns>
        public IHttpSession GetSession(bool create)
        {
            return HttpServletRequest.GetSession(create);
        }


        /// <summary>
        /// The default behavior of this property is to Get Session
        /// on the wrapped request object.
        /// </summary>
        public IHttpSession Session
        {
            get { return HttpServletRequest.Session; }
        }


        /// <summary>
        /// The default behavior of this property is to Get IsRequestedSessionIdValid
        /// on the wrapped request object.
        /// </summary>
        public bool IsRequestedSessionIdValid
        {
            get { return HttpServletRequest.IsRequestedSessionIdValid; }
        }


        /// <summary>
        /// The default behavior of this property is to Get IsRequestedSessionIdFromCookie
        /// on the wrapped request object.
        /// </summary>
        public bool IsRequestedSessionIdFromCookie
        {
            get { return HttpServletRequest.IsRequestedSessionIdFromCookie; }
        }


        /// <summary>
        /// The default behavior of this property is to Get IsRequestedSessionIdFromURL
        /// on the wrapped request object.
        /// </summary>
        public bool IsRequestedSessionIdFromURL
        {
            get { return HttpServletRequest.IsRequestedSessionIdFromURL; }
        }


        /// <summary>
        /// The default behavior of this property is to Get IsRequestedSessionIdFromUrl
        /// on the wrapped request object.
        /// </summary>
        public bool IsRequestedSessionIdFromUrl
        {
            get { return HttpServletRequest.IsRequestedSessionIdFromUrl; }
        }
    }
}

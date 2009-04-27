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
    /// Provides a convenient implementation of the HttpServletResponse interface that
    /// can be subclassed by developers wishing to adapt the response from a Servlet.
    /// This class implements the Wrapper or Decorator pattern. Methods default to
    /// calling through to the wrapped response object.
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// April 27, 2009
    /// </date>
    public class HttpServletResponseWrapper : ServletResponseWrapper, IHttpServletResponse
    {
        /// <summary>
        /// Constructs a response adaptor wrapping the given response.
        /// </summary>
        /// <param name="response"></param>
        /// <exception cref="ArgumentException">if the response is null</exception>
        public HttpServletResponseWrapper(IHttpServletResponse response)
            : base(response)
        {
        }

        private IHttpServletResponse HttpServletResponse
        {
            get { return (IHttpServletResponse)base.Response; }
        }


        /// <summary>
        /// The default behavior of this method is to call AddCookie(Cookie cookie)
        /// on the wrapped response object.
        /// </summary>
        /// <param name="cookie"></param>
        public void AddCookie(Cookie cookie)
        {
            HttpServletResponse.AddCookie(cookie);
        }

        /// <summary>
        /// The default behavior of this method is to call ContainsHeader(string name)
        /// on the wrapped response object.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool ContainsHeader(string name)
        {
            return HttpServletResponse.ContainsHeader(name);
        }

        /// <summary>
        /// The default behavior of this method is to call EncodeURL(string url)
        /// on the wrapped response object.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string EncodeURL(string url)
        {
            return HttpServletResponse.EncodeURL(url);
        }

        /// <summary>
        /// The default behavior of this method is to call EncodeRedirectURL(string url)
        /// on the wrapped response object.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string EncodeRedirectURL(string url)
        {
            return HttpServletResponse.EncodeRedirectURL(url);
        }


        /// <summary>
        /// The default behavior of this method is to call EncodeUrl(string url)
        /// on the wrapped response object.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string EncodeUrl(string url)
        {
            return HttpServletResponse.EncodeUrl(url);
        }


        /// <summary>
        /// The default behavior of this method is to call EncodeRedirectUrl(string url)
        /// on the wrapped response object.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string EncodeRedirectUrl(string url)
        {
            return HttpServletResponse.EncodeRedirectUrl(url);
        }

        /// <summary>
        /// The default behavior of this method is to call SendError(int sc, string msg)
        /// on the wrapped response object.
        /// </summary>
        /// <param name="sc"></param>
        /// <param name="msg"></param>
        public void SendError(int sc, string msg)
        {
            HttpServletResponse.SendError(sc, msg);
        }

        /// <summary>
        /// The default behavior of this method is to call SendError(int sc)
        /// on the wrapped response object.
        /// </summary>
        /// <param name="sc"></param>
        public void SendError(int sc)
        {
            HttpServletResponse.SendError(sc);
        }

        /// <summary>
        /// The default behavior of this method is to call SendRedirect(string location)
        /// on the wrapped response object.
        /// </summary>
        /// <param name="location"></param>
        public void SendRedirect(string location)
        {
            HttpServletResponse.SendRedirect(location);
        }

        /// <summary>
        /// The default behavior of this method is to call SetDateHeader(string name, long date)
        /// on the wrapped response object.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="date"></param>
        public void SetDateHeader(string name, long date)
        {
            HttpServletResponse.SetDateHeader(name, date);
        }

        /// <summary>
        /// The default behavior of this method is to call AddDateHeader(string name, long date)
        /// on the wrapped response object.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="date"></param>
        public void AddDateHeader(string name, long date)
        {
            HttpServletResponse.AddDateHeader(name, date);
        }
        

        /// <summary>
        /// The default behavior of this method is to call SetHeader(string name, string value)
        /// on the wrapped response object.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetHeader(string name, string value)
        {
            HttpServletResponse.SetHeader(name, value);
        }
        
        /// <summary>
        /// The default behavior of this method is to call AddHeader(string name, string value)
        /// on the wrapped response object.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void AddHeader(string name, string value)
        {
            HttpServletResponse.AddHeader(name, value);
        }

        /// <summary>
        /// The default behavior of this method is to call SetIntHeader(string name, int value)
        /// on the wrapped response object.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetIntHeader(string name, int value)
        {
            HttpServletResponse.SetIntHeader(name, value);
        }

        /// <summary>
        /// The default behavior of this method is to call AddIntHeader(string name, int value)
        /// on the wrapped response object.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void AddIntHeader(string name, int value)
        {
            HttpServletResponse.AddIntHeader(name, value);
        }


        /// <summary>
        /// The default behavior of this property is to set Status
        /// on the wrapped response object.
        /// </summary>
        public int Status
        {
            set { HttpServletResponse.Status = value; }
        }


        /// <summary>
        /// The default behavior of this method is to call SetStatus(int sc, string sm)
        /// on the wrapped response object.
        /// </summary>
        /// <param name="sc"></param>
        /// <param name="sm"></param>
        public void SetStatus(int sc, string sm)
        {
            HttpServletResponse.SetStatus(sc, sm);
        }
    }
}

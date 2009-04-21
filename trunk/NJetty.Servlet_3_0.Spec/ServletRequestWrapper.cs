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
    /// Provides a convenient implementation of the ServletRequest interface that
    /// can be subclassed by developers wishing to adapt the request to a Servlet.
    /// This class implements the Wrapper or Decorator pattern. Methods default to
    /// calling through to the wrapped request object.
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// April 21, 2009
    /// </date>
    public class ServletRequestWrapper : IServletRequest
    {
        IServletRequest request;

        /// <summary>
        /// Creates a ServletRequest adaptor wrapping the given request object. 
        /// </summary>
        /// <param name="request"></param>
        /// <exception cref="ArgumentException">if the request is null</exception>
        public ServletRequestWrapper(IServletRequest request)
        {
            if (request == null)
            {
                throw new ArgumentException("Request cannot be null");
            }
            this.request = request;
        }

        /// <summary>
        /// Gets or Sets the wrapped request object.
        /// </summary>
        /// <exception cref="ArgumentException">if the request is set to null</exception>
        public IServletRequest Request
        {
            get { return this.request; }
            set 
            {
                if (request == null)
                {
                    throw new ArgumentException("Request cannot be null");
                }
                this.request = value;
            }
        }


        /// <summary>
        /// The default behavior of this method is to call GetAttribute(string name)
        /// on the wrapped request object.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object GetAttribute(string name)
        {
            return request.GetAttribute(name);
        }


        /// <summary>
        /// The default behavior of this property is to get AttributeNames
        /// on the wrapped request object.
        /// </summary>
        public System.Collections.IList AttributeNames
        {
            get { return request.AttributeNames; }
        }


        /// <summary>
        /// The default behavior of this property is to get or set CharacterEncoding
        /// on the wrapped request object.
        /// </summary>
        public string CharacterEncoding
        {
            get
            {
                return request.CharacterEncoding;
            }
            set
            {
                request.CharacterEncoding = value;
            }
        }


        /// <summary>
        /// The default behavior of this property is to get ContentLength
        /// on the wrapped request object.
        /// </summary>
        public int ContentLength
        {
            get { return request.ContentLength; }
        }



        /// <summary>
        /// The default behavior of this property is to get ContentType
        /// on the wrapped request object.
        /// </summary>
        public string ContentType
        {
            get { return request.ContentType; }
        }


        /// <summary>
        /// The default behavior of this property is to get InputStream
        /// on the wrapped request object.
        /// </summary>
        public ServletInputStream InputStream
        {
            get { return request.InputStream; }
        }


        /// <summary>
        /// The default behavior of this method is to return GetParameter(string name)
        /// on the wrapped request object.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetParameter(string name)
        {
            return request.GetParameter(name);
        }


        /// <summary>
        /// The default behavior of this property is to get ParameterMap
        /// on the wrapped request object.
        /// </summary>
        public Dictionary<string, string> ParameterMap
        {
            get { return request.ParameterMap; }
        }

        /// <summary>
        /// The default behavior of this property is to get ParameterNames
        /// on the wrapped request object.
        /// </summary>
        public System.Collections.IList ParameterNames
        {
            get { return request.ParameterNames; }
        }



        /// <summary>
        /// The default behavior of this method is to return GetParameterValues(string name)
        /// on the wrapped request object.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string[] GetParameterValues(string name)
        {
            return request.GetParameterValues(name);
        }


        /// <summary>
        /// The default behavior of this property is to get Protocol
        /// on the wrapped request object.
        /// </summary>
        public string Protocol
        {
            get { return request.Protocol; }
        }

        /// <summary>
        /// The default behavior of this property is to get Scheme
        /// on the wrapped request object.
        /// </summary>
        public string Scheme
        {
            get { return request.Scheme; }
        }
        
        /// <summary>
        /// The default behavior of this property is to get ServerName
        /// on the wrapped request object.
        /// </summary>
        public string ServerName
        {
            get { return request.ServerName; }
        }

        /// <summary>
        /// The default behavior of this property is to get ServerPort
        /// on the wrapped request object.
        /// </summary>
        public int ServerPort
        {
            get { return request.ServerPort; }
        }

        /// <summary>
        /// The default behavior of this property is to get Reader
        /// on the wrapped request object.
        /// </summary>
        public System.IO.BufferedStream Reader
        {
            get { return request.Reader; }
        }

        /// <summary>
        /// The default behavior of this property is to get RemoteAddr
        /// on the wrapped request object.
        /// </summary>
        public string RemoteAddr
        {
            get { return request.RemoteAddr; }
        }

        /// <summary>
        /// The default behavior of this property is to get RemoteHost
        /// on the wrapped request object.
        /// </summary>
        public string RemoteHost
        {
            get { return request.RemoteHost; }
        }
        

        /// <summary>
        /// The default behavior of this method is to return SetAttribute(string name, object o)
        /// on the wrapped request object.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="o"></param>
        public void SetAttribute(string name, object o)
        {
            request.SetAttribute(name, o);
        }


        /// <summary>
        /// The default behavior of this method is to return RemoveAttribute(string name)
        /// on the wrapped request object.
        /// </summary>
        /// <param name="name"></param>
        public void RemoveAttribute(string name)
        {
            request.RemoveAttribute(name);
        }

        /// <summary>
        /// The default behavior of this property is to get Locale
        /// on the wrapped request object.
        /// </summary>
        public System.Globalization.CultureInfo Locale
        {
            get { return request.Locale; }
        }

        /// <summary>
        /// The default behavior of this property is to get Locales
        /// on the wrapped request object.
        /// </summary>
        public System.Collections.IList Locales
        {
            get { return request.Locales; }
        }

        /// <summary>
        /// The default behavior of this property is to get IsSecure
        /// on the wrapped request object.
        /// </summary>
        public bool IsSecure
        {
            get { return request.IsSecure; }
        }

        /// <summary>
        /// The default behavior of this method is to return GetRequestDispatcher(string path)
        /// on the wrapped request object.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public IRequestDispatcher GetRequestDispatcher(string path)
        {
            return request.GetRequestDispatcher(path);
        }

        /// <summary>
        /// The default behavior of this method is to return GetRealPath(string path)
        /// on the wrapped request object.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string GetRealPath(string path)
        {
            return request.GetRealPath(path);
        }

        /// <summary>
        /// The default behavior of this property is to get RemotePort
        /// on the wrapped request object.
        /// </summary>
        public int RemotePort
        {
            get { return request.RemotePort; }
        }

        /// <summary>
        /// The default behavior of this property is to get LocalName
        /// on the wrapped request object.
        /// </summary>
        public string LocalName
        {
            get { return request.LocalName; }
        }

        /// <summary>
        /// The default behavior of this property is to get LocalAddr
        /// on the wrapped request object.
        /// </summary>
        public string LocalAddr
        {
            get { return request.LocalAddr; }
        }

        /// <summary>
        /// The default behavior of this property is to get LocalPort
        /// on the wrapped request object.
        /// </summary>
        public int LocalPort
        {
            get { return request.LocalPort; }
        }

        /// <summary>
        /// The default behavior of this property is to get ServletContext
        /// on the wrapped request object.
        /// </summary>
        public IServletContext ServletContext
        {
            get { return request.ServletContext; }
        }

        /// <summary>
        /// The default behavior of this property is to get ServletResponse
        /// on the wrapped request object.
        /// </summary>
        public IServletResponse ServletResponse
        {
            get { return request.ServletResponse; }
        }

        /// <summary>
        /// The default behavior of this method is to return AddAsyncListener(IAsyncListener listener)
        /// on the wrapped request object.
        /// </summary>
        /// <param name="listener"></param>
        public void AddAsyncListener(IAsyncListener listener)
        {
            request.AddAsyncListener(listener);
        }

        /// <summary>
        /// The default behavior of this method is to return AddAsyncListener(IAsyncListener listener, IServletRequest request, IServletResponse response)
        /// on the wrapped request object.
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="request"></param>
        /// <param name="response"></param>
        public void AddAsyncListener(IAsyncListener listener, IServletRequest request, IServletResponse response)
        {
            request.AddAsyncListener(listener, request, response);
        }

        /// <summary>
        /// The default behavior of this property is to get AsyncContext
        /// on the wrapped request object.
        /// </summary>
        public IAsyncContext AsyncContext
        {
            get { return request.AsyncContext; }
        }

        /// <summary>
        /// The default behavior of this property is to get IsAsyncStarted
        /// on the wrapped request object.
        /// </summary>
        public bool IsAsyncStarted
        {
            get { return request.IsAsyncStarted; }
        }

        /// <summary>
        /// The default behavior of this property is to get IsAsyncSupported
        /// on the wrapped request object.
        /// </summary>
        public bool IsAsyncSupported
        {
            get { return request.IsAsyncSupported; }
        }

        /// <summary>
        /// The default behavior of this property is to set AsyncTimeout
        /// on the wrapped request object.
        /// </summary>
        public long AsyncTimeout
        {
            set { request.AsyncTimeout = value; }
        }

        /// <summary>
        /// The default behavior of this method is to call StartAsync
        /// on the wrapped request object.
        /// </summary>
        /// <returns></returns>
        public IAsyncContext StartAsync()
        {
            return request.StartAsync();
        }

        /// <summary>
        /// The default behavior of this method is to return StartAsync(IServletRequest request, IServletResponse response)
        /// on the wrapped request object.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public IAsyncContext StartAsync(IServletRequest request, IServletResponse response)
        {
            return request.StartAsync(request, response);
        }

        /// <summary>
        /// The default behavior of this property is to get DispatcherType
        /// on the wrapped request object.
        /// </summary>
        public DispatcherType DispatcherType
        {
            get { return request.DispatcherType; }
        }
    }
}

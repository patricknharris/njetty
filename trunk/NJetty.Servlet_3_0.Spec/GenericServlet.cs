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
    /// Defines a generic, protocol-independent
    /// servlet. To write an HTTP servlet for use on the
    /// Web, extend {@link javax.servlet.http.HttpServlet} instead.
    ///
    /// <p><code>GenericServlet</code> implements the <code>Servlet</code>
    /// and <code>ServletConfig</code> interfaces. <code>GenericServlet</code>
    /// may be directly extended by a servlet, although it's more common to extend
    /// a protocol-specific subclass such as <code>HttpServlet</code>.
    ///
    /// <p><code>GenericServlet</code> makes writing servlets
    /// easier. It provides simple versions of the lifecycle methods 
    /// <code>init</code> and <code>destroy</code> and of the methods 
    /// in the <code>ServletConfig</code> interface. <code>GenericServlet</code>
    /// also implements the <code>log</code> method, declared in the
    /// <code>ServletContext</code> interface. 
    ///
    /// <p>To write a generic servlet, you need only
    /// override the abstract <code>service</code> method. 
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// April 17, 2009
    /// </date>
    [Serializable]
    public abstract class GenericServlet : IServlet, IServletConfig
    {
        [NonSerialized]
        private IServletConfig config;

        /// <summary>
        /// Does nothing. All of the servlet initialization
        /// is done by one of the <code>init</code> methods.
        /// </summary>
        public GenericServlet()
        { }

        /// <summary>
        /// Called by the servlet container to indicate to a servlet that the
        /// servlet is being taken out of service.  See {@link Servlet#destroy}.
        /// </summary>
        public void Destroy()
        { }


        /// <summary>
        /// Returns a <code>String</code> containing the value of the named
        /// initialization parameter, or <code>null</code> if the parameter does
        /// not exist.  See {@link ServletConfig#getInitParameter}.
        /// 
        /// This method is supplied for convenience. It gets the 
        /// value of the named parameter from the servlet's 
        /// <code>ServletConfig</code> object.
        /// </summary>
        /// <param name="name">a string specifying the name of the initialization parameter</param>
        /// <returns>a string containing the value of the initialization parameter</returns>
        public string GetInitParameter(string name)
        {
            return ServletConfig.GetInitParameter(name);
        }

        /// <summary>
        /// Returns the names of the servlet's initialization parameters 
        /// as an List of string objects,
        /// or an empty List if the servlet has no
        /// initialization parameters.  See {@link
        /// ServletConfig#getInitParameterNames}.
        ///
        /// <p>This method is supplied for convenience. It gets the 
        /// parameter names from the servlet's <code>ServletConfig</code> object. 
        ///
        ///
        /// returs a List of string objects containing the names of 
        ///	the servlet's initialization parameters
        /// </summary>
        public System.Collections.IList InitParameterNames
        {
            get { return ServletConfig.InitParameterNames; }
        }

        /// <summary>
        /// Returns this servlet's {@link ServletConfig} object.
        /// 
        /// Returns ServletConfig 	the ServletConfig object
        /// that initialized this servlet
        /// </summary>
        public IServletConfig ServletConfig
        {
            get { return config; }
        }


        /// <summary>
        /// Returns a reference to the {@link ServletContext} in which this servlet
        /// is running.  See {@link ServletConfig#getServletContext}.
        ///
        /// This method is supplied for convenience. It gets the 
        /// context from the servlet's ServletConfig object.
        ///
        /// Returns ServletContext 	the ServletContext object
        ///	passed to this servlet by the Init
        ///	method
        /// </summary>
        public IServletContext ServletContext
        {
            get { return ServletConfig.ServletContext; }
        }



        /// <summary>
        /// Returns information about the servlet, such as 
        /// author, version, and copyright. 
        /// By default, this method returns an empty string.  Override this method
        /// to have it return a meaningful value.  See {@link
        /// Servlet#getServletInfo}.
        ///
        ///
        /// return String information about this servlet, by default an
        /// empty string
        /// </summary>
        public string ServletInfo
        {
            get { return string.Empty; }
        }


        /// <summary>
        /// Called by the servlet container to indicate to a servlet that the
        /// servlet is being placed into service.  See {@link Servlet#init}.
        ///
        /// This implementation stores the {@link ServletConfig}
        /// object it receives from the servlet container for later use.
        /// When overriding this form of the method, call 
        /// base.Init(config).
        /// </summary>
        /// <param name="config">
        ///     the ServletConfig object
        ///     that contains configutation
        ///     information for this servlet
        /// </param>
        /// <exception cref="ServletException">
        ///     if an exception occurs that
        ///     interrupts the servlet's normal
        ///     operation
        /// </exception>
        /// <see cref="UnavailableException"/>
        public void Init(IServletConfig config)
        {
            this.config = config;
            this.Init();
        }



        /// <summary>
        /// A convenience method which can be overridden so that there's no need
        /// to call <code>base.Init(config)</code>.
        ///
        /// Instead of overriding {@link #Init(ServletConfig)}, simply override
        /// this method and it will be called by
        /// <code>GenericServlet.init(ServletConfig config)</code>.
        /// The <code>ServletConfig</code> object can still be retrieved via {@link
        /// #getServletConfig}. 
        /// </summary>
        /// <exception cref="ServletException">
        ///     if an exception occurs that
        ///     interrupts the servlet's
        ///     normal operation
        /// </exception>
        public void Init()
        { }


        /// <summary>
        /// Writes the specified message to a servlet log file, prepended by the
        /// servlet's name.  See {@link ServletContext#Log(string)}. 
        /// </summary>
        /// <param name="msg">a string specifying the message to be written to the log file</param>
        public void Log(string msg)
        {
            ServletContext.Log(ServletName + ": " + msg);
        }


        /// <summary>
        /// Writes an explanatory message and a stack trace
        /// for a given <code>Throwable</code> exception
        /// to the servlet log file, prepended by the servlet's name.
        /// See {@link ServletContext#Log(String, Throwable)}.
        /// </summary>
        /// <param name="message">a string that describes the error or exception</param>
        /// <param name="t">the error or exception</param>
        public void Log(string message, Exception t)
        {
            ServletContext.Log(ServletName + ": " + message, t);
        }


        /// <summary>
        /// Called by the servlet container to allow the servlet to respond to
        /// a request.  See {@link Servlet#Service}.
        /// 
        /// This method is declared abstract so subclasses, such as 
        /// <code>HttpServlet</code>, must override it.
        /// </summary>
        /// <param name="req">the IServletRequest object that contains the client's request</param>
        /// <param name="res">the IServletResponse object that will contain the servlet's response</param>
        /// <exception cref="ServletException">
        ///     if an exception occurs that
        ///     interferes with the servlet's
        ///     normal operation occurred
        /// </exception>
        /// <exception cref="System.IO.IOException">
        ///     if an input or output
        ///     exception occurs
        /// </exception>
        public abstract void Service(IServletRequest req, IServletResponse res);

        /// <summary>
        /// Gets the name of this servlet instance.
        /// 
        /// Returns the name of this servlet instances
        /// </summary>
        public string ServletName
        {
            get { return ServletConfig.ServletName; }
        }


    }
}

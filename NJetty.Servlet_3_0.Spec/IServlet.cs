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
    /// Defines methods that all servlets must implement.
    /// 
    /// A servlet is a small Java program that runs within a Web server.
    /// Servlets receive and respond to requests from Web clients,
    /// usually across HTTP, the HyperText Transfer Protocol. 
    /// 
    /// To implement this interface, you can write a generic servlet
    /// that extends
    /// Javax.NServlet.GenericServlet or an HTTP servlet that
    /// extends Javax.NServlet.Http.HttpServlet
    /// 
    /// This interface defines methods to initialize a servlet,
    /// to service requests, and to remove a servlet from the server.
    /// These are known as life-cycle methods and are called in the
    /// following sequence:
    /// - The servlet is constructed, then initialized with the Init method.
    /// - Any calls from clients to the Service method are handled.
    /// - The servlet is taken out of service, then destroyed with the 
    /// Destroy method, then garbage collected and finalized.
    ///
    /// In addition to the life-cycle methods, this interface
    /// provides the ServletConfig property, which the servlet 
    /// can use to get any startup information, and the ServletInfo property
    /// which allows the servlet to return basic information about itself,
    /// such as author, version, and copyright.
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// January 2009
    /// </date>
    public interface IServlet
    {

        /// <summary>
        /// Called by the servlet container to indicate to a servlet that the 
        /// servlet is being placed into service.
        /// 
        /// The servlet container calls the Init
        /// method exactly once after instantiating the servlet.
        /// The Init method must complete successfully
        /// before the servlet can receive any requests.
        /// 
        /// The servlet container cannot place the servlet into service
        /// if the Init method
        /// 1) Throws a ServletException
        /// 2) Does not return within a time period defined by the Web server
        /// </summary>
        /// <param name="config">
        ///     a IServletConfig object
        ///     containing the servlet's
        ///     configuration and initialization parameters
        /// </param>
        /// <exception cref="ServletException">
        ///     if an exception has occurred that
        ///     interferes with the servlet's normal
        ///     operation
        /// </exception>
        /// <see cref="UnavailableException"/>
        /// <see cref="#ServletConfig"/>
        void Init(IServletConfig config); //throws ServletException;


        /// <summary>
        /// Gets a IServletConfig object, which contains
        /// initialization and startup parameters for this servlet.
        /// The IServletConfig object returned is the one 
        /// passed to the Init method. 
        ///
        /// Implementations of this interface are responsible for storing the 
        /// IServletConfig object so that this 
        /// method can return it. The <seealso cref="GenericServlet"/>
        /// class, which implements this interface, already does this.
        /// 
        /// Returns the IServletConfig object that Initializes this servlet
        /// </summary>
        /// <see cref="Init"/>
        IServletConfig ServletConfig
        {
            get;
        }


        
        /// <summary>
        /// Called by the servlet container to allow the servlet to respond to 
        /// a request.
        /// 
        /// This method is only called after the servlet's Init()
        /// method has completed successfully.
        /// 
        /// The status code of the response always should be set for a servlet 
        /// that throws or sends an error.
        ///
        /// 
        /// Servlets typically run inside multithreaded servlet containers
        /// that can handle multiple requests concurrently. Developers must 
        /// be aware to synchronize access to any shared resources such as files,
        /// network connections, and as well as the servlet's class and instance 
        /// variables.
        /// </summary>
        /// <param name="req">
        /// the IServletRequest object that contains
        /// the client's request
        /// </param>
        /// <param name="res">
        /// the IServletResponse object that contains
        /// the servlet's response
        /// </param>
        /// <exception cref="ServletException">
        ///     if an exception occurs that interferes 
        ///     with the servlet's normal operation 
        /// </exception>
        /// <exception cref="System.IO.Exception">if an input or output exception occurs</exception>
        void Service(IServletRequest req, IServletResponse res);



        /// <summary>
        /// Gets information about the servlet, such
        /// as author, version, and copyright.
        /// 
        /// The string that this method returns should
        /// be plain text and not markup of any kind (such as HTML, XML, etc.).
        /// </summary>
        string ServletInfo
        {
            get;
        }



        /// <summary>
        /// Called by the servlet container to indicate to a servlet that the
        /// servlet is being taken out of service.  This method is
        /// only called once all threads within the servlet's
        /// Service method have exited or after a timeout
        /// period has passed. After the servlet container calls this 
        /// method, it will not call the Service method again
        /// on this servlet.
        /// 
        /// This method gives the servlet an opportunity 
        /// to clean up any resources that are being held (for example, memory,
        /// file handles, threads) and make sure that any persistent state is
        /// synchronized with the servlet's current state in memory.
        /// </summary>
        void Destroy();
    }
}

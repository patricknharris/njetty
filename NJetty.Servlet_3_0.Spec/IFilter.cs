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
    /// A filter is an object that performs filtering tasks on either the request to a resource (a servlet or static content), or on the response from a resource, or both.
    /// 
    /// Filters perform filtering in the <code>DoFilter</code> method. Every Filter has access to 
    /// a FilterConfig object from which it can obtain its initialization parameters, a
    /// reference to the ServletContext which it can use, for example, to load resources
    /// needed for filtering tasks.
    /// 
    /// Filters are configured in the deployment descriptor of a web application
    /// 
    /// Examples that have been identified for this design are
    /// 1) Authentication Filters
    /// 2) Logging and Auditing Filters
    /// 3) Image conversion Filters
    /// 4) Data compression Filters
    /// 5) Encryption Filters
    /// 6) Tokenizing Filters
    /// 7) Filters that trigger resource access events
    /// 8) XSL/T filters
    /// 9) Mime-type chain Filter
    /// since: Servlet 2.3
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// January 2009
    /// </date>
    public interface IFilter
    {

        /// <summary>
        /// Called by the web container to indicate to a filter that it is being placed into 
        /// service. The servlet container calls the init method exactly once after instantiating the
        /// filter. The init method must complete successfully before the filter is asked to do any
        /// filtering work.
        /// 
        /// The web container cannot place the filter into service if the init method either
        /// 1.Throws a ServletException
        /// 2.Does not return within a time period defined by the web container
        /// </summary>
        /// <param name="filterConfig"></param>
        /// <exception cref="ServletException"></exception>
        void Init(IFilterConfig filterConfig);


        /// <summary>
        /// The DoFilter method of the Filter is called by the container
        /// each time a request/response pair is passed through the chain due
        /// to a client request for a resource at the end of the chain. The FilterChain passed in to this
        /// method allows the Filter to pass on the request and response to the next entity in the chain.
        /// 
        /// A typical implementation of this method would follow the following pattern:-
        /// 1. Examine the request
        /// 2. Optionally wrap the request object with a custom implementation to 
        /// filter content or headers for input filtering
        /// 3. Optionally wrap the response object with a custom implementation to
        /// filter content or headers for output filtering 
        /// 4. a) <strong>Either</strong> invoke the next entity in the chain using the FilterChain object (<code>chain.doFilter()</code>),
        /// 4. b) <strong>or</strong> not pass on the request/response pair to the next entity in the filter chain to block the request processing
        /// 5. Directly set headers on the response after invocation of the next entity in the filter chain.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <param name="chain"></param>
        /// <exception cref="System.IO.IOException"></exception>
        /// <exception cref="ServletException"></exception>
        void DoFilter(IServletRequest request, IServletResponse response, IFilterChain chain);


        /// <summary>
        /// Called by the web container to indicate to a filter that it is being taken out of service. This 
        /// method is only called once all threads within the filter's doFilter method have exited or after
        /// a timeout period has passed. After the web container calls this method, it will not call the
        /// doFilter method again on this instance of the filter.
        /// 
        /// This method gives the filter an opportunity to clean up any resources that are being held (for
        /// example, memory, file handles, threads) and make sure that any persistent state is synchronized
        /// with the filter's current state in memory.
        /// </summary>
        void Destroy();
    }
}

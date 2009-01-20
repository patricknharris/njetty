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
    /// A ServletRequestListener can be implemented by the developer
    /// interested in being notified of requests coming in and out of
    /// scope in a web component. A request is defined as coming into
    /// scope when it is about to enter the first servlet or filter
    /// in each web application, as going out of scope when it exits
    /// the last servlet or the first filter in the chain.
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// January 2009
    /// </date>
    /// TODO: in java extends EventListener, make dotnet equivalent
    public interface IServletRequestListener
    {

        /// <summary>
        /// The request is about to go out of scope of the web application. 
        /// </summary>
        /// <param name="sre">event containing request</param>
        void RequestDestroyed(ServletRequestEvent sre);

        /// <summary>
        /// The request is about to come into scope of the web application.
        /// </summary>
        /// <param name="sre">event containing request</param>
        void RequestInitialized(ServletRequestEvent sre);

        /// <summary>
        /// Called after suspend
        /// </summary>
        /// <param name="sre">event containing request</param>
        void RequestSuspended(ServletRequestEvent sre);

        /// <summary>
        /// called before resume
        /// </summary>
        /// <param name="sre">event containing request</param>
        void RequestResumed(ServletRequestEvent sre);

        /// <summary>
        /// called after completion 
        /// </summary>
        /// <param name="sre">event containing request</param>
        void RequestCompleted(ServletRequestEvent sre);
    }
}

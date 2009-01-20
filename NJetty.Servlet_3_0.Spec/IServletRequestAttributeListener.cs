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
    /// A ServletRequestAttributeListener can be implemented by the
    /// developer interested in being notified of request attribute
    /// changes. Notifications will be generated while the request
    /// is within the scope of the web application in which the listener
    /// is registered. A request is defined as coming into scope when
    /// it is about to enter the first servlet or filter in each web
    /// application, as going out of scope when it exits the last servlet
    /// or the first filter in the chain.
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// January 2009
    /// </date>
    /// TODO: in java extends EventListener, make dotnet equivalent
    public interface IServletRequestAttributeListener
    {
        /// <summary>
        /// Notification that a new attribute was added to the
        /// servlet request. Called after the attribute is added.
        /// </summary>
        /// <param name="srae"></param>
        void AttributeAdded(ServletRequestAttributeEvent srae);

        /// <summary>
        /// Notification that an existing attribute has been removed from the
        /// servlet request. Called after the attribute is removed.
        /// </summary>
        /// <param name="srae"></param>
        void AttributeRemoved(ServletRequestAttributeEvent srae);

        /// <summary>
        /// Notification that an attribute was replaced on the
        /// servlet request. Called after the attribute is replaced.
        /// </summary>
        /// <param name="srae"></param>
        void AttributeReplaced(ServletRequestAttributeEvent srae);
    }
}

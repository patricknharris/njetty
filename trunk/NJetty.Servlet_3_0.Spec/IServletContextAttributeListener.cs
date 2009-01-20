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
    /// Implementations of this interface receive notifications of
    /// changes to the attribute list on the servlet context of a web application. 
    /// To receive notification events, the implementation class
    /// must be configured in the deployment descriptor for the web application.
    /// <see cref="ServletContextAttributeEvent"/> 
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// January 2009
    /// </date>

    // TODO: in java extends EventListener, make dotnet equivalent
    public interface IServletContextAttributeListener
    {
        /// <summary>
        /// Notification that a new attribute was added to the servlet context. Called after the attribute is added.
        /// </summary>
        /// <param name="scab"></param>
        void AttributeAdded(ServletContextAttributeEvent scab);
        
        /// <summary>
        /// Notification that an existing attribute has been removed from the servlet context. Called after the attribute is removed.
        /// </summary>
        /// <param name="scab"></param>
        void AttributeRemoved(ServletContextAttributeEvent scab);
        
        /// <summary>
        /// Notification that an attribute on the servlet context has been replaced. Called after the attribute is replaced.
        /// </summary>
        /// <param name="scab"></param>
        void AttributeReplaced(ServletContextAttributeEvent scab);
    }
}

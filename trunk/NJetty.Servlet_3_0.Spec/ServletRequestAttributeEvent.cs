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
    /// This is the event class for notifications of changes to the 
    /// attributes of the servlet request in an application.
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// April 21, 2009
    /// </date>
    public class ServletRequestAttributeEvent : ServletRequestEvent
    {
        private string name;
        private object value;

        /// <summary>
        /// Construct a ServletRequestAttributeEvent giving the servlet context
        /// of this web application, the ServletRequest whose attributes are
        /// changing and the name and value of the attribute.
        /// </summary>
        /// <param name="sc">the ServletContext that is sending the event.</param>
        /// <param name="request">the ServletRequest that is sending the event.</param>
        /// <param name="name">the name of the request attribute.</param>
        /// <param name="value">the value of the request attribute.</param>
        public ServletRequestAttributeEvent(IServletContext sc, IServletRequest request, string name, object value)
            : base(sc, request)
        {
            this.name = name;
            this.value = value;
        }

        /// <summary>
        /// Gets the name of the attribute that changed on the ServletRequest.
        /// 
        /// returns the name of the changed request attribute
        /// </summary>
        public string Name
        {
            get { return this.name; }
        }

        /// <summary>
        /// Gets the value of the attribute that has been added, removed or 
        /// replaced. If the attribute was added, this is the value of the 
        /// attribute. If the attribute was removed, this is the value of the 
        /// removed attribute. If the attribute was replaced, this is the old 
        /// value of the attribute.
        /// 
        /// returns the value of the changed request attribute
        /// </summary>
        public object Value
        {
            get { return this.value; }
        }

    }
}

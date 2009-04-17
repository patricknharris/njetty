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
    /// This is the event class for notifications about changes to the attributes of the
    /// servlet context of a web application.
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// April 17, 2009
    /// </date>
    public class ServletContextAttributeEvent : ServletContextEvent
    {
        string name;
        object value;

        /// <summary>
        /// Construct a ServletContextAttributeEvent from the given context for the
        /// given attribute name and attribute value. 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public ServletContextAttributeEvent(IServletContext source, string name, object value) : base(source)
        {
            this.name = name;
            this.value = value;
        }

        /// <summary>
        /// Gets the name of the attribute that changed on the ServletContext.
        /// </summary>
        public string Name
        {
            get { return this.name; }
        }

        
        /// <summary>
        /// Returns the value of the attribute that has been added, removed, or replaced.
        /// If the attribute was added, this is the value of the attribute. If the attribute was
        /// removed, this is the value of the removed attribute. If the attribute was replaced, this
        /// is the old value of the attribute.
        /// </summary>
        public object Value
        {
            get { return this.value; }
        }
    }
}

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
    /// Events of this type are either sent to an object that implements
    /// HttpSessionBindingListener when it is bound or
    /// unbound from a session, or to a HttpSessionAttributeListener 
    /// that has been configured in the deployment descriptor when any attribute is
    /// bound, unbound or replaced in a session.
    /// 
    /// The session binds the object by a call to
    /// <code>HttpSession.setAttribute</code> and unbinds the object
    /// by a call to HttpSession.RemoveAttribute.
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// April 28, 2009
    /// </date>
    public class HttpSessionBindingEvent : HttpSessionEvent
    {

        /// <summary>
        /// The name to which the object is being bound or unbound
        /// </summary>
        string name;

        /// <summary>
        /// The object is being bound or unbound
        /// </summary>
        object value;




        /// <summary>
        /// Constructs an event that notifies an object that it
        /// has been bound to or unbound from a session. 
        /// To receive the event, the object must implement
        /// HttpSessionBindingListener.
        /// </summary>
        /// <param name="session">the session to which the object is bound or unbound</param>
        /// <param name="name">the name with which the object is bound or unbound</param>
        public HttpSessionBindingEvent(IHttpSession session, string name) : base(session)
        {
            this.name = name;
        }

        
        /// <summary>
        /// Constructs an event that notifies an object that it
        /// has been bound to or unbound from a session. 
        /// To receive the event, the object must implement
        /// HttpSessionBindingListener.
        /// </summary>
        /// <param name="session">the session to which the object is bound or unbound</param>
        /// <param name="name">the name with which the object is bound or unbound</param>
        /// <param name="value"></param>
        public HttpSessionBindingEvent(IHttpSession session, string name, object value)
            : base(session)
        {
            this.name = name;
            this.value = value;
        }

        /// <summary>
        /// Return the session that changed.
        /// </summary>
        public new IHttpSession Session
        {
            get { return base.Session; }
        }




        /// <summary>
        /// Returns the name with which the attribute is bound to or
        /// unbound from the session.
        ///
        /// returns a string specifying the name with which
        /// the object is bound to or unbound from
        /// the session
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        
        /// <summary>
        /// Returns the value of the attribute that has been added, removed or replaced.
        /// If the attribute was added (or bound), this is the value of the attribute. If the attribute was
        /// removed (or unbound), this is the value of the removed attribute. If the attribute was replaced, this
        /// is the old value of the attribute.
        /// </summary>
        public object Value
        {
            get { return this.value; }
        }
    }
}

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
    /// Causes an object to be notified when it is bound to
    /// or unbound from a session. The object is notified
    /// by an {@link HttpSessionBindingEvent} object. This may be as a result
    /// of a servlet programmer explicitly unbinding an attribute from a session,
    /// due to a session being invalidated, or due to a session timing out.
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// April 17, 2009
    /// </date>
    public interface IHttpSessionBindingListener // TODO: in java extends java.util.EventListener
    {
        /// <summary>
        /// Notifies the object that it is being bound to
        /// a session and identifies the session. 
        /// </summary>
        /// <param name="evnt">the event that identifies the session </param>
        /// <see cref="#ValueUnbound"/>
        void ValueBound(HttpSessionBindingEvent evnt);


        /// <summary>
        /// Notifies the object that it is being unbound
        /// from a session and identifies the session.
        /// </summary>
        /// <param name="evnt">the event that identifies the session</param>
        /// <see cref="#ValueBound"/>
        void ValueUnbound(HttpSessionBindingEvent evnt);
    }
}

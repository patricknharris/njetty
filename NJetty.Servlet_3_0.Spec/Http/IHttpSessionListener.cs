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
    /// Implementations of this interface are notified of changes to the 
    /// list of active sessions in a web application.
    /// To receive notification events, the implementation class
    /// must be configured in the deployment descriptor for the web application.
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// April 17, 2009
    /// </date>
    public interface IHttpSessionListener // TODO: in java extends java.util.EventListener
    {
        /// <summary>
        /// Notification that a session was created. 
        /// </summary>
        /// <param name="se">the notification event</param>
        void SessionCreated(HttpSessionEvent se);

        /// <summary>
        /// Notification that a session is about to be invalidated.
        /// </summary>
        /// <param name="se">the notification event</param>
        void SessionDestroyed(HttpSessionEvent se);

    }
}

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
    /// This is the event class for notifications about changes to
    /// the servlet context of a web application.
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// April 17, 2009
    /// </date>
    public class ServletContextEvent // TODO: in java extends java.util.EventObject
    {
        /// <summary>
        /// Construct a ServletContextEvent from the given context.
        /// </summary>
        /// <param name="source">the ServletContext that is sending the event.</param>
        IServletContext source;
        public ServletContextEvent(IServletContext source)
        {
            //TODO in java: super(source); modified below
            this.source = source;
        }

        /// <summary>
        /// Gets the ServletContext that changed.
        /// 
        /// returns the ServletContext that sent the event
        /// </summary>
        public IServletContext ServletContext
        {
            get
            {
                // TODO: in java : return (ServletContext)super.getSource(); modified below
                return source;
            }
        }

    }
}

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
using System.Collections;

namespace Javax.NServlet
{

    /// <summary>
    /// A filter configuration object used by a servlet container
    /// to pass information to a filter during initialization.
    /// </summary>
    /// <see cref="IFilter"/>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// January 2009
    /// </date>
    public interface IFilterConfig
    {
        /// <summary>
        /// Gets the filter-name of this filter as defined in the deployment descriptor. 
        /// </summary>
        string FilterName
        {
            get;
        }

        /// <summary>
        /// Gets a reference to the <see cref="IServletContext"/> in which the caller
        /// a <see cref="IServletContext"/> object, used
        /// by the caller to interact with its servlet container
        /// </summary>
        /// <see cref="IServletContext"/>
        IServletContext ServletContext
        {
            get;
        }

        /// <summary>
        /// Returns a string containing the value of the 
        /// named initialization parameter, or null if 
        /// the parameter does not exist.
        /// </summary>
        /// <param name="name">a string specifying the name of the initialization parameter</param>
        /// <returns>a string containing the value of the initialization parameter</returns>
        string GetInitParameter(string name);

        /// <summary>
        /// Gets the names of the filter's initialization parameters
        /// Returns the names of the filter's initialization parameters
        /// as an List of string objects, 
        /// or an empty <code>Enumeration</code> if the filter has
        /// no initialization parameters.
        /// 
        /// Returns an List of string objects containing the names of the filter's 
        /// initialization parameters
        /// </summary>
        IList InitParameterNames
        {
            get;
        }
    }
}

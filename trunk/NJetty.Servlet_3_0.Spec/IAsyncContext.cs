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
using System.Threading;

namespace Javax.NServlet
{

    /// <summary>
    /// TODO: Class/Interface Information here
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// February 2009
    /// </date>
    public interface IAsyncContext
    {
        //string ASYNC_CONTEXT_PATH = "javax.servlet.async.context_path";
        //string ASYNC_PATH_INFO = "javax.servlet.async.path_info";
        //string ASYNC_QUERY_STRING = "javax.servlet.async.query_string";
        //string ASYNC_REQUEST_URI = "javax.servlet.async.request_uri";
        //string ASYNC_SERVLET_PATH = "javax.servlet.async.servlet_path";

        void Complete();

        void Dispatch();

        void Dispatch(IServletContext servletContext, string path);

        IServletRequest Request
        {
            get;
        }

        IServletResponse Response
        {
            get;
        }

        bool HasOriginalRequestAndResonse
        {
            get;
        }

        void Start(ThreadStart run);
    }
}

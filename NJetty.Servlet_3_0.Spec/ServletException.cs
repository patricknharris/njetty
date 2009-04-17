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
    /// Defines a general exception a servlet can throw when it
    /// encounters difficulty.
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// April 17, 2009
    /// </date>
    public class ServletException : Exception
    {
        
        /// <summary>
        /// Constructs a new servlet exception.
        /// </summary>
        public ServletException() : base()
        {
        }

        /// <summary>
        /// Constructs a new servlet exception with the
        /// specified message. The message can be written 
        /// to the server log and/or displayed for the user. 
        /// </summary>
        /// <param name="message">a string specifying the text of the exception message</param>
        public ServletException(string message) : base(message)
        {
            
        }

        /// <summary>
        /// Constructs a new servlet exception when the servlet 
        /// needs to throw an exception and include a message 
        /// about the "root cause" exception that interfered with its 
        /// normal operation, including a description message.
        /// </summary>
        /// <param name="message">a string containing the text of the exception message</param>
        /// <param name="rootCause">
        ///     the Exception
        ///     that interfered with the servlet's
        ///     normal operation, making this servlet
        ///     exception necessary
        /// </param>
        public ServletException(string message, Exception rootCause) : base(message, rootCause)
        {   
        }

        /// <summary>
        /// Constructs a new servlet exception when the servlet 
        /// needs to throw an exception and include a message
        /// about the "root cause" exception that interfered with its
        /// normal operation.  The exception's message is based on the localized
        /// message of the underlying exception.
        /// 
        /// This method calls the <code>getLocalizedMessage</code> method
        /// on the <code>Throwable</code> exception to get a localized exception
        /// message. When subclassing <code>ServletException</code>, 
        /// this method can be overridden to create an exception message 
        /// designed for a specific locale.
        /// </summary>
        /// <param name="rootCause">
        ///     the <code>Throwable</code> exception
        ///     that interfered with the servlet's
        ///     normal operation, making the servlet exception
        ///     necessary
        /// </param>
        public ServletException(Exception rootCause)
            : base(rootCause.Message,rootCause)
        {
            
        }

        /// <summary>
        /// Returns the exception that caused this servlet exception.
        /// 
        /// Returns the Exception that caused this servlet exception
        /// </summary>
        public Exception RootCause
        {
            get { return InnerException; }
        }
    }
}

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
    /// Defines an exception that a servlet or filter throws to indicate
    /// that it is permanently or temporarily unavailable. 
    ///
    /// When a servlet or filter is permanently unavailable, something is wrong
    /// with it, and it cannot handle
    /// requests until some action is taken. For example, a servlet
    /// might be configured incorrectly, or a filter's state may be corrupted.
    /// The component should log both the error and the corrective action
    /// that is needed.
    ///
    /// A servlet or filter is temporarily unavailable if it cannot handle
    /// requests momentarily due to some system-wide problem. For example,
    /// a third-tier server might not be accessible, or there may be 
    /// insufficient memory or disk storage to handle requests. A system
    /// administrator may need to take corrective action.
    ///
    /// Servlet containers can safely treat both types of unavailable
    /// exceptions in the same way. However, treating temporary unavailability
    /// effectively makes the servlet container more robust. Specifically,
    /// the servlet container might block requests to the servlet or filter for a period
    /// of time suggested by the exception, rather than rejecting them until
    /// the servlet container restarts.
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// April 24, 2009
    /// </date>
    public class UnavailableException : ServletException
    {

        IServlet servlet;           // what's unavailable
        bool permanent;         // needs admin action?
        int seconds;           // unavailability estimate


        /// <summary>
        /// 
        /// </summary>
        /// <param name="servlet">the IServlet instance that is unavailable</param>
        /// <param name="msg">a string specifying the descriptive message</param>
        [Obsolete("As of Java Servlet API 2.2, use UnavailableException(string) instead.")]
        public UnavailableException(IServlet servlet, string msg)
            : base(msg)
        {
            this.servlet = servlet;
            permanent = true;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="seconds">
        /// an integer specifying the number of seconds
        /// the servlet expects to be unavailable; if
        /// zero or negative, indicates that the servlet
        /// can't make an estimate
        /// </param>
        /// <param name="servlet">the IServlet that is unavailable</param>
        /// <param name="msg">
        /// a string specifying the descriptive
        /// message, which can be written to a log file or 
        /// displayed for the user.
        /// </param>
        [Obsolete("As of Java Servlet API 2.2, use UnavailableException(string, int) instead.")]
        public UnavailableException(int seconds, IServlet servlet, string msg)
            : base(msg)
        {
            this.servlet = servlet;
            if (seconds <= 0)
                this.seconds = -1;
            else
                this.seconds = seconds;
            permanent = false;
        }


        /// <summary>
        /// Constructs a new exception with a descriptive
        /// message indicating that the servlet is permanently
        /// unavailable.
        /// </summary>
        /// <param name="msg">a string specifying the descriptive message</param>
        public UnavailableException(string msg)
            : base(msg)
        {
            permanent = true;
        }



        /// <summary>
        /// Constructs a new exception with a descriptive message
        /// indicating that the servlet is temporarily unavailable
        /// and giving an estimate of how long it will be unavailable.
        /// 
        /// In some cases, the servlet cannot make an estimate. For
        /// example, the servlet might know that a server it needs is
        /// not running, but not be able to report how long it will take
        /// to be restored to functionality. This can be indicated with
        /// a negative or zero value for the seconds argument.
        /// </summary>
        /// <param name="msg">
        /// a string specifying the
        /// descriptive message, which can be written
        /// to a log file or displayed for the user.
        /// </param>
        /// <param name="seconds">
        /// an integer specifying the number of seconds
        /// the servlet expects to be unavailable; if
        /// zero or negative, indicates that the servlet
        /// can't make an estimate
        /// </param>
        public UnavailableException(String msg, int seconds)
            : base(msg)
        {
            if (seconds <= 0)
                this.seconds = -1;
            else
                this.seconds = seconds;

            permanent = false;
        }

        /// <summary>
        /// Returns a bool indicating
        /// whether the servlet is permanently unavailable.
        /// If so, something is wrong with the servlet, and the
        /// system administrator must take some corrective action.
        ///
        /// returns	true if the servlet is
        ///	permanently unavailable; false
        ///	if the servlet is available or temporarily
        ///	unavailable
        /// </summary>
        public bool IsPermanent
        {
            get { return permanent; }
        }



        /// <summary>
        /// Gets the servlet that is reporting its unavailability. 
        /// returns the IServlet object that is 
        /// throwing the UnavailableException
        /// </summary>
        [Obsolete("As of Java Servlet API 2.2, with no replacement.")]
        public IServlet Servlet
        {
            get { return servlet; }
        }

        /// <summary>
        /// Gets the number of seconds the servlet expects to 
        /// be temporarily unavailable.  
        ///
        /// If this method returns a negative number, the servlet
        /// is permanently unavailable or cannot provide an estimate of
        /// how long it will be unavailable. No effort is
        /// made to correct for the time elapsed since the exception was
        /// first reported.
        ///
        /// returs an integer specifying the number of seconds
        ///	the servlet will be temporarily unavailable,
        ///	or a negative number if the servlet is permanently
        ///	unavailable or cannot make an estimate
        /// </summary>
        public int UnavailableSeconds
        {
            get { return permanent ? -1 : seconds; }
        }
    }
}

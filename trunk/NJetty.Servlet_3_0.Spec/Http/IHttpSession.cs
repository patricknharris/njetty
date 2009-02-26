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

namespace Javax.NServlet.Http
{

    /// <summary>
    /// Provides a way to identify a user across more than one page
    /// request or visit to a Web site and to store information about that user.
    ///
    /// The servlet container uses this interface to create a session
    /// between an HTTP client and an HTTP server. The session persists
    /// for a specified time period, across more than one connection or
    /// page request from the user. A session usually corresponds to one 
    /// user, who may visit a site many times. The server can maintain a 
    /// session in many ways such as using cookies or rewriting URLs.
    ///
    /// This interface allows servlets to 
    /// <ul>
    /// <li>View and manipulate information about a session, such as
    /// the session identifier, creation time, and last accessed time
    /// <li>Bind objects to sessions, allowing user information to persist 
    /// across multiple user connections
    /// </ul>
    /// 
    /// When an application stores an object in or removes an object from a
    /// session, the session checks whether the object implements
    /// {@link HttpSessionBindingListener}. If it does, 
    /// the servlet notifies the object that it has been bound to or unbound 
    /// from the session. Notifications are sent after the binding methods complete. 
    /// For session that are invalidated or expire, notifications are sent after
    /// the session has been invalidated or expired.
    /// 
    /// When container migrates a session between VMs in a distributed container
    /// setting, all session attributes implementing the {@link HttpSessionActivationListener}
    /// interface are notified.
    /// 
    /// A servlet should be able to handle cases in which
    /// the client does not choose to join a session, such as when cookies are
    /// intentionally turned off. Until the client joins the session,
    /// <code>isNew</code> returns <code>true</code>.  If the client chooses 
    /// not to join
    /// the session, <code>getSession</code> will return a different session
    /// on each request, and <code>isNew</code> will always return
    /// <code>true</code>.
    /// 
    /// Session information is scoped only to the current web application
    /// (<code>ServletContext</code>), so information stored in one context
    /// will not be directly visible in another.
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// February 2009
    /// </date>
    public interface IHttpSession
    {

        /// <summary>
        /// Returns the time when this session was created, measured
        /// in milliseconds since midnight January 1, 1970 GMT.
        /// 
        /// returns a <code>long</code> specifying
        /// when this session was created,
        /// expressed in milliseconds since 1/1/1970 GMT
        /// </summary>
        /// <exception cref="InvalidOperationException">if this method is called on an invalidated session</exception>
        long CreationTime
        {
            get;
        }


        /// <summary>
        /// Returns a string containing the unique identifier assigned 
        /// to this session. The identifier is assigned 
        /// by the servlet container and is implementation dependent.
        /// 
        /// returns a string specifying the identifier assigned to this session
        /// </summary>
        /// <exception cref="InvalidOperationException">if this method is called on an invalidated session</exception>
        string Id
        {
            get;
        }




        
        /// <summary>
        /// Returns the last time the client sent a request associated with
        /// this session, as the number of milliseconds since midnight
        /// January 1, 1970 GMT, and marked by the time the container received the request. 
        /// 
        /// Actions that your application takes, such as getting or setting
        /// a value associated with the session, do not affect the access
        /// time.
        /// 
        /// returns a <code>long</code>
        /// representing the last time 
        /// the client sent a request associated
        /// with this session, expressed in 
        /// milliseconds since 1/1/1970 GMT
        /// </summary>
        /// <exception cref="InvalidOperationException">if this method is called on an invalidated session</exception>
        long LastAccessedTime
        {
            get;
        }

        /// <summary>
        /// Returns the ServletContext to which this session belongs.
        /// returns The ServletContext object for the web application 
        /// </summary>
        IServletContext ServletContext
        {
            get;
        }


      

        /// <summary>
        /// getter: 
        /// gets the maximum time interval, in seconds, that 
        /// the servlet container will keep this session open between 
        /// client accesses. After this interval, the servlet container
        /// will invalidate the session.  The maximum time interval can be set
        /// with the set MaxInactiveInterval property.
        /// A negative time indicates the session should never timeout.
        /// 
        /// returns an integer specifying the number of
        /// seconds this session remains open
        /// between client requests
        /// 
        /// setter:
        /// Specifies the time, in seconds, between client requests before the 
        /// servlet container will invalidate this session.  A negative time
        /// indicates the session should never timeout.
        /// </summary>
        int MaxInactiveInterval
        {
            get;
            set;
        }




        [Obsolete("deprecated As of Version 2.1, this method is deprecated and has no replacement. It will be removed in a future version of the Java Servlet API.")]
        IHttpSessionContext SessionContext
        {
            get;
        }

        

        /// <summary>
        /// Returns the object bound with the specified name in this session, or
        /// <code>null</code> if no object is bound under the name. 
        /// </summary>
        /// <param name="name">a string specifying the name of the object</param>
        /// <returns>the object with the specified name</returns>
        /// <exception cref="InvalidOperationException">if this method is called on an invalidated session</exception>
        object GetAttribute(string name);




        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">a string specifying the name of the object</param>
        /// <returns>the object with the specified name</returns>
        /// <exception cref="InvalidOperationException">if this method is called on an invalidated session</exception>
        [Obsolete("As of Version 2.2, this method is replaced by {@link #getAttribute}.")]
        object GetValue(string name);


        /// <summary>
        /// Returns an <code>List</code> of <code>String</code> objects
        /// containing the names of all the objects bound to this session.
        /// 
        /// returns	a List of <code>string</code> objects specifying the
        /// names of all the objects bound to
        /// this session
        /// </summary>
        /// <exception cref="InvalidOperationException">if this method is called on an invalidated session</exception>
        IList AttributeNames
        {
            get;
        }



        /// <summary>
        /// returns	an array of <code>String</code>
        /// objects specifying the
        /// names of all the objects bound to\
        /// this session
        /// </summary>
        /// <exception cref="InvalidOperationException">if this method is called on an invalidated session</exception>
        [Obsolete("deprecated 	As of Version 2.2, this method is replaced by {@link #getAttributeNames}")]
        string[] ValueNames
        {
            get;
        }




        

        /// <summary>
        /// Binds an object to this session, using the name specified.
        /// If an object of the same name is already bound to the session,
        /// the object is replaced.
        /// 
        /// After this method executes, and if the new object
        /// implements <code>HttpSessionBindingListener</code>,
        /// the container calls 
        /// <code>HttpSessionBindingListener.valueBound</code>. The container then   
        /// notifies any <code>HttpSessionAttributeListener</code>s in the web 
        /// application.
        /// 
        /// If an object was already bound to this session of this name
        /// that implements <code>HttpSessionBindingListener</code>, its 
        /// <code>HttpSessionBindingListener.valueUnbound</code> method is called.
        /// 
        /// If the value passed in is null, this has the same effect as calling 
        /// <code>RemoveAttribute()<code>.
        /// </summary>
        /// <param name="name">the name to which the object is bound; cannot be null</param>
        /// <param name="value">the object to be bound</param>
        /// <exception cref="InvalidOperationException">if this method is called on an invalidated session</exception>
        void SetAttribute(string name, object value);



        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">the name to which the object is bound; cannot be null</param>
        /// <param name="value">the object to be bound; cannot be null</param>
        /// <exception cref="InvalidOperationException">if this method is called on an invalidated session</exception>
        [Obsolete("deprecated 	As of Version 2.2, this method is replaced by {@link #setAttribute}")]
        void PutValue(string name, object value);


        
        /// <summary>
        /// Removes the object bound with the specified name from
        /// this session. If the session does not have an object
        /// bound with the specified name, this method does nothing.
        /// 
        /// After this method executes, and if the object
        /// implements <code>HttpSessionBindingListener</code>,
        /// the container calls 
        /// <code>HttpSessionBindingListener.valueUnbound</code>. The container
        /// then notifies any <code>HttpSessionAttributeListener</code>s in the web 
        /// application.
        /// </summary>
        /// <param name="name">the name of the object to remove from this session</param>
        /// <exception cref="InvalidOperationException">if this method is called on an invalidated session</exception>
        void RemoveAttribute(string name);





        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">the name of the object to remove from this session</param>
        /// <exception cref="InvalidOperationException">if this method is called on an invalidated session</exception>
        [Obsolete("deprecated As of Version 2.2, this method is replaced by {@link #removeAttribute}")]
        void RemoveValue(string name);


        

        /// <summary>
        /// Invalidates this session then unbinds any objects bound
        /// to it. 
        /// </summary>
        /// <exception cref="InvalidOperationException">if this method is called on an invalidated session</exception>
        void Invalidate();

       

        /// <summary>
        /// Returns <code>true</code> if the client does not yet know about the
        /// session or if the client chooses not to join the session.  For 
        /// example, if the server used only cookie-based sessions, and
        /// the client had disabled the use of cookies, then a session would
        /// be new on each request.
        /// 
        /// returns true if the server has created a session, but the client has not yet joined
        /// </summary>
        /// <exception cref="InvalidOperationException">if this method is called on an invalidated session</exception>
        bool IsNew
        {
            get;
        }

    }
}

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
using System.Security.Principal;

namespace Javax.NServlet.Http
{

    /// <summary>
    ///  Extends the Javax.NServlet.IServletRequest interface
    ///  to provide request information for HTTP servlets. 
    ///  
    /// The servlet container creates an <code>IHttpServletRequest</code> 
    /// object and passes it as an argument to the servlet's service
    /// methods (<code>DoGet</code>, <code>DoPost</code>, etc).
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// January 2009
    /// </date>
    public interface IHttpServletRequest : IServletRequest
    {


        /// <summary>
        /// Returns the name of the authentication scheme used to protect
        /// the servlet. All servlet containers support basic, form and client 
        /// certificate authentication, and may additionally support digest 
        /// authentication.
        /// If the servlet is not authenticated <code>null</code> is returned. 
        ///
        /// Same as the value of the CGI variable AUTH_TYPE.
        ///
        /// returns one of the static members BASIC_AUTH, 
        /// FORM_AUTH, CLIENT_CERT_AUTH, DIGEST_AUTH
        /// (suitable for == comparison) or
        /// the container-specific string indicating
        /// the authentication scheme, or
        /// <code>null</code> if the request was 
        /// not authenticated.
        /// </summary>
        string AuthType
        {
            get;
        }

        /// <summary>
        /// Returns an array containing all of the <code>Cookie</code>
        /// objects the client sent with this request.
        /// This method returns <code>null</code> if no cookies were sent.
        ///
        /// returns an array of all the <code>Cookies</code>
        /// included with this request, or <code>null</code>
        /// if the request has no cookies
        /// </summary>
        Cookie[] Cookies
        {
            get;
        }


        /// <summary>
        /// Returns the value of the specified request header
        /// as a <code>long</code> value that represents a 
        /// <code>Date</code> object. Use this method with
        /// headers that contain dates, such as
        /// <code>If-Modified-Since</code>. 
        ///
        /// The date is returned as
        /// the number of milliseconds since January 1, 1970 GMT.
        /// The header name is case insensitive.
        ///
        /// If the request did not have a header of the
        /// specified name, this method returns -1. If the header
        /// can't be converted to a date, the method throws
        /// an <code>ArgumentException</code>.
        /// </summary>
        /// <param name="name">a <code>string</code> specifying the name of the header</param>
        /// <returns>
        ///     a <code>long</code> value
        ///	    representing the date specified
        ///     in the header expressed as
        ///     the number of milliseconds
        ///     since January 1, 1970 GMT,
        ///     or -1 if the named header
        ///     was not included with the
        ///     request
        /// </returns>
        /// <exception cref="ArgumentException">If the header value can't be converted to a date</exception>
        long GetDateHeader(string name);



        /// <summary>
        /// Returns the value of the specified request header
        /// as a <code>string</code>. If the request did not include a header
        /// of the specified name, this method returns <code>null</code>.
        /// If there are multiple headers with the same name, this method
        /// returns the first head in the request.
        /// The header name is case insensitive. You can use
        /// this method with any request header. 
        /// </summary>
        /// <param name="name">a <code>string</code> specifying the header name</param>
        /// <returns>
        ///     a <code>string</code> containing the
        ///	    value of the requested
        ///	    header, or <code>null</code>
        ///	    if the request does not
        ///	    have a header of that name
        /// </returns>
        string GetHeader(string name);



        /// <summary>
        /// Returns all the values of the specified request header
        /// as an List of string objects.
        ///
        /// Some headers, such as <code>Accept-Language</code> can be sent
        /// by clients as several headers each with a different value rather than
        /// sending the header as a comma separated list.
        ///
        /// If the request did not include any headers
        /// of the specified name, this method returns an empty
        /// List.
        /// The header name is case insensitive. You can use
        /// this method with any request header.
        /// </summary>
        /// <param name="name">a <code>string</code> specifying the header name</param>
        /// <returns>
        ///     a List containing
        ///     the values of the requested header. If
        ///     the request does not have any headers of
        ///     that name return an empty
        ///     enumeration. If 
        ///     the container does not allow access to
        ///     header information, return null
        /// </returns>
        IList GetHeaders(string name);

        /// <summary>
        /// Returns a List of all the header names
        /// this request contains. If the request has no
        /// headers, this method returns an empty List.
        ///
        /// Some servlet containers do not allow
        /// servlets to access headers using this method, in
        /// which case this method returns <code>null</code>
        ///
        /// returns	a list of all the
        ///	header names sent with this
        ///	request; if the request has
        ///	no headers, an empty List;
        ///	if the servlet container does not
        ///	allow servlets to use this method,
        ///	<code>null</code>
        /// </summary>
        IList HeaderNames
        {
            get;
        }




        /// <summary>
        /// 
        /// Returns the value of the specified request header
        /// as an <code>int</code>. If the request does not have a header
        /// of the specified name, this method returns -1. If the
        /// header cannot be converted to an integer, this method
        /// throws a <code>FormatException</code>.
        ///
        /// The header name is case insensitive.
        /// </summary>
        /// <param name="name">a <code>string</code> specifying the name of a request header</param>
        /// <returns>
        ///     an integer expressing the value 
        ///     of the request header or -1
        ///	    if the request doesn't have a
        ///	    header of this name
        ///	</returns>
        ///	<exception cref="FormatException">If the header value can't be converted to an <code>int</code></exception>
        int GetIntHeader(string name);



        /// <summary>
        /// Returns the name of the HTTP method with which this 
        /// request was made, for example, GET, POST, or PUT.
        /// Same as the value of the CGI variable REQUEST_METHOD.
        ///
        /// returns a <code>string</code> 
        ///	specifying the name
        ///	of the method with which
        ///	this request was made
        /// </summary>
        string Method
        {
            get;
        }




        /// <summary>
        /// Returns any extra path information associated with
        /// the URL the client sent when it made this request.
        /// The extra path information follows the servlet path
        /// but precedes the query string and will start with
        /// a "/" character.
        ///
        /// This method returns <code>null</code> if there
        /// was no extra path information.
        ///
        /// Same as the value of the CGI variable PATH_INFO.
        ///
        ///
        /// returns a <code>string</code>, decoded by the
        ///	web container, specifying 
        ///	extra path information that comes
        ///	after the servlet path but before
        ///	the query string in the request URL;
        ///	or <code>null</code> if the URL does not have
        ///	any extra path information
        /// </summary>
        string PathInfo
        {
            get;
        }


        /// <summary>
        /// Returns any extra path information after the servlet name
        /// but before the query string, and translates it to a real
        /// path. Same as the value of the CGI variable PATH_TRANSLATED.
        ///
        /// If the URL does not have any extra path information,
        /// this method returns <code>null</code> or the servlet container
        /// cannot translate the virtual path to a real path for any reason
        /// (such as when the web application is executed from an archive).
        ///
        /// The web container does not decode this string.
        ///
        ///
        /// returns	a <code>string</code> specifying the
        ///	real path, or <code>null</code> if
        ///	the URL does not have any extra path
        ///	information
        /// </summary>
        string PathTranslated
        {
            get;
        }


        /// <summary>
        /// Returns the portion of the request URI that indicates the context
        /// of the request.  The context path always comes first in a request
        /// URI.  The path starts with a "/" character but does not end with a "/"
        /// character.  For servlets in the default (root) context, this method
        /// returns "". The container does not decode this string.
        ///
        ///
        /// returns	a <code>string</code> specifying the
        ///	portion of the request URI that indicates the context
        ///	of the request
        /// </summary>
        string ContextPath
        {
            get;
        }


        /// <summary>
        /// Returns the query string that is contained in the request
        /// URL after the path. This method returns <code>null</code>
        /// if the URL does not have a query string. Same as the value
        /// of the CGI variable QUERY_STRING. 
        ///
        /// returns	a <code>string</code> containing the query
        ///	string or <code>null</code> if the URL 
        ///	contains no query string. The value is not
        ///	decoded by the container.
        /// </summary>
        string QueryString
        {
            get;
        }



        /// <summary>
        /// Returns the login of the user making this request, if the
        /// user has been authenticated, or <code>null</code> if the user 
        /// has not been authenticated.
        /// Whether the user name is sent with each subsequent request
        /// depends on the browser and type of authentication. Same as the 
        /// value of the CGI variable REMOTE_USER.
        ///
        /// returns a <code>string</code> specifying the login
        ///	of the user making this request, or <code>null</code>
        ///	if the user login is not known
        /// </summary>
        string RemoteUser
        {
            get;
        }


        /// <summary>
        /// Returns a bool indicating whether the authenticated user is included
        /// in the specified logical "role".  Roles and role membership can be
        /// defined using deployment descriptors.  If the user has not been
        /// authenticated, the method returns <code>false</code>.
        /// </summary>
        /// <param name="role">a <code>string</code> specifying the name of the role</param>
        /// <returns>
        ///     a <code>bool</code> indicating whether
        ///	    the user making this request belongs to a given role;
        ///	    <code>false</code> if the user has not been 
        ///	    authenticated
        /// </returns>
        bool IsUserInRole(string role);


        /// <summary>
        /// Returns a <code>java.security.Principal</code> object containing
        /// the name of the current authenticated user. If the user has not been
        /// authenticated, the method returns <code>null</code>.
        ///
        /// returns	a <code>java.security.Principal</code> containing
        ///	the name of the user making this request;
        ///	<code>null</code> if the user has not been 
        ///	authenticated
        /// </summary>
        IPrincipal UserPrincipal //TODO: confirm the equivalent (in java java.security.Principal)
        {
            get;
        }


        /// <summary>
        /// Returns the session ID specified by the client. This may
        /// not be the same as the ID of the current valid session
        /// for this request.
        /// If the client did not specify a session ID, this method returns
        /// <code>null</code>.
        ///
        ///
        /// returns	a <code>string</code> specifying the session
        ///	ID, or <code>null</code> if the request did
        ///	not specify a session ID
        /// </summary>
        /// <see cref="IsRequestedSessionIdValid"/>
        string RequestedSessionId
        {
            get;
        }

        /// <summary>
        /// Returns the part of this request's URL from the protocol
        /// name up to the query string in the first line of the HTTP request.
        /// The web container does not decode this string.
        /// For example:
        ///
        ///
        /// <table summary="Examples of Returned Values">
        /// <tr align=left><th>First line of HTTP request      </th>
        /// <th>     Returned Value</th>
        /// <tr><td>POST /some/path.html HTTP/1.1<td><td>/some/path.html
        /// <tr><td>GET http://foo.bar/a.html HTTP/1.0
        /// <td><td>/a.html
        /// <tr><td>HEAD /xyz?a=b HTTP/1.1<td><td>/xyz
        /// </table>
        ///
        /// To reconstruct an URL with a scheme and host, use
        /// HttpUtils#GetRequestURL.
        ///
        /// returns	a <code>string</code> containing
        ///	the part of the URL from the 
        ///	protocol name up to the query string
        /// </summary>
        /// <see cref="HttpUtils#GetRequestURL"/>
        string RequestURI
        {
            get;
        }

        /// <summary>
        /// Reconstructs the URL the client used to make the request.
        /// The returned URL contains a protocol, server name, port
        /// number, and server path, but it does not include query
        /// string parameters.
        ///
        /// Because this method returns a <code>StringBuffer</code>,
        /// not a string, you can modify the URL easily, for example,
        /// to append query parameters.
        ///
        /// This method is useful for creating redirect messages
        /// and for reporting errors.
        ///
        /// returns	a <code>StringBuffer</code> object containing
        ///	the reconstructed URL
        /// </summary>
        StringBuilder RequestURL
        {
            get;
        }



        /// <summary>
        /// Returns the part of this request's URL that calls
        /// the servlet. This path starts with a "/" character
        /// and includes either the servlet name or a path to
        /// the servlet, but does not include any extra path
        /// information or a query string. Same as the value of
        /// the CGI variable SCRIPT_NAME.
        ///
        /// This method will return an empty string ("") if the
        /// servlet used to process this request was matched using
        /// the "/*" pattern.
        ///
        /// returns	a <code>string</code> containing
        ///	the name or path of the servlet being
        ///	called, as specified in the request URL,
        ///	decoded, or an empty string if the servlet
        ///	used to process the request is matched
        ///	using the "/*" pattern.
        /// </summary>
        string ServletPath
        {
            get;
        }




        /// <summary>
        /// Returns the current <code>HttpSession</code>
        /// associated with this request or, if there is no
        /// current session and <code>create</code> is true, returns 
        /// a new session.
        ///
        /// If <code>create</code> is <code>false</code>
        /// and the request has no valid <code>HttpSession</code>,
        /// this method returns <code>null</code>.
        ///
        /// To make sure the session is properly maintained,
        /// you must call this method before 
        /// the response is committed. If the container is using cookies
        /// to maintain session integrity and is asked to create a new session
        /// when the response is committed, an ArgumentException is thrown.
        /// </summary>
        /// <param name="create">
        ///     <code>true</code> to create
        ///	    a new session for this request if necessary; 
        ///	    <code>false</code> to return <code>null</code>
        ///	    if there's no current session
        /// </param>
        /// <returns>
        ///     the <code>HttpSession</code> associated 
        ///	    with this request or <code>null</code> if
        ///     <code>create</code> is <code>false</code>
        ///	    and the request has no valid session
        /// </returns>
        /// <see cref="#Session"/>
        IHttpSession GetSession(bool create);



        /// <summary>
        /// Returns the current session associated with this request,
        /// or if the request does not have a session, creates one.
        /// 
        /// returns	the <code>HttpSession</code> associated
        ///	with this request
        /// </summary>
        IHttpSession Session
        {
            get;
        }


        /// <summary>
        /// Checks whether the requested session ID is still valid.
        ///
        /// returns	<code>true</code> if this
        ///	request has an id for a valid session
        ///	in the current session context;
        ///	<code>false</code> otherwise
        /// </summary>
        /// <see cref="GetRequestSessionId"/>
        /// <see cref="Session"/>
        /// <see cref="IHttpSessionContext"/>
        bool IsRequestedSessionIdValid
        {
            get;
        }




        /// <summary>
        /// Checks whether the requested session ID came in as a cookie.
        ///
        /// returns	<code>true</code> if the session ID
        ///	came in as a
        ///	cookie; otherwise, <code>false</code>
        ///	<see cref="#Session"/>
        /// </summary>
        bool IsRequestedSessionIdFromCookie
        {
            get;
        }



        /// <summary>
        /// Checks whether the requested session ID came in as part of the 
        /// request URL.
        ///
        /// returns	<code>true</code> if the session ID
        ///	came in as part of a URL; otherwise,
        ///	<code>false</code>
        /// </summary>
        /// <see cref="#Session"/>

        bool IsRequestedSessionIdFromURL
        {
            get;
        }



        [Obsolete("As of Version 2.1 of the Java Servlet API, use IsRequestedSessionIdFromURL instead.")]
        bool IsRequestedSessionIdFromUrl
        {
            get;
        }
    }
}

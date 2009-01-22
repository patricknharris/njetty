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
    ///  Extends the IServletResponse interface to provide HTTP-specific
    ///  functionality in sending a response.  For example, it has methods
    ///  to access HTTP headers and cookies.
    ///  
    /// The servlet container creates an <code>HttpServletResponse</code> object
    /// and passes it as an argument to the servlet's service methods
    /// (<code>DoGet</code>, <code>DoPost</code>, etc).
    /// 
    /// <see cref="Javax.NServlet.IServletResponse"/>
    /// <see cref="HttpServletResponseStatusCode"/>
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// January 2009
    /// </date>
    public interface IHttpServletResponse : IServletResponse
    {

        /// <summary>
        /// Adds the specified cookie to the response.  This method can be called
        /// multiple times to set more than one cookie. 
        /// </summary>
        /// <param name="cookie">the Cookie to return to the client</param>
        void AddCookie(Cookie cookie);

        
        /// <summary>
        /// Returns a bool indicating whether the named response header 
        /// has already been set.
        /// </summary>
        /// <param name="name">the header name</param>
        /// <returns>
        ///     <code>true</code> if the named response header 
        ///     has already been set; 
        ///     <code>false</code> otherwise
        /// </returns>
        bool ContainsHeader(string name);

        /// <summary>
        /// Encodes the specified URL by including the session ID in it,
        /// or, if encoding is not needed, returns the URL unchanged.
        /// The implementation of this method includes the logic to
        /// determine whether the session ID needs to be encoded in the URL.
        /// For example, if the browser supports cookies, or session
        /// tracking is turned off, URL encoding is unnecessary.
        /// 
        /// For robust session tracking, all URLs emitted by a servlet 
        /// should be run through this
        /// method.  Otherwise, URL rewriting cannot be used with browsers
        /// which do not support cookies.
        /// </summary>
        /// <param name="url">the url to be encoded.</param>
        /// <returns>
        ///     the encoded URL if encoding is needed;
        ///     the unchanged URL otherwise.
        /// </returns>
        string EncodeURL(string url);

        
        /// <summary>
        /// Encodes the specified URL for use in the
        /// <code>sendRedirect</code> method or, if encoding is not needed,
        /// returns the URL unchanged.  The implementation of this method
        /// includes the logic to determine whether the session ID
        /// needs to be encoded in the URL.  Because the rules for making
        /// this determination can differ from those used to decide whether to
        /// encode a normal link, this method is separated from the
        /// <code>encodeURL</code> method.
        /// 
        /// All URLs sent to the <code>HttpServletResponse.sendRedirect</code>
        /// method should be run through this method.  Otherwise, URL
        /// rewriting cannot be used with browsers which do not support
        /// cookies.
        /// </summary>
        /// <param name="url">the url to be encoded.</param>
        /// <returns>
        ///     the encoded URL if encoding is needed;
        ///     the unchanged URL otherwise.
        /// </returns>
        /// <see cref="#SendRedirect"/>
        /// <see cref="#EncodeUrl"/>
        string EncodeRedirectURL(string url);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url">the url to be encoded.</param>
        /// <returns>
        ///     the encoded URL if encoding is needed; 
        ///     the unchanged URL otherwise.
        /// </returns>
        [Obsolete("As of version 2.1, use EncodeURL(string url) instead")]
        string EncodeUrl(string url);

        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url">the url to be encoded.</param>
        /// <returns>
        ///     the encoded URL if encoding is needed; 
        ///     the unchanged URL otherwise.
        /// </returns>
        [Obsolete("As of version 2.1, use EncodeRedirectURL(string url) instead")]
        string EncodeRedirectUrl(string url);

       
        /// <summary>
        /// Sends an error response to the client using the specified
        /// status.  The server defaults to creating the
        /// response to look like an HTML-formatted server error page
        /// containing the specified message, setting the content type
        /// to "text/html", leaving cookies and other headers unmodified.
        ///
        /// If an error-page declaration has been made for the web application
        /// corresponding to the status code passed in, it will be served back in 
        /// preference to the suggested msg parameter. 
        ///
        /// If the response has already been committed, this method throws 
        /// an InvalidOperationException.
        /// After using this method, the response should be considered
        /// to be committed and should not be written to.
        /// </summary>
        /// <param name="sc">the error status code</param>
        /// <param name="msg">the descriptive message</param>
        /// <exception cref="System.IO.IOException">If an input or output exception occurs</exception>
        /// <exception cref="InvalidOperationException">If the response was committed</exception>
        void SendError(int sc, string msg);

        
        /// <summary>
        /// Sends an error response to the client using the specified status
        /// code and clearing the buffer. 
        /// If the response has already been committed, this method throws 
        /// an InvalidOperationException.
        /// After using this method, the response should be considered
        /// to be committed and should not be written to.
        /// </summary>
        /// <param name="sc">the error status code</param>
        /// <exception cref="System.IO.IOException">If an input or output exception occurs</exception>
        /// <exception cref="InvalidOperationException">If the response was committed before this method call</exception>
        void SendError(int sc); 

        

        /// <summary>
        /// Sends a temporary redirect response to the client using the
        /// specified redirect location URL.  This method can accept relative URLs;
        /// the servlet container must convert the relative URL to an absolute URL
        /// before sending the response to the client. If the location is relative 
        /// without a leading '/' the container interprets it as relative to
        /// the current request URI. If the location is relative with a leading
        /// '/' the container interprets it as relative to the servlet container root.
        ///
        /// If the response has already been committed, this method throws 
        /// an InvalidOperationException.
        /// After using this method, the response should be considered
        /// to be committed and should not be written to.
        /// </summary>
        /// <param name="location">the redirect location URL</param>
        /// <exception cref="System.IO.IOException">If an input or output exception occurs</exception>
        /// <exception cref="InvalidOperationException">
        ///     If the response was committed or
        //      if a partial URL is given and cannot be converted into a valid URL
        /// </exception>
        void SendRedirect(string location); 

        
        /// <summary>
        /// Sets a response header with the given name and
        /// date-value.  The date is specified in terms of
        /// milliseconds since the epoch.  If the header had already
        /// been set, the new value overwrites the previous one.  The
        /// <code>containsHeader</code> method can be used to test for the
        /// presence of a header before setting its value.
        /// </summary>
        /// <param name="name">the name of the header to set</param>
        /// <param name="date">the assigned date value</param>
        /// <see cref="#ContainsHeader"/>
        /// <see cref="#AddDateHeader"/>
        void SetDateHeader(string name, long date);

        /// <summary>
        /// Adds a response header with the given name and
        /// date-value.  The date is specified in terms of
        /// milliseconds since the epoch.  This method allows response headers 
        /// to have multiple values.
        /// </summary>
        /// <param name="name">the name of the header to set</param>
        /// <param name="date">the additional date value</param>
        void AddDateHeader(string name, long date);

        
        /// <summary>
        /// Sets a response header with the given name and value.
        /// If the header had already been set, the new value overwrites the
        /// previous one.  The <code>containsHeader</code> method can be
        /// used to test for the presence of a header before setting its
        /// value.
        /// </summary>
        /// <param name="name">the name of the header</param>
        /// <param name="value">
        ///     the header value  If it contains octet string,
        ///	    it should be encoded according to RFC 2047
        ///	    (http://www.ietf.org/rfc/rfc2047.txt)
        /// </param>
        /// <see cref="#ContainsHeader"/>
        /// <see cref="#AddHeader"/>
        void SetHeader(string name, string value);

        
        /// <summary>
        /// Adds a response header with the given name and value.
        /// This method allows response headers to have multiple values.
        /// </summary>
        /// <param name="name">the name of the header</param>
        /// <param name="value">
        ///     the additional header value   If it contains
        ///	    octet string, it should be encoded
        ///	    according to RFC 2047
        ///	    (http://www.ietf.org/rfc/rfc2047.txt)
        /// </param>
        /// <see cref="#SetHeader"/>
        void AddHeader(string name, string value);

        /// <summary>
        /// Sets a response header with the given name and
        /// integer value.  If the header had already been set, the new value
        /// overwrites the previous one.  The <code>containsHeader</code>
        /// method can be used to test for the presence of a header before
        /// setting its value.
        /// </summary>
        /// <param name="name">the name of the header</param>
        /// <param name="value">the assigned integer value</param>
        /// <see cref="#ContainsHeader"/>
        /// <see cref="AddIntHeader"/>
        void SetIntHeader(string name, int value);

        
        /// <summary>
        /// Adds a response header with the given name and
        /// integer value.  This method allows response headers to have multiple
        /// values.
        /// </summary>
        /// <param name="name">the name of the header</param>
        /// <param name="value">the assigned integer value</param>
        /// <see cref="#SetIntHeader"/>
        void AddIntHeader(string name, int value);



        
        /// <summary>
        /// Sets the status code for this response.  This method is used to
        /// set the return status code when there is no error (for example,
        /// for the status codes SC_OK or SC_MOVED_TEMPORARILY).  If there
        /// is an error, and the caller wishes to invoke an error-page defined
        /// in the web application, the <code>sendError</code> method should be used
        /// instead.
        /// The container clears the buffer and sets the Location header, preserving
        /// cookies and other headers.
        /// <see cref="#SendError"/>
        /// </summary>
        int Status
        {
            set;
        }

        
        /// <summary>
        /// Sets the status code and message for this response.
        /// </summary>
        /// <param name="sc">the status code</param>
        /// <param name="sm">the status message</param>
        [Obsolete("As of version 2.1, due to ambiguous meaning of the message parameter. To set a status code use <code>SetStatus(int)</code>, to send an error with a description use <code>SendError(int, string)</code>.")]
        void SetStatus(int sc, string sm);

    }
}

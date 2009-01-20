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
using System.IO;
using System.Globalization;

namespace Javax.NServlet
{

    /// <summary>
    /// Defines an object to provide client request information to a servlet.  The
    /// servlet container creates a ServletRequest object and passes
    /// it as an argument to the servlet's service method.
    /// 
    /// A IServletRequest object provides data including
    /// parameter name and values, attributes, and an input stream.
    /// Interfaces that extend IServletRequest can provide
    /// additional protocol-specific data (for example, HTTP data is
    /// provided by IJavax.NServlet.Http.HttpServletRequest.
    /// </summary>
    /// <see cref="IJavax.NServlet.Http.HttpServletRequest"/>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// January 2009
    /// </date>
    public interface IServletRequest
    {
    
        /// <summary>
        /// Returns the value of the named attribute as an object,
        /// or null if no attribute of the given name exists. 
        /// 
        /// Attributes can be set two ways.  The servlet container may set
        /// attributes to make available custom information about a request.
        /// For example, for requests made using HTTPS, the attribute
        /// javax.servlet.request.X509Certificate can be used to
        /// retrieve information on the certificate of the client.  Attributes
        /// can also be set programatically using 
        /// ServletRequest#SetAttribute.  This allows information to be
        /// embedded into a request before a IRequestDispatcher call.
        /// 
        /// Attribute names should follow the same conventions as package
        /// names. This specification reserves names matching java.*,
        /// javax.*, and sun.*. 
        /// </summary>
        /// <param name="name">a string specifying the name of the attribute</param>
        /// <returns>an object containing the value of the attribute, or null if the attribute does not exist</returns>
        object GetAttribute(string name);
        
        /// <summary>
        /// Gets an List containing the
        /// names of the attributes available to this request. 
        /// This method returns an empty Enumeration
        /// if the request has no attributes available to it. 
        /// 
        /// Returns: an List of strings containing the names of the request's attributes
        /// </summary>
        IList AttributeNames
        {
            get;
        }
    

        /// <summary>
        /// get:
        /// Returns the name of the character encoding used in the body of this
        /// request. This method returns null if the request
        /// does not specify a character encoding
        /// 
        /// set:
        /// Overrides the name of the character encoding used in the body of this
        /// request. This method must be called prior to reading request parameters
        /// or reading input using get property Reader.
        /// </summary>
        /// <exception cref="ArgumentException">when setting a not valid encoding</exception>
        string CharacterEncoding
        {
            get;
        }

        /// <summary>
        /// Gets the length, in bytes, of the request body 
        /// and made available by the input stream, or -1 if the
        /// length is not known. For HTTP servlets, same as the value
        /// of the CGI variable CONTENT_LENGTH. 
        /// 
        /// Gets an integer containing the length of the request body or -1 if the length is not known
        /// </summary>
        int ContentLength
        {
            get;
        }

        /// <summary>
        /// Gets the MIME type of the body of the request, or 
        /// null if the type is not known. For HTTP servlets, 
        /// same as the value of the CGI variable CONTENT_TYPE.
        /// 
        /// Gets a string containing the name of the MIME type of the request, or null if the type is not known
        /// </summary>
        string ContentType
        {
            get;
        }

        /// <summary>
        /// Retrieves the body of the request as binary data using
        /// a ServletInputStream.  Either this method or 
        /// get Reader property may be called to read the body, not both.
        /// 
        /// Gets a ServletInputStream object containing the body of the request
        /// </summary>
        /// <exception cref="ArgumentException">if the get Reader property has already been called for this request</exception>
        /// <exception cref="System.IO.IOException">if an input or output exception occurred</exception>
        ServletInputStream InputStream //throws IOException; 
        {
            get;
        }
     
    

        /// <summary>
        /// Returns the value of a request parameter as a string,
        /// or null if the parameter does not exist. Request parameters
        /// are extra information sent with the request.  For HTTP servlets,
        /// parameters are contained in the query string or posted form data.
        ///
        /// You should only use this method when you are sure the
        /// parameter has only one value. If the parameter might have
        /// more than one value, use #GetParameterValues.
        /// 
        /// If you use this method with a multivalued
        /// parameter, the value returned is equal to the first value
        /// in the array returned by getParameterValues.
        /// 
        /// If the parameter data was sent in the request body, such as occurs
        /// with an HTTP POST request, then reading the body directly via get
        /// #InputStream property or get #Reader property can interfere
        /// with the execution of this method.
        /// </summary>
        /// <param name="name">a string specifying the name of the parameter</param>
        /// <returns>a string representing the single value of the parameter</returns>
        /// <see cref="#GetParameterValues"/>
        string GetParameter(string name);
    
     
        /// <summary>
        /// Returns an List of string
        /// objects containing the names of the parameters contained
        /// in this request. If the request has 
        /// no parameters, the method returns an 
        /// empty List. 
        /// 
        /// returns an List of string
        /// objects, each string containing
        /// the name of a request parameter; or an 
        /// empty Enumeration if the
        /// request has no parameters
        /// </summary>
        IList ParameterNames
        {
            get;
        }

        /// <summary>
        /// Returns an array of string objects containing 
        /// all of the values the given request parameter has, or 
        /// null if the parameter does not exist.
        /// 
        /// If the parameter has a single value, the array has a length
        /// of 1.
        /// </summary>
        /// <param name="name">a string containing the name of the parameter whose value is requested</param>
        /// <returns>an array of string objects containing the parameter's values</returns>
        /// <see cref="#GetParameter"/>
        string[] GetParameterValues(string name);
 
    

        /// <summary>
        /// Returns a Dictionary<string, string> of the parameters of this request.
        /// Request parameters
        /// are extra information sent with the request.  For HTTP servlets,
        /// parameters are contained in the query string or posted form data.
        ///
        /// returns an immutable java.util.Map containing parameter names as 
        /// keys and parameter values as Dictinary values. The keys in the parameter
        /// Dictionary are of type string. The values in the parameter Dictionary are of type
        /// string array.
        /// </summary>
        Dictionary<string, string> ParameterMap
        {
            get;
        }

        /// <summary>
        /// Returns the name and version of the protocol the request uses
        /// in the form protocol/majorVersion.minorVersion, for 
        /// example, HTTP/1.1. For HTTP servlets, the value
        /// returned is the same as the value of the CGI variable 
        /// SERVER_PROTOCOL.
        /// 
        /// returns a string containing the protocol 
        /// name and version number
        /// </summary>
        string Protocol
        {
            get;
        }
    
    
    
        /// <summary>
        /// Returns the name of the scheme used to make this request, 
        /// for example,
        /// http, https, or ftp.
        /// Different schemes have different rules for constructing URLs,
        /// as noted in RFC 1738.
        /// 
        /// returns a string containing the name 
        /// of the scheme used to make this request
        /// </summary>
        string Scheme
        {
            get;
        }


        /// <summary>
        /// Returns the host name of the server to which the request was sent.
        /// It is the value of the part before ":" in the Host
        /// header value, if any, or the resolved server name, or the server IP address.
        ///
        /// returns a string containing the name 
        /// of the server
        /// </summary>
        string ServerName
        {
            get;
        }
    
    
    

        /// <summary>
        /// Returns the port number to which the request was sent.
        /// It is the value of the part after ":" in the Host
        /// header value, if any, or the server port where the client connection
        /// was accepted on.
        ///
        /// returns an integer specifying the port number
        /// </summary>
        int ServerPort
        {
            get;
        }
    
    
    

        /// <summary>
        /// Retrieves the body of the request as character data using
        /// a BufferedStream (BufferedReader in java).  The reader translates the character
        /// data according to the character encoding used on the body.
        /// Either this property or get #InputStream property may be called to read the
        /// body, not both.
        /// 
        /// returns a BufferedStream (BufferedReader in java) containing the body of the request
        /// </summary>
        /// <exception cref="ArgumentException">if the character set encoding used is not supported and the text cannot be decoded</exception>
        /// <exception cref="ArgumentException">if get #InputStream method has been called on this request</exception>
        /// <exception cref="System.IO.Exception">if an input or output exception occurred</exception>
        /// <see cref="#InputStream"/>
        BufferedStream Reader
        {
            get;
        }



        /// <summary>
        /// Returns the Internet Protocol (IP) address of the client 
        /// or last proxy that sent the request.
        /// For HTTP servlets, same as the value of the 
        /// CGI variable REMOTE_ADDR.
        ///
        /// returns a string containing the 
        /// IP address of the client that sent the request
        /// </summary>
        string RemoteAddr
        {
            get;
        }



        /// <summary>
        /// Returns the fully qualified name of the client
        /// or the last proxy that sent the request.
        /// If the engine cannot or chooses not to resolve the hostname 
        /// (to improve performance), this method returns the dotted-string form of 
        /// the IP address. For HTTP servlets, same as the value of the CGI variable 
        /// REMOTE_HOST.
        /// 
        /// returns a string containing the fully 
        /// qualified name of the client
        /// </summary>
        string RemoteHost
        {
            get;
        }

        /// <summary>
        /// Stores an attribute in this request.
        /// Attributes are reset between requests.  This method is most
        /// often used in conjunction with {@link RequestDispatcher}.
        ///
        /// Attribute names should follow the same conventions as
        /// package names. Names beginning with java.*,
        /// javax.*, and com.sun.*, are
        /// reserved for use by Sun Microsystems.
        /// 
        /// If the object passed in is null, the effect is the same as
        /// calling {@link #removeAttribute}.
        /// It is warned that when the request is dispatched from the
        /// servlet resides in a different web application by
        /// RequestDispatcher, the object set by this method
        /// may not be correctly retrieved in the caller servlet.
        /// </summary>
        /// <param name="name">a string specifying the name of the attribute</param>
        /// <param name="o">the object to be stored</param>
        void SetAttribute(string name, object o);
    

        /// <summary>
        /// Attribute names should follow the same conventions as
        /// package names. Names beginning with java.*,
        /// javax.*, and com.sun.*, are
        /// reserved for use by Sun Microsystems.
        /// 
        /// Removes an attribute from this request.  This method is not
        /// generally needed as attributes only persist as long as the request
        /// is being handled.
        /// </summary>
        /// <param name="name">a string specifying the name of the attribute to remove</param>
        void RemoveAttribute(string name);
    
    
    
        /// <summary>
        /// Returns the preferred CultureInfo (Locale in java) that the client will 
        /// accept content in, based on the Accept-Language header.
        /// If the client request doesn't provide an Accept-Language header,
        /// this method returns the default locale for the server.
        /// 
        /// returns the preferred CultureInfo (Locale in java) for the client
        /// </summary>
        CultureInfo Locale
        {
            get;
        }

        
        /// <summary>
        /// Returns an List of CultureInfo (Locale in java) objects
        /// indicating, in decreasing order starting with the preferred locale, the
        /// locales that are acceptable to the client based on the Accept-Language
        /// header.
        /// If the client request doesn't provide an Accept-Language header,
        /// this method returns an Enumeration containing one 
        /// Locale, the default locale for the server.
        /// 
        /// returns an List of preferred CultureInfo (Locale in java) objects for the client
        /// </summary>
        IList Locales
        {
            get;
        }
    
        /// <summary>
        /// Returns a bool indicating whether this request was made using a
        /// secure channel, such as HTTPS.
        /// 
        /// returns a bool indicating if the request was made using a
        /// secure channel
        /// </summary>
        bool IsSecure
        {
            get;
        }
    
    

        /// <summary>
        /// Returns a IRequestDispatcher object that acts as a wrapper for
        /// the resource located at the given path.  
        /// A RequestDispatcher object can be used to forward
        /// a request to the resource or to include the resource in a response.
        /// The resource can be dynamic or static.
        /// 
        /// The pathname specified may be relative, although it cannot extend
        /// outside the current servlet context.  If the path begins with 
        /// a "/" it is interpreted as relative to the current context root.  
        /// This method returns null if the servlet container
        /// cannot return a RequestDispatcher.
        /// 
        /// The difference between this method and 
        /// IServletContext#GetRequestDispatcher is that this method can take a
        /// relative path.
        /// </summary>
        /// <param name="path">
        ///     a string specifying the pathname
        ///     to the resource. If it is relative, it must be
        ///     relative against the current servlet.
        /// </param>
        /// <returns>   
        ///     a RequestDispatcher object
        ///     that acts as a wrapper for the resource
        ///     at the specified path, or null
        ///     if the servlet container cannot return a
        ///     RequestDispatcher
        /// </returns>
        /// <see cref="IRequestDispatcher"/>
        /// <see cref="IServletContext#GetRequestDispatcher"/>
        IRequestDispatcher GetRequestDispatcher(string path);
    
    
    

    
        [Obsolete("As of Version 2.1 of the Java Servlet API, use IServletContext#GetRealPath instead")]
        string GetRealPath(string path);
    
    
        /// <summary>
        /// Returns the Internet Protocol (IP) source port of the client
        /// or last proxy that sent the request.
        /// 
        /// returns	an integer specifying the port number 
        /// </summary>
        int RemotePort
        {
            get;
        }


        /// <summary>
        /// Returns the host name of the Internet Protocol (IP) interface on
        /// which the request was received.
        /// 
        /// returns a string containing the host
        /// name of the IP on which the request was received.
        /// </summary>
        string LocalName
        {
            get;
        }

        /// <summary>
        /// Returns the Internet Protocol (IP) address of the interface on
        /// which the request  was received.
        /// 
        /// returns	a string containing the
        /// IP address on which the request was received. 
        /// </summary>
        string LocalAddr
        {
            get;
        }


        /// <summary>
        /// Gets the Internet Protocol (IP) port number of the interface
        /// on which the request was received.
        /// 
        /// returns an integer specifying the port number
        /// </summary>
        int LocalPort
        {
            get;
        }

        /// <summary>
        /// Get the servlet context the request-response pair was last dispatched through. 
        /// returns the latest ServletContext on the dispatch chain.
        /// </summary>
        IServletContext ServletContext
        {
            get;
        }

        /// <summary>
        /// Gets the associated servlet response.
        /// returns the ServletResponse associated with this request.
        /// </summary>
        IServletResponse ServletResponse
        {
            get;
        }

   
        /// <summary>
        /// complete a suspended request.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        void Complete();

        /// <summary>
        /// Suspend request processing.  Must be called by a thread that is processing this request. 
        /// </summary>
        /// <param name="timeoutMilliseconds">new timeout period, in milliseconds</param>
        /// <exception cref="ArgumentException">if called by a thread not processing this request or after error dispatch</exception>
        /// <see cref="#Complete"/>
        /// <see cref="#Resume"/>
        void Suspend(long timeoutMilliseconds);

    
        /// <summary>
        /// Similar to suspend(timeoutMilliseconds) but with a container supplied timeout period.    
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        void Suspend();

    
        /// <summary>
        /// Resume a suspended request  
        /// </summary>
        /// <exception cref="ArgumentException">if the request is not suspended</exception>
        void Resume();

    
        /// <summary>
        /// returns true if the request is suspended
        /// </summary>
        bool isSuspended
        {
            get;
        }

    
        /// <summary>
        /// returns true if the request is resumed
        /// </summary>
        bool IsResumed
        {
            get;
        }
    
        /// <summary>
        /// returns true if the request is timed out
        /// </summary>
        bool IsTimeout
        {
            get;
        }

        /// <summary>
        /// returns true if the request has never been suspended (or resumed)
        /// </summary>
        bool IsInitial
        {
            get;
        }
    }
}

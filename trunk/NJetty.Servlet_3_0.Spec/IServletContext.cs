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
using System.IO;
using System.Collections;

namespace Javax.NServlet
{

    /// <summary>
    /// Defines a set of methods that a servlet uses to communicate with its
    /// servlet container, for example, to get the MIME type of a file, dispatch
    /// requests, or write to a log file.
    ///
    /// There is one context per "web application" per Java Virtual Machine.  (A
    /// "web application" is a collection of servlets and content installed under a
    /// specific subset of the server's URL namespace such as /catalog
    /// and possibly installed via a .war file.) 
    ///
    /// In the case of a web
    /// application marked "distributed" in its deployment descriptor, there will
    /// be one context instance for each virtual machine.  In this situation, the 
    /// context cannot be used as a location to share global information (because
    /// the information won't be truly global).  Use an external resource like 
    /// a database instead.
    ///
    /// The IServletContext object is contained within 
    /// the IServletConfig object, which the Web server provides the
    /// servlet when the servlet is initialized.
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// TODO: date here
    /// </date>
    public interface IServletContext
    {


        /// <summary>
        /// Returns a ServletContext object that 
        /// corresponds to a specified URL on the server.
        ///
        /// This method allows servlets to gain
        /// access to the context for various parts of the server, and as
        /// needed obtain {@link RequestDispatcher} objects from the context.
        /// The given path must be begin with "/", is interpreted relative 
        /// to the server's document root and is matched against the context roots of
        /// other web applications hosted on this container.
        /// 
        /// In a security conscious environment, the servlet container may
        /// return null for a given URL.
        /// </summary>
        /// <param name="uripath">a string specifying the context path of another web application in the container.</param>
        /// <returns>
        ///     the ServletContext object that
        ///		corresponds to the named URL, or null if either
        ///	    none exists or the container wishes to restrict 
        /// 	this access.
        /// </returns>
        /// <see cref="IRequestDispatcher"/>
        IServletContext GetContext(string uripath);


        string ContextPath
        {
            get;
        }


        /// <summary>
        /// Returns the major version of the Java Servlet API that this
        /// servlet container supports. All implementations that comply
        /// with Version 2.4 must have this method
        /// return the integer 2.
        ///
        /// returns:    2
        /// </summary>
        int MajorVersion
        {
            get;
        }



        /// <summary>
        /// Returns the minor version of the Servlet API that this
        /// servlet container supports. All implementations that comply
        /// with Version 2.4 must have this method
        /// return the integer 4.
        ///
        /// returns: 4
        /// </summary>
        int MinorVersion
        {
            get;
        }

        /// <summary>
        /// Returns the MIME type of the specified file, or null if 
        /// the MIME type is not known. The MIME type is determined
        /// by the configuration of the servlet container, and may be specified
        /// in a web application deployment descriptor. Common MIME
        /// types are "text/html" and "image/gif".
        ///
        /// </summary>
        /// <param name="file">a string specifying the name of a file</param>
        /// <returns>a string specifying the file's MIME type</returns>
        string GetMimeType(string file);

        ///////
        /// Returns a directory-like listing of all the paths to resources within the web application whose longest sub-path
        /// matches the supplied path argument. Paths indicating subdirectory paths end with a '/'. The returned paths are all 
        /// relative to the root of the web application and have a leading '/'. For example, for a web application 
        /// containing<br><br>

        /// /welcome.html<br>
        /// /catalog/index.html<br>
        /// /catalog/products.html<br>
        /// /catalog/offers/books.html<br>
        /// /catalog/offers/music.html<br>
        /// /customer/login.jsp<br>
        /// /WEB-INF/web.xml<br>
        /// /WEB-INF/classes/com.acme.OrderServlet.class,<br><br>
        ///
        /// getResourcePaths("/") returns {"/welcome.html", "/catalog/", "/customer/", "/WEB-INF/"}<br>
        /// getResourcePaths("/catalog/") returns {"/catalog/index.html", "/catalog/products.html", "/catalog/offers/"}.<br>



        ///@param path		the partial path used to match the resources,
        ///				which must start with a /
        ///@return a Set containing the directory listing, or null if there are no resources in the web application whose path
        /// begins with the supplied path.

        /// @since Servlet 2.3
        ////

        HashSet<string> GetResourcePaths(string path);



        ///////
        /// Returns a URL to the resource that is mapped to a specified
        /// path. The path must begin with a "/" and is interpreted
        /// as relative to the current context root.
        ///
        /// This method allows the servlet container to make a resource 
        /// available to servlets from any source. Resources 
        /// can be located on a local or remote
        /// file system, in a database, or in a .war file. 
        ///
        /// The servlet container must implement the URL handlers
        /// and URLConnection objects that are necessary
        /// to access the resource.
        ///
        /// This method returns null
        /// if no resource is mapped to the pathname.
        ///
        /// Some containers may allow writing to the URL returned by
        /// this method using the methods of the URL class.
        ///
        /// The resource content is returned directly, so be aware that 
        /// requesting a .jsp page returns the JSP source code.
        /// Use a RequestDispatcher instead to include results of 
        /// an execution.
        ///
        /// This method has a different purpose than
        /// java.lang.Class.getResource,
        /// which looks up resources based on a class loader. This
        /// method does not use class loaders.
        /// 
        /// @param path 				a string specifying
        ///						the path to the resource
        ///
        /// @return 					the resource located at the named path,
        /// 						or null if there is no resource
        ///						at that path
        ///
        /// @exception MalformedURLException 	if the pathname is not given in 
        /// 						the correct form
        ///
        ////

        Uri GetResource(string path); //throws MalformedURLException;
        // by bong see http://j-integra.intrinsyc.com/support/espresso/doc/JavaConn/mapping.html



        ///////
        /// Returns the resource located at the named path as
        /// an InputStream object.
        ///
        /// The data in the InputStream can be 
        /// of any type or length. The path must be specified according
        /// to the rules given in getResource.
        /// This method returns null if no resource exists at
        /// the specified path. 
        /// 
        /// Meta-information such as content length and content type
        /// that is available via getResource
        /// method is lost when using this method.
        ///
        /// The servlet container must implement the URL handlers
        /// and URLConnection objects necessary to access
        /// the resource.
        ///
        /// This method is different from 
        /// java.lang.Class.getResourceAsStream,
        /// which uses a class loader. This method allows servlet containers 
        /// to make a resource available
        /// to a servlet from any location, without using a class loader.
        /// 
        ///
        /// @param path 	a string specifying the path
        ///			to the resource
        ///
        /// @return 		the InputStream returned to the 
        ///			servlet, or null if no resource
        ///			exists at the specified path 
        ///
        ///
        ////

        Stream GetResourceAsStream(string path);




        ///////
        /// 
        /// Returns a {@link RequestDispatcher} object that acts
        /// as a wrapper for the resource located at the given path.
        /// A RequestDispatcher object can be used to forward 
        /// a request to the resource or to include the resource in a response.
        /// The resource can be dynamic or static.
        ///
        /// The pathname must begin with a "/" and is interpreted as relative
        /// to the current context root.  Use getContext to obtain
        /// a RequestDispatcher for resources in foreign contexts.
        /// This method returns null if the ServletContext
        /// cannot return a RequestDispatcher.
        ///
        /// @param path 	a string specifying the pathname
        ///			to the resource
        ///
        /// @return 		a RequestDispatcher object
        ///			that acts as a wrapper for the resource
        ///			at the specified path, or null if 
        ///			the ServletContext cannot return
        ///			a RequestDispatcher
        ///
        /// @see 		RequestDispatcher
        /// @see 		ServletContext#getContext
        ///
        ////

        IRequestDispatcher GetRequestDispatcher(string path);



        ///////
        /// Returns a {@link RequestDispatcher} object that acts
        /// as a wrapper for the named servlet.
        ///
        /// Servlets (and JSP pages also) may be given names via server 
        /// administration or via a web application deployment descriptor.
        /// A servlet instance can determine its name using 
        /// {@link ServletConfig#getServletName}.
        ///
        /// This method returns null if the 
        /// ServletContext
        /// cannot return a RequestDispatcher for any reason.
        ///
        /// @param name 	a string specifying the name
        ///			of a servlet to wrap
        ///
        /// @return 		a RequestDispatcher object
        ///			that acts as a wrapper for the named servlet,
        ///			or null if the ServletContext
        ///			cannot return a RequestDispatcher
        ///
        /// @see 		RequestDispatcher
        /// @see 		ServletContext#getContext
        /// @see 		ServletConfig#getServletName
        ///
        ////

        IRequestDispatcher GetNamedDispatcher(string name);



        /// <summary>
        /// This method was originally defined to retrieve a servlet
        /// from a ServletContext. In this version, this method 
        /// always returns null and remains only to preserve 
        /// binary compatibility. This method will be permanently removed 
        /// in a future version of the Java Servlet API.
        ///
        /// In lieu of this method, servlets can share information using the 
        /// ServletContext class and can perform shared business logic
        /// by invoking methods on common non-servlet classes. 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [Obsolete("As of Java Servlet API 2.1, with no direct replacement.")]
        IServlet GetServlet(string name);



        /// <summary>
        /// This method was originally defined to return an Enumeration
        /// of all the servlets known to this servlet context. In this
        /// version, this method always returns an empty enumeration and
        /// remains only to preserve binary compatibility. This method
        /// will be permanently removed in a future version of the Java
        /// Servlet API.
        /// </summary>
        /// <returns></returns>
        [Obsolete("As of Servlet API 2.0, with no replacement")]
        IList GetServlets();


        /// <summary>
        /// This method was originally defined to return an 
        /// IList
        /// of all the servlet names known to this context. In this version,
        /// this method always returns an empty IList and 
        /// remains only to preserve binary compatibility. This method will 
        /// be permanently removed in a future version of the Java Servlet API.
        /// </summary>
        /// <returns></returns>
        [Obsolete("As of Java Servlet API 2.1, with no replacement.")]
        IList GetServletNames();


        /// <summary>
        /// Writes the specified message to a servlet log file, usually
        /// an event log. The name and type of the servlet log file is 
        /// specific to the servlet container.
        /// </summary>
        /// <param name="msg">string specifiying the message to be written to the log file</param>
        void Log(string msg);

        /// <summary>
        /// This method was originally defined to write an
        /// exception's stack trace and an explanatory error message
        /// to the servlet log file.
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="msg"></param>
        [Obsolete("As of Java Servlet API 2.1, use Log(string message, Exception throwable) instead")]
        void Log(Exception exception, string msg);





        /// <summary>
        /// Writes an explanatory message and a stack trace
        /// for a given Exception exception
        /// to the servlet log file. The name and type of the servlet log 
        /// file is specific to the servlet container, usually an event log. 
        /// </summary>
        /// <param name="message">a string that describes the error or exception</param>
        /// <param name="throwable">the Exception error or exception</param>
        void Log(string message, Exception throwable);



        /// <summary>
        /// Returns a string containing the real path 
        /// for a given virtual path. For example, the path "/index.html"
        /// returns the absolute file path on the server's filesystem would be
        /// served by a request for "http://host/contextPath/index.html",
        /// where contextPath is the context path of this ServletContext..
        ///
        /// The real path returned will be in a form
        /// appropriate to the computer and operating system on
        /// which the servlet container is running, including the
        /// proper path separators. This method returns null
        /// if the servlet container cannot translate the virtual path
        /// to a real path for any reason (such as when the content is
        /// being made available from a .war archive).
        /// </summary>
        /// <param name="path">a string specifying the virtual path</param>
        /// <returns>
        ///     a string specifying the real path,
        ///     or null if the translation cannot be performed
        /// </returns>
        string GetRealPath(string path);


        /// <summary>
        /// Gets the name and version of the servlet container on which
        /// the servlet is running. 
        ///
        /// The form of the returned string is 
        /// servername versionnumber.
        /// For example, the JavaServer Web Development Kit may return the string
        /// JavaServer Web Dev Kit/1.0.
        ///
        /// The servlet container may return other optional information 
        /// after the primary string in parentheses, for example,
        /// JavaServer Web Dev Kit/1.0 (JDK 1.1.6; Windows NT 4.0 x86).
        ///
        /// returns a string containing at least the servlet container name and version number 
        /// </summary>
        string ServerInfo
        {
            get;
        }


        /// <summary>
        /// Gets a string containing the value of the named
        /// context-wide initialization parameter, or null if the 
        /// parameter does not exist.
        ///
        /// This method can make available configuration information useful
        /// to an entire "web application".  For example, it can provide a 
        /// webmaster's email address or the name of a system that holds 
        /// critical data. 
        /// </summary>
        /// <param name="name">a string containing the name of the parameter whose value is requested</param>
        /// <returns>a string containing at least the servlet container name and version number</returns>
        string GetInitParameter(string name);



        /// <summary>
        /// Returns the names of the context's initialization parameters as an
        /// IList of string objects, or an
        /// empty IList if the context has no initialization
        /// 
        /// returns: an IList of string
        /// objects containing the names of the context's
        /// initialization parameters
        /// parameters. 
        /// </summary>
        IList InitParameterNames
        {
            get;
        }




        /// <summary>
        /// Returns the servlet container attribute with the given name, 
        /// or null if there is no attribute by that name.
        /// An attribute allows a servlet container to give the
        /// servlet additional information not
        /// already provided by this interface. See your
        /// server documentation for information about its attributes.
        /// A list of supported attributes can be retrieved using
        /// getAttributeNames.
        ///
        /// The attribute is returned as a java.lang.object
        /// or some subclass.
        /// Attribute names should follow the same convention as package
        /// names. The Java Servlet API specification reserves names
        /// matching java.*, javax.*,
        /// and sun.*.
        /// </summary>
        /// <param name="name">a string specifying the name of the attribute</param>
        /// <returns>
        ///     an object containing the value 
        ///		of the attribute, or null
        ///		if no attribute exists matching the given
        ///		name
        ///	</returns>
        ///	<see cref="IServletContext#AttributeNames"/>
        object GetAttribute(string name);

        /// <summary>
        /// Returns an IList containing the 
        /// attribute names available
        /// within this servlet context. Use the
        /// <see cref="#GetAttribute"/> method with an attribute name
        /// to get the value of an attribute.
        /// 
        /// Returns an IList of attribute names
        /// </summary>
        /// <see cref="#GetAttribute"/>
        IList AttributeNames
        {
            get;
        }

        /// <summary>
        /// Binds an object to a given attribute name in this servlet context. If
        /// the name specified is already used for an attribute, this
        /// method will replace the attribute with the new to the new attribute.
        /// If listeners are configured on the ServletContext the  
        /// container notifies them accordingly.
        /// 
        /// If a null value is passed, the effect is the same as calling 
        /// removeAttribute().
        /// 
        /// Attribute names should follow the same convention as package
        /// names. The Java Servlet API specification reserves names
        /// matching java.*, javax.*, and
        /// sun.*.
        /// </summary>
        /// <param name="name">a string specifying the name of the attribute</param>
        /// <param name="obj">an object representing the attribute to be bound</param>
        void SetAttribute(string name, object obj);



        /// <summary>
        /// Removes the attribute with the given name from 
        /// the servlet context. After removal, subsequent calls to
        /// {@link #getAttribute} to retrieve the attribute's value
        /// will return null.
        /// 
        /// If listeners are configured on the ServletContext the 
        /// container notifies them accordingly.
        /// </summary>
        /// <param name="name">a string specifying the name of the attribute to be removed</param>
        void RemoveAttribute(string name);

        /// <summary>
        /// Gets the name of this web application corresponding to this ServletContext as specified in the deployment
        /// descriptor for this web application by the display-name element.
        ///
        ///
        /// returns: The name of the web application or null if no name has been declared in the deployment descriptor.
        /// </summary>
        string ServletContextName
        {
            get;
        }

        /// <summary>
        /// Add the specified servlet to the context
        /// </summary>
        /// <param name="servletName">servlet's name</param>
        /// <param name="description">description of servlet</param>
        /// <param name="className">class name of servlet</param>
        /// <param name="initParameters">init parameters for servlet</param>
        /// <param name="loadOnStartup">load startup order</param>
        /// <exception cref="ArgumentException">duplicate servletName</exception>
        /// <exception cref="IllegalStateException">this method called after #Initialize</exception>
        void AddServlet(string servletName,
                        string description,
                        string className,
                        Dictionary<string, string> initParameters,
                        int loadOnStartup);

        /// <summary>
        /// Add the specified servlet mappings
        /// </summary>
        /// <param name="servletName">name of servlet mapping applies to</param>
        /// <param name="urlPatterns">url patterns for the mapping</param>
        /// <exception cref="ArgumentException">if urlPatterns are null or empty</exception>
        /// <exception cref="IllegalStateException">if called after #Initialize</exception>
        void AddServletMapping(string servletName,
                               string[] urlPatterns);


        /// <summary>
        /// Add a filter to this context
        /// </summary>
        /// <param name="filterName">name of filter</param>
        /// <param name="description">description of filter</param>
        /// <param name="className">class name of filter</param>
        /// <param name="initParams">init params for filter</param>
        /// <exception cref="ArgumentException">duplicate filter name</exception>
        /// <exception cref="IllegalStateException">if called after #Initialize</exception>
        void AddFilter(string filterName,
                       string description,
                       string className,
                       Dictionary<string, string> initParams);

        /// <summary>
        /// Add a filter mapping to this context
        /// </summary>
        /// <param name="filterName">name of filter mapping applies to</param>
        /// <param name="urlPatterns">url patterns for mapping</param>
        /// <param name="servletNames">servlet names for mapping</param>
        /// <param name="dispatcherTypes">dispatcher types for mapping</param>
        /// <param name="isMatchAfter">before or after xml or annotation specified filter mappings</param>
        /// <exception cref="ArgumentException">bad url patterns</exception>
        /// <exception cref="IllegalStateException">if called after #Initialize</exception>
        void AddFilterMapping(string filterName,
                              string[] urlPatterns,
                              string[] servletNames,
                              HashSet<DispatcherType> dispatcherTypes,
                              bool isMatchAfter); //throws ArgumentException, IllegalStateException;
    }
}

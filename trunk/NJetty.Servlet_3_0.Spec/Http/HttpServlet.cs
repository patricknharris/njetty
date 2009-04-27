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
using System.Reflection;
using System.IO;
using System.Collections;

namespace Javax.NServlet.Http
{

    /// <summary>
    /// Provides an abstract class to be subclassed to create
    /// an HTTP servlet suitable for a Web site. A subclass of
    /// IHttpServlet must override at least 
    /// one method, usually one of these:
    ///
    /// <ul>
    /// <li> <code>doGet</code>, if the servlet supports HTTP GET requests
    /// <li> <code>doPost</code>, for HTTP POST requests
    /// <li> <code>doPut</code>, for HTTP PUT requests
    /// <li> <code>doDelete</code>, for HTTP DELETE requests
    /// <li> <code>init</code> and <code>destroy</code>, 
    /// to manage resources that are held for the life of the servlet
    /// <li> <code>getServletInfo</code>, which the servlet uses to
    /// provide information about itself 
    /// </ul>
    ///
    /// There's almost no reason to override the Service
    /// method. Service handles standard HTTP
    /// requests by dispatching them to the handler methods
    /// for each HTTP request type (the <code>do</code><i>XXX</i>
    /// methods listed above).
    ///
    /// Likewise, there's almost no reason to override the 
    /// <code>doOptions</code> and <code>doTrace</code> methods.
    /// 
    /// Servlets typically run on multithreaded servers,
    /// so be aware that a servlet must handle concurrent
    /// requests and be careful to synchronize access to shared resources.
    /// Shared resources include in-memory data such as
    /// instance or class variables and external objects
    /// such as files, database connections, and network 
    /// connections.
    /// See the
    /// <a href="http://java.sun.com/Series/Tutorial/java/threads/multithreaded.html">
    /// Java Tutorial on Multithreaded Programming</a> for more
    /// information on handling multiple threads in a Java program.
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// April 27, 2009
    /// </date>
    public abstract class HttpServlet //: GenericServlet
    {

        const string METHOD_DELETE = "DELETE";
        const string METHOD_HEAD = "HEAD";
        const string METHOD_GET = "GET";
        const string METHOD_OPTIONS = "OPTIONS";
        const string METHOD_POST = "POST";
        const string METHOD_PUT = "PUT";
        const string METHOD_TRACE = "TRACE";

        const string HEADER_IFMODSINCE = "If-Modified-Since";
        const string HEADER_LASTMOD = "Last-Modified";

        //const string LSTRING_FILE = "javax.servlet.http.LocalStrings";
        //TODO: static ResourceBundle lStrings = ResourceBundle.getBundle(LSTRING_FILE);




        /// <summary>
        /// Does nothing, because this is an abstract class.
        /// </summary>
        public HttpServlet() { }



        /// <summary>
        /// Called by the server (via the Service method) to
        /// allow a servlet to handle a GET request. 
        /// 
        /// Overriding this method to support a GET request also
        /// automatically supports an HTTP HEAD request. A HEAD
        /// request is a GET request that returns no body in the
        /// response, only the request header fields.
        /// 
        /// When overriding this method, read the request data,
        /// write the response headers, get the response's writer or 
        /// output stream object, and finally, write the response data.
        /// It's best to include content type and encoding. When using
        /// a <code>PrintWriter</code> object to return the response,
        /// set the content type before accessing the
        /// <code>PrintWriter</code> object.
        /// 
        /// The servlet container must write the headers before
        /// committing the response, because in HTTP the headers must be sent
        /// before the response body.
        /// 
        /// Where possible, set the Content-Length header (with the
        /// {@link javax.servlet.ServletResponse#setContentLength} method),
        /// to allow the servlet container to use a persistent connection 
        /// to return its response to the client, improving performance.
        /// The content length is automatically set if the entire response fits
        /// inside the response buffer.
        /// 
        /// When using HTTP 1.1 chunked encoding (which means that the response
        /// has a Transfer-Encoding header), do not set the Content-Length header.
        /// 
        /// The GET method should be safe, that is, without
        /// any side effects for which users are held responsible.
        /// For example, most form queries have no side effects.
        /// If a client request is intended to change stored data,
        /// the request should use some other HTTP method.
        /// 
        /// The GET method should also be idempotent, meaning
        /// that it can be safely repeated. Sometimes making a
        /// method safe also makes it idempotent. For example, 
        /// repeating queries is both safe and idempotent, but
        /// buying a product online or modifying data is neither
        /// safe nor idempotent. 
        /// 
        /// If the request is incorrectly formatted, <code>doGet</code>
        /// returns an HTTP "Bad Request" message.
        /// </summary>
        /// <param name="req">
        /// an IHttpServletRequest object that
        /// contains the request the client has made
        /// of the servlet
        /// </param>
        /// <param name="resp">
        /// an IHttpServletResponse object that
        /// contains the response the servlet sends
        /// to the client
        /// </param>
        /// <exception cref="IOException">
        /// if an input or output error is
        /// detected when the servlet handles
        /// the GET request
        /// </exception>
        /// <exception cref="ServletException">
        /// if the request for the GET could not be handled
        /// </exception>
        protected void DoGet(IHttpServletRequest req, IHttpServletResponse resp)
        {
            string protocol = req.Protocol;
            //TODO: string msg = lStrings.getString("http.method_get_not_supported");
            string msg = "HTTP method GET is not supported by this URL";
            if (protocol.EndsWith("1.1"))
            {
                resp.SendError((int)HttpServletResponseStatusCode.SC_METHOD_NOT_ALLOWED, msg);
            }
            else
            {
                resp.SendError((int)HttpServletResponseStatusCode.SC_BAD_REQUEST, msg);
            }
        }




        /// <summary>
        /// Returns the time the IHttpServletRequest
        /// object was last modified,
        /// in milliseconds since midnight January 1, 1970 GMT.
        /// If the time is unknown, this method returns a negative
        /// number (the default).
        /// 
        /// Servlets that support HTTP GET requests and can quickly determine
        /// their last modification time should override this method.
        /// This makes browser and proxy caches work more effectively,
        /// reducing the load on server and network resources.
        /// </summary>
        /// <param name="req">the object that is sent to the servlet</param>
        /// <returns>
        /// a long integer specifying
        /// the time the IHttpServletRequest
        /// object was last modified, in milliseconds
        /// since midnight, January 1, 1970 GMT, or
        /// -1 if the time is not known
        /// </returns>
        protected long GetLastModified(IHttpServletRequest req)
        {
            return -1;
        }




        /// <summary>
        /// Receives an HTTP HEAD request from the protected
        /// Service method and handles the
        /// request.
        /// The client sends a HEAD request when it wants
        /// to see only the headers of a response, such as
        /// Content-Type or Content-Length. The HTTP HEAD
        /// method counts the output bytes in the response
        /// to set the Content-Length header accurately.
        /// 
        /// If you override this method, you can avoid computing
        /// the response body and just set the response headers
        /// directly to improve performance. Make sure that the
        /// DoHead method you write is both safe
        /// and idempotent (that is, protects itself from being
        /// called multiple times for one HTTP HEAD request).
        /// 
        /// If the HTTP HEAD request is incorrectly formatted,
        /// DoHead returns an HTTP "Bad Request"
        /// message.
        /// </summary>
        /// <param name="req">the request object that is passed to the servlet</param>
        /// <param name="resp">the response object that the servlet uses to return the headers to the clien</param>
        /// <exception cref="IOException">if an input or output error occurs</exception>
        /// <exception cref="ServletException">if the request for the HEAD could not be handled</exception>
        protected void DoHead(IHttpServletRequest req, IHttpServletResponse resp)
        {
            NoBodyResponse response = new NoBodyResponse(resp);

            DoGet(req, (IHttpServletResponse)response);
            response.SetContentLength();
        }



        /// <summary>
        /// Called by the server (via the Service method)
        /// to allow a servlet to handle a POST request.
        ///
        /// The HTTP POST method allows the client to send
        /// data of unlimited length to the Web server a single time
        /// and is useful when posting information such as
        /// credit card numbers.
        ///
        /// When overriding this method, read the request data,
        /// write the response headers, get the response's writer or output
        /// stream object, and finally, write the response data. It's best 
        /// to include content type and encoding. When using a
        /// PrintWriter object to return the response, set the 
        /// content type before accessing the PrintWriter object. 
        ///
        /// The servlet container must write the headers before committing the
        /// response, because in HTTP the headers must be sent before the 
        /// response body.
        ///
        /// Where possible, set the Content-Length header (with the
        /// {@link javax.servlet.ServletResponse#setContentLength} method),
        /// to allow the servlet container to use a persistent connection 
        /// to return its response to the client, improving performance.
        /// The content length is automatically set if the entire response fits
        /// inside the response buffer.  
        ///
        /// When using HTTP 1.1 chunked encoding (which means that the response
        /// has a Transfer-Encoding header), do not set the Content-Length header. 
        ///
        /// This method does not need to be either safe or idempotent.
        /// Operations requested through POST can have side effects for
        /// which the user can be held accountable, for example, 
        /// updating stored data or buying items online.
        ///
        /// If the HTTP POST request is incorrectly formatted,
        /// <code>DoPost</code> returns an HTTP "Bad Request" message.
        /// </summary>
        /// <param name="req">
        /// an IHttpServletRequest object that
        /// contains the request the client has made
        /// of the servlet
        /// </param>
        /// <param name="resp">
        /// an IHttpServletResponse object that
        /// contains the response the servlet sends
        /// to the client
        /// </param>
        /// <exception cref="IOException">
        ///     if an input or output error is 
        ///     detected when the servlet handles
        ///     the request
        /// </exception>
        /// <exception cref="ServletException">if the request for the POST could not be handled</exception>
        protected void DoPost(IHttpServletRequest req, IHttpServletResponse resp)
        //throws ServletException, IOException
        {
            string protocol = req.Protocol;
            //TODO: string msg = lStrings.getString("http.method_post_not_supported");
            string msg = "HTTP method POST is not supported by this URL";
            if (protocol.EndsWith("1.1"))
            {
                resp.SendError((int)HttpServletResponseStatusCode.SC_METHOD_NOT_ALLOWED, msg);
            }
            else
            {
                resp.SendError((int)HttpServletResponseStatusCode.SC_BAD_REQUEST, msg);
            }
        }



        /// <summary>
        /// Called by the server (via the Service method)
        /// to allow a servlet to handle a PUT request.
        ///
        /// The PUT operation allows a client to 
        /// place a file on the server and is similar to 
        /// sending a file by FTP.
        ///
        /// When overriding this method, leave intact
        /// any content headers sent with the request (including
        /// Content-Length, Content-Type, Content-Transfer-Encoding,
        /// Content-Encoding, Content-Base, Content-Language, Content-Location,
        /// Content-MD5, and Content-Range). If your method cannot
        /// handle a content header, it must issue an error message
        /// (HTTP 501 - Not Implemented) and discard the request.
        /// For more information on HTTP 1.1, see RFC 2616
        /// <a href="http://www.ietf.org/rfc/rfc2616.txt"></a>.
        ///
        /// This method does not need to be either safe or idempotent.
        /// Operations that <code>doPut</code> performs can have side
        /// effects for which the user can be held accountable. When using
        /// this method, it may be useful to save a copy of the
        /// affected URL in temporary storage.
        ///
        /// If the HTTP PUT request is incorrectly formatted,
        /// DoPut returns an HTTP "Bad Request" message.
        /// </summary>
        /// <param name="req">
        ///     the IHttpServletRequest object that
        ///	    contains the request the client made of
        ///	    the servlet
        /// </param>
        /// <param name="resp">
        ///     the IHttpServletResponse object that
        ///	    contains the response the servlet returns
        ///	    to the client
        ///	</param>
        ///	<exception cref="IOException">
        ///	    if an input or output error occurs
        ///	    while the servlet is handling the
        ///	    PUT request
        /// </exception>
        ///	<exception cref="ServletException">if the request for the PUT cannot be handled</exception>
        protected void DoPut(IHttpServletRequest req, IHttpServletResponse resp)
        {
            string protocol = req.Protocol;
            //TODO: string msg = lStrings.getString("http.method_put_not_supported");
            string msg = "HTTP method PUT is not supported by this URL";
            if (protocol.EndsWith("1.1"))
            {
                resp.SendError((int)HttpServletResponseStatusCode.SC_METHOD_NOT_ALLOWED, msg);
            }
            else
            {
                resp.SendError((int)HttpServletResponseStatusCode.SC_BAD_REQUEST, msg);
            }
        }


       

        /// <summary>
        /// Called by the server (via the Service method)
        /// to allow a servlet to handle a DELETE request.
        ///
        /// The DELETE operation allows a client to remove a document
        /// or Web page from the server.
        /// 
        /// This method does not need to be either safe
        /// or idempotent. Operations requested through
        /// DELETE can have side effects for which users
        /// can be held accountable. When using
        /// this method, it may be useful to save a copy of the
        /// affected URL in temporary storage.
        ///
        /// If the HTTP DELETE request is incorrectly formatted,
        /// DoDelete returns an HTTP "Bad Request"
        /// message.
        /// </summary>
        /// <param name="req">
        ///     the IHttpServletRequest object that
        ///	    contains the request the client made of
        ///	    the servlet
        /// </param>
        /// <param name="resp">
        ///     the IHttpServletResponse object that
        ///	    contains the response the servlet returns
        ///	    to the client
        /// </param>
        /// <exception cref="IOException">
        ///     if an input or output error occurs
        ///     while the servlet is handling the
        ///     DELETE request
        /// </exception>
        /// <exception cref="ServletException">if the request for the DELETE cannot be handled</exception>
        protected void DoDelete(IHttpServletRequest req,
                    IHttpServletResponse resp)
        {
            string protocol = req.Protocol;
            //TODO: string msg = lStrings.getString("http.method_delete_not_supported");
            string msg = "Http method DELETE is not supported by this URL";
            if (protocol.EndsWith("1.1"))
            {
                resp.SendError((int)HttpServletResponseStatusCode.SC_METHOD_NOT_ALLOWED, msg);
            }
            else
            {
                resp.SendError((int)HttpServletResponseStatusCode.SC_BAD_REQUEST, msg);
            }
        }

        /// <summary>
        /// Gets the declared Servlet method
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        private static MethodInfo[] GetAllDeclaredMethods(Type c)
        {

            if (c == typeof(NServlet.Http.HttpServlet))
            {
                return null;
            }

            MethodInfo[] parentMethods = GetAllDeclaredMethods(c.BaseType);
            MethodInfo[] thisMethods = c.GetMethods(BindingFlags.DeclaredOnly);


            if ((parentMethods != null) && (parentMethods.Length > 0))
            {
                MethodInfo[] allMethods =
                    new MethodInfo[parentMethods.Length + thisMethods.Length];
                Array.Copy(parentMethods, 0, allMethods, 0,
                                     parentMethods.Length);
                Array.Copy(thisMethods, 0, allMethods, parentMethods.Length,
                                     thisMethods.Length);

                thisMethods = allMethods;
            }

            return thisMethods;
        }

        

        /// <summary>
        /// Called by the server (via the Service method)
        /// to allow a servlet to handle a OPTIONS request.
        ///
        /// The OPTIONS request determines which HTTP methods 
        /// the server supports and
        /// returns an appropriate header. For example, if a servlet
        /// overrides DoGet, this method returns the
        /// following header:
        ///
        /// <code>Allow: GET, HEAD, TRACE, OPTIONS</code>
        ///
        /// There's no need to override this method unless the
        /// servlet implements new HTTP methods, beyond those 
        /// implemented by HTTP 1.1.
        /// </summary>
        /// <param name="req">
        ///     the IHttpServletRequest object that
        ///     contains the request the client made of
        ///     the servlet</param>
        /// <param name="resp">
        ///     the IHttpServletResponse object that
        ///     contains the response the servlet returns
        ///     to the client
        /// </param>
        /// <exception cref="IOException">
        ///     if an input or output error occurs
        ///     while the servlet is handling the
        ///     OPTIONS request
        /// </exception>
        /// <exception cref="ServletException">
        ///     if the request for the
        ///     OPTIONS cannot be handled
        /// </exception>
        protected void DoOptions(IHttpServletRequest req, IHttpServletResponse resp)
        {
            MethodInfo[] methods = GetAllDeclaredMethods(this.GetType());

            bool ALLOW_GET = false;
            bool ALLOW_HEAD = false;
            bool ALLOW_POST = false;
            bool ALLOW_PUT = false;
            bool ALLOW_DELETE = false;
            bool ALLOW_TRACE = true;
            bool ALLOW_OPTIONS = true;

            for (int i = 0; i < methods.Length; i++)
            {
                MethodInfo m = methods[i];

                if (m.Name.Equals("DoGet"))
                {
                    ALLOW_GET = true;
                    ALLOW_HEAD = true;
                }
                if (m.Name.Equals("DoPost"))
                    ALLOW_POST = true;
                if (m.Name.Equals("DoPut"))
                    ALLOW_PUT = true;
                if (m.Name.Equals("DoDelete"))
                    ALLOW_DELETE = true;

            }

            string allow = null;
            if (ALLOW_GET)
                if (allow == null) allow = METHOD_GET;
            if (ALLOW_HEAD)
                if (allow == null) allow = METHOD_HEAD;
                else allow += ", " + METHOD_HEAD;
            if (ALLOW_POST)
                if (allow == null) allow = METHOD_POST;
                else allow += ", " + METHOD_POST;
            if (ALLOW_PUT)
                if (allow == null) allow = METHOD_PUT;
                else allow += ", " + METHOD_PUT;
            if (ALLOW_DELETE)
                if (allow == null) allow = METHOD_DELETE;
                else allow += ", " + METHOD_DELETE;
            if (ALLOW_TRACE)
                if (allow == null) allow = METHOD_TRACE;
                else allow += ", " + METHOD_TRACE;
            if (ALLOW_OPTIONS)
                if (allow == null) allow = METHOD_OPTIONS;
                else allow += ", " + METHOD_OPTIONS;

            resp.SetHeader("Allow", allow);
        }


        

        /// <summary>
        /// Called by the server (via the Service method)
        /// to allow a servlet to handle a TRACE request.
        ///
        /// A TRACE returns the headers sent with the TRACE
        /// request to the client, so that they can be used in
        /// debugging. There's no need to override this method. 
        /// </summary>
        /// <param name="req">
        ///     the IHttpServletRequest object that
        ///     contains the request the client made of
        ///     the servlet
        /// </param>
        /// <param name="resp">
        ///     the IHttpServletResponse object that
        ///     contains the response the servlet returns
        ///     to the client	
        /// </param>
        /// <exception cref="IOException">
        ///     if an input or output error occurs
        ///     while the servlet is handling the
        ///     TRACE request
        /// </exception>
        /// <exception cref="ServletException">
        ///     if the request for the
        ///     TRACE cannot be handled
        /// </exception>
        protected void DoTrace(IHttpServletRequest req, IHttpServletResponse resp)
        {

            int responseLength;

            string CRLF = "\r\n";
            string responseString = "TRACE " + req.RequestURI +
                " " + req.Protocol;

            foreach (object elem in req.HeaderNames)
            {
                string headerName = (string)elem;
                responseString += CRLF + headerName + ": " +
                req.GetHeader(headerName);
            }

            responseString += CRLF;

            responseLength = responseString.Length;

            resp.ContentType = "message/http";
            resp.ContentLength = responseLength;
            ServletOutputStream output = resp.OutputStream;
            output.Print(responseString);
            output.Close();
            return;
        }


        

        /// <summary>
        /// Receives standard HTTP requests from the public
        /// Service method and dispatches
        /// them to the <code>Do</code><i>XXX</i> methods defined in 
        /// this class. This method is an HTTP-specific version of the 
        /// {@link javax.servlet.Servlet#Service} method. There's no
        /// need to override this method.
        /// </summary>
        /// <param name="req">
        ///     the IHttpServletRequest object that
        ///     contains the request the client made of
        ///     the servlet
        /// </param>
        /// <param name="resp">
        ///     the IHttpServletResponse object that
        ///     contains the response the servlet returns
        ///     to the client
        /// </param>
        /// <exception cref="IOException">
        ///     if an input or output error occurs
        ///     while the servlet is handling the
        ///     HTTP request
        /// </exception>
        /// <exception cref="ServletException">
        /// if the HTTP request cannot be handled
        /// </exception>
        protected void Service(IHttpServletRequest req, IHttpServletResponse resp)
        //throws ServletException, IOException
        {
            string method = req.Method;

            if (method.Equals(METHOD_GET))
            {
                long lastModified = GetLastModified(req);
                if (lastModified == -1)
                {
                    // servlet doesn't support if-modified-since, no reason
                    // to go through further expensive logic
                    DoGet(req, resp);
                }
                else
                {
                    long ifModifiedSince = req.GetDateHeader(HEADER_IFMODSINCE);
                    if (ifModifiedSince < (lastModified / 1000 * 1000))
                    {
                        // If the servlet mod time is later, call doGet()
                        // Round down to the nearest second for a proper compare
                        // A ifModifiedSince of -1 will always be less
                        MaybeSetLastModified(resp, lastModified);
                        DoGet(req, resp);
                    }
                    else
                    {
                        resp.Status = (int)HttpServletResponseStatusCode.SC_NOT_MODIFIED;
                    }
                }

            }
            else if (method.Equals(METHOD_HEAD))
            {
                long lastModified = GetLastModified(req);
                MaybeSetLastModified(resp, lastModified);
                DoHead(req, resp);

            }
            else if (method.Equals(METHOD_POST))
            {
                DoPost(req, resp);

            }
            else if (method.Equals(METHOD_PUT))
            {
                DoPut(req, resp);

            }
            else if (method.Equals(METHOD_DELETE))
            {
                DoDelete(req, resp);

            }
            else if (method.Equals(METHOD_OPTIONS))
            {
                DoOptions(req, resp);

            }
            else if (method.Equals(METHOD_TRACE))
            {
                DoTrace(req, resp);

            }
            else
            {
                //
                // Note that this means NO servlet supports whatever
                // method was requested, anywhere on this server.
                //

                //TODO: string errMsg = lStrings.getString("http.method_not_implemented");
                string errMsg = "Method {0} is not defined in RFC 2068 and is not supported by the Servlet API";
                object[] errArgs = new object[1];
                errArgs[0] = method;
                errMsg = string.Format(errMsg, errArgs);

                resp.SendError((int)HttpServletResponseStatusCode.SC_NOT_IMPLEMENTED, errMsg);
            }
        }





        /// <summary>
        /// Sets the Last-Modified entity header field, if it has not
        /// already been set and if the value is meaningful.  Called before
        /// doGet, to ensure that headers are set before response data is
        /// written.  A subclass might have set this header already, so we
        /// check.
        /// </summary>
        /// <param name="resp"></param>
        /// <param name="lastModified"></param>
        private void MaybeSetLastModified(IHttpServletResponse resp,
                          long lastModified)
        {
            if (resp.ContainsHeader(HEADER_LASTMOD))
                return;
            if (lastModified >= 0)
                resp.SetDateHeader(HEADER_LASTMOD, lastModified);
        }




        /// <summary>
        /// Dispatches client requests to the protected
        /// Service method. There's no need to
        /// override this method.
        /// </summary>
        /// <param name="req">
        ///     the IHttpServletRequest object that
        ///     contains the request the client made of
        ///     the servlet
        /// </param>
        /// <param name="res">
        ///     the IHttpServletResponse object that
        ///     contains the response the servlet returns
        ///     to the client	
        /// </param>
        /// <exception cref="IOException">
        ///     if an input or output error occurs
        ///     while the servlet is handling the
        ///     HTTP request
        /// </exception>
        /// <exception cref="ServletException">
        ///     if the HTTP request cannot be handled
        /// </exception>
        public void Service(IServletRequest req, IServletResponse res)
        {
            IHttpServletRequest request;
            IHttpServletResponse response;

            try
            {
                request = (IHttpServletRequest)req;
                response = (IHttpServletResponse)res;
            }
            catch (InvalidCastException)
            {
                throw new ServletException("non-HTTP request or response");
            }
            Service(request, response);
        }


    }




    /// <summary>
    /// A response that includes no body, for use in (dumb) "HEAD" support.
    /// This just swallows that body, counting the bytes in order to set
    /// the content length appropriately.  All other methods delegate directly
    /// to the HTTP Servlet Response object used to construct this one.
    /// </summary>
    class NoBodyResponse : HttpServletResponseWrapper
    {
        NoBodyOutputStream noBody;
        TextWriter writer;
        bool didSetContentLength;

        // file private
        public NoBodyResponse(IHttpServletResponse r)
            : base(r)
        {
            noBody = new NoBodyOutputStream();
        }

        // file private
        public void SetContentLength()
        {
            if (!didSetContentLength)
                base.ContentLength = noBody.ContentLength;
        }


        // SERVLET RESPONSE interface methods

        public new int ContentLength
        {
            set
            {
                base.ContentLength = value;
                didSetContentLength = true;
            }
        }

        public new ServletOutputStream OutputStream //throws IOException
        {
            get { return noBody; }
        }

        public new TextWriter Writer //throws UnsupportedEncodingException
        {
            get
            {
                if (writer == null)
                {
                    //TODO: do the equivalent java code of below
                    //OutputStreamWriter w;
                    //w = new OutputStreamWriter(noBody, CharacterEncoding);
                    //writer = new PrintWriter(w);

                    //TODO: below is just temporary for the java code above
                    //noBody.Encoding = CharacterEncoding;
                    writer = noBody;


                }
                return writer;
            }
        }

    }


    
    /// <summary>
    /// Servlet output stream that gobbles up all its data.
    /// </summary>
    class NoBodyOutputStream : ServletOutputStream
    {

        //const string LSTRING_FILE = "javax.servlet.http.LocalStrings";
        //TODO: private static ResourceBundle lStrings = ResourceBundle.getBundle(LSTRING_FILE);

        int contentLength = 0;

        // file private
        public NoBodyOutputStream() { }

        // file private
        public int ContentLength
        {
            get { return contentLength; }
        }

        public new void Write(int b)
        {
            contentLength++;
        }

        public void Write(byte[] buf, int offset, int len)
        //throws IOException
        {
            if (len >= 0)
            {
                contentLength += len;
            }
            else
            {
                // XXX
                // isn't this really an IllegalArgumentException?

                //string msg = lStrings.getString("err.io.negativelength");
                //string msg = "Negative Length given in write method";
                throw new IOException("negative length");
            }
        }

    }
}

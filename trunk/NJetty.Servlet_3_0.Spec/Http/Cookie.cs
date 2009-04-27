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
    /// Creates a cookie, a small amount of information sent by a servlet to 
    /// a Web browser, saved by the browser, and later sent back to the server.
    /// A cookie's value can uniquely 
    /// identify a client, so cookies are commonly used for session management.
    /// 
    /// A cookie has a name, a single value, and optional attributes
    /// such as a comment, path and domain qualifiers, a maximum age, and a
    /// version number. Some Web browsers have bugs in how they handle the 
    /// optional attributes, so use them sparingly to improve the interoperability 
    /// of your servlets.
    ///
    /// <p>The servlet sends cookies to the browser by using the
    /// {@link HttpServletResponse#addCookie} method, which adds
    /// fields to HTTP response headers to send cookies to the 
    /// browser, one at a time. The browser is expected to 
    /// support 20 cookies for each Web server, 300 cookies total, and
    /// may limit cookie size to 4 KB each.
    /// 
    /// <p>The browser returns cookies to the servlet by adding 
    /// fields to HTTP request headers. Cookies can be retrieved
    /// from a request by using the {@link HttpServletRequest#getCookies} method.
    /// Several cookies might have the same name but different path attributes.
    /// 
    /// Cookies affect the caching of the Web pages that use them. 
    /// HTTP 1.0 does not cache pages that use cookies created with
    /// this class. This class does not support the cache control
    /// defined with HTTP 1.1.
    ///
    /// This class supports both the Version 0 (by Netscape) and Version 1 
    /// (by RFC 2109) cookie specifications. By default, cookies are
    /// created using Version 0 to ensure the best interoperability.
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// April 25, 2009
    /// </date>
    public class Cookie : ICloneable
    {

        //const string LSTRING_FILE = "javax.servlet.http.LocalStrings";
        //static ResourceBundle lStrings = ResourceBundle.getBundle(LSTRING_FILE);

        //
        // The value of the cookie itself.
        //

        string name;	// NAME= ... "$Name" style is reserved
        string value;	// value of NAME

        //
        // Attributes encoded in the header's cookie fields.
        //

        string comment;	// ;Comment=VALUE ... describes cookie's use
        // ;Discard ... implied by maxAge < 0
        string domain;	// ;Domain=VALUE ... domain that sees cookie
        int maxAge = -1;	// ;Max-Age=VALUE ... cookies auto-expire
        string path;	// ;Path=VALUE ... URLs that see the cookie
        bool secure;	// ;Secure ... e.g. use SSL
        int version = 0;	// ;Version=1 ... means RFC 2109++ style
        bool httpOnly;


        /// <summary>
        /// Constructs a cookie with a specified name and value.
        /// 
        /// The name must conform to RFC 2109. That means it can contain 
        /// only ASCII alphanumeric characters and cannot contain commas, 
        /// semicolons, or white space or begin with a $ character. The cookie's
        /// name cannot be changed after creation.
        /// 
        /// The value can be anything the server chooses to send. Its
        /// value is probably of interest only to the server. The cookie's
        /// value can be changed after creation with the
        /// Set Value property.
        /// 
        /// By default, cookies are created according to the Netscape
        /// cookie specification. The version can be changed with the 
        /// set Version property.
        /// </summary>
        /// <param name="name">a string specifying the name of the cookie</param>
        /// <param name="value">a string specifying the value of the cookie</param>
        /// <exception cref="ArgumentException">
        ///     if the cookie name contains illegal characters
        ///     (for example, a comma, space, or semicolon)
        ///     or it is one of the tokens reserved for use
        ///     by the cookie protocol
        /// </exception>
        public Cookie(string name, string value)
        {
            if (!IsToken(name)
                || name.Equals("Comment", StringComparison.OrdinalIgnoreCase)	// rfc2019
                || name.Equals("Discard", StringComparison.OrdinalIgnoreCase)	// 2019++
                || name.Equals("Domain", StringComparison.OrdinalIgnoreCase)
                || name.Equals("Expires", StringComparison.OrdinalIgnoreCase)	// (old cookies)
                || name.Equals("Max-Age", StringComparison.OrdinalIgnoreCase)	// rfc2019
                || name.Equals("Path", StringComparison.OrdinalIgnoreCase)
                || name.Equals("Secure", StringComparison.OrdinalIgnoreCase)
                || name.Equals("Version", StringComparison.OrdinalIgnoreCase)
                || name.StartsWith("$")
                )
            {
                //TODO in java: string errMsg = lStrings.getString("err.cookie_name_is_token");
                string errMsg = "Cookie name \"{0}\" is a reserved token";
                object[] errArgs = new object[1];
                errArgs[0] = name;
                errMsg = string.Format(errMsg, errArgs);
                throw new ArgumentException(errMsg);
            }

            this.name = name;
            this.value = value;
        }







        /// <summary>
        /// Getter:
        /// Returns the comment describing the purpose of this cookie, or
        /// null if the cookie has no comment.
        ///
        /// returns a string containing the comment,
        /// or null if none
        /// 
        /// Setter:
        /// Specifies a comment that describes a cookie's purpose.
        /// The comment is useful if the browser presents the cookie \
        /// to the user. Comments
        /// are not supported by Netscape Version 0 cookies.
        /// </summary>
        public string Comment
        {
            get { return comment; }
            set { comment = value; }
        }


        /// <summary>
        /// Specifies the domain within which this cookie should be presented.
        /// 
        /// The form of the domain name is specified by RFC 2109. A domain
        /// name begins with a dot (.foo.com) and means that
        /// the cookie is visible to servers in a specified Domain Name System
        /// (DNS) zone (for example, www.foo.com, but not 
        /// a.b.foo.com. By default, cookies are only returned
        /// to the server that sent them.
        /// 
        /// Getter:
        /// Returns the domain name set for this cookie. The form of 
        /// the domain name is set by RFC 2109.
        /// 
        /// returns a string containing the domain name
        /// 
        /// Setter:
        /// 
        /// sets a value (pattern) string containing the domain name
        /// within which this cookie is visible;
        /// form is according to RFC 2109
        /// </summary>
        public string Domain
        {
            get { return domain; }
            set { domain = value; }
        }



        /// <summary>
        /// Getter:
        /// Returns the maximum age of the cookie, specified in seconds,
        /// By default, -1 indicating the cookie will persist
        /// until browser shutdown.
        /// 
        /// returns an integer specifying the maximum age of the
        /// cookie in seconds; if negative, means
        /// the cookie persists until browser shutdown
        /// 
        /// Setter:
        /// Sets the maximum age of the cookie in seconds.
        /// 
        /// A positive value indicates that the cookie will expire
        /// after that many seconds have passed. Note that the value is
        /// the maximum age when the cookie will expire, not the cookie's
        /// current age.
        /// 
        /// A negative value means
        /// that the cookie is not stored persistently and will be deleted
        /// when the Web browser exits. A zero value causes the cookie
        /// to be deleted.
        /// 
        /// value to set: an integer specifying the maximum age of the
        /// cookie in seconds; if negative, means
        /// the cookie is not stored; if zero, deletes
        /// the cookie
        /// </summary>
        public int MaxAge
        {
            get { return maxAge; }
            set { maxAge = value; }
        }




        /// <summary>
        /// Getter:
        /// Returns the path on the server 
        /// to which the browser returns this cookie. The
        /// cookie is visible to all subpaths on the server.
        ///
        /// returns a string specifying a path that contains
        /// a servlet name, for example, "/catalog"
        /// 
        /// Setter:
        /// Specifies a path for the cookie
        /// to which the client should return the cookie.
        /// 
        /// The cookie is visible to all the pages in the directory
        /// you specify, and all the pages in that directory's subdirectories. 
        /// A cookie's path must include the servlet that set the cookie,
        /// for example, "/catalog", which makes the cookie
        /// visible to all directories on the server under "/catalog".
        ///
        /// Consult RFC 2109 (available on the Internet) for more
        /// information on setting path names for cookies.
        /// 
        /// value to set: a string specifying a path
        /// </summary>
        public string Path
        {
            get { return path; }
            set { path = value; }
        }





        /// <summary>
        /// Getter:
        /// Returns true if the browser is sending cookies
        /// only over a secure protocol, or false if the
        /// browser can send cookies using any protocol.
        /// 
        /// returns true if the browser uses a secure protocol;
        /// otherwise, true
        /// 
        /// Setter: 
        /// Indicates to the browser whether the cookie should only be sent
        /// using a secure protocol, such as HTTPS or SSL.
        /// 
        /// The default value is false.
        ///
        /// if set to if true, sends the cookie from the browser
        /// to the server only when using a secure protocol;
        /// if false, sent on any protocol
        /// </summary>
        public bool IsSecure
        {
            get { return secure; }
            set { secure = value; }
        }


        /// <summary>
        /// Gets the name of the cookie. The name cannot be changed after
        /// creation.
        /// returns a string specifying the cookie's name
        /// </summary>
        public string Name
        {
            get { return name; }
        }




        /// <summary>
        /// Getter:
        /// Returns the value of the cookie.
        ///
        /// returns	a string containing the cookie's
        /// present value
        /// 
        /// Setter: 
        /// Assigns a new value to a cookie after the cookie is created.
        /// If you use a binary value, you may want to use BASE64 encoding.
        /// 
        /// With Version 0 cookies, values should not contain white 
        /// space, brackets, parentheses, equals signs, commas,
        /// double quotes, slashes, question marks, at signs, colons,
        /// and semicolons. Empty values may not behave the same way
        /// on all browsers.
        /// </summary>
        public string Value
        {
            get { return value; }
        }





        /// <summary>
        /// Getter:
        /// Returns the version of the protocol this cookie complies 
        /// with. Version 1 complies with RFC 2109, 
        /// and version 0 complies with the original
        /// cookie specification drafted by Netscape. Cookies provided
        /// by a browser use and identify the browser's cookie version.
        /// 
        /// returns 0 if the cookie complies with the
        /// original Netscape specification; 1
        /// if the cookie complies with RFC 2109
        /// 
        /// Setter:
        /// Sets the version of the cookie protocol this cookie complies
        /// with. Version 0 complies with the original Netscape cookie
        /// specification. Version 1 complies with RFC 2109.
        ///
        /// Since RFC 2109 is still somewhat new, consider
        /// version 1 as experimental; do not use it yet on production sites.
        /// </summary>
        public int Version
        {
            get { return version; }
            set { version = value; }
        }





        // Note -- disabled for now to allow full Netscape compatibility
        // from RFC 2068, token special case characters
        // 
        // private static final string tspecials = "()<>@,;:\\\"/[]?={} \t";

        const string tspecials = ",; ";




        /// <summary>
        /// Tests a string and returns true if the string counts as a 
        /// reserved token in the Java language.
        /// </summary>
        /// <param name="value">the string to be tested</param>
        /// <returns>true if the string is a reserved token; false if it is not</returns>
        private bool IsToken(string value)
        {
            int len = value.Length;

            for (int i = 0; i < len; i++)
            {
                char c = value[i];

                if (c < 0x20 || c >= 0x7f || tspecials.IndexOf(c) != -1)
                    return false;
            }
            return true;
        }





        /// <summary>
        /// returns a copy of this cookie
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            Cookie clone = new Cookie(this.name, this.value);
            clone.name = this.name;
            clone.value = this.value;

            clone.comment = this.comment;
            clone.domain = this.domain;
            clone.maxAge = this.maxAge;
            clone.path = this.path;
            clone.secure = this.secure;
            clone.version = this.version;
            clone.httpOnly = this.httpOnly;

            return clone;
        }



        /// <summary>
        /// Gets or Sets whether cookie is for http only
        /// </summary>
        public bool IsHttpOnly
        {
            get { return httpOnly; }
            set { httpOnly = value; }
        }



    }
}

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
using NJetty.Commons.Util;
using System.IO;

namespace Javax.NServlet.Http
{

    /// <summary>
    /// As of Java(tm) Servlet API 2.3.
    /// These methods were only useful
    /// with the default encoding and have been moved
    /// to the request interfaces.
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// April 28, 2009
    /// </date>
    public class HttpUtils
    {

        //TODO: do the C# equivalent below
        //const string LSTRING_FILE = "javax.servlet.http.LocalStrings";
        //static ResourceBundle lStrings = ResourceBundle.getBundle(LSTRING_FILE);



        /// <summary>
        /// Constructs an empty HttpUtils object.
        /// </summary>
        public HttpUtils() { }



        /// <summary>
        /// Parses a query string passed from the client to the
        /// server and builds a Dictonary object
        /// with key-value pairs. 
        /// The query string should be in the form of a string
        /// packaged by the GET or POST method, that is, it
        /// should have key-value pairs in the form <i>key=value</i>,
        /// with each pair separated from the next by a &amp; character.
        ///
        /// A key can appear more than once in the query string
        /// with different values. However, the key appears only once in 
        /// the hashtable, with its value being
        /// an array of strings containing the multiple values sent
        /// by the query string.
        /// 
        /// The keys and values in the hashtable are stored in their
        /// decoded form, so
        /// any + characters are converted to spaces, and characters
        /// sent in hexadecimal notation (like <i>%xx</i>) are
        /// converted to ASCII characters.
        /// </summary>
        /// <param name="s">a string containing the query to be parsed</param>
        /// <returns>a Dictonary object built from the parsed key-value pairs</returns>
        /// <exception cref="ArgumentException">if the query string is invalid</exception>
        public static Dictionary<string, object> ParseQueryString(string s)
        {

            string[] valArray = null;

            if (s == null)
            {
                throw new ArgumentException();
            }
            Dictionary<string, object> ht = new Dictionary<string, object>();
            StringBuilder sb = new StringBuilder();
            StringTokenizer st = new StringTokenizer(s, "&");
            while (st.HasMoreTokens())
            {
                string pair = (string)st.NextToken();
                int pos = pair.IndexOf('=');
                if (pos == -1)
                {
                    // XXX
                    // should give more detail about the illegal argument
                    throw new ArgumentException();
                }
                string key = ParseName(pair.Substring(0, pos), sb);
                string val = ParseName(pair.Substring(pos + 1, pair.Length), sb);
                if (ht.ContainsKey(key))
                {
                    string[] oldVals = (string[])ht[key];
                    valArray = new string[oldVals.Length + 1];
                    for (int i = 0; i < oldVals.Length; i++)
                        valArray[i] = oldVals[i];
                    valArray[oldVals.Length] = val;
                }
                else
                {
                    valArray = new string[1];
                    valArray[0] = val;
                }
                ht.Add(key, valArray);
            }
            return ht;
        }





        /// <summary>
        /// Parses data from an HTML form that the client sends to 
        /// the server using the HTTP POST method and the 
        /// <i>application/x-www-form-urlencoded</i> MIME type.
        ///
        /// The data sent by the POST method contains key-value
        /// pairs. A key can appear more than once in the POST data
        /// with different values. However, the key appears only once in 
        /// the hashtable, with its value being
        /// an array of strings containing the multiple values sent
        /// by the POST method.
        ///
        /// The keys and values in the hashtable are stored in their
        /// decoded form, so
        /// any + characters are converted to spaces, and characters
        /// sent in hexadecimal notation (like <i>%xx</i>) are
        /// converted to ASCII characters.
        /// </summary>
        /// <param name="len">
        ///     len	an integer specifying the length,
        ///	    in characters, of the 
        ///	    ServletInputStream
        ///	    object that is also passed to this
        ///	    method
        /// </param>
        /// <param name="input">
        ///     the ServletInputStream
        ///	    object that contains the data sent
        ///	    from the client
        /// </param>
        /// <returns>a Dictonary object built from the parsed key-value pairs</returns>
        /// <exception cref="ArgumentException">if the data sent by the POST method is invalid</exception>
        static public Dictionary<string, object> ParsePostData(int len,
                          ServletInputStream input)
        {
            // XXX
            // should a length of 0 be an ArgumentException

            if (len <= 0)
                return new Dictionary<string, object>(); // cheap hack to return an empty hash

            if (input == null)
            {
                throw new ArgumentException();
            }

            //
            // Make sure we read the entire POSTed body.
            //
            char[] postedBytes = new char[len];
            try
            {
                int offset = 0;

                do
                {
                    int inputLen = input.Read(postedBytes, offset, len - offset);
                    if (inputLen <= 0)
                    {
                        //string msg = lStrings.getString("err.io.short_read");
                        string msg = "Short Read";
                        throw new ArgumentException(msg);
                    }
                    offset += inputLen;
                } while ((len - offset) > 0);

            }
            catch (IOException e)
            {
                throw new ArgumentException(e.Message);
            }

            // XXX we shouldn't assume that the only kind of POST body
            // is FORM data encoded using ASCII or ISO Latin/1 ... or
            // that the body should always be treated as FORM data.
            //

            try
            {

                string postedBody = new string(postedBytes, 0, len);
                return ParseQueryString(postedBody);
            }
            catch (ArgumentException e)
            {
                // XXX function should accept an encoding parameter & throw this
                // exception.  Otherwise throw something expected.
                throw new ArgumentException(e.Message);
            }
        }



        /// <summary>
        /// Parse a name in the query string.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="sb"></param>
        /// <returns></returns>
        static string ParseName(string s, StringBuilder sb)
        {
            sb.Length = 0;
            for (int i = 0; i < s.Length; i++)
            {
                char c = s[i];
                switch (c)
                {
                    case '+':
                        sb.Append(' ');
                        break;
                    case '%':
                        try
                        {
                            
                            sb.Append((char)Convert.ToInt32(s.Substring(i + 1, i + 3), 16));
                            i += 2;
                        }
                        catch (FormatException)
                        {
                            // XXX
                            // need to be more specific about illegal arg
                            throw new ArgumentException();
                        }
                        catch (OverflowException)
                        {
                            string rest = s.Substring(i);
                            sb.Append(rest);
                            if (rest.Length == 2)
                                i++;
                        }

                        break;
                    default:
                        sb.Append(c);
                        break;
                }
            }
            return sb.ToString();
        }


       

        /// <summary>
        /// Reconstructs the URL the client used to make the request,
        /// using information in the HttpServletRequest object.
        /// The returned URL contains a protocol, server name, port
        /// number, and server path, but it does not include query
        /// string parameters.
        /// 
        /// Because this method returns a StringBuilder,
        /// not a string, you can modify the URL easily, for example,
        /// to Append query parameters.
        ///
        /// This method is useful for creating redirect messages
        /// and for reporting errors.
        /// </summary>
        /// <param name="req">a HttpServletRequest object containing the client's request</param>
        /// <returns>a StringBuilder object containing the reconstructed URL</returns>
        public static StringBuilder GetRequestURL(IHttpServletRequest req)
        {
            StringBuilder url = new StringBuilder();
            string scheme = req.Scheme;
            int port = req.ServerPort;
            string urlPath = req.RequestURI;

            //string		servletPath = req.getServletPath ();
            //string		pathInfo = req.getPathInfo ();

            url.Append(scheme);		// http, https
            url.Append("://");
            url.Append(req.ServerName);
            if ((scheme.Equals("http") && port != 80)
                || (scheme.Equals("https") && port != 443))
            {
                url.Append(':');
                url.Append(req.ServerPort);
            }
            //if (servletPath != null)
            //    url.Append (servletPath);
            //if (pathInfo != null)
            //    url.Append (pathInfo);
            url.Append(urlPath);
            return url;
        }
    }
}

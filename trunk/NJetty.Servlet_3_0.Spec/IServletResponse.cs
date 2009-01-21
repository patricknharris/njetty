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
using System.Globalization;

namespace Javax.NServlet
{

    /// <summary>
    /// Defines an object to assist a servlet in sending a response to the client.
    /// The servlet container creates a IServletResponse object and
    /// passes it as an argument to the servlet's Service method.
    /// 
    /// To send binary data in a MIME body response, use
    /// the ServletOutputStream returned by get #OutputStream property.
    /// To send character data, use the TextWriter (PrintWriter in java) object 
    /// returned by get #Writer property. To mix binary and text data,
    /// for example, to create a multipart response, use a
    /// ServletOutputStream and manage the character sections
    /// manually.
    /// 
    /// The charset for the MIME body response can be specified
    /// explicitly using the set #CharacterEncoding and
    /// set #ContentType properties, or implicitly
    /// using the set #Locale property.
    /// Explicit specifications take precedence over
    /// implicit specifications. If no charset is specified, ISO-8859-1 will be
    /// used. The set CharacterEncoding property,
    /// set ContentType, or set Locale property must
    /// be called before  get Writer and before committing
    /// the response for the character encoding to be used.
    /// 
    /// See the Internet RFCs such as 
    /// <a href="http://www.ietf.org/rfc/rfc2045.txt">
    /// RFC 2045</a> for more information on MIME. Protocols such as SMTP
    /// and HTTP define profiles of MIME, and those standards
    /// are still evolving.
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// January 2009
    /// </date>
    public interface IServletResponse
    {

        /// <summary>
        /// getter:
        /// Returns the name of the character encoding (MIME charset)
        /// used for the body sent in this response.
        /// The character encoding may have been specified explicitly
        /// using the  set CharacterEncoding or
        /// set ContentType properties, or implicitly using the
        /// set Locale property. Explicit specifications take
        /// precedence over implicit specifications. Calls made
        /// to these methods after get Writer property has been
        /// called or after the response has been committed have no
        /// effect on the character encoding. If no character encoding
        /// has been specified, ISO-8859-1 is returned.
        /// <p>See RFC 2047 (http://www.ietf.org/rfc/rfc2047.txt)
        /// for more information about character encoding and MIME.
        /// 
        /// returns	a string specifying the
        /// name of the character encoding, for
        /// example, UTF-8
        /// 
        /// setter:
        /// Sets the character encoding (MIME charset) of the response
        /// being sent to the client, for example, to UTF-8.
        /// If the character encoding has already been set by
        /// {@link #setContentType} or {@link #setLocale},
        /// this method overrides it.
        /// Calling {@link #setContentType} with the string
        /// of text/html and calling
        /// this method with the string of UTF-8
        /// is equivalent with calling
        /// set ContentType property with the string of
        /// <code>text/html; charset=UTF-8</code>.
        /// 
        /// This method can be called repeatedly to change the character
        /// encoding.
        /// This method has no effect if it is called after
        /// <code>get Writer property</code> has been
        /// called or after the response has been committed.
        /// <p>Containers must communicate the character encoding used for
        /// the servlet response's writer to the client if the protocol
        /// provides a way for doing so. In the case of HTTP, the character
        /// encoding is communicated as part of the <code>Content-Type</code>
        /// header for text media types. Note that the character encoding
        /// cannot be communicated via HTTP headers if the servlet does not
        /// specify a content type; however, it is still used to encode text
        /// written via the servlet response's writer.
        /// 
        /// value to set, a string specifying only the character set defined by IANA Character Sets
        /// (http://www.iana.org/assignments/character-sets)
        /// <see cref="ContentType"/>
        /// <see cref="Locale"/>
        /// </summary>
        string CharacterEncoding
        {
            get;
            set;
        }



        /// <summary>
        /// getter:
        /// 
        /// Returns the content type used for the MIME body
        /// sent in this response. The content type proper must
        /// have been specified using set #ContentType property
        /// before the response is committed. If no content type
        /// has been specified, this method returns null.
        /// If a content type has been specified and a
        /// character encoding has been explicitly or implicitly
        /// specified as described in get #CharacterEncoding property,
        /// the charset parameter is included in the string returned.
        /// If no character encoding has been specified, the
        /// charset parameter is omitted.
        /// 
        /// returns a string specifying the	content type, for example,
        /// text/html; charset=UTF-8, or null
        /// 
        /// 
        /// setter:
        /// 
        /// Sets the content type of the response being sent to
        /// the client, if the response has not been committed yet.
        /// The given content type may include a character encoding
        /// specification, for example, text/html;charset=UTF-8 .
        /// The response's character encoding is only set from the given
        /// content type if this method is called before get Writer property
        /// is called.
        /// 
        /// This method may be called repeatedly to change content type and
        /// character encoding.
        /// This method has no effect if called after the response
        /// has been committed. It does not set the response's character
        /// encoding if it is called after get Writer property
        /// has been called or after the response has been committed.
        /// 
        /// Containers must communicate the content type and the character
        /// encoding used for the servlet response's writer to the client if
        /// the protocol provides a way for doing so. In the case of HTTP,
        /// the Content-Type header is used.
        /// 
        /// value to set: the MIME type of the content
        /// 
        /// <see cref="Locale"/>
        /// <see cref="CharacterEncoding"/>
        /// <see cref="OutputStream"/>
        /// <see cref="Writer"/>
        /// </summary>
        string ContentType
        {
            get;
            set;
        }

        /// <summary>
        /// Returns a ServletOutputStream suitable for writing binary 
        /// data in the response. The servlet container does not encode the
        /// binary data.  
        ///
        /// Calling Flush() on the ServletOutputStream commits the response.
        ///
        /// Either this method or get Writer may 
        /// be called to write the body, not both.
        /// 
        /// returns	a ServletOutputStream for writing binary data	
        /// 
        /// </summary>
        /// <exception cref="InvalidOperationException">if the <code>get Writer</code> property has been called on this response</exception>
        /// <exception cref="IOExceptoin">if an input or output exception occurred</exception>
        ServletOutputStream OutputStream  //throws IOException;
        {
            get;
        }




        /// <summary>
        /// Returns a <code>TextWriter (PrintWriter in java)</code> object that
        /// can send character text to the client.
        /// The <code>TextWriter (PrintWriter in java)</code> uses the character
        /// encoding returned by get CharacterEncoding property.
        /// If the response's character encoding has not been
        /// specified as described in <code>get CharacterEncoding property</code>
        /// (i.e., the method just returns the default value 
        /// <code>ISO-8859-1</code>), <code>get Writer property</code>
        /// updates it to <code>ISO-8859-1</code>.
        /// 
        /// Calling Flush() on the <code>TextWriter (PrintWriter in java)</code>
        /// commits the response.
        /// 
        /// Either this method or get OutputStream property may be called
        /// to write the body, not both.
        /// 
        /// returns a <code>TextWriter (PrintWriter in java)</code> object that can return character data to the client 
        /// </summary>
        /// <exception cref="ArgumentException">if the character encoding returned by <code>getCharacterEncoding</code> cannot be used</exception>
        /// <exception cref="InvalidOperationException">if the <code>getOutputStream</code> method has already been called for this response object</exception>
        /// <exception cref="System.IO.Exception">if an input or output exception occurred</exception>
        TextWriter Writer
        {
            get;
        }



        /// <summary>
        /// Sets the length of the content body in the response
        /// In HTTP servlets, this method sets the HTTP Content-Length header.
        /// 
        /// value to set: 	an integer specifying the length of the 
        /// content being returned to the client; sets
        /// the Content-Length header
        /// </summary>
        int ContentLength
        {
            set;
        }





        /// <summary>
        /// getter:
        /// Returns the actual buffer size used for the response.  If no buffering
        /// is used, this method returns 0.
        /// 
        /// returns the actual buffer size used
        /// <see cref="BufferSize"/>
        /// <see cref="FlushBuffer"/>
        /// <see cref="IsCommited"/>
        /// <see cref="Reset"/>
        /// 
        /// setter:
        /// Sets the preferred buffer size for the body of the response.  
        /// The servlet container will use a buffer at least as large as 
        /// the size requested.  The actual buffer size used can be found
        /// using  get BufferSize property.
        /// 
        /// A larger buffer allows more content to be written before anything is
        /// actually sent, thus providing the servlet with more time to set
        /// appropriate status codes and headers.  A smaller buffer decreases 
        /// server memory load and allows the client to start receiving data more
        /// quickly.
        /// 
        /// This method must be called before any response body content is
        /// written; if content has been written or the response object has
        /// been committed, this method throws an 
        /// InvalidOperationException.
        /// <see cref="BufferSize"/>
        /// <see cref="FlushBuffer"/>
        /// <see cref="IsCommited"/>
        /// <see cref="Reset"/>
        /// </summary>
        /// <exception cref="InvalidOperationException">when setting this property after content has been written</exception>
        int BufferSize
        {
            get;
            set;
        }



        /// <summary>
        /// Forces any content in the buffer to be written to the client.  A call
        /// to this method automatically commits the response, meaning the status 
        /// code and headers will be written.
        /// </summary>
        /// <exception cref="System.IO.IOException"></exception>
        /// <see cref="BufferSize"/>
        /// <see cref="IsCommited"/>
        /// <see cref="Reset"/>
        void FlushBuffer(); //throws IOException;



        /// <summary>
        /// Clears the content of the underlying buffer in the response without
        /// clearing headers or status code. If the 
        /// response has been committed, this method throws an 
        /// InvalidOperationException
        /// </summary>
        /// <exception cref="InvalidOperationException">If the response has been committed</exception>
        /// <see cref="BufferSize"/>
        /// <see cref="IsCommited"/>
        /// <see cref="Reset"/>
        void ResetBuffer();



        /// <summary>
        /// Returns a bool indicating if the response has been
        /// committed.  A committed response has already had its status 
        /// code and headers written.
        /// 
        /// returns a bool indicating if the response has been committed 
        /// </summary>
        /// <see cref="#BufferSize"/>
        /// <see cref="#FlushBuffer"/>
        /// <see cref="#Reset"/>
        bool IsCommitted
        {
            get;
        }




        /// <summary>
        /// Clears any data that exists in the buffer as well as the status code and
        /// headers.  If the response has been committed, this method throws an 
        /// ArgumentException.
        /// </summary>
        /// <exception cref="ArgumentException">if the response has already been committed</exception>
        void Reset();






        /// <summary>
        /// getter:
        /// Returns the locale specified for this response
        /// using the set #Locale property. Calls made to
        /// set Locale property after the response is committed
        /// have no effect. If no locale has been specified,
        /// the container's default locale is returned.
        /// 
        /// returns locale specified for this response
        /// 
        /// setter:
        /// Sets the locale of the response, if the response has not been
        /// committed yet. It also sets the response's character encoding
        /// appropriately for the locale, if the character encoding has not
        /// been explicitly set using set #ContentType property or
        /// set #CharacterEncoding property, get Writer property hasn't
        /// been called yet, and the response hasn't been committed yet.
        /// If the deployment descriptor contains a 
        /// locale-encoding-mapping-list element, and that
        /// element provides a mapping for the given locale, that mapping
        /// is used. Otherwise, the mapping from locale to character
        /// encoding is container dependent.
        /// 
        /// This property may be called repeatedly to change locale and
        /// character encoding. The method has no effect if called after the
        /// response has been committed. It does not set the response's
        /// character encoding if it is called after set #ContentType property
        /// has been called with a charset specification, after
        /// set #CharacterEncoding property has been called, after
        /// get Writer property has been called, or after the response
        /// has been committed.
        /// 
        /// Containers must communicate the locale and the character encoding
        /// used for the servlet response's writer to the client if the protocol
        /// provides a way for doing so. In the case of HTTP, the locale is
        /// communicated via the Content-Language header,
        /// the character encoding as part of the Content-Type
        /// header for text media types. Note that the character encoding
        /// cannot be communicated via HTTP headers if the servlet does not
        /// specify a content type; however, it is still used to encode text
        /// written via the servlet response's writer.
        /// </summary>
        /// <see cref="#ContentType"/>
        /// <see cref="#CharacterEncoding"/>
        CultureInfo Locale
        {
            get;
            set;
        }

        /// <summary>
        /// Helper for suspend/resume: disables output
        /// </summary>
        void Disable();


        /// <summary>
        /// Helper for suspend/resume: enables output
        /// </summary>
        void Enable();


        /// <summary>
        /// Helper for suspend/resume, shows disabled state
        /// returns true if disable is most recent disable/enable call
        /// </summary>
        bool IsDisabled
        {
            get;
        }
    }
}

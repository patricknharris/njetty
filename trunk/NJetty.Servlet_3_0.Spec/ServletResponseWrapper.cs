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
    /// Provides a convenient implementation of the ServletResponse interface that
    /// can be subclassed by developers wishing to adapt the response from a Servlet.
    /// This class implements the Wrapper or Decorator pattern. Methods default to
    /// calling through to the wrapped response object.
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// April 21, 2009
    /// </date>
    public class ServletResponseWrapper : IServletResponse
    {
        IServletResponse response;

        /// <summary>
        /// Creates a ServletResponse adaptor wrapping the given response object.
        /// </summary>
        /// <param name="response"></param>
        /// <exception cref="ArgumentException">if the response is null</exception>
        public ServletResponseWrapper(IServletResponse response)
        {
            if (response == null)
            {
                throw new ArgumentException("Response cannot be null");
            }
            this.response = response;
        }

        /// <summary>
        /// Gets or Sets the wrapped ServletResponse object.
        /// </summary>
        /// <exception cref="ArgumentException">if the response is set to null.</exception>
        public IServletResponse Response
        {
            get { return response; }
            set 
            {
                if (value == null)
                {
                    throw new ArgumentException("Response cannot be null");
                }
                this.response = value;
            }
        }


        /// <summary>
        /// The default behavior of this property is to get or set CharacterEncoding
        /// on the wrapped response object.
        /// </summary>
        public string CharacterEncoding
        {
            get
            {
                return response.CharacterEncoding;
            }
            set
            {
                response.CharacterEncoding = value;
            }
        }

        /// <summary>
        /// The default behavior of this property is to get OutputStream
        /// on the wrapped response object.
        /// </summary>
        public ServletOutputStream OutputStream
        {
            get { return response.OutputStream; }
        }

        /// <summary>
        /// The default behavior of this property is to get Writer
        /// on the wrapped response object.
        /// </summary>
        public System.IO.TextWriter Writer
        {
            get { return response.Writer; }
        }


        /// <summary>
        /// The default behavior of this property is to set ContentLength
        /// on the wrapped response object.
        /// </summary>
        public int ContentLength
        {
            set { response.ContentLength = value; }
        }

        /// <summary>
        /// The default behavior of this property is to get or set ContentType
        /// on the wrapped response object.
        /// </summary>
        public string ContentType
        {
            get
            {
                return response.ContentType;
            }
            set
            {
                response.ContentType = value;
            }
        }



        /// <summary>
        /// The default behavior of this property is to get or set BufferSize
        /// on the wrapped response object.
        /// </summary>
        public int BufferSize
        {
            get
            {
                return response.BufferSize;
            }
            set
            {
                response.BufferSize = value;
            }
        }


        /// <summary>
        /// The default behavior of this method is to call FlushBuffer()
        /// on the wrapped response object.
        /// </summary>
        public void FlushBuffer()
        {
            response.FlushBuffer();
        }


        /// <summary>
        /// The default behavior of this property is to get IsCommitted
        /// on the wrapped response object.
        /// </summary>
        public bool IsCommitted
        {
            get { return response.IsCommitted; }
        }



        /// <summary>
        /// The default behavior of this method is to call Reset()
        /// on the wrapped response object.
        /// </summary>
        public void Reset()
        {
            response.Reset();
        }

        /// <summary>
        /// The default behavior of this method is to call ResetBuffer()
        /// on the wrapped response object.
        /// </summary>
        public void ResetBuffer()
        {
            response.ResetBuffer();
        }

        /// <summary>
        /// The default behavior of this property is to get or set Locale
        /// on the wrapped response object.
        /// </summary>
        public System.Globalization.CultureInfo Locale
        {
            get
            {
                return response.Locale;
            }
            set
            {
                response.Locale = value;
            }
        }

        /// <summary>
        /// Helper for suspend/resume: enables output
        /// </summary>
        public void Disable()
        {
            response.Disable();
        }

        /// <summary>
        /// Helper for suspend/resume: enables output
        /// </summary>
        public void Enable()
        {
            response.Enable();
        }

        /// <summary>
        /// Helper for suspend/resume, shows disabled state
        /// 
        /// return true if disable is most recent disable/enable call
        /// </summary>
        public bool IsDisabled
        {
            get { return response.IsDisabled; }
        }

    }
}

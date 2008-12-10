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

namespace NJetty.Util.Util
{

    /// <summary>
    /// Wraps multiple exceptions.
    /// Allows multiple exceptions to be thrown as a single exception.
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// December 2008
    /// </date>
    public class MultiException : Exception
    {
        private object nested;

        public MultiException() : base("Multiple exceptions")
        {}

        /// <summary>
        /// Add and Exception to the List of Multiple Exceptions
        /// </summary>
        /// <param name="e"></param>
        public void Add(Exception e)
        {
            if (e is MultiException)
            {
                MultiException me = (MultiException)e;
                for (int i = 0; i < LazyList.Size(me.nested); i++)
                {
                    nested = LazyList.Add(nested, LazyList.Get(me.nested, i));
                }
            }
            else
            {
                nested = LazyList.Add(nested, e);
            }
        }

        public int Count
        {
            get {return LazyList.Size(nested);}
        }
        
        public List<object> Exceptions
        {
            get { return LazyList.GetList(nested); }
        }
        
        public Exception GetException(int i)
        {
            return (Exception) LazyList.Get(nested,i);
        }

        /// <summary>
        /// Throw a multiexception.
        /// If this multi exception is empty then no action is taken. If it
        /// contains a single exception that is thrown, otherwise the this
        /// multi exception is thrown.
        /// </summary>
        /// 
        /// <exception cref="Exception"></exception>
        /// <exception cref="SystemException"></exception>
        /// <exception cref="MultiException">throws this instance, If there are more that one Exception</exception>
        public void IfExceptionThrow()
        {
            switch (LazyList.Size(nested))
            {
              case 0:
                  break;

              case 1:
                  Exception exception=(Exception)LazyList.Get(nested,0);
                  if (exception is SystemException)
                  {
                      throw (SystemException)exception;
                  }
                  if (exception is Exception)
                  {
                      throw (Exception)exception;
                  }
                  throw this;

              default:
                  throw this;
            }
        }
        
        /// <summary>
        /// Throw a ApplicationException exception.
        /// If this multi exception is empty then no action is taken. If it
        /// contains a single error or ApplicationException that is thrown, otherwise the this
        /// multi exception is thrown, wrapped in a ApplicationException.
        /// </summary>
        /// 
        /// <exception cref="SystemException">If this exception contains only 1 SystemException</exception>
        /// <exception cref="ApplicationException">
        ///  If this exception contains only 1 Exception but it is not a SystemException
        ///  If contains more than 1 Exceptions of any type, then multi-exception is wrapped as an ApplicationException
        /// </exception>
        
        public void IfExceptionApplicationException()
        {
            switch (LazyList.Size(nested))
            {
              case 0:
                  break;
              case 1:
                  Exception exception=(Exception)LazyList.Get(nested,0);
                  if (exception is SystemException)
                  {
                      throw (SystemException)exception;
                  }
                  else if (exception is ApplicationException)
                  {
                      throw (ApplicationException)exception;
                  }
                  else
                  {
                      throw new ApplicationException(exception.Message, exception);
                  }
              default:
                  throw new ApplicationException("Mulitple Exceptions", this);
            }
        }
        
       
        /// <summary>
        /// Throw a multiexception.
        /// If this multi exception is empty then no action is taken. If it
        /// contains a any exceptions then this
        /// </summary>
        /// 
        /// <exception cref="MultiException"></exception>
        public void IfExceptionThrowMulti()
        {
            if (LazyList.Size(nested) > 0)
            {
                throw this;
            }
        }

        public override string ToString()
        {
            if (LazyList.Size(nested) > 0)
            {
                return "NJetty.Util.Util.MultiException" 
                    + LazyList.GetList(nested);
            }
            return "NJetty.Util.Util.MultiException[]";
        }

        public override string StackTrace
        {
            get
            {
                StringBuilder sb = new StringBuilder(base.StackTrace);
                for (int i = 0; i < LazyList.Size(nested); i++)
                {
                    sb.AppendLine(((Exception)LazyList.Get(nested, i)).StackTrace);
                }

                return sb.ToString();
            }

        }
    }
}

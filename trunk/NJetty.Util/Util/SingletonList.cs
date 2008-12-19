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
    /// This simple efficient implementation of a List with a single
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// December 2008
    /// </date>
    public class SingletonList : IList<object>
    {

        
        private object o;
        
        private SingletonList(object o)
        {
            this.o=o;
        }

        public static SingletonList newSingletonList(object o)
        {
            return new SingletonList(o);
        }


        

        

        #region IList<object> Members

        public int IndexOf(object item)
        {
            if (o == item)
            {
                return 0;
            }
            else
            {
                return -1;
            }
        }

        public void Insert(int index, object item)
        {
            throw new NotSupportedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        public object this[int index]
        {
            get
            {
                if (index != 0)
                    throw new IndexOutOfRangeException("index " + index);
                return o;
            }
            set
            {
                if (index != 0)
                    throw new IndexOutOfRangeException("index " + index);
                o = value;
            }
        }

        #endregion

        #region ICollection<object> Members

        public void Add(object item)
        {
            throw new NotSupportedException();
        }

        public void Clear()
        {
            throw new NotSupportedException();
        }

        public bool Contains(object item)
        {
            return item == o;
        }

        public void CopyTo(object[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get { return 1; }
        }

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        public bool Remove(object item)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IEnumerable<object> Members

        public IEnumerator<object> GetEnumerator()
        {
            return new SingleTonEnumerator(o);
        }

        class SingleTonEnumerator : IEnumerator<object>
        {
            #region IEnumerator<object> Members
            
            object o;
            object current = null;

            public SingleTonEnumerator(object value)
            {
                o = value;
            }

            public object Current
            {
                get { return current; }
            }

            #endregion

            #region IDisposable Members

            public void Dispose()
            {
                throw new NotSupportedException();
            }

            #endregion

            #region IEnumerator Members


            public bool MoveNext()
            {
                if (current == null)
                {
                    current = o;
                    return true;
                }
                return false;
            }

            public void Reset()
            {
                throw new NotSupportedException();
            }

            #endregion
        }

        #endregion


        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

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
using System.Runtime.Serialization;

namespace NJetty.Util.Util
{

    /// <summary>
    /// Thread safe wrapper for Dictionary
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// November 2008
    /// </date>
    public class ConcurrentDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    {

        #region Constructors

        public ConcurrentDictionary()
            : base()
        { }

        public ConcurrentDictionary(int capacity)
            : base(capacity)
        { }

        public ConcurrentDictionary(IEqualityComparer<TKey> comparer)
            : base(comparer)
        { }



        public ConcurrentDictionary(IDictionary<TKey, TValue> dictionary)
            : base(dictionary)
        { }

        public ConcurrentDictionary(int capacity, IEqualityComparer<TKey> comparer)
            : base(capacity, comparer)
        { }



        public ConcurrentDictionary(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }


        public ConcurrentDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
            : base(dictionary, comparer)
        { }

        #endregion

        object _lock = new object();

        #region Dictionary Overrides

        public void Add(TKey key, TValue value)
        {
            lock (_lock)
            {
                base.Add(key, value);
            }
        }

        public bool ContainsKey(TKey key)
        {
            lock (_lock)
            {
                return base.ContainsKey(key);
            }
        }

        public Dictionary<TKey, TValue>.KeyCollection Keys
        {
            get
            {
                lock (_lock)
                {
                    return base.Keys;
                }
            }
        }

        public bool Remove(TKey key)
        {
            lock (_lock)
            {
                return base.Remove(key);
            }

        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            lock (_lock)
            {
                return base.TryGetValue(key, out value);
            }
        }

        public Dictionary<TKey, TValue>.ValueCollection Values
        {
            get
            {
                lock (_lock)
                {
                    return base.Values;
                }
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                lock (_lock)
                {
                    return base[key];
                }
            }
            set
            {
                lock (_lock)
                {
                    base[key] = value;
                }
            }
        }

        #endregion

        #region ICollection<KeyValuePair<TKey,TValue>> Members



        public void Clear()
        {
            lock (_lock)
            {
                base.Clear();
            }
        }

        public int Count
        {
            get
            {
                lock (_lock)
                {
                    return base.Count;
                }
            }
        }

        #endregion


        public void AddIfAbsent(TKey key, TValue value)
        {
            lock (_lock)
            {
                if (!base.ContainsKey(key))
                {
                    base.Add(key, value);
                }
            }
        }

        public bool Remove(TKey key, TValue value)
        {
            lock (_lock)
            {
                if (base.ContainsKey(key) && base[key].Equals(value))
                {
                    base.Remove(key);
                    return true;
                }
                else
                    return false;
            }
        }

        public bool Replace(TKey key, TValue oldValue, TValue newValue)
        {
            lock (_lock)
            {
                if (base.ContainsKey(key) && base[key].Equals(oldValue)) 
                {
                     base.Add(key, newValue);
                     return true;
                } 
                else 
                    return false;

            }
        }

        public void Replace(TKey key, TValue value)
        {
            if(base.ContainsKey(key))
            {
                base[key] = value;
            }
        }
    }
}

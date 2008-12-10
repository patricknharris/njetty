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

namespace NJetty.Util.Util
{

    /// <summary>
    /// TODO: Class/Interface Information here
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// November 2008
    /// </date>
    public class MultiMap<K> : Dictionary<K, object>
    {

        Dictionary<K, object> _map;
        ConcurrentDictionary<K, object> _cmap;

        #region Constructors

        public MultiMap()
        {
            _map = new Dictionary<K, object>();
        }

        public MultiMap(IDictionary<K, object> map)
        {
            if (map is ConcurrentDictionary<K, object>)
                _map = _cmap = new ConcurrentDictionary<K, object>(map);
            else
                _map = new Dictionary<K, object>(map);
        }

        public MultiMap(int capacity)
        {
            _map = new Dictionary<K, object>(capacity);
        }

        public MultiMap(bool concurrent)
        {
            if (concurrent)
                _map = _cmap = new ConcurrentDictionary<K, object>();
            else
                _map = new Dictionary<K, object>();
        }

        #endregion


        /// <summary>
        /// Get multiple values.
        /// Single valued entries are converted to singleton lists.
        /// </summary>
        /// <param name="name">The entry key.</param>
        /// <returns>Unmodifieable List of values.</returns>
        public List<object> GetValues(K name)
        {
            return LazyList.GetList(_map[name], true);
        }

        /// <summary>
        /// Get a value from a multiple value.
        /// If the value is not a multivalue, then index 0 retrieves the
        /// value or null.
        /// </summary>
        /// <param name="name">The entry key.</param>
        /// <param name="i">Index of element to get.</param>
        /// <returns>Unmodifieable List of values.</returns>
        public object GetValue(K name, int i)
        {
            object l = _map.ContainsKey(name) ? _map[name] : null;
            if (i == 0 && LazyList.Size(l) == 0)
                return null;
            return LazyList.Get(l, i);
        }


        /// <summary>
        /// Get value as string.
        /// Single valued items are converted to a string with the ToString()
        /// object method. Multi valued entries are converted to a comma separated
        /// List.  No quoting of commas within values is performed.
        /// </summary>
        /// <param name="name">The entry key</param>
        /// <returns>string value</returns>
        public string GetString(K name)
        {
            object l = _map.ContainsKey(name) ? _map[name] : null;
            switch (LazyList.Size(l))
            {
                case 0:
                    return null;
                case 1:
                    object o = LazyList.Get(l, 0);
                    return o == null ? null : o.ToString();
                default:
                    {
                        StringBuilder values = new StringBuilder(128);
                        for (int i = 0; i < LazyList.Size(l); i++)
                        {
                            object e = LazyList.Get(l, i);
                            if (e != null)
                            {
                                if (values.Length > 0)
                                    values.Append(',');
                                values.Append(e.ToString());
                            }
                        }
                        return values.ToString();
                    }
            }
        }

        
        public new object this[K name]
        {
            get
            {
                object l = _map.ContainsKey(name) ? _map[name] : null;
                switch (LazyList.Size(l))
                {
                    case 0:
                        return null;
                    case 1:
                        object o = LazyList.Get(l, 0);
                        return o;
                    default:
                        return LazyList.GetList(l, true);
                }
            }
            set
            {
                Add(name, value);
            }
        }

        /// <summary>
        /// Put and entry into the map.
        /// Existing values will be relaced with the new value
        /// </summary>
        /// <param name="name">The entry key.</param>
        /// <param name="value">The entry value.</param>
        public new void Add(K name, object value)
        {
            _map.Add(name, LazyList.Add(null, value));
        }

        /// <summary>
        /// Put multi valued entry.
        /// Existing values will be relaced with the new value
        /// </summary>
        /// <param name="name">The entry key.</param>
        /// <param name="values">The List of multiple values.</param>
        public void AddValues(K name, IList values)
        {
            _map.Add(name, values);
        }

        /// <summary>
        /// Put multi valued entry.
        /// Existing values will be relaced with the new value
        /// </summary>
        /// <param name="name"></param>
        /// <param name="values"></param>
        public void AddValues(K name, string[] values)
        {
            object list = null;
            for (int i = 0; i < values.Length; i++)
                list = LazyList.Add(list, values[i]);
            Add(name, list);
        }


        /// <summary>
        /// Add value to multi valued entry.
        /// If the entry is single valued, it is converted to the first
        /// value of a multi valued entry.
        /// The value will be (existing value[s] + the new value[s])
        /// </summary>
        /// <param name="name">The entry key.</param>
        /// <param name="value">The entry value.</param>
        public void Append(K name, object value)
        {
            object lo = _map.ContainsKey(name) ? _map[name] : null; 
            object ln = LazyList.Add(lo, value);
            if (lo != ln)
            {
                if (_map.ContainsKey(name))
                {
                    _map[name] = ln;
                }
                else
                {
                    _map.Add(name, ln);
                }
            }
            
        }

        /// <summary>
        /// Add values to multi valued entry.
        /// If the entry is single valued, it is converted to the first
        /// value of a multi valued entry.
        /// The value will be (existing value[s] + the new value[s])
        /// </summary>
        /// <param name="name">The entry key.</param>
        /// <param name="values">The List of multiple values.</param>
        public void AppendValues(K name, ICollection<object> values)
        {
            object lo = _map.ContainsKey(name) ? _map[name] : null;
            object ln = LazyList.AddCollection(lo, values);
            if (lo != ln)
            {
                if (_map.ContainsKey(name))
                {
                    _map[name] = ln;
                }
                else
                {
                    _map.Add(name, ln);
                }
            }
        }

        

        /// <summary>
        /// Add values to multi valued entry.
        /// If the entry is single valued, it is converted to the first
        /// value of a multi valued entry.
        /// The value will be (existing value[s] + the new value[s])
        /// </summary>
        /// <param name="name">The entry key</param>
        /// <param name="values">The string array of multiple values.</param>
        public void AppendValues(K name, string[] values)
        {
            object lo = _map.ContainsKey(name) ? _map[name] : null;
            object ln = LazyList.AddArray(lo, values);
            if (lo != ln)
            {
                if (_map.ContainsKey(name))
                {
                    _map[name] = ln;
                }
                else
                {
                    _map.Add(name, ln);
                }
            }
        }

       

        /// <summary>
        /// Remove value.
        /// </summary>
        /// <param name="name">The entry key</param>
        /// <param name="value">The entry value</param>
        /// <returns>true if it was removed</returns>
        public bool RemoveValue(K name, object value)
        {
            object lo = _map.ContainsKey(name) ? _map[name] : null;
            object ln = lo;
            int s = LazyList.Size(lo);
            if (s > 0)
            {
                ln = LazyList.Remove(lo, value);
                if (ln == null)
                    _map.Remove(name);
                else
                    _map.Add(name, ln);
            }
            return LazyList.Size(ln) != s;
        }

        /// <summary>
        /// Add all contents of dictionary in our multi-map,
        /// replacing existing once with new onces
        /// </summary>
        /// <param name="m">Dictionary</param>
        public void AddAll(Dictionary<K, object> m)
        {
            bool multi = m is MultiMap<K>;
            if (m != null)
            {
                foreach (K item in m.Keys)
                {
                    if (multi)
                        _map.Add(item, LazyList.Clone(m[item]));
                    else
                        _map.Add(item, m[item]);
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Dictionary of string arrays</returns>
        public Dictionary<K, object>  ToStringArrayMap()
        {
            Dictionary<K, object> map = new Dictionary<K, object>(_map.Count * 3 / 2);

            foreach (K key in _map.Keys)
            {
                object l = _map[key];
                string[] a = LazyList.ToStringArray(l);
                map.Add(key, a);
            }
            return map;
        }

        public new void Clear()
        {
            _map.Clear();
        }

        public new bool ContainsKey(K key)
        {
            return _map.ContainsKey(key);
        }

        public new bool ContainsValue(object value)
        {
            return _map.ContainsValue(value);
        }

        public new bool Equals(object o)
        {
            return _map.Equals(o);
        }

        public override int GetHashCode()
        {
            return _map.GetHashCode();
        }

        public bool IsEmpty
        {
            get
            {
                return _map.Count <= 0;
            }
        }

        public new Dictionary<K, object>.KeyCollection Keys
        {
            get { return _map.Keys; }

        }

        public new object Remove(K key)
        {
            return _map.Remove(key);
        }

        public new int Count
        {
            get { return _map.Count; }
        }

        public new Dictionary<K, object>.ValueCollection Values
        {

            get { return _map.Values; }
        }



        public void AddIfAbsent(K key, object value)
        {
            if (_cmap == null)
                throw new NotSupportedException();
            _cmap.AddIfAbsent(key, value);
        }

        public bool Remove(K key, object value)
        {
            if (_cmap == null)
                throw new NotSupportedException();
            return _cmap.Remove(key, value);
        }

        public bool Replace(K key, object oldValue, object newValue)
        {
            if (_cmap == null)
                throw new NotSupportedException();
            return _cmap.Replace(key, oldValue, newValue);
        }

        public bool Replace(K key, object value)
        {
            if (_cmap == null)
                throw new NotSupportedException();
            return _cmap.Replace(key, value);
        }

       

        public new bool TryGetValue(K key, out object value)
        {
            object l = _map.TryGetValue(key, out value);
            switch (LazyList.Size(l))
            {
                case 0:
                    return false;
                case 1:
                    value = LazyList.Get(l, 0);
                    return true;
                default:
                    value = LazyList.GetList(l, true);
                    return true;
            }
        }

    }
}

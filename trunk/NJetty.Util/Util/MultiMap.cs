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


        /* ------------------------------------------------------------ */
        /** Get multiple values.
         * Single valued entries are converted to singleton lists.
         * @param name The entry key. 
         * @return Unmodifieable List of values.
         */
        public List<object> GetValues(K name)
        {
            return LazyList.GetList(_map[name], true);
        }

        /* ------------------------------------------------------------ */
        /** Get a value from a multiple value.
         * If the value is not a multivalue, then index 0 retrieves the
         * value or null.
         * @param name The entry key.
         * @param i Index of element to get.
         * @return Unmodifieable List of values.
         */
        public object GetValue(K name, int i)
        {
            object l = _map[name];
            if (i == 0 && LazyList.Size(l) == 0)
                return null;
            return LazyList.Get(l, i);
        }


        /* ------------------------------------------------------------ */
        /** Get value as string.
         * Single valued items are converted to a string with the toString()
         * object method. Multi valued entries are converted to a comma separated
         * List.  No quoting of commas within values is performed.
         * @param name The entry key. 
         * @return string value.
         */
        public string GetString(K name)
        {
            object l = _map[name];
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

        /* ------------------------------------------------------------ */



        public object this[K name]
        {
            get
            {
                object l = _map[name];
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
                
            }
        }

        /* ------------------------------------------------------------ */
        /** Put and entry into the map.
         * @param name The entry key. 
         * @param value The entry value.
         * @return The previous value or null.
         */
        public void Add(K name, object value)
        {
            _map.Add(name, LazyList.Add(null, value));
        }

        /* ------------------------------------------------------------ */
        /** Put multi valued entry.
         * @param name The entry key. 
         * @param values The List of multiple values.
         * @return The previous value or null.
         */
        public void AddValues(K name, IList values)
        {
            _map.Add(name, values);
        }

        /* ------------------------------------------------------------ */
        /** Put multi valued entry.
         * @param name The entry key. 
         * @param values The string array of multiple values.
         * @return The previous value or null.
         */
        public void AppendValues(K name, string[] values)
        {
            object list = null;
            for (int i = 0; i < values.Length; i++)
                list = LazyList.Add(list, values[i]);
            Add(name, list);
        }


        /* ------------------------------------------------------------ */
        /** Add value to multi valued entry.
         * If the entry is single valued, it is converted to the first
         * value of a multi valued entry.
         * @param name The entry key. 
         * @param value The entry value.
         */
        public void Append(K name, object value)
        {
            object lo = _map[name];
            object ln = LazyList.Add(lo, value);
            if (lo != ln)
                _map.Add(name, ln);
        }

        /* ------------------------------------------------------------ */
        /** Add values to multi valued entry.
         * If the entry is single valued, it is converted to the first
         * value of a multi valued entry.
         * @param name The entry key. 
         * @param values The List of multiple values.
         */
        public void AppendValues(K name, ICollection<object> values)
        {
            object lo = _map[name];
            object ln = LazyList.AddCollection(lo, values);
            if (lo != ln)
                _map.Add(name, ln);
        }

        /* ------------------------------------------------------------ */
        /** Add values to multi valued entry.
         * If the entry is single valued, it is converted to the first
         * value of a multi valued entry.
         * @param name The entry key. 
         * @param values The string array of multiple values.
         */
        public void AddValues(K name, string[] values)
        {
            object lo = _map[name];
            object ln = LazyList.AddArray(lo, values);
            if (lo != ln)
                _map.Add(name, ln);
        }

        /* ------------------------------------------------------------ */
        /** Remove value.
         * @param name The entry key. 
         * @param value The entry value. 
         * @return true if it was removed.
         */
        public bool RemoveValue(K name, object value)
        {
            object lo = _map[name];
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

        /* ------------------------------------------------------------ */
        /** Put all contents of map.
         * @param m Map
         */
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

        /* ------------------------------------------------------------ */
        /** 
         * @return Map of string arrays
         */
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

        public void Clear()
        {
            _map.Clear();
        }

        public bool ContainsKey(K key)
        {
            return _map.ContainsKey(key);
        }

        public bool ContainsValue(object value)
        {
            return _map.ContainsValue(value);
        }

        //public Set<Entry<K, object>> entrySet()
        //{
        //    return _map.
        //}

        public bool Equals(object o)
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

        public Dictionary<K, object>.KeyCollection Keys
        {
            get { return _map.Keys; }

        }

        public object Remove(K key)
        {
            return _map.Remove(key);
        }

        public int Count
        {
            get { return _map.Count; }
        }

        public Dictionary<K, object>.ValueCollection Values
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

        public void Replace(K key, object value)
        {
            if (_cmap == null)
                throw new NotSupportedException();
            _cmap.Replace(key, value);
        }

       

        public bool TryGetValue(K key, out object value)
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

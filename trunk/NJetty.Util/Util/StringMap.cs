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
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace NJetty.Util.Util
{

    /// <summary>
    /// Map implementation Optimized for Strings keys..
    /// This string Map has been optimized for mapping small sets of
    /// Strings where the most frequently accessed Strings have been put to
    /// the map first.
    /// 
    /// It also has the benefit that it can look up entries by substring or
    /// sections of char and byte arrays.  This can prevent many string
    /// objects from being created just to look up in the map.
    /// 
    /// This map is NOT synchronized.
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// December 2008
    /// </date>
    public class StringMap : Dictionary<string, object>
    {
        #region Constants
        public const bool CASE_INSENSTIVE = true;
        protected const int __HASH_WIDTH = 17;
        #endregion

        #region Instance Fields
        protected int _width = __HASH_WIDTH;
        protected Node _root = new Node();
        protected bool _ignoreCase = false;
        protected NullEntry _nullEntry = null;
        protected object _nullValue = null;
        protected HashSet<object> _entrySet = new HashSet<object>();
        ReadOnlyHashSet _umEntrySet;

        #endregion

        #region Constructors

        public StringMap()
        {
            _umEntrySet = new ReadOnlyHashSet(_entrySet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ignoreCase">if true, key is case insensitive</param>
        public StringMap(bool ignoreCase)
            : base()
        {
            _ignoreCase = ignoreCase;
            _umEntrySet = new ReadOnlyHashSet(_entrySet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ignoreCase">if true, key is case insensitive</param>
        /// <param name="width">Width of hash tables, larger values are faster but consume more memory</param>
        public StringMap(bool ignoreCase, int width)
            : base()
        {
            _ignoreCase = ignoreCase;
            _width = width;
            _umEntrySet = new ReadOnlyHashSet(_entrySet);
        }

        #endregion

        #region Hashmap Properties

        /// <summary>
        /// Gets or Sets Ignore Case Attribute, If true map is case insensiteve for keys
        /// </summary>
        public bool IgnoreCase
        {
            get { return _ignoreCase; }
            set
            {
                if (_root._children != null)
                    throw new ArgumentException("Must be set before first put");
                _ignoreCase = value;
            }
        }

        /// <summary>
        /// Gets or Sets the hash width
        /// Note: Larger values are faster but consume more memory
        /// </summary>
        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        #endregion

        #region Add Value

        public void Add(object key, object value)
        {
            if (key == null)
            {
                Add(null, value);
            }
            else
            {
                Add(key.ToString(), value);
            }
        }

        public new void Add(string key, object value)
        {
            if (key == null)
            {
                _nullValue = value;
                if (_nullEntry == null)
                {
                    _nullEntry = new NullEntry(this);
                    _entrySet.Add(_nullEntry);
                }
                return;
            }

            Node node = _root;
            int ni = -1;
            Node prev = null;
            Node parent = null;

            // look for best match
            //charLoop:
            for (int i = 0; i < key.Length; i++)
            {
                bool charLoopContinue = false;
                char c = key[i];

                // Advance node
                if (ni == -1)
                {
                    parent = node;
                    prev = null;
                    ni = 0;
                    node = (node._children == null) ? null : node._children[c % _width];
                }

                // Loop through a node chain at the same level
                while (node != null)
                {
                    // If it is a matching node, goto next char
                    if (node._char[ni] == c || _ignoreCase && node._ochar[ni] == c)
                    {
                        prev = null;
                        ni++;
                        if (ni == node._char.Length)
                            ni = -1;
                        //continue charLoop;
                        charLoopContinue = true; break;
                    }

                    // no char match
                    // if the first char,
                    if (ni == 0)
                    {
                        // look along the chain for a char match
                        prev = node;
                        node = node._next;
                    }
                    else
                    {
                        // Split the current node!
                        node.Split(this, ni);
                        i--;
                        ni = -1;
                        //continue charLoop;
                        charLoopContinue = true; break;
                    }
                }
                if (charLoopContinue) continue;

                // We have run out of nodes, so as this is a put, make one
                node = new Node(_ignoreCase, key, i);

                if (prev != null) // add to end of chain
                    prev._next = node;
                else if (parent != null) // add new child
                {
                    if (parent._children == null)
                        parent._children = new Node[_width];
                    parent._children[c % _width] = node;
                    int oi = node._ochar[0] % _width;
                    if (node._ochar != null && node._char[0] % _width != oi)
                    {
                        if (parent._children[oi] == null)
                            parent._children[oi] = node;
                        else
                        {
                            Node n = parent._children[oi];
                            while (n._next != null)
                                n = n._next;
                            n._next = node;
                        }
                    }
                }
                else // this is the root.
                    _root = node;
                break;
            }

            // Do we have a node
            if (node != null)
            {
                // Split it if we are in the middle
                if (ni > 0)
                    node.Split(this, ni);

                node._key = key;
                node._value = value;
                _entrySet.Add(node);
                return;
            }
        }
        #endregion

        #region Indexers

        public object this[object key]
        {
            get
            {
                if (key == null)
                {
                    return _nullValue;
                }
                if (key is string)
                {
                    return this[(string)key];
                }

                return this[key.ToString()];
            }
            set
            {
                if (key == null)
                {
                    this[(string)null] = value;
                }

                if (key is string)
                {
                    this[(string)key] = value;
                }


                this[key.ToString()] = value;

            }
        }

        public new object this[string key]
        {
            get
            {
                if (key == null)
                {
                    return _nullValue;
                }

                Entry entry = GetEntry(key, 0, key.Length);
                if (entry == null)
                {
                    return null;
                }
                return entry.Value;
            }

            set
            {
                if (ContainsKey(key))
                {
                    Remove(key);
                }

                Add(key, value);

            }
        }

        #endregion

        #region Get Entry

        /// <summary>
        /// Get a map entry by substring key.
        /// </summary>
        /// <param name="key">string containing the key</param>
        /// <param name="offset">Offset of the key within the string.</param>
        /// <param name="length">The length of the key</param>
        /// <returns>The Entry for the key or null if the key is not in the map.</returns>
        public Entry GetEntry(string key, int offset, int length)
        {
            if (key == null)
                return _nullEntry;

            Node node = _root;
            int ni = -1;

            // look for best match
            for (int i = 0; i < length; i++)
            {
                bool charLoopContinue = false;

                char c = key[offset + i];

                // Advance node
                if (ni == -1)
                {
                    ni = 0;
                    node = (node._children == null) ? null : node._children[c % _width];
                }

                // Look through the node chain
                while (node != null)
                {
                    // If it is a matching node, goto next char
                    if (node._char[ni] == c || _ignoreCase && node._ochar[ni] == c)
                    {
                        ni++;
                        if (ni == node._char.Length)
                            ni = -1;

                        charLoopContinue = true; break;
                    }

                    // No char match, so if mid node then no match at all.
                    if (ni > 0) return null;

                    // try next in chain
                    node = node._next;
                }
                if (charLoopContinue) continue;

                return null;
            }

            if (ni > 0) return null;
            if (node != null && node._key == null)
                return null;
            return node;
        }

        /// <summary>
        /// Get a map entry by char array key.
        /// </summary>
        /// <param name="key">char array containing the key</param>
        /// <param name="offset">Offset of the key within the array.</param>
        /// <param name="length">The length of the key</param>
        /// <returns>The Entry for the key or null if the key is not in the map</returns>
        public Entry GetEntry(char[] key, int offset, int length)
        {
            if (key == null)
                return _nullEntry;

            Node node = _root;
            int ni = -1;

            // look for best match
            for (int i = 0; i < length; i++)
            {
                bool charLoopContinue = false;

                char c = key[offset + i];

                // Advance node
                if (ni == -1)
                {
                    ni = 0;
                    node = (node._children == null) ? null : node._children[c % _width];
                }

                // While we have a node to try
                while (node != null)
                {
                    // If it is a matching node, goto next char
                    if (node._char[ni] == c || _ignoreCase && node._ochar[ni] == c)
                    {
                        ni++;
                        if (ni == node._char.Length)
                            ni = -1;

                        charLoopContinue = true; break;
                    }

                    // No char match, so if mid node then no match at all.
                    if (ni > 0) return null;

                    // try next in chain
                    node = node._next;
                }
                if (charLoopContinue) continue;
                return null;
            }

            if (ni > 0) return null;
            if (node != null && node._key == null)
                return null;
            return node;
        }

        /// <summary>
        /// Get a map entry by byte array key, using as much of the passed key as needed for a match.
        /// A simple 8859-1 byte to char mapping is assumed.
        /// </summary>
        /// <param name="key">char array containing the key</param>
        /// <param name="offset">Offset of the key within the array.</param>
        /// <param name="maxLength">The length of the key</param>
        /// <returns>The Map.Entry for the key or null if the key is not in the map.</returns>
        public Entry GetBestEntry(byte[] key, int offset, int maxLength)
        {
            if (key == null)
                return _nullEntry;

            Node node = _root;
            int ni = -1;

            // look for best match
            for (int i = 0; i < maxLength; i++)
            {
                bool charLoopContinue = false;
                char c = (char)key[offset + i];

                // Advance node
                if (ni == -1)
                {
                    ni = 0;

                    Node child = (node._children == null) ? null : node._children[c % _width];

                    if (child == null && i > 0)
                        return node; // This is the best match
                    node = child;
                }

                // While we have a node to try
                while (node != null)
                {
                    // If it is a matching node, goto next char
                    if (node._char[ni] == c || _ignoreCase && node._ochar[ni] == c)
                    {
                        ni++;
                        if (ni == node._char.Length)
                            ni = -1;

                        charLoopContinue = true; break;
                    }

                    // No char match, so if mid node then no match at all.
                    if (ni > 0) return null;

                    // try next in chain
                    node = node._next;
                }
                if (charLoopContinue) continue;
                return null;
            }

            if (ni > 0) return null;
            if (node != null && node._key == null)
                return null;
            return node;
        }



        public new bool TryGetValue(string key, out object value)
        {
            if (!ContainsKey(key))
            {
                value = _nullEntry;
                return false;
            }

            value = this[key];
            return true;
        }

        #endregion

        #region Remove

        public bool Remove(object key)
        {
            if (key == null)
                return Remove((string)null);
            return Remove(key.ToString());
        }

        public new bool Remove(string key)
        {
            if (key == null)
            {
                if (_nullEntry != null)
                {
                    bool retval = _entrySet.Remove(_nullEntry);
                    _nullEntry = null;
                    _nullValue = null;
                    return retval;
                }
                return false;
            }

            Node node = _root;
            int ni = -1;

            // look for best match
            //charLoop:
            for (int i = 0; i < key.Length; i++)
            {
                bool charLoopContinue = false;
                char c = key[i];

                // Advance node
                if (ni == -1)
                {
                    ni = 0;
                    node = (node._children == null) ? null : node._children[c % _width];
                }

                // While we have a node to try
                while (node != null)
                {
                    // If it is a matching node, goto next char
                    if (node._char[ni] == c || _ignoreCase && node._ochar[ni] == c)
                    {
                        ni++;
                        if (ni == node._char.Length)
                            ni = -1;
                        //continue charLoop;
                        charLoopContinue = true; break;
                    }

                    // No char match, so if mid node then no match at all.
                    if (ni > 0) return false;

                    // try next in chain
                    node = node._next;
                }
                if (charLoopContinue) continue;
                return false;
            }

            if (ni > 0) return false;
            if (node != null && node._key == null)
                return false;

            bool ret = _entrySet.Remove(node);
            node._value = null;
            node._key = null;
            return ret;

        }

        #endregion

        public ICollection EntrySet
        {
            get { return _umEntrySet; }
        }

        public new int Count
        {
            get
            {
                return _entrySet.Count;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return _entrySet.Count <= 0;
            }
        }

        public bool ContainsKey(object key)
        {
            if (key == null)
                return _nullEntry != null;

            return ContainsKey(key.ToString());

        }



        public new bool ContainsKey(string key)
        {
            if (key == null)
                return _nullEntry != null;
            return
                GetEntry(key, 0, key == null ? 0 : key.Length) != null;
        }

        public new void Clear()
        {
            _root = new Node();
            _nullEntry = null;
            _nullValue = null;
            _entrySet.Clear();
        }

        public override string ToString()
        {
            IEnumerator i = EntrySet.GetEnumerator();
            if (!i.MoveNext())
                return "{}";

            StringBuilder sb = new StringBuilder();
            sb.Append('{');
            for (; ; )
            {
                Entry e = (Entry)i.Current;
                object key = e.Key;
                object value = e.Value;
                sb.Append(key == this ? "(this Map)" : key);
                sb.Append('=');
                sb.Append(value == this ? "(this Map)" : value);
                if (!i.MoveNext())
                    return sb.Append('}').ToString();
                sb.Append(", ");
            }

        }

        #region Node Entry Classes and Interface

        public interface Entry
        {
            string Key { get; }
            object Value { get; set; }
            bool Equals(Object o);
            int GetHashCode();
        }

        protected class Node : Entry
        {
            internal char[] _char;
            internal char[] _ochar;
            internal Node _next;
            internal Node[] _children;
            internal string _key;
            internal object _value;

            internal Node() { }

            internal Node(bool ignoreCase, string s, int offset)
            {
                int l = s.Length - offset;
                _char = new char[l];
                _ochar = new char[l];
                for (int i = 0; i < l; i++)
                {
                    char c = s[offset + i];
                    _char[i] = c;
                    if (ignoreCase)
                    {
                        char o = c;
                        if (char.IsUpper(c))
                            o = char.ToLower(c);
                        else if (char.IsLower(c))
                            o = char.ToUpper(c);
                        _ochar[i] = o;
                    }
                }
            }

            internal Node Split(StringMap map, int offset)
            {
                Node split = new Node();
                int sl = _char.Length - offset;

                char[] tmp = this._char;
                this._char = new char[offset];
                split._char = new char[sl];
                Buffer.BlockCopy(tmp, 0, this._char, 0, offset);
                Buffer.BlockCopy(tmp, offset, split._char, 0, sl);

                if (this._ochar != null)
                {
                    tmp = this._ochar;
                    this._ochar = new char[offset];
                    split._ochar = new char[sl];
                    Buffer.BlockCopy(tmp, 0, this._ochar, 0, offset);
                    Buffer.BlockCopy(tmp, offset, split._ochar, 0, sl);
                }

                split._key = this._key;
                split._value = this._value;
                this._key = null;
                this._value = null;
                if (map._entrySet.Remove(this))
                    map._entrySet.Add(split);

                split._children = this._children;
                this._children = new Node[map._width];
                this._children[split._char[0] % map._width] = split;
                if (split._ochar != null && this._children[split._ochar[0] % map._width] != split)
                    this._children[split._ochar[0] % map._width] = split;

                return split;
            }

            public override string ToString()
            {
                StringBuilder buf = new StringBuilder();
                ToString(buf);
                return buf.ToString();
            }

            private void ToString(StringBuilder buf)
            {
                buf.Append("{[");
                if (_char == null)
                    buf.Append('-');
                else
                    for (int i = 0; i < _char.Length; i++)
                        buf.Append(_char[i]);
                buf.Append(':');
                buf.Append(_key);
                buf.Append('=');
                buf.Append(_value);
                buf.Append(']');
                if (_children != null)
                {
                    for (int i = 0; i < _children.Length; i++)
                    {
                        buf.Append('|');
                        if (_children[i] != null)
                            _children[i].ToString(buf);
                        else
                            buf.Append("-");
                    }
                }
                buf.Append('}');
                if (_next != null)
                {
                    buf.Append(",\n");
                    _next.ToString(buf);
                }
            }

            #region Entry Members

            public string Key
            {
                get
                {
                    return _key;
                }
            }

            public object Value
            {
                get
                {
                    return _value;
                }
                set
                {
                    _value = value;
                }
            }

            #endregion
        }

        protected class NullEntry : Entry
        {
            StringMap _stringMap;
            public NullEntry(StringMap map)
            {
                _stringMap = map;
            }

            public override string ToString()
            {
                return "[:null=" + _stringMap._nullValue + "]";
            }

            #region Entry Members

            public string Key
            {
                get
                {
                    return null;
                }
            }

            public object Value
            {
                get
                {
                    return _stringMap._nullValue;
                }
                set
                {
                    _stringMap._nullValue = value;
                }
            }

            #endregion
        }


        public class ReadOnlyHashSet : ReadOnlyCollectionBase
        {
            HashSet<object> _hash;
            public ReadOnlyHashSet(HashSet<object> hash)
            {
                _hash = hash;
            }


            public override int Count { get { return _hash.Count; } }
            public override IEnumerator GetEnumerator()
            {
                return _hash.GetEnumerator();
            }
        }

        #endregion



        public void WriteExternal(Stream output)
        {
            List<object[]> list = new List<object[]>();
            foreach (object obj in EntrySet)
            {
                Entry entry = (Entry)obj;
                list.Add(new object[] { entry.Key, entry.Value });
            }

            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(output, new object[] { _ignoreCase, list });
        }

        public void ReadExternal(Stream input)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            object[] objArr = (object[])formatter.Deserialize(input);
            bool ic = (bool)objArr[0];
            List<object[]> list = (List<object[]>)objArr[1];

            IgnoreCase = ic;

            foreach (object[] entry in list)
            {
                if (entry != null)
                {
                    this.Add(entry[0], entry[1]);
                }
            }

        }
    }
}

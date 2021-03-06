﻿#region Apache License
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
using NJetty.Commons;

namespace NJetty.Util.Util
{

    /// <summary>
    /// Circualar Array Based Queue, the array is grawable.
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// November 2008
    /// </date>
    public class ArrayQueue<T>// : IList<T>
    {
        const int DEFAULT_CAPACITY = 64;
        const int DEFAULT_GROWTH = 32;
        object _lock = new object();
        T[] _elements;
        int _nextE;
        int _nextSlot;
        int _size;
        int _growCapacity;

        #region Constructors

        public ArrayQueue()
        {
            _elements = new T[DEFAULT_CAPACITY];
            _growCapacity = 32;
        }

        public ArrayQueue(int capacity)
        {
            _elements = new T[capacity];
            _growCapacity = -1;
        }

        public ArrayQueue(int initCapacity, int growBy)
        {
            _elements = new T[initCapacity];
            _growCapacity = growBy;
        }

        public ArrayQueue(int initCapacity, int growBy, object objectLock)
        {
            _elements = new T[initCapacity];
            _growCapacity = growBy;
            _lock = objectLock;
        }

        #endregion

        #region CustomImplementations

        public int Capacity
        {
            get { return _elements.Length; }
        }


        

        public T Element()
        {
            lock (_lock)
            {
                if (_nextSlot == _nextE)
                    throw new NoSuchElementException();
                return _elements[_nextE];
            }
        }


        public bool Offer(T item)
        {
            lock (_lock)
            {
                _size++;
                _elements[_nextSlot++] = item;
                if (_nextSlot == _elements.Length)
                {
                    _nextSlot = 0;
                }
                if (_nextSlot == _nextE)
                {
                    if (_growCapacity <= 0)
                        return false;

                    T[] elements = new T[_elements.Length + _growCapacity];

                    int split = _elements.Length - _nextE;
                    if (split > 0)
                    {
                        Array.Copy(_elements, _nextE, elements, 0, split);
                    }
                    if (_nextE != 0)
                    {
                        Array.Copy(_elements, 0, elements, split, _nextSlot);
                    }

                    _elements = elements;
                    _nextE = 0;
                    _nextSlot = _size;
                }

                return true;
            }
        }


        public T Peek()
        {
            lock (_lock)
            {
                if (_nextSlot == _nextE)
                    return default(T);
                return _elements[_nextE];
            }
        }

        public T Poll()
        {
            return Dequeue();
        }

        public T Dequeue()
        {
            lock (_lock)
            {
                if (_size == 0)
                    return default(T);
                T e = _elements[_nextE];
                _elements[_nextE] = default(T);
                _size--;
                if (++_nextE == _elements.Length)
                    _nextE = 0;
                return e;
            }
        }

        public T Remove()
        {
            lock (_lock)
            {
                if (_nextSlot == _nextE)
                    throw new NoSuchElementException();
                T e = _elements[_nextE++];
                if (_nextE == _elements.Length)
                    _nextE = 0;
                return e;
            }
        }

        public bool IsEmpty
        {
            get
            {
                lock (_lock)
                {
                    return _size == 0;
                }
            }
        }


        public T GetUnsafe(int index)
        {
            int i = _nextE + index;
            if (i >= _elements.Length)
                i -= _elements.Length;
            return _elements[i];
        }

        public override string ToString()
        {
            List<string> list = new List<string>();
            foreach (T item in this)
            {
                list.Add(item.ToString());
            }

            return LazyList.ToString(list);
        }


        #endregion

        #region Helper

        protected void Grow()
        {
            if (_growCapacity <= 0)
            {
                throw new InvalidOperationException("Full");
            }

            T[] elements = new T[_elements.Length + _growCapacity];

            int split = _elements.Length - _nextE;

            if (split > 0)
            {
                Array.Copy(_elements, _nextE, elements, 0, split);
            }

            if (_nextE != 0)
            {
                Array.Copy(_elements, 0, elements, split, _nextSlot);
            }

            _elements = elements;
            _nextE = 0;
            _nextSlot = _size;
        }

        #endregion

        #region IList<E> Members

        public int IndexOf(T item)
        {
            if (item == null)
            {
                return -1;
            }

            lock (_lock)
            {
                for (int i = 0; i < _size; i++)
                {
                    if (item.Equals(GetUnsafe(i)))
                    {
                        return i;
                    }
                }

                return -1;
            }

        }

        public void Insert(int index, T item)
        {
            lock (_lock)
            {
                if (index < 0 || index > _size)
                    throw new IndexOutOfRangeException("!(" + 0 + "<" + index + "<=" + _size + ")");

                if (index == _size)
                {
                    Add(item);
                }
                else
                {
                    int i = _nextE + index;
                    if (i >= _elements.Length)
                        i -= _elements.Length;

                    _size++;
                    _nextSlot++;
                    if (_nextSlot == _elements.Length)
                        _nextSlot = 0;

                    if (_nextSlot == _nextE)
                        Grow();

                    if (i < _nextSlot)
                    {
                        // 0                         _elements.length
                        //       _nextE.....i..._nextSlot
                        // 0                         _elements.length
                        // ..i..._nextSlot   _nextE..........
                        Array.Copy(_elements, i, _elements, i + 1, _nextSlot - i);
                        _elements[i] = item;
                    }
                    else
                    {
                        // 0                         _elements.length
                        // ......_nextSlot   _nextE.....i....
                        if (_nextSlot > 0)
                        {
                            Array.Copy(_elements, 0, _elements, 1, _nextSlot);
                            _elements[0] = _elements[_elements.Length - 1];
                        }

                        Array.Copy(_elements, i, _elements, i + 1, _elements.Length - i - 1);
                        _elements[i] = item;
                    }
                }
            }
        }


        

        public T this[int index]
        {
            get
            {
                lock (_lock)
                {
                    if (index < 0 || index >= _size)
                        throw new IndexOutOfRangeException("!(" + 0 + "<" + index + "<=" + _size + ")");
                    int i = _nextE + index;
                    if (i >= _elements.Length)
                        i -= _elements.Length;
                    return _elements[i];
                }
            }
            set
            {
                lock (_lock)
                {
                    if (index < 0 || index >= _size)
                        throw new IndexOutOfRangeException("!(" + 0 + "<" + index + "<=" + _size + ")");

                    int i = _nextE + index;
                    if (i >= _elements.Length)
                        i -= _elements.Length;
                    T old = _elements[i];
                    _elements[i] = value;
                }
            }
        }

        #endregion

        #region ICollection<E> Members



        public void AddUnsafe(T item)
        {
            _size++;
            _elements[_nextSlot++] = item;
            if (_nextSlot == _elements.Length)
            {
                _nextSlot = 0;
            }
            if (_nextSlot == _nextE)
            {
                Grow();
            }
        }


        public void Add(T item)
        {
            Enqueue(item);
        }

        public int Enqueue(T item)
        {
            lock (_lock)
            {
                AddUnsafe(item);

                return _size;
            }
        }

        public void Clear()
        {
            lock (_lock)
            {
                _size = 0;
                _nextE = 0;
                _nextSlot = 0;
            }
        }

        public bool Contains(T item)
        {


            lock (_lock)
            {
                if (item == null)
                {
                    throw new ArgumentNullException();
                }

                for (int i = 0; i < _size; i++)
                {
                    if (item.Equals(GetUnsafe(i)))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        //public void CopyTo(T[] array, int arrayIndex)
        //{
        //    throw new NotImplementedException();
        //}

        public int Count
        {
            get
            {
                lock (_lock) 
                { 
                    return _size; 
                }
            }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        void RemoveRealIndex(int i)
        {
            if (i < _nextSlot)
            {
                // copy the array overwriting 
                // i with i+1 to (i+n) with (n+1) until with the length of (_nextSlot-1)
                // in other words move the array from i+1 one step backward until (_nextSlot-1)
                Array.Copy(_elements, i + 1, _elements, i, _nextSlot - i);

                _nextSlot--;
                _size--;
            }
            else
            {
                // copy the array overwriting 
                // i with i+1 to (i+n) with (n+1) until with the length of ((_elements.Length-i)-1)
                // in other words move the array from i+1 one step backward until ((_elements.Length-i)-1)
                Array.Copy(_elements, i + 1, _elements, i, (_elements.Length - i) - 1);
                if (_nextSlot > 0)
                {
                    _elements[_elements.Length-1] = _elements[0];
                    Array.Copy(_elements, 1, _elements, 0, _nextSlot - 1);
                    _nextSlot--;
                }
                else
                {
                    _nextSlot = _elements.Length - 1;
                }

                _size--;
            }
        }


        public void RemoveAt(int index)
        {
            lock (_lock)
            {
                if (index < 0 || index >= _size)
                    throw new IndexOutOfRangeException("!(" + 0 + "<" + index + "<=" + _size + ")");

                int i = _nextE + index;
                if (i >= _elements.Length)
                    i -= _elements.Length;

                RemoveRealIndex(i);
            }
        }

        public bool Remove(T item)
        {
            lock (_lock)
            {
                int i = Array.IndexOf(_elements, item);
                if (i < 0)
                {
                    return false;
                }

                RemoveRealIndex(i);
                return true;
            }
        }

        #endregion


        // Iterator method.
        public IEnumerator GetEnumerator()
        {
            lock (_lock)
            {
                for (int i = 0; i < _size; i++)
                {
                    yield return GetUnsafe(i);
                }
            }
        }

        //#region IEnumerable<E> Members

        //public IEnumerator<T> GetEnumerator()
        //{
        //    throw new NotImplementedException();
        //}

        //#endregion

        //#region IEnumerable Members

        //IEnumerator System.Collections.IEnumerable.GetEnumerator()
        //{
        //    throw new NotImplementedException();
        //}

        //#endregion
    }
}

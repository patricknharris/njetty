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
using NJetty.Util.Component;
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
    public static class LazyList
    {

        private static readonly string[] __EMTPY_STRING_ARRAY = new string[0];

        #region Add To LazyList

        /// <summary>
        /// Add an item to a LazyList
        /// </summary>
        /// <typeparam name="E">Generics of The List to be created</typeparam>
        /// <param name="list">list The list to add to or null if none yet created.</param>
        /// <param name="item">list where the item to add.</param>
        /// <returns>The lazylist created or added to.</returns>
        public static object Add<E>(object list, E item)
        {
            if (list == null)
            {
                if (item is List<E> || item == null)
                {
                    List<E> l = new List<E>();
                    l.Add(item);
                    return l;
                }

                return item;
            }

            if (list is List<E>)
            {
                ((List<E>)list).Add(item);
                return list;
            }

            List<object> lobject = new List<object>();
            lobject.Add(list);
            lobject.Add(item);
            return lobject;
        }

        /// <summary>
        /// Add an item to a LazyList
        /// </summary>
        /// <typeparam name="E">Generics of The List to be created</typeparam>
        /// <param name="list">list The list to add to or null if none yet created.</param>
        /// <param name="index">index The index to add the item at.</param>
        /// <param name="item">item The item to add.</param>
        /// <returns>The lazylist created or added to.</returns>
        public static object Add<E>(object list, int index, E item)
        {
            if (list == null)
            {
                if (index > 0 || item is List<E> || item == null)
                {
                    List<E> l = new List<E>();
                    l.Insert(index, item);
                    return l;
                }
                return item;
            }

            if (list is List<E>)
            {
                ((List<E>)list).Insert(index, item);
                return list;
            }

            List<object> lobject = new List<object>();
            lobject.Add(list);
            lobject.Insert(index, item);
            return lobject;
        }


        #endregion


        #region Add Collection To LazyList


        /// <summary>
        /// Add the contents of a Collection to a LazyList
        /// </summary>
        /// <typeparam name="E">Generics of The List to be created</typeparam>
        /// <param name="list">The list to add to or null if none yet created.</param>
        /// <param name="collection">The Collection whose contents should be added.</param>
        /// <returns>The lazylist created or added to.</returns>
        public static object AddCollection<E>(object list, ICollection<E> collection)
        {
            foreach (E item in collection)
            {
                list = LazyList.Add(list, item);
            }
            return list;
        }

        /// <summary>
        /// Add the contents of an array to a LazyList
        /// </summary>
        /// <param name="list">The list to add to or null if none yet created.</param>
        /// <param name="array">The Array whose contents should be added.</param>
        /// <returns>The lazylist created or added to.</returns>
        public static object AddArray(object list, Array array)
        {
            for (int i = 0; array != null && i < array.Length; i++)
                list = LazyList.Add(list, array.GetValue(i));
            return list;
        }

        #endregion


        #region Ensure the capcity of list

        public static object EnsureSize(object list, int initialSize)
        {
            if (list == null)
            {
                return new List<object>(initialSize);
            }



            if (list is IList)
            {
                Type t = list.GetType();

                IList ol = (IList)list;


                if (ol.Count > initialSize)
                {
                    return ol;
                }

                IList nl = (IList)Activator.CreateInstance(list.GetType(), initialSize); //new ArrayList<Object>(initialSize);
                foreach (object o in ol)
                {
                    nl.Add(o);
                }


                return nl;
            }

            List<object> lobject = new List<object>(initialSize);
            lobject.Add(list);
            return lobject;
        }

        #endregion


        #region Remove From LazyList
        public static object Remove(object list, object o)
        {
            if (list == null)
                return null;

            if (list is IList)
            {
                IList l = (IList)list;
                l.Remove(o);
                if (l.Count == 0)
                {
                    return null;
                }
                return list;
            }

            if (list.Equals(o))
                return null;
            return list;
        }

        public static object Remove(object list, int i)
        {
            if (list == null)
                return null;

            if (list is IList)
            {
                IList l = (IList)list;
                l.RemoveAt(i);
                if (l.Count == 0)
                    return null;
                return list;
            }

            if (i == 0)
                return null;
            return list;
        }

        #endregion

        #region Get the real List from a LazyList


        /// <summary>
        /// Get the real List from a LazyList.
        /// </summary>
        /// <typeparam name="E">Generics of The List to be created</typeparam>
        /// <param name="list">A LazyList returned from LazyList.Add(object)</param>
        /// <returns>The List of added items, which may be an EMPTY_LIST or a single entry list</returns>
        public static List<E> GetList<E>(object list)
        {
            return GetList<E>(list, false);
        }


        /// <summary>
        /// Get the real List from a LazyList.
        /// </summary>
        /// <typeparam name="E">Generics of The List to be created</typeparam>
        /// <param name="list">A LazyList returned from LazyList.Add(object) or null</param>
        /// <param name="nullForEmpty">If true, null is returned instead of an empty list.</param>
        /// <returns>The List of added items, which may be an EMPTY_LIST or a single entry list</returns>
        public static List<E> GetList<E>(object list, bool nullForEmpty)
        {
            if (list == null)
            {
                if (nullForEmpty)
                    return null;
                return new List<E>(); // return an empty list
            }
            if (list is List<E>)
                return (List<E>)list;



            List<E> l = new List<E>();
            l.Add((E)list);

            return l;
        }



        #endregion

        #region To String Array Helper

        /// <summary>
        /// Converts a List to an Array of Strings
        /// </summary>
        /// <param name="list">list of object you want to convert to an Array of strings</param>
        /// <returns>returns an array of element.ToStrings() from list or returns a Single element Array with the list.ToString() value </returns>
        public static string[] ToStringArray(object list)
        {
            if (list == null)
            {
                return __EMTPY_STRING_ARRAY;
            }

            if (list is IList)
            {
                IList l = (IList)list;
                string[] a = new string[l.Count];
                for (int i = l.Count; i-- > 0; )
                {
                    object o = l[i];
                    if (o != null)
                    {
                        a[i] = o.ToString();
                    }
                }
                return a;
            }

            return new string[] { list.ToString() };
        }

        #endregion


        // TODO Add other lazy lists





        #region Add/Remove Array Helper

        /// <summary>
        /// Add an Item to An Array
        /// </summary>
        /// <param name="array">the array to add and can also be null</param>
        /// <param name="item">the item to add</param>
        /// <param name="type">if array is null, the type will be used to create an instance of an array</param>
        /// <returns>a new array with contents plus the item</returns>
        public static Array AddToArray(Array array, object item, Type type)
        {
            if (array == null)
            {
                if (type == null && item != null)
                {
                    type = item.GetType();
                }

                Array na = Array.CreateInstance(type, 1);
                na.SetValue(item, 0);
                return na;
            }
            else
            {
                type = array.GetType().GetElementType();
                Array na = Array.CreateInstance(type, array.Length + 1);
                Array.Copy(array, na, array.Length);
                na.SetValue(item, array.Length);
                return na;
            }
        }

        /// <summary>
        /// Removes an Item in an array
        /// </summary>
        /// <param name="array">Source Array</param>
        /// <param name="item">item to remove</param>
        /// <returns>a new array with contents minus the item</returns>
        public static Array RemoveFromArray(Array array, object item)
        {
            if (item == null || array == null)
            {
                return array;
            }

            for (int i = array.Length; i-- > 0; )
            {
                if (item.Equals(array.GetValue(i)))
                {
                    Type type = array.GetType().GetElementType();
                    Array na = Array.CreateInstance(type, array.Length - 1);
                    if (i > 0)
                    {
                        Array.Copy(array, na, i);
                    }

                    if (i + 1 < array.Length)
                    {
                        Array.Copy(array, i + 1, na, i, array.Length - (i + 1));
                    }
                    return na;
                }
            }
            return array;

        }

        #endregion





    }
}

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


        public static object Add(object _listeners, IContainerListener listener)
        {
            throw new NotImplementedException();
        }

        public static object Remove(object _listeners, IContainerListener listener)
        {
            throw new NotImplementedException();
        }
    }
}

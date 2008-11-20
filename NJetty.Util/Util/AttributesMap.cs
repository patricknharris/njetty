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

    public class AttributesMap : IAttributes
    {
        Dictionary<string, object> _map;


        #region Constructors

        public AttributesMap()
        {
            _map = new Dictionary<string, object>();
        }

        public AttributesMap(Dictionary<string, object> map)
        {
            _map = map;
        }

        #endregion


        #region IAttributes Members

        public void RemoveAttribute(string name)
        {
            _map.Remove(name);
        }

        public void SetAttribute(string name, object attribute)
        {
            if (attribute == null)
            {
                _map.Remove(name);
            }
            else
            {
                _map.Add(name, attribute);
            }
        }

        public object GetAttribute(string name)
        {
            if (name != null && _map.ContainsKey(name))
            {
                return _map[name];
            }

            return null;
        }

        public IEnumerable GetAttributeNames()
        {
            if (_map.Keys != null)
            {
                foreach (string item in _map.Keys)
                {
                    yield return item;
                }
            }
        }

        public void ClearAttributes()
        {
            _map.Clear();
        }

        #endregion



        public static IEnumerable GetAttributeNamesCopy(IAttributes attrs)
        {
            if (attrs is AttributesMap)
            {
                if (((AttributesMap)attrs)._map.Keys != null)
                {
                    foreach (string item in ((AttributesMap)attrs)._map.Keys)
                    {
                        yield return item;
                    }
                }

                yield break;

            }

            //List<string> names = new List<string>();
            foreach (string name in attrs.GetAttributeNames())
            {
                yield return name;
            }

            yield break;
        }



        public override string ToString()
        {
            return _map.ToString();
        }
    }
}

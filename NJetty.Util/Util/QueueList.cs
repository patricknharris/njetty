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
using NJetty.Util.Logger;

namespace NJetty.Util.Util
{
    
    /// <summary>
    /// Thread Safe Queue Implementation.
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// November 2008
    /// </date>
    public class QueueList<T> : List<T>
    {
        object _thisLock = new object();
        
        public QueueList()
            : base()
        {

        }


        public QueueList(int capacity) : base(capacity) 
        {
            
        }

        public T Dequeue()
        {
            lock (_thisLock)
            {
                if (Count == 0)
                {
                    return default(T);
                }

                T item = this[0];
                this.Remove(item);
                return item;
            }

        }

        public int Enqueue(T item)
        {
            lock (_thisLock)
            {
                this.Add(item);
                return this.Count;
            }
        }

        public void Remove(T item)
        {
            lock (_thisLock)
            {
                base.Remove(item);
            }
        }

        public int Count
        {
            get
            {
                lock (_thisLock)
                {
                    return base.Count;
                }
            }
        }

    }
}

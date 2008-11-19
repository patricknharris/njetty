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

namespace NJetty.Util.Thread
{

    /// <summary>
    /// Thread Pooling Interface, for implementing a thread pool
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// November 2008
    /// </date>
    public class Timeout
    {

        
        internal object _lock;
        long _duration;
        internal long _now=System.DateTime.Now.TimeOfDay.Milliseconds;
        TimeoutTask _head=new TimeoutTask();

        public Timeout()
        {
            _lock=new object();
            _head._timeout=this;
        }

        public Timeout(object lockObject)
        {
            _lock = lockObject;
            _head._timeout=this;
        }

        
        /// <summary>
        /// Gets or Sets the duration 
        /// </summary>
 
        public long Duration
        {
            set {_duration =value;}
            get{return _duration;}
        }


        public long Now
        {
            get
            {
                lock (_lock)
                {
                    return _now;
                }
            }
            set
            {
                lock (_lock)
                {
                    _now = value;
                }
            }
        }

        public long SetNow()
        {
            lock(_lock)
            {
                _now=System.DateTime.Now.TimeOfDay.Milliseconds;
                return _now; 
            }
        }

        /// <summary>
        /// Get an expired tasks.
        /// This is called instead of Tick() to obtain the next
        /// expired Task, but without calling it's Task#Expire() or
        /// Task#Expired() methods.
        /// </summary>
        /// <returns>the next expired task or null.</returns>
        public TimeoutTask Expired()
        {
            lock (_lock)
            {
                long _expiry = _now-_duration;

                if (_head._next!=_head)
                {
                    TimeoutTask task = _head._next;
                    if (task._timestamp>_expiry)
                        return null;

                    task.Unlink();
                    task._expired=true;
                    return task;
                }
                return null;
            }
        }

        public void Tick(long now)
        {
            long _expiry = -1;

            TimeoutTask task=null;
            while (true)
            {
                try
                {
                    lock (_lock)
                    {
                        if (_expiry==-1)
                        {
                            if (now!=-1)
                                _now=now;
                            _expiry = _now-_duration;
                        }
                            
                        task= _head._next;
                        if (task==_head || task._timestamp>_expiry)
                            break;
                        task.Unlink();
                        task._expired=true;
                        task.Expire();
                    }
                    
                    task.Expired();
                }
                catch(Exception exception)
                {
                    Log.Warn(Log.EXCEPTION, exception);
                }
            }
        }

        public void Tick()
        {
            Tick(-1);
        }

        public void Schedule(TimeoutTask task)
        {
            Schedule(task,0L);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="task">task</param>
        /// <param name="delay">A delay in addition to the default duration of the timeout</param>
        public void Schedule(TimeoutTask task,long delay)
        {
            lock (_lock)
            {
                if (task._timestamp!=0)
                {
                    task.Unlink();
                    task._timestamp=0;
                }
                task._timeout=this;
                task._expired=false;
                task._delay=delay;
                task._timestamp = _now+delay;

                TimeoutTask last=_head._prev;
                while (last!=_head)
                {
                    if (last._timestamp <= task._timestamp)
                        break;
                    last=last._prev;
                }
                last.Link(task);
            }
        }


        public void CancelAll()
        {
            lock (_lock)
            {
                _head._next=_head._prev=_head;
            }
        }

        public bool IsEmpty
        {
            get
            {
                lock (_lock)
                {
                    return _head._next == _head;
                }
            }
        }

        public long TimeToNext
        {
            get
            {
                lock (_lock)
                {
                    if (_head._next == _head)
                        return -1;
                    long to_next = _duration + _head._next._timestamp - _now;
                    return to_next < 0 ? 0 : to_next;
                }
            }
        }

        public override string ToString()
        {
            StringBuilder buf = new StringBuilder();
            buf.Append(base.ToString());

            TimeoutTask task = _head._next;
            while (task != _head)
            {
                buf.Append("-->");
                buf.Append(task);
                task = task._next;
            }

            return buf.ToString();
        }

    }
}

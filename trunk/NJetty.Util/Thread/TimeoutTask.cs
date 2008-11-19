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

namespace NJetty.Util.Thread
{
    /// <summary>
    /// Task.
    /// The base class for scheduled timeouts.  This class should be
    /// extended to implement the expire() method, which is called if the
    /// timeout expires.
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// November 2008
    /// </date>


    public class TimeoutTask
    {
        internal TimeoutTask _next;
        internal TimeoutTask _prev;
        internal Timeout _timeout;
        internal long _delay;
        internal long _timestamp=0;
        internal bool _expired=false;

        public TimeoutTask()
        {
            _next=_prev=this;
        }

        public long Timestamp
        {
            get { return _timestamp; }
        }

        public long Age
        {
            get
            {
                Timeout t = _timeout;
                if (t != null && t.Now != 0 && _timestamp != 0)
                    return t.Now - _timestamp;
                return 0;
            }
        }

        internal void Unlink()
        {
            _next._prev=_prev;
            _prev._next=_next;
            _next=_prev=this;
            _expired=false;
        }

        internal void Link(TimeoutTask task)
        {
            TimeoutTask next_next = _next;
            _next._prev=task;
            _next=task;
            _next._next=next_next;
            _next._prev=this;   
        }
        
        
        /// <summary>
        /// Schedule the task on the given timeout.
        /// The task exiry will be called after the timeout duration. 
        /// </summary>
        /// <param name="timer"></param>
        public void Schedule(Timeout timer)
        {
            timer.Schedule(this);
        }
        
        
        /// <summary>
        /// Schedule the task on the given timeout.
        /// The task exiry will be called after the timeout duration.
        /// </summary>
        /// <param name="timer"></param>
        /// <param name="delay"></param>
        public void Schedule(Timeout timer, long delay)
        {
            timer.Schedule(this,delay);
        }
        
        
        /// <summary>
        /// Reschedule the task on the current timeout.
        /// The task timeout is rescheduled as if it had been cancelled and
        /// scheduled on the current timeout. 
        /// </summary>
        public void Reschedule()
        {
            Timeout timeout = _timeout;
            if (timeout!=null)
                timeout.Schedule(this,_delay);
        }
        
        /// <summary>
        /// Cancel the task.
        /// Remove the task from the timeout.
        /// </summary>
        public void Cancel()
        {
            Timeout timeout = _timeout;
            if (timeout!=null)
            {
                lock (timeout._lock)
                {
                    Unlink();
                    _timestamp=0;
                }
            }
        }
        
        public bool IsExpired 
        { 
            get { return _expired; } 
        }

        public bool IsScheduled { get { return _next != this; } }
        
        /// <summary>
        /// Expire task.
        /// This method is called when the timeout expires. It is called
        /// in the scope of the lock block (on this) that sets 
        /// the property IsExpired state to true.
        /// </summary>
        public virtual void Expire(){}

        /// <summary>
        /// Expire task. This method is called when the timeout expires. 
        /// It is called outside of any synchronization scope and may be delayed. 
        /// </summary>
        public virtual void Expired() { }

    }
}

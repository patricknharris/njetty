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
using NJetty.Util.Logging;
using NJetty.Util.Util;

namespace NJetty.Util.Component
{
    /// <summary>
    /// Abstract implementation of ILifeCycle component.
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// November 2008
    /// </date>
    public class AbstractLifeCycle : ILifeCycle
    {
        object _lock = new object();
        //const int FAILED = -1, STOPPED = 0, STARTING = 1, STARTED = 2, STOPPING = 3;
        enum LifeCycleState
        {
            FAILED = -1,
            STOPPED = 0,
            STARTING = 1,
            STARTED = 2,
            STOPPING = 3
        }


        [NonSerialized]
        LifeCycleState _state = LifeCycleState.STOPPED;
        protected IListener[] _listeners;

        protected virtual void DoStart()
        {
        }

        protected virtual void DoStop()
        {
        }

        public void Start()
        {
            lock (_lock)
            {
                try
                {
                    if (_state == LifeCycleState.STARTED || _state == LifeCycleState.STARTING)
                        return;
                    SetStarting();
                    DoStart();
                    Log.Debug("started {0}", this);
                    SetStarted();
                }
                catch (SystemException e)
                {
                    Log.Warn("failed " + this, e);
                    SetFailed(e);
                    throw e;
                }
                catch (Exception e)
                {
                    Log.Warn("failed " + this, e);
                    SetFailed(e);
                    throw e;
                }
            }
        }

        public void Stop()
        {
            lock (_lock)
            {
                try
                {
                    if (_state == LifeCycleState.STOPPING || _state == LifeCycleState.STOPPED)
                        return;
                    SetStopping();
                    DoStop();
                    Log.Debug("stopped {0}", this);
                    SetStopped();
                }
                catch (SystemException e)
                {
                    Log.Warn("failed " + this, e);
                    SetFailed(e);
                    throw e;
                }
                catch (Exception e)
                {
                    Log.Warn("failed " + this, e);
                    SetFailed(e);
                    throw e;
                }
            }
        }

        public bool IsRunning
        {
            get { return _state == LifeCycleState.STARTED || _state == LifeCycleState.STARTING; }
        }

        public bool IsStarted
        {
            get { return _state == LifeCycleState.STARTED; }
        }

        public bool IsStarting
        {
            get { return _state == LifeCycleState.STARTING; }
        }

        public bool IsStopping
        {
            get { return _state == LifeCycleState.STOPPING; }
        }

        public bool IsStopped
        {
            get { return _state == LifeCycleState.STOPPED; }
        }

        public bool IsFailed
        {
            get { return _state == LifeCycleState.FAILED; }
        }

        public void AddLifeCycleListener(IListener listener)
        {
            _listeners = (IListener[])LazyList.AddToArray(_listeners, listener, typeof(IListener));
        }

        public void RemoveLifeCycleListener(IListener listener)
        {
            LazyList.RemoveFromArray(_listeners, listener);
        }

        private void SetStarted()
        {
            _state = LifeCycleState.STARTED;
            if (_listeners != null)
            {

                foreach (IListener listener in _listeners)
                {
                    listener.LifeCycleStarted(this);
                }
            }
        }

        private void SetStarting()
        {
            _state = LifeCycleState.STARTING;
            if (_listeners != null)
            {

                foreach (IListener listener in _listeners)
                {
                    listener.LifeCycleStarting(this);
                }
            }
        }

        private void SetStopping()
        {
            _state = LifeCycleState.STOPPING;
            
            if (_listeners != null)
            {
                foreach (IListener listener in _listeners)
                {
                    listener.LifeCycleStopping(this);
                }
            }
        }

        private void SetStopped()
        {
            _state = LifeCycleState.STOPPED;
            if (_listeners != null)
            {
                foreach (IListener listener in _listeners)
                {
                    listener.LifeCycleStopped(this);
                }
            }
        }

        private void SetFailed(Exception error)
        {
            _state = LifeCycleState.FAILED;
            if (_listeners != null)
            {
                foreach (IListener listener in _listeners)
                {
                    listener.LifeCycleFailure(this, error);
                }
            }
        }
    }
}

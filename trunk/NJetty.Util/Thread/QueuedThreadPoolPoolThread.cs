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
using System.Threading;
using NJetty.Util.Logger;

namespace NJetty.Util.Thread
{

    /// <summary>
    /// A pool of threads.
    ///
    /// Avoids the expense of thread creation by pooling threads after
    /// their run methods exit for reuse.
    ///
    /// If an idle thread is available a job is directly dispatched,
    /// otherwise the job is queued.  After queuing a job, if the total
    /// number of threads is less than the maximum pool size, a new thread 
    /// is spawned.
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// November 2008
    /// </date>
    internal class QueuedThreadPoolPoolThread : IThread
    {
        ThreadStart _job = null;
        QueuedThreadPool _queuedThreadPool = null;
        object _thisLock = new object();
        System.Threading.Thread _thread = null;

        /* ------------------------------------------------------------ */
        internal QueuedThreadPoolPoolThread(QueuedThreadPool queuedThreadPool)
        {
            _queuedThreadPool = queuedThreadPool;


            ThreadStart ts = new ThreadStart(Run);
            _thread = new System.Threading.Thread(ts);
            _thread.Priority = _queuedThreadPool._priority;
            _thread.IsBackground = _queuedThreadPool._background;

            
            
            
            
            
        }

        public int Id
        {
            get { return _thread.ManagedThreadId; }
        }


        public string Name
        {
            get { return _thread.Name; }
            set { _thread.Name = value; }
        }

        public void Start()
        {
            //lock (_thisLock)
            //{
                _thread.Start();
            //}
        }

        public void Interrupt()
        {
            //lock (_thisLock)
            //{
                _thread.Interrupt();
            //}
        }

        
        /* ------------------------------------------------------------ */
        /** QueuedThreadPoolPoolThread run.
         * Loop getting jobs and handling them until idle or stopped.
         */
        public void Run()
        {
            bool idle=false;
            ThreadStart job=null;
            try
            {
                while (_queuedThreadPool.IsRunning)
                {   
                    // Run any job that we have.
                    if (job!=null)
                    {
                        ThreadStart todo=job;
                        job=null;
                        idle=false;
                        // Execute the Delegated job
                        todo();
                        Log.Warn("heheh ni exit");
                    }

                    lock (_queuedThreadPool._lock)
                    {
                        // is there a queued job?
                        if (_queuedThreadPool._queued > 0)
                        {
                            // de-que
                            _queuedThreadPool._queued--;

                            // assign to the next job to execute
                            job = _queuedThreadPool._jobs[_queuedThreadPool._nextJob++];
                            if (_queuedThreadPool._nextJob == _queuedThreadPool._jobs.Length)
                            {
                                // reset to 0 if there no more jobs todo
                                _queuedThreadPool._nextJob = 0;
                            }
                            continue;
                        }

                        // Should we shrink?
                        int threads = _queuedThreadPool._threads.Count;
                        if (threads > _queuedThreadPool._minThreads &&
                            (threads > _queuedThreadPool._maxThreads ||
                             _queuedThreadPool._idle.Count > _queuedThreadPool._spawnOrShrinkAt))   
                        {
                            long now = System.DateTime.Now.ToFileTime();
                            if ((now - _queuedThreadPool._lastShrink) > _queuedThreadPool.MaxIdleTimeMs)
                            {
                                _queuedThreadPool._lastShrink = now;
                                _queuedThreadPool._idle.Remove(this);
                                return;
                            }
                        }

                        if (!idle)
                        {   
                            // Add ourselves to the idle set.
                            _queuedThreadPool._idle.Add(this);
                            idle=true;
                        }
                    }

                    // We are idle
                    // wait for a dispatched job
                    lock (_thisLock)
                    {
                        if (_job == null)
                        {
                            _thread.Join(_queuedThreadPool.MaxIdleTimeMs);
                        }
                            
                        job=_job;
                        _job=null;
                    }
                }
            }
            catch (ThreadInterruptedException e)
            {
                Log.Ignore(e);
            }
            finally
            {
                lock (_queuedThreadPool._lock)
                {
                    _queuedThreadPool._idle.Remove(this);
                }
                lock (_queuedThreadPool._threadsLock)
                {
                    _queuedThreadPool._threads.Remove(this);
                }
                lock (_thisLock)
                {
                    job=_job;
                }
                
                // we died with a job! reschedule it
                if (job!=null)
                {
                    _queuedThreadPool.Dispatch(job);
                }
            }
        }
        
        internal void Dispatch(ThreadStart job)
        {
            lock (_thisLock)
            {
                _job=job;
                _thread.Interrupt();
            }
        }
    
    }
}

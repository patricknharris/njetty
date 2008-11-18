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
using System.Threading;
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
    public class QueuedThreadPool : AbstractLifeCycle, IThreadPool
    {
        static int __id;
        
        string _name;
        internal HashSet<QueuedThreadPoolPoolThread> _threads;

        internal NJetty.Util.Util.Queue<QueuedThreadPoolPoolThread> _idleQue;

        internal NJetty.Util.Util.Queue<ThreadStart> _jobsQue;
        int _maxQueued;

        internal bool _background;
        int _id;

        internal readonly object _lock = new QueuedThreadPoolLock();
        internal readonly object _threadsLock = new QueuedThreadPoolLock();
        internal readonly object _joinLock = new QueuedThreadPoolLock();

        internal long _lastShrink;
        int _maxIdleTimeMs=60000;
        internal int _maxThreads=250;
        internal int _minThreads=2;
        internal bool _warned=false;
        int _lowThreads=0;
        internal ThreadPriority _priority= ThreadPriority.Normal;
        internal int _spawnOrShrinkAt=0;
        int _maxStopTimeMs;

        #region Constructors

        public QueuedThreadPool()
        {
            _name="qtp"+__id++;
        }
        
        public QueuedThreadPool(int maxThreads) :this()
        {
            _maxThreads = maxThreads;
        }

        #endregion

        /// <summary>
        /// Run a Job
        /// </summary>
        /// <param name="job">job to run</param>
        /// <returns>true</returns>
        public bool Dispatch(ThreadStart job) 
        {  
            if (!IsRunning || job==null)
                return false;

            QueuedThreadPoolPoolThread thread = null;
            bool spawn=false;
                
            

            // Look for an idle thread
            thread = _idleQue.Dequeue();
            
            if (thread == null)
            {
                // queue the job
                int count = _jobsQue.Enqueue(job);
                
                lock (_lock)
                {
                    //int count = _jobsQue.Count;
                    if (count > _maxQueued)
                    {
                        _maxQueued = count;
                    }
                }

                spawn = count > _spawnOrShrinkAt;
                if (spawn)
                {
                    NewThread();
                }

            }
            else
            {
                // assign the job to the selected thread
                thread.Dispatch(job);
            }
            
            
            
            return true;
        }

        /// <summary>
        /// Get the number of idle threads in the pool.
        /// </summary>
        public int IdleThreads
        {
            get { return _idleQue == null ? 0 : _idleQue.Count; }
        }
        
       
        /// <summary>
        /// Gests low resource threads threshhold
        /// </summary>
        public int LowThreads
        {
            set { _lowThreads = value; }
            get { return _lowThreads; }
        }
        
        /// <summary>
        /// Gets maximum queue size
        /// </summary>
        public int MaxQueued
        {
            get { return _maxQueued; }
        }
        
        
        
        

        

        

        /// <summary>
        /// Get the number of threads in the pool.
        /// </summary>
        public int Threads
        {
            get { return _threads.Count; }
        }

        /// <summary>
        /// Gets or Sets the priority of the pool threads.
        /// </summary>
        public ThreadPriority ThreadsPriority
        {
            set { _priority = value; }
            get { return _priority; }
        }

        public int QueueSize
        {
            //get { return _queued; }
            get { return _jobsQue.Count; }
        }
        
        /// <summary>
        /// Gets or Sets The number of queued jobs (or idle threads) needed 
        /// before the thread pool is grown (or shrunk)
        /// </summary>
        public int SpawnOrShrinkAt
        {
            get { return _spawnOrShrinkAt; }
            set { _spawnOrShrinkAt = value; }
        }

        

        /// <summary>
        /// Gets or Sets maximum total time that Stop() will wait for threads to die.
        /// </summary>
        public int MaxStopTimeMs
        {
            get { return _maxStopTimeMs; }
            set { _maxStopTimeMs = value; }
        }

        
        /// <summary>
        /// Delegated to the named or anonymous Pool.
        /// </summary>
        public bool IsBackground
        {
            set { _background = value; }
            get { return _background; }
        }

        /// <summary>
        /// lowThreads low resource threads threshhold
        /// </summary>
        public bool IsLowOnThreads
        {
            get { return _jobsQue.Count > _lowThreads; }
        }

         

        public void Join()
        {
            lock (_joinLock)
            {
                while (IsRunning)
                {
                    Monitor.Wait(_joinLock);
                }
            }
            
            // TODO remove this semi busy loop!
            while (IsStopping)
            {
                Monitor.Wait(_joinLock, 100);
                //System.Threading.Thread.Sleep(100);
            }
                
        }

        

       
        /// <summary>
        /// Gets or Sets the maximum thread idle time. 
        /// Threads that are idle for longer than this period may be stopped. 
        /// Delegated to the named or anonymous Pool.
        /// </summary>
        public int MaxIdleTimeMs
        {
            get { return _maxIdleTimeMs; }
            set { _maxIdleTimeMs = value; }
        }

        /// <summary>
        /// Gests or Set the maximum number of threads.
        /// Delegated to the named or anonymous Pool.
        /// </summary>
        public int MaxThreads
        {
            get { return _maxThreads; }
            set
            {
                if (IsStarted && value < _minThreads)
                    throw new ArgumentException("!minThreads<maxThreads");
                _maxThreads = value;
                
            }
        }

        /// <summary>
        /// Set the minimum number of threads. Delegated to the named or anonymous Pool.
        /// </summary>
        public int MinThreads
        {
            get { return _minThreads; }

            set
            {
                if (IsStarted && (value <= 0 || value > _maxThreads))
                    throw new ArgumentException("!0<=minThreads<maxThreads");
                _minThreads = value;
                lock (_threadsLock)
                {
                    while (IsStarted && _threads.Count < _minThreads)
                    {
                        NewThread();
                    }
                }
            }
        }

        /// <summary>
        /// Name of the QueuedThreadPool to use when naming Threads.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        

        /// <summary>
        /// Start the QueuedThreadPool.
        /// Construct the minimum number of threads.
        /// </summary>
        protected override void DoStart()
        {
            if (_maxThreads<_minThreads || _minThreads<=0)
                throw new ArgumentException("!0<minThreads<maxThreads");

            _threads = new HashSet<QueuedThreadPoolPoolThread>();
            _idleQue = new NJetty.Util.Util.Queue<QueuedThreadPoolPoolThread>();
            
            _jobsQue = new NJetty.Util.Util.Queue<ThreadStart>(_maxThreads);
            
            for (int i=0;i<_minThreads;i++)
            {
                NewThread();
            }   
        }

        /// <summary>
        /// Stop the QueuedThreadPool.
        /// New jobs are no longer accepted,idle threads are interrupted
        /// and stopJob is called on active threads.
        /// The method then waits 
        /// min(MaxStopTimeMs,MaxIdleTimeMs), for all jobs to
        /// stop, at which time killJob is called.
        /// </summary>
        protected override void DoStop()
        {   
            base.DoStop();

            long start = System.DateTime.Now.ToFileTime();
            for (int i=0;i<100;i++)
            {
                lock (_threadsLock)
                {
                    if (_threads != null)
                    {
                        foreach (QueuedThreadPoolPoolThread thread in _threads)
                        {
                            thread.Interrupt();
                        }
                    }

                }
                
                //Thread.yield();
                // TODO: check if below is equivalent to Thread.yield() in java
                System.Threading.Thread.Sleep(0);


                if (_threads.Count==0 || (_maxStopTimeMs>0 && _maxStopTimeMs < (DateTime.Now.ToFileTime()-start)))
                   break;
                
                try
                {
                    System.Threading.Thread.Sleep(i * 100);
                }
                catch (ThreadInterruptedException e) { }
                
                
            }

            // TODO perhaps force stops
            if (_threads.Count>0)
                Log.Warn(_threads.Count+" threads could not be stopped");
            
            lock (_joinLock)
            {
                Monitor.PulseAll(_joinLock);
                
            }
        }

        protected void NewThread()
        {
            lock (_threadsLock)
            {
                if (_threads.Count<_maxThreads)
                {
                    QueuedThreadPoolPoolThread thread = new QueuedThreadPoolPoolThread(this);
                    _threads.Add(thread);
                    thread.Name = thread.Id + "@" + _name + "-" + _id++;
                    thread.Start(); 
                }
                else if (!_warned)    
                {
                    _warned=true;
                    Log.Debug("Max threads for {0}",this);
                }
            }
        }

        /// <summary>
        /// This method is called by the Pool if a job needs to be stopped.
        /// The default implementation does nothing and should be extended by a
        /// derived thread pool class if special action is required.
        /// </summary>
        /// <param name="thread">The thread allocated to the job, or null if no thread allocated.</param>
        /// <param name="job">The job object passed to run.</param>
        protected void StopJob(IThread thread, object job)
        {
            thread.Interrupt();
            
        }
    }
}

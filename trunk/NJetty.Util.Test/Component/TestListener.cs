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


namespace NJetty.Util.Test.Component
{
    /// <summary>
    /// Mock Listener
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// November 2008
    /// </date>
    

    public class TestListener : IListener
    {
        bool failure = false;
        public bool Failure
        {
            get { return failure; }
        }

        bool started = false;
        public bool Started
        {
            get { return started; }
        }

        bool starting = false;
        public bool Starting
        {
            get { return starting; }
        }


        bool stopped = false;
        public bool Stopped
        {
            get { return stopped; }
        }


        bool stopping = false;
        public bool Stopping
        {
            get { return stopping; }
        }

        long startedTime;

        public long StartedTime
        {
            get { return startedTime; }
        }

        long startingTime;
        public long StartingTime
        {
            get { return startingTime; }
        }

        long stoppedTime;
        public long StoppedTime
        {
            get { return stoppedTime; }
        }

        long stoppingTime;
        public long StoppingTime
        {
            get { return stoppingTime; }
        }

        private Exception cause = null;

        public Exception Cause
        {
            get { return cause; }
        }


        #region IListener Members

        public void LifeCycleStarting(ILifeCycle e)
        {
            starting = true;
            startingTime = DateTime.Now.TimeOfDay.Milliseconds;

            // need to sleep to make sure the starting and started times are not
            // the same
            try
            {
                System.Threading.Thread.Sleep(1);
            }
            catch (Exception err)
            {
                Console.WriteLine(err.StackTrace);
            }
        }

        public void LifeCycleStarted(ILifeCycle e)
        {
            started = true;
            startedTime = DateTime.Now.TimeOfDay.Milliseconds;
        }

        public void LifeCycleFailure(ILifeCycle e, Exception cause)
        {
            failure = true;
            this.cause = cause;
        }

        public void LifeCycleStopping(ILifeCycle e)
        {
            stopping = true;
            stoppingTime = DateTime.Now.TimeOfDay.Milliseconds;

            // need to sleep to make sure the stopping and stopped times are not
            // the same
            try
            {
                System.Threading.Thread.Sleep(1);
            }
            catch (Exception err)
            {
                Console.WriteLine(err.StackTrace);
            }
        }

        public void LifeCycleStopped(ILifeCycle e)
        {
            stopped = true;
            stoppedTime = DateTime.Now.TimeOfDay.Milliseconds;
        }

        #endregion
    }
}

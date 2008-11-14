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
using NUnit.Framework;
using NJetty.Util.Logger;

namespace NJetty.Util.Test.Component
{


    /// <summary>
    /// Test For Component
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// November 2008
    /// </date>



    [TestFixture]
    public class LifeCycleListenerTest
    {

        static Exception cause = new Exception("expected test exception");

        

        [Test]
        public void TestStart()
        {
            TestLifeCycle lifecycle = new TestLifeCycle();
            TestListener listener = new TestListener();
            lifecycle.AddLifeCycleListener(listener);
            ((StdErrLog)Log.Logger).HideStacks = true;
            lifecycle.Cause = cause;
            
            try
            {
                lifecycle.Start();
                Assert.IsTrue(false);
            }
            catch(Exception e)
            {
                Assert.AreEqual(cause, e);
                Assert.AreEqual(cause, listener.Cause);
            }
            lifecycle.Cause = null;
            ((StdErrLog)Log.Logger).HideStacks = false;
            
            
            lifecycle.Start();

            // check that the starting event has been thrown
            Assert.IsTrue(listener.Starting, "The staring event didn't occur");


            // check that the started event has been thrown
            Assert.IsTrue(listener.Started, "The started event didn't occur");

            // check that the starting event occurs before the started event
            Assert.IsTrue(listener.StartingTime <= listener.StartedTime, "The starting event must occur before the started event");

            // check that the lifecycle's state is started
            Assert.IsTrue(lifecycle.IsStarted, "The lifecycle state is not started");
        }

        [Test]
        public void TestStop()
        {
            TestLifeCycle lifecycle = new TestLifeCycle();
            TestListener listener = new TestListener();
            lifecycle.AddLifeCycleListener(listener);

            
            // need to set the state to something other than stopped or stopping or
            // else
            // Stop() will return without doing anything

            lifecycle.Start();

            ((StdErrLog)Log.Logger).HideStacks = true;
            lifecycle.Cause = cause;
            
            try
            {
                lifecycle.Stop();
                Assert.IsTrue(false);
            }
            catch(Exception e)
            {
                Assert.AreEqual(cause, e);
                Assert.AreEqual(cause, listener.Cause);
            }

            
            lifecycle.Cause = null;
            ((StdErrLog)Log.Logger).HideStacks = false;
            
            lifecycle.Stop();

            // check that the stopping event has been thrown
            Assert.IsTrue(listener.Stopping, "The stopping event didn't occur");

            // check that the stopped event has been thrown
            Assert.IsTrue(listener.Stopped, "The stopped event didn't occur");

            // check that the stopping event occurs before the stopped event
            Assert.IsTrue(listener.StoppingTime <= listener.StoppedTime, "The stopping event must occur before the stopped event");
            
            // check that the lifecycle's state is stopped
            Assert.IsTrue(lifecycle.IsStopped, "The lifecycle state is not stooped");
        }



    }
}

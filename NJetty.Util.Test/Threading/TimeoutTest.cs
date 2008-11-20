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
using NJetty.Util.Threading;

namespace NJetty.Util.Test.Threading
{
    /// <summary>
    /// Test Timeout class
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// November 2008
    /// </date>
    [TestFixture]
    public class TimeoutTest
    {
        object lockObject = new Object();
        Timeout timeout = new Timeout(null);
        TimeoutTask[] tasks;

        [TestFixtureSetUp]
        public void SetUp()
        {
            timeout = new Timeout(lockObject);
            tasks= new TimeoutTask[10]; 
            
            for (int i=0;i<tasks.Length;i++)
            {
                tasks[i]=new TimeoutTask();
                timeout.Now=1000+i*100;
                timeout.Schedule(tasks[i]);
            }
            timeout.Now = 100;
        }

        
        
        [Test]
        public void TestExpiry()
        {
            timeout.Duration = 200;
            timeout.Now = 1500;
            timeout.Tick();
            
            for (int i=0;i<tasks.Length;i++)
            {
                Assert.AreEqual(i<4, tasks[i].IsExpired, "IsExpired "+i);
            }
        //}

        //[Test]
        //public void TestCancel()
        //{
            timeout.Duration = 200;
            timeout.Now = 1700;

            for (int i = 0; i < tasks.Length; i++)
                if (i % 2 == 1)
                    tasks[i].Cancel();

            timeout.Tick();

            for (int i = 0; i < tasks.Length; i++)
            {
                Assert.AreEqual(i % 2 == 0 && i < 6, tasks[i].IsExpired, "isExpired " + i);
            }
        //}

        //[Test]
        //public void TestTouch()
        //{
            timeout.Duration=200;
            timeout.Now=1350;
            timeout.Schedule(tasks[2]);

            timeout.Now=1500;
            timeout.Tick();
            for (int i = 0; i < tasks.Length; i++)
            {
                Assert.AreEqual(i != 2 && i < 4, tasks[i].IsExpired, "isExpired " + i);
            }

            timeout.Now=1550;
            timeout.Tick();
            for (int i = 0; i < tasks.Length; i++)
            {
                Assert.AreEqual(i < 4, tasks[i].IsExpired, "isExpired " + i);
            }
        }


        //public void TestDelay()
        //{
        //    Timeout.Task task = new Timeout.Task();

        //    timeout.setNow(1100);
        //    timeout.schedule(task, 300);
        //    timeout.setDuration(200);
            
        //    timeout.setNow(1300);
        //    timeout.tick();
        //    assertEquals("delay", false, task.isExpired());
            
        //    timeout.setNow(1500);
        //    timeout.tick();
        //    assertEquals("delay", false, task.isExpired());
            
        //    timeout.setNow(1700);
        //    timeout.tick();
        //    assertEquals("delay", true, task.isExpired());
        //}

        //public void TestStress() throws Exception
        //{
        //    final int LOOP=500;
        //    final boolean[] running = {true};
        //    final int[] count = {0,0,0};

        //    timeout.setNow(System.currentTimeMillis());
        //    timeout.setDuration(500);
            
        //    // Start a ticker thread that will tick over the timer frequently.
        //    Thread ticker = new Thread()
        //    {
        //        public void run()
        //        {
        //            while (running[0])
        //            {
        //                try
        //                {
        //                    // use lock.wait so we have a memory barrier and
        //                    // have no funny optimisation issues.
        //                    synchronized (lock)
        //                    {
        //                        lock.wait(30);
        //                    }
        //                    Thread.sleep(30);
        //                    timeout.tick(System.currentTimeMillis());
        //                }
        //                catch(Exception e)
        //                {
        //                    e.printStackTrace();
        //                }
        //            }
        //        }
        //    };
        //    ticker.start();

        //    // start lots of test threads
        //    for (int i=0;i<LOOP;i++)
        //    {
        //        // 
        //        Thread th = new Thread()
        //        { 
        //            public void run()
        //            {
        //                // count how many threads were started (should == LOOP)
        //                synchronized(count)
        //                {
        //                    count[0]++;
        //                }
                        
        //                // create a task for this thread
        //                Timeout.Task task = new Timeout.Task()
        //                {
        //                    public void expired()
        //                    {       
        //                        // count the number of expires
        //                        synchronized(count)
        //                        {
        //                            count[2]++;
        //                        }
        //                    }
        //                };
                        
        //                // this thread will loop and each loop with schedule a 
        //                // task with a delay  on top of the timeouts duration
        //                // mostly this thread will then cancel the task
        //                // But once it will wait and the task will expire
                        
                        
        //                int once = (int)( 10+(System.currentTimeMillis() % 50));
                        
        //                // do the looping until we are stopped
        //                int loop=0;
        //                while (running[0])
        //                {
        //                    try
        //                    {
        //                        long delay=1000;
        //                        long wait=100-once;
        //                        if (loop++==once)
        //                        { 
        //                            // THIS loop is the one time we wait 1000ms
        //                            synchronized(count)
        //                            {
        //                                count[1]++;
        //                            }
        //                            delay=200;
        //                            wait=1000;
        //                        }
                                
        //                        timeout.schedule(task,delay);
                                
        //                        // do the wait
        //                        Thread.sleep(wait);
                                
        //                        // cancel task (which may have expired)
        //                        task.cancel();
        //                    }
        //                    catch(Exception e)
        //                    {
        //                        e.printStackTrace();
        //                    }
        //                }
        //            }
        //        };
        //        th.start();
        //    }
            
        //    // run test for 5s
        //    Thread.sleep(8000);
        //    synchronized (lock)
        //    {
        //        running[0]=false;
        //    }
        //    // give some time for test to stop
        //    Thread.sleep(2000);
        //    timeout.tick(System.currentTimeMillis());
        //    Thread.sleep(1000);
            
        //    // check the counts
        //    assertEquals("count threads", LOOP,count[0]);
        //    assertEquals("count once waits",LOOP,count[1]);
        //    assertEquals("count expires",LOOP,count[2]);
        //}

    }
}

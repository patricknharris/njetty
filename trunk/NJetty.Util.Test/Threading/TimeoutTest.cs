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



        [Test]
        public void TestExpiry()
        {
            #region Initialization
            object lockObject = new Object();
            Timeout timeout = new Timeout(null);
            TimeoutTask[] tasks;


            timeout = new Timeout(lockObject);
            tasks = new TimeoutTask[10];

            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i] = new TimeoutTask();
                timeout.Now = 1000 + i * 100;
                timeout.Schedule(tasks[i]);
            }
            timeout.Now = 100;
            #endregion



            timeout.Duration = 200;
            timeout.Now = 1500;
            timeout.Tick();

            for (int i = 0; i < tasks.Length; i++)
            {
                Assert.AreEqual(i < 4, tasks[i].IsExpired, "IsExpired " + i);
            }
        }

        [Test]
        public void TestCancel()
        {
            #region Initialization
            object lockObject = new Object();
            Timeout timeout = new Timeout(null);
            TimeoutTask[] tasks;


            timeout = new Timeout(lockObject);
            tasks = new TimeoutTask[10];

            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i] = new TimeoutTask();
                timeout.Now = 1000 + i * 100;
                timeout.Schedule(tasks[i]);
            }
            timeout.Now = 100;
            #endregion

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
        }

        [Test]
        public void TestTouch()
        {
            #region Initialization
            object lockObject = new Object();
            Timeout timeout = new Timeout(null);
            TimeoutTask[] tasks;


            timeout = new Timeout(lockObject);
            tasks = new TimeoutTask[10];

            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i] = new TimeoutTask();
                timeout.Now = 1000 + i * 100;
                timeout.Schedule(tasks[i]);
            }
            timeout.Now = 100;
            #endregion

            timeout.Duration = 200;
            timeout.Now = 1350;
            timeout.Schedule(tasks[2]);

            timeout.Now = 1500;
            timeout.Tick();
            for (int i = 0; i < tasks.Length; i++)
            {
                Assert.AreEqual(i != 2 && i < 4, tasks[i].IsExpired, "isExpired " + i);
            }

            timeout.Now = 1550;
            timeout.Tick();
            for (int i = 0; i < tasks.Length; i++)
            {
                Assert.AreEqual(i < 4, tasks[i].IsExpired, "isExpired " + i);
            }
        }


        public void TestDelay()
        {
            #region Initialization
            object lockObject = new Object();
            Timeout timeout = new Timeout(null);
            TimeoutTask[] tasks;


            timeout = new Timeout(lockObject);
            tasks = new TimeoutTask[10];

            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i] = new TimeoutTask();
                timeout.Now = 1000 + i * 100;
                timeout.Schedule(tasks[i]);
            }
            timeout.Now = 100;
            #endregion


            TimeoutTask task = new TimeoutTask();

            timeout.Now = 1100;
            timeout.Schedule(task, 300);
            timeout.Duration = 200;

            timeout.Now = 1300;
            timeout.Tick();
            Assert.IsFalse(task.IsExpired, "delay");

            timeout.Now = 1500;
            timeout.Tick();
            Assert.IsFalse(task.IsExpired, "delay");

            timeout.Now = 1700;
            timeout.Tick();
            Assert.IsTrue(task.IsExpired, "delay");
        }

        public void TestStress()
        {
            #region Initialization
            object lockObject = new Object();
            Timeout timeout = new Timeout(null);
            TimeoutTask[] tasks;


            timeout = new Timeout(lockObject);
            tasks = new TimeoutTask[10];

            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i] = new TimeoutTask();
                timeout.Now = 1000 + i * 100;
                timeout.Schedule(tasks[i]);
            }
            timeout.Now = 100;
            #endregion


            int LOOP = 500;
            bool[] running = new bool[] { true };
            int[] count = new int[] { 0, 0, 0 };

            timeout.Now = (DateTime.UtcNow.Ticks / 1000);
            timeout.Duration = 500;

            // Start a ticker thread that will tick over the timer frequently.
            System.Threading.Thread ticker = new System.Threading.Thread(

                new System.Threading.ThreadStart(
                  () =>
                  {
                      while (running[0])
                      {
                          try
                          {
                              // use lock.wait so we have a memory barrier and
                              // have no funny optimisation issues.
                              lock (lockObject)
                              {
                                  System.Threading.Monitor.Wait(lockObject, 30);
                              }
                              System.Threading.Thread.Sleep(30);
                              timeout.Tick((DateTime.UtcNow.Ticks / 1000));
                          }
                          catch (Exception e)
                          {
                              Console.Error.WriteLine(e.StackTrace);
                          }
                      }
                  }
                    )
                );
            ticker.Start();

            // start lots of test threads
            for (int i = 0; i < LOOP; i++)
            {
                // 
                System.Threading.Thread th = new System.Threading.Thread(

                    new System.Threading.ThreadStart(
                  () =>
                  {
                      // count how many threads were started (should == LOOP)
                      lock (count)
                      {
                          count[0]++;
                      }

                      // create a task for this thread
                      TimeoutTask task = new TimeoutTask1(count);

                      // this thread will loop and each loop with schedule a 
                      // task with a delay  on top of the timeouts duration
                      // mostly this thread will then cancel the task
                      // But once it will wait and the task will expire


                      int once = (int)(10 + ((DateTime.UtcNow.Ticks / 1000) % 50));

                      // do the looping until we are stopped
                      int loop = 0;
                      while (running[0])
                      {
                          try
                          {
                              long delay = 1000;
                              long wait = 100 - once;
                              if (loop++ == once)
                              {
                                  // THIS loop is the one time we wait 1000ms
                                  lock (count)
                                  {
                                      count[1]++;
                                  }
                                  delay = 200;
                                  wait = 1000;
                              }

                              timeout.Schedule(task, delay);

                              // do the wait
                              System.Threading.Thread.Sleep((int)wait);

                              // cancel task (which may have expired)
                              task.Cancel();
                          }
                          catch (Exception e)
                          {
                              Console.Error.WriteLine(e.StackTrace);
                          }
                      }
                  }
                ));
                th.Start();
            }

            // run test for 5s
            System.Threading.Thread.Sleep(8000);
            lock (lockObject)
            {
                running[0] = false;
            }
            // give some time for test to stop
            System.Threading.Thread.Sleep(2000);
            timeout.Tick((DateTime.UtcNow.Ticks / 1000));
            System.Threading.Thread.Sleep(1000);

            // check the counts
            Assert.AreEqual(LOOP, count[0], "count threads");
            Assert.AreEqual(LOOP, count[1], "count once waits");
            Assert.AreEqual(LOOP, count[2], "count expires");


        }

    }

    class TimeoutTask1 : TimeoutTask
    {

        int[] count;

        internal TimeoutTask1(int[] count)
        {
            this.count = count;
        }

        public override void Expired()
        {
            lock (count)
            {
                count[2]++;
            }
        }

    }

}

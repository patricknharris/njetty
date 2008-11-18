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
using NUnit.Framework;
using NJetty.Util.Thread;
using NJetty.Util.Logger;

namespace NJetty.Util.Test.Thread
{
    /// <summary>
    /// Test For QueuedThreadPool
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// November 2008
    /// </date>
    
    [TestFixture]
    public class ThreadPoolTest
    {

        
        int _jobs;
        long _result;
        static object _threadPoolTestLock = new object();
        ThreadStart _job;

        [TestFixtureSetUp]
        public void InitializeThreadPoolTest()
        {
            _job = new ThreadStart(run);
        }

        void run()
        {
            try 
            {
                Log.Warn("111 - Job>>>>" + _job);
                System.Threading.Thread.Sleep(100);
                Log.Warn("222 - Job>>>>" + _job);

            }
            catch(Exception e)
            {
                Log.Warn(e);
            }
            
            long t = DateTime.Now.ToFileTime()%10000;
            long r=t;
            for (int i=0;i<t;i++)
                r+=i;
                
            lock(_threadPoolTestLock)
            {
                _jobs++;
                _result+=r;
            }
        }
    
    
    
        [Test]
        public void TestQueuedThreadPool()
        {        
            QueuedThreadPool tp= new QueuedThreadPool();
            tp.MinThreads = 5;
            tp.MaxThreads = 10;
            tp.MaxIdleTimeMs = 1000;
            tp.SpawnOrShrinkAt = 2;
            tp.ThreadsPriority = ThreadPriority.Normal;

            tp.Start();
            System.Threading.Thread.Sleep(1000);

            
            Assert.AreEqual(5,tp.Threads);
            Assert.AreEqual(5,tp.IdleThreads);
            
            tp.Dispatch(_job);
            Log.Warn(">>>>>>1");
            tp.Dispatch(_job);
            Log.Warn(">>>>>>2");

            Assert.AreEqual(5,tp.Threads);
            //Assert.AreEqual(3,tp.IdleThreads);
            System.Threading.Thread.Sleep(1000);
            Assert.AreEqual(5,tp.Threads);
            Assert.AreEqual(5,tp.IdleThreads);

            for (int i = 0; i < 100; i++)
            {
                tp.Dispatch(_job);
            }
            
            Assert.IsTrue(tp.QueueSize>10);
            Assert.IsTrue(tp.IdleThreads<=1);

            System.Threading.Thread.Sleep(2000);

            Assert.AreEqual(0,tp.QueueSize);
            Assert.IsTrue(tp.IdleThreads>5);

            int threads=tp.Threads;
            Assert.IsTrue(threads>5);
            System.Threading.Thread.Sleep(1500);
            Assert.IsTrue(tp.Threads<threads);
        }
        
        [Test]
        public void TestStress()
        {
            QueuedThreadPool tp= new QueuedThreadPool();
            tp.MinThreads = 240;
            tp.MaxThreads = 250;
            tp.MaxIdleTimeMs = 100;
            tp.Start();

            tp.MinThreads = 90;

            
            int count = 0;
            
            Random random = new Random((int)DateTime.Now.ToFileTime());
            int loops = 1600;//16000;

            try
            {
                for (int i=0;i<loops;)
                {
                    int burst= random.Next(100);
                    for (int b=0;b<burst && i<loops; b++)
                    {
                        if (i%20==0)
                            Console.Error.Write('.');
                        if (i%1600==1599)
                            Console.Error.WriteLine();
                        if (i==1000)
                            tp.MinThreads = 10;
                        
                        if (i==10000)
                            tp. MaxThreads= 20;
                        
                        i++;
                        tp.Dispatch(
                            new ThreadStart(
                                () =>
                                {

                                    int s=random.Next(50);
                                    try
                                    {
                                        System.Threading.Thread.Sleep(s);
                                    }
                                    catch (ThreadInterruptedException e)
                                    {
                                        Console.WriteLine(e.StackTrace);
                                    }
                                    finally
                                    {
                                        Interlocked.Increment(ref count);
                                    }

                                }


                                )
                            
                            );
                    }

                    System.Threading.Thread.Sleep(random.Next(100));
                }

                System.Threading.Thread.Sleep(1000);
                    
                tp.Stop();
                
                Assert.AreEqual(loops,count);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.StackTrace);
                Assert.IsTrue(false, e.StackTrace);
            }
        }

        //public void testMaxStopTime() throws Exception
        //{
        //    QueuedThreadPool tp= new QueuedThreadPool();
        //    tp.setMaxStopTimeMs(500);
        //    tp.Start();
        //    tp.dispatch(new Runnable(){
        //        public void run () {
        //            while (true) {
        //                try {
        //                    Thread.sleep(10000);
        //                } catch (InterruptedException ie) {}
        //            }
        //        }
        //    });

        //    long beforeStop = System.currentTimeMillis();
        //    tp.stop();
        //    long afterStop = System.currentTimeMillis();
        //    Assert.IsTrue(tp.isStopped());
        //    Assert.IsTrue(afterStop - beforeStop < 1000);
        //}


    }
}

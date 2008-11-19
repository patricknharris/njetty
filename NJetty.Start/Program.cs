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
using NJetty.Util.Threading;
using System.Threading;
using NJetty.Util.Logging;



namespace NJetty.Start
{

    /// <summary>
    /// TODO: Class/Interface Information here
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// November 2008
    /// </date>
    public class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Thread t = new Thread(new ThreadStart(RunThreadPool));
            t.Start();
            t.Join();

            Log.Info("Done Executing Jobs!!!");
            System.Console.ReadLine();

        }


        static void RunThreadPool()
        {
            QueuedThreadPool tp = new QueuedThreadPool();
            tp.MaxStopTimeMs = 500;
            tp.Start();

            // dispatch jobs
            for (int i = 0; i < 10; i++)
            {

                tp.Dispatch(new ThreadStart(
                  () =>
                  {
                      Log.Info("Running job with thread id " + System.Threading.Thread.CurrentThread.Name);
                      //while (true)
                      //{
                          try
                          {
                              System.Threading.Thread.Sleep(5000);
                          }
                          catch (ThreadInterruptedException) { }
                      //}
                  }
                ));
            }



            System.Threading.Thread.Sleep(1000);
            long beforeStop = DateTime.Now.TimeOfDay.Milliseconds;
            tp.Stop();
            long afterStop = DateTime.Now.TimeOfDay.Milliseconds;

            Log.Info("Time to Stop {0}", afterStop - beforeStop);



            
        }

    }

}

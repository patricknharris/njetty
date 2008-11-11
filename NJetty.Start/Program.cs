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
using NJetty.Util.Log;

using System.Reflection;



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
        static NLogLog log = new NLogLog(); 

        static void Main(string[] args)
        {

            PrintAllAssembliesInAppDomain(AppDomain.CurrentDomain);



            // Make a new AppDomain in the current process.
            AppDomain anotherAD = AppDomain.CreateDomain("SecondAppDomain");
            anotherAD.Load("NLog");
            anotherAD.Load("NJetty.Start");
            PrintAllAssembliesInAppDomain(anotherAD);

            //anotherAD.DomainUnload += new EventHandler(anotherAD_DomainUnload);
            //anotherAD.ProcessExit += new EventHandler(defaultAD_ProcessExit);

            AppDomain.Unload(anotherAD);

            
           


            Console.ReadLine();

        }


        static void PrintAllAssembliesInAppDomain(AppDomain ad)
        {
            Assembly[] loadedAssemblies = ad.GetAssemblies();
            log.Warn("***** Here are the assemblies loaded in {0} *****\n",
            ad.FriendlyName);
            foreach (Assembly a in loadedAssemblies)
            {
                log.Warn("-> Name: {0}", a.GetName().Name);
                log.Warn("-> Version: {0}\n", a.GetName().Version);
            }
        }


        static void anotherAD_DomainUnload(object sender, EventArgs e)
        {
            log.Warn("***** Unloaded anotherAD! *****\n");
            log.Warn(string.Format("Sender: {0}, EventArgs: {1}", 
                sender, 
                e));
            log.Warn("*******************************\n");
        }


        static void defaultAD_ProcessExit(object sender, EventArgs e)
        {
            log.Warn("***** Unloaded Process Exit! *****\n");
            log.Warn(string.Format("Sender: {0}, EventArgs: {1}",
                sender,
                e));
            log.Warn("*******************************\n");
        }

    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NJetty.Util.Log;

using System.Reflection;

namespace NJetty.Start
{
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

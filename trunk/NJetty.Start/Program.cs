using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NJetty.Util.Log;

using NLog;

namespace NJetty.Start
{
    public class Program
    {
        static void Main(string[] args)
        {

            object[] currentVideoGames = {"Morrowind", "BioShock",
            "Half Life 2: Episode 1", "The Darkness",
            "Daxter", "System Shock 2", 1,2,3,4,5, 1.1};
            
            var result = from g in currentVideoGames  select g;

            //Logger l = LogManager.GetLogger("heheheh");
            

            try
            {
                NLogLog log = new NLogLog();



                foreach (var item in result)
                {

                    log.Info("{0} {1}", "[[[[[Info]]]]", item);
                    log.Debug("{0} {1}", "[[[[[Debug]]]]", item);
                    log.Warn("{0} {1}", "[[[[[Warn]]]]", item);
                    log.Info(" is debug enabled " + log.IsDebugEnabled);


                }


                try
                {
                    string s = null;
                    s.ToLower();
                }
                catch (Exception e)
                {

                    log.Info("<<<Info>>>>", e);
                    log.Debug("<<<Debug>>>>", e);
                    log.Warn("<<<Warn>>>>", e);
                }

                Console.WriteLine("\n\n\n\n" + log.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message + "\n " + e.StackTrace);
            }


            

            Console.ReadLine();

        }



    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace dhcp_simulacio
{
    class Program
    {
        static List<string> excluded = new List<string>();
        static void BeolvasExcluded()
        {
            try
            {
                StreamReader file = new StreamReader("excluded.csv");

                try
                {
                    while (!file.EndOfStream)
                    {
                        excluded.Add(file.ReadLine());
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }
                finally
                {
                    file.Close();
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
        }
        static string CimEggyelNo(string cim)
        {
            /* cím=192.168.10.100
             * return 192.168.10.101
             */
            string[] adatok = cim.Split('.');
            int okt4 = int.Parse(adatok[3]);
            if (okt4<255)
            {
                okt4++;
               
            }
            return adatok[0] + "." + adatok[1] + "." + adatok[2] + "." + okt4.ToString();                    
        }
        static void Main(string[] args)
        {
            BeolvasExcluded();
            //foreach (var i in excluded) 
            //{
            //    Console.WriteLine(i);
            //}

            Console.WriteLine("\nVége..."); 
            Console.ReadLine();
        }
    }
}

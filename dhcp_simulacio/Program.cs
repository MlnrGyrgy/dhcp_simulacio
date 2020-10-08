﻿using System;
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
        static Dictionary<string, string> dhcp = new Dictionary<string, string>();
        static Dictionary<string, string> reserved = new Dictionary<string, string>();
        static List<string> commands = new List<string>();
        static void BeolvasList(List<string> l, string filename)
        {
            try
            {
                StreamReader file = new StreamReader(filename);

                try
                {
                    while (!file.EndOfStream)
                    {
                        l.Add(file.ReadLine());
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
        static void Feladat(string parancs)
        {
            /*parancs="request;D192313670A82
             * először csak a request parancsokkal foglalkozunk
             *
             * Megnézzük, hogy request-e.
             * ki kell szedni a MAC címet a parancsból
             */
            if (parancs.Contains("request"))
            {
                string[] a = parancs.Split(';');
                string mac = a[1];
                if (dhcp.ContainsKey(mac))
                {
                    Console.WriteLine($" {mac} --> {dhcp[mac]}");
                }
                else
                {
                    if (reserved.ContainsKey(mac))
                    {
                        Console.WriteLine($"Res. {mac} --> {reserved[mac]}");
                        dhcp.Add(mac, reserved[mac]);
                    }
                    else
                    {
                        string indulo ="192.168.10.100";
                        int okt4 = 100;

                        while (okt4<200 && (dhcp.ContainsValue(indulo) || reserved.ContainsValue(indulo) || excluded.Contains(indulo)))
                        {
                            okt4++;
                            indulo = CimEggyelNo(indulo);
                        }
                        if (okt4 < 200)
                        {
                            Console.WriteLine($"Kiosztott {mac} --> {indulo}");
                            dhcp.Add(mac,indulo);
                        }
                        else
                        {
                            Console.WriteLine($"{mac} nincs IP!");
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Nem oké");
            }
        }
        static void Feladatok()
        {
            foreach (var command in commands)
            {
                Feladat(command);
            }
        }
        static void BeolvasDictionary(Dictionary<string,string>d,string filenev)
        {
            try
            {
                StreamReader file = new StreamReader(filenev);
                while (!file.EndOfStream)
                {
                    string[] adatok = file.ReadLine().Split(';');   
                    d.Add(adatok[0], adatok[1]);
                }
                file.Close();
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
        }
        static void Main(string[] args)
        {
            BeolvasList(excluded,"excluded.csv");
            BeolvasDictionary(dhcp, "dhcp.csv");
            BeolvasDictionary(reserved, "reserved.csv");
            BeolvasList(commands,"test.csv");
            //foreach (var i in commands)
            //{
            //    Console.WriteLine(i);
            //}
            Feladatok();

            Console.WriteLine("\nVége..."); 
            Console.ReadLine();
        }
    }
}

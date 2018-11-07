using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Lextm.SharpSnmpLib;
using Mono.Options;
using Lextm.SharpSnmpLib.Security;
using Lextm.SharpSnmpLib.Messaging;
using System.Reflection;
using SnmpConnect.Extensions;

namespace SnmpConnect
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Console.WriteLine("--------------------------Get İşlemleri Başladı--------------------------");

            //var GetResponse = SnmpRequests.Get("192.168.1.147", "smartpackread", ".1.3.6.1.2.1.1.9.1.2.3", 161);
            //foreach (var item in GetResponse)
            //{
            //    Console.WriteLine(item);
            //}

            //Console.WriteLine("--------------------------GetNext İşlemleri Başladı--------------------------");

            //var GetNextResponse = SnmpRequests.GetNext("192.168.1.147", "smartpackread", ".1.3.6.1.2.1.1.9.1.2.3",161);
            //foreach (var item in GetNextResponse)
            //{
            //    Console.WriteLine(item);
            //}

            //Console.WriteLine("--------------------------GetBulk İşlemleri Başladı--------------------------");

            //var GetBulkResponse = SnmpRequests.GetBulk("192.168.1.147", "smartpackread", ".1.3.6.1.2.1.1.9.1.2.3", 161);
            //foreach (var item in GetBulkResponse)
            //{
            //    Console.WriteLine(item);
            //}

            //Console.WriteLine("--------------------------Set İşlemleri Başladı--------------------------");

            //var SetResponse = SnmpRequests.Set("192.168.1.147", "smartpackwrite", ".1.3.6.1.4.1.35483.1.1.2.2.2.0", 161, "15");
            //foreach (var item in SetResponse)
            //{
            //    Console.WriteLine(item);
            //}


            Console.WriteLine("--------------------------Trap İşlemleri Başladı--------------------------");
            SnmpTraps.SenTrap(args);

            //You can watch traps on MIB Browser



            Console.Read();

        }
    }
}

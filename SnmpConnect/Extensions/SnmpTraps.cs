﻿using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using Lextm.SharpSnmpLib.Security;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace SnmpConnect.Extensions
{
    public static class SnmpTraps
    {
        //192.168.1.71
        public static void Trap(string deviceIpAddress, string masterIpAddress,int postNumber, string identifier,string readPass)
        {
          Messenger.SendTrapV1(new IPEndPoint(IPAddress.Parse(deviceIpAddress), postNumber),
                    IPAddress.Parse(masterIpAddress),
                    new OctetString(readPass),
                    new ObjectIdentifier(identifier),
                    GenericCode.ColdStart,
                    0,0,
                    new List<Variable>());
  
        }

        public static void SenTrap(string[] args)
        {
            IPAddress address = args.Length == 1 ? IPAddress.Parse(args[0]) : IPAddress.Loopback;

            Messenger.SendTrapV1(
                new IPEndPoint(address, 162),
                IPAddress.Loopback,
                new OctetString("public"),
                new ObjectIdentifier(new uint[] { 1, 3, 6 }),
                GenericCode.ColdStart,
                0,
                0,
                new List<Variable>());

            //Thread.Sleep(50);


            Messenger.SendTrapV2(
                0,
                VersionCode.V2,
                new IPEndPoint(address, 162),
                new OctetString("public"),
                new ObjectIdentifier(new uint[] { 1, 3, 6 }),
                0,
                new List<Variable>());
            //Thread.Sleep(50);

            try
            {
                Messenger.SendInform(
                    0,
                    VersionCode.V2,
                    new IPEndPoint(address, 162),
                    new OctetString("public"),
                    OctetString.Empty,
                    new ObjectIdentifier(new uint[] { 1, 3, 6 }),
                    0,
                    new List<Variable>(),
                    2000,
                    null,
                    null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            try
            {
                Discovery discovery = Messenger.GetNextDiscovery(SnmpType.InformRequestPdu);
                ReportMessage report = discovery.GetResponse(2000, new IPEndPoint(address, 162));

                Messenger.SendInform(
                    0,
                    VersionCode.V3,
                    new IPEndPoint(address, 162),
                    new OctetString("neither"),
                    OctetString.Empty,
                    new ObjectIdentifier(new uint[] { 1, 3, 6 }),
                    0,
                    new List<Variable>(),
                    2000,
                    DefaultPrivacyProvider.DefaultPair,
                    report);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            {
                var trap = new TrapV2Message(
                    VersionCode.V3,
                    528732060,
                    1905687779,
                    new OctetString("neither"),
                    new ObjectIdentifier("1.3.6"),
                    0,
                    new List<Variable>(),
                    DefaultPrivacyProvider.DefaultPair,
                    0x10000,
                    new OctetString(ByteTool.Convert("80001F8880E9630000D61FF449")),
                    0,
                    0);
                trap.Send(new IPEndPoint(address, 162));
            }

            {
                if (DESPrivacyProvider.IsSupported)
                {
                    var trap = new TrapV2Message(
                        VersionCode.V3,
                        528732060,
                        1905687779,
                        new OctetString("privacy"),
                        new ObjectIdentifier("1.3.6"),
                        0,
                        new List<Variable>(),
                        new DESPrivacyProvider(
                            new OctetString("privacyphrase"),
                            new MD5AuthenticationProvider(new OctetString("authentication"))),
                        0x10000,
                        new OctetString(ByteTool.Convert("80001F8880E9630000D61FF449")),
                        0,
                        0);
                    trap.Send(new IPEndPoint(address, 162));
                }
            }
        }
    }
}

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
using Newtonsoft.Json;

namespace SnmpConnect.Extensions
{
    public static class SnmpRequests
    {


        public static IList<Variable> Get(string IpAddress, string readPermissionString, string Identifier, int readPort)
        {
            var result = Messenger.Get(VersionCode.V1,
                         new IPEndPoint(IPAddress.Parse(IpAddress), readPort),
                         new OctetString(readPermissionString),
                         new List<Variable> { new Variable(new ObjectIdentifier(Identifier)) },
                         60000);
            return result;
        }

        public static IList<Variable> GetNext(string IpAddress, string readPermissionString, string Identifier, int readPort)
        {
            GetNextRequestMessage message = new GetNextRequestMessage(0,
                                                          VersionCode.V1,
                                                          new OctetString(readPermissionString),
                                                          new List<Variable> { new Variable(new ObjectIdentifier(Identifier)) });
            ISnmpMessage response = message.GetResponse(60000, new IPEndPoint(IPAddress.Parse(IpAddress), readPort));
            if (response.Pdu().ErrorStatus.ToInt32() != 0)
            {
                throw ErrorException.Create(
                    "error in response",
                    IPAddress.Parse("192.168.1.2"),
                    response);
            }

            var result = response.Pdu().Variables;
            return result;
            
        }

        public static IList<Variable> GetBulk(string IpAddress, string readPermissionString, string Identifier, int readPort)
        {
            GetBulkRequestMessage message = new GetBulkRequestMessage(0,
                                                           VersionCode.V2,
                                                           new OctetString(readPermissionString),
                                                           0,
                                                           10,
                                                           new List<Variable> { new Variable(new ObjectIdentifier(Identifier)) });
            ISnmpMessage response = message.GetResponse(60000, new IPEndPoint(IPAddress.Parse(IpAddress), readPort));
            if (response.Pdu().ErrorStatus.ToInt32() != 0)
            {
                throw ErrorException.Create(
                    "error in response",
                    IPAddress.Parse(IpAddress),
                    response);
            }

            var result = response.Pdu().Variables;
            return result;
        }

        public static List<string> GetSubTree()
        {
            return null;
        }

        public static List<string> Walk(string IpAddress, string readPermissionString, string Identifier, int readPort)
        {
            var result = new List<Variable>();
            Messenger.Walk(VersionCode.V1,
                           new IPEndPoint(IPAddress.Parse("192.168.1.2"), 161),
                           new OctetString("public"),
                           new ObjectIdentifier("1.3.6.1.2.1.1"),
                           result,
                           60000,
                           WalkMode.WithinSubtree);
            return null;
        }

        public static IList<Variable> Set(string IpAddress, string writePermissionString, string Identifier, int readPort,string InsertedValue)
        {
            ISnmpData insertedValue =  new Integer32(int.Parse(InsertedValue));
            var result = Messenger.Set(VersionCode.V1,
                              new IPEndPoint(IPAddress.Parse(IpAddress), readPort),
                              new OctetString(writePermissionString),
                              new List<Variable> { new Variable(new ObjectIdentifier(Identifier), insertedValue) },
                              60000);

            
            return result;
        }

    }
}

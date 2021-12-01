using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
namespace Networking
{
    public class NetworkInfo
    {
        public string GameName { get; set; }
        public List<ClientInfo> Clients { get; set; }
    }

    public class NPlayerInfo
    {
        public string PlayerName { get; set; }
        public int Playernum { get; set; }
        public int skin { get; set; }
    }

    public class ClientInfo
    {
        public IPAddress IpAddress { get; private set; }
        public int Port { get; private set; }
        public string ClientName { get; set; }
        public int PortIntern { get; private set; }
        public UdpClient Socket { get; private set; }
        public List<NPlayerInfo> Players { get; private set; }
        public ClientInfo(IPAddress IpAddress, int Port, int PortIntern)
        {
            UdpClient udpClient = new UdpClient(PortIntern);
            udpClient.Connect(IpAddress,Port);
            Socket = udpClient;
        }
    }
}
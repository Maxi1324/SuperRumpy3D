using System.Net;
using System.Net.Sockets;

namespace Networking.NetworkClient
{
    class PeerToPeerNetworkCient : INetworkClient
    {
        public UdpClient Connect(IPAddress Address, int Port)
        {
            UdpClient Client = new UdpClient();
            Client.Connect(Address, Port);
            int OldPort = ((IPEndPoint)Client.Client.LocalEndPoint).Port;
            NetworkCommunication.SendDataSicher(Client, Settings.WelcomeMessage);
            IPEndPoint EndPoint = new IPEndPoint(IPAddress.Any, 0);
            string[] str = NetworkCommunication.ReceiveDataSicher(Client,ref EndPoint).Split(';');
            Client.Close();
            if(str[1] == "Port")
            {
                int PortNew = int.Parse(str[0]);
                UdpClient ClientReal = new UdpClient(OldPort);
                ClientReal.Connect(Address, PortNew);
                return ClientReal;
            }
            else
            {
                throw new System.Exception("Was BRUDA");
            }
        }
    }
}

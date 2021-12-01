using System.Net;
using System.Net.Sockets;
namespace Networking.NetworkClient
{
    public interface INetworkClient
    {
        public abstract UdpClient Connect(IPAddress Address, int Port);
    }
}

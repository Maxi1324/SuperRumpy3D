using Networking.Security;
using Open.Nat;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Networking.NetworkingManager
{
    internal class UPnPNetworkingManager : INetworkingManager
    {
        public NatDiscoverer discoverer { get; private set; }
        public NatDevice device { get; private set; }
        public Mapping mapping { get; private set; }
        public IPAddress Address { get; private set; }
        public int InPort { get; private set; }
        public ConnectInfo CInfo { get; private set; }

        private NetworkInfo _NInfo;
        public NetworkInfo NetworkInfo { get => _NInfo; set => _NInfo = value; }

        public void init()
        {
            new WelcomeNewClients(this);
            CInfo = new ConnectInfo() { Adresse = IPAddress.Parse("127.0.0.1"), Port = Settings.UDPPortInternal };
            new Task(() => { AsyncInit(); }).Start();
        }
        public void MenuUpdate()
        {

        }
        public void AsyncInit()
        {
            //OpenPort();
            //getIp();
        }

        public async void getIp()
        {
            discoverer = new NatDiscoverer();
            device = await discoverer.DiscoverDeviceAsync();
            Address = await device.GetExternalIPAsync();
            CInfo = new ConnectInfo() { Adresse = Address, Port = InPort };
        }

        public async void OpenPort(Protocol protocol = Protocol.Udp, int InternalPort = Settings.UDPPortExternal, int ExternalPort = Settings.UDPPortInternal, string desc = "ForNormalTraffic")
        {
            InPort = InternalPort;
            discoverer = new NatDiscoverer();
            device = await discoverer.DiscoverDeviceAsync();
            mapping = new Mapping(protocol, InternalPort, ExternalPort, desc);
            await device.CreatePortMapAsync(mapping);
        }
        public async void showMappings()
        {
            var nat = new NatDiscoverer();
            var cts = new CancellationTokenSource(5000);
            var device = await nat.DiscoverDeviceAsync(PortMapper.Upnp, cts);
            foreach (var mapping in await device.GetAllMappingsAsync())
            {
                Debug.Log(mapping);
            }
        }
        public void Update()
        {
            throw new NotImplementedException();
        }

        public string ConnectionInfo()
        {
            string str = "";
            if (CInfo != null)
            {
                str = "ConnectString: " + CInfo.connectString;
                str += "\nKey: " + StringEncrypt.instance.key;
            }
            else
            {
                str = "Online things loading...";
            }
            return str;
        }

        public string CopyInfo()
        {
            return CInfo.connectString;
        }
    }

    public class ConnectInfo
    {
        public IPAddress Adresse { get; set; }
        public int Port { get; set; }

        private string En;
        private string lastInfo;

        public string connectString
        {
            get
            {
                string str = Adresse + ";" + Port;
                string newstr = StringEncrypt.instance.Encrypt(str);
                if (str == lastInfo) return En;
                else
                {
                    lastInfo = str;
                    En = newstr;
                }
                return newstr;
            }
            set
            {
                string Plain = StringEncrypt.instance.Decrypt(value);
                string[] Parts = Plain.Split(';');
                Adresse = IPAddress.Parse(Parts[0]);
                Port = int.Parse(Parts[1]);
            }
        }
    }

    public class WelcomeNewClients
    {
        public Thread Thread1 { get; set; }
        public INetworkingManager NManager { get; set; }
        UdpClient Socket { get; set; }

        public WelcomeNewClients(INetworkingManager NManager)
        {
            this.NManager = NManager;
            Socket = new UdpClient(Settings.UDPPortInternal);
            Thread1 = new Thread(() =>
            {

            });
        }
        public void WelcomeClient()
        {
            for (int i = 1; i < 4;)
            {
                try
                {
                    IPEndPoint End = new IPEndPoint(IPAddress.Any, 0);
                    string str = NetworkCommunication.ReceiveDataSicher(Socket, ref End);
                    if (str == Settings.WelcomeMessage)
                    {
                        int Port = Settings.UDPPortInternal + i;
                        NetworkCommunication.SendDataSicher(Socket, (Port) + ";Port");
                        UdpClient Client = new UdpClient(Port);
                        Client.Connect(End);
                        ClientInfo CInfo = new ClientInfo(End.Address, End.Port, Port);
                        CInfo.ClientName = "Client " + i;
                        i++;
                    }
                }
                catch (Exception e)
                {
                    Debug.Log(e.StackTrace);
                }
            }

        }
    }
}
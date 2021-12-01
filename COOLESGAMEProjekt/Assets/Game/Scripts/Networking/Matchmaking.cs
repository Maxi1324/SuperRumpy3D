using UnityEngine;
using Networking.NetworkingManager;
using Networking.Security;
using Networking.NetworkClient;

namespace Networking
{
    public class Matchmaking : MonoBehaviour
    {
        public INetworkingManager NetworkingManager { get; private set; }
        public INetworkClient NetworkClient { get; set; }

        public Matchmaking()
        {
            new StringEncrypt("Cool1324");
            StringEncrypt.instance.Decrypt(StringEncrypt.instance.Encrypt("HAllo"));
        }

        public void InitUHPServer()
        {
           
        }

        public void InitUPnP()
        {
            NetworkingManager = new UPnPNetworkingManager();
            NetworkingManager.init();
        }

        public void UsePeerToPeer()
        {
            NetworkClient = new PeerToPeerNetworkCient();
        }

        public void CreateSession()
        {

        }
    }
}
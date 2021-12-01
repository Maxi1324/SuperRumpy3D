using UnityEngine;
using Networking;
using System.Threading.Tasks;
using Networking.NetworkingManager;
using TMPro;
using Gui.Lobby;
using Networking.NetworkClient;
using System.Text;

class MatchmakingGui:MonoBehaviour
{
    public GameObject JCChoose;
    public GameObject CreateChoose;
    public GameObject JoinChoose;
    public GameObject CreateGameOptions;
    public GameObject Lobby;
    public TextMeshProUGUI key;
    public GameObject EnterIPPort;
    public TextMeshProUGUI ConnectionString;


    public Matchmaking matchmaking;
    public LobbyGUI LobbyGui;
    private void Start()
    {
        JCChoose.SetActive(true);
        CreateChoose.SetActive(false);
        JoinChoose.SetActive(false);
        Lobby.SetActive(false);
    }

    public void ChooseCreate()
    {
        CreateChoose.SetActive(true);
    }

    public void ChooseJoin()
    {
        JoinChoose.SetActive(true);
    }

    public void UPnP()
    {
        matchmaking.InitUPnP();
        CreateGameOptions.SetActive(true);
    }

    public void UHPServer()
    {
        matchmaking.InitUHPServer();
        CreateGameOptions.SetActive(true);
    }

    public void ShowLobby()
    {
        Lobby.SetActive(true);
        CreateChoose.SetActive(false);
        JCChoose.SetActive(false);
        LobbyGui.init(true);
    }

    public void ShowUPnPMapping()
    {
        new Task(() => { ((UPnPNetworkingManager)matchmaking.NetworkingManager).showMappings(); }).Start();
    }

    public void EnterIPandPort()
    {
        EnterIPPort.SetActive(true);
    }

    public void UseConnectionString()
    {
        matchmaking.UsePeerToPeer();
        string conString = ConnectionString.text;
        ConnectInfo info = new ConnectInfo();
        conString = conString.Substring(0, conString.Length - 1);
        info.connectString = conString;
        matchmaking.NetworkClient.Connect(info.Adresse,info.Port);
        EnterIPPort.SetActive(false);
        JoinChoose.SetActive(false);
    }
}

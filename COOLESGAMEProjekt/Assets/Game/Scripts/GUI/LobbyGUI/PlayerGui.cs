using UnityEngine;
using TMPro;
using Networking;

namespace Gui.Lobby
{
    class PlayerGui:MonoBehaviour
    {
        public TextMeshProUGUI PlayerName;
        public LobbyGUI Lobby { get; set; }
        public int index { get; set; }
        public NPlayerInfo PInfo { get; set; }
        private void Update()
        {
            RectTransform rt2 = Lobby.PlayerGuiParrent.GetComponent<RectTransform>();
            RectTransform rt = GetComponent<RectTransform>();
            float pos = ((rt.rect.width * 2 + Lobby.PPadding) * (index+.5f))+20;
             
            transform.position = new Vector3(pos, 0, 0)+transform.parent.position;
        }

        public void init(string Name, NPlayerInfo PInfo)
        {
            PlayerName.text = (PInfo == null)?"Player "+(index+1):PInfo.PlayerName;
            this.PInfo = PInfo;
        }
    }
}

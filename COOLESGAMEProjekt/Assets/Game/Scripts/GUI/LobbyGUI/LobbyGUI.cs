using System;
using UnityEngine;
using Networking;
using TMPro;
using Networking.Security;
using System.Collections.Generic;

namespace Gui.Lobby
{
    class LobbyGUI : MonoBehaviour
    {
        public TMP_InputField ConnectInfo;
        public GameObject PlayerGui;
        public Transform PlayerGuiParrent;
        public bool IsOnline { get; set; } = false;
        public Matchmaking MMaking;

        public float PPadding = 20;

        public List<PlayerGui> PlayersGui { get; set; } = new List<PlayerGui>();

        internal static string Clipboard
        {
            get
            {
                TextEditor _textEditor = new TextEditor();
                _textEditor.Paste();
                return _textEditor.text;
            }
            set
            {
                TextEditor _textEditor = new TextEditor
                { text = value };

                _textEditor.OnFocus();
                _textEditor.Copy();
            }
        }

        public void init(bool IsOnline = false)
        {
            this.IsOnline = IsOnline;
        }

        private void Start()
        {
            for(int i = 0; i < 2; i++)
            {
                addPlayerToGui();
            }
        }

        private void Update()
        {
            if (IsOnline)
            {
                ConnectInfo.text = MMaking.NetworkingManager.ConnectionInfo();
            }
        }

        public void CopyConString()
        {
            Clipboard = MMaking.NetworkingManager.CopyInfo();
        }

        public void addPlayerToGui()
        {
            addPlayerToGui(PlayersGui.Count,null);
        }
        public void addPlayerToGui(int index,NPlayerInfo PI)
        {
            if(index > Settings.MaxPlayer)
            {
                throw new Exception("Zu Viele Spieler");
            }
            GameObject ob = Instantiate(PlayerGui,new Vector3(0,0,0),Quaternion.identity, PlayerGuiParrent);
            PlayerGui pl = ob.GetComponent<PlayerGui>();
            pl.index = index;
            pl.Lobby = this;
            pl.init("Player" + index, PI);
            PlayersGui.Add(pl);
        }
    }
}

using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace UI.CharacterSelection
{
    public class PlayerUIOB : MonoBehaviour
    {
        public TextMeshProUGUI Name;
        public Image TopDeckel;
        public Image Hintergrund;
        public RawImage PlayerRender;

        public UIPlayerInfo Player;
        private PlayerSkin? skin = null;

        public void Update()
        {
            if(Player != null && skin != Player.Skin)
            {
                Init(ColorVerzeichnis.Instance.Colors[Player.Skin]);
                skin = Player.Skin;
                PlayerRender.texture = UICreatePlayerStatisten.Instance.RenderTextures[(int)Player.Skin];
                TopDeckel.gameObject.SetActive(false);
                Debug.Log(Player.Skin);
            }    
        }

        public void Init(ColorOb Color)
        {
            TopDeckel.color = Color.PrimaryColor;
            Hintergrund.color = Color.TertiaryColor;
            Name.color = Color.TextColor;
        }
    }
}
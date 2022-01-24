using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class ColorVerzeichnis : MonoBehaviour
    {
        public Dictionary<PlayerSkin, ColorOb> Colors = new Dictionary<PlayerSkin, ColorOb>();
        public static ColorVerzeichnis Instance;
        public ColorVerzeichnis()
        {
            Instance = this;
            Colors.Add((PlayerSkin)0, new ColorOb() { PrimaryColor = new Color(155f / 255f, 89f /255f, 182f / 255),SecundaryColor=new Color(217 / 255f, 125f / 255, 255f / 255f),TertiaryColor=new Color(142f / 255f, 68f / 255f, 173f / 255f),TextColor= new Color(1,1,1) });
            Colors.Add((PlayerSkin)1, new ColorOb() { PrimaryColor = new Color(155f / 255f, 89f /255f, 182f / 255),SecundaryColor=new Color(217 / 255f, 125f / 255, 255f / 255f),TertiaryColor=new Color(142f / 255f, 68f / 255f, 173f / 255f),TextColor= new Color(1,1,1) });
            Colors.Add((PlayerSkin)2, new ColorOb() { PrimaryColor = new Color(155f / 255f, 89f /255f, 182f / 255),SecundaryColor=new Color(217 / 255f, 125f / 255, 255f / 255f),TertiaryColor=new Color(142f / 255f, 68f / 255f, 173f / 255f),TextColor= new Color(1,1,1) });
            Colors.Add((PlayerSkin)3, new ColorOb() { PrimaryColor = new Color(155f / 255f, 89f /255f, 182f / 255),SecundaryColor=new Color(217 / 255f, 125f / 255, 255f / 255f),TertiaryColor=new Color(142f / 255f, 68f / 255f, 173f / 255f),TextColor= new Color(1,1,1) });
        }
    }

    public class ColorOb{
        public Color PrimaryColor { get; set; }
        public Color SecundaryColor { get; set; }
        public Color TertiaryColor { get; set; }
        public Color TextColor { get; set; }
    }
}
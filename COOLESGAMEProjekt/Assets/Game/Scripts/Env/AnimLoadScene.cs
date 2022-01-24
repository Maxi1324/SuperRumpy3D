using Generell;
using Generell.LevelManager1;
using System;
using UnityEngine;

namespace Assets.Game.Scripts.Env
{
    public class AnimLoadScene : MonoBehaviour
    {
        public LevelManager LM;

        private void Start()
        {
        }
        public void load(string str)
        {
            PlayerPrefs.SetInt("FirstTime", 0);
            SceneLoader.loadScene(str, ()=> { });
        }
        public void CutSceneFinished()
        {
            LM.StartEvent();
        }
    }
}

using Generell.LevelManager1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Generell
{
    public class EndCutSceneStart : MonoBehaviour
    {
        bool once = false;
        public LevelManager LM;

        public int Level = 0;

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "Player" && !once)
            {
                once = true;
                LM.TriggerEndCutScene();
                int level = PlayerPrefs.GetInt("Level");
                if(level < Level)
                {
                    PlayerPrefs.SetInt("Level", Level);
                    Debug.Log("LevelChanged");
                }
            }
        }
    }
}
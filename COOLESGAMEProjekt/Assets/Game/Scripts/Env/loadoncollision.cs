using Generell;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loadoncollision : MonoBehaviour
{
    bool eins = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!eins && other.tag == "Player")
        {
            int level = PlayerPrefs.GetInt("Level");
            if (level < 2)
            {
                PlayerPrefs.SetInt("Level", 2);
                SceneLoader.loadScene("LevelSelection", () => { });
            }
        }
    }
}

using Generell;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubArealoader : MonoBehaviour
{
    public static int spawnplace = 0;
    public int spawnplace1 = 0;
    public string scene;

    public bool einaml;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (einaml)
            {
                return;
            }
            spawnplace = spawnplace1;
            einaml = true;
            SceneLoader.loadScene(scene, () =>
            {

            },false);
        }
    }
}

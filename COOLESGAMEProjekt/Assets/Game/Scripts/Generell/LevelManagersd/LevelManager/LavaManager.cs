using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Generell.LevelManager1
{
    public class LavaManager : LevelManager
    {
        public Transform Lava;
        public float LavaSpeed;

        private Vector3 StartPos;
        public float MaxY;
        private new void Start()
        {
            base.Start();
            StartPos = Lava.transform.position;
        }

        private new void Update()
        {
            if (Lava.transform.position.y < MaxY)
            {
                Lava.transform.position = Lava.transform.position + Vector3.up * LavaSpeed * Time.deltaTime;
            }
        }

        public override void ResetLevel()
        {
            Lava.transform.position = StartPos;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
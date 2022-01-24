using System;
using System.Collections.Generic;
using UnityEngine;

namespace Generell.LevelManager1
{
    public class BergLevelManager : LevelManager
    {
        public List<Transform> SecSpawn;

        public override void ResetLevel()
        {
            //throw new NotImplementedException();
        }

        private new void Start()
        {
            if(SubArealoader.spawnplace == 1)
            {
                SpawnPoints.Clear();
                SecSpawn.ForEach(s =>
                {
                    SpawnPoints.Add(s);
                });
                SubArealoader.spawnplace = 0;
                GameObject StartC = GameObject.Find("StartC");
                GameObject EndC = GameObject.Find("EndC");
                Destroy(StartC);
                StartEvent();
            }
            else
            {
                base.Start();
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Generell.LevelManager1
{
    public class BergLevelSubArea1 : LevelManager
    {
        public List<int> There = new List<int>();
        public GameObject Upgrade;

        private new void Start()
        {
            base.Start();
        }
        public override void ResetLevel()
        {
        }

        private void Update()
        {
            if (There.Count == 4)
            {
                RaetzelFertig();
            }
        }

        public void RaetzelFertig()
        {
            Upgrade.SetActive(true);
        }

        public void AddToRaetzel(int typ)
        {
            if (!There.Contains(typ))
            {
                There.Add(typ);
            }
        }
    }
}
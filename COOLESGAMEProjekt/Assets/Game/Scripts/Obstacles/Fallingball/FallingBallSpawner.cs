using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Obstacles.FallingBall {
    public class FallingBallSpawner : MonoBehaviour
    {
        public int MaxDistance;
        public Vector3 axis;
        public GameObject Ball;

        public List<FallingBall> FallingBalls;

        private void Start()
        {
            StartCoroutine(Spawn());
        }

        private void Update()
        {
            
        }

        IEnumerator Spawn()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(1, 2));
                Spawn1();
            }
        }

        public void Spawn1()
        {
            float dis = Random.Range(0, MaxDistance);
            Instantiate(Ball, new Vector3(axis.x * dis, axis.y * dis, axis.z * dis)+transform.position, Quaternion.identity);
        }
    }
}
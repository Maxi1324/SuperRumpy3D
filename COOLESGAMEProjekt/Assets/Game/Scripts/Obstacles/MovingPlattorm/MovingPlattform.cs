using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Obstacles.MovingPlattform
{
    public class MovingPlattform : MonoBehaviour
    {
        public int steps = 2;
        public List<Transform> poses;
        public float WaitAtStartEnd = 4;
        public float WaitBetweenSteps = 1;
        public GameObject ShowPathPrefab;
        public float LStepsPreLength = 2;
        public float speed = 2f;
        public bool waitForPlayer = false;

        private float timer = 0;
        private int atStep = 1;
        private bool wait = false;
        private float timeToWait = 0;
        private int dir = 1;

        private void FixedUpdate()
        {
            //transform.position = Vector3.Slerp(transform.position, poses[atStep].position, speed);
            Vector3 vec = (poses[atStep].position - transform.position).normalized;
            if (!wait && !waitForPlayer)
            {
                transform.position += vec * speed * Time.deltaTime;
                transform.rotation = Quaternion.Lerp(transform.rotation, poses[atStep].rotation, 0.01f);
            }
            if ((transform.position - poses[atStep].position).magnitude < 1f && !wait)
            {
                wait = true;
                timeToWait = WaitBetweenSteps;
                if (atStep == 0 || atStep == poses.Count - 1)
                {
                    timeToWait = WaitAtStartEnd;
                }
                timer = 0;
            }
            if (wait)
            {
                timer = timer + Time.deltaTime;
                if (timer > timeToWait)
                {
                    atStep = atStep + dir;
                    if (atStep >= poses.Count)
                    {
                        dir = -1;
                        atStep = atStep + dir;
                    }
                    if (atStep < 0)
                    {
                        dir = 1;
                        atStep = atStep + dir;
                    }
                    wait = false;
                }
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.tag == "Player")
            {
                waitForPlayer = false;
            }
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(MovingPlattform))]
    class PlattformEditor : Editor
    {
        int PathLength;
        MovingPlattform movePlatt;

        public override void OnInspectorGUI()
        {
            movePlatt = (MovingPlattform)target;
            base.OnInspectorGUI();

            if (EditorGUI.EndChangeCheck())
            {
                if (movePlatt.poses == null) movePlatt.poses = new List<Transform>();
            }

            if (GUILayout.Button("Generate Path"))
            {
                createPath(movePlatt.steps);
            }
            if (GUILayout.Button("Generate LSteps"))
            {
                createPathLittleSteps();
            }
            if (GUILayout.Button("Reset Path"))
            {
                if (movePlatt.poses != null)
                {
                    movePlatt.poses.ForEach((Transform t) =>
                    {
                        if (t != null)
                        {
                            DestroyImmediate(t.gameObject);
                        }
                    });
                }
                movePlatt.poses = new List<Transform>();
                Debug.Log("Path reseted");
            }
            if (GUILayout.Button("LoadPathObs"))
            {
                loadPath();
            }

        }

        public void loadPath()
        {
            movePlatt.poses = new List<Transform>();
            Transform[] trans = movePlatt.transform.parent.gameObject.GetComponentsInChildren<Transform>();
            for (int i = 0; i < trans.Length; i++)
            {
                string str = trans[i].name;
                if (str.Contains("Path:"))
                {
                    movePlatt.poses.Add(trans[i]);
                }
            }
            movePlatt.poses.Sort(new Comparer1());
            Debug.Log("Path loaded: " + movePlatt.poses.Count);
        }

        public void createPath(int Items)
        {
            if (Items < 2)
            {
                Debug.LogError("Strecke muss mindestens 2 Punkte haben");
                return;
            }
            int newItems = Items - movePlatt.poses.Count;
            if (newItems >= 0)
            {
                for (int i = 0; i < newItems; i++)
                {
                    int pos = (movePlatt.poses.Count > 0) ? 1 : 0;
                    GameObject gb = Instantiate(movePlatt.ShowPathPrefab, movePlatt.transform.position, Quaternion.identity, movePlatt.transform.parent);
                    movePlatt.poses.Insert(pos, gb.transform);
                }
            }
            else
            {
                for (int i = 0; i < -newItems; i++)
                {
                    Transform trans = movePlatt.poses[1];
                    movePlatt.poses.RemoveAt(1);
                    DestroyImmediate(trans.gameObject);
                }
            }
            Vector3 startPos = movePlatt.poses[0].position;
            Vector3 lastPos = movePlatt.poses[movePlatt.poses.Count - 1].position;

            Vector3 vec = (lastPos - startPos);
            float part = vec.magnitude / (Items - 1);
            vec = vec.normalized;
            for (int i = 0; i < movePlatt.poses.Count; i++)
            {
                GameObject ob = movePlatt.poses[i].gameObject;
                ob.transform.position = startPos + (vec * i * part);
                ob.name = "Path:" + i;
            }
            //createPathLittleSteps();
        }

        public void createPathLittleSteps()
        {
            List<Transform> poses = movePlatt.poses;

            if (poses.Count < 2)
            {
                Debug.LogError("LSteps können nicht erzeugt werden, da es weniger als Zwei Steps gibt");
                return;
            }

            float prefearedLength = movePlatt.LStepsPreLength;

            for (int i = 0; i < poses.Count - 1; i++)
            {
                Vector3 start = poses[i].position;
                Vector3 dis = poses[i + 1].position - poses[i].position;
                int LSteps = (int)(dis.magnitude / prefearedLength);
                float length = dis.magnitude / LSteps;
                Vector3 nDis = dis.normalized;

                for (int j = 1; j < LSteps; j++)
                {
                    GameObject ob = Instantiate(movePlatt.ShowPathPrefab, poses[i]);
                    ob.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                    ob.transform.position = start + length * j * nDis;
                }
            }
        }
    }
#endif

    class Comparer1 : IComparer<Transform>
    {
        public int Compare(Transform x, Transform y)
        {
            return x.name.CompareTo(y.name);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Entity.Player;

namespace Camera.Bad
{
    public class CameraController : MonoBehaviour
    {
        public Vector3 CamWorldOffset;
        public CinemachineTargetGroup targetGroup;
        Queue<Vector3> ToGetToOffset = new Queue<Vector3>();
        Vector3 localGoal;
        Vector3 lastLocalGoal;
        public CinemachineVirtualCamera cam;
        private CinemachineTransposer transposer;

        public float zoom = 1;

        public float t = 1;
        public float backToWorldOffset = 2;
        private float changeToNormal = 0;
        private bool wantToClear;

        private List<Transform> target = new List<Transform>();
        private List<float> Aim = new List<float>();
        private List<float> Max = new List<float>();

        public static CameraController CController;

        public CameraController()
        {
            CController = this;
        }

        void Start()
        {
            transposer = cam.GetCinemachineComponent<CinemachineTransposer>();
            localGoal = CamWorldOffset;

            PlayerManager[] manager = FindObjectsOfType<PlayerManager>();

            for (int i = 0; i < manager.Length; i++)
            {
                AddToFollowList(manager[i].transform, 5, 3);
            }
        }

        void FixedUpdate()
        {
            if (wantToClear)
            {
                if (changeToNormal > backToWorldOffset)
                {
                    clearReal();
                }
                changeToNormal += Time.deltaTime;
            }
            else
            {
                changeToNormal = 0;
            }

            if (localGoal == Vector3.zero)
            {
                if (ToGetToOffset.Count != 0)
                {
                    localGoal = ToGetToOffset.Dequeue();
                    if (localGoal.x == 999) localGoal = new Vector3(lastLocalGoal.x, localGoal.y, localGoal.z);
                    if (localGoal.y == 999) localGoal = new Vector3(localGoal.x, lastLocalGoal.y, localGoal.z);
                    if (localGoal.z == 999) localGoal = new Vector3(localGoal.x, localGoal.y, lastLocalGoal.z);

                }
            }

            else
            {
                Vector3 FollowOffset = transposer.m_FollowOffset;
                if ((FollowOffset - localGoal).magnitude > 0.1f)
                {
                    transposer.m_FollowOffset = Vector3.Slerp(FollowOffset, localGoal * zoom, t);
                }
                else
                {
                    lastLocalGoal = localGoal;
                    localGoal = Vector3.zero;
                }
            }

            for (int i = 0; i < target.Count; i++)
            {
                Transform tar = target[i];
                CinemachineTargetGroup.Target targalt = (CinemachineTargetGroup.Target)targetGroup.m_Targets.GetValue(i);
                targalt.weight += Aim[i] * 0.5f * Time.deltaTime;
                if (targalt.weight > Max[i])
                {
                    targalt.weight = Max[i];

                }
                else if (targalt.weight < 0)
                {
                    targalt.weight = 0;
                }
                targetGroup.m_Targets.SetValue(targalt, i);
            }
        }

        public void toTrack(List<Vector3> vecs)
        {
            wantToClear = false;
            ToGetToOffset.Clear();
            vecs.ForEach((Vector3 v) =>
            {
                ToGetToOffset.Enqueue(v);
            });
        }

        public void clearReal()
        {
            List<Vector3> vec = new List<Vector3>();
            vec.Add(CamWorldOffset);
            toTrack(vec);
        }

        public void clear()
        {
            wantToClear = true;
        }

        public void AddToFollowList(Transform trans, float radius = 20, float weigth = 1)
        {
            if (!target.Contains(trans))
            {
                target.Add(trans);
                Aim.Add(1);
                targetGroup.AddMember(trans, 1, radius);
                Max.Add(weigth);
            }
            else
            {
                int index = -1;
                for (int i = 0; i < target.Count; i++)
                {
                    if (target[i] == trans) index = i;
                }
                Aim[index] = 1;
                Max[index] = weigth;
            }
        }

        public void RemoveFromFollowing(Transform trans)
        {
            if (target.Contains(trans))
            {
                int index = -1;
                for (int i = 0; i < target.Count; i++)
                {
                    if (target[i] == trans) index = i;
                }
                Aim[index] = -1;
            }
        }
    }
}

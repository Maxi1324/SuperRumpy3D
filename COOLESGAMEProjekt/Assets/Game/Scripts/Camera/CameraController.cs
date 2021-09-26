using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public Vector3 CamWorldOffset;
    public CinemachineTargetGroup targetGroup;
    Queue<Vector3> ToGetToOffset = new Queue<Vector3>();
    Vector3 localGoal;
    Vector3 lastLocalGoal;
    public CinemachineVirtualCamera cam;
    private CinemachineTransposer transposer;

    public float t = 1;
    public float backToWorldOffset = 2;
    private float changeToNormal = 0;
    private bool wantToClear;

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
            AddToFollowList(manager[i].transform, 1, 20);
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
                transposer.m_FollowOffset = Vector3.Slerp(FollowOffset, localGoal, t);
            }
            else
            {
                lastLocalGoal = localGoal;
                localGoal = Vector3.zero;
            }
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

    public void AddToFollowList(Transform trans, float weight, float radius = 20)
    {
        if (targetGroup.FindMember(trans) == -1)
        {
            targetGroup.AddMember(trans, weight, radius);
            Debug.Log("da");
        }
    }

    public void RemoveFromFollowing(Transform trans)
    {
        Debug.Log("weg");
        if (targetGroup.FindMember(trans) != -1)
        {
            targetGroup.RemoveMember(trans);
        }
    }
}

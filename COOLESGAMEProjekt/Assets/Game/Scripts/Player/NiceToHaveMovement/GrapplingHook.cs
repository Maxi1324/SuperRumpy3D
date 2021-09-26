using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MovementAbility
{
    public PlayerManager PManager;
    public float hookRadius;

    public float AnimTime = 1;
    public float AnimHeight = 2;
    public int AnimQuality = 5;

    private Transform hookPoint;
    private float AnimTimer = 0;

    private bool lastWasOk = false;

    private void Reset()
    {
        PManager = GetComponent<PlayerManager>();
    }

    public override bool Active(float distance, InteractPlayer ob, bool allowed)
    {
        if (hookRadius > distance )
        {
            CameraController.CController.AddToFollowList(ob.transform, 0.9f);
            if (allowed)
            {
                lastWasOk = true;
                if (ob.key == "GrapplingPoint" && hookPoint == null && PManager.Fire3)
                {
                    hookPoint = ob.transform;
                    Debug.Log("hook");
                    PManager.AllowedMoves = 2;
                    return true;
                }
            }
        }
        if(lastWasOk && !(hookRadius > distance))
        {
            CameraController.CController.RemoveFromFollowing(ob.transform);
        }
        return false;
    }

    private void Update()
    {
        if (PManager.AllowedMoves == 2 && hookPoint != null)
        {
            LineRenderer renderer = PManager.PInfo.LineRenderer;
            AnimTimer += Time.deltaTime;

            Vector3 Line =  hookPoint.transform.position- transform.position ;
            renderer.positionCount = (int)(Line.magnitude) * AnimQuality;
            Vector3 NLine = Line.normalized;
            float length = Line.magnitude / AnimTime;
            Vector3 lastPos = Vector3.zero;

            Quaternion quat = Quaternion.LookRotation(NLine, Vector3.up);
            for(int i = 0; i < renderer.positionCount; i++)
            {
                Vector3 PPosOr = (NLine / AnimQuality)*i;
                Debug.DrawRay(transform.position, Line, Color.green);
                Vector3 PPos = PPosOr + transform.position;

                PPos += Mathf.Sin((Line.magnitude-PPosOr.magnitude)) * AnimHeight * (PPosOr.magnitude- Line.magnitude) * (AnimTime-AnimTimer) * (quat*Vector3.up);

                if (Line.magnitude - PPosOr.magnitude == 0) Debug.Log("wefg");
                if (PPosOr.magnitude <= length*AnimTimer)
                {
                    lastPos = PPos;
                    renderer.SetPosition(i, PPos);
                }
                else
                {
                    renderer.SetPosition(i, lastPos);
                }
            }


            if (AnimTimer > AnimTime)
            {
                renderer.positionCount = 2;
                renderer.SetPosition(1, hookPoint.transform.position);
                renderer.SetPosition(0, transform.position);

                SpringJoint joint = GetComponent<SpringJoint>();
                if (joint == null)
                {
                    gameObject.AddComponent<SpringJoint>();
                    joint = GetComponent<SpringJoint>();

                    joint.connectedBody = hookPoint.GetComponent<Rigidbody>();
                    joint.anchor = Vector3.zero;
                    joint.autoConfigureConnectedAnchor = false;
                    joint.connectedAnchor = Vector3.zero;
                    joint.spring = 1.25f;
                }
            }
        }
        else
        {
            LineRenderer renderer = PManager.PInfo.LineRenderer;
            renderer.positionCount = 0;

            SpringJoint joint = GetComponent<SpringJoint>();
            if (joint != null)
            {
                Destroy(joint);
            }
        }
        if ((hookPoint != null && !PManager.Fire3) || PManager.OnGround)
        {
            CameraController.CController.RemoveFromFollowing(hookPoint);
            AnimTimer = 0;
            hookPoint = null;
            PManager.AllowedMoves = 1;
            if (PManager.OnGround) PManager.AllowedMoves = 0;
        }
    }

    public override bool Allowed(int allowedMoves)
    {
        return allowedMoves == 1 || (allowedMoves == 0 && !PManager.OnGround);
    }
}

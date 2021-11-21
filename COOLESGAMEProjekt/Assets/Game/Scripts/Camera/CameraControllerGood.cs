using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraControllerGood : MonoBehaviour
{
    public Vector3 Offset { get; set; }

    public CinemachineTargetGroup Targetgroup;
    public CinemachineVirtualCamera VirtualCamera;

    public float dis = 1;

    public float t = 0.1f;

    private CinemachineTransposer Transposer;

    public static CameraControllerGood Instance { get; set; }


    public CameraControllerGood()
    {
        Instance = this;
    }

    private void Start()
    {
        FindPlayers();
        Transposer = VirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
    }

    private void LateUpdate()
    {
        DoOffset();
    }

    private void FindPlayers()
    {
        PlayerManager[] Players = FindObjectsOfType<PlayerManager>();
        foreach(PlayerManager Player in Players)
        {
            Transform transform = Player.transform;
            if (Targetgroup.FindMember(transform) == - 1)
            {
                Targetgroup.AddMember(transform, 1, 15);
            }
        }
    }

    private void DoOffset()
    {
        Vector3 OldOffset = Transposer.m_FollowOffset;
        Vector3 NewOffset = Vector3.Lerp(OldOffset, Offset*dis, t);
        Transposer.m_FollowOffset = NewOffset;
    }
}

/*
 * Player Suchen und zur Follow list hinzufügen
 * Zwischen CameraZones interpolieren
 */

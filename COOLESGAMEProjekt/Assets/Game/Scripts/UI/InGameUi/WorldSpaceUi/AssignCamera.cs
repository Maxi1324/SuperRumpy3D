using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignCamera : MonoBehaviour
{
    public Canvas canvas;
    public Canvas SecRender;

    private void Reset()
    {
        canvas = GetComponent<Canvas>();
    }

    void Start()
    {
        UnityEngine.Camera cam = FindObjectOfType<CinemachineBrain>().GetComponent<UnityEngine.Camera>();
        canvas.worldCamera = cam;

        UnityEngine.Camera cam2 = cam.transform.GetChild(0).GetComponent<UnityEngine.Camera>();
        SecRender.worldCamera = cam2;
    }
}

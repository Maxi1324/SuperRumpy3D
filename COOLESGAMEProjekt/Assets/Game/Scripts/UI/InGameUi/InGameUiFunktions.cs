using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class InGameUiFunktions : MonoBehaviour
{
    public TextMeshProUGUI DialogText;
    public GameObject Dialog;
    public GameObject D;
    public Transform AButton;
    private bool isShowing;

    private float orTimeScale;

    public static InGameUiFunktions Instance;

    private void Start()
    {
        Instance = this;
        orTimeScale = Time.timeScale;
        D.SetActive(false);
    }

    public void ShowText(string str, Action OnFertig)
    {
        isShowing = true;
        D.SetActive(true);
        DialogText.text = str;
        Vector3 oldScale = Dialog.transform.localScale;
        Time.timeScale = 0;
        D.SetActive(true);
        StartCoroutine(ScaleUD(true,oldScale,()=>
        {
            StartCoroutine(WaitForA(OnFertig));
        }, Dialog,0));
    }

    public IEnumerator ScaleUD(bool scaleUp, Vector3 oldScale, Action Fertig, GameObject Dialog, float MinScale = 0, float speed = 0.7f)
    {
        if (Dialog == null) yield break;
        Vector3 Dir = scaleUp ? oldScale : oldScale* MinScale;
        if (scaleUp)
        {
            Dialog.transform.localScale = oldScale*MinScale;
        }
        else
        {
            Dialog.transform.localScale = oldScale;
        }
        while (Dialog != null && Dialog.transform.localScale != Dir)
        {
            Dialog.transform.localScale = Vector3.Slerp(Dir, Dialog.transform.localScale, speed);
            yield return null;
        }
        Fertig();
    }

    IEnumerator WaitForA(Action OnFertig)
    {
        bool run = true;
        while (run)
        {
            for(int i = 0; i < 5;i++) {
                if (Input.GetButtonDown("Fire3"+ (i == 0?"":i+"")))
                {
                    Vector3 oldScale = Dialog.transform.localScale;
                    Time.timeScale = 1;
                    isShowing = false;
                    D.SetActive(false);
                    DialogText.text = "";
                    OnFertig();
                    run = false;
                    yield break;
                }
                else
                {
                    yield return null;
                }
            }

        }
    }
}

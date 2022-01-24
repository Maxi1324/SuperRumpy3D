using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Generell
{
    public class SceneLoader : MonoBehaviour
    {
        private static SceneLoader Instance;
        public Animator TransitionAnimator;
        public Canvas canvas;
        public Transform LoadingBar;
        public GameObject LoadingUeberOb;

        private AsyncOperation operation;

        private int SceneLoad = 0;
        private Action AfterLoaded;

        public SceneLoader()
        {
            Instance = this;
        }

        private void Start()
        {
            LoadingUeberOb.SetActive(false);
            SceneManager.sceneLoaded += (Scene, LoadSceneMode) =>
            {
                SceneLoad = 10;
                TransitionAnimator.SetBool("TransitionBack", true);

            };
        }

        private void Update()
        {
            if (SceneLoad > 0)
            {
                SceneLoad--;
                canvas.gameObject.SetActive(false);
                canvas.gameObject.SetActive(true);
                TransitionAnimator.SetBool("TransitionBack", true);
                AfterLoaded();
            }
        }

        public void stop1()
        {
            canvas.enabled = false;
            TransitionAnimator.SetBool("TransitionBack", false);
        }

        public static void loadScene(string Scene, Action AfterSceneLoad, bool waitforAnimation = true)
        {
            Instance.AfterLoaded = AfterSceneLoad;
            if (Instance.operation == null || Instance.operation.progress > 0.8f)
            {
                Instance.SC(Scene, waitforAnimation);
            }
        }


        public static void Transition(bool Fadein, Action AfterSceneLoad)
        {
            Instance.StartCoroutine(Instance.Trans(Fadein, AfterSceneLoad));
        }

        IEnumerator Trans(bool Fadein,Action Hallo)
        {
            Instance.canvas.enabled = true;
            TransitionAnimator.Play(Fadein? "FadeInd": "ToLoading", 0);
            operation = null;
            LoadingBar.gameObject.SetActive(false);
            if (!Fadein)
            {
                TransitionAnimator.SetBool("QuickOut", true);
            }
            yield return null;
            yield return new WaitForSeconds(Fadein?1.5f:1.7f);
            TransitionAnimator.SetBool("QuickOut", false);
            TransitionAnimator.SetBool(Fadein ? "TransitionBack" : "TransitionTo", false);
            Instance.canvas.enabled = false;
            Hallo();
        }

        private void SC (string scene, bool WatiAnim)
        {
            StartCoroutine(WaitForLoading(scene, WatiAnim));
        }



        IEnumerator WaitForLoading(string Scene, bool WaitAnim)
        {
            Instance.canvas.enabled = true;
            Instance.TransitionAnimator.SetBool("TransitionTo", true);
            LoadingUeberOb.SetActive(true);
            yield return new WaitForSeconds(1);
            operation = SceneManager.LoadSceneAsync(Scene);
            operation.allowSceneActivation = !WaitAnim;
            while (!(operation.progress > 0.8))
            {
                LoadingUeberOb.SetActive(true);
                LoadingBar.transform.localScale = new Vector3(operation.progress, 1, 1);
                yield return null;
            }
            Instance.TransitionAnimator.SetBool("TransitionTo", false);
        }

        public void LoadScene()
        {
            if (operation != null)
            {
                LoadingUeberOb.SetActive(false);
                operation.allowSceneActivation = true;
                Debug.Log("active");
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class LavaFontaene : MonoBehaviour
{
    public List<GameObject> Fonts1;

    public void Start()
    {
        StartCoroutine(TurnOnOff());
    }

    IEnumerator TurnOnOff()
    {
        bool zweites = false;
        while (true)
        {
            for (int i = 0; i < Fonts1.Count; i++)
            {
                GameObject Fontaene = Fonts1[i];
                StartCoroutine(changeState(Fontaene, zweites ? i % 2 == 0 : i % 2 != 0));
            }
            zweites = !zweites;
            yield return new WaitForSeconds(5);
        }
    }

    IEnumerator changeState(GameObject ob, bool state)
    {
        yield return new WaitForSeconds(2);
        if (!state)
        {
            ob.GetComponent<VisualEffect>().Stop();
        }
        else
        {
            ob.GetComponent<VisualEffect>().Play();
        }
        yield return new WaitForSeconds(2);
        if (!state)
        {
            ob.GetComponent<Collider>().enabled = false;
        }
        else
        {
            ob.GetComponent<Collider>().enabled = true;
        }
        yield return new WaitForSeconds(1);
    }
}

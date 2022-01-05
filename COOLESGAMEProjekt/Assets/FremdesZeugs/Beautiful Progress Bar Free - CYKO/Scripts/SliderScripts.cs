using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class SliderScripts : MonoBehaviour
{
    public Slider slider;
    public Image fill;

    private void Start()
    {
        FillSlider();
    }

    public void FillSlider()
    {
        fill.fillAmount = slider.value;
    }
}

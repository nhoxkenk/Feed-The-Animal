using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpBarController : MonoBehaviour
{
    Slider slider;
    int progress = 0;
    int maxProgressValue = 10;

    public float expSliderValue { get { return slider.value; } }
    public float expSliderMaxValue { get { return slider.maxValue; } }

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
    }

    public void UpdateProgressBar()
    {
        progress++;
        slider.value = progress;    
    }

    public void IncreaseMaxProgress()
    {
        maxProgressValue += 10;
        progress = 0;
        slider.maxValue = maxProgressValue;
        slider.value = progress;
    }
}

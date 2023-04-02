using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FramerateSlider : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI sliderText;

    private void Start()
    {
        slider.onValueChanged.AddListener(delegate { SetMaxFPS((int)slider.value); });
        slider.value = slider.maxValue;
    }

    // Update is called once per frame
    void Update()
    {
        if (sliderText != null)
        {           
            sliderText.text = Application.targetFrameRate.ToString();
        }   
    }

    private void SetMaxFPS(int framerate)
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = framerate;
    }
}

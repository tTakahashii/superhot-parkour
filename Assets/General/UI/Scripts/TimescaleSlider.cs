using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimescaleSlider : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI sliderText;

    private void Start()
    {
        slider.onValueChanged.AddListener(delegate { GameState.SetTimeScale(slider.value); });
        slider.value = GameState.GetTimeScale();
    }

    // Update is called once per frame
    void Update()
    {
        if (sliderText != null)
        {
            sliderText.text = GameState.GetTimeScale().ToString("0.00");
        }   
    }
}

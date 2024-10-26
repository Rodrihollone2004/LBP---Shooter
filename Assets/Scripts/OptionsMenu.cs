using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private Slider sensitivitySlider;

    private void Start()
    {
        float savedSensitivity = PlayerPrefs.GetFloat("CameraSensitivity", 100f);
        sensitivitySlider.value = savedSensitivity;

        sensitivitySlider.onValueChanged.AddListener(ChangeSensitivity);
    }

    private void ChangeSensitivity(float newSensitivity)
    {
        PlayerPrefs.SetFloat("CameraSensitivity", newSensitivity);
        PlayerPrefs.Save();
    }
}

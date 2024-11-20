using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private Slider volumeSlider;

    private void Start()
    {
        sensitivitySlider.onValueChanged.AddListener(ChangeSensitivity);
        volumeSlider.onValueChanged.AddListener(ChangeVolume);
        float savedSensitivity = PlayerPrefs.GetFloat("CameraSensitivity", 100f);
        sensitivitySlider.value = savedSensitivity;

        float savedVolume = PlayerPrefs.GetFloat("GeneralVolume", 1f);
        volumeSlider.value = savedVolume;

    }

    private void ChangeSensitivity(float newSensitivity)
    {
        PlayerPrefs.SetFloat("CameraSensitivity", newSensitivity);
        PlayerPrefs.Save();
    }

    private void ChangeVolume(float newVolume)
    {
        AudioListener.volume = volumeSlider.value;
        PlayerPrefs.SetFloat("GeneralVolume", newVolume);
        PlayerPrefs.Save();
    }
}

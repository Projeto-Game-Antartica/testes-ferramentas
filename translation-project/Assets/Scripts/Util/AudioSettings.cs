using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour {

    public AudioMixer mixer;
    public Slider slider;

    void Start()
    {
        slider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
    }

    public void SetLevel(float sliderValue)
    {
        mixer.SetFloat("volume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("MusicVolume", sliderValue);
    }

    public void EnableSlider(Slider slider)
    {
        if (slider.IsActive())
            slider.gameObject.SetActive(false);
        else
            slider.gameObject.SetActive(true);
    }

    public void OnValueChange(float value)
    {
        Debug.Log("Volume " + value);
        if (Parameters.ACCESSIBILITY) TolkUtil.Speak("Volume " + value);
    }
}

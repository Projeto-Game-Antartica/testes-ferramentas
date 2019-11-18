using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings : AbstractScreenReader {

    /*
     * volslider is the master volume of the game. Soundfxslider represents some of sounds effects in the game.
     * soundfx depends on volslider, i.e., if volslider is 0 then soundfx is 0.
     */
    public AudioMixer mixer;
    public Slider volslider;
    public Slider soundfxslider;

    public GameObject volumeControl;

    void Start()
    {
        volslider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
        soundfxslider.value = PlayerPrefs.GetFloat("Soundfx", 1f);
    }

    public void ReadVolSlider(Slider slider)
    {
        ReadText("Volume " + slider.value.ToString("F"));
    }

    public void ReadSoundFXSlider(Slider slider)
    {
        ReadText("Volume dos efeitos especiais " + slider.value.ToString("F"));
    }
    
    public void SetVolumeLevel(float sliderValue)
    {
        mixer.SetFloat("volume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("MusicVolume", sliderValue);
    }

    public void SetSoundFXLevel(float sliderValue)
    {
        mixer.SetFloat("soundfx", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("Soundfx", sliderValue);
    }

    public void EnableSlider(Slider slider)
    {
        if (slider.IsActive())
            slider.gameObject.SetActive(false);
        else
            slider.gameObject.SetActive(true);
    }

    public void EnableVolumeControlPainel()
    {
        if (volumeControl.activeSelf)
            volumeControl.SetActive(false);
        else
            volumeControl.SetActive(true);
    }

    public void OnValueChange(float value)
    {
        //Debug.Log("Volume " + value);
        //ReadText("Volume " + value);
    }
}

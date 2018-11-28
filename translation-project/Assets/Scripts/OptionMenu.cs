using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DavyKager;
using UnityEngine.UI;
using TMPro;

public class OptionMenu : MonoBehaviour {

    public Slider slider;
    public TextMeshProUGUI volumeText;
    public Button backButton;

    void Start()
    {
        slider = GameObject.Find("SliderVolume").GetComponent<Slider>();
        volumeText = GameObject.Find("Volume").GetComponent<TextMeshProUGUI>();
        backButton = GameObject.Find("BackButton").GetComponent<Button>();

        
        TolkUtil.Load();

        slider.Select();
    }

    public void ButtonBackAudio()
    {
        Debug.Log("BackButton");
        TolkUtil.Speak(backButton.GetComponentInChildren<TextMeshProUGUI>().text);
    }

    public void SliderVolumeAudio()
    {
        Debug.Log("VolumeSlider");
        TolkUtil.Speak(volumeText.text + slider.value);
    }

    public void TextVolumeAudio()
    {
        Debug.Log("TextVolume");
        TolkUtil.Speak(volumeText.text);
    }
}

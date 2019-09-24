using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DavyKager;
using UnityEngine.UI;
using TMPro;

public class OptionMenu : AbstractScreenReader {

    public Slider slider;
    public Toggle ScreenReaderToggle;
    public Toggle HighContrastToggle;
    public LocalizationManager localization;

    private ReadableTexts readableTexts;

    public FontSizeText fontSizeText;

    public AudioClip selectClip;

    void Start()
    {
        readableTexts = GameObject.Find("ReadableTexts").GetComponent<ReadableTexts>();
        TolkUtil.Instructions();
        ReadText(readableTexts.GetReadableText(ReadableTexts.key_optionmenu_instructions, LocalizationManager.instance.GetLozalization()));
        
        //TolkUtil.Load();

        slider.Select();
        ScreenReaderToggle.isOn = Parameters.ACCESSIBILITY;
        HighContrastToggle.isOn = Parameters.HIGH_CONTRAST;
    }

    public void DropDownHandler(int index)
    {
    }

    private void OnEnable()
    {
        //GameObject[] newTexts = GameObject.FindGameObjectsWithTag("text-hc");
        //fontSizeText.SetNewTexts(newTexts);

        FontSizeText.texts = GameObject.FindGameObjectsWithTag("text-hc");
    }

    public void SliderSettings(Slider slider)
    {
        ReadText("volume" + slider.value);
    }

    public void ReadToggle(Toggle toggle)
    {
        ReadText(toggle.name + "" + toggle.isOn);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            ReadText(readableTexts.GetReadableText(ReadableTexts.key_optionmenu_instructions, LocalizationManager.instance.GetLozalization()));
        }
    }

    public void SetAcessibilityParameter()
    {
        Parameters.ACCESSIBILITY = ScreenReaderToggle.isOn;
        Debug.Log("A: " + Parameters.ACCESSIBILITY);

        if (Parameters.ACCESSIBILITY) TolkUtil.Load();
        else TolkUtil.Unload();
    }

    public void SetHighContrastParameter()
    {
        Parameters.HIGH_CONTRAST = HighContrastToggle.isOn;
        Debug.Log("HC " + Parameters.HIGH_CONTRAST);
    }

    public void PlaySelectionAudio()
    {
        if (GetComponent<AudioSource>() == null)
        {
            gameObject.AddComponent<AudioSource>();

            UnityEngine.Audio.AudioMixer audioMixer = Resources.Load("Audio/AudioMixer") as UnityEngine.Audio.AudioMixer;
            GetComponent<AudioSource>().outputAudioMixerGroup = audioMixer.FindMatchingGroups("Master")[0];
        }

        PlaySelectAudio(GetComponent<AudioSource>(), selectClip);
    }
}

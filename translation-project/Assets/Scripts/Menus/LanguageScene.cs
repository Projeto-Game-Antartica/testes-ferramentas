using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageScene : AbstractScreenReader {

    private Button brButton;

    private ReadableTexts readableTexts;

    public AudioClip selectClip;

    private void Start()
    {
        readableTexts = GameObject.Find("ReadableTexts").GetComponent<ReadableTexts>();
        brButton = GameObject.Find("locales-ptbr").GetComponent<Button>();

        // accessibility and high contrast functions inactive
        Parameters.ACCESSIBILITY = false;
        Parameters.HIGH_CONTRAST = false;

        //TolkUtil.Load();

        TolkUtil.Instructions();
        ReadText(readableTexts.GetReadableText(ReadableTexts.key_languagemenu_instructions, "locales_ptbr.json"));

        brButton.Select();  
    }

    public void Update()
    {
        if (Input.GetKey(KeyCode.F1))
        {
            ReadText(readableTexts.GetReadableText(ReadableTexts.key_languagemenu_instructions, "locales_ptbr.json"));
        }
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

    public void Quit()
    {
        Application.Quit();
    }
}

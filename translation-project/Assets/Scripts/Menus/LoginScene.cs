using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginScene : AbstractScreenReader {

    public UnityEngine.UI.Button firstButton;

    public AudioClip selectClip;

    private void Start()
    {
        firstButton.Select();
    }

    public void CreateAccount()
    {
        SceneManager.LoadScene(ScenesNames.Cadastro);
    }

    public void LoginFacebook()
    {
        // TO DO
        SceneManager.LoadScene(ScenesNames.Menu);
    }

    public void LoginEmail()
    {
        // TO DO
        SceneManager.LoadScene(ScenesNames.Menu);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HelpMenu : AbstractScreenReader {

    public UnityEngine.UI.Button firstButton;

    public FontSizeText fontSizeText;

    public AudioClip selectClip;

    public void Awake()
    {
        TolkUtil.Load();
        firstButton.Select();
    }

    private void OnEnable()
    {
        FontSizeText.texts = GameObject.FindGameObjectsWithTag("text-hc");
    }

    public void ManualAluno()
    {
        // TO DO
    }

    public void ManualProfessor()
    {
        // TO DO
    }

    public void GlossarioLibras()
    {
        SceneManager.LoadScene(ScenesNames.Glossary);
    }

    public void GlossarioSons()
    {
        SceneManager.LoadScene(ScenesNames.GlossarySound);
    }

    public void PrivacidadeTermos()
    {
        // TO DO
    }

    public void Voltar()
    {
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

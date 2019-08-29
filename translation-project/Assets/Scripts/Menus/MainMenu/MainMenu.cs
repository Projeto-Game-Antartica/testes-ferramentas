using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.Audio;

public class MainMenu : AbstractScreenReader
{
    public Slider slider;
    private Button playButton;

    private ReadableTexts readableTexts;

    public GameObject loadScreenObject;
    public Slider loadingSlider;

    public AudioClip selectClip;

    public GameObject confirmQuit;

    AsyncOperation async;

    // player preferences hp and exp
    private float HP = 1f;
    private float EXP = 0f;

    void Start()
    {
        // localization
        LocalizationManager.instance.LoadLocalizedText("locales_ptbr.json");

        // accessibility and high contrast functions inactive
        Parameters.ACCESSIBILITY = false;
        Parameters.HIGH_CONTRAST = false;

        // set the volume value as slider value
        slider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);

        readableTexts = GameObject.Find("ReadableTexts").GetComponent<ReadableTexts>();

        playButton = GameObject.Find("PlayButton").GetComponent<Button>();

        //TolkUtil.Load();

        TolkUtil.Instructions();
        ReadText(readableTexts.GetReadableText(ReadableTexts.key_mainmenu_instructions, LocalizationManager.instance.GetLozalization()));

        playButton.Select();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1))
        {
            ReadText(readableTexts.GetReadableText(ReadableTexts.key_mainmenu_instructions, LocalizationManager.instance.GetLozalization()));
        }

        if (Parameters.HIGH_CONTRAST) HighContrastText.ChangeTextBackgroundColor();
        else HighContrastText.RestoreToDefault("bgothm");
    }

    public void TryQuitGame()
    {
        confirmQuit.SetActive(true);
        ReadText("Tem certeza que deseja sair do jogo?");
        confirmQuit.GetComponentInChildren<Button>().Select();
    }

    public void QuitGame()
    {
        TolkUtil.Unload();
        Debug.Log("Quit");
        Application.Quit();
        confirmQuit.SetActive(false);
    }

    public void LoadGlossary()
    {
        SceneManager.LoadScene(ScenesNames.Glossary);
    }

    public void LoadSoundGlossary()
    {
        SceneManager.LoadScene(ScenesNames.GlossarySound);
    }

    public void LoadAjudaGlossarioScene()
    {
        //SceneManager.LoadScene("");
    }

    public void LoadGame()
    {
        StartCoroutine(LoadingScreen());

        PlayerPrefs.SetFloat("HealthPoints", HP);
        PlayerPrefs.SetFloat("Experience", EXP);
    }

    public IEnumerator LoadingScreen()
    {
        loadScreenObject.SetActive(true);
        async = SceneManager.LoadSceneAsync(ScenesNames.M004Ship);
        async.allowSceneActivation = false;

        while(!async.isDone)
        {
            slider.value = async.progress;
            if(async.progress == 0.9f)
            {
                slider.value = 1f;
                async.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    public void PlaySelectionAudio()
    {
        if(GetComponent<AudioSource>() == null)
        {
            gameObject.AddComponent<AudioSource>();

            AudioMixer audioMixer = Resources.Load("Audio/AudioMixer") as AudioMixer;
            GetComponent<AudioSource>().outputAudioMixerGroup = audioMixer.FindMatchingGroups("Master")[0];
        }

        PlaySelectAudio(GetComponent<AudioSource>(), selectClip);
    }
}

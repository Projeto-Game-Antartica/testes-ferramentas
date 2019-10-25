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

    public TMPro.TextMeshProUGUI loadingText;

    AsyncOperation async;

    // player preferences hp and exp
    private float HP = 1f;
    private float EXP = 0f;

    void Start()
    {
        // set the parameter to show the instruction interface when loading the game
        PlayerPrefs.SetInt("InstructionInterface", 0);
        // set the saved position int to 0
        PlayerPrefs.SetInt("Saved", 0);
        
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
        //ReadText(readableTexts.GetReadableText(ReadableTexts.key_mainmenu_instructions, LocalizationManager.instance.GetLozalization()));
        ReadText("Sob o fundo da tela de navegação principal no canto inferior direito botões de funcionalidades do jogo.");

        playButton.Select();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1))
        {
            //ReadText(readableTexts.GetReadableText(ReadableTexts.key_mainmenu_instructions, LocalizationManager.instance.GetLozalization()));
            ReadText("Sob o fundo da tela de navegação principal no canto inferior direito botões de funcionalidades do jogo.");
        }

        if (Parameters.HIGH_CONTRAST) HighContrastText.ChangeTextBackgroundColor();
        else HighContrastText.RestoreToDefault("average");
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
        async = SceneManager.LoadSceneAsync(ScenesNames.M002Ushuaia);

        loadScreenObject.SetActive(true);

        while(!async.isDone)
        {
            float progress = Mathf.Clamp01(async.progress / .9f);

            loadingSlider.value = progress;

            Debug.Log(progress);

            loadingText.text = (progress * 100f).ToString("F0") + "%";

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

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

    public GameObject loadScreenObject;
    public Slider loadingSlider;

    public GameObject optionMenu;
    public GameObject helpMenu;


    public GameObject confirmQuit;

    public TMPro.TextMeshProUGUI loadingText;
    AsyncOperation async;

    public AudioSource audioSource;
    public AudioClip warningClip;
    public AudioClip loadingClip;

    // player preferences hp and exp
    private float HP = 1f;
    private float EXP = 0f;

    void Start()
    {
        //// localization
        //LocalizationManager.instance.LoadLocalizedText("locales_ptbr.json");
        
        // set the volume value as slider value
        slider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);

        playButton = GameObject.Find("PlayButton").GetComponent<Button>();

        //TolkUtil.Load();

        //TolkUtil.Instructions();
        ReadText(ReadableTexts.instance.GetReadableText(ReadableTexts.key_prejogo_menu, LocalizationManager.instance.GetLozalization()));

        //Debug.Log(ReadableTexts.instance.GetReadableText(ReadableTexts.key_prejogo_menu, LocalizationManager.instance.GetLozalization()));
        //ReadText("Sob o fundo da tela de navegação principal no canto inferior direito botões de funcionalidades do jogo.");

        playButton.Select();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1) && !helpMenu.activeSelf && !optionMenu.activeSelf)
        {
            ReadText(ReadableTexts.instance.GetReadableText(ReadableTexts.key_prejogo_menu, LocalizationManager.instance.GetLozalization()));
            //ReadText("Sob o fundo da tela de navegação principal no canto inferior direito botões de funcionalidades do jogo.");
        }

        //if (Parameters.HIGH_CONTRAST) HighContrastText.ChangeTextBackgroundColor();
        //else HighContrastText.RestoreToDefault("average");
    }

    public void TryQuitGame()
    {
        confirmQuit.SetActive(true);

        ReadText(ReadableTexts.instance.GetReadableText(ReadableTexts.key_prejogo_aviso, LocalizationManager.instance.GetLozalization()));

        ReadText("Tem certeza que deseja sair do jogo?");

        audioSource.PlayOneShot(warningClip);

        confirmQuit.GetComponentInChildren<Button>().Select();
    }

    public void QuitGame()
    {
        ReadText("Saindo do jogo.");
        Debug.Log("Quit");

        TolkUtil.Unload();
        confirmQuit.SetActive(false);

        Parameters.BOLD = false;

        Application.Quit();
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

        ReadText(ReadableTexts.instance.GetReadableText(ReadableTexts.key_prejogo_menu_carregamento, LocalizationManager.instance.GetLozalization()));

        PlayerPrefs.SetFloat("HealthPoints", HP);
        PlayerPrefs.SetFloat("Experience", EXP);
    }

    public IEnumerator LoadingScreen()
    {
        //async = SceneManager.LoadSceneAsync(ScenesNames.M002Ushuaia);
        async = SceneManager.LoadSceneAsync(ScenesNames.M009Camp);
        //async = SceneManager.LoadSceneAsync(ScenesNames.M004Ship);

        loadScreenObject.SetActive(true);

        //ReadText("O jogo está carregando...");

        while (!async.isDone)
        {
            if (!audioSource.isPlaying) audioSource.PlayOneShot(loadingClip);
            float progress = Mathf.Clamp01(async.progress / .9f);

            loadingSlider.value = progress;

            Debug.Log(progress);

            loadingText.text = (progress * 100f).ToString("F0") + "%";

            yield return null;
        }
        audioSource.Stop();
    }
}

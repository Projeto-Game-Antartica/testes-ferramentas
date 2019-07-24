using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : AbstractScreenReader
{
    public Slider slider;
    private Button playButton;

    private ReadableTexts readableTexts;

    public GameObject loadScreenObject;
    public Slider loadingSlider;

    AsyncOperation async;

    void Start()
    {
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

    public void QuitGame()
    {
        TolkUtil.Unload();
        Debug.Log("Quit");
        Application.Quit();
    }

    public void LoadGlossary()
    {
        SceneManager.LoadScene("GlossaryScene");
    }

    public void LoadSoundGlossary()
    {
        SceneManager.LoadScene("GlossarySoundScene");
    }

    public void LoadAjudaGlossarioScene()
    {
        SceneManager.LoadScene("AjudaGlossariosScene");
    }

    public void LoadGame()
    {
        StartCoroutine(LoadingScreen());
    }

    public IEnumerator LoadingScreen()
    {
        loadScreenObject.SetActive(true);
        async = SceneManager.LoadSceneAsync("ShipScene");
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
}

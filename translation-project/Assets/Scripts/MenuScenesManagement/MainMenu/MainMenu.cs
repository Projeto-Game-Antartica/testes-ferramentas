using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DavyKager;
using TMPro;
using UnityEngine.Audio;

public class MainMenu : AbstractScreenReader
{
    public Slider slider;
    private Button playButton;

    private ReadableTexts readableTexts;

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

    public void LoadAntarticaScene()
    {
        SceneManager.LoadScene("ShipScene");
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
}

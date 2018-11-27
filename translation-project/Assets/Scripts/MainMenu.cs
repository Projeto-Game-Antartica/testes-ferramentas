using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using DavyKager;
using TMPro;

public class MainMenu : MonoBehaviour
{
    private Button playButton;
    private Button quitButton;
    private Button optionButton;

    void Start()
    {
        playButton = GameObject.Find("PlayButton").GetComponent<Button>();
        quitButton = GameObject.Find("QuitButton").GetComponent<Button>();
        optionButton = GameObject.Find("OptionButton").GetComponent<Button>();

        TolkUtil.Load();
        Debug.Log("Tolk loaded");

        playButton.Select();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("QuizScene");
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        TolkUtil.Unload();
        Application.Quit();
    }

    public void ButtonPlayAudio()
    {
        Debug.Log("PlayButton");
        if (!Tolk.IsSpeaking()) Tolk.Speak(playButton.GetComponentInChildren<TextMeshProUGUI>().text);
    }

    public void ButtonOptionAudio()
    {
        Debug.Log("OptionButton");
        if (!Tolk.IsSpeaking()) Tolk.Speak(optionButton.GetComponentInChildren<TextMeshProUGUI>().text);
    }

    public void ButtonQuitAudio()
    {
        Debug.Log("QuitButton");
        if (!Tolk.IsSpeaking()) Tolk.Speak(quitButton.GetComponentInChildren<TextMeshProUGUI>().text);
    }
}

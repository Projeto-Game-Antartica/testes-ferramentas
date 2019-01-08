using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DavyKager;
using TMPro;

public class MainMenu : MonoBehaviour
{
    private Button playButton;
    private Button glossaryButton;
    private Button quitButton;
    private Button optionButton;

    private const string menuText = "Menu principal do jogo. Utilize as setas cima ou baixo ou a tecla TAB para navegação" +
                                    "a tecla enter para selecionar os itens.";

    void Start()
    {
        playButton = GameObject.Find("PlayButton").GetComponent<Button>();
        glossaryButton = GameObject.Find("GlossaryButton").GetComponent<Button>();
        quitButton = GameObject.Find("QuitButton").GetComponent<Button>();
        optionButton = GameObject.Find("OptionButton").GetComponent<Button>();

        //TolkUtil.Load();

        TolkUtil.Instructions();
        TolkUtil.Speak(menuText);

        playButton.Select();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("QuizScene");
    }
    public void QuitGame()
    {
        //Debug.Log("Quit");
        TolkUtil.Unload();
        Application.Quit();
    }

    public void ButtonPlayAudio()
    {
        //Debug.Log("PlayButton");
        TolkUtil.Speak(playButton.GetComponentInChildren<TextMeshProUGUI>().text);
    }

    public void ButtonGlossaryAudio()
    {
        //Debug.Log("GlossaryButton");
        TolkUtil.Speak(glossaryButton.GetComponentInChildren<TextMeshProUGUI>().text);
    }

    public void ButtonOptionAudio()
    {
        //Debug.Log("OptionButton");
        TolkUtil.Speak(optionButton.GetComponentInChildren<TextMeshProUGUI>().text);
    }

    public void ButtonQuitAudio()
    {
        //Debug.Log("QuitButton");
        TolkUtil.Speak(quitButton.GetComponentInChildren<TextMeshProUGUI>().text);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1))
        {
            TolkUtil.Speak(menuText);
        }
    }
}

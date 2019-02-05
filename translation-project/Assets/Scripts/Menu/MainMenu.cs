using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DavyKager;
using TMPro;

public class MainMenu : AbstractMenu
{
    private Button playButton;

    private const string instructions = "Menu principal do jogo. Utilize as setas cima ou baixo ou a tecla TAB para navegação" +
                                        "a tecla enter para selecionar os itens.";

    void Start()
    {
        playButton = GameObject.Find("PlayButton").GetComponent<Button>();

        //TolkUtil.Load();

        TolkUtil.Instructions();
        TolkUtil.Speak(instructions);

        playButton.Select();
    }

    public void QuitGame()
    {
        //Debug.Log("Quit");
        TolkUtil.Unload();
        Application.Quit();
    }

    public void LoadGlossary()
    {
        SceneManager.LoadScene("GlossaryScene");
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1))
        {
            TolkUtil.Speak(instructions);
        }
    }
}

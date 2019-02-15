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

    void Start()
    {
        playButton = GameObject.Find("PlayButton").GetComponent<Button>();

        //TolkUtil.Load();

        TolkUtil.Instructions();
        ReadableTexts.ReadText(ReadableTexts.mainmenu_instructions);

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

    public void LoadAntarticaScene()
    {
        SceneManager.LoadScene("ShipScene");
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1))
        {
            ReadableTexts.ReadText(ReadableTexts.mainmenu_instructions);
        }
    }
}

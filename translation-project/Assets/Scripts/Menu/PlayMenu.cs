using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayMenu : AbstractMenu {

    private Button minigame2;

    private ReadableTexts readableTexts;

    void Start()
    {
        readableTexts = GameObject.Find("ReadableTexts").GetComponent<ReadableTexts>();
        minigame2 = GameObject.Find("Minigame2").GetComponent<Button>();

        TolkUtil.Instructions();
        ReadableTexts.ReadText(readableTexts.GetReadableText(ReadableTexts.key_playmenu_instructions, LocalizationManager.instance.GetLozalization()));

        minigame2.Select();
    }

    public void QuitGame()
    {
        TolkUtil.Unload();
        Application.Quit();
    }

    public void LoadMinigame1()
    {
        SceneManager.LoadScene("SlidingBlockScene");
    }

    public void LoadMinigame2()
    {
        SceneManager.LoadScene("TailMissionScene");
    }

    public void LoadMinigame3()
    {
        SceneManager.LoadScene("QuizScene");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            ReadableTexts.ReadText(readableTexts.GetReadableText(ReadableTexts.key_playmenu_instructions, LocalizationManager.instance.GetLozalization()));
        }
    }
}

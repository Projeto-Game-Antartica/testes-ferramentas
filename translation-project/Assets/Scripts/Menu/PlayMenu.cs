using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayMenu : AbstractMenu {

    private Button minigame1;

    private const string instructions = "Escolhas de minigames do jogo. Utilize as setas cima ou baixo ou a tecla TAB para navegação" +
                                        "a tecla enter para selecionar os itens.";

    void Start()
    {
        minigame1 = GameObject.Find("Minigame1").GetComponent<Button>();

        TolkUtil.Instructions();
        TolkUtil.Speak(instructions);

        minigame1.Select();
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
            TolkUtil.Speak(instructions);
        }
    }
}

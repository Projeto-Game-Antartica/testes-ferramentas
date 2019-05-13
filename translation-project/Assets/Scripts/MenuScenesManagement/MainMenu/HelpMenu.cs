using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HelpMenu : AbstractScreenReader {

    public UnityEngine.UI.Button firstButton;

    public void Awake()
    {
        TolkUtil.Load();
        firstButton.Select();
    }

    public void ManualAluno()
    {
        // TO DO
    }

    public void ManualProfessor()
    {
        // TO DO
    }

    public void GlossarioLibras()
    {
        // TO DO
    }

    public void GlossarioSons()
    {
        SceneManager.LoadScene("GlossarySoundScene");
    }

    public void PrivacidadeTermos()
    {
        // TO DO
    }

    public void Voltar()
    {
        SceneManager.LoadScene("MenuScene");
    }
}

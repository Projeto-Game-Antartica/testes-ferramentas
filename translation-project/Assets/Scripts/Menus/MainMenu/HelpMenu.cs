using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HelpMenu : AbstractScreenReader {

    public UnityEngine.UI.Button firstButton;

    private void Start()
    {
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
        ReadText("Carregando glossário de libras");
        SceneManager.LoadScene(ScenesNames.Glossary);
    }

    public void GlossarioSons()
    {
        ReadText("Carregando áudios do jogo");
        SceneManager.LoadScene(ScenesNames.GlossarySound);
    }

    public void PrivacidadeTermos()
    {
        // TO DO
    }

    public void Voltar()
    {
        SceneManager.LoadScene(ScenesNames.Menu);
    }
}

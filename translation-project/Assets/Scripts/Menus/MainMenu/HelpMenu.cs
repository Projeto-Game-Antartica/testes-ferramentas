using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HelpMenu : AbstractScreenReader {

    public UnityEngine.UI.Button firstButton;

    public GameObject optionMenu;

    private void Start()
    {
        firstButton.Select();

        ReadText(ReadableTexts.instance.GetReadableText(ReadableTexts.key_prejogo_ajuda, LocalizationManager.instance.GetLozalization()));
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1) && !optionMenu.activeSelf)
        {
            ReadText(ReadableTexts.instance.GetReadableText(ReadableTexts.key_prejogo_ajuda, LocalizationManager.instance.GetLozalization()));
        }
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

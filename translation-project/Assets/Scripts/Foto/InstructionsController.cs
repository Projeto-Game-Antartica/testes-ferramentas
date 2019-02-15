using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InstructionsController : MonoBehaviour {
    
    public Text ui_title;
    public Text ui_instructions;
    public Button first_button;
    
    public void Start()
    {
        ui_instructions.text = ReadableTexts.foto_instructions;
        ReadableTexts.ReadText(ui_title.text);
        ReadInstructions();
        first_button.Select();
    }

    public void ReadButtonText(Text button_text)
    {
        ReadableTexts.ReadText(button_text.text);
    }

    public void ReadInstructions()
    {
        ReadableTexts.ReadText(ReadableTexts.foto_instructions);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MenuScene"); 
    }

    public void ReturnToAntarticaScene()
    {
        SceneManager.LoadScene("ShipScene");
    }

    public void PlayGame()
    {
        gameObject.SetActive(false);
        ReadableTexts.ReadText("Início do jogo. Pressione F3 para repetir a descrição do cenário.");
        ReadableTexts.ReadText(ReadableTexts.foto_sceneDescription);
    }
}

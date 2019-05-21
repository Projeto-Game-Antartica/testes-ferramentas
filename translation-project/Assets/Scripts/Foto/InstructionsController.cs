using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class InstructionsController : AbstractScreenReader {
    
    public TextMeshProUGUI ui_title;
    public TextMeshProUGUI ui_instructions;
    public Button first_button;

    private ReadableTexts readableTexts;
    
    public void Start()
    {
        //Parameters.HIGH_CONTRAST = true;
        readableTexts = GameObject.Find("ReadableTexts").GetComponent<ReadableTexts>();
        ui_instructions.text = readableTexts.GetReadableText(ReadableTexts.key_foto_instructions, LocalizationManager.instance.GetLozalization());
        ReadText(ui_title.text);
        ReadInstructions();
        first_button.Select();
    }

    public void ReadButtonText(Text button_text)
    {
        ReadText(button_text.text);
    }

    public void ReadInstructions()
    {
        ReadText(readableTexts.GetReadableText(ReadableTexts.key_foto_instructions, LocalizationManager.instance.GetLozalization()));
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
        ReadText("Início do jogo. Pressione F3 para repetir a descrição do cenário.");
        ReadText(readableTexts.GetReadableText(ReadableTexts.key_foto_sceneDescription, LocalizationManager.instance.GetLozalization()));
    }
}

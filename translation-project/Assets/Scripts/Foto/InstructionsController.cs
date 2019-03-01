using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InstructionsController : MonoBehaviour {
    
    public Text ui_title;
    public Text ui_instructions;
    public Button first_button;

    private ReadableTexts readableTexts;
    
    public void Start()
    {
        readableTexts = GameObject.Find("ReadableTexts").GetComponent<ReadableTexts>();
        ui_instructions.text = readableTexts.GetReadableText(ReadableTexts.key_foto_instructions, LocalizationManager.instance.GetLozalization());
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
        ReadableTexts.ReadText(readableTexts.GetReadableText(ReadableTexts.key_foto_instructions, LocalizationManager.instance.GetLozalization()));
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
        ReadableTexts.ReadText(readableTexts.GetReadableText(ReadableTexts.key_foto_sceneDescription, LocalizationManager.instance.GetLozalization()));
    }
}

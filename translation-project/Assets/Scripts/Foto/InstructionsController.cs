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
        TolkUtil.Speak(ui_title.text);
        ReadInstructions();
        first_button.Select();
    }

    public void ReadButtonText(Text button_text)
    {
        TolkUtil.Speak(button_text.text);
    }

    public void ReadInstructions()
    {
        TolkUtil.Speak(ReadableTexts.foto_instructions);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MenuScene"); 
    }

    public void PlayGame()
    {
        gameObject.SetActive(false);
        TolkUtil.Speak("Início do jogo. Pressione F3 para repetir a descrição do cenário.");
    }
}

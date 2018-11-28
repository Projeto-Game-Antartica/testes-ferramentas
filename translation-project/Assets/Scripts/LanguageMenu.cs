using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageMenu : MonoBehaviour {

    public Button brButton;
    public Button enButton;

    private void Start()
    {
        TolkUtil.Load();

        TolkUtil.Speak("Selecione o idioma do jogo. Há dois botões, o primeiro com a bandeira do Brasil" +
            "referindo-se ao idioma portugûes e o segundo uma bandeira do Reino Unido" +
           "referindo-se ao idioma inglês. Utilize as setas direcionais para navegar pelos botões." +
           "Utilize a tecla ENTER para selecioná-los");

        brButton.Select();
    }

    public void OnSelectBRButton()
    {
        TolkUtil.SpeakAnyway(brButton.GetComponentInChildren<Text>().text);
    }

    public void OnSelectENButton()
    {
        TolkUtil.SpeakAnyway(enButton.GetComponentInChildren<Text>().text);
    }
}

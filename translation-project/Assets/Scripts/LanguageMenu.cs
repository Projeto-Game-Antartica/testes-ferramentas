using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageMenu : MonoBehaviour {

    private Button brButton;
    private Button enButton;

    private const string initialText = "Selecione o idioma do jogo. Há dois botões, o primeiro com a bandeira do Brasil" +
                                       "referindo-se ao idioma portugûes e o segundo uma bandeira do Reino Unido" +
                                       "referindo-se ao idioma inglês. Utilize as setas para cima ou baixo ou a tecla TAB" +
                                       "para navegar pelos botões. Utilize a tecla ENTER para selecioná-los.";

    private void Start()
    {

        brButton = GameObject.Find("locales-ptbr").GetComponent<Button>();
        enButton = GameObject.Find("locales-en").GetComponent<Button>();

        TolkUtil.Load();

        TolkUtil.Instructions();
        TolkUtil.Speak(initialText);

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

    public void Update()
    {
        if (Input.GetKey(KeyCode.F1))
        {
            TolkUtil.Speak(initialText);
        }
    }
}

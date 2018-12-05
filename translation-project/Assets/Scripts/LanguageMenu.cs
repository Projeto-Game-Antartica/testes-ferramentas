using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LanguageMenu : MonoBehaviour {

    public Button brButton;
    public Button enButton;
    private EventSystem system;

    private const string initialText = "Selecione o idioma do jogo. Há dois botões, o primeiro com a bandeira do Brasil" +
                                       "referindo-se ao idioma portugûes e o segundo uma bandeira do Reino Unido" +
                                       "referindo-se ao idioma inglês. Utilize as setas para cima ou baixo ou a tecla TAB" +
                                       "para navegar pelos botões. Utilize a tecla ENTER para selecioná-los.";

    private void Start()
    {
        system = EventSystem.current;

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

        // Navegação dos itens selecionáveis através do TAB
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Selectable next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();

            if (next != null)
            {

                InputField inputfield = next.GetComponent<InputField>();
                if (inputfield != null) inputfield.OnPointerClick(new PointerEventData(system));  //if it's an input field, also set the text caret

                system.SetSelectedGameObject(next.gameObject, new BaseEventData(system));
            }
            else //Here is the navigating back part
            {
                next = Selectable.allSelectables[0];
                system.SetSelectedGameObject(next.gameObject, new BaseEventData(system));
            }

        }
    }
}

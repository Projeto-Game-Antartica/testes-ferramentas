using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DavyKager;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class OptionMenu : MonoBehaviour {

    public Slider slider;
    public TextMeshProUGUI volumeText;
    public Button backButton;
    public GameObject optionMenu;
    private EventSystem system;

    private const string optionText = "Menu de opções do jogo. Utilize as setas cima ou baixo ou a tecla TAB para navegação" +
                                      "as teclas direita ou esquerda para mudança de opções" +
                                      "a tecla enter para selecionar os itens.";

    void Start()
    {
        slider = GameObject.Find("SliderVolume").GetComponent<Slider>();
        volumeText = GameObject.Find("Volume").GetComponent<TextMeshProUGUI>();
        backButton = GameObject.Find("BackButton").GetComponent<Button>();
        optionMenu = GameObject.Find("OptionMenu");
        system = EventSystem.current;

        TolkUtil.Instructions();
        TolkUtil.Speak(optionText);

        TolkUtil.Load();

        slider.Select();
    }

    public void ButtonBackAudio()
    {
        Debug.Log("BackButton");
        TolkUtil.Speak(backButton.GetComponentInChildren<TextMeshProUGUI>().text);
    }

    public void SliderVolumeAudio()
    {
        Debug.Log("VolumeSlider");
        TolkUtil.Speak(volumeText.text + slider.value);
    }

    public void TextVolumeAudio()
    {
        Debug.Log("TextVolume");
        TolkUtil.Speak(volumeText.text);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            TolkUtil.Speak(optionText);
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

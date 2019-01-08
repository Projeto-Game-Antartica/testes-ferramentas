using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DavyKager;
using UnityEngine.UI;
using TMPro;

public class OptionMenu : MonoBehaviour {

    private Slider slider;
    private TextMeshProUGUI volumeText;
    private Button backButton;
    private GameObject optionMenu;

    private const string optionText = "Menu de opções do jogo. Utilize as setas cima ou baixo ou a tecla TAB para navegação" +
                                      "as teclas direita ou esquerda para mudança de opções" +
                                      "a tecla enter para selecionar os itens.";

    void Start()
    {
        slider = GameObject.Find("SliderVolume").GetComponent<Slider>();
        volumeText = GameObject.Find("Volume").GetComponent<TextMeshProUGUI>();
        backButton = GameObject.Find("BackButton").GetComponent<Button>();
        optionMenu = GameObject.Find("OptionMenu");

        TolkUtil.Instructions();
        TolkUtil.Speak(optionText);

        //TolkUtil.Load();

        slider.Select();
    }

    public void ButtonBackAudio()
    {
        //Debug.Log("BackButton");
        TolkUtil.Speak(backButton.GetComponentInChildren<TextMeshProUGUI>().text);
    }

    public void SliderVolumeAudio()
    {
        //Debug.Log("VolumeSlider");
        TolkUtil.Speak(volumeText.text + slider.value);
    }

    public void TextVolumeAudio()
    {
        //Debug.Log("TextVolume");
        TolkUtil.Speak(volumeText.text);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            TolkUtil.Speak(optionText);
        }
    }
}

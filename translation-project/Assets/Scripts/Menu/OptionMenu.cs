using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DavyKager;
using UnityEngine.UI;
using TMPro;

public class OptionMenu : AbstractMenu {

    private Slider slider;

    private const string instructions = "Menu de opções do jogo. Utilize as setas cima ou baixo ou a tecla TAB para navegação" +
                                        "as teclas direita ou esquerda para mudança de opções a tecla enter para selecionar os itens.";

    void Start()
    {
        slider = GameObject.Find("SliderVolume").GetComponent<Slider>();

        TolkUtil.Instructions();
        TolkUtil.Speak(instructions);

        //TolkUtil.Load();

        slider.Select();
    }

    public void ReadSliderTextMeshPro(Slider slider)
    {
        TolkUtil.Speak("volume" + slider.value);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            TolkUtil.Speak(instructions);
        }
    }
}

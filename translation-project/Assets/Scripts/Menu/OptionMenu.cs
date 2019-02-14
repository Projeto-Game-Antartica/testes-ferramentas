using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DavyKager;
using UnityEngine.UI;
using TMPro;

public class OptionMenu : AbstractMenu {

    private Slider slider;

    void Start()
    {
        slider = GameObject.Find("SliderVolume").GetComponent<Slider>();

        TolkUtil.Instructions();
        TolkUtil.Speak(ReadableTexts.optionmenu_instructions);

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
            TolkUtil.Speak(ReadableTexts.optionmenu_instructions);
        }
    }
}

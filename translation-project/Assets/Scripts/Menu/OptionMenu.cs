using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DavyKager;
using UnityEngine.UI;
using TMPro;

public class OptionMenu : AbstractMenu {

    public Slider slider;
    public Toggle toggle;

    void Start()
    {
        TolkUtil.Instructions();
        ReadableTexts.ReadText(ReadableTexts.optionmenu_instructions);

        //TolkUtil.Load();

        slider.Select();
    }

    public void ReadSliderTextMeshPro(Slider slider)
    {
        ReadableTexts.ReadText("volume" + slider.value);
    }

    public void ReadToggle(Toggle toggle)
    {
        ReadableTexts.ReadText(toggle.name + "" + toggle.isOn);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            ReadableTexts.ReadText(ReadableTexts.optionmenu_instructions);
        }
    }

    public void SetAcessibilityParameter()
    {
        Parameters.ACCESSIBILITY = toggle.isOn;
        Debug.Log(Parameters.ACCESSIBILITY);
    }
}

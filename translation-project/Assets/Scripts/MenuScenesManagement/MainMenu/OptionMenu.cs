using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DavyKager;
using UnityEngine.UI;
using TMPro;

public class OptionMenu : AbstractMenu {

    public Slider slider;
    public Toggle toggle;
    public LocalizationManager localization;

    private ReadableTexts readableTexts;

    void Start()
    {
        readableTexts = GameObject.Find("ReadableTexts").GetComponent<ReadableTexts>();
        TolkUtil.Instructions();
        ReadableTexts.ReadText(readableTexts.GetReadableText(ReadableTexts.key_optionmenu_instructions, LocalizationManager.instance.GetLozalization()));
        
        //TolkUtil.Load();

        slider.Select();
        toggle.isOn = Parameters.ACCESSIBILITY;
    }

    public void DropDownHandler(int index)
    {
        //if (index == 0) // ptbr
        //{
        //    localization.LoadLocalizedText("locales_ptbr.json");
        //}
        //else
        //{
        //    localization.LoadLocalizedText("locales_en.json");
        //}

        //while (!localization.GetIsReady()) ;
    }

    public void SliderSettings(Slider slider)
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
            ReadableTexts.ReadText(readableTexts.GetReadableText(ReadableTexts.key_optionmenu_instructions, LocalizationManager.instance.GetLozalization()));
        }
    }

    public void SetAcessibilityParameter()
    {
        Parameters.ACCESSIBILITY = toggle.isOn;
        Debug.Log(Parameters.ACCESSIBILITY);
    }
}

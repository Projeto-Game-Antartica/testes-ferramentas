using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DavyKager;
using UnityEngine.UI;
using TMPro;

public class OptionMenu : AbstractScreenReader {

    public Slider slider;
    public Toggle ScreenReaderToggle;
    public Toggle HighContrastToggle;
    public HighContrastSettings hcsettings;

    private ReadableTexts readableTexts;

    void Start()
    {
        readableTexts = GameObject.FindGameObjectWithTag("Accessibility").GetComponent<ReadableTexts>();
        //TolkUtil.Instructions();

        ReadText(readableTexts.GetReadableText(ReadableTexts.key_optionmenu_instructions, LocalizationManager.instance.GetLozalization()));
        
        //TolkUtil.Load();

        slider.Select();
        ScreenReaderToggle.isOn = Parameters.ACCESSIBILITY;
        HighContrastToggle.isOn = Parameters.HIGH_CONTRAST;

        Debug.Log(Parameters.HIGH_CONTRAST);
    }

    private void OnEnable()
    {
        //GameObject[] newTexts = GameObject.FindGameObjectsWithTag("text-hc");
        //fontSizeText.SetNewTexts(newTexts);

        //HighContrastToggle.isOn = Parameters.HIGH_CONTRAST;

        //hcsettings.ChangeHighContrast();
    }
    
    public void ReadToggle(Toggle toggle)
    {
        var tmp = "";

        if (toggle.isOn)
            tmp = "ativado";
        else
            tmp = "desativado";

        ReadText(toggle.name + "" + tmp);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            ReadText(readableTexts.GetReadableText(ReadableTexts.key_optionmenu_instructions, LocalizationManager.instance.GetLozalization()));
        }
    }

    public void SetAcessibilityParameter()
    {
        Parameters.ACCESSIBILITY = ScreenReaderToggle.isOn;
        //Debug.Log("A: " + Parameters.ACCESSIBILITY);

        if (Parameters.ACCESSIBILITY)
        {
            TolkUtil.Load();
            ReadText("Leitura de tela habilitada;");
        }
        else
        {
            TolkUtil.Unload();
            ReadText("Leitura de tela desabilitada;");
        }
    }

    public void SetHighContrastParameter()
    {
        hcsettings.SetHighAccessibility();

        if (Parameters.HIGH_CONTRAST)
            ReadText("Alto contraste ativado");
        else
            ReadText("Alto contraste desativado");
    }
}

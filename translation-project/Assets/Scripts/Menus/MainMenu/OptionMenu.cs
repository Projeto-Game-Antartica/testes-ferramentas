using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DavyKager;
using UnityEngine.UI;
using TMPro;

public class OptionMenu : AbstractScreenReader {
    
    public GameObject helpMenu;

    public Slider slider;
    public Toggle ScreenReaderToggle;
    public Toggle HighContrastToggle;
    public HighContrastSettings hcsettings;

    void Start()
    {
        //TolkUtil.Instructions();

        ReadText(ReadableTexts.instance.GetReadableText(ReadableTexts.key_prejogo_config, LocalizationManager.instance.GetLozalization()));
        
        //TolkUtil.Load();

        slider.Select();
        
        ScreenReaderToggle.isOn = Parameters.ACCESSIBILITY;

        Debug.Log("HC - B: " + Parameters.HIGH_CONTRAST);

        HighContrastToggle.isOn = Parameters.HIGH_CONTRAST;

        Debug.Log("HC: " + Parameters.HIGH_CONTRAST);
    }

    private void OnEnable()
    {
        //GameObject[] newTexts = GameObject.FindGameObjectsWithTag("text-hc");
        //fontSizeText.SetNewTexts(newTexts);

        //HighContrastToggle.isOn = Parameters.HIGH_CONTRAST;

        //hcsettings.ChangeHighContrast();
    }
    
    new public void ReadToggle(Toggle toggle)
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
        if (Input.GetKeyDown(InputKeys.AUDIODESCRICAO_KEY) && !helpMenu.activeSelf)
        {
            ReadText(ReadableTexts.instance.GetReadableText(ReadableTexts.key_prejogo_config, LocalizationManager.instance.GetLozalization()));
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

    public void SetHighContrastParameter(bool isOn)
    {
        hcsettings.SetHighAccessibility(isOn);

        if (Parameters.HIGH_CONTRAST)
            ReadText("Alto contraste ativado");
        else
            ReadText("Alto contraste desativado");
    }
}

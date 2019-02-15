using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageMenu : AbstractMenu {

    private Button brButton;

    private void Start()
    {

        brButton = GameObject.Find("locales-ptbr").GetComponent<Button>();

        // accessibility functions active
        Parameters.ACCESSIBILITY = true;

        TolkUtil.Load();

        TolkUtil.Instructions();
        ReadableTexts.ReadText(ReadableTexts.languagemenu_instructions);

        brButton.Select();  
    }

    public void Update()
    {
        if (Input.GetKey(KeyCode.F1))
        {
            ReadableTexts.ReadText(ReadableTexts.languagemenu_instructions);
        }
    }
}

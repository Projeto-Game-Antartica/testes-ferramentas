using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageScene : AbstractScreenReader {

    private Button brButton;

    private ReadableTexts readableTexts;

    private void Start()
    {
        readableTexts = GameObject.Find("ReadableTexts").GetComponent<ReadableTexts>();
        brButton = GameObject.Find("locales-ptbr").GetComponent<Button>();

        // accessibility and high contrast functions inactive
        Parameters.ACCESSIBILITY = false;
        Parameters.HIGH_CONTRAST = false;

        TolkUtil.Load();

        TolkUtil.Instructions();
        ReadText(readableTexts.GetReadableText(ReadableTexts.key_languagemenu_instructions, "locales_ptbr.json"));

        brButton.Select();  
    }

    public void Update()
    {
        if (Input.GetKey(KeyCode.F1))
        {
            ReadText(readableTexts.GetReadableText(ReadableTexts.key_languagemenu_instructions, "locales_ptbr.json"));
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}

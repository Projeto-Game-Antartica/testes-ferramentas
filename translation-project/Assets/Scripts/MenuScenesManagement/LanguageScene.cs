using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageScene : AbstractMenu {

    private Button brButton;

    private ReadableTexts readableTexts;

    private void Start()
    {
        readableTexts = GameObject.Find("ReadableTexts").GetComponent<ReadableTexts>();
        brButton = GameObject.Find("locales-ptbr").GetComponent<Button>();

        // accessibility functions inactive
        Parameters.ACCESSIBILITY = false;

        TolkUtil.Load();

        TolkUtil.Instructions();
        ReadableTexts.ReadText(readableTexts.GetReadableText(ReadableTexts.key_languagemenu_instructions, "locales_ptbr.json"));

        brButton.Select();  
    }

    public void Update()
    {
        if (Input.GetKey(KeyCode.F1))
        {
            ReadableTexts.ReadText(readableTexts.GetReadableText(ReadableTexts.key_languagemenu_instructions, "locales_ptbr.json"));
        }
    }
}

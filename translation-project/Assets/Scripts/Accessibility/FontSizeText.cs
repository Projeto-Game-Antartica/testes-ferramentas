using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class FontSizeText : AbstractScreenReader
{
    public void SetTextBold(bool isOn)
    {
        Parameters.BOLD = isOn;
    }
    
    /*
    public static TextMeshProUGUI[] texts;
    //public RectTransform[] rectTransforms;

    private readonly int maxSize = 36;
    private readonly int minSize = 16;

    void Start()
    {
        //texts = FindObjectsOfType<TextMeshProUGUI>(); too expensive
        FindTexts();

        if (Parameters.BOLD)
            SetToBold();
    }

    public void FindTexts()
    {
        //texts = GameObject.FindGameObjectsWithTag("text-hc");
        texts = Resources.FindObjectsOfTypeAll(typeof(TextMeshProUGUI)) as TextMeshProUGUI[];
    }

    public void IncreaseFontSize()
    {
        foreach (TextMeshProUGUI text in texts)
        {
            text.fontSize += 1;
            //text.GetComponentInChildren<TextMeshProUGUI>().fontStyle = FontStyles.Bold;
            //text.rectTransform.sizeDelta = new Vector2(text.fontSize*2f,text.fontSize*2f);
            //if (text.GetComponentInChildren<TextMeshProUGUI>().fontSize >= maxSize) text.GetComponentInChildren<TextMeshProUGUI>().fontSize = maxSize;
            if (text.fontSize >= maxSize) text.fontSize = maxSize;
        }
    }

    public void DecreseFontSize()
    {
        foreach (TextMeshProUGUI text in texts)
        {
            text.fontSize -= 1;
            //text.rectTransform.sizeDelta = new Vector2(text.fontSize * 1, text.fontSize *1);
            if (text.fontSize <= minSize) text.fontSize = minSize;
        }
    }

    public void SetToBold()
    {
        foreach (TextMeshProUGUI text in texts)
        {
            var tmp = text.text;

            if (tmp.Contains("<b>"))
            {
                Parameters.BOLD = false;
                tmp = tmp.Replace("<b>", "");
                tmp = tmp.Replace("</b>", "");
                text.text = tmp;
            }
            else
            {
                Parameters.BOLD = true;
                text.text = "<b>" + tmp + "</b>";
            }
        }

        if (Parameters.BOLD)
            ReadText("Texto em negrito ativado");
        else
            ReadText("Texto em negrito desativado");
    }

    */
}

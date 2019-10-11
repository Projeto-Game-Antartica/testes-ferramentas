using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class FontSizeText : MonoBehaviour {

    public static GameObject[] texts;
    //public RectTransform[] rectTransforms;

    private readonly int maxSize = 36;
    private readonly int minSize = 16;

    void Start()
    {
        //texts = FindObjectsOfType<TextMeshProUGUI>(); too expensive
        texts = GameObject.FindGameObjectsWithTag("text-hc");
        //rectTransforms = FindObjectsOfType<RectTransform>();
    }

    public void IncreaseFontSize()
    {
        foreach (GameObject text in texts) {
            text.GetComponentInChildren<TextMeshProUGUI>().fontSize += 1;
            //text.GetComponentInChildren<TextMeshProUGUI>().fontStyle = FontStyles.Bold;
            //text.rectTransform.sizeDelta = new Vector2(text.fontSize*2f,text.fontSize*2f);
            if (text.GetComponentInChildren<TextMeshProUGUI>().fontSize >= maxSize) text.GetComponentInChildren<TextMeshProUGUI>().fontSize = maxSize;
        }
    }

    public void DecreseFontSize()
    {
        foreach (GameObject text in texts)
        {
            text.GetComponentInChildren<TextMeshProUGUI>().fontSize -= 1;
            //text.rectTransform.sizeDelta = new Vector2(text.fontSize * 1, text.fontSize *1);
            if (text.GetComponentInChildren<TextMeshProUGUI>().fontSize <= minSize) text.GetComponentInChildren<TextMeshProUGUI>().fontSize = minSize;
        }
    }

    public void SetToBold()
    {
        foreach (GameObject text in texts)
        {
            var tmp = text.GetComponentInChildren<TextMeshProUGUI>().text;

            if (tmp.Contains("<b>"))
            {
                tmp = tmp.Replace("<b>", "");
                tmp = tmp.Replace("</b>", "");
                text.GetComponentInChildren<TextMeshProUGUI>().text = tmp;
            }
            else
                text.GetComponentInChildren<TextMeshProUGUI>().text = "<b>" + tmp + "</b>";
        }
    }
}

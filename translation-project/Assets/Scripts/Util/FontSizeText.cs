using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class FontSizeText : MonoBehaviour {

    public TextMeshProUGUI[] texts;
    public RectTransform[] rectTransforms;

    void Start()
    {
        texts = Component.FindObjectsOfType<TextMeshProUGUI>();
        rectTransforms = Component.FindObjectsOfType<RectTransform>();
    }
    public void IncreaseFontSize()
    {
        foreach (TextMeshProUGUI text in texts) {
            text.fontSize += 1;
            //text.rectTransform.sizeDelta = new Vector2(text.fontSize*2f,text.fontSize*2f);
            if (text.fontSize >= 33) text.fontSize = 33;
        }
    }

    public void DecreseFontSize()
    {
        foreach (TextMeshProUGUI text in texts)
        {
            text.fontSize -= 1;
//text.rectTransform.sizeDelta = new Vector2(text.fontSize * 1, text.fontSize *1);
            if (text.fontSize <= 16) text.fontSize = 16;
        }
    }
}

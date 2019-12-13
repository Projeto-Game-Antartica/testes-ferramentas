using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HighContrastText : MonoBehaviour {

    /*
     * Hexadecimal colors:
     *  white  = #FFFFFF
     *  black  = #000000
     *  yellow = #FFFF00
     *  
     *  
     * Change the background color of text using TextMeshProUGUI
     * UI Text need to be TMPro
     * Attach this script to a HighContrastText prefab
     */
     
    private TMP_FontAsset arialFont;

    private TextMeshProUGUI text;
    private Color originalTextColor;
    private TMP_FontAsset originalFont;
    private Image bgImage;

    private void Start()
    {
        arialFont = Resources.Load<TMP_FontAsset>("Fonts/ARIAL SDF");

        text = GetComponentInChildren<TextMeshProUGUI>();
        originalTextColor = text.color;
        originalFont = text.font;
        bgImage = GetComponent<Image>();
        Debug.Log(bgImage.name);
    }

    private void Update()
    {
        ChangeHighContrast();
    }

    public void ChangeHighContrast()
    {
        if (Parameters.HIGH_CONTRAST)
        {
            var tempColor = bgImage.color;
            tempColor = Color.black;
            tempColor.a = 1f;
            bgImage.color = tempColor;

            //text.font = arialFont;

            text.color = Color.white;
        }
        else
        {
            var tempColor = bgImage.color;
            tempColor.a = 0f;
            bgImage.color = tempColor;

            text.font = originalFont;
            text.color = originalTextColor;
        }
    }
}

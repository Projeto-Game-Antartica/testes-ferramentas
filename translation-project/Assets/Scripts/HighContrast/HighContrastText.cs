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
     */

    public static TMP_FontAsset arialFont   = Resources.Load<TMP_FontAsset>("Fonts/ARIAL SDF");
    public static TMP_FontAsset bgothmFont  = Resources.Load<TMP_FontAsset>("Fonts/bgothm SDF");
    public static TMP_FontAsset swiss       = Resources.Load<TMP_FontAsset>("Fonts/Swiss 721 SDF");
    public static TMP_FontAsset averageFont       = Resources.Load<TMP_FontAsset>("Fonts/AverageSans SDF");

    /*
     * Change the background color of text using TextMeshProUGUI
     * UI Text need to be TMPro
     * Problem: restore to default
     */
    public static void ChangeAllTextColors(string color)
    {
        Object[] textComponents = Resources.FindObjectsOfTypeAll(typeof(TextMeshProUGUI));

        foreach(TextMeshProUGUI t in textComponents)
        {
            ChangeFontToArial(t);
        }
    }

    public static void ChangeFontToArial(TextMeshProUGUI text)
    {
        text.font = arialFont;
        text.fontSize = 24;
        text.fontStyle = FontStyles.Bold;
        //text.fontStyle = FontStyles.UpperCase;
        //Debug.Log(text.text);
        text.text = "<font=\"LiberationSans SDF\"><mark=#000000>" + text.text + "</mark>";
    }

    public static void ChangeFontToBgoth(TextMeshProUGUI text)
    {
        text.font = bgothmFont;
        text.fontStyle = FontStyles.Normal;
    }

    /*
     * Prefab: image with textmeshpro as children tagged with text-hc
     * Change the color of image
     * HighContrast: image black and text white with arial font upper case
     * Default: set alpha 0
     */

    public static void ChangeTextBackgroundColor()
    {
        GameObject[] imageComponents = GameObject.FindGameObjectsWithTag("text-hc");

        foreach(GameObject g in imageComponents)
        {
            Image image = g.GetComponent<Image>();
            TextMeshProUGUI text = g.GetComponentInChildren<TextMeshProUGUI>();

            var tempColor = image.color;
            tempColor = Color.black;
            tempColor.a = 1f;
            image.color = tempColor;

            text.font = arialFont;
            //text.fontStyle = FontStyles.UpperCase;
        }
    }

    public static void RestoreToDefault(string fontName)
    {
        GameObject[] imageComponents = GameObject.FindGameObjectsWithTag("text-hc");

        foreach (GameObject g in imageComponents)
        {
            Image image = g.GetComponent<Image>();
            TextMeshProUGUI text = g.GetComponentInChildren<TextMeshProUGUI>();

            var tempColor = image.color;
            tempColor.a = 0f;
            image.color = tempColor;

            switch(fontName)
            {
                case "bgothm":
                    text.font = bgothmFont;
                    break;
                case "swiss":
                    text.font = swiss;
                    break;
                case "average":
                    text.font = averageFont;
                    break;
                default:
                    text.font = arialFont;
                    break;
            }

            text.fontStyle = FontStyles.Normal;
        }
    }
}

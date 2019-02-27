using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractMenu : MonoBehaviour {

    public void ReadButtonTextMeshPro(UnityEngine.UI.Button button)
    {
        ReadableTexts.ReadText(button.GetComponentInChildren<TMPro.TextMeshProUGUI>().text);
    }

    public void ReadButtonText(UnityEngine.UI.Button button)
    {
        ReadableTexts.ReadText(button.GetComponentInChildren<UnityEngine.UI.Text>().text);
    }

    public void ReadText(UnityEngine.UI.Text text)
    {
        ReadableTexts.ReadText(text.text);
    }
}

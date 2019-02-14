using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractMenu : MonoBehaviour {

    public void ReadButtonTextMeshPro(UnityEngine.UI.Button button)
    {
        TolkUtil.Speak(button.GetComponentInChildren<TMPro.TextMeshProUGUI>().text);
    }

    public void ReadButtonText(UnityEngine.UI.Button button)
    {
        TolkUtil.Speak(button.GetComponentInChildren<UnityEngine.UI.Text>().text);
    }
}

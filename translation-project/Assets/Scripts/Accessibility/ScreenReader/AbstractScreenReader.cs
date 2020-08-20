using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// class to help with screen reader
public abstract class AbstractScreenReader : MonoBehaviour {

    // receive the UI TextMeshPro
    public void ReadButtonTextMeshPro(UnityEngine.UI.Button button)
    {
        ReadText(CheckBold(button.GetComponentInChildren<TMPro.TextMeshProUGUI>().text));
    }

    // receive the UI Button
    public void ReadButtonText(UnityEngine.UI.Button button)
    {
        ReadText(CheckBold(button.GetComponentInChildren<UnityEngine.UI.Text>().text));
    }

    // receive the UI Text
    public void ReadText(UnityEngine.UI.Text text)
    {
        ReadText(CheckBold(text.text));
    }

    // receive the UI TextMeshPro
    public void ReadText(TMPro.TextMeshProUGUI text)
    {
        ReadText(CheckBold(text.text));
    }

    // receive the UI Inputfield and reads his placeholder
    public void ReadInputfieldPlaceHolder(UnityEngine.UI.InputField inputField)
    {
        ReadText(CheckBold(inputField.placeholder.GetComponent<UnityEngine.UI.Text>().text));
    }

    // receive the UI InputField and reads his text
    public void ReadInputfieldText(UnityEngine.UI.InputField inputField)
    {
        ReadText(CheckBold(inputField.text));
    }

    // verify if the acessibility parameter is TRUE, is so read the text, if not do nothing.
    public void ReadText(string text)
    {
        if (Parameters.READ_TEXT_DEBUG_MODE) Debug.Log("READ_TEXT_DEBUG_MODE: " + text);
        if (Parameters.ACCESSIBILITY) TolkUtil.Speak(text);
    }

    // read toggle
    public void ReadToggle(UnityEngine.UI.Toggle toggle)
    {
        var tmp = "";

        if (toggle.isOn)
            tmp = "verdadeiro";
        else
            tmp = "falso";

        ReadText(toggle.name + "" + tmp);
    }

    // play clip when selecting a button
    public static void PlaySelectAudio(AudioSource audioSource, AudioClip audioClip)
    {
        if (Parameters.ACCESSIBILITY)
        {
            audioSource.volume = 0.3f;
            audioSource.PlayOneShot(audioClip);
        }
    }

    public string CheckBold(string text)
    {
        var tmp = text;

        if (text.Contains("<b>"))
        {
            tmp = tmp.Replace("<b>", "");
            tmp = tmp.Replace("</b>", "");
        }

        return tmp;
    }

    public void TolkUnload()
    {
        TolkUtil.Unload();
    }
}

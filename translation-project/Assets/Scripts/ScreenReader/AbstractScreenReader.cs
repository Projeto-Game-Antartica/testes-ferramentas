using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractScreenReader : MonoBehaviour {

    // receive the UI TextMeshPro
    public void ReadButtonTextMeshPro(UnityEngine.UI.Button button)
    {
        ReadText(button.GetComponentInChildren<TMPro.TextMeshProUGUI>().text);
    }

    // receive the UI Button
    public void ReadButtonText(UnityEngine.UI.Button button)
    {
        ReadText(button.GetComponentInChildren<UnityEngine.UI.Text>().text);
    }

    // receive the UI Text
    public void ReadText(UnityEngine.UI.Text text)
    {
        ReadText(text.text);
    }

    // receive the UI TextMeshPro
    public void ReadText(TMPro.TextMeshProUGUI text)
    {
        ReadText(text.text);
    }

    // receive the UI Inputfield and reads his placeholder
    public void ReadInputfieldPlaceHolder(UnityEngine.UI.InputField inputField)
    {
        ReadText(inputField.placeholder.GetComponent<UnityEngine.UI.Text>().text);
    }

    // receive the UI InputField and reads his text
    public void ReadInputfieldText(UnityEngine.UI.InputField inputField)
    {
        ReadText(inputField.text);
    }

    // verify if the acessibility parameter is TRUE, is so read the text, if not do nothing.
    public static void ReadText(string text)
    {
        if (Parameters.ACCESSIBILITY) TolkUtil.Speak(text);
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
}

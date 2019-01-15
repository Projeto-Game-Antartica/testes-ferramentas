using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DictionaryButton : MonoBehaviour
{
    public Button buttonComponent;
    public Text keyLabel;
    public DictionaryController dictionaryController;
    public static bool contentButton;

    // Use this for initialization
    void Start()
    {
        dictionaryController = buttonComponent.GetComponentInParent<DictionaryController>();
        buttonComponent.onClick.AddListener(HandleClick);
    }

    public void HandleClick()
    {
        dictionaryController.ShowDescriptionContent(keyLabel.text);
    }

    public void SetContentButton(bool content)
    {
        contentButton = content;
    }

    public void ReadButton()
    {
        TolkUtil.Speak(keyLabel.text);
    }
}

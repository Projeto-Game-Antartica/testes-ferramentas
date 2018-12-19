using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DictionaryButton : MonoBehaviour
{
    public Button buttonComponent;
    public Text keyLabel;
    public DictionaryController dictionaryController;

    // Use this for initialization
    void Start()
    {
        dictionaryController = buttonComponent.GetComponentInParent<DictionaryController>();
        buttonComponent.onClick.AddListener(HandleClick);
    }

    public void HandleClick()
    {
        Debug.Log(keyLabel.text);
        dictionaryController.ShowDescriptionContent(keyLabel.text);
    }
}

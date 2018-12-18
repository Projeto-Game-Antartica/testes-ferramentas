using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DictionaryButton : MonoBehaviour
{
    public Button buttonComponent;
    public Text keyLabel;

    // Use this for initialization
    void Start()
    {
        buttonComponent.onClick.AddListener(HandleClick);
    }

    public void HandleClick()
    {
            
    }
}

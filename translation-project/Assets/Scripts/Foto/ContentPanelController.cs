using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// not used
public class ContentPanelController : MonoBehaviour {

    public Button first_button;
    public GameObject panelCatalogo;

    private ReadableTexts readableTexts;

    private void Start()
    {
        readableTexts = GameObject.Find("ReadableTexts").GetComponent<ReadableTexts>();
        ReadInstructions();
        first_button.Select();
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            ReadInstructions();
        }
	}

    public void ReadInstructions()
    {
        ReadableTexts.ReadText(readableTexts.GetReadableText(ReadableTexts.key_foto_catalogDescription, LocalizationManager.instance.GetLozalization()));
    }

    public void Save()
    {
        this.gameObject.SetActive(false);
        panelCatalogo.SetActive(true);
    }
}

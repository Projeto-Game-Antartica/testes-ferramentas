using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MJInstructionInterfaceController : AbstractScreenReader {

    public Button iniciarButton;
    public Button voltarButton;
    public TextMeshProUGUI minijogoName;
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;

    public AudioSource audioSource;
    public AudioClip closeClip;

    public string audiodescriptionKey;
    
    // Use this for initialization
	void Start ()
    {
        iniciarButton.gameObject.SetActive(true);
        voltarButton.gameObject.SetActive(false);

        //This is only set in
        //This is set in case Location Manager has not yet been instantied to avoid crash 
        if(LocalizationManager.instance == null)
            (new LocalizationManager()).LoadLocalizedText("locales_ptbr.json");

        if(ReadableTexts.instance == null) {
            ReadableTexts.instance = new ReadableTexts();
            ReadableTexts.instance.LoadReadableTexts();
        }

        ReadText(ReadableTexts.instance.GetReadableText(audiodescriptionKey, LocalizationManager.instance.GetLozalization()));

        ReadInstructions();

        iniciarButton.Select();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(InputKeys.INSTRUCTIONS_KEY))
        {
            ReadInstructions();
        }

        if (Input.GetKeyDown(InputKeys.AUDIODESCRICAO_KEY))
        {
            ReadText(ReadableTexts.instance.GetReadableText(audiodescriptionKey, LocalizationManager.instance.GetLozalization()));
        }

        if(Input.GetKeyDown(KeyCode.Tab))
        {
            if (gameObject.activeSelf)
            {
                if (iniciarButton.gameObject.activeSelf)
                    iniciarButton.Select();
                else
                    voltarButton.Select();
            }
        }
    }

    //private void OnEnable()
    //{
    //    ReadInstructions();
    //}

    public void ReadInstructions()
    {
        ReadText(minijogoName.text);
        ReadText(title.text);
        ReadText(description.text);
    }

    public void FirstInstruction()
    {
        iniciarButton.gameObject.SetActive(false);
        voltarButton.gameObject.SetActive(true);
    }
}

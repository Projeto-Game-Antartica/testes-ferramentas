﻿using System.Collections;
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
    
    // Use this for initialization
	void Start ()
    {
        iniciarButton.gameObject.SetActive(true);
        voltarButton.gameObject.SetActive(false);

        ReadInstructions();

        iniciarButton.Select();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(InputKeys.INSTRUCTIONS_KEY))
        {
            ReadInstructions();
        }
	}

    public void ReadInstructions()
    {
        ReadText(minijogoName.text);
        ReadText(title.text);
        ReadText(description.text);
        Debug.Log(minijogoName.text);
        Debug.Log(title.text);
        Debug.Log(description.text);
    }

    public void FirstInstruction()
    {
        iniciarButton.gameObject.SetActive(false);
        voltarButton.gameObject.SetActive(true);
    }
}

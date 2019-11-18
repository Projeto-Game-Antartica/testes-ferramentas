﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CasaUshuaiaSceneManagement : AbstractScreenReader {

    private bool isTrigger;

    public SimpleCharacterController character;
    public ChasingCamera chasingCamera;

    private Vector2 positionSceneChange;

    public TextMeshProUGUI warningText;
    public GameObject warningInterface;

    private Collider2D colliderControl;

    // instruction settings
    public GameObject instructionInterface;
    public TextMeshProUGUI missionName;
    public TextMeshProUGUI descriptionText;
    
    public string sceneDescription;

    // Use this for initialization
    void Start()
    {
        isTrigger = false;

        Debug.Log(SceneManager.GetActiveScene().name);
    }

    private void Update()
    {
        if (isTrigger && Input.GetKeyDown(KeyCode.Return))
        {
            positionSceneChange = new Vector3(transform.position.x, transform.position.y);

            // save the position when loading another scene
            //character.SavePosition(positionSceneChange);

            if (colliderControl.name.Equals("door"))
                SceneManager.LoadScene(ScenesNames.M002Ushuaia);
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            if (!instructionInterface.activeSelf)
            {
                ReadSceneDescription();
            }
        }
    }

    public void ReadSceneDescription()
    {
        ReadText(sceneDescription);
        Debug.Log(sceneDescription);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Equals("door"))
        {
            warningInterface.SetActive(true);
            warningText.text = "Pressione ENTER para sair da casa.";
        }

        isTrigger = true;
        colliderControl = collision;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        warningInterface.SetActive(false);

        isTrigger = false;
        colliderControl = null;
    }
}

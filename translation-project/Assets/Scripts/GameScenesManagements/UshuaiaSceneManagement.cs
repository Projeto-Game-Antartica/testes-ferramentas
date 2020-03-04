using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UshuaiaSceneManagement : AbstractScreenReader {

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

    public AudioSource audioSource;

    public AudioClip avisoClip;
    public AudioClip closeClip;

    private string missionNumber = "M002";


    // Use this for initialization
    void Start ()
    {
        isTrigger = false;

        //InitialInstruction();

        if (PlayerPrefs.GetInt("Saved_"+missionNumber) == 1)
        {
            transform.position = character.GetPosition(missionNumber);
            chasingCamera.SetCameraPosition(character.GetPosition(missionNumber));
            Debug.Log(transform.position);
        }

        Debug.Log(SceneManager.GetActiveScene().name);
    }

    private void Update()
    {
        if (isTrigger && Input.GetKeyDown(KeyCode.Return))
        {
            if (colliderControl.name.Equals("casa_ushuaia"))
            {
                positionSceneChange = new Vector3(transform.position.x, transform.position.y);

                // save the position when loading another scene
                character.SavePosition(positionSceneChange, missionNumber);
                SceneManager.LoadScene(ScenesNames.M002CasaUshuaia);
            }
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

    void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.name);
        if (collision.name.Equals("casa_ushuaia"))
        {
            audioSource.PlayOneShot(avisoClip);
            warningInterface.SetActive(true);
            warningText.text = "Pressione ENTER para entrar na casinha de Ushuaia.";
            ReadText(warningText.text);
        }
        
        if(collision.name.Equals("placa_casinha"))
        {
            Debug.Log("casinha de ushuaia");
            ReadText("casinha de ushuaia");
        }

        if (collision.name.Equals("placa_centroinf"))
        {
            Debug.Log("Centro de informações");
            ReadText("centro de informações");
        }

        isTrigger = true;
        colliderControl = collision;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (warningInterface.activeSelf)
            audioSource.PlayOneShot(closeClip);

        warningInterface.SetActive(false);

        isTrigger = false;
        colliderControl = null;
    }
}

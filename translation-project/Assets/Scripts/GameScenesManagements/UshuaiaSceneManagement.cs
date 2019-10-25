using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UshuaiaSceneManagement : MonoBehaviour {

    private bool isTrigger;

    public SimpleCharacterController character;
    public ChasingCamera chasingCamera;

    private Vector2 positionSceneChange;

    public TextMeshProUGUI warningText;
    public GameObject warningInterface;

    private Collider2D colliderControl;

    // Use this for initialization
    void Start ()
    {
        isTrigger = false;

        //InitialInstruction();

        if (PlayerPrefs.GetInt("Saved") == 1)
        {
            transform.position = character.GetPosition();
            chasingCamera.SetCameraPosition(character.GetPosition());
            Debug.Log(transform.position);
        }

        Debug.Log(SceneManager.GetActiveScene().name);
    }

    private void Update()
    {
        if (isTrigger && Input.GetKeyDown(KeyCode.Return))
        {
            positionSceneChange = new Vector3(transform.position.x, transform.position.y);

            // save the position when loading another scene
            character.SavePosition(positionSceneChange);

            if (colliderControl.name.Equals("casa_ushuaia"))
                SceneManager.LoadScene(ScenesNames.M002CasaUshuaia);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.name);
        if (collision.name.Equals("casa_ushuaia"))
        {
            warningInterface.SetActive(true);
            warningText.text = "Pressione ENTER para entrar na casa.";
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

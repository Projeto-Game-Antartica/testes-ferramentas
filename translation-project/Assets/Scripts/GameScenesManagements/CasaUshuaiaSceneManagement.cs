using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CasaUshuaiaSceneManagement : MonoBehaviour {

    private bool isTrigger;

    public SimpleCharacterController character;
    public ChasingCamera chasingCamera;

    private Vector2 positionSceneChange;

    public TextMeshProUGUI warningText;
    public GameObject warningInterface;

    private Collider2D colliderControl;

    // Use this for initialization
    void Start()
    {
        isTrigger = false;

        //InitialInstruction();

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

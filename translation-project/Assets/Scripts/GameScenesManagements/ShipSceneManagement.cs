using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

// attached to the character
public class ShipSceneManagement : MonoBehaviour {

    private bool isTrigger;
    public GameObject warningInterface;
    public SimpleCharacterController character;
    public ChasingCamera chasingCamera;

    private Vector2 positionSceneChange;

    private Collider2D colliderControl = null;

    public TextMeshProUGUI warningText;

    public void Start()
    {
        isTrigger = false;

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
        if (isTrigger && Input.GetKeyDown(KeyCode.E))
        {
            positionSceneChange = new Vector3(transform.position.x, transform.position.y);
            // save the position when loading another scene
            character.SavePosition(positionSceneChange);

            //if (colliderControl.name.Equals("cabine principal"))
            //    SceneManager.LoadScene("ShipInsideScene");
            //else 
            if (colliderControl.name.Equals("Figurante") && PlayerPreferences.finishedAllM004Games())
                SceneManager.LoadScene("TailMissionScene");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("scene-trigger");
        
        //if (collision.name.Equals("cabine principal"))
        //{
        //    warningInterface.SetActive(true);
        //    warningText.text = "Pressione E para entrar no passadiço do navio.";
        //}
        //else 
        if(collision.name.Equals("Figurante"))
        {
            warningInterface.SetActive(true);
            if (PlayerPreferences.finishedAllM004Games())
                warningText.text = "Pressione E para realizar o desafio.";
            else
                warningText.text = "Para realizar a missão é necessário concluir todos os minijogos.";
        }

        isTrigger = true;
        colliderControl = collision;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        warningInterface.SetActive(false);
        isTrigger = false;

        colliderControl = null;
    }
}

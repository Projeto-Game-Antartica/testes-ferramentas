using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

// attached to the character
public class ShipSceneManagement : MonoBehaviour {

    private bool isTrigger;
    public GameObject warningInterface;
    public SimpleCharacterController character;

    private Vector3 positionSceneChange;
    
    public void Start()
    {
        if (PlayerPrefs.GetInt("Saved") == 1) transform.position = character.GetPosition();
    }

    private void Update()
    {
        if (isTrigger)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                positionSceneChange = new Vector3(transform.position.x, transform.position.y, transform.position.z);

                // save the position when loading another scene
                character.SavePosition(positionSceneChange);
                SceneManager.LoadScene("ShipInsideScene");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger");
        if (collision.name.Equals("cabine principal"))
        {
            warningInterface.SetActive(true);
            isTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        warningInterface.SetActive(false);
        isTrigger = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

// attached to the character
public class InsideShipSceneManagement : MonoBehaviour
{
    private bool isTrigger;
    public GameObject warningInterface;

    private void Update()
    {
        if (isTrigger)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                SceneManager.LoadScene("ShipScene");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger");
        if (collision.name.Equals("door"))
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

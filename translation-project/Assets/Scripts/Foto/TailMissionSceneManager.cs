using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TailMissionSceneManager : MonoBehaviour
{

    public GameObject instructionInterface;
    public Button audioButton;
    public LifeExpController lifeExpController;

    public CameraOverlayMissionController cameraOverlayController;

    public AudioSource audioSource;
    public AudioClip avisoClip;
    public AudioClip closeClip;

    public GameObject confirmQuit;

    private bool isOnMenu = false;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (instructionInterface.activeSelf)
            {
                instructionInterface.SetActive(false);
                audioSource.PlayOneShot(closeClip);
            }
            else
            {
                TryReturnToShip();
            }
        }

        if (Input.GetKeyDown(InputKeys.MJMENU_KEY))
        {
            if (!isOnMenu)
            {
                audioButton.Select();
                isOnMenu = true;
            }
            else
            {
                cameraOverlayController.GetComponentInChildren<Button>().Select();
                isOnMenu = false;
            }
        }

        if (Input.GetKeyDown(InputKeys.PARAMETERS_KEY))
        {
            lifeExpController.ReadHPandEXP();
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (!instructionInterface.activeSelf)
            {
                instructionInterface.SetActive(true);
                instructionInterface.GetComponentInChildren<Button>().Select();
            }
        }
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(ScenesNames.M004TailMission);
    }

    public void TryReturnToShip()
    {
        audioSource.PlayOneShot(avisoClip);

        confirmQuit.SetActive(true);
        confirmQuit.GetComponentInChildren<Button>().Select();
    }

    public void ReturnToShip()
    {
        confirmQuit.SetActive(false);
        SceneManager.LoadScene("ShipScene");
    }

    public IEnumerator ReturnToShipCoroutine()
    {
        yield return new WaitForSeconds(4f);

        SceneManager.LoadScene(ScenesNames.M004Ship);
    }

    public void ReturnToMJ()
    {
        confirmQuit.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FotoidentificationSceneManager : AbstractScreenReader
{
    public GameObject instructionInterface;
    public Button audioButton;
    public LifeExpController lifeExpController;

    public AudioSource audioSource;
    public AudioClip avisoClip;
    public AudioClip closeClip;

    public GameObject confirmQuit;

    public ContentPanelController contentPanelController;
    public FotoidentificacaoController fotoidentificacaoController;

    private bool isOnMenu = false;

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
                contentPanelController.first_button.Select();
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
    
    public void TryReturnToShip()
    {
        audioSource.PlayOneShot(avisoClip);

        confirmQuit.SetActive(true);

        ReadText(ReadableTexts.instance.GetReadableText(ReadableTexts.key_gameplay_aviso_botoes, LocalizationManager.instance.GetLozalization()));

        ReadText(confirmQuit.GetComponentInChildren<TMPro.TextMeshProUGUI>().text);

        confirmQuit.GetComponentInChildren<Button>().Select();
    }

    public void ReturnToShip()
    {
        confirmQuit.SetActive(false);
        SceneManager.LoadScene(ScenesNames.M004Ship);
    }

    public IEnumerator ReturnToShipCoroutine()
    {
        yield return new WaitForSeconds(4f);

        SceneManager.LoadScene(ScenesNames.M004Ship);
    }

    public void ReturnToMJ()
    {
        confirmQuit.SetActive(false);

        if (contentPanelController.gameObject.activeSelf)
        {
            contentPanelController.first_button.Select();
        }
        else
        {
            fotoidentificacaoController.options[0].GetComponentInChildren<Button>().Select();
        }
    }
}

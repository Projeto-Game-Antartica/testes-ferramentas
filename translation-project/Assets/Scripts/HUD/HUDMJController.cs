using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDMJController : AbstractScreenReader
{
    public GameObject acessoTeclado;

    public TMPro.TextMeshProUGUI descricaoText;

    public TMPro.TextMeshProUGUI playerName;

    public AudioSource audioSource;

    public GameObject confirmQuit;
    public AudioClip avisoClip;

    public Toggle LibrasButton;

    private void Start()
    {
        playerName.text = PlayerPreferences.PlayerName.ToUpper();
        LibrasButton.isOn = Parameters.LIBRAS_ENABLED;
    }

    private void Update()
    {
        if(Input.GetKeyDown(InputKeys.ACESSOTECLADO_KEY))
        {
            acessoTeclado.SetActive(true);
            ReadText("Navegação via teclado: ");
            ReadText(descricaoText.text);
            acessoTeclado.GetComponentInChildren<Button>().Select();
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (acessoTeclado.activeSelf)
            {
                acessoTeclado.SetActive(false);
                ReadText("Paniel de acesso via teclado fechado.");
            } 
            else
            {
                TryQuit();
            }
        }
    }

    public void TryQuit()
    {
        audioSource.PlayOneShot(avisoClip);

        confirmQuit.SetActive(true);

        ReadText(ReadableTexts.instance.GetReadableText(ReadableTexts.key_gameplay_aviso_botoes, LocalizationManager.instance.GetLozalization()));

        ReadText(confirmQuit.GetComponentInChildren<TMPro.TextMeshProUGUI>().text);

        confirmQuit.GetComponentInChildren<Button>().Select();

    }

    public void Quit()
    {
        //if (!PlayerPreferences.M009_Memoria) lifeExpController.RemoveEXP(0.0001f); // saiu sem concluir o minijogo
        if(ScenesNames.GoBackTo != "")
            UnityEngine.SceneManagement.SceneManager.LoadScene(ScenesNames.GoBackTo);
        else 
            Debug.Log("ScenesNames.GoBackTo not set.");
    }
}

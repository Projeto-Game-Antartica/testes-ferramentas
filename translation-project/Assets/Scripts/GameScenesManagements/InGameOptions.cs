using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class InGameOptions : AbstractScreenReader {

    public Toggle toggle;
    public Animator charAnimator;
    public GameObject confirmQuit;
    public Button yesButton;

    public TextMeshProUGUI confirmText;

    public AudioSource audioSource;
    public AudioClip warningClip;

    public GameObject leituraToggle;
    public GameObject voltarButton;

    private void Start()
    {
        toggle.isOn = Parameters.ACCESSIBILITY;    
    }

    private void OnEnable()
    {
        // runs idle animation when the ingameoption is active
        charAnimator.SetFloat("Magnitude", 0);

        if(leituraToggle.activeInHierarchy)
        {
            Debug.Log("toggle");
            leituraToggle.GetComponentInChildren<Toggle>().Select();
        }
        else if (voltarButton.activeInHierarchy)
        {
            Debug.Log("button");
            voltarButton.GetComponentInChildren<Button>().Select();
        }
    }

    public void TryReturnToMenu()
    {
        confirmQuit.SetActive(true);
        audioSource.PlayOneShot(warningClip);

        ReadText(ReadableTexts.instance.GetReadableText(ReadableTexts.key_gameplay_aviso_botoes, LocalizationManager.instance.GetLozalization()));

        confirmText.text = "Tem certeza que deseja retornar ao menu principal?";

        ReadText(confirmText.text);

        yesButton.onClick.AddListener(ReturnToMainMenu);

        yesButton.Select();
    }


    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(ScenesNames.Menu);
    }

    public void TryQuitGame()
    {
        confirmQuit.SetActive(true);
        audioSource.PlayOneShot(warningClip);

        ReadText(ReadableTexts.instance.GetReadableText(ReadableTexts.key_gameplay_aviso_botoes, LocalizationManager.instance.GetLozalization()));

        confirmText.text = "Tem certeza que deseja sair do jogo?";

        ReadText(confirmText.text);

        yesButton.onClick.AddListener(QuitGame);

        yesButton.Select();
    }

    public void QuitGame()
    {
        TolkUtil.Unload();
        Debug.Log("Quit");
        Application.Quit();
        confirmQuit.SetActive(false);
    }

    public void SetAcessibility(bool acessibility)
    {
        Parameters.ACCESSIBILITY = acessibility;

        try
        {
            if (Parameters.ACCESSIBILITY) TolkUtil.Load();
            else TolkUtil.Unload();
        }
        catch(Exception e)
        {
            Debug.Log("Tolk isnt running: " + e.StackTrace);
        }
    }

    public void ReadToggle(Toggle toggle)
    {
        var tmp = "";

        if (toggle.isOn)
            tmp = "ativado";
        else
            tmp = "desativado";

        ReadText(toggle.name + "" + tmp);
    }
}

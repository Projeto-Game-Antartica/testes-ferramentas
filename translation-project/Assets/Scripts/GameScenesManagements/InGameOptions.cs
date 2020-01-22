using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameOptions : AbstractScreenReader {

    public Toggle toggle;
    public Animator charAnimator;
    public GameObject confirmQuit;

    public AudioSource audioSource;
    public AudioClip warningClip;

    private void Start()
    {
        toggle.isOn = Parameters.ACCESSIBILITY;    
    }

    private void OnEnable()
    {
        // runs idle animation when the ingameoption is active
        charAnimator.SetFloat("Magnitude", 0);
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

        ReadText("Tem certeza que deseja sair do jogo?");

        confirmQuit.GetComponentInChildren<Button>().Select();
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

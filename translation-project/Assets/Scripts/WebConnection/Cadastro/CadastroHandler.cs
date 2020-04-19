 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class CadastroHandler : AbstractScreenReader
{
    [SerializeField]
    private TMP_InputField inputName;
    [SerializeField]
    private TMP_InputField inputEmail;
    [SerializeField]
    private TMP_InputField inputPassw;
    [SerializeField]
    private TMP_InputField inputConfirm_passw;
    //[SerializeField]
    //private Toggle toggleTermos;

    [SerializeField]
    private GameObject CadastroMsg;
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip avisoClip;

    private void Start()
    {
        inputName.Select();
    }

    private void Update()
    {
        if(Input.GetKeyDown(InputKeys.AUDIODESCRICAO_KEY))
        {
            // audiodescription
        }
    }

    public void TryToRegister()
    {
        string result = "";
        if (!string.IsNullOrEmpty(inputName.text) && !string.IsNullOrEmpty(inputEmail.text))
        {
            if (!string.IsNullOrEmpty(inputPassw.text) && inputPassw.text.Equals(inputConfirm_passw.text))
            {
                //Debug.Log("cadastrando...");

                string hashedPassword = SecurePasswordHasher.Hash(inputPassw.text);

                //Debug.Log(hashedPassword);
                
                StartCoroutine(DBConnection.instance.RegisterUser(inputName.text, inputEmail.text, hashedPassword, DateTime.Now, Register));
            }
            else
            {
                SetWarningMsg("As senhas digitadas não combinam");
            }
        }
        else
        {
            SetWarningMsg("Preencha todos os campos");
        }
    }

    public void Register(bool success)
    {
        if (success)
        {
            Debug.Log("Registration successfull");
            ReadText("Registrado com sucesso");
            SceneManager.LoadScene(ScenesNames.Menu);
        }
        else
        {
            SetWarningMsg("Erro ao registrar");
            SceneManager.LoadScene(ScenesNames.Cadastro);
        }
    }

    public void Back()
    {
        SceneManager.LoadScene(ScenesNames.Login);
    }

    public void SetWarningMsg(string result)
    {
        CadastroMsg.SetActive(true);
        audioSource.PlayOneShot(avisoClip);

        CadastroMsg.GetComponentInChildren<TextMeshProUGUI>().text = result;
        ReadText(result);
    }
}

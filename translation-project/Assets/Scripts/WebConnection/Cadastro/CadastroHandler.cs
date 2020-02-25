 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class CadastroHandler : MonoBehaviour
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

    public void TryToRegister()
    {
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
                Debug.Log("As senhas digitadas não combinam");
            }
        }
        else
        {
            Debug.Log("Preencha todos os campos.");
        }
    }

    public void Register(bool success)
    {
        if (success)
        {
            Debug.Log("Registration successfull");
            SceneManager.LoadScene(ScenesNames.Menu);
        }
        else
        {
            Debug.Log("Registration failed");
            SceneManager.LoadScene(ScenesNames.Cadastro);
        }
    }

    public void Back()
    {
        SceneManager.LoadScene(ScenesNames.Login);
    }
}

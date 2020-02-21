using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LoginScene : AbstractScreenReader {

    public Button criarContaButton;
    public Button facebookButton;
    public Button emailButton;
    
    public GameObject emailLoginHolder;

    [SerializeField]
    private TMP_InputField emailInput;
    [SerializeField]
    private TMP_InputField passwordInput;
    [SerializeField]
    private GameObject response;


    private void Start()
    {
        criarContaButton.Select();
    }

    public void CreateAccount()
    {
        SceneManager.LoadScene(ScenesNames.Cadastro);
    }

    public void LoginFacebook()
    {
        // TO DO
        SceneManager.LoadScene(ScenesNames.Menu);
    }

    public void LoginEmail()
    {
        emailLoginHolder.SetActive(true);
        //SetButtonsInteractable(false);
    }

    public void TryLoginEmail()
    {
        //Debug.Log("trying to log in...");

        if (!string.IsNullOrEmpty(emailInput.text) && !string.IsNullOrEmpty(passwordInput.text))
        {
            StartCoroutine(DBConnection.instance.TryLogIn(emailInput.text, passwordInput.text, System.DateTime.Now, LoginSuccessfull));
        }
        else
        {
            Debug.Log("Wrong Credentials");
            response.SetActive(true);
            response.GetComponentInChildren<TextMeshProUGUI>().text = "O endereço de email ou a senha que você inseriu não é válido.";
        }
    }

    public void LoginSuccessfull(bool success)
    {
        if (success)
        {
            Debug.Log("Log in successful");
            SceneManager.LoadScene(ScenesNames.Menu);
        }
        else
        {
            Debug.Log("Wrong Credentials");
            response.SetActive(true);
            response.GetComponentInChildren<TextMeshProUGUI>().text = "O endereço de email ou a senha que você inseriu não é válido.";
        }
    }

    public void SetButtonsInteractable(bool value)
    {
        criarContaButton.interactable = value;
        emailButton.interactable = value;
        facebookButton.interactable = value;
    }
}

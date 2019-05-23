using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginScene : AbstractScreenReader {

    public UnityEngine.UI.Button firstButton;

    private void Start()
    {
        firstButton.Select();
    }

    public void CreateAccount()
    {
        SceneManager.LoadScene("CadastroScene");
    }

    public void LoginFacebook()
    {
        // TO DO
        SceneManager.LoadScene("MenuScene");
    }

    public void LoginEmail()
    {
        // TO DO
        SceneManager.LoadScene("MenuScene");
    }
}

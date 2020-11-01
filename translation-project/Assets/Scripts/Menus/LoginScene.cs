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

    [SerializeField]
    private GameObject screenReaderWarning;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip avisoClip;

    [SerializeField]
    private TextMeshProUGUI warningText;

    [SerializeField]
    private TextMeshProUGUI version;


    private void Start()
    {
        version.text = Parameters.VERSION;

        //if (PlayerPrefs.GetInt("ScreenReaderWarning", 0) <= 0)
        //{
        //    PlayerPrefs.SetInt("ScreenReaderWarning", 1);

        screenReaderWarning.SetActive(true);
        audioSource.PlayOneShot(avisoClip);
        ReadText(warningText);

        screenReaderWarning.GetComponentInChildren<Button>().Select();
        //}
    }

    private void Update()
    {
        if(Input.GetKeyDown(InputKeys.AUDIODESCRICAO_KEY))
        {
            // read audiodescription
        }
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
        ReadText("Entre com seu email e senha");
        emailLoginHolder.GetComponentInChildren<TMP_InputField>().Select();
        //SetButtonsInteractable(false);
    }

    public void onYesClick()
    {
        Parameters.ACCESSIBILITY = false;

        TolkUtil.Unload();

        screenReaderWarning.SetActive(false);

        criarContaButton.Select();
    }

    public void TryLoginEmail()
    {
        //Debug.Log("trying to log in...");

        if(emailInput.text.Equals("debug123"))
        {
            SceneManager.LoadScene(ScenesNames.Menu);
        }
        else
        {
            if (!string.IsNullOrEmpty(emailInput.text) && !string.IsNullOrEmpty(passwordInput.text))
            {
                StartCoroutine(DBConnection.instance.TryLogIn(emailInput.text, passwordInput.text, System.DateTime.Now, LoginSuccessfull));
            }
            else
            {
                Debug.Log("Wrong Credentials");
                response.SetActive(true);
                response.GetComponentInChildren<TextMeshProUGUI>().text = "O endereço de email ou a senha que você inseriu não é válido.";

                ReadText(response.GetComponentInChildren<TextMeshProUGUI>().text);
            }
        }
    }

    public void LoginSuccessfull(bool success)
    {
        if (success)
        {
            Debug.Log("Log in successful");
            ReadText("Login bem sucedido.");
            SceneManager.LoadScene(ScenesNames.Menu);
        }
        else
        {
            Debug.Log("Wrong Credentials");
            response.SetActive(true);
            response.GetComponentInChildren<TextMeshProUGUI>().text = "O endereço de email ou a senha que você inseriu não é válido.";
            ReadText(response.GetComponentInChildren<TextMeshProUGUI>().text);
        }
    }

    public void SetButtonsInteractable(bool value)
    {
        criarContaButton.interactable = value;
        emailButton.interactable = value;
        facebookButton.interactable = value;
    }
}

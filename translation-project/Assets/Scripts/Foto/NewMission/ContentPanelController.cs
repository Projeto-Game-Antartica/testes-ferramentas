using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// not used
public class ContentPanelController : AbstractScreenReader {

    public Button first_button;
    public GameObject panelCatalogo;
    public GameObject panelNomear;
    public GameObject panelOptions;
    public GameObject panelFotoidentificacao;
    public GameObject instructionInterface;
    public GameObject titleText;
    public Button saveButton;
    public Button restartButton;
    public InputField whaleNameInput;
    public WhaleController whaleController;
    public WhaleImages whaleImages;
    public GameObject[] buttons;
    public Button[] fotoidentificationButtons;
    public FotoidentificacaoController fotoidentificacaoController;

    public TextMeshProUGUI pigmentacaoTMPro;
    public TextMeshProUGUI manchasTMPro;
    public TextMeshProUGUI riscosTMPro;
    public TextMeshProUGUI marcasTMPro;
    public TextMeshProUGUI entalheTMPro;
    public TextMeshProUGUI bordaTMPro;
    public TextMeshProUGUI pontaTMPro;

    private ReadableTexts readableTexts;

    public GameObject WinImage;
    
    private void Start()
    {
        //TolkUtil.Load();
        //Parameters.ACCESSIBILITY = true;
        
        //readableTexts = GameObject.Find("ReadableTexts").GetComponent<ReadableTexts>();
        //ReadInstructions();
        first_button.Select();
        
    }

    // Update is called once per frame
    void Update () {

        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (!instructionInterface.activeSelf)
                instructionInterface.SetActive(true);
            else
                instructionInterface.SetActive(false);
            ReadInstructions();
        }

        // not the best way
        if (Parameters.ISWHALEIDENTIFIED)
        {
            ActivatePanelNomear();
        }
        else
        {
            if (panelNomear != null)
            {
                panelNomear.SetActive(false);
                titleText.SetActive(true);
            }
        }

        CheckFotoidentificacaoButtons();

        //Debug.Log(Parameters.ISBORDADONE && Parameters.ISENTALHEDONE && Parameters.ISMANCHASDONE && Parameters.ISMARCASDONE && Parameters.ISPIGMENTACAODONE
        //    && Parameters.ISPONTADONE && Parameters.ISRISCOSDONE);
	}
    
    
    private void OnEnable()
    {
        if (Parameters.HIGH_CONTRAST) HighContrastText.ChangeTextBackgroundColor();
    }

    public void ReadInstructions()
    {
        ReadText(readableTexts.GetReadableText(ReadableTexts.key_foto_catalogDescription, LocalizationManager.instance.GetLozalization()));
    }

    public void Save()
    {
        gameObject.SetActive(false);
        panelCatalogo.SetActive(true);
    }

    public void SetWhalePhoto()
    {
        // adaptint to choose a whale (from cameraOverlay)
        // reset the panel
        whaleImages.SetPhotographedWhaleImage(null);
        SetFotoidentificacaoParameters(false);
        SetFotoidentificationButtons(true);
        StartCoroutine(GetWhaleInfo());
        saveButton.interactable = true;
        restartButton.interactable = true;
        Debug.Log("whale photo set");
    }

    public void StartFotoidenfiticacao(TextMeshProUGUI type)
    {
        gameObject.SetActive(false);
        panelFotoidentificacao.SetActive(true);
        
        switch(type.text.ToLower())
        {
            case "pigmentação":
                fotoidentificacaoController.RoundSettings(Parameters.PIGMENTACAO);
                break;
            case "manchas":
                fotoidentificacaoController.RoundSettings(Parameters.MANCHAS);
                break;
            case "riscos":
                fotoidentificacaoController.RoundSettings(Parameters.RISCOS);
                break;
            case "marcas":
                fotoidentificacaoController.RoundSettings(Parameters.MARCAS);
                break;
            case "borda":
                fotoidentificacaoController.RoundSettings(Parameters.BORDA);
                break;
            case "ponta":
                fotoidentificacaoController.RoundSettings(Parameters.PONTAS);
                break;
            case "entalhe":
                fotoidentificacaoController.RoundSettings(Parameters.ENTALHE);
                break;
            default:
                break;
        }
    }

    public void ActivatePanelNomear()
    {
        if (panelNomear != null)
        {
            panelOptions.SetActive(false);
            titleText.SetActive(false);
            saveButton.interactable = false;
            panelNomear.SetActive(true);

            if (Parameters.HIGH_CONTRAST) HighContrastText.ChangeTextBackgroundColor();

            string whale_name = whaleController.getWhaleById(Parameters.WHALE_ID).whale_name;

            if (!whale_name.Equals(""))
            {
                Debug.Log(whale_name);
                whaleNameInput.text = whale_name;
                //whaleNameInput.text = whale_name;
                whaleNameInput.interactable = false;
            }
            else
            {
                whaleNameInput.interactable = true;
                whaleNameInput.Select();
            }
        }
    }

    public void ClearInputFieldPanelNomear()
    {
        whaleNameInput.text = string.Empty;
    }

    public void ResetButtonColors()
    {
        foreach(GameObject b in buttons)
        {
            b.GetComponent<Image>().color = Color.white;
        }
    }

    public void CheckFotoidentificacaoButtons()
    {
        if (Parameters.ISPIGMENTACAODONE)
            pigmentacaoTMPro.color = Color.green;
        else
            pigmentacaoTMPro.color = Color.white;

        if (Parameters.ISMANCHASDONE)
            manchasTMPro.color = Color.green;
        else
            manchasTMPro.color = Color.white;

        if (Parameters.ISRISCOSDONE)
            riscosTMPro.color = Color.green;
        else
            riscosTMPro.color = Color.white;

        if (Parameters.ISMARCASDONE)
            marcasTMPro.color = Color.green;
        else
            marcasTMPro.color = Color.white;

        if (Parameters.ISENTALHEDONE)
            entalheTMPro.color = Color.green;
        else
            entalheTMPro.color = Color.white;

        if (Parameters.ISBORDADONE)
            bordaTMPro.color = Color.green;
        else
            bordaTMPro.color = Color.white;

        if (Parameters.ISPONTADONE)
            pontaTMPro.color = Color.green;
        else
            pontaTMPro.color = Color.white;

        if (Parameters.ISBORDADONE && Parameters.ISENTALHEDONE && Parameters.ISMANCHASDONE && Parameters.ISMARCASDONE && Parameters.ISPIGMENTACAODONE
            && Parameters.ISPONTADONE && Parameters.ISRISCOSDONE)
        {
            // wins the game
            PlayerPreferences.M004_FotoIdentificacao = true;
            WinImage.SetActive(true);
            // return to ship after 3 seconds
            StartCoroutine(ReturnToShipCoroutine());
        }
    }

    // set the fotoidentificacao parameters to false to begin the proccess 
    // false = buttons interactables, true = button not interactables
    public void SetFotoidentificacaoParameters(bool value)
    {
        Parameters.ISPIGMENTACAODONE = value;
        Parameters.ISMANCHASDONE = value;
        Parameters.ISRISCOSDONE = value;
        Parameters.ISMARCASDONE = value;
        Parameters.ISBORDADONE = value;
        Parameters.ISPONTADONE = value;
        Parameters.ISENTALHEDONE = value;
    }

    public void SetFotoidentificationButtons(bool value)
    {
        foreach (Button b in fotoidentificationButtons)
        {
            b.interactable = value;
        }
    }

    // retrieve whale info according to WhaleData class
    public IEnumerator GetWhaleInfo()
    {
        yield return new WaitForEndOfFrame();

        // get and random id
        Parameters.WHALE_ID = Random.Range(Parameters.MIN_ID, Parameters.MAX_ID);
        //Parameters.WHALE_ID = 7;

        // get the whale from random id
        WhaleData whale = whaleController.getWhaleById(Parameters.WHALE_ID);

        Debug.Log(whale.image_path);

        // set the image to all panels
        //panelImage.sprite = Resources.Load<Sprite>(whale.image_path);
        whaleImages.SetPhotographedWhaleImage(whale.image_path);

        // set the image to the catalog panel
        //catalogImage.sprite = panelImage.sprite;
    }

    public IEnumerator ReturnToShipCoroutine()
    {
        yield return new WaitForSeconds(7f);

        UnityEngine.SceneManagement.SceneManager.LoadScene(ScenesNames.M004Ship);
    }
}

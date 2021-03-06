﻿using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ContentPanelController : AbstractScreenReader {

    public Button first_button;
    public GameObject panelOptions;
    public GameObject panelFotoidentificacao;
    public Button saveButton;
    public Button restartButton;
    public WhaleController whaleController;
    public WhaleImages whaleImages;
    public Button[] fotoidentificationButtons;
    public FotoidentificacaoController fotoidentificacaoController;

    public TextMeshProUGUI descriptionText;

    public TextMeshProUGUI pigmentacaoTMPro;
    public TextMeshProUGUI manchasTMPro;
    public TextMeshProUGUI riscosTMPro;
    public TextMeshProUGUI marcasTMPro;
    public TextMeshProUGUI entalheTMPro;
    public TextMeshProUGUI bordaTMPro;
    public TextMeshProUGUI pontaTMPro;

    public GameObject WinImage;
    public TextMeshProUGUI winText;

    public AudioSource audioSource;
    public AudioClip victoryClip;
    public AudioClip closeClip;

    public bool finished = false;

    public FotoidentificationSceneManager sceneManager;

    public LifeExpController lifeExpController;

    // array containing the 8 indexes for photo
    // sort the array for randomization
    private int[] whaleIndexes = { 0, 1, 2, 3, 4, 5, 6, 7 };

    private int index;
    private bool isOnButtons = false;

    private void Start()
    {
        //TolkUtil.Load();
        //Parameters.ACCESSIBILITY = true;

        //readableTexts = GameObject.Find("ReadableTexts").GetComponent<ReadableTexts>();
        //ReadInstructions();
    }

    // Update is called once per frame
    void Update () {

        if (!finished)
            CheckFotoidentificacaoButtons();
        
        //if (Input.GetKeyDown(KeyCode.F2))
        //{
        //    ReadText(whaleController.getWhaleById(Parameters.WHALE_ID).description);
        //    Debug.Log(whaleController.getWhaleById(Parameters.WHALE_ID).description);
        //}

        if(Input.GetKeyDown(KeyCode.F3))
        {
            ReadText(ReadableTexts.instance.GetReadableText(ReadableTexts.key_m004_fotoidentificacao, LocalizationManager.instance.GetLozalization()));
        }

        if (Input.GetKeyDown(KeyCode.F6))
        {
            if(!isOnButtons)
            {
                first_button.Select();
                isOnButtons = true;
            }
            else
            {
                // leitura da audiodescricao da baleia...
                ReadText(whaleController.getWhaleById(Parameters.WHALE_ID).description);
                Debug.Log(whaleController.getWhaleById(Parameters.WHALE_ID).description);
            }

        }

        //Debug.Log(Parameters.ISBORDADONE && Parameters.ISENTALHEDONE && Parameters.ISMANCHASDONE && Parameters.ISMARCASDONE && Parameters.ISPIGMENTACAODONE
        //    && Parameters.ISPONTADONE && Parameters.ISRISCOSDONE);
    }

    public void Initialize()
    {
        ReadText(ReadableTexts.instance.GetReadableText(ReadableTexts.key_m004_fotoidentificacao, LocalizationManager.instance.GetLozalization()));

        SetWhalePhoto();

        ReadText(descriptionText);
        // randomize the indexes
        System.Random r = new System.Random();
        whaleIndexes = whaleIndexes.OrderBy(x => r.Next()).ToArray();

        index = 0;

        first_button.Select();
    }

    public void Save()
    {
        gameObject.SetActive(false);
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

        if (type.text.Contains("<b>"))
        {
            var tmp = type.text;

            tmp = tmp.Replace("<b>", "");
            tmp = tmp.Replace("</b>", "");

            type.text = tmp;
        }

        switch (type.text.ToLower())
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

    public void CheckFotoidentificacaoButtons()
    {
        if (Parameters.ISPIGMENTACAODONE)
            pigmentacaoTMPro.color = Color.yellow;
        else
            pigmentacaoTMPro.color = Color.white;

        if (Parameters.ISMANCHASDONE)
            manchasTMPro.color = Color.yellow;
        else
            manchasTMPro.color = Color.white;

        if (Parameters.ISRISCOSDONE)
            riscosTMPro.color = Color.yellow;
        else
            riscosTMPro.color = Color.white;

        if (Parameters.ISMARCASDONE)
            marcasTMPro.color = Color.yellow;
        else
            marcasTMPro.color = Color.white;

        if (Parameters.ISENTALHEDONE)
            entalheTMPro.color = Color.yellow;
        else
            entalheTMPro.color = Color.white;

        if (Parameters.ISBORDADONE)
            bordaTMPro.color = Color.yellow;
        else
            bordaTMPro.color = Color.white;

        if (Parameters.ISPONTADONE)
            pontaTMPro.color = Color.yellow;
        else
            pontaTMPro.color = Color.white;

        if (Parameters.ISBORDADONE && Parameters.ISENTALHEDONE && Parameters.ISMANCHASDONE && Parameters.ISMARCASDONE && Parameters.ISPIGMENTACAODONE
            && Parameters.ISPONTADONE && Parameters.ISRISCOSDONE)
        {
            finished = true;
            StartCoroutine(EndGame());
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

        Parameters.WHALE_ID = whaleIndexes[index];

        // get the whale from random id
        WhaleData whale = whaleController.getWhaleById(Parameters.WHALE_ID);

        Debug.Log(whale.image_path);

        // read the tail description
        ReadText(whale.description);
        Debug.Log(whale.description);

        // set the image to all panels
        //panelImage.sprite = Resources.Load<Sprite>(whale.image_path);
        whaleImages.SetPhotographedWhaleImage(whale.image_path);

        // increment whale index
        index++;
        if (index >= whaleIndexes.Length)
            index = 0;

        // set the image to the catalog panel
        //catalogImage.sprite = panelImage.sprite;
    }

    public IEnumerator EndGame()
    {
        // wins the game
        PlayerPreferences.M004_FotoIdentificacao = true;
        WinImage.SetActive(true);

        ReadText(winText);

        lifeExpController.AddEXP(PlayerPreferences.XPwinPuzzle);

        audioSource.PlayOneShot(victoryClip);
        
        yield return new WaitWhile(() => audioSource.isPlaying);

        ReadText(ReadableTexts.instance.GetReadableText(ReadableTexts.key_m004_fotoidentificacao_vitoria, LocalizationManager.instance.GetLozalization()));

        // return to ship after 3 seconds
        StartCoroutine(sceneManager.ReturnToShipCoroutine());
    }

    public void ReadButton(TextMeshProUGUI buttonText)
    {
        if (buttonText.text.Contains("<b>"))
        {
            buttonText.text = buttonText.text.Replace("<b>", "");
            buttonText.text = buttonText.text.Replace("</b>", "");
        }

        switch (buttonText.text.ToLower())
        {
            case "pigmentação":
                if(Parameters.ISPIGMENTACAODONE)
                {
                    ReadText("Característica identificada. " + buttonText.text);
                    Debug.Log("Característica identificada. " + buttonText.text);
                }
                else
                {
                    ReadText(buttonText.text);
                    Debug.Log(buttonText.text);
                }
                break;
            case "manchas":
                if (Parameters.ISMANCHASDONE)
                {
                    ReadText("Característica identificada. " + buttonText.text);
                    Debug.Log("Característica identificada. " + buttonText.text);
                }
                else
                {
                    ReadText(buttonText.text);
                    Debug.Log(buttonText.text);
                }
                break;
            case "riscos":
                if(Parameters.ISRISCOSDONE)
                {
                    ReadText("Característica identificada. " + buttonText.text);
                    Debug.Log("Característica identificada. " + buttonText.text);
                }
                else
                {
                    ReadText(buttonText.text);
                    Debug.Log(buttonText.text);
                }
                break;
            case "marcas":
                if(Parameters.ISMARCASDONE)
                {
                    ReadText("Característica identificada. " + buttonText.text);
                    Debug.Log("Característica identificada. " + buttonText.text);
                }
                else
                {
                    ReadText(buttonText.text);
                    Debug.Log(buttonText.text);
                }
                break;
            case "entalhe":
                if(Parameters.ISENTALHEDONE)
                {
                    ReadText("Característica identificada. " + buttonText.text);
                    Debug.Log("Característica identificada. " + buttonText.text);
                }
                else
                {
                    ReadText(buttonText.text);
                    Debug.Log(buttonText.text);
                }
                break;
            case "borda":
                if(Parameters.ISBORDADONE)
                {
                    ReadText("Característica identificada. " + buttonText.text);
                    Debug.Log("Característica identificada. " + buttonText.text);
                }
                else
                {
                    ReadText(buttonText.text);
                    Debug.Log(buttonText.text);
                }
                break;
            case "ponta":
                if(Parameters.ISPONTADONE)
                {
                    ReadText("Característica identificada. " + buttonText.text);
                    Debug.Log("Característica identificada. " + buttonText.text);
                }
                else
                {
                    ReadText(buttonText.text);
                    Debug.Log(buttonText.text);
                }
                break;
            default:
                break;
        }
    }
}

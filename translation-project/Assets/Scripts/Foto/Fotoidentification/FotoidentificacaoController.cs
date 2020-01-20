using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class FotoidentificacaoController : AbstractScreenReader {

    /*
     * Fixed parameters
     */
    private readonly string path_pigmentacao    = "Whales/sprites/pigmentacao/";
    private readonly string path_manchas        = "Whales/sprites/manchas/";
    private readonly string path_riscos         = "Whales/sprites/riscos/";
    private readonly string path_marcas         = "Whales/sprites/marcas/";
    private readonly string path_borda          = "Whales/sprites/borda/";
    private readonly string path_pontas         = "Whales/sprites/pontas/";
    private readonly string path_entalhe        = "Whales/sprites/entalhe/";
    

    /*
     * Questions and answers
     */
    private readonly string[] questions = new string[] 
    {
        "Identifique o padrão de pigmentação branca da cauda fotografada!",
        "Identifique a opção que caracteriza as manchas da cauda fotografada",
        "Identifique a opção que caracteriza os riscos da cauda fotografada",
        "Identifique a opção que caracteriza as marcas da cauda fotografada",
        "Identifique a opção que caracteriza a borda da cauda fotografada",
        "Identifique a opção que caracteriza a ponta da cauda fotografada",
        "Identifique a opção que caracteriza o entalhe da cauda fotografada",
    };

    private readonly string[,] answersInLine = new string[,]
    {
        {"1A - 95%","1B - 95%","2 - 75%","3 - 50%","4 - 25%","5 - 5%","","",""},
        {"Esquerda","Centro","Direita", "Esquerda e Direita", "Esquerda e Centro", "Centro e Direita", "Esquerda, Centro e Direita", "Sem", "" },
        {"Esquerda","Centro","Direita", "Esquerda e Direita", "Esquerda e Centro", "Centro e Direita", "Esquerda, Centro e Direita", "Sem", "" },
        {"Esquerda","Centro","Direita", "Esquerda e Direita", "Esquerda e Centro", "Centro e Direita", "Esquerda, Centro e Direita", "Sem", "" },
        {"Lisa","Áspera", "", "","","","","",""},
        {"Arredondada","Aguda","","","","","","",""},
        {"Forma de V","Forma de U","","","","","","",""},
    };

    /*
     * Game Parameters
     */
    private int roundIndex;
    private WhaleData whaleData;
    public WhaleController whaleController;
    public Button nextButton;
    public Button undentifiedButton;
    public GameObject fotoidentificacaoPanel;
    public Image whalesCatalogImage;
    private int attempts = 0;
    public GameObject LoseImage;

    public AudioSource audioSource;
    public AudioClip loseClip;
    public AudioClip correctClip;
    public AudioClip wrongClip;

    public TextMeshProUGUI attemptsText;

    public FotoidentificationSceneManager sceneManager;

    /*
     *  Set the GameObjects on the inspector according to the view. Answers options in line or in rows.
     *  For row option: with design purposes, the order of columns is 1,0,2.
     */
    public GameObject[] options;
    public GameObject[] whale_images;
    public TextMeshProUGUI questionText;

    public bool buttonSelected = false;

    public LifeExpController lifeExpController;

    private void Start()
    {
        // index from whale of photo for first round
        //whaleData = whaleController.getWhaleById(Parameters.WHALE_ID);  

        //roundIndex = 0;
        //RoundSettings(roundIndex);
        //SetOptionActiveInactive(roundIndex);
        //SetOptionSprites(roundIndex);

        audioSource = GetComponent<AudioSource>();
        attempts = 0;
    }

    private void Update()
    {
        // not the best way but works
        whaleData = whaleController.getWhaleById(Parameters.WHALE_ID);

        
        //if (Input.GetKeyDown(KeyCode.F2))
        //{
        //    ReadText(whaleController.getWhaleById(Parameters.WHALE_ID).description);
        //    Debug.Log(whaleController.getWhaleById(Parameters.WHALE_ID).description);
        //}

        if (Input.GetKeyDown(KeyCode.F3))
        {
            ReadCharacteristics(roundIndex);
        }

        if (Input.GetKeyDown(KeyCode.F6))
        {
            if (!buttonSelected)
            {
                undentifiedButton.Select();

                ReadText(whaleController.getWhaleById(Parameters.WHALE_ID).description);
                Debug.Log(whaleController.getWhaleById(Parameters.WHALE_ID).description);

                buttonSelected = true;
            }
            else
            {
                options[0].GetComponentInChildren<Button>().Select();
                buttonSelected = false;
            }
        }
    }

    public void CheckAnswer(GameObject option)
    {
        if(isCorrect(option.GetComponentInChildren<TextMeshProUGUI>(), roundIndex))
        {
            audioSource.PlayOneShot(correctClip);

            Debug.Log(option.GetComponentInChildren<TextMeshProUGUI>().text);
            ChangeBackgroundColor(option, Color.green);

            PlayerPreferences.M004_FotoIdentificacao = true;

            // enable and select the next button
            nextButton.interactable = true;
            nextButton.Select();
        }
        else
        {
            lifeExpController.AddEXP(PlayerPreferences.XPwrongTry);

            audioSource.PlayOneShot(wrongClip);
            attempts++;
            attemptsText.text = "Erros: " + attempts;

            Debug.Log(attempts);
            ChangeBackgroundColor(option, Color.red);

            ReadText(attemptsText.text);
            Debug.Log(attemptsText.text);

            if (attempts > 3)
            {
                StartCoroutine(EndGame());
            }
        }
    }

    public IEnumerator EndGame()
    {
        LoseImage.SetActive(true);

        ReadText(ReadableTexts.instance.GetReadableText(ReadableTexts.key_m004_memoria_derrota, LocalizationManager.instance.GetLozalization()));

        audioSource.PlayOneShot(loseClip);

        lifeExpController.AddEXP(PlayerPreferences.XPlosePuzzle);

        yield return new WaitWhile(() => audioSource.isPlaying);

        ReadText("Infelizmente você não conseguiu finalizar o minijogo com êxito. Tente novamente.");

        StartCoroutine(sceneManager.ReturnToShipCoroutine());
    }

    public void RoundSettings(int roundIndex)
    {
        // next button starts unactive
        nextButton.interactable = false;

        // set the roundIndex
        this.roundIndex = roundIndex;

        // set the question
        questionText.text = questions[roundIndex];

        ReadText(questionText.text);
        
        // show the needed buttons for round
        SetOptionActiveInactive(roundIndex);
        SetOptionSprites(roundIndex);

        // get all options for round
        string[] result = GetOptionText(roundIndex);

        // set the corresponding option text
        for (int i = 0; i < answersInLine.GetLength(1); i++)
        {
            if (options[i].activeSelf)
            {
                options[i].GetComponentInChildren<TextMeshProUGUI>().text = result[i];
            }
        }

        options[0].GetComponentInChildren<Button>().Select();
    }
    
    public void ChangeBackgroundColor(GameObject option, Color color)
    {
        option.GetComponent<Image>().color = color;
    }

    public string[] GetOptionText(int roundIndex)
    {
        string[] result;
        switch (roundIndex)
        {
            case Parameters.PIGMENTACAO:
                result = new string[6] 
                {
                    answersInLine[Parameters.PIGMENTACAO, 0], answersInLine[Parameters.PIGMENTACAO,1], answersInLine[Parameters.PIGMENTACAO,2],
                    answersInLine[Parameters.PIGMENTACAO, 3], answersInLine[Parameters.PIGMENTACAO, 4], answersInLine[Parameters.PIGMENTACAO, 5]
                };
                break;
            case Parameters.MANCHAS:
                result = new string[8] 
                {
                    answersInLine[Parameters.MANCHAS, 0], answersInLine[Parameters.MANCHAS, 1], answersInLine[Parameters.MANCHAS, 2], answersInLine[Parameters.MANCHAS, 3],
                    answersInLine[Parameters.MANCHAS, 4], answersInLine[Parameters.MANCHAS, 5], answersInLine[Parameters.MANCHAS, 6], answersInLine[Parameters.MANCHAS, 7]
                };
                break;
            case Parameters.RISCOS:
                result = new string[8]
                {
                    answersInLine[Parameters.RISCOS, 0], answersInLine[Parameters.RISCOS, 1], answersInLine[Parameters.RISCOS, 2], answersInLine[Parameters.RISCOS, 3],
                    answersInLine[Parameters.RISCOS, 4], answersInLine[Parameters.RISCOS, 5], answersInLine[Parameters.RISCOS, 6], answersInLine[Parameters.RISCOS, 7]
                };
                break;
            case Parameters.MARCAS:
                result = new string[8]
                {
                    answersInLine[Parameters.MARCAS, 0], answersInLine[Parameters.MARCAS, 1], answersInLine[Parameters.MARCAS, 2], answersInLine[Parameters.MARCAS, 3],
                    answersInLine[Parameters.MARCAS, 4], answersInLine[Parameters.MARCAS, 5], answersInLine[Parameters.MARCAS, 6], answersInLine[Parameters.MARCAS, 7]
                };
                break;
            case Parameters.BORDA:
                result = new string[3] { answersInLine[Parameters.BORDA, 0], answersInLine[Parameters.BORDA, 1], answersInLine[Parameters.BORDA,2] };
                break;
            case Parameters.PONTAS:
                result = new string[2] { answersInLine[Parameters.PONTAS, 0], answersInLine[Parameters.PONTAS, 1] };
                break;
            case Parameters.ENTALHE:
                result = new string[2] { answersInLine[Parameters.ENTALHE, 0], answersInLine[Parameters.ENTALHE, 1] };
                break;
            default:
                result = null;
                break;
        }
        return result;
    }
    
    public void SetOptionActiveInactive(int roundIndex)
    {
        //answersInLine.GetLenght(1) = # columns of matrix answers
        for (int i = 0; i < answersInLine.GetLength(1); i++)
        {
            if (answersInLine[roundIndex, i].Equals(""))
            {
                options[i].SetActive(false);
            }
            else
            {
                options[i].SetActive(true);
            }
        }
    }

    public void SetOptionSprites(int roundIndex)
    {
        switch(roundIndex)
        {
            case Parameters.PIGMENTACAO:
                whale_images[0].GetComponent<Image>().sprite = LoadSprite(path_pigmentacao + "1A");
                whale_images[1].GetComponent<Image>().sprite = LoadSprite(path_pigmentacao + "1B");
                whale_images[2].GetComponent<Image>().sprite = LoadSprite(path_pigmentacao + "2");
                whale_images[3].GetComponent<Image>().sprite = LoadSprite(path_pigmentacao + "3");
                whale_images[4].GetComponent<Image>().sprite = LoadSprite(path_pigmentacao + "4");
                whale_images[5].GetComponent<Image>().sprite = LoadSprite(path_pigmentacao + "5");

                ReadCharacteristics(Parameters.PIGMENTACAO);
                Debug.Log(ReadableTexts.instance.GetReadableText(ReadableTexts.key_m004_fotoidentificacao_pigmentacao, LocalizationManager.instance.GetLozalization()));
                break;
            case Parameters.MANCHAS:
                whale_images[0].GetComponent<Image>().sprite = LoadSprite(path_manchas + "mancha_esquerda");
                whale_images[1].GetComponent<Image>().sprite = LoadSprite(path_manchas + "mancha_centro");
                whale_images[2].GetComponent<Image>().sprite = LoadSprite(path_manchas + "mancha_direita");
                whale_images[3].GetComponent<Image>().sprite = LoadSprite(path_manchas + "mancha_esquerda_direita");
                whale_images[4].GetComponent<Image>().sprite = LoadSprite(path_manchas + "mancha_esquerda_centro");
                whale_images[5].GetComponent<Image>().sprite = LoadSprite(path_manchas + "mancha_centro_direita");
                whale_images[6].GetComponent<Image>().sprite = LoadSprite(path_manchas + "mancha_amarelada");
                whale_images[7].GetComponent<Image>().sprite = LoadSprite(path_manchas + "sem");

                ReadCharacteristics(Parameters.MANCHAS);
                break;
            case Parameters.RISCOS:
                whale_images[0].GetComponent<Image>().sprite = LoadSprite(path_riscos + "risco_esquerda");
                whale_images[1].GetComponent<Image>().sprite = LoadSprite(path_riscos + "risco_centro");
                whale_images[2].GetComponent<Image>().sprite = LoadSprite(path_riscos + "risco_direita");
                whale_images[3].GetComponent<Image>().sprite = LoadSprite(path_riscos + "risco_esquerda_direita");
                whale_images[4].GetComponent<Image>().sprite = LoadSprite(path_riscos + "risco_esquerda_centro");
                whale_images[5].GetComponent<Image>().sprite = LoadSprite(path_riscos + "risco_centro_direita");
                whale_images[6].GetComponent<Image>().sprite = LoadSprite(path_riscos + "risco");
                whale_images[7].GetComponent<Image>().sprite = LoadSprite(path_riscos + "sem");

                ReadCharacteristics(Parameters.RISCOS);
                break;
            case Parameters.MARCAS:
                whale_images[0].GetComponent<Image>().sprite = LoadSprite(path_marcas + "marca_esquerda");
                whale_images[1].GetComponent<Image>().sprite = LoadSprite(path_marcas + "marca_centro");
                whale_images[2].GetComponent<Image>().sprite = LoadSprite(path_marcas + "marca_direita");
                whale_images[3].GetComponent<Image>().sprite = LoadSprite(path_marcas + "marca_esquerda_direita");
                whale_images[4].GetComponent<Image>().sprite = LoadSprite(path_marcas + "marca_esquerda_centro");
                whale_images[5].GetComponent<Image>().sprite = LoadSprite(path_marcas + "marca_centro_direita");
                whale_images[6].GetComponent<Image>().sprite = LoadSprite(path_marcas + "marca");
                whale_images[7].GetComponent<Image>().sprite = LoadSprite(path_marcas + "sem");

                ReadCharacteristics(Parameters.MARCAS);
                break;
            case Parameters.BORDA:
                whale_images[0].GetComponent<Image>().sprite = LoadSprite(path_borda + "lisa");
                //whale_images[1].GetComponent<Image>().sprite = LoadSprite(path_borda + "poucoaspera");
                whale_images[2].GetComponent<Image>().sprite = LoadSprite(path_borda + "aspera");
                ReadCharacteristics(Parameters.BORDA);
                break;
            case Parameters.PONTAS:
                whale_images[0].GetComponent<Image>().sprite = LoadSprite(path_pontas + "arredondada");
                whale_images[1].GetComponent<Image>().sprite = LoadSprite(path_pontas + "aguda");

                ReadCharacteristics(Parameters.PONTAS);
                break;
            case Parameters.ENTALHE: 
                whale_images[0].GetComponent<Image>().sprite = LoadSprite(path_entalhe + "v_final");
                whale_images[1].GetComponent<Image>().sprite = LoadSprite(path_entalhe + "u_final");

                ReadCharacteristics(Parameters.ENTALHE);
                break;
            default:
                break;
        }
    }

    public void ReadCharacteristics(int type)
    {
        switch(type)
        {
            case Parameters.PIGMENTACAO:
                ReadText(ReadableTexts.instance.GetReadableText(ReadableTexts.key_m004_fotoidentificacao_pigmentacao, LocalizationManager.instance.GetLozalization()));
                //Debug.Log(ReadableTexts.instance.GetReadableText(ReadableTexts.key_m004_fotoidentificacao_pigmentacao, LocalizationManager.instance.GetLozalization()));
                break;
            case Parameters.MANCHAS:
                ReadText(ReadableTexts.instance.GetReadableText(ReadableTexts.key_m004_fotoidentificacao_mancha, LocalizationManager.instance.GetLozalization()));
                break;
            case Parameters.RISCOS:
                ReadText(ReadableTexts.instance.GetReadableText(ReadableTexts.key_m004_fotoidentificacao_riscos, LocalizationManager.instance.GetLozalization()));
                break;
            case Parameters.MARCAS:
                ReadText(ReadableTexts.instance.GetReadableText(ReadableTexts.key_m004_fotoidentificacao_marcas, LocalizationManager.instance.GetLozalization()));
                break;
            case Parameters.BORDA:
                ReadText(ReadableTexts.instance.GetReadableText(ReadableTexts.key_m004_fotoidentificacao_borda, LocalizationManager.instance.GetLozalization()));
                break;
            case Parameters.PONTAS:
                ReadText(ReadableTexts.instance.GetReadableText(ReadableTexts.key_m004_fotoidentificacao_ponta, LocalizationManager.instance.GetLozalization()));
                break;
            case Parameters.ENTALHE:
                ReadText(ReadableTexts.instance.GetReadableText(ReadableTexts.key_m004_fotoidentificacao_entalhe, LocalizationManager.instance.GetLozalization()));
                break;
            default:
                break;
        }
    }

    public Sprite LoadSprite(string path)
    {
        return Resources.Load<Sprite>(path);
    }

    public bool isCorrect(TextMeshProUGUI answer, int roundIndex)
    {
        // compare the answer with the respective field on json file.
        // compare the texts with lower case.
        switch(roundIndex)
        {
            case Parameters.PIGMENTACAO:
                Debug.Log(whaleData.image_path +": " + answer.text + " " + whaleData.pigmentacao);
                if (answer.text.ToLower().Equals(whaleData.pigmentacao.ToLower()))
                {
                    Parameters.ISPIGMENTACAODONE = true;
                    return true;
                }
                return false;
            case Parameters.MANCHAS:
                Debug.Log(whaleData.image_path + ": " + answer.text + " " + whaleData.manchas);
                if (answer.text.ToLower().Equals(whaleData.manchas.ToLower()))
                {
                    Parameters.ISMANCHASDONE = true;
                    return true;
                }
                return false;
            case Parameters.RISCOS:
                Debug.Log(whaleData.image_path + ": " + answer.text + " " + whaleData.riscos);
                if (answer.text.ToLower().Equals(whaleData.riscos.ToLower()))
                {
                    Parameters.ISRISCOSDONE = true;
                    return true;
                }
                return false;
            case Parameters.MARCAS:
                Debug.Log(whaleData.image_path + ": " + answer.text + " " + whaleData.marcas);
                if (answer.text.ToLower().Equals(whaleData.marcas.ToLower()))
                {
                    Parameters.ISMARCASDONE = true;
                    return true;
                }
                return false;
            case Parameters.BORDA:
                Debug.Log(whaleData.image_path +": " + answer.text + " " + whaleData.borda);
                if (answer.text.ToLower().Equals(whaleData.borda.ToLower()))
                {
                    Parameters.ISBORDADONE = true;
                    return true;
                }
                return false;
            case Parameters.PONTAS:
                Debug.Log(whaleData.image_path +": " + answer.text + " " + whaleData.ponta);
                if (answer.text.ToLower().Equals(whaleData.ponta.ToLower()))
                {
                    Parameters.ISPONTADONE = true;
                    return true;
                }
                return false;
            case Parameters.ENTALHE:
                Debug.Log(whaleData.image_path +": " + answer.text + " " + whaleData.entalhe);
                if (answer.text.ToLower().Equals(whaleData.entalhe.ToLower()))
                {
                    Parameters.ISENTALHEDONE = true;
                    return true;
                }
                return false;
            default:
                return false;
        }
    }

    public void ResetColors()
    {
        foreach(GameObject g in options)
        {
            g.GetComponent<Image>().color = Color.white;
        }
    }
    /*
    public void NextRound()
    {
        roundIndex++;
        Debug.Log("round num " + roundIndex);
        if (!CheckEndGame())
        {
            ResetColors();
            RoundSettings(roundIndex);
            nextButton.gameObject.SetActive(false);
        }
        else
        {
            // to do
        }
    }

    public bool CheckEndGame()
    {
        if (roundIndex >= lastQuestion)
        {
            roundIndex = 0;
            RoundSettings(roundIndex);
            ResetColors();
            fotoidentificacaoPanel.SetActive(false);

            whalesCatalogPanel.SetActive(true);
            ReadText("Fotoidentificação finalizada. Painel de catálogo de baleias aberto");
            return true;
        }

        return false;
    }
    */
}

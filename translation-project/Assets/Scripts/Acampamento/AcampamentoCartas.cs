using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;


public class AcampamentoCartas : AbstractCardManager
{
    public Button audioButton;
    public AudioSource audioSource;
    public GameObject confirmQuit;
    public AudioClip victoryClip;

    public GameObject WinImage;
    public TMPro.TextMeshProUGUI WinText;
    public GameObject LoseImage;

    public Button resetButton;
    public Button backButton;

    private bool WinGame = false;

    public LifeExpController lifeExpController;

    public GameObject instruction_interface;

    public AudioClip victoryAudio;
    public AudioClip loseAudio;
    public AudioClip closeClip;
    public AudioClip avisoClip;

    public Image fill;

    public Image fills;

    public Image fillm;

    private float variavel = 50; 

    private float coracao;

    private float estrela;

    private float mapa;

    private readonly int MAX_COR = 100;
    private readonly int MAX_EST = 100;
    private readonly int MAX_MAP = 100;
    // Use this for initialization

    private bool isOnMJMenu = false;

    private void Start()
    {
        fill.fillAmount =  variavel / MAX_COR;
        fills.fillAmount = variavel / MAX_EST;
        fillm.fillAmount = variavel / MAX_MAP;

        resetButton.interactable = false;
        backButton.interactable = false;

        if (instruction_interface.activeSelf)
            instruction_interface.GetComponentInChildren<Button>().Select();
	}

    private void Update()
    {
        if(fill.fillAmount != 0 && fills.fillAmount != 0 && fillm.fillAmount != 0 && WinGame)
        {
            StartCoroutine(EndGame(true));                              
	    }

        if (fill.fillAmount == 0 || fills.fillAmount == 0 || fillm.fillAmount == 0)
        {
            StartCoroutine(EndGame(false));              
		}

        if (Input.GetKeyDown(InputKeys.INSTRUCTIONS_KEY))
        {
            instruction_interface.SetActive(true);
        }

        if(Input.GetKey(KeyCode.Escape))
        {
            instruction_interface.SetActive(false);
        }

        if (Input.GetKeyDown(InputKeys.MJMENU_KEY))
        {
            if (!isOnMJMenu)
                audioButton.Select();
            else
                likeButton.Select();

            isOnMJMenu = !isOnMJMenu;
        }

        if (Input.GetKeyDown(KeyCode.F6))
        {
            likeButton.Select();
        }

        if (Input.GetKeyDown(InputKeys.AUDIODESCRICAO_KEY))
        {
            // audiodescricao
        }

        if (Input.GetKeyDown(InputKeys.REPEAT_KEY))
        {
            //ReadCard(cardIndex);
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (instruction_interface.activeSelf)
            {
                audioSource.PlayOneShot(closeClip);
                instruction_interface.SetActive(false);
            }
            else
            {
                TryReturnToCamp();
            }
        }
    }

    public void InitializeGame()
    {
        WinGame = false;
        cardIndex = 0;

        fill.fillAmount = 0.5f;
        fills.fillAmount = 0.5f;
        fillm.fillAmount = 0.5f;

        currentImage.sprite = sprites[cardIndex];
        currentImage.name = sprites[cardIndex].name;
        cardName.text = currentImage.name;

        Debug.Log(cardName.text);

        nextImage.GetComponentInChildren<Image>().sprite = sprites[cardIndex + 1];
        nextImage.name = sprites[cardIndex + 1].name;

        initialPosition = currentImage.transform.parent.position;

        resetButton.interactable = true;
        backButton.interactable = true;
    }

    public override void CheckLike()
    {
        Debug.Log(cardIndex);
        // do something
        //cardname
        switch (currentImage.name.ToLower())
        {
            case "Abridor de latas":
                estrela = -1;
                coracao = 0;
                mapa = 1;
                break;
            case "Bandeira":
                estrela = 3;
                coracao = 0;
                mapa = -1;
                break;
            case "Barraca-depósito (material, banheiro)":
                estrela = 3;
                coracao = 2;
                mapa = 3;
                break;
            case "Barracas dormitórios(individual)":
                coracao = 1;
                estrela = 3;
                mapa = 2;
                break;
            case "Barracas amplas (polar haven)":
                coracao = 1;
                estrela = 3;
                mapa = 3;
                break;
            case "Benjamin":
                coracao = -1;
                estrela = -1;
                mapa = 0;
                break;
            case "Cadeiras de praia":
                coracao = 0;
                estrela = -2;
                mapa = -1;
                break;
            case "cafeteira":
                coracao = -3;
                estrela = -2;
                mapa = -3;
                break;
            case "Caixas de suprimento":
                coracao = 2;
                estrela = 3;
                mapa = 1;
                break;
            case "Capa para chuva":
                coracao = 0;
                estrela = -1;
                mapa = 0;
                break;
            case "carreta de carga":
                coracao = -1;
                estrela = 3;
                mapa = -1;
                break;
            case "Celular e carregador":
                coracao = 0;
                estrela = 2;
                mapa = 0;
                break;
            case "Chinelo":
                coracao = 0;
                estrela = -2;
                mapa = 0;
                break;
            case "combustivel":
                coracao = 2;
                estrela = 4;
                mapa = -2;
                break;
            case "Copos descartáveis":
                coracao = -3;
                estrela = -1;
                mapa = -3;
                break;
            case "Detergente":
                coracao = 3;
                estrela = -1;
                mapa = -3;
                break;
            case "Espelho":
                coracao = 0;
                estrela = -1;
                mapa = 0;
                break;
            case "Espetos":
                coracao = -2;
                estrela = -2;
                mapa = -2;
                break;
            case "Esponja":
                coracao = 2;
                estrela = -1;
                mapa = 0;
                break;
            case "estação meteorológica":
                coracao = 0;
                estrela = 3;
                mapa = 0;
                break;
            case "fita adesiva":
                coracao = 0;
                estrela = 1;
                mapa = -1;
                break;
            case "Garrafa térmica":
                coracao = 2;
                estrela = 4;
                mapa = 2;
                break;
            case "Gerador":
                coracao = 0;
                estrela = 3;
                mapa = -1;
                break;
            case "guarda-sol":
                coracao = 0;
                estrela = -2;
                mapa = -2;
                break;
            case "Guardanapos":
                coracao = 2;
                estrela = -1;
                mapa = -2;
                break;
            case "gás":
                coracao = -1;
                estrela = -2;
                mapa = -2;
                break;
            case "Lanterna e pilhas":
                coracao = 1;
                estrela = 2;
                mapa = -2;
                break;
            case "lençois e cobertas":
                coracao = -1;
                estrela = -1;
                mapa = 0;
                break;
            case "Luminária":
                coracao = 1;
                estrela = 2;
                mapa = 1;
                break;
            case "Luvas de proteção":
                coracao = 2;
                estrela = 4;
                mapa = 3;
                break;
            case "Martelete":
                coracao = 2;
                estrela = 4;
                mapa = -1;
                break;
            case "Martelo":
                coracao = 2;
                estrela = 4;
                mapa = -1;
                break;
            case "mesa":
                coracao = -1;
                estrela = -2;
                mapa = 0;
                break;
            case "Panela":
                coracao = -2;
                estrela = -2;
                mapa = 0;
                break;
            case "Papel alumínio":
                coracao = 2;
                estrela = 2;
                mapa = 2;
                break;
            case "Papel higiênico":
                coracao = 2;
                estrela = 2;
                mapa = -1;
                break;
            case "Pincel":
                coracao = 2;
                estrela = 4;
                mapa = 2;
                break;
            case "Pratos":
                coracao = 1;
                estrela = -1;
                mapa = 0;
                break;
            case "Protetor solar":
                coracao = -2;
                estrela = 1;
                mapa = -1;
                break;
            case "Quadriciclo":
                coracao = -1;
                estrela = 3;
                mapa = -1;
                break;
            case "Repelente para mosquito":
                coracao = 0;
                estrela = -2;
                mapa = -3;
                break;
            case "sabonete":
                coracao = 2;
                estrela = -1;
                mapa = -1;
                break;
            case "Saco de dormir":
                coracao = -1;
                estrela = 3;
                mapa = 1;
                break;
            case "Saco plástico":
                coracao = 2;
                estrela = 3;
                mapa = 2;
                break;
            case "Saco para lixo":
                coracao = 3;
                estrela = 2;
                mapa = 2;
                break;
            case "Talhadeira":
                coracao = 2;
                estrela = 4;
                mapa = -1;
                break;
            case "Talheres de alumínio":
                coracao = 0;
                estrela = -1;
                mapa = 0;
                break;
            case "talheres descatavéis":
                coracao = 2;
                estrela = 4;
                mapa = -1;
                break;
            case "tesoura":
                coracao = 0;
                estrela = 1;
                mapa = 1;
                break;
            case "toalhas":
                coracao = 1;
                estrela = -2;
                mapa = 0;
                break;
            case "tonel":
                coracao = 2;
                estrela = 3;
                mapa = 3;
                break;
            case "Travesseiros":
                coracao = 0;
                estrela = -1;
                mapa = 0;
                break;
            case "Ventilador pequeno":
                coracao = 0;
                estrela = -1;
                mapa = 0;
                break;
            case "Óculos de proteção":
                coracao = 2;
                estrela = 4;
                mapa = 2;
                WinGame = true;
                break;

            default:
                coracao = 0;
                estrela = 0;
                mapa = 0;
                break;
        }

        CheckCalories(coracao, estrela, mapa);
        NextCard();
    }

    public override void CheckDislike()
    {
        switch (currentImage.name.ToLower())
        {
            case "Abridor de latas":
                estrela = 1;
                coracao = 0;
                mapa =-1;
                break;
            case "Bandeira":
                estrela =-3;
                coracao = 0;
                mapa = 1;
                break;
            case "Barraca-depósito (material, banheiro)":
                estrela =-3;
                coracao =-2;
                mapa =-3;
                break;
            case "Barracas dormitórios(individual)":
                coracao =-1;
                estrela =-3;
                mapa =-2;
                break;
            case "Barracas amplas (polar haven)":
                coracao =-1;
                estrela =-3;
                mapa =-3;
                break;
            case "Benjamin":
                coracao = 1;
                estrela = 1;
                mapa = 0;
                break;
            case "Cadeiras de praia":
                coracao = 0;
                estrela = 2;
                mapa = 1;
                break;
            case "cafeteira":
                coracao = 3;
                estrela = 2;
                mapa = 3;
                break;
            case "Caixas de suprimento":
                coracao =-2;
                estrela =-3;
                mapa =-1;
                break;
            case "Capa para chuva":
                coracao = 0;
                estrela = 1;
                mapa = 0;
                break;
            case "carreta de carga":
                coracao = 1;
                estrela =-3;
                mapa = 1;
                break;
            case "Celular e carregador":
                coracao = 0;
                estrela =-2;
                mapa = 0;
                break;
            case "Chinelo":
                coracao = 0;
                estrela = 2;
                mapa = 0;
                break;
            case "combustivel":
                coracao =-2;
                estrela =-4;
                mapa = 2;
                break;
            case "Copos descartáveis":
                coracao = 3;
                estrela = 1;
                mapa = 3;
                break;
            case "Detergente":
                coracao =-3;
                estrela = 1;
                mapa = 3;
                break;
            case "Espelho":
                coracao = 0;
                estrela = 1;
                mapa = 0;
                break;
            case "Espetos":
                coracao = 2;
                estrela = 2;
                mapa = 2;
                break;
            case "Esponja":
                coracao =-2;
                estrela = 1;
                mapa = 0;
                break;
            case "estação meteorológica":
                coracao = 0;
                estrela =-3;
                mapa = 0;
                break;
            case "fita adesiva":
                coracao = 0;
                estrela =-1;
                mapa = 1;
                break;
            case "Garrafa térmica":
                coracao =-2;
                estrela =-4;
                mapa =-2;
                break;
            case "Gerador":
                coracao = 0;
                estrela =-3;
                mapa = 1;
                break;
            case "guarda-sol":
                coracao = 0;
                estrela = 2;
                mapa = 2;
                break;
            case "Guardanapos":
                coracao =-2;
                estrela = 1;
                mapa = 2;
                break;
            case "gás":
                coracao = 1;
                estrela = 2;
                mapa = 2;
                break;
            case "Lanterna e pilhas":
                coracao =-1;
                estrela =-2;
                mapa = 2;
                break;
            case "lençois e cobertas":
                coracao = 1;
                estrela = 1;
                mapa = 0;
                break;
            case "Luminária":
                coracao =-1;
                estrela =-2;
                mapa =-1;
                break;
            case "Luvas de proteção":
                coracao =-2;
                estrela =-4;
                mapa =-3;
                break;
            case "Martelete":
                coracao =-2;
                estrela =-4;
                mapa = 1;
                break;
            case "Martelo":
                coracao =-2;
                estrela =-4;
                mapa = 1;
                break;
            case "mesa":
                coracao = 1;
                estrela = 2;
                mapa = 0;
                break;
            case "Panela":
                coracao = 2;
                estrela = 2;
                mapa = 0;
                break;
            case "Papel alumínio":
                coracao =-2;
                estrela =-2;
                mapa =-2;
                break;
            case "Papel higiênico":
                coracao =-2;
                estrela =-2;
                mapa = 1;
                break;
            case "Pincel":
                coracao =-2;
                estrela =-4;
                mapa =-2;
                break;
            case "Pratos":
                coracao =-1;
                estrela = 1;
                mapa = 0;
                break;
            case "Protetor solar":
                coracao = 2;
                estrela =-1;
                mapa = 1;
                break;
            case "Quadriciclo":
                coracao = 1;
                estrela =-3;
                mapa = 1;
                break;
            case "Repelente para mosquito":
                coracao = 0;
                estrela = 2;
                mapa = 3;
                break;
            case "sabonete":
                coracao =-2;
                estrela = 1;
                mapa = 1;
                break;
            case "Saco de dormir":
                coracao = 1;
                estrela =-3;
                mapa =-1;
                break;
            case "Saco plástico":
                coracao =-2;
                estrela =-3;
                mapa =-2;
                break;
            case "Saco para lixo":
                coracao =-3;
                estrela =-2;
                mapa =-2;
                break;
            case "Talhadeira":
                coracao =-2;
                estrela =-4;
                mapa = 1;
                break;
            case "Talheres de alumínio":
                coracao = 0;
                estrela = 1;
                mapa = 0;
                break;
            case "talheres descatavéis":
                coracao =-2;
                estrela =-4;
                mapa = 1;
                break;
            case "tesoura":
                coracao = 0;
                estrela =-1;
                mapa =-1;
                break;
            case "toalhas":
                coracao =-1;
                estrela = 2;
                mapa = 0;
                break;
            case "tonel":
                coracao =-2;
                estrela =-3;
                mapa =-3;
                break;
            case "Travesseiros":
                coracao = 0;
                estrela = 1;
                mapa = 0;
                break;
            case "Ventilador pequeno":
                coracao = 0;
                estrela = 1;
                mapa = 0;
                break;
            case "Óculos de proteção":
                coracao =-2;
                estrela =-4;
                mapa =-2;
                WinGame = true;
                break;

            default:
                coracao = 0;
                estrela = 0;
                mapa = 0;
                break;
        }

        CheckCalories(coracao, estrela, mapa);
        NextCard();
    }

    public void CheckCalories(float coracao, float estrela, float mapa)
    {
        // normalize
        fill.fillAmount += coracao / MAX_COR;

        fills.fillAmount += estrela / MAX_EST;

        fillm.fillAmount += mapa / MAX_MAP;
    
        if (fill.fillAmount <= 0.2)
        {
            minijogosDicas.GetComponent<MinijogosDicas>().ShowIsolatedHint("Cuidado com a limpeza do acampamento!!");
            Debug.Log("Cuidado com a limpeza do acampamento!!");
        }
            
        else if (fills.fillAmount <= 0.2)
        {   
            minijogosDicas.GetComponent<MinijogosDicas>().ShowIsolatedHint("Cuidado com seus pontos de experiência!!");
                    //minijogosDicas.SetHintByIndex(1);

            Debug.Log("Cuidado com seus pontos de experiência!!");
        }

        else if (fillm.fillAmount <= 0.2)
        {
            minijogosDicas.GetComponent<MinijogosDicas>().ShowIsolatedHint("A Antártica não pode sofrer mais danos!!");
                    //minijogosDicas.SetHintByIndex(2);

            Debug.Log("A Antártica não pode sofrer mais danos!");
        }
    }

    public void NextCard()
    {
        cardIndex++;
        
        if (cardIndex < sprites.Length)
        {
            currentImage.sprite = nextImage.sprite;
            currentImage.name = sprites[cardIndex].name;
            cardName.text = currentImage.name;

            Debug.Log("Novo alimento: " + cardName.text);
            ReadText("Novo alimento: " + cardName.text);

            if (cardIndex < sprites.Length - 1)
            {
                nextImage.sprite = sprites[cardIndex + 1];
                nextImage.name = sprites[cardIndex + 1].name;
            }
            else
            {
                Debug.Log("fim das cartas... Começando de novo");
                cardIndex = -1;
                nextImage.sprite = sprites[cardIndex+1];
                nextImage.name = sprites[cardIndex+1].name;
            }
        }
        ResetPosition();
    }
    public void TryReturnToCamp()
    {
        audioSource.PlayOneShot(avisoClip);

        confirmQuit.SetActive(true);

        //ReadText(ReadableTexts.instance.GetReadableText(ReadableTexts.key_gameplay_aviso_botoes, LocalizationManager.instance.GetLozalization()));

        //ReadText(confirmQuit.GetComponentInChildren<TMPro.TextMeshProUGUI>().text);

        confirmQuit.GetComponentInChildren<Button>().Select();
    }

    public IEnumerator EndGame(bool win)
    {
        if (win)
        {
            likeButton.interactable = false;
            dislikeButton.interactable = false;

            PlayerPreferences.M009_Itens = true;

            audioSource.PlayOneShot(victoryClip);

            yield return new WaitWhile(() => audioSource.isPlaying);

            WinImage.SetActive(true);

            //audioSource.PlayOneShot(victoryAudio);
            //yield return new WaitWhile(() => audioSource.isPlaying);

            lifeExpController.AddEXP(PlayerPreferences.XPwinPuzzle); // finalizou o minijogo
            lifeExpController.AddEXP(3*PlayerPreferences.XPwinItem); // ganhou o item    
        }
        else
        {
              LoseImage.SetActive(true);

              //ReadText(ReadableTexts.instance.GetReadableText(ReadableTexts.key_m004_memoria_derrota, LocalizationManager.instance.GetLozalization()));

              audioSource.PlayOneShot(loseAudio);

              yield return new WaitWhile(() => audioSource.isPlaying);

              //ReadText("Infelizmente você não conseguiu finalizar o minijogo com êxito. Tente novamente.");
              resetButton.Select();
              lifeExpController.AddEXP(PlayerPreferences.XPlosePuzzle); // jogou um minijogo
              Debug.Log("Zerou um marcador!");
        }

        StartCoroutine(ReturnToCampCoroutine());
    }

    public IEnumerator ReturnToCampCoroutine()
    {
        yield return new WaitForSeconds(4f);

        UnityEngine.SceneManagement.SceneManager.LoadScene(ScenesNames.M009Camp);
    }
}
 
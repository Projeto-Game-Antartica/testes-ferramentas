using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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

    private Dictionary<string, string> cardsDescription = new Dictionary<string, string>
    {
        {"abridor de latas", "Imagem de abridor de latas de alumínio." },
        {"bandeira", "Imagem da bandeira do brasil hasteada sobre fundo de céu azul." },
        {"barraca-depósito (material, banheiro)", "Imagem de barraca de acampamento preta, em poliester, utilizada como banheiro." },
        {"barracas dormitórios(individual)", "Imagem de barraca de acampamento grande, na cor vermelha com detalhes em amarelo." },
        {"benjamin", "Imagem de benjamin branco de três pontas." },
        {"cadeiras de praia", "Imagem de cadeira de praia com estrutura em alumínio, encosto e assento azuis." },
        {"caixas de suprimento", "Imagem de uma caixa plástica,  retangular, branca." },
        {"capa para chuva", "Imagem de capa de chuva amarela." },
        {"celular e carregador", "Imagem de celular branco com carregador preto." },
        {"chinelo", "Imagem de par de chinelos brancos com as tiras azuis." },
        {"detergente", "Imagem de detergente amarelo em embalagem plástica transparente, com rótulo azul." },
        {"espelho", "Imagem de espelho dobravél com a parte de trás branca." },
        {"espetos", "Imagem de três espetos de churrasco em alumínio com cabo de madeira." },
        {"esponja", "Imagem de duas esponjas amarela com lã de aço verde em um dos lados." },
        {"garrafa térmica", "Imagem de uma garrafa térmica de alumínio com tampa e bico dosador." },
        {"gerador", "Imagem de um grande  gerador de energia elétrica." },
        {"guardanapos", "Imagem com pilha de guardanapos de papel brancos." },
        {"lanterna e pilhas", "Imagem de uma lanterna preta ao lado de duas pilhas amarelas." },
        {"luminária", "Imagem de luminária de mesa preta." },
        {"luvas de proteção", "Imagem de par de luvas de pano pigmentada." },
        {"martelete", "Imagem de um martelete com detalhes azuis nas laterais." },
        {"martelo", "Imagem de martelo marrom com cabo preto." },
        {"óculos de proteção", "Imagem de óculos de proteção com lentes transparentes e armação preta." },
        {"panela", "Imagem de panela pequena, com cabo e tampa pretos." },
        {"papel alumínio", "Imagem de um rolo de papel alumínio." },
        {"papel higiênico", "Imagem com três rolos de papel higiênico." },
        {"pincel", "Imagem de um pincel chato com cabo preto." },
        {"polar Harven", "Imagem de barraca de acampamento azul, uso de área comum, como cozinha, sala, entre outros." },
        {"pratos", "Imagem de três pratos fundos de alumínio." },
        {"protetor solar", "Imagem de protetor solar em embalagem laranja e roxa, com foto de um sol e rótulo vermelho." },
        {"quadriciclo", "Imagem de um quadriciclo preto com detalhes laterais em laranja." },
        {"repelente para mosquito", "Imagem de repelente com embalagem cinza, tampa verde e rótulo vermelho." },
        {"saco de dormir", "Imagem de saco de dormir azul escuro e detalhe em azul claro na ponta superior." },
        {"saco para lixo", "Imagem de um saco de lixo preto, com coisas dentro, amarrado nas pontas." },
        {"saco plástico", "Imagem com três sacos plásticos transparentes amarrados nas extremidades." },
        {"talhadeira", "Imagem de uma talhadeira cinza de ferro" },
        {"talheres de alumínio", "Imagem de uma colher, um garfo e uma faca. Os talheres são dobráveis, de alumínio, e possuem uma sacola de tecido, na cor preta para serem carregados." },
        {"travesseiros", "Imagem de dois travesseiros brancos,  um em cima do outro." },
        {"ventilador pequeno", "Imagem de ventilador pequeno de mesa, preto." }
    };

    private void Start()
    {
        //fill.fillAmount =  variavel / MAX_COR;
        //fills.fillAmount = variavel / MAX_EST;
        //fillm.fillAmount = variavel / MAX_MAP;

        resetButton.interactable = true;
        backButton.interactable = true;

        if (instruction_interface.activeSelf)
            instruction_interface.GetComponentInChildren<Button>().Select();
	}

    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow) ||
                                    Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow)))
        {
            GameObject nextCell = EventSystem.current.currentSelectedGameObject.gameObject;

            if (nextCell.GetComponent<Button>() != null)
            {
                nextCell.GetComponent<Selectable>().Select();
                ReadButton(nextCell);
            }
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

            GameObject nextCell = EventSystem.current.currentSelectedGameObject.gameObject;
            
            ReadButton(nextCell);
        }

        if (Input.GetKeyDown(InputKeys.AUDIODESCRICAO_KEY))
        {
            ReadText(ReadableTexts.instance.GetReadableText(ReadableTexts.key_m009_acampamento, LocalizationManager.instance.GetLozalization()));
        }
        if (Input.GetKeyDown(InputKeys.REPEAT_KEY))
        {
            ReadCard();
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

        if (Input.GetKeyDown(KeyCode.F5))
        {
            minijogosDicas.ShowHint();
        }
    }

    public void InitializeGame()
    {
        ReadText(ReadableTexts.instance.GetReadableText(ReadableTexts.key_m009_acampamento, LocalizationManager.instance.GetLozalization()));

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

        ReadCard();

        resetButton.interactable = true;
        backButton.interactable = true;
    }

    public override void CheckLike()
    {
        Debug.Log(cardIndex);
        // do something
        //cardname
        switch (currentImage.name)
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
            case "Polar Harven":
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
        switch (currentImage.name)
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
            case "Polar Harven":
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
        Debug.Log("entrou no CheckCalories");
        // normalize
        fill.fillAmount += coracao / MAX_COR;

        fills.fillAmount += estrela / MAX_EST;

        fillm.fillAmount += mapa / MAX_MAP;

        Debug.Log("entrou no coracao: " + coracao +"estrela: " +estrela + "mapa: " +mapa);
    
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

        likeButton.Select();
    }

    public void NextCard()
    {
        cardIndex++;
        
        if (cardIndex < sprites.Length)
        {
            currentImage.sprite = nextImage.sprite;
            currentImage.name = sprites[cardIndex].name;
            cardName.text = currentImage.name;

            Debug.Log("Novo item: " + cardName.text);
            ReadText("Novo item: " + cardName.text);

            ReadCard();

            if (cardIndex < sprites.Length - 1)
            {
                nextImage.sprite = sprites[cardIndex + 1];
                nextImage.name = sprites[cardIndex + 1].name;

                if(fill.fillAmount != 0 && fills.fillAmount != 0 && fillm.fillAmount != 0 && WinGame)
                {
                    StartCoroutine(EndGame(true));                              
	            }

                else if (fill.fillAmount == 0 || fills.fillAmount == 0 || fillm.fillAmount == 0)
                {
                    StartCoroutine(EndGame(false));              
		        }
            }
            else
            {
                if(fill.fillAmount != 0 && fills.fillAmount != 0 && fillm.fillAmount != 0 && WinGame)
                {
                    StartCoroutine(EndGame(true));                              
	            }

                else if (fill.fillAmount == 0 || fills.fillAmount == 0 || fillm.fillAmount == 0)
                {
                    StartCoroutine(EndGame(false));              
		        }

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
            WinImage.SetActive(true);

            PlayerPreferences.M009_Itens = true;

            ReadText("Parabéns, você ganhou dois itens necessário para sua aventura na antártica: luvas de proteção e sacos plásticos.");

            likeButton.interactable = false;
            dislikeButton.interactable = false;

            audioSource.PlayOneShot(victoryClip);

            yield return new WaitWhile(() => audioSource.isPlaying);

            ReadText(ReadableTexts.instance.GetReadableText(ReadableTexts.key_m009_acampamento_vitoria, LocalizationManager.instance.GetLozalization()));

            yield return new WaitForSeconds(5f);

            lifeExpController.AddEXP(PlayerPreferences.XPwinPuzzle); // finalizou o minijogo
            lifeExpController.AddEXP(3*PlayerPreferences.XPwinItem); // ganhou o item    

            // add the heart points in hp points
            //lifeExpController.AddHP(PlayerPreferences.calculateMJExperiencePoints(heartImage.fillAmount));
            // add the antartica and star points to xp points
            //lifeExpController.AddEXP(PlayerPreferences.calculateMJExperiencePoints(starImage.fillAmount, antarticaImage.fillAmount));

            minijogosDicas.GetComponent<MinijogosDicas>().dicas.SetActive(false);
        }
        else
        {
              LoseImage.SetActive(true);

              ReadText("Infelizmente você não conseguiu finalizar o minijogo com êxito. Tente novamente.");

              audioSource.PlayOneShot(loseAudio);

              yield return new WaitWhile(() => audioSource.isPlaying);

              ReadText(ReadableTexts.instance.GetReadableText(ReadableTexts.key_m009_acampamento_derrota, LocalizationManager.instance.GetLozalization()));

              resetButton.Select();

              yield return new WaitForSeconds(5f);

              lifeExpController.AddEXP(PlayerPreferences.XPlosePuzzle); // jogou um minijogo
              
              Debug.Log("Zerou um marcador!");

              minijogosDicas.GetComponent<MinijogosDicas>().dicas.SetActive(false);
        }

        StartCoroutine(ReturnToCampCoroutine());
    }

    public IEnumerator ReturnToCampCoroutine()
    {
        yield return new WaitForSeconds(4f);

        UnityEngine.SceneManagement.SceneManager.LoadScene(ScenesNames.M009Camp);
    }

    public void ReturnToCamp()
    {
        //confirmQuit.SetActive(false);

        if (!PlayerPreferences.M009_Itens) lifeExpController.RemoveEXP(0.0001f); // saiu sem concluir o minijogo
            UnityEngine.SceneManagement.SceneManager.LoadScene(ScenesNames.M009Camp);
    }

    public void ReadCard()
    {
        Debug.Log(cardsDescription[currentImage.name.ToLower()]);
        ReadText(cardsDescription[currentImage.name.ToLower()]);
    }

    public void ResetGameObjects()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(ScenesNames.M009Itens);
    }

    void ReadButton (GameObject nextCell)
    {
        if(nextCell.GetComponent<Button>() != null)
        {
            if(nextCell.GetComponent<Button>().name == "dislike")
            {
                    Debug.Log("Rejeitar item.");
                    ReadText("Rejeitar item.");
            }
            
            else if(nextCell.GetComponent<Button>().name == "like")
            {
                    Debug.Log("Aceitar item.");
                    ReadText("Aceitar item.");
            }
        }
    }
}
 
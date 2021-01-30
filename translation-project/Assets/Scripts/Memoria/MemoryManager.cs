using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class MemoryManager : AbstractScreenReader {

    // round 0
    public Sprite[] cardFace0;
    public Sprite[] cardText0;
    
    // round 1
    public Sprite[] cardFace1;
    public Sprite[] cardText1;

    public Sprite cardBack;

    public GameObject[] cards;

    private GameObject lastCardSelected;

    public Button confirmarButton;
    public Button cancelarButton;
    public Button resetButton;
    public Button audioButton;
    public Button backButton;

    public int[] index; 
    private int matches = 9;
    private int miss = 0;
    private int tries = 3;

    private bool init;

    public static int CARDFACE = 1;
    public static int CARDTEXT = 2;

    public TMPro.TextMeshProUGUI attempts;

    private AudioSource audioSource;

    public AudioClip correctAudio;
    public AudioClip wrongAudio;
    public AudioClip selectAudio;
    public AudioClip victoryAudio;
    public AudioClip loseAudio;
    public AudioClip closeClip;
    public AudioClip avisoClip;

    private List<int> c;

    public GameObject WinImage;
    public TMPro.TextMeshProUGUI WinText;
    public GameObject LoseImage;

    public GameObject acessoTeclado;

    // hint settings
    public MinijogosDicas dicas;

    public LifeExpController lifeExpController;

    public MJInstructionInterfaceController instructionInterface;
    public GameObject confirmQuit;

    private bool _first;
    private bool isOnMenu;

    private enum Operation { correct, confirm, wrong }

    public string missionName;

    private int indexOfFirstCardAdded = -1;

    private void Start()
    {
        init = false;
        _first = true;
        isOnMenu = false;

        audioSource = GetComponent<AudioSource>();

        if (instructionInterface.gameObject.activeSelf)
            instructionInterface.GetComponentInChildren<Button>().Select();
    }

    // Update is called once per frame
    void Update () {
        
        if (!_first && (Input.GetMouseButtonUp(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)) && !Card.DO_NOT)
        {
            Debug.Log("Checando cartas....");
            checkCards();
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (instructionInterface.gameObject.activeSelf)
            {
                instructionInterface.gameObject.SetActive(false);
                audioSource.PlayOneShot(closeClip);
            }
            else if(acessoTeclado.gameObject.activeSelf)
            {
                SelectLastCard(); // select last selected card when close acessoteclado
            }
            else
            {
                TryReturnToShip();
            }
        }

        if(Input.GetKeyDown(InputKeys.MJMENU_KEY))
        {
            if (!isOnMenu)
            {
                audioButton.Select();
                isOnMenu = true;
            }
            else
            {
                getFirstCard().GetComponent<Button>().Select();
                isOnMenu = false;
            }
        }

        if(Input.GetKeyDown(InputKeys.PARAMETERS_KEY))
        {
            lifeExpController.ReadHPandEXP();
            //ReadText("Faltam " + matches + " pares. E você tem mais " + (tries - miss) + " tentativas restantes");
            ReadText("Tentativas " + attempts + " de 3");
        }

        if(Input.GetKeyDown(KeyCode.F6))
        {
            getFirstCard().GetComponent<Button>().Select();
        }

        if (c != null && c.Count >= 2)
        {
            Card.DO_NOT = true;
            cancelarButton.interactable = true;
            confirmarButton.interactable = true;
        }

        if (Input.GetKeyDown(InputKeys.INSTRUCTIONS_KEY))
        {
            if (!instructionInterface.gameObject.activeSelf)
            {
                instructionInterface.gameObject.SetActive(true);
                instructionInterface.ReadInstructions();
                instructionInterface.gameObject.GetComponentInChildren<Button>().Select();
            }
        }

        if (Input.GetKeyDown(InputKeys.AUDIODESCRICAO_KEY))
        {
            switch (missionName)
            {
                case "baleias":
                    ReadText(ReadableTexts.instance.GetReadableText(ReadableTexts.key_m004_memoria, LocalizationManager.instance.GetLozalization()));
                break;
                case "paleo":
                    ReadText(ReadableTexts.instance.GetReadableText(ReadableTexts.key_m009_memoria, LocalizationManager.instance.GetLozalization()));
                    break;
                default:
                    break;
            }    
        }

        if (Input.GetKeyDown(InputKeys.DICAS_KEY))
        {
            dicas.ShowHint();
        }
    }

    public void initializeGame()
    {
        if (!init)
            initializeCards();
        
        // start afther dicas.time seconds and repeat at dicas.repeatRate rate
        //InvokeRepeating("CallHintMethod", dicas.time, dicas.repeatRate);
        
        StartCoroutine(showCards());

        resetButton.interactable = true;
        backButton.interactable = true;
    }

    //public void CallHintMethod()
    //{
    //    dicas.StartHints();
    //}

    public void initializeCards()
    {
        // first half cards with images
        for (int i = 1; i < (cards.Length / 2 + 1); i++)
        {
            bool test = false;
            int choice = 0;
            while (!test)
            {
                choice = UnityEngine.Random.Range(0, cards.Length);
                test = !(cards[choice].GetComponent<Card>().initialized);
            }

            cards[choice].GetComponent<Card>().cardValue = i;
            cards[choice].GetComponent<Card>().initialized = true;
            
            cards[choice].GetComponent<Card>().setupGraphics(CARDFACE);

        }

        // last half images with text
        for (int i = 1; i < (cards.Length / 2 + 1); i++)
        {
            bool test = false;
            int choice = 0;
            while (!test)
            {
                choice = UnityEngine.Random.Range(0, cards.Length);
                test = !(cards[choice].GetComponent<Card>().initialized);
            }

            cards[choice].GetComponent<Card>().cardValue = i;
            cards[choice].GetComponent<Card>().initialized = true;

            cards[choice].GetComponent<Card>().setupGraphics(CARDTEXT);
        }


        if (!init)
        {
            init = true;
            cards[0].GetComponent<Button>().Select();
            //StartCoroutine(ReadCards());
        }
    }

    public GameObject getFirstCard()
    {
        return cards[0];
    }

    public Sprite getCardBack()
    {
        return cardBack;
    }

    public Sprite getCardFace(int i)
    {
        return Parameters.MEMORY_ROUNDINDEX == 0 ? cardFace0[i-1] : cardFace1[i-1];
    }

    public Sprite getCardText(int i)
    {
        return Parameters.MEMORY_ROUNDINDEX == 0 ? cardText0[i-1] : cardText1[i-1];
    }

    void checkCards()
    {
        c = new List<int>();

        for (int i = 0; i < cards.Length; i++)
        {
            if (cards[i].GetComponent<Card>().state == Card.VIRADA_CIMA && !_first)
            {
                
                //Debug.Log("carta adicionada >> " + cards[i]);
                c.Add(i);
                audioSource.PlayOneShot(selectAudio);
                //Debug.Log("após adicionar carta >> " + c.Count);

                // salva o indice da primeira carta que foi selecionada
                if (indexOfFirstCardAdded == -1)
                    indexOfFirstCardAdded = c[0];

                Debug.Log(indexOfFirstCardAdded);

                if (c.Count == 2)
                {
                    // primeira carta imagem e segunda carta imagem = nao pode
                    // primeira carta texto e segunda carta texto = nao pode
                    // desvira a ultima carta selecionada
                    if ((!cards[c[0]].GetComponent<Card>().isText && !cards[c[1]].GetComponent<Card>().isText) ||
                        (cards[c[0]].GetComponent<Card>().isText && cards[c[1]].GetComponent<Card>().isText))
                    {
                        //Debug.Log(c[0] + " >>> " + cards[c[0]]);
                        //Debug.Log(c[1] + " >>> " + cards[c[1]]);

                        // sao adicionados os indices das cartas. Entao se o indice da segunda carta for menor, ele é adicionado antes
                        // entao é preciso fazer essa verificacao pra remover sempre a ultima que foi selecionada.
                        if (c[0] == indexOfFirstCardAdded)
                        {
                            cards[c[1]].GetComponent<Card>().state = Card.VIRADA_BAIXO;
                            cards[c[1]].GetComponent<Card>().turnCardDown();
                            c.Remove(c[1]);
                        }
                        else
                        {
                            cards[c[0]].GetComponent<Card>().state = Card.VIRADA_BAIXO;
                            cards[c[0]].GetComponent<Card>().turnCardDown();
                            c.Remove(c[0]);
                        }
                    //c.RemoveAt(indexOfFirstCardAdded);
                    }
                    else
                    {

                        Debug.Log("As duas cartas foram selecionadas. Confirme para checar se a combinação está correta ou cancele para combinar " +
                        "outras cartas.");
                        ReadText("As duas cartas foram selecionadas. Confirme para checar se a combinação está correta ou cancele para combinar " +
                            "outras cartas.");

                        StartCoroutine(ChangeBGColor(cards[c[0]].GetComponent<Card>().BGImage, (int)Operation.confirm));
                        StartCoroutine(ChangeBGColor(cards[c[1]].GetComponent<Card>().BGImage, (int)Operation.confirm));

                        if (c[0] > c[1])
                            lastCardSelected = cards[c[0]];
                        else
                            lastCardSelected = cards[c[1]];

                        confirmarButton.Select();

                        indexOfFirstCardAdded = -1;
                    }

                    //// block the comparison of two text cards
                    //if (cards[c[0]].GetComponent<Card>().isText && cards[c[1]].GetComponent<Card>().isText)
                    //{
                    //    Debug.Log("clear....");
                    //    cards[c[i]].GetComponent<Card>().state = Card.VIRADA_BAIXO;
                    //    c.Clear();
                    //}
                    //else
                    //{
                    //}
                }
            }
        }

        Debug.Log("Após checar cartas >> " + c.Count);

        //if (c.Count == 2)
        //    cardComparison(c);
    }

    public void CompareCards()
    {
        cardComparison(c);
        cancelarButton.interactable = false;
        confirmarButton.interactable = false;
    }

    public void Cancel()
    {
        StartCoroutine(ChangeBGColor(cards[c[0]].GetComponent<Card>().BGImage, -1));
        StartCoroutine(ChangeBGColor(cards[c[1]].GetComponent<Card>().BGImage, -1));

        for (int i = 0; i < c.Count; i++)
        {
            cards[c[i]].GetComponent<Card>().state = Card.VIRADA_BAIXO;
            cards[c[i]].GetComponent<Card>().turnCardDown();
        }

        c.Clear();
        Debug.Log("Depois de limpar a lista >> " +  c.Count);

        cancelarButton.interactable = false;
        confirmarButton.interactable = false;

        SelectLastCard();
    }

    void cardComparison(List<int> c)
    {
        Card.DO_NOT = true;

        int x = 0;

        if(cards[c[0]].GetComponent<Card>().cardValue == cards[c[1]].GetComponent<Card>().cardValue)
        {
            audioSource.PlayOneShot(correctAudio);

            StartCoroutine(ChangeBGColor(cards[c[0]].GetComponent<Card>().BGImage, (int)Operation.correct));
            StartCoroutine(ChangeBGColor(cards[c[1]].GetComponent<Card>().BGImage, (int)Operation.correct));

            x = 2;
            matches--;
            //matchesText.text = "Pares restantes: " + matches;
            if (matches == 0)
            {
                switch (missionName)
                    {
                        case "baleias":
                            PlayerPreferences.M004_Memoria = true;
                            break;
                        case "paleo":
                            PlayerPreferences.M009_Memoria = true;
                            break;
                    }
                StartCoroutine(EndGame(true));
            }
        }
        else
        {
            lifeExpController.AddEXP(PlayerPreferences.XPwrongTry);

            audioSource.PlayOneShot(wrongAudio);

            StartCoroutine(ChangeBGColor(cards[c[0]].GetComponent<Card>().BGImage, (int)Operation.wrong));
            StartCoroutine(ChangeBGColor(cards[c[1]].GetComponent<Card>().BGImage, (int)Operation.wrong));

            miss++;
            //missText.text = "Tentativas incorretas: " + miss;
            attempts.text = "Tentativas " + miss + "/3";
            ReadText(attempts.text);

            if(miss >= tries)
            {
                StartCoroutine(EndGame(false));
                // 0 or 1
                Parameters.MEMORY_ROUNDINDEX = (Parameters.MEMORY_ROUNDINDEX + 1) % 2;
            }
        }

        for(int i = 0; i<c.Count; i++)
        {
            cards[c[i]].GetComponent<Card>().state = x;
            cards[c[i]].GetComponent<Card>().falseCheck();
        }

        c.Clear();

        // select the next card that does not have a match yet
        //if (matches != 0) cards[FindIndexNextCard()].GetComponent<Button>().Select();

        if (matches != 0)
        {
            SelectLastCard();
        }
    }

    public IEnumerator EndGame(bool win)
    {
        switch (missionName)
        {
            case "baleias":
                dicas.SupressDicas();
                if (win)
                {
                    WinImage.SetActive(true);
                    //WinImage.GetComponentInChildren<Button>().Select();

                    if (!PlayerPreferences.M004_TeiaAlimentar)
                    {
                        WinText.text = "Parabéns, você ganhou um dos itens necessário para sua aventura na antártica: Câmera fotográfica";

                        audioSource.PlayOneShot(victoryAudio);
                        yield return new WaitWhile(() => audioSource.isPlaying);

                        ReadText(WinText.text);
                    }
                    else
                    {
                        WinText.text = "Parabéns! Você ganhou a câmera fotográfica. Agora você pode fotografar caudas de baleias jubarte e " +
                            "contribuir com as pesquisas da Ciência Cidadã.";

                        audioSource.PlayOneShot(victoryAudio);
                        yield return new WaitWhile(() => audioSource.isPlaying);

                        ReadText(WinText.text);
                    }

                    ReadText(ReadableTexts.instance.GetReadableText(ReadableTexts.key_m004_memoria_vitoria, LocalizationManager.instance.GetLozalization()));

                    lifeExpController.AddEXP(PlayerPreferences.XPlosePuzzle); // finalizou o minijogo
                    lifeExpController.AddEXP(PlayerPreferences.XPwinItem); // ganhou o item
                }
                else
                {
                    LoseImage.SetActive(true);


                    audioSource.PlayOneShot(loseAudio);

                    yield return new WaitWhile(() => audioSource.isPlaying);

                    ReadText("Infelizmente você não conseguiu finalizar o minijogo com êxito. Tente novamente.");

                    resetButton.Select();
                    lifeExpController.AddEXP(PlayerPreferences.XPlosePuzzle); // jogou um minijogo

                    ReadText(ReadableTexts.instance.GetReadableText(ReadableTexts.key_m004_memoria_derrota, LocalizationManager.instance.GetLozalization()));
                }

                StartCoroutine(ReturnToShipCoroutine()); // volta para o navio perdendo ou ganhando o minijogo
                break;

            case "paleo":
                if (win)
                {
                    WinImage.SetActive(true);
                    //WinImage.GetComponentInChildren<Button>().Select();

                    if (!PlayerPreferences.M009_Memoria)
                    {
                        WinText.text = "Parabéns!! Você ganhou o kit de primeiros socorros, mas ainda falta conquistar outros itens.";

                        audioSource.PlayOneShot(victoryAudio);
                        yield return new WaitWhile(() => audioSource.isPlaying);

                        ReadText(WinText.text);
                    }
                    else
                    {//MUDAARR
                        WinText.text = "Parabéns! Você ganhou o kit de primeiros socorros. Agora você que conquistar outros itens.";

                        audioSource.PlayOneShot(victoryAudio);
                        yield return new WaitWhile(() => audioSource.isPlaying);

                        ReadText(WinText.text);
                    }
                    ReadText(ReadableTexts.instance.GetReadableText(ReadableTexts.key_m009_memoria_vitoria, LocalizationManager.instance.GetLozalization()));

                    lifeExpController.AddEXP(0.001f); // finalizou o minijogo
                    lifeExpController.AddEXP(0.0002f); // ganhou o item
                }
                else
                {
                    LoseImage.SetActive(true);
                    //MUDAARR

                    audioSource.PlayOneShot(loseAudio);

                    yield return new WaitWhile(() => audioSource.isPlaying);

                    ReadText("Infelizmente você não conseguiu finalizar o minijogo com êxito. Tente novamente.");
                    resetButton.Select();
                    lifeExpController.AddEXP(0.0001f); // jogou um minijogo

                    ReadText(ReadableTexts.instance.GetReadableText(ReadableTexts.key_m009_memoria_derrota, LocalizationManager.instance.GetLozalization()));
                }

                StartCoroutine(ReturnToShipCoroutine()); // volta para o navio perdendo ou ganhando o minijogo
                break;
            default:
                break;
        } 
    }

    public void TryReturnToShip()
    {
        confirmQuit.SetActive(true);

        ReadText(ReadableTexts.instance.GetReadableText(ReadableTexts.key_gameplay_aviso_botoes, LocalizationManager.instance.GetLozalization()));

        audioSource.PlayOneShot(avisoClip);

        ReadText(confirmQuit.GetComponentInChildren<TMPro.TextMeshProUGUI>().text);
        confirmQuit.GetComponentInChildren<Button>().Select();
    }

    public void ReturnToShip()
    {
        switch (missionName)
        {
            case "baleias":
                confirmQuit.SetActive(false);
                if (!PlayerPreferences.M004_Memoria) lifeExpController.RemoveEXP(0.0001f); // saiu sem concluir o minijogo
                UnityEngine.SceneManagement.SceneManager.LoadScene(ScenesNames.M004Ship);
            break;
            case "paleo":
                //confirmQuit.SetActive(false);
                if (!PlayerPreferences.M009_Memoria) lifeExpController.RemoveEXP(0.0001f); // saiu sem concluir o minijogo
                UnityEngine.SceneManagement.SceneManager.LoadScene(ScenesNames.M009Camp);
                break;
            default:    
                break;
        }        
    }

    public void ResetScene()
    {
        switch (missionName)
        {
            case "baleias":
                UnityEngine.SceneManagement.SceneManager.LoadScene(ScenesNames.M004MemoryGame);
                break;
            case "paleo":
                UnityEngine.SceneManagement.SceneManager.LoadScene(ScenesNames.M009MemoryGame);
                break;
            default:
                break;
        } 
        
    }

    public IEnumerator ReadCards()
    {
        GameObject[] tmpCards = cards;
        
        // ordenar o array baseado no numero da carta (posicao)
        Array.Sort(tmpCards, delegate (GameObject g1, GameObject g2)
        {
            return int.Parse(new string(g1.name.Where(char.IsDigit).ToArray())).CompareTo(
                int.Parse(new string(g2.name.Where(char.IsDigit).ToArray())));
            //return string.Join(string.Empty, Regex.Matches(g1.gameObject.name, @"\d+").OfType<Match>().Select(m => m.Value)).CompareTo(
            //    string.Join(string.Empty, Regex.Matches(g2.gameObject.name, @"\d+").OfType<Match>().Select(m => m.Value)));
        });

        // imprime e le o conteudo a cada meio segundo (tempo que as cartas ficarão abertas no início)
        for (int i = 0; i < tmpCards.Length; i++)
        {
            string objectName = CardsDescription.GetCardText(tmpCards[i].name);
            //Debug.Log(objectName != null ? (tmpCards[i].name.Substring(0, tmpCards[i].name.IndexOf(":")) + ": " + objectName) : tmpCards[i].gameObject.name);

            //tmpCards[i].GetComponent<Button>().Select();
            yield return new WaitForSeconds(0.5f);
        }
    }

    public IEnumerator ChangeBGColor(Image image, int op)
    {
        //Debug.Log("ChangeBGColor");

        Color color;

        switch (op)
        {
            case (int)Operation.confirm:
                color = new Color(1, 1, 0, 1); // yellow
                break;
            case (int)Operation.correct:
                color = new Color(0, 0.3921569f, 0, 1); // green
                //color = new Color(0, 0, 0, 0); // transparent
                break;
            case (int)Operation.wrong:
                color = new Color(1, 0, 0, 1); // red
                break;
            default:
                color = new Color(0, 0, 0, 0);
                break;
        }
            image.color = color;

        // setting alpha color to 1 and bg color to green
        //var tempColor = image.color;
        //tempColor.a = 1f;
        //image.color = tempColor;


        
        if (op != (int)Operation.confirm && op != (int)Operation.correct)
        {
            // wait seconds
            yield return new WaitForSeconds(2f);

            // back to normal!!
            image.color = new Color(1, 1, 1, 0);
        }
    }

    public bool ContainJustText(List<Card> list)
    {
        foreach(Card c in list)
        {
            if (!c.isText)
                return false;
        }

        return true;
    }

    public void SelectLastCard()
    {
        if (lastCardSelected != null) 
            lastCardSelected.GetComponent<Button>().Select();
        else
            cards[FindIndexNextCard()].GetComponent<Button>().Select();
    }

    // find the index of next card that does not have a match
    public int FindIndexNextCard()
    {
        int index = 0;

        // state == 2 is when card have a match
        while (cards[index].GetComponent<Card>().state == 2)
        {
            index++;
        }

        return index;
    }

    public IEnumerator showCards()
    {
        for (int i = 0; i < cards.Length; i++)
        {
            //if (!cards[i].GetComponent<Card>().isText)
            cards[i].GetComponent<Card>().state = Card.VIRADA_BAIXO;

            cards[i].GetComponent<Card>().turnCardDown();
        }

        yield return new WaitForSeconds(0.5f);

        _first = false;
    }

    public IEnumerator ReturnToShipCoroutine()
    {
        switch (missionName)
        {
            case "baleias":
                yield return new WaitForSeconds(4f);
                UnityEngine.SceneManagement.SceneManager.LoadScene(ScenesNames.M004Ship);
                break;
            case "paleo":
                yield return new WaitForSeconds(4f);
                UnityEngine.SceneManagement.SceneManager.LoadScene(ScenesNames.M009Camp);
                break;
            default:
                break;
        } 
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class MemoryManagerPaleo : AbstractScreenReader {

    private readonly string instructions = "Início do jogo. Mini jogo de memória. Descrição..";
    
    // round 0
    public Sprite[] cardFace0;
    public Sprite[] cardText0;

    // round 1
    //public Sprite[] cardFace1;
    //public Sprite[] cardText1;

    public Sprite cardBack;

    public GameObject[] cards;

    public Button confirmarButton;
    public Button cancelarButton;
    public Button backButton;
    public Button resetButton;
    public Button audioButton;

    public int[] index; 
    private int matches = 6;
    private int miss = 0;

    private bool init;

    public static int CARDFACE = 1;
    public static int CARDTEXT = 2;

    public TMPro.TextMeshProUGUI missText;
    public TMPro.TextMeshProUGUI matchesText;

    private AudioSource audioSource;

    public AudioClip correctAudio;
    public AudioClip wrongAudio;

    private List<int> c;

    public GameObject WinImage;
    public TMPro.TextMeshProUGUI WinText;
    public GameObject LoseImage;

    public GameObject BigImage1;
    public GameObject BigImage2;

    // hint settings
    public MinijogosDicas dicas;

    public LifeExpController lifeExpController;

    public GameObject instructionInterface;

    private bool _first;

    private enum Operation { correct, confirm, wrong }

    public string missionName;
    
    private void Start()
    {
        backButton.interactable = false;
        resetButton.interactable = false;

        init = false;
        _first = true;        

        //Debug.Log(Parameters.MEMORY_ROUNDINDEX);

        ReadText(instructions);

        audioSource = GetComponent<AudioSource>();

        if (instructionInterface.activeSelf)
            instructionInterface.GetComponentInChildren<Button>().Select();
    }

    // Update is called once per frame
    void Update () {
        
        if (!_first && (Input.GetMouseButtonUp(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)) && !Card.DO_NOT)
        {
            Debug.Log("Checando cartas....");
            checkCards();
        }

        if(Input.GetKeyDown(KeyCode.P))
        {
            audioButton.Select();
        }

        if(Input.GetKeyDown(KeyCode.F6))
        {
            cards[0].GetComponent<Button>().Select();
        }

        if (c != null && c.Count >= 2)
        {
            Card.DO_NOT = true;
            cancelarButton.interactable = true;
            confirmarButton.interactable = true;
            Debug.Log(c.Count);
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (!instructionInterface.activeSelf)
            {
                instructionInterface.SetActive(true);
                instructionInterface.GetComponentInChildren<Button>().Select();
            }
            else
                instructionInterface.SetActive(false);
        }

        //else
        //{
        //    confirmarButton.interactable = false;
        //    cancelarButton.interactable = false;

        //}

        //Debug.Log(Card.DO_NOT);
    }

    public void initializeGame()
    {
        if (!init)
            initializeCards();
        
        // start afther dicas.time seconds and repeat at dicas.repeatRate rate
        InvokeRepeating("CallHintMethod", dicas.time, dicas.repeatRate);
        
        StartCoroutine(showCards());

        backButton.interactable = true;
        resetButton.interactable = true;

    }

    //public void CallHintMethod()
    //{
    //    dicas.StartHints();
    //}

    public void initializeCards()
    {
        // first 9 cards with images
        for (int i = 1; i < 7; i++)
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


            //Debug.Log(choice);
            
            cards[choice].GetComponent<Card>().setupGraphics(CARDFACE);

        }

        // last 9 images with text
        for (int i = 1; i < 7; i++)
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

            //cards[choice].gameObject.name = getCardText(choice).name;

            //Debug.Log(choice);

            cards[choice].GetComponent<Card>().setupGraphics(CARDTEXT);
        }


        if (!init)
        {
            init = true;
            cards[0].GetComponent<Button>().Select();
            StartCoroutine(ReadCards());
        }
    }

    public Sprite getCardBack()
    {
        return cardBack;
    }

    public Sprite getCardFace(int i)
    {
        return Parameters.MEMORY_ROUNDINDEX == 0 ? cardFace0[i-1] : null;
    }

    public Sprite getCardText(int i)
    {
        return Parameters.MEMORY_ROUNDINDEX == 0 ? cardText0[i - 1] : null;
    }

    void checkCards()
    {
        c = new List<int>();

        for (int i = 0; i < cards.Length; i++)
        {
            if (cards[i].GetComponent<Card>().state == Card.VIRADA_CIMA && !_first)
            {
                Debug.Log("carta adicionada >> " + cards[i]);
                c.Add(i);
                Debug.Log("após adicionar carta >> " + c.Count);
                
                if (c.Count == 2)
                {
                    // block the comparison of two text cards
                    //if (cards[c[0]].GetComponent<Card>().isText && cards[c[1]].GetComponent<Card>().isText)
                    //{
                    //    Debug.Log("clear....");
                    //    cards[c[i]].GetComponent<Card>().state = Card.VIRADA_BAIXO;
                    //    c.Clear();
                    //}
                    //else
                    //{
                        //BigImage1.SetActive(true);
                        //BigImage1.GetComponentInChildren<Image>().sprite = cards[c[0]].GetComponent<Card>().cardFace ?? cards[c[0]].GetComponent<Card>().cardText;
                        //BigImage2.SetActive(true);
                        //BigImage2.GetComponentInChildren<Image>().sprite = cards[c[1]].GetComponent<Card>().cardFace ?? cards[c[1]].GetComponent<Card>().cardText;

                        StartCoroutine(ChangeBGColor(cards[c[0]].GetComponent<Card>().BGImage, (int)Operation.confirm));
                        StartCoroutine(ChangeBGColor(cards[c[1]].GetComponent<Card>().BGImage, (int)Operation.confirm));

                        confirmarButton.Select();
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
        //BigImage1.SetActive(false);
        //BigImage2.SetActive(false);

        cancelarButton.interactable = false;
        confirmarButton.interactable = false;
    }

    public void Cancel()
    {
        //BigImage1.SetActive(false);
        //BigImage2.SetActive(false);

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
        cards[0].GetComponent<Button>().Select();
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
            matchesText.text = "Pares restantes: " + matches;
            if (matches == 0)
            {
                Debug.Log("Fim de jogo! Você conseguiu terminar com sucesso. Volte ao navio para novas aventuras.");
                ReadText("Fim de jogo! Você conseguiu terminar com sucesso. Volte ao navio para novas aventuras.");
                PlayerPreferences.M009_Memoria = true;
                EndGame(true);
            }
        }
        else
        {
            audioSource.PlayOneShot(wrongAudio);

            StartCoroutine(ChangeBGColor(cards[c[0]].GetComponent<Card>().BGImage, (int)Operation.wrong));
            StartCoroutine(ChangeBGColor(cards[c[1]].GetComponent<Card>().BGImage, (int)Operation.wrong));

            miss++;
            missText.text = "Tentativas incorretas: " + miss;

            if(miss >= 3)
            {
                Debug.Log("Fim de jogo! Você não conseguiu concluir o objetivo. Tente novamente.");
                ReadText("Fim de jogo! Você não conseguiu concluir o objetivo. Tente novamente.");
                EndGame(false);
                // 0 or 1
                //Parameters.MEMORY_ROUNDINDEX = (Parameters.MEMORY_ROUNDINDEX + 1) % 2;
            }
        }

        for(int i = 0; i<c.Count; i++)
        {
            cards[c[i]].GetComponent<Card>().state = x;
            cards[c[i]].GetComponent<Card>().falseCheck();
        }

        c.Clear();
        cards[0].GetComponent<Button>().Select();
    }

    public void EndGame(bool win)
    {
        BigImage1.SetActive(false);
        BigImage2.SetActive(false);

        if (win)
        {
            WinImage.SetActive(true);
            //WinImage.GetComponentInChildren<Button>().Select();

            if (!PlayerPreferences.M004_TeiaAlimentar)
                WinText.text = "Parabéns, você ganhou a câmera fotográfica para realizar a missão, mas ainda falta um item.";
            else
                WinText.text = "Parabéns, você ganhou a câmera fotográfica. Agora você já tem os itens necessários para realizar a missão.";

            lifeExpController.AddEXP(0.001f); // finalizou o minijogo
            lifeExpController.AddEXP(0.0002f); // ganhou o item
        }
        else
        {
            LoseImage.SetActive(true);
            resetButton.Select();
            lifeExpController.AddEXP(0.0001f); // jogou um minijogo
        }

        StartCoroutine(ReturnToShipCoroutine()); // volta para o navio perdendo ou ganhando o minijogo
    }

    public void ReturnToShip()
    {
        if (!PlayerPreferences.M009_Memoria) lifeExpController.RemoveEXP(0.0001f); // saiu sem concluir o minijogo
        UnityEngine.SceneManagement.SceneManager.LoadScene(ScenesNames.M009Camp);
    }

    public void ResetScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(ScenesNames.M009MemoryGame);
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
            string objectName;

            switch (missionName)
            {
                case "baleias":
                    objectName = CardsDescription.GetCardText(tmpCards[i].name);
                    break;
                case "paleo":
                    objectName = CardsDescriptionPaleo.GetCardDescriptionPaleo(tmpCards[i].name);
                    break;
                default:
                    objectName = "";
                    Debug.Log("check mission name");
                    break;
            }

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
                color = new Color(0, 1, 0, 1); // green
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

    public IEnumerator showCards()
    {
        for (int i = 0; i < cards.Length; i++)
        {
            //if (!cards[i].GetComponent<Card>().isText)
            cards[i].GetComponent<Card>().state = Card.VIRADA_BAIXO;

            cards[i].GetComponent<Card>().turnCardDown();
        }

        yield return new WaitForSeconds(4);

        _first = false;
    }

    public IEnumerator ReturnToShipCoroutine()
    {
        yield return new WaitForSeconds(7f);

        UnityEngine.SceneManagement.SceneManager.LoadScene(ScenesNames.M004Ship);
    }
}

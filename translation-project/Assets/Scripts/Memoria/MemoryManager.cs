using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemoryManager : AbstractScreenReader {

    private readonly string instructions = "Início do jogo. Mini jogo de memória. Descrição..";

    public Sprite[] cardFace;
    public Sprite[] cardText;
    public Sprite cardBack;

    public GameObject[] cards;

    public Button confirmarButton;
    public Button cancelarButton;

    public int[] index; 
    private int matches = 9;
    private int miss = 0;

    private bool init;

    public static int CARDFACE = 1;
    public static int CARDTEXT = 2;

    public Text missText;
    public Text matchesText;

    private AudioSource audioSource;

    public AudioClip correctAudio;
    public AudioClip wrongAudio;

    private List<int> c;

    public GameObject WinImage;
    public GameObject LoseImage;

    public GameObject BigImage1;
    public GameObject BigImage2;

    // hint settings
    public Minijogos_dicas dicas;


    private void Start()
    {
        init = false;

        ReadText(instructions);

        audioSource = GetComponent<AudioSource>();
        
        // start afther dicas.time seconds and repeat at dicas.repeatRate rate
        InvokeRepeating("CallHintMethod", dicas.time, dicas.repeatRate);
    }

    // Update is called once per frame
    void Update () {
        if (!init)
            initializeCards();

        if ((Input.GetMouseButtonUp(0) || Input.GetKeyDown(KeyCode.Return)) && !Card.DO_NOT)
        {
            checkCards();
        }

        if (c != null && c.Count >= 2)
        {
            Card.DO_NOT = true;
            confirmarButton.interactable = true;
            cancelarButton.interactable = true;

            //Debug.Log(c.Count);
        }
        else
        {
            confirmarButton.interactable = false;
            cancelarButton.interactable = false;
        }

        //Debug.Log(Card.DO_NOT);
    }

    public void CallHintMethod()
    {
        dicas.StartHints();
    }

    public void initializeCards()
    {
        // first 9 cards with images
        for (int i = 1; i < 10; i++)
        {
            bool test = false;
            int choice = 0;
            while (!test)
            {
                choice = Random.Range(0, cards.Length);
                test = !(cards[choice].GetComponent<Card>().initialized);
            }

            cards[choice].GetComponent<Card>().cardValue = i;
            cards[choice].GetComponent<Card>().initialized = true;


            //Debug.Log(choice);
            
            cards[choice].GetComponent<Card>().setupGraphics(CARDFACE);

        }

        // last 9 images with text
        for (int i = 1; i < 10; i++)
        {
            bool test = false;
            int choice = 0;
            while (!test)
            {
                choice = Random.Range(0, cards.Length);
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
        }
    }

    public Sprite getCardBack()
    {
        return cardBack;
    }

    public Sprite getCardFace(int i)
    {
        return cardFace[i-1];
    }

    public Sprite getCardText(int i)
    {
        return cardText[i-1];
    }

    void checkCards()
    {
        c = new List<int>();

        for (int i = 0; i < cards.Length; i++)
        {
            if (cards[i].GetComponent<Card>().state == 1)
            {
                c.Add(i);
                
                if (c.Count == 2)
                {
                    BigImage1.SetActive(true);
                    BigImage1.GetComponentInChildren<Image>().sprite = cards[c[0]].GetComponent<Card>().cardFace ?? cards[c[0]].GetComponent<Card>().cardText;
                    BigImage2.SetActive(true);
                    BigImage2.GetComponentInChildren<Image>().sprite = cards[c[1]].GetComponent<Card>().cardFace ?? cards[c[1]].GetComponent<Card>().cardText;

                    confirmarButton.Select();
                }
            }
        }

        //Debug.Log(c.Count);
        //if (c.Count == 2)
        //    cardComparison(c);
    }

    public void CompareCards()
    {
        cardComparison(c);
        BigImage1.SetActive(false);
        BigImage2.SetActive(false);
    }

    public void Cancel()
    {
        BigImage1.SetActive(false);
        BigImage2.SetActive(false);

        for (int i = 0; i < c.Count; i++)
        {
            cards[c[i]].GetComponent<Card>().state = 0;
            cards[c[i]].GetComponent<Card>().turnCardDown();
        }

        c.Clear();
        Debug.Log(c.Count);

        cards[0].GetComponent<Button>().Select();
    }

    void cardComparison(List<int> c)
    {
        Card.DO_NOT = true;

        int x = 0;

        if(cards[c[0]].GetComponent<Card>().cardValue == cards[c[1]].GetComponent<Card>().cardValue)
        {
            audioSource.PlayOneShot(correctAudio);

            x = 2;
            matches--;
            matchesText.text = "Pares restantes: " + matches;
            if (matches == 0)
            {
                Debug.Log("Fim de jogo! Você conseguiu terminar com sucesso.");
                PlayerPreferences.M004_Memoria = true;
                EndGame(true);
            }
        }
        else
        {
            audioSource.PlayOneShot(wrongAudio);

            miss++;
            missText.text = "Tentativas incorretas: " + miss;

            if(miss >= 3)
            {
                Debug.Log("Fim de jogo! Você não conseguiu concluir o objetivo. Tente novamente.");
                EndGame(false);
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
        if (win)
            WinImage.SetActive(true);
        else
            LoseImage.SetActive(true);
    }

    public void ReturnToShip()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("ShipScene");
    }

    public void ResetScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MemoryGameScene");
    }
}

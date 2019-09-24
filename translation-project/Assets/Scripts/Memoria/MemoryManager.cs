using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class MemoryManager : AbstractScreenReader {

    private readonly string instructions = "Início do jogo. Mini jogo de memória. Descrição..";
    
    // round 0
    public Sprite[] cardFace0;
    public Sprite[] cardText0;
    // round 1
    public Sprite[] cardFace1;
    public Sprite[] cardText1;

    public Sprite cardBack;

    public GameObject[] cards;

    public Button confirmarButton;
    public Button cancelarButton;
    public Button resetButton;
    public Button audioButton;

    public int[] index; 
    private int matches = 9;
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
    public GameObject LoseImage;

    public GameObject BigImage1;
    public GameObject BigImage2;

    // hint settings
    public Minijogos_dicas dicas;

    public LifeExpController lifeExpController;


    private void Start()
    {
        init = false;

        Debug.Log(Parameters.MEMORY_ROUNDINDEX);

        ReadText(instructions);

        audioSource = GetComponent<AudioSource>();
        
        // start afther dicas.time seconds and repeat at dicas.repeatRate rate
        InvokeRepeating("CallHintMethod", dicas.time, dicas.repeatRate);
    }

    // Update is called once per frame
    void Update () {
        if (!init)
            initializeCards();

        if ((Input.GetMouseButtonUp(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)) && !Card.DO_NOT)
        {
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
            confirmarButton.interactable = true;
            cancelarButton.interactable = true;

            //Debug.Log(c.Count);
        }
        //else
        //{
        //    confirmarButton.interactable = false;
        //    cancelarButton.interactable = false;
        
        //}

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
                choice = UnityEngine.Random.Range(0, cards.Length);
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

        cancelarButton.interactable = false;
        confirmarButton.interactable = false;
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

            StartCoroutine(ChangeBGColor(cards[c[0]].GetComponent<Card>().BGImage));
            StartCoroutine(ChangeBGColor(cards[c[1]].GetComponent<Card>().BGImage));

            x = 2;
            matches--;
            matchesText.text = "Pares restantes: " + matches;
            if (matches == 0)
            {
                Debug.Log("Fim de jogo! Você conseguiu terminar com sucesso. Volte ao navio para novas aventuras.");
                ReadText("Fim de jogo! Você conseguiu terminar com sucesso. Volte ao navio para novas aventuras.");
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
                ReadText("Fim de jogo! Você não conseguiu concluir o objetivo. Tente novamente.");
                EndGame(false);
            }

            // 0 or 1
            Parameters.MEMORY_ROUNDINDEX = (Parameters.MEMORY_ROUNDINDEX + 1) % 2;
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

            lifeExpController.AddEXP(0.001f); // finalizou o minijogo
            lifeExpController.AddEXP(0.0002f); // ganhou o item
        }
        else
        {
            LoseImage.SetActive(true);
            resetButton.Select();
            lifeExpController.AddEXP(0.0001f); // jogou um minijogo
        }
    }

    public void ReturnToShip()
    {
        if (!PlayerPreferences.M004_Memoria) lifeExpController.RemoveEXP(0.0001f); // saiu sem concluir o minijogo
        UnityEngine.SceneManagement.SceneManager.LoadScene(ScenesNames.M004Ship);
    }

    public void ResetScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(ScenesNames.M004MemoryGame);
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
            string objectName = CardsDescription.GetCardDescription(tmpCards[i].name);
            Debug.Log(objectName != null ? (tmpCards[i].name.Substring(0, tmpCards[i].name.IndexOf(":")) + ": " + objectName) : tmpCards[i].gameObject.name);

            //tmpCards[i].GetComponent<Button>().Select();
            yield return new WaitForSeconds(0.5f);
        }
    }

    public IEnumerator ChangeBGColor(Image image)
    {
        Debug.Log("ChangeBGColor");

        image.color = new Color(0,1,0,1);

        // setting alpha color to 1 and bg color to green
        //var tempColor = image.color;
        //tempColor.a = 1f;
        //image.color = tempColor;


        // wait seconds
        yield return new WaitForSeconds(3f);

        // back to normal!!
        image.color = new Color(1, 1, 1, 0);
    }
}

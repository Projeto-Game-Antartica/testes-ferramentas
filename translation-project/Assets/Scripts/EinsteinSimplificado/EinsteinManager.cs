using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class EinsteinManager : AbstractScreenReader
{
    private readonly string instructions = "Início do jogo. Mini jogo de memória. Descrição..";

    // round 0
    public Sprite[] cardFace;

    public Sprite cardBack;

    public GameObject[] cards;

    public Button backButton;
    public Button resetButton;
    public Button audioButton;
    public Button confirmarButton;
    public Button cancelButton;

    public int[] index;

    private bool init;

    private AudioSource audioSource;

    public AudioClip correctAudio;
    public AudioClip wrongAudio;

    private List<int> c;

    public GameObject WinImage;
    public TMPro.TextMeshProUGUI WinText;
    public GameObject LoseImage;

    public TMPro.TMP_Dropdown processDropDown;

    // hint settings
    public MinijogosDicas dicas;

    public LifeExpController lifeExpController;

    public GameObject instructionInterface;

    private enum Operation { correct, wrong }

    private enum DropDownColors { blue = 1, orange, yellow, green, grey }

    private Color lightBlue = new Color(0, 0.6f, 1);
    private Color lightOrange = new Color(1, 0.7f, 0.4f);
    private Color lightYellow = new Color(1, 1, 0.3f);
    private Color lightGreen = new Color(0.3f, 1, 0.4f);
    private Color grey = new Color(0.7f, 0.7f, 0.7f);

    private int remainingOptionsBlue = 4;
    private int remainingOptionsOrange = 4;
    private int remainingOptionsYellow = 4;
    private int remainingOptionsGreen = 4;
    private int remainingOptionsGrey = 4;

    private void Start()
    {
        backButton.interactable = false;
        resetButton.interactable = false;

        init = false;

        ReadText(instructions);

        audioSource = GetComponent<AudioSource>();

        //initializeGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (!init)
            initializeGame();

        if ((Input.GetMouseButtonUp(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)) && !EinsteinCard.DO_NOT)
        {
            //Debug.Log("Checando cartas....");
            checkCards();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            audioButton.Select();
        }

        if (Input.GetKeyDown(KeyCode.F6))
        {
            cards[0].GetComponent<Button>().Select();
        }

        if (c != null && c.Count >= GetRemainingOptions(GetDropDownValue()))
        {
            EinsteinCard.DO_NOT = true;
            confirmarButton.interactable = true;
            cancelButton.interactable = true;
            //Debug.Log(c.Count);
        }
        else
        {
            confirmarButton.interactable = false;
            cancelButton.interactable = false;
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (!instructionInterface.activeSelf)
                instructionInterface.SetActive(true);
            else
                instructionInterface.SetActive(false);
        }
    }

    public void initializeGame()
    {
        if (!init)
            initializeCards();

        // start afther dicas.time seconds and repeat at dicas.repeatRate rate
        InvokeRepeating("CallHintMethod", dicas.time, dicas.repeatRate);

        //StartCoroutine(showCards());

        backButton.interactable = true;
        resetButton.interactable = true;

    }

    public void CallHintMethod()
    {
        dicas.StartHints();
    }

    public void initializeCards()
    {
        for (int i = 1; i < cards.Length+1; i++)
        {
            bool test = false;
            int choice = 0;
            while (!test)
            {
                choice = UnityEngine.Random.Range(0, cards.Length);
                test = !(cards[choice].GetComponent<EinsteinCard>().initialized);
            }

            cards[choice].GetComponent<EinsteinCard>().cardValue = i;
            cards[choice].GetComponent<EinsteinCard>().initialized = true;


            //Debug.Log(choice);

            cards[choice].GetComponent<EinsteinCard>().setupGraphics();
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
        return cardFace[i - 1];
    }

    void checkCards()
    {
        c = new List<int>();

        for (int i = 0; i < cards.Length; i++)
        {
            if (cards[i].GetComponent<EinsteinCard>().state == EinsteinCard.VIRADA_CIMA)
            {
                Debug.Log("carta adicionada >> " + cards[i]);
                c.Add(i);
                //Debug.Log("após adicionar carta >> " + c.Count);

                cards[i].GetComponent<EinsteinCard>().BGImage.color = GetColor(GetDropDownValue());
            }
        }

        //Debug.Log("Após checar cartas >> " + c.Count);

        //if (c.Count == GetRemainingOptions(GetDropDownValue()))
        //    cardComparison(c);
    }

    public void CompareCards()
    {
        cardComparison(c);
    }

    public void Cancel()
    {
        //for (int i = 0; i < c.Count; i++)
        //{
        //    //cards[c[i]].GetComponent<EinsteinCard>().state = EinsteinCard.VIRADA_BAIXO;
        //    cards[c[i]].GetComponent<EinsteinCard>().BGImage.color = GetColor(-1);
        //    cards[c[i]].GetComponent<EinsteinCard>().turnCardDown();
        //}

        for (int i = 0; i < c.Count; i++)
        {
            cards[c[i]].GetComponent<EinsteinCard>().BGImage.color = GetColor(-1);
            cards[c[i]].GetComponent<EinsteinCard>().state = 0;
            cards[c[i]].GetComponent<EinsteinCard>().turnCardDown();
        }

        c.Clear();
        //Debug.Log("Depois de limpar a lista >> " + c.Count);
        
        cards[0].GetComponent<Button>().Select();
    }

    void cardComparison(List<int> c)
    {
        Card.DO_NOT = true;

        int x;

        switch(GetDropDownValue())
        {
            case (int)DropDownColors.blue:
                for (int i = 0; i < remainingOptionsBlue; i++)
                {
                    x = 0;
                    if (cards[c[i]].GetComponent<EinsteinCard>().name.Contains("azul"))
                    {
                        StartCoroutine(CheckAnswer(cards[c[i]].GetComponent<EinsteinCard>().BGImage, (int)Operation.correct));
                        x = 2;
                    }
                    else
                    {
                        StartCoroutine(CheckAnswer(cards[c[i]].GetComponent<EinsteinCard>().BGImage, (int)Operation.wrong));
                    }

                    cards[c[i]].GetComponent<EinsteinCard>().state = x;
                }

                break;

            case (int)DropDownColors.orange:
                for (int i = 0; i < remainingOptionsOrange; i++)
                {
                    x = 0;
                    if (cards[c[i]].GetComponent<EinsteinCard>().name.Contains("laranja"))
                    {
                        StartCoroutine(CheckAnswer(cards[c[i]].GetComponent<EinsteinCard>().BGImage, (int)Operation.correct));
                        x = 2;
                    }
                    else
                    {
                        StartCoroutine(CheckAnswer(cards[c[i]].GetComponent<EinsteinCard>().BGImage, (int)Operation.wrong));
                    }

                    cards[c[i]].GetComponent<EinsteinCard>().state = x;
                }

                break;

            case (int)DropDownColors.yellow:
                for (int i = 0; i < remainingOptionsYellow; i++)
                {
                    Debug.Log(cards[c[i]].name);
                    x = 0;
                    if (cards[c[i]].GetComponent<EinsteinCard>().name.Contains("amarelo"))
                    {
                        StartCoroutine(CheckAnswer(cards[c[i]].GetComponent<EinsteinCard>().BGImage, (int)Operation.correct));
                        x = 2;
                    }
                    else
                    {
                        StartCoroutine(CheckAnswer(cards[c[i]].GetComponent<EinsteinCard>().BGImage, (int)Operation.wrong));
                    }

                    cards[c[i]].GetComponent<EinsteinCard>().state = x;
                }

                break;

            case (int)DropDownColors.green:
                for (int i = 0; i < remainingOptionsGreen; i++)
                {
                    x = 0;
                    if (cards[c[i]].GetComponent<EinsteinCard>().name.Contains("verde"))
                    {
                        StartCoroutine(CheckAnswer(cards[c[i]].GetComponent<EinsteinCard>().BGImage, (int)Operation.correct));
                        x = 2;
                    }
                    else
                    {
                        StartCoroutine(CheckAnswer(cards[c[i]].GetComponent<EinsteinCard>().BGImage, (int)Operation.wrong));
                    }

                    cards[c[i]].GetComponent<EinsteinCard>().state = x;
                }

                break;

            case (int)DropDownColors.grey:
                for (int i = 0; i < remainingOptionsGrey; i++)
                {
                    x = 0;
                    if (cards[c[i]].GetComponent<EinsteinCard>().name.Contains("cinza"))
                    {
                        StartCoroutine(CheckAnswer(cards[c[i]].GetComponent<EinsteinCard>().BGImage, (int)Operation.correct));
                        x = 2;
                    }
                    else
                    {
                        StartCoroutine(CheckAnswer(cards[c[i]].GetComponent<EinsteinCard>().BGImage, (int)Operation.wrong));
                    }

                    cards[c[i]].GetComponent<EinsteinCard>().state = x;
                }

                break;

            default:
                for (int i = 0; i < c.Count; i++)
                {
                    x = 0;
                    StartCoroutine(CheckAnswer(cards[c[i]].GetComponent<EinsteinCard>().BGImage, (int)Operation.wrong));
                    cards[c[i]].GetComponent<EinsteinCard>().state = x;
                }

                break;
        }

        for (int i = 0; i < GetRemainingOptions(GetDropDownValue()); i++)
        {
            cards[c[i]].GetComponent<EinsteinCard>().falseCheck();
        }

        c.Clear();
        cards[0].GetComponent<Button>().Select();
    }

    public void EndGame(bool win)
    {
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
            //Debug.Log(objectName != null ? (tmpCards[i].name.Substring(0, tmpCards[i].name.IndexOf(":")) + ": " + objectName) : tmpCards[i].gameObject.name);

            //tmpCards[i].GetComponent<Button>().Select();
            yield return new WaitForSeconds(0.5f);
        }
    }

    // return the color depending on dropdown value
    public Color GetColor(int dropDownValue)
    {
        Color color;

        switch(dropDownValue)
        {
            case (int)DropDownColors.blue:
                color = lightBlue;
                break;
            case (int)DropDownColors.orange:
                color = lightOrange;
                break;
            case (int)DropDownColors.yellow:
                color = lightYellow;
                break;
            case (int)DropDownColors.green:
                color = lightGreen;
                break;
            case (int)DropDownColors.grey:
                color = grey;
                break;
            default:
                color = new Color(1, 1, 1, 0);
                break;
        }

        return color;
    }

    public int GetRemainingOptions(int dropDownValue)
    {
        int result;

        switch (dropDownValue)
        {
            case (int)DropDownColors.blue:
                result = remainingOptionsBlue;
                break;
            case (int)DropDownColors.orange:
                result = remainingOptionsOrange;
                break;
            case (int)DropDownColors.yellow:
                result = remainingOptionsYellow;
                break;
            case (int)DropDownColors.green:
                result = remainingOptionsGreen;
                break;
            case (int)DropDownColors.grey:
                result = remainingOptionsGrey;
                break;
            default:
                result = -1;
                break;
        }

        return result;
    }

    public void  SetRemainingOptions(int dropDownValue, int value)
    {
        switch (dropDownValue)
        {
            case (int)DropDownColors.blue:
                remainingOptionsBlue = value;
                break;
            case (int)DropDownColors.orange:
                remainingOptionsOrange = value;
                break;
            case (int)DropDownColors.yellow:
                remainingOptionsYellow = value;
                break;
            case (int)DropDownColors.green:
                remainingOptionsGreen = value;
                break;
            case (int)DropDownColors.grey:
                remainingOptionsGrey = value;
                break;
            default:
                break;
        }
    }

    public IEnumerator CheckAnswer(Image image, int op)
    {
        Color color;

        int dropDownValue = GetDropDownValue();

        //Debug.Log("Checking answer...");

        switch (op)
        {
            case (int)Operation.correct:
                color = new Color(0, 1, 0, 1); // green
                
                // subtract 1 from remaining option
                SetRemainingOptions(dropDownValue, GetRemainingOptions(dropDownValue) - 1);
                Debug.Log(GetRemainingOptions(dropDownValue));
                break;
            case (int)Operation.wrong:
                color = new Color(1, 0, 0, 1); // red
                break;
            default:
                color = new Color(0, 0, 0, 0);
                break;
        }

        image.color = color;

        // wait seconds
        yield return new WaitForSeconds(2f);

        if (op == (int)Operation.wrong)
        {
            // back to normal!!
            image.color = new Color(1, 1, 1, 0);
        }
        else
        {
            // set the color to dropdown color
            image.color = GetColor(GetDropDownValue());
        }
    }
    
    public int GetDropDownValue()
    {
        return processDropDown.value;
    }

    public void ChangeDropDownColor(int dropdownValue)
    {
        EinsteinCard.DO_NOT = false;

        processDropDown.GetComponent<Image>().color = GetColor(dropdownValue);
    }   

    public IEnumerator ReturnToShipCoroutine()
    {
        yield return new WaitForSeconds(7f);

        UnityEngine.SceneManagement.SceneManager.LoadScene(ScenesNames.M004Ship);
    }
}

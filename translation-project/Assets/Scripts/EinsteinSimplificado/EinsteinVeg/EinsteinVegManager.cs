using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;


public class EinsteinVegManager : AbstractScreenReader
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
    public GameObject LoseImage;

    public TMPro.TMP_Dropdown processDropDown;

    // hint settings
    public MinijogosDicas dicas;

    public LifeExpController lifeExpController;

    public GameObject instruction_interface;

    private enum Operation { correct, wrong }

    private enum DropDownColors { blue = 1, orange, purple, green, red }

    private Color blue = new Color(0, 0.361f, 0.624f);
    private Color orange = new Color(0.788f, 0.576f, 0.427f);
    private Color purple = new Color(0.69f, 0.09f, 0.78f);
    private Color green = new Color(0.353f, 0.698f, 0.459f);
    private Color red = new Color(0.792f, 0.435f, 0.431f);

    private int remainingOptionsBlue = 4;
    private int remainingOptionsOrange = 4;
    private int remainingOptionsPurple = 4;
    private int remainingOptionsGreen = 4;
    private int remainingOptionsRed = 4;

    public Sprite blue_ret;
    public Sprite green_ret;
    public Sprite orange_ret;
    public Sprite purple_ret;
    public Sprite red_ret;

    private int attempts = 3;
    private int tries = 0;

    public TMPro.TextMeshProUGUI attemptsText;

    //My code -----

    public enum TokensTypes {Briofitas = 1, Liquens = 2, Algas = 3, Angiospermas = 4}

    public String[] tokensText = new String[20];
    public TokensTypes[] tokensType = new TokensTypes[20];




    private void Start()
    {
        resetButton.interactable = false;

        init = false;

        ReadText(instructions);

        audioSource = GetComponent<AudioSource>();

        //initializeGame();
    }

    // Update is called once per frame
    void Update()
    {
        //if (!init)
        //{
        //    initializeGame();

        //    c = new List<int>();
        //}

        if ((Input.GetMouseButtonUp(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)) && !EinsteinVegCard.DO_NOT)
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
            EinsteinVegCard.DO_NOT = true;
            confirmarButton.interactable = true;
            cancelButton.interactable = true;
            //Debug.Log(c.Count);
        }
        else if (c != null && c.Count > 0)
        {
            processDropDown.interactable = false;
            cancelButton.interactable = true;
        }
        else
        {
            confirmarButton.interactable = false;
            cancelButton.interactable = false;
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            instruction_interface.SetActive(true);
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            instruction_interface.SetActive(false);
        }

        if (remainingOptionsGreen == 0 && remainingOptionsBlue == 0 && remainingOptionsOrange == 0 
            && remainingOptionsPurple == 0 && remainingOptionsRed == 0)
        {
            EndGame(true);
        }
    }

    public void initializeGame()
    {
        if (!init)
            initializeCards();

        c = new List<int>();

        // start afther dicas.time seconds and repeat at dicas.repeatRate rate
        //InvokeRepeating("CallHintMethod", dicas.time, dicas.repeatRate);

        //StartCoroutine(showCards());

        backButton.interactable = true;
        resetButton.interactable = true;

    }

    public void CallHintMethod()
    {
        dicas.StartHints();
    }

    public void initializeCards() {
        for (int i = 0; i < cards.Length; i++) {
            cards[i].GetComponent<EinsteinVegCard>().cardValue = i;
            cards[i].GetComponent<EinsteinVegCard>().cardText = tokensText[i];
            cards[i].GetComponent<EinsteinVegCard>().initialized = true;
            cards[i].GetComponent<EinsteinVegCard>().cardBack = getCardFace(i);
            cards[i].GetComponent<EinsteinVegCard>().cardFace = getCardFace(i);
            cards[i].GetComponent<EinsteinVegCard>().setupGraphics();
        }

        init = true;
        cards[0].GetComponent<Button>().Select();
        StartCoroutine(ReadCards());

        // for (int i = 1; i < cards.Length+1; i++)
        // {
        //     bool test = false;
        //     int choice = 0;
        //     while (!test)
        //     {
        //         choice = UnityEngine.Random.Range(0, cards.Length);
        //         test = !(cards[choice].GetComponent<EinsteinVegCard>().initialized);
        //     }

        //     cards[choice].GetComponent<EinsteinVegCard>().cardValue = i;
        //     cards[choice].GetComponent<EinsteinVegCard>().initialized = true;


        //     //Debug.Log(choice);

        //     cards[choice].GetComponent<EinsteinVegCard>().setupGraphics();
        // }
        
        // if (!init)
        // {
        //     init = true;
        //     cards[0].GetComponent<Button>().Select();
        //     StartCoroutine(ReadCards());
        // }
    }

    public Sprite getCardBack()
    {
        return cardBack;
    }

    public Sprite getCardFace(int i)
    {
        return cardFace[i];
    }

    void checkCards()
    {
        for (int i = 0; i < cards.Length; i++)
        {
            var card = cards[i].GetComponent<EinsteinVegCard>();
            if (card.state == EinsteinVegCard.VIRADA_CIMA && !card.added)
            {
                Debug.Log("carta adicionada >> " + cards[i]);
                c.Add(i);
                //Debug.Log("após adicionar carta >> " + c.Count);

                card.BGImage.color = GetColor(GetDropDownValue());
                card.added = true;
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
        int dropDownValue = GetDropDownValue();
        
        for (int i = 0; i < c.Count; i++)
        {
            cards[c[i]].GetComponent<EinsteinVegCard>().state = EinsteinCard.VIRADA_BAIXO;
            cards[c[i]].GetComponent<EinsteinVegCard>().turnCardDown();
            cards[c[i]].GetComponent<EinsteinVegCard>().BGImage.color = GetColor(-1);
            cards[c[i]].GetComponent<EinsteinVegCard>().added = false;
        }

        c.Clear();
        cards[0].GetComponent<Button>().Select();
        processDropDown.interactable = true;
    }

    void cardComparison(List<int> c)
    {
        Card.DO_NOT = true;
        
        int dropDownValue = GetDropDownValue();

        int correct;

        switch(dropDownValue)
        {
            case (int)DropDownColors.blue:
                correct = CheckCombination(c, dropDownValue, remainingOptionsBlue, "azul");
                break;

            case (int)DropDownColors.orange:
                correct = CheckCombination(c, dropDownValue, remainingOptionsOrange, "laranja");
                break;

            case (int)DropDownColors.purple:
                correct = CheckCombination(c, dropDownValue, remainingOptionsPurple, "roxo");
                break;

            case (int)DropDownColors.green:
                correct = CheckCombination(c, dropDownValue, remainingOptionsGreen, "verde");
                break;

            case (int)DropDownColors.red:
                correct = CheckCombination(c, dropDownValue, remainingOptionsRed, "vermelho");
                break;

            default:
                correct = 0;
                break;
        }
        
        for (int i = 0; i < GetRemainingOptions(GetDropDownValue()); i++)
        {
            cards[c[i]].GetComponent<EinsteinVegCard>().falseCheck();
        }

        Debug.Log("correct answers >> " + correct);
        // subtract 1 from remaining option
        SetRemainingOptions(dropDownValue, GetRemainingOptions(dropDownValue) - correct);
        Debug.Log(GetRemainingOptions(dropDownValue));

        c.Clear();
        cards[0].GetComponent<Button>().Select();
    }

    public int CheckCombination(List<int> c, int dropDownValue, int lenght, string cardType)
    {
        int x = 0;
        int correct = 0;
        bool wrong = false;

        for (int i = 0; i < lenght; i++)
        {
            var card = cards[c[i]].GetComponent<EinsteinVegCard>();
            x = 0;
            if (card.name.Contains(cardType))
            {
                Debug.Log("correct >> " + card);
                StartCoroutine(CheckAnswer(card.BGImage, (int)Operation.correct));
                x = 2;
                correct++;
            }
            else
            {
                Debug.Log("wrong >> " + card);
                StartCoroutine(CheckAnswer(card.BGImage, (int)Operation.wrong));
                wrong = true;
                // wrong answer, can add card to the list again
                card.added = false;
            }
            
            card.state = x;
        }


        if (wrong)
        {
            tries++;
            attemptsText.text = "Tentativas restantes: " + tries + "/" + attempts;
        }

        // loses the game
        if (tries > attempts)
        {
            EndGame(false);
        }

        processDropDown.interactable = true;
        return correct;
    }

    public void EndGame(bool win)
    {
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
            lifeExpController.AddEXP(0.0001f); // jogou um minijogo
        }

        StartCoroutine(ReturnToUshuaiaCoroutine()); // volta para o navio perdendo ou ganhando o minijogo
    }

    public void ReturnToUshuaia()
    {
        //if (!PlayerPreferences.M004_Memoria) lifeExpController.RemoveEXP(0.0001f); // saiu sem concluir o minijogo
        UnityEngine.SceneManagement.SceneManager.LoadScene(ScenesNames.M002Ushuaia);
    }

    public void ResetScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(ScenesNames.M002Einstein);
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

    // return the bg depending on dropdown value
    public Sprite GetBackground(int dropDownValue)
    {
        Sprite sprite = null;

        switch (dropDownValue)
        {
            case (int)DropDownColors.blue:
                sprite = blue_ret;
                break;
            case (int)DropDownColors.red:
                sprite = red_ret;
                break;
            case (int)DropDownColors.green:
                sprite = green_ret;
                break;
            case (int)DropDownColors.purple:
                sprite = purple_ret;
                break;
            case (int)DropDownColors.orange:
                sprite = orange_ret;
                break;
        }

        return sprite;
    }

    // return the color depending on dropdown value
    public Color GetColor(int dropDownValue)
    {
        Color color;

        switch(dropDownValue)
        {
            case (int)DropDownColors.blue:
                color = blue;
                break;
            case (int)DropDownColors.orange:
                color = orange;
                break;
            case (int)DropDownColors.purple:
                color = purple;
                break;
            case (int)DropDownColors.green:
                color = green;
                break;
            case (int)DropDownColors.red:
                color = red;
                break;
            case -1:
                color = new Color(1, 1, 1, 1); // cancel case
                break;
            default:
                color = new Color(1, 1, 1, 1);
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
            case (int)DropDownColors.purple:
                result = remainingOptionsPurple;
                break;
            case (int)DropDownColors.green:
                result = remainingOptionsGreen;
                break;
            case (int)DropDownColors.red:
                result = remainingOptionsRed;
                break;
            default:
                result = 1;
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
            case (int)DropDownColors.purple:
                remainingOptionsPurple = value;
                break;
            case (int)DropDownColors.green:
                remainingOptionsGreen = value;
                break;
            case (int)DropDownColors.red:
                remainingOptionsRed = value;
                break;
            default:
                break;
        }
    }

    public IEnumerator CheckAnswer(Image image, int op)
    {
        Color color;
        //Debug.Log("Checking answer...");

        switch (op)
        {
            case (int)Operation.correct:
                color = new Color(0, 1, 0, 1); // green
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
            image.color = new Color(1, 1, 1, 1);
        }
        else
        {
            // set the color to dropdown color
            image.sprite = GetBackground(GetDropDownValue());
            image.color = Color.white;
        }
    }
    
    public int GetDropDownValue()
    {
        return processDropDown.value;
    }

    public void ChangeDropDownColor(int dropdownValue)
    {
        EinsteinVegCard.DO_NOT = false;

        processDropDown.GetComponent<Image>().color = GetColor(dropdownValue);
    }   

    public IEnumerator ReturnToUshuaiaCoroutine()
    {
        yield return new WaitForSeconds(7f);

        UnityEngine.SceneManagement.SceneManager.LoadScene(ScenesNames.M002Ushuaia);
    }
}

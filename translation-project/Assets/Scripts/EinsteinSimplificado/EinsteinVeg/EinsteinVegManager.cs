using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class EinsteinVegManager : AbstractScreenReader
{
    private readonly string instructions = "Início do jogo. Mini jogo de memória. Descrição..";

    private string currentDescription = null;

    // round 0
    public Sprite[] cardFace;

    public Sprite cardBack;

    public GameObject[] cards;

    public Button backButton;
    public Button resetButton;
    public Button audioButton;
    public Button librasButton;
    public Button confirmarButton;
    public Button cancelButton;

    //public int[] index;

    private bool initialized = false;

    private AudioSource audioSource;

    public AudioClip correctAudio;
    public AudioClip wrongAudio;

    //private List<int> tokensToCompare = new List<int>();
    private List<EinsteinVegCard> tokensToCompare = new List<EinsteinVegCard>();

    public GameObject WinImage;
    public GameObject LoseImage;

    public TMPro.TMP_Dropdown processDropDown;

    // hint settings
    public MinijogosDicas dicas;

    public LifeExpController lifeExpController;

    public GameObject instruction_interface;

    private enum Operation { correct, wrong }

    private enum DropDownColors { white = 0, blue, orange, purple, green, red }

    private List<EinsteinVegCard> doneTokens = new List<EinsteinVegCard>(); //List to store cards already done

    private Color[] colorsList = new Color[] {
        new Color(1, 1, 1, 1), //White
        new Color(0, 0.361f, 0.624f),   //Blue
        new Color(0.788f, 0.576f, 0.427f), //Orange
        new Color(0.69f, 0.09f, 0.78f), //Purple
        new Color(0.353f, 0.698f, 0.459f), //Green
        new Color(0.792f, 0.435f, 0.431f) //Red
    };

    private int[] remainingOptionsCounter = new int[] {
        0, //Offset only. Must be zero because we don't update it and all values must be 0 for ending the game.
        5, //Blue
        5, //Orange
        5, //Purple
        5, //Green
        //4, //Red
    };

    private string[] colorNames = new string[] {"branco", "azul", "laranja", "roxo", "verde", "vermelho"};

    public Sprite blue_ret;
    public Sprite green_ret;
    public Sprite orange_ret;
    public Sprite purple_ret;
    public Sprite red_ret;


    private int attempts = 3;
    private int tries = 0;

    public TMPro.TextMeshProUGUI attemptsText;


    public enum TokensTypes {Briofitas = 1, Liquens = 2, Algas = 3, Angiospermas = 4}

    public String[] tokensText = new String[20];
    public TokensTypes[] tokensType = new TokensTypes[20];

    public HUDMJController hud;

    private void Start()
    {
        //Inventory debug flag
        //PlayerPreferences.M010_Amostras = true;
        
        audioSource = GetComponent<AudioSource>();

        ReadText(instructions); //Read the instructions of the minigame
    }

    //solve hand walkiing in the minigame

    // Update is called once per frame
    void Update()
    {
        //Enable or disable components
        processDropDown.interactable = tokensToCompare.Count == 0;
        cancelButton.interactable = !processDropDown.interactable;
        confirmarButton.interactable = tokensToCompare.Count > 0 && 
            tokensToCompare.Count == GetRemainingOptions(GetDropDownValue());
        
        // if ((Input.GetMouseButtonUp(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))) {
        //         if(!EinsteinVegCard.DO_NOT)
        //             checkCards();
        // }

        //Check keys press
        if (Input.GetKeyDown(InputKeys.MJMENU_KEY)) {
            audioButton.Select();
        }

        if (Input.GetKeyDown(KeyCode.F6)) {
            if(isAnySelected(confirmarButton, cancelButton))
                processDropDown.Select();
            else if(isAnySelected(processDropDown))
                cards[0].GetComponent<Button>().Select();
            else
                cancelButton.Select();
        }

        if (Input.GetKeyDown(KeyCode.F1)) {
            instruction_interface.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            if(instruction_interface.activeSelf)
                instruction_interface.SetActive(false);
            else
                hud.TryQuit();
        }

        if (Input.GetKeyDown(InputKeys.REPEAT_KEY) && currentDescription != null) {
            ReadText(currentDescription);
        }

        // if(Input.GetKeyDown(InputKeys.MJMENU_KEY))
        // {
        //     if(isAnySelected(audioButton, librasButton, resetButton, backButton))
        //         cards[0].GetComponent<Button>().Select();
        //     else
        //         audioButton.Select();
        // }

        //Checks if all the options are already done. If so, end the game
        bool allDone = true;
        foreach(int option in remainingOptionsCounter) {
            if(option > 0) {
                allDone = false;
                break;
            }
        }
        if(allDone)
            EndGame(true);
    }

    private bool isSelected(GameObject go) {
        return go == EventSystem.current.currentSelectedGameObject;
    }

    private bool isAnySelected(params Component[] components) {
        foreach(Component c in components) {
            if(c.gameObject == EventSystem.current.currentSelectedGameObject)
                return true;
        }
        return false;
    }

    private bool isAnySelected(params Selectable[] selectables) {
        foreach(Selectable s in selectables) {
            if(s.gameObject == EventSystem.current.currentSelectedGameObject)
                return true;
        }
        return false;
    }

    public void initializeGame() {
        
        if(!initialized){
            initializeCards();
            cards[0].GetComponent<Button>().Select();
            StartCoroutine(ReadCards());
            initialized = true;
            resetButton.interactable = true;
        }

        processDropDown.Select();

        currentDescription = ReadableTexts.instance.GetReadableText("m010_amostra_screen", LocalizationManager.instance.GetLozalization());

        ReadText(currentDescription);
    }

    public void CallHintMethod() {
        dicas.StartHints();
    }

    private int[] randomIntArray(int range) {
        System.Random r = new System.Random();
        int[] intArray = new int[range];
        for(int i = 0; i < range; i++)
            intArray[i] = i;
        return intArray.OrderBy(x => r.Next()).ToArray();
    }

    public void initializeCards() {
        
        //For initialize random, we need to setup a random list of indexes
        int[] randomIndexes = randomIntArray(cards.Length);

        for (int i = 0; i < cards.Length; i++) {
            int randId = randomIndexes[i];
            cards[randId].GetComponent<EinsteinVegCard>().cardValue = i;
            cards[randId].GetComponent<EinsteinVegCard>().cardText = tokensText[i];
            cards[randId].GetComponent<EinsteinVegCard>().cardBack = cardFace[i];
            cards[randId].GetComponent<EinsteinVegCard>().cardFace = cardFace[i];
            cards[randId].GetComponent<EinsteinVegCard>().setupGraphics();
        }
   
    }


    public void OnTokenClick(EinsteinVegCard token) {

        //Conditions to avoid select one card
        if(
            tokensToCompare.Contains(token) ||
            doneTokens.Contains(token) ||
            GetRemainingOptions(GetDropDownValue()) <= 0 ||
            tokensToCompare.Count == GetRemainingOptions(GetDropDownValue())
        )
            return;

        tokensToCompare.Add(token);
        token.BGImage.color = GetColor(GetDropDownValue());

        //Select confirm button after last card is selected
        if(tokensToCompare.Count == GetRemainingOptions(GetDropDownValue()))
            confirmarButton.Select();
    }

    // void checkCards() {
    //     for (int i = 0; i < cards.Length; i++) {
    //         EinsteinVegCard card = cards[i].GetComponent<EinsteinVegCard>();
    //         if (card.state == EinsteinVegCard.VIRADA_CIMA && !tokensToCompare.Contains(i)) {
    //                 Debug.Log("carta adicionada >> " + cards[i]);
    //                 tokensToCompare.Add(i);
    //                 Debug.Log("após adicionar carta >> " + tokensToCompare.Count);

    //                 card.BGImage.color = GetColor(GetDropDownValue());
    //         }
    //     }

    // }

    public void CompareCards() {
        Card.DO_NOT = true;

        
        int dropDownValue = GetDropDownValue();

        int correct;

        if(dropDownValue <= 0) 
            correct = 0;
        else
            correct = CheckCombination(dropDownValue, colorNames[dropDownValue]);
        
        foreach(EinsteinVegCard token in tokensToCompare)
            token.falseCheck();



        Debug.Log("correct answers >> " + correct);
        // subtract 1 from remaining option
        SetRemainingOptions(dropDownValue, GetRemainingOptions(dropDownValue) - correct);
        Debug.Log(GetRemainingOptions(dropDownValue));

        //tokensToCompare.Clear();
        cards[0].GetComponent<Button>().Select();

        //Check if the current option is done:
        if (GetRemainingOptions(dropDownValue) == 0) {
            processDropDown.options[GetDropDownValue()].text += " (Finalizado)";
        }
    }

    public void Cancel()
    {
        foreach(EinsteinVegCard token in tokensToCompare)
            token.BGImage.color = GetColor(-1);

        tokensToCompare.Clear();
        cards[0].GetComponent<Button>().Select();
    }

    public int CheckCombination(int dropDownValue, string cardType) {

        int x = 0;
        int correct = 0;
        bool wrong = false;

        foreach(EinsteinVegCard card in tokensToCompare) {
            x = 0;
            if(tokensType[card.cardValue] == (TokensTypes)dropDownValue) {
                doneTokens.Add(card);
                Debug.Log("correct >> " + card);
                StartCoroutine(CheckAnswer(card.BGImage, (int)Operation.correct));
                x = 2;
                correct++;
            } else {
                Debug.Log("wrong >> " + card);
                StartCoroutine(CheckAnswer(card.BGImage, (int)Operation.wrong));
                wrong = true;
            }
            
            card.state = x;
        }


        if (wrong) {
            tries++;
            attemptsText.text = "Tentativas: " + tries + "/" + attempts;
        }

        // loses the game
        if (tries > attempts) {
            EndGame(false);
        }

        return correct;


    }

    public void EndGame(bool win)
    {
        if (win)
        {
            currentDescription = ReadableTexts.instance.GetReadableText("m010_amostra_win", LocalizationManager.instance.GetLozalization());
            PlayerPreferences.M010_Amostras = true;
            WinImage.SetActive(true);
            //WinImage.GetComponentInChildren<Button>().Select();

            //lifeExpController.AddEXP(0.001f); // finalizou o minijogo
            //lifeExpController.AddEXP(0.0002f); // ganhou o item
        }
        else
        {
            currentDescription = ReadableTexts.instance.GetReadableText("m010_amostra_lose", LocalizationManager.instance.GetLozalization());
            LoseImage.SetActive(true);
            //lifeExpController.AddEXP(0.0001f); // jogou um minijogo
        }

        ReadText(currentDescription);

        DoAfter(3, ReturnToCamp);
    }

    public void DoAfter(int secs, UnityAction action) {
        StartCoroutine(DoAfterCoroutine(secs, action));
    }

    public IEnumerator DoAfterCoroutine(int secs, UnityAction action) {
        yield return new WaitForSeconds(secs);
        action();
    }

    //Volta para o acampamento
    public void ReturnToCamp() {
        UnityEngine.SceneManagement.SceneManager.LoadScene(ScenesNames.M010Camp);
    }

    public void ResetScene() {
        UnityEngine.SceneManagement.SceneManager.LoadScene(ScenesNames.M010AmostrasVegetacao);
    }

    public IEnumerator ReadCards() {
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
    public Sprite GetBackground(int dropDownValue) {
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
    public Color GetColor(int dropDownValue) {
        if(dropDownValue < 0)
            dropDownValue = 0;
        return colorsList[dropDownValue];
    }

    public int GetRemainingOptions(int dropDownValue) {
        return remainingOptionsCounter[dropDownValue];
    }

    public void  SetRemainingOptions(int dropDownValue, int value) {
        remainingOptionsCounter[dropDownValue] = value;
    }

    public IEnumerator CheckAnswer(Image image, int op) {
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

        tokensToCompare.Clear();
    }
    
    public int GetDropDownValue()
    {
        return processDropDown.value;
    }

    public void UpdateDropDownColor() {
        ChangeDropDownColor(GetDropDownValue());
    }

    public void ChangeDropDownColor(int dropdownValue)
    {
        EinsteinVegCard.DO_NOT = false;

        processDropDown.GetComponent<Image>().color = GetColor(dropdownValue);
    }   
}

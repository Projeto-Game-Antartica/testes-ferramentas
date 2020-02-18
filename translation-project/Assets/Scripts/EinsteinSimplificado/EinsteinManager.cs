using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EinsteinManager : AbstractScreenReader
{
    //private readonly string instructions = "Início do jogo. Mini jogo de memória. Descrição..";

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

    public GameObject confirmQuit;

    public AudioClip correctAudio;
    public AudioClip wrongAudio;
    public AudioClip closeClip;
    public AudioClip avisoClip;
    public AudioClip selectClip;
    public AudioClip victoryClip;
    public AudioClip loseClip;

    private List<int> c;

    public GameObject WinImage;
    public GameObject LoseImage;

    public TMPro.TMP_Dropdown processDropDown;

    // hint settings
    public MinijogosDicas dicas;

    public LifeExpController lifeExpController;

    public GameObject instruction_interface;

    private enum Operation { correct, wrong }

    private enum DropDownColors { blue = 1, orange, green, red }

    private Color blue = new Color(0, 0.361f, 0.624f);
    private Color orange = new Color(0.788f, 0.576f, 0.427f);
    private Color green = new Color(0.353f, 0.698f, 0.459f);
    private Color red = new Color(0.792f, 0.435f, 0.431f);

    private int remainingOptionsBlue = 4;
    private int remainingOptionsOrange = 4;
    private int remainingOptionsGreen = 4;
    private int remainingOptionsRed = 4;

    public Sprite blue_ret;
    public Sprite green_ret;
    public Sprite orange_ret;
    public Sprite red_ret;

    private int attempts = 3;
    private int tries = 0;

    private int selectedArea;
    private bool isOnMenu;

    private bool finished = false;

    public TMPro.TextMeshProUGUI attemptsText;

    private void Start()
    {
        resetButton.interactable = false;
        selectedArea = 0;
        isOnMenu = false;

        init = false;

        //ReadText(instructions);

        audioSource = GetComponent<AudioSource>();

        //initializeGame();

        Parameters.HIGH_CONTRAST = true;
    }

    // Update is called once per frame
    void Update()
    {
        //if (!init)
        //{
        //    initializeGame();

        //    c = new List<int>();
        //}

        if ((Input.GetMouseButtonUp(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)) && !EinsteinCard.DO_NOT)
        {
            //Debug.Log("Checando cartas....");
            checkCards();
        }

        if (Input.GetKeyDown(InputKeys.MJMENU_KEY))
        {
            isOnMenu = !isOnMenu;

            if (!isOnMenu)
                audioButton.Select();
            else
                SelectNextAvailableCard();
        }

        if (Input.GetKeyDown(KeyCode.F6))
        {
            selectedArea = (selectedArea + 1) % 3;
            
            if (selectedArea == 1)
            {
                if (processDropDown.interactable)
                    processDropDown.Select();
                else
                    selectedArea = 2;
            }

            if (selectedArea == 2)
            {
                if (cancelButton.interactable)
                    cancelButton.Select();
                else
                    selectedArea = 0;
            }

            if (selectedArea == 0)
            {
                SelectNextAvailableCard();
            }
        }

        if (c != null && c.Count >= GetRemainingOptions(GetDropDownValue()))
        {
            EinsteinCard.DO_NOT = true;
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
            if (instruction_interface.activeSelf)
            {
                audioSource.PlayOneShot(closeClip);
                instruction_interface.SetActive(false);
            }
            else
            {
                TryReturnToUshuaia();
            }
        }

        if (Input.GetKeyDown(InputKeys.PARAMETERS_KEY))
        {
            lifeExpController.ReadHPandEXP();
            ReadText(attemptsText.text);
            Debug.Log(attemptsText.text);
        }

        if (Input.GetKeyDown(InputKeys.AUDIODESCRICAO_KEY))
        {
            ReadText(ReadableTexts.instance.GetReadableText(ReadableTexts.key_m002_processo, LocalizationManager.instance.GetLozalization()));
        }

        if (Input.GetKeyDown(InputKeys.REPEAT_KEY))
        {
        }

        if (remainingOptionsGreen == 0 && remainingOptionsBlue == 0 && remainingOptionsOrange == 0 && remainingOptionsRed == 0)
        {
            StartCoroutine(EndGame(true));
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
            //cards[0].GetComponent<Button>().Select();
            processDropDown.Select();
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
        for (int i = 0; i < cards.Length; i++)
        {
            var card = cards[i].GetComponent<EinsteinCard>();
            if (card.state == EinsteinCard.VIRADA_CIMA && !card.added)
            {
                Debug.Log("carta adicionada >> " + cards[i]);

                audioSource.PlayOneShot(selectClip);

                c.Add(i);
                //Debug.Log("após adicionar carta >> " + c.Count);

                card.BGImage.color = GetColor(GetDropDownValue());
                card.added = true;

                if (c.Count == GetRemainingOptions(GetDropDownValue()))
                    confirmarButton.Select();
            }
        }

        //Debug.Log("Após checar cartas >> " + c.Count);

        //if (c.Count == GetRemainingOptions(GetDropDownValue()))
        //    cardComparison(c);
    }

    public void CompareCards()
    {
        cardComparison(c);

        SelectNextAvailableCard();
    }

    public void Cancel()
    {
        int dropDownValue = GetDropDownValue();
        
        for (int i = 0; i < c.Count; i++)
        {
            cards[c[i]].GetComponent<EinsteinCard>().state = EinsteinCard.VIRADA_BAIXO;
            cards[c[i]].GetComponent<EinsteinCard>().turnCardDown();
            cards[c[i]].GetComponent<EinsteinCard>().BGImage.color = GetColor(-1);
            cards[c[i]].GetComponent<EinsteinCard>().added = false;
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
            cards[c[i]].GetComponent<EinsteinCard>().falseCheck();
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
            var card = cards[c[i]].GetComponent<EinsteinCard>();
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
                // card#: content
                string wrongCard = card.name.Substring(0, card.name.IndexOf(":")) + ": " + card.GetComponentInChildren<TextMeshProUGUI>().text;
                
                //Debug.Log("wrong >> " + card);
                Debug.Log("Carta selecionada errada: " + wrongCard);
                ReadText("Carta selecionada errada: " + wrongCard);

                StartCoroutine(CheckAnswer(card.BGImage, (int)Operation.wrong));

                wrong = true;
                // wrong answer, can add card to the list again
                card.added = false;
            }
            
            card.state = x;
        }


        if (wrong)
        {
            audioSource.PlayOneShot(wrongAudio);

            lifeExpController.AddEXP(PlayerPreferences.XPwrongTry);

            tries++;
            attemptsText.text = "Tentativa: " + tries + "/" + attempts;

            ReadText("Tentativa " + tries + " de " + attempts);
            Debug.Log("Tentativa " + tries + " de " + attempts);
        }
        else
        {
            audioSource.PlayOneShot(correctAudio);

            processDropDown.options[GetDropDownValue()].text += " (Finalizado)";
        }

        // loses the game
        if (tries > attempts)
        {
            StartCoroutine(EndGame(false));
        }

        processDropDown.interactable = true;
        return correct;
    }

    public IEnumerator EndGame(bool win)
    {
        if (win)
        {
            if (!finished)
            {
                finished = true;
                WinImage.SetActive(true);

                PlayerPreferences.M002_ProcessoPesquisa = true;

                //WinImage.GetComponentInChildren<Button>().Select();

                ReadText("Parabéns, você ganhou alguns dos itens necessário para sua aventura na antártica: galocha, calça de fleece, calça impermeável, jaqueta polar e calça segunda pele.");
                
                audioSource.PlayOneShot(victoryClip);
            
                yield return new WaitWhile(() => audioSource.isPlaying);

                ReadText(ReadableTexts.instance.GetReadableText(ReadableTexts.key_m002_processo_vitoria, LocalizationManager.instance.GetLozalization()));

                yield return new WaitForSeconds(5f);
                
                lifeExpController.AddEXP(PlayerPreferences.XPwinPuzzle); // finalizou o minijogo
                lifeExpController.AddEXP(5*PlayerPreferences.XPwinItem); // ganhou o item
            }
        }
        else
        {
            LoseImage.SetActive(true);

            ReadText("Infelizmente você não conseguiu finalizar o minijogo com êxito. Tente novamente.");

            audioSource.PlayOneShot(loseClip);

            yield return new WaitWhile(() => audioSource.isPlaying);

            ReadText(ReadableTexts.instance.GetReadableText(ReadableTexts.key_m002_processo_derrota, LocalizationManager.instance.GetLozalization()));

            yield return new WaitForSeconds(5f);

            resetButton.Select();
            lifeExpController.AddEXP(PlayerPreferences.XPlosePuzzle); // jogou um minijogo
        }

        StartCoroutine(ReturnToUshuaiaCoroutine()); // volta para o ushuaia perdendo ou ganhando o minijogo
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
        EinsteinCard.DO_NOT = false;

        processDropDown.GetComponent<Image>().color = GetColor(dropdownValue);

        Debug.Log(processDropDown.options[processDropDown.value].text);

        SelectNextAvailableCard();
    }

    public void SelectNextAvailableCard()
    {
        foreach (GameObject ec in cards)
        {
            if (!ec.GetComponent<EinsteinCard>().added)
            {
                ec.GetComponent<Button>().Select();
                Debug.Log(ec.name + " selected");
                selectedArea = 0;
                break;
            }
        }
    }

    public void ReadDropDown()
    {
        Debug.Log(processDropDown.options[processDropDown.value].text);
        ReadText(processDropDown.options[processDropDown.value].text);
    }

    //public void ReadDropDownItem(RectTransform item)
    //{
    //    string result = "";
    //    string itemText = item.GetComponentInChildren<TextMeshProUGUI>().text;


    //    itemText.Replace("OK. ", "");

    //    switch (itemText)
    //    {
    //        case "A metodologia desta Ciência Cidadã é a Fotoidentificação.":
    //            if (GetRemainingOptions((int)DropDownColors.blue) == 0)
    //            {
    //                result += " pistas já encontradas. ";
    //                //item.GetComponentInChildren<Image>().color = blue;
    //                //item.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
    //            }
    //            result += itemText;
    //            break;
    //        case "O objetivo desta pesquisa brasileira é investigar o passado biológico na Antártica.":
    //            if (GetRemainingOptions((int)DropDownColors.orange) == 0)
    //            {
    //                result += " pistas já encontradas. ";
    //                //item.GetComponentInChildren<Image>().color = orange;
    //                //item.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
    //            }

    //            result += itemText;
    //            break;
    //        case "Para colaborar com esta Ciência Cidadã é necessário binóculos e catálogo de imagens.":
    //            if (GetRemainingOptions((int)DropDownColors.green) == 0)
    //            {
    //                result += " pistas já encontradas. ";
    //                //item.GetComponentInChildren<Image>().color = green;
    //                //item.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
    //            }

    //            result += itemText;
    //            break;
    //        case "Esta pesquisa brasileira estuda a Vegetação Antártica.":
    //            if (GetRemainingOptions((int)DropDownColors.red) == 0)
    //            {
    //                result += " pistas já encontradas. ";
    //                //item.GetComponentInChildren<Image>().color = red;
    //                //item.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
    //            }

    //            result += itemText;
    //            break;
    //        default:
    //            //item.GetComponentInChildren<Image>().color = Color.white;
    //            result += itemText;
    //            break;
    //    }

    //    Debug.Log(result);
    //    ReadText(result);
    //}

    public void TryReturnToUshuaia()
    {
        audioSource.PlayOneShot(avisoClip);

        confirmQuit.SetActive(true);

        ReadText(ReadableTexts.instance.GetReadableText(ReadableTexts.key_gameplay_aviso_botoes, LocalizationManager.instance.GetLozalization()));

        ReadText(confirmQuit.GetComponentInChildren<TMPro.TextMeshProUGUI>().text);

        confirmQuit.GetComponentInChildren<Button>().Select();
    }

    public IEnumerator ReturnToUshuaiaCoroutine()
    {
        yield return new WaitForSeconds(7f);

        UnityEngine.SceneManagement.SceneManager.LoadScene(ScenesNames.M002Ushuaia);
    }
}
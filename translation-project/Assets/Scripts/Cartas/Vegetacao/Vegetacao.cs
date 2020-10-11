using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class Vegetacao : AbstractCardManager
{
    //public Image kcalBar;

    private Sprite[] cardSprites;
    private string[] cardDescriptions;

    private string currentDescription = null;
    private string screenDescription = null;

    public Transform alimentos;

    private float kcal;

    private readonly int MAX_KCAL = 2000;

    public Image[] alimentosCesta;

    private bool initialized = false;

    //private int alimentosCestaIndex = 20;

    //public GameObject alimentoCestaPrefab;

    //private List<GameObject> alimentosCestaList;

    public Button satisfeitoButton;

    public Button LikeButton, DislikeButton;

    public Button audioButton;
    public Button librasButton;
    public Button resetButton;
    public Button backButton;
    public GameObject confirmQuit;

    public GameObject instruction_interface;

    public HUDMJController hud;

    //Lucas code
    private System.Random rand;
    private enum Vegetais { Planta, Fungo, Alga };
    private Vegetais[] correctAnswer;

    private readonly String[] questions = new String[] {
        "É uma planta?",
        "É um fungo?",
        "É uma alga?"
    };
    private int questionId;

    private T randomChoice<T>(T[] array) {
        int randIndex = rand.Next(array.Length);
        return array[randIndex];
    }

    public Image algaImg, plantaImg, fungoImg;
    public GameObject winImg, loseImg;

    private void updateCurrentCard(int cardIndex) {
        //Updates current showing card
        //currentImage.sprite = sprites[cardIndex];
        //currentImage.name = sprites[cardIndex].name;

        currentImage.sprite = cardSprites[cardIndex];
        currentImage.name = cardSprites[cardIndex].name;
        currentDescription = cardDescriptions[cardIndex];

        //updateQuestion();
        updateRandomQuestion();

        ReadText(cardName.text);
        ReadText(currentDescription);
    }

    private void updateNextCard(int cardIndex){
        //Updates the next showing card
        if(cardIndex < sprites.Length) {
            nextImage.GetComponentInChildren<Image>().sprite = cardSprites[cardIndex];
            nextImage.name = cardSprites[cardIndex].name;
        } else {
            nextImage.transform.parent.gameObject.SetActive(false); //Get hid of the last card
            nextImage.name = "";
        }
    }

    private void updateQuestion() {
        //Updates the question text according to the selected card
        if(sprites[cardIndex].name.EndsWith("P"))
            questionId = 0;
        else if(sprites[cardIndex].name.EndsWith("F"))
            questionId = 1;
        else if(sprites[cardIndex].name.EndsWith("A"))
            questionId = 2;
        else
            throw new NotImplementedException("Question not implemented for " + sprites[cardIndex].name);
        cardName.text = questions[questionId];
    }

    private void clearQuestion() {
        cardName.text = " ";
    }

    protected override void beforePositiveSwipe() {
        clearQuestion();
    }

    protected override void beforeNegativeSwipe() {
        clearQuestion();
    }

    private void updateRandomQuestion() {
        //Write random question in the board and returning the right answer
        questionId = rand.Next(questions.Length);
        cardName.text = questions[questionId];
    }

    private Vegetais getCardType(int cardIndex) {
        //Function to return card type according to its index
        Vegetais cardType = correctAnswer[cardIndex];
        // if(sprites[cardIndex].name.StartsWith("A")) 
        //     cardType = Vegetais.Alga;
        // else if(sprites[cardIndex].name.StartsWith("F"))
        //     cardType = Vegetais.Fungo;
        // else if(sprites[cardIndex].name.StartsWith("P"))
        //     cardType = Vegetais.Planta;
        // else
        //     throw new NotImplementedException("Type not implemented for " + sprites[cardIndex].name);

        return cardType;
    }

    private bool isAnySelected(params Selectable[] selectables) {
        foreach(Selectable s in selectables) {
            if(s.gameObject == EventSystem.current.currentSelectedGameObject)
                return true;
        }
        return false;
    }


    private void Update() {    

        if (Input.GetKeyDown(InputKeys.INSTRUCTIONS_KEY))
            instruction_interface.SetActive(true);

        if (Input.GetKeyDown(KeyCode.Escape)) {
            if(instruction_interface.activeSelf)
                instruction_interface.SetActive(false);
            else
                hud.TryQuit();

        }

        if (Input.GetKeyDown(KeyCode.F5))
            minijogosDicas.ShowHint();

        if (Input.GetKeyDown(InputKeys.REPEAT_KEY) && currentDescription != null) {
            ReadText(screenDescription);
            ReadText(cardName.text);
            ReadText(currentDescription);
        }

        if(Input.GetKeyDown(InputKeys.MJMENU_KEY))
        {
            if(isAnySelected(audioButton, librasButton, resetButton, backButton))
                LikeButton.Select();
            else
                audioButton.Select();
        }

        
    }

    private void Start() {

        //PlayerPreferences.M010_Tipos = true;

        //Load sprites
        UnityEngine.Object[] cardSpriteAssets = Resources.LoadAll("tipos_veg", typeof(Sprite));
        cardSprites = new Sprite[cardSpriteAssets.Length];
        foreach(UnityEngine.Object c in cardSpriteAssets) {
            int index = Int32.Parse(c.name);
            cardSprites[index] = c as Sprite;
        }

        //Load card descriptions and correct answers
        UnityEngine.Object[] descTexts = Resources.LoadAll("tipos_veg", typeof(TextAsset));
        cardDescriptions = new string[descTexts.Length];
        correctAnswer = new Vegetais[descTexts.Length];
        foreach(UnityEngine.Object desc in descTexts) {
            int index = Int32.Parse(desc.name);
            string[] descSplit = (desc as TextAsset).text.Split('\t');
            cardDescriptions[index] = descSplit[1];
            if(descSplit[0] == "alga")
                correctAnswer[index] = Vegetais.Alga;
            else if(descSplit[0] == "planta")
                correctAnswer[index] = Vegetais.Planta;
            else if(descSplit[0] == "fungo")
                correctAnswer[index] = Vegetais.Fungo;
            else
                throw new NotImplementedException("Unknown vegetable: " + descSplit[0]);      
        }
        
        rand = new System.Random(); //Inits random number generator

        sprites = Shuffle<Sprite>(sprites);

        //alimentosCestaList = new List<GameObject>();

        cardIndex = 0;
        //isDone = false;

        screenDescription = ReadableTexts.instance.GetReadableText("m010_tipos_screen", LocalizationManager.instance.GetLozalization());
        ReadText(screenDescription);

        updateCurrentCard(cardIndex);
        updateNextCard(cardIndex + 1);

        initialPosition = currentImage.transform.parent.position;

        resetButton.interactable = true;
        backButton.interactable = true;

        
    }

    // initialize after button click on instruction
    public void Initialize() {
        if(!initialized) {
            algaImg.fillAmount = 1.0f;
            plantaImg.fillAmount = 1.0f;
            fungoImg.fillAmount = 1.0f;

            initialized = true;
        }
        LikeButton.Select();
    }

    override public void CheckLike()
    {
        // do something
        if(checkAnswer(true))
            Debug.Log("Resposta certa!");
        else
            Debug.Log("Resposta errada!");


        Transform cardImage = currentImage.GetComponentInChildren<Image>().transform;

        NextCard();

        LikeButton.Select();
    }

    override public void CheckDislike()
    {
        // do something else
        if(checkAnswer(false))
            Debug.Log("Resposta certa!");
        else
            Debug.Log("Resposta errada!");
        NextCard();

        DislikeButton.Select();
    }

    private bool checkAnswer(bool answer) {
        //Check if the question type and the card type matches
        Vegetais currentCardType = getCardType(cardIndex);
        Vegetais chosenCardType = (Vegetais)questionId;
        bool rightAnswer = answer == (currentCardType == chosenCardType);

        if(!rightAnswer) { //If it is the wrong answer...
            switch(currentCardType) {
                case Vegetais.Alga:
                    algaImg.fillAmount -= 0.4f;
                    if(algaImg.fillAmount <= 0)
                        doLose();
                    break;
                case Vegetais.Fungo:
                    fungoImg.fillAmount -= 0.4f;
                    if(fungoImg.fillAmount <= 0)
                        doLose();
                    break;
                case Vegetais.Planta:
                    plantaImg.fillAmount -= 0.4f;

                    break;
                default:
                    break;
            }
        }

        return rightAnswer;       
    }


    private bool checkWin() {
        //Check if the player won the game
        return cardIndex >= sprites.Length;
    }

    private bool checkLose() {
        //Checks if the user lose the game
        return algaImg.fillAmount <= 0 || plantaImg.fillAmount <= 0 || fungoImg.fillAmount <= 0;
    }

    private void doWin() { //Routine to happen once the user wins
        screenDescription = null;
        currentDescription = ReadableTexts.instance.GetReadableText("m010_tipos_win", LocalizationManager.instance.GetLozalization());
        PlayerPreferences.M010_Tipos = true;
        minijogosDicas.SupressDicas();
        winImg.SetActive(true);
        Debug.Log("Você ganhou!");

        ReadText(currentDescription);

        DoAfter(5, ReturnToCamp);
    }

    private void doLose() { //Routine to happen once the user wins
        screenDescription = null;
        currentDescription = ReadableTexts.instance.GetReadableText("m010_tipos_lose", LocalizationManager.instance.GetLozalization());
        minijogosDicas.SupressDicas();
        loseImg.SetActive(true);
        Debug.Log("Você perdeu!");

        ReadText(currentDescription);

        DoAfter(5, ReturnToCamp);
    }

    public void NextCard() {
        cardIndex++;

        if(checkLose())
            doLose();
        else if(checkWin())
            doWin();
        else {
            updateCurrentCard(cardIndex);
            updateNextCard(cardIndex + 1);
            ResetPosition();
        }
    }

    
	/// <summary>
	/// Shuffle the array.
	/// </summary>
	/// <typeparam name="T">Array element type.</typeparam>
	/// <param name="array">Array to shuffle.</param>
	public T[] Shuffle<T>(T[] array)
	{
		var random = new System.Random();
		for (int i = array.Length; i > 1; i--)
		{
			// Pick random element to swap.
			int j = random.Next(i); // 0 <= j <= i-1
			// Swap.
			T tmp = array[j];
			array[j] = array[i - 1];
			array[i - 1] = tmp;
		}
		return array;
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

    public void ResetScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(ScenesNames.M010TiposVegetacao);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Vegetacao : AbstractCardManager
{
    //public Image kcalBar;

    public Transform alimentos;

    private int cardIndex;

    private float kcal;

    private readonly int MAX_KCAL = 2000;

    public Image[] alimentosCesta;

    private int alimentosCestaIndex = 20;

    public GameObject alimentoCestaPrefab;

    private List<GameObject> alimentosCestaList;

    public Button satisfeitoButton;

    public Button resetButton;
    public Button backButton;
    public GameObject confirmQuit;

    public GameObject instruction_interface;

    //Lucas code
    private System.Random rand;
    private enum Vegetais { Planta, Fungo, Alga };

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
        currentImage.sprite = sprites[cardIndex];
        currentImage.name = sprites[cardIndex].name;
        //cardName.text = currentImage.name;
        //updateRandomQuestion();
        updateQuestion();
    }

    private void updateNextCard(int cardIndex){
        //Updates the next showing card
        if(cardIndex < sprites.Length) {
            nextImage.GetComponentInChildren<Image>().sprite = sprites[cardIndex];
            nextImage.name = sprites[cardIndex].name;
        } else {
            nextImage.GetComponentInChildren<Image>().sprite = null;
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

    // private void updateRandomQuestion() {
    //     //Write random question in the board and returning the right answer
    //     questionId = rand.Next(questions.Length);
    //     cardName.text = questions[questionId];
    // }

    private Vegetais getCardType(int cardIndex) {
        //Function to return card type according to its index
        Vegetais cardType;
        if(sprites[cardIndex].name.StartsWith("A")) 
            cardType = Vegetais.Alga;
        else if(sprites[cardIndex].name.StartsWith("F"))
            cardType = Vegetais.Fungo;
        else if(sprites[cardIndex].name.StartsWith("P"))
            cardType = Vegetais.Planta;
        else
            throw new NotImplementedException("Type not implemented for " + sprites[cardIndex].name);

        return cardType;
    }


    private void Update() {
        if (Input.GetKeyDown(KeyCode.F1))
            instruction_interface.SetActive(true);

        if (Input.GetKey(KeyCode.Escape))
            instruction_interface.SetActive(false);
    }

    // initialize after button click on instruction
    public void Initialize() {
        Debug.Log(SceneManager.GetActiveScene().name);
        rand = new System.Random(); //Inits random number generator

        sprites = Shuffle<Sprite>(sprites);

        alimentosCestaList = new List<GameObject>();

        algaImg.fillAmount = 1;
        plantaImg.fillAmount = 1;
        fungoImg.fillAmount = 1;

        cardIndex = 0;
        //isDone = false;

        updateCurrentCard(cardIndex);
        updateNextCard(cardIndex + 1);

        initialPosition = currentImage.transform.parent.position;

        resetButton.interactable = true;
        backButton.interactable = true;
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
    }

    override public void CheckDislike()
    {
        // do something else
        if(checkAnswer(false))
            Debug.Log("Resposta certa!");
        else
            Debug.Log("Resposta errada!");
        NextCard();
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
        winImg.SetActive(true);
        Debug.Log("Você ganhou!");
    }

    private void doLose() { //Routine to happen once the user wins
        loseImg.SetActive(true);
        Debug.Log("Você perdeu!");
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

    public void ResetScene()
    {
        //SceneManager.LoadScene(ScenesNames.M010TiposVegetacaoScene);
    }

    public void ReturnToUshuaia()
    {
        SceneManager.LoadScene(ScenesNames.M002CasaUshuaia);
    }
}
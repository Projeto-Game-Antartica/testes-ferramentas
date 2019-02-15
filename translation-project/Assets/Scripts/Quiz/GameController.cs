using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using DavyKager;

public class GameController : MonoBehaviour
{
    public Text questionDisplayText;
    public Text scoreDisplayText;
    public Text roundOverDisplayText;
    public SimpleObjectPool answerButtonObjectPool;
    public Transform answerButtonParent;
    public GameObject questionDisplay;
    public GameObject roundEndDisplay;
    public Button returnToMenuButton;

    private DataController dataController;
    private RoundData currentRoundData;
    private QuestionData[] questionPool;

    private bool isRoundActive;
    private int questionIndex;
    private int playerScore;
    private List<GameObject> answerButtonGameObjects = new List<GameObject>();

    // Use this for initialization
    void Start()
    {
        //TolkUtil.Load();

        dataController = FindObjectOfType<DataController>();
        currentRoundData = dataController.GetCurrentRoundData();
        questionPool = currentRoundData.questions;

        playerScore = 0;
        questionIndex = 0;

        ShowQuestion();
        isRoundActive = true;
    }

    private void Awake()
    {
        TolkUtil.Instructions();
        ReadableTexts.ReadText(ReadableTexts.quiz_instructions);
    }

    private void ShowQuestion()
    {
        RemoveAnswerButtons();
        QuestionData questionData = questionPool[questionIndex];
        questionDisplayText.text = questionData.questionText;

        ReadableTexts.ReadText("Questão" + (questionIndex + 1));
        ReadableTexts.ReadText(questionDisplayText.text);

        for (int i = 0; i < questionData.answers.Length; i++)
        {
            GameObject answerButtonGameObject = answerButtonObjectPool.GetObject();
            answerButtonGameObjects.Add(answerButtonGameObject);
            answerButtonGameObject.transform.SetParent(answerButtonParent);

            AnswerButton answerButton = answerButtonGameObject.GetComponent<AnswerButton>();
            answerButton.Setup(questionData.answers[i]);
        }
        
    }

    private void RemoveAnswerButtons()
    {
        while (answerButtonGameObjects.Count > 0)
        {
            answerButtonObjectPool.ReturnObject(answerButtonGameObjects[0]);
            answerButtonGameObjects.RemoveAt(0);
        }
    }

    public void AnswerButtonClicked(bool isCorrect)
    {
        if (isCorrect)
        {
            ReadableTexts.ReadText("Resposta correta!");
            playerScore += currentRoundData.pointsAddedForCorrectAnswer;
            scoreDisplayText.text = "Score: " + playerScore.ToString();
        }
        else
        {
            ReadableTexts.ReadText("Resposta incorreta!");
        }

        if (questionPool.Length > questionIndex + 1)
        {
            questionIndex++;
            ShowQuestion();
        }
        else
        {
            EndRound();
        }

    }

    public void EndRound()
    {
        isRoundActive = false;

        questionDisplay.SetActive(false);
        roundEndDisplay.SetActive(true);

        roundOverDisplayText.text = "FIM DE JOGO! VOCÊ MARCOU " + (playerScore / 10) + " PONTOS";

        ReadableTexts.ReadText(roundOverDisplayText.text);

        returnToMenuButton.Select();
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void ReturnToMenuButtonSelectEvent()
    {
        ReadableTexts.ReadText(returnToMenuButton.GetComponentInChildren<Text>().text);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            ReadableTexts.ReadText(questionDisplayText.text);
        }
    }
}
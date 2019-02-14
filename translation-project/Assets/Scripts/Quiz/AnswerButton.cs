using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DavyKager;

public class AnswerButton : MonoBehaviour
{

    public Text answerText;
    private AnswerData answerData;
    private GameController gameController;
    public Button answerButton;
    public Transform transform;

    // Use this for initialization
    void Start()
    {
        //TolkUtil.Load();

        gameController = FindObjectOfType<GameController>();
        answerButton = FindObjectOfType<Button>();

        transform.localScale = Vector3.one;

        answerButton.Select();
    }

    public void Setup(AnswerData data)
    {
        answerData = data;
        answerText.text = answerData.answerText;
    }


    public void HandleClick()
    {
        gameController.AnswerButtonClicked(answerData.isCorrect);
    }

    public void OnSelectEvent()
    {
        TolkUtil.Speak(answerData.answerText);
    }
}
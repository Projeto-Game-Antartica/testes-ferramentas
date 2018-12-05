using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
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

    private string gameText = "Inicio do Jogo. Utilize as setas cima ou baixo ou a tecla TAB" +
                              "para navegar entre as opções de resposta e a tecla ENTER para selecioná-las.";

    private bool isRoundActive;
    private float timeRemaining;
    private int questionIndex;
    private int playerScore;
    private List<GameObject> answerButtonGameObjects = new List<GameObject>();
    private EventSystem system;

    private List<Selectable> m_orderedSelectables;

    // Use this for initialization
    void Start()
    {
        TolkUtil.Load();

        dataController = FindObjectOfType<DataController>();
        currentRoundData = dataController.GetCurrentRoundData();
        questionPool = currentRoundData.questions;

        playerScore = 0;
        questionIndex = 0;

        ShowQuestion();
        isRoundActive = true;

        system = EventSystem.current;
    }

    private void Awake()
    {
        TolkUtil.Instructions();
        TolkUtil.SpeakAnyway(gameText);
    }

    private void ShowQuestion()
    {
        RemoveAnswerButtons();
        QuestionData questionData = questionPool[questionIndex];
        questionDisplayText.text = questionData.questionText;

        TolkUtil.Speak("Questão" + (questionIndex + 1));
        TolkUtil.Speak(questionDisplayText.text);

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
            TolkUtil.SpeakAnyway("Resposta correta!");
            playerScore += currentRoundData.pointsAddedForCorrectAnswer;
            scoreDisplayText.text = "Score: " + playerScore.ToString();
        }
        else
        {
            TolkUtil.SpeakAnyway("Resposta incorreta!");
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

        TolkUtil.Speak(roundOverDisplayText.text);

        returnToMenuButton.Select();
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void ReturnToMenuButtonSelectEvent()
    {
        TolkUtil.Speak(returnToMenuButton.GetComponentInChildren<Text>().text);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            TolkUtil.Speak(questionDisplayText.text);
        }

        // Navegação dos itens selecionáveis através do TAB (de um modoaprimorada para o quiz)
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            HandleHotkeySelect(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift), true, false); // Navigate backward when holding shift, else navigate forward.
        }
    }

    public void HandleHotkeySelect(bool _isNavigateBackward, bool _isWrapAround, bool _isEnterSelect)
    {
        SortSelectables();

        GameObject selectedObject = EventSystem.current.currentSelectedGameObject;
        if (selectedObject != null && selectedObject.activeInHierarchy) // Ensure a selection exists and is not an inactive object.
        {
            Selectable currentSelection = selectedObject.GetComponent<Selectable>();
            if (currentSelection != null)
            {
                if (_isEnterSelect)
                {
                    if (currentSelection.GetComponent<InputField>() != null)
                    {
                        ApplyEnterSelect(FindNextSelectable(m_orderedSelectables.IndexOf(currentSelection), _isNavigateBackward, _isWrapAround));
                    }
                }
                else // Tab select.
                {
                    Selectable nextSelection = FindNextSelectable(m_orderedSelectables.IndexOf(currentSelection), _isNavigateBackward, _isWrapAround);
                    if (nextSelection != null)
                    {
                        nextSelection.Select();
                    }
                }
            }
            else
            {
                SelectFirstSelectable(_isEnterSelect);
            }
        }
        else
        {
            SelectFirstSelectable(_isEnterSelect);
        }
    }

    ///<summary> Selects an input field or button, activating the button if one is found. </summary>
    private void ApplyEnterSelect(Selectable _selectionToApply)
    {
        if (_selectionToApply != null)
        {
            if (_selectionToApply.GetComponent<InputField>() != null)
            {
                _selectionToApply.Select();
            }
            else
            {
                Button selectedButton = _selectionToApply.GetComponent<Button>();
                if (selectedButton != null)
                {
                    _selectionToApply.Select();
                    selectedButton.OnPointerClick(new PointerEventData(EventSystem.current));
                }
            }
        }
    }

    private void SelectFirstSelectable(bool _isEnterSelect)
    {
        if (m_orderedSelectables.Count > 0)
        {
            Selectable firstSelectable = m_orderedSelectables[0];
            if (_isEnterSelect)
            {
                ApplyEnterSelect(firstSelectable);
            }
            else
            {
                firstSelectable.Select();
            }
        }
    }

    private Selectable FindNextSelectable(int _currentSelectableIndex, bool _isNavigateBackward, bool _isWrapAround)
    {
        Selectable nextSelection = null;

        int totalSelectables = m_orderedSelectables.Count;
        if (totalSelectables > 1)
        {
            if (_isNavigateBackward)
            {
                if (_currentSelectableIndex == 0)
                {
                    nextSelection = (_isWrapAround) ? m_orderedSelectables[totalSelectables - 1] : null;
                }
                else
                {
                    nextSelection = m_orderedSelectables[_currentSelectableIndex - 1];
                }
            }
            else // Navigate forward.
            {
                if (_currentSelectableIndex == (totalSelectables - 1))
                {
                    nextSelection = (_isWrapAround) ? m_orderedSelectables[0] : null;
                }
                else
                {
                    nextSelection = m_orderedSelectables[_currentSelectableIndex + 1];
                }
            }
        }

        return (nextSelection);
    }

    private List<Selectable> SortSelectables()
    {
        List<Selectable> originalSelectables = Selectable.allSelectables;
        int totalSelectables = originalSelectables.Count;
        m_orderedSelectables = new List<Selectable>(totalSelectables);
        for (int index = 0; index < totalSelectables; ++index)
        {
            Selectable selectable = originalSelectables[index];
            m_orderedSelectables.Insert(FindSortedIndexForSelectable(index, selectable), selectable);
        }

        return m_orderedSelectables;
    }

    ///<summary> Recursively finds the sorted index by positional order within m_orderedSelectables (positional order is determined from left-to-right followed by top-to-bottom). </summary>
    private int FindSortedIndexForSelectable(int _selectableIndex, Selectable _selectableToSort)
    {
        int sortedIndex = _selectableIndex;
        if (_selectableIndex > 0)
        {
            int previousIndex = _selectableIndex - 1;
            Vector3 previousSelectablePosition = m_orderedSelectables[previousIndex].transform.position;
            Vector3 selectablePositionToSort = _selectableToSort.transform.position;

            if (previousSelectablePosition.y == selectablePositionToSort.y)
            {
                if (previousSelectablePosition.x > selectablePositionToSort.x)
                {
                    // Previous selectable is in front, try the previous index:
                    sortedIndex = FindSortedIndexForSelectable(previousIndex, _selectableToSort);
                }
            }
            else if (previousSelectablePosition.y < selectablePositionToSort.y)
            {
                // Previous selectable is in front, try the previous index:
                sortedIndex = FindSortedIndexForSelectable(previousIndex, _selectableToSort);
            }
        }

        return (sortedIndex);
    }
}
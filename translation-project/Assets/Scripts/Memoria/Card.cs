using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Card : AbstractScreenReader, ISelectHandler {

    public const int VIRADA_BAIXO = 0;
    public const int VIRADA_CIMA  = 1;

    public static bool DO_NOT = false;

    public int state { get; set; }
    public int cardValue { get; set; }
    public bool initialized { get; set; }
    public bool isText { get; set; }
   
    private Sprite cardBack;
    public Sprite cardFace;
    public Sprite cardText;

    private GameObject memoryManager;

    public Image BGImage;

    private bool _init = false;

    private void Start()
    {
        state = VIRADA_BAIXO;
        initialized = false;
        memoryManager = GameObject.FindGameObjectWithTag("GameController");

        //StartCoroutine(showCards());
    }

    //public IEnumerator showCards()
    //{
    //    yield return new WaitForSeconds(9);
    //    if(!isText) state = 0;
    //    turnCardDown();
    //}

    public void setupGraphics(int choice)
    {
        cardBack = memoryManager.GetComponent<MemoryManager>().getCardBack();
        if (choice == MemoryManager.CARDFACE)
        {
            cardFace = memoryManager.GetComponent<MemoryManager>().getCardFace(cardValue);
            cardText = null;
            gameObject.name += ": " + cardFace.name;
            isText = false;
        }
        else if (choice == MemoryManager.CARDTEXT)
        {
            cardText = memoryManager.GetComponent<MemoryManager>().getCardText(cardValue);
            cardFace = null;
            gameObject.name += ": " + cardText.name;
            isText = true;
        }
        //state = 1;

        flipCard();

        _init = true;
    }

    public void flipCard()
    {
        //if (isText)
        //{
        //    state = 1;
        //    GetComponent<Image>().sprite = cardText;
        //}

        if (state == VIRADA_BAIXO)
            state = VIRADA_CIMA;
        else if (state == VIRADA_CIMA)
            state = VIRADA_BAIXO;

        if (state == VIRADA_BAIXO && !DO_NOT)
        {
            GetComponent<Image>().sprite = cardBack;

            if(isText)
                GetComponent<Image>().sprite = cardText;
        }
        else if (state == VIRADA_CIMA && !DO_NOT)
        {
            if (cardText == null)
                GetComponent<Image>().sprite = cardFace;
            else
                GetComponent<Image>().sprite = cardText;

            if (_init)
            {
                string objectName = CardsDescription.GetCardDescription(gameObject.name);

                ReadAndDebugCardText(objectName);
            }
        }
    }

    public void falseCheck()
    {
        StartCoroutine(pause());
    }

    public void turnCardDown()
    {

        if(!isText) GetComponent<Image>().sprite = cardBack;

        state = VIRADA_BAIXO;
        DO_NOT = false;
    }

    IEnumerator pause()
    {
        yield return new WaitForSeconds(2f);

        if (state == VIRADA_BAIXO)
        {
            if(isText)
                GetComponent<Image>().sprite = cardText;
            else
                GetComponent<Image>().sprite = cardBack;
        }
        else if (state == VIRADA_CIMA)
        {
            if (cardText == null)
                GetComponent<Image>().sprite = cardFace;
            else
                GetComponent<Image>().sprite = cardText;
        }

        DO_NOT = false;
    }

    public void OnSelect(BaseEventData eventData)
    {
        //Debug.Log(state);

        if (state == VIRADA_BAIXO || state == VIRADA_CIMA && !isText)
        {
            //Debug.Log(gameObject.name.Substring(0, gameObject.name.IndexOf(":")));
            ReadText(gameObject.name.Substring(0, gameObject.name.IndexOf(":")));
        }
        else
        {
            string objectName = CardsDescription.GetCardDescription(gameObject.name);
            ReadAndDebugCardText(objectName);
        }
    }

    public void ReadAndDebugCardText(string objectName)
    {
        // numero da carta + descrição ou numero da carta + nome do animal
        //Debug.Log(objectName != null ? (gameObject.name.Substring(0, gameObject.name.IndexOf(":")) + ": " + objectName) : gameObject.name);
        ReadText(objectName != null ? (gameObject.name.Substring(0, gameObject.name.IndexOf(":")) + ": " + objectName) : gameObject.name);
    }

    public void ChangeDisabledCardColor(Button button, Color color)
    {
        var newCardColor = button.colors;
        newCardColor.disabledColor = color;
        button.colors = newCardColor;
    }
}

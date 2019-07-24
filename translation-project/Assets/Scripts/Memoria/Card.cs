using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Card : AbstractScreenReader, ISelectHandler {

    public static bool DO_NOT = false;

    public int state { get; set; }
    public int cardValue { get; set; }
    public bool initialized { get; set; }

    private Sprite cardBack;
    public Sprite cardFace;
    public Sprite cardText;

    private GameObject memoryManager;

    private bool _init = false;

    private void Start()
    {
        state = 0;
        initialized = false;
        memoryManager = GameObject.FindGameObjectWithTag("GameController");

        StartCoroutine(showCards());
    }

    public IEnumerator showCards()
    {
        yield return new WaitForSeconds(2);

        flipCard();
    }

    public void setupGraphics(int choice)
    {
        cardBack = memoryManager.GetComponent<MemoryManager>().getCardBack();
        if (choice == MemoryManager.CARDFACE)
        {
            cardFace = memoryManager.GetComponent<MemoryManager>().getCardFace(cardValue);
            cardText = null;
            gameObject.name += ": " + cardFace.name;
        }
        else if (choice == MemoryManager.CARDTEXT)
        {
            cardText = memoryManager.GetComponent<MemoryManager>().getCardText(cardValue);
            cardFace = null;
            gameObject.name += ": " + cardText.name;
        }

        flipCard();

        _init = true;
    }

    public void flipCard()
    {
        if (state == 0)
            state = 1;
        else if (state == 1)
            state = 0;

        if (state == 0 && !DO_NOT)
            GetComponent<Image>().sprite = cardBack;
        else if (state == 1 && !DO_NOT)
        {
            if (cardText == null)
                GetComponent<Image>().sprite = cardFace;
            else
                GetComponent<Image>().sprite = cardText;

            if(_init)
            {
                string objectName = CardsDescription.GetCardDescription(gameObject.name);

                if (objectName != null)
                    //Debug.Log(objectName);
                    ReadText(objectName);
                else
                    //Debug.Log(gameObject.name);
                    ReadText(gameObject.name);
            }
        }
    }

    public void falseCheck()
    {
        StartCoroutine(pause());
    }

    public void turnCardDown()
    {
        GetComponent<Image>().sprite = cardBack;

        DO_NOT = false;
    }

    IEnumerator pause()
    {
        yield return new WaitForSeconds(1);

        if (state == 0)
            GetComponent<Image>().sprite = cardBack;
        else if (state == 1)
        {
            if(cardText == null)
                GetComponent<Image>().sprite = cardFace;
            else
                GetComponent<Image>().sprite = cardText;
        }

        DO_NOT = false;
    }

    public void OnSelect(BaseEventData eventData)
    {
        //Debug.Log(gameObject.name.Substring(0, gameObject.name.IndexOf(":")));
        ReadText(gameObject.name.Substring(0, gameObject.name.IndexOf(":")));
    }
}

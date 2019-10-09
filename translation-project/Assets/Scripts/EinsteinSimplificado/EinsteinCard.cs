using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EinsteinCard : AbstractScreenReader, ISelectHandler
{

    public const int VIRADA_BAIXO = 0;
    public const int VIRADA_CIMA = 1;

    public static bool DO_NOT = false;

    public int state { get; set; }
    public int cardValue { get; set; }
    public bool initialized { get; set; }

    private Sprite cardBack;
    public Sprite cardFace;

    private GameObject einsteinManager;

    public Image BGImage;

    private bool _init = false;

    private void Start()
    {
        state = VIRADA_BAIXO;
        initialized = false;
        einsteinManager = GameObject.FindGameObjectWithTag("GameController");

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
        cardBack = einsteinManager.GetComponent<EinsteinManager>().getCardBack();
        cardFace = einsteinManager.GetComponent<EinsteinManager>().getCardFace(cardValue);
        gameObject.name += ": " + cardFace.name;
        //state = 1;

        flipCard();

        _init = true;
    }

    public void flipCard()
    {
        if (state == VIRADA_BAIXO)
            state = VIRADA_CIMA;
        else if (state == VIRADA_CIMA)
            state = VIRADA_BAIXO;

        if (state == VIRADA_BAIXO && !DO_NOT)
        {
            GetComponent<Image>().sprite = cardBack;
        }
        else if (state == VIRADA_CIMA && !DO_NOT)
        {
            GetComponent<Image>().sprite = cardFace;

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
        state = VIRADA_BAIXO;
        DO_NOT = false;
    }

    IEnumerator pause()
    {
        yield return new WaitForSeconds(2f);

        if (state == VIRADA_BAIXO)
        {
            GetComponent<Image>().sprite = cardBack;
        }
        else if (state == VIRADA_CIMA)
        {
            GetComponent<Image>().sprite = cardFace;
        }

        DO_NOT = false;
    }

    public void OnSelect(BaseEventData eventData)
    {
        //Debug.Log(state);

        if (state == VIRADA_BAIXO || state == VIRADA_CIMA)
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

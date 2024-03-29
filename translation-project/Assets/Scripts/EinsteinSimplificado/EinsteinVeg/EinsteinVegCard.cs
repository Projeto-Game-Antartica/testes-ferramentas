﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EinsteinVegCard : AbstractScreenReader, ISelectHandler
{

    public const int VIRADA_BAIXO = 0;
    public const int VIRADA_CIMA = 1;

    public static bool DO_NOT = false;

    public bool added { get; set; }

    public int state { get; set; }
    public int cardValue { get; set; }

    public string cardText {get; set;}
    public bool initialized { get; set; }

    public string cardDescription {
        get {
            // numero da carta + descrição ou numero da carta + nome do animal
            string cardName = gameObject.name.Substring(0, gameObject.name.IndexOf(":"));
            return cardName + " " + cardText;
        }
    }

    public Sprite cardBack;
    public Sprite cardFace;

    public EinsteinVegManager einsteinManager;

    public Image BGImage;

    private bool _init = false;

    public GameObject proccessTextPrefab;

    private void Start()
    {
        state = VIRADA_BAIXO;
        //initialized = false;
        //added = false;
        StartCoroutine(showCards());
    }

    public IEnumerator showCards()
    {
        yield return new WaitForSeconds(0.5f);

        flipCard();
    }

    public void setupGraphics() {
        //cardBack = einsteinManager.getCardBack();
        //cardBack = einsteinManager.getCardFace(cardValue);
        //cardFace = einsteinManager.getCardFace(cardValue);
        
        GameObject text = Instantiate(proccessTextPrefab);
        text.transform.SetParent(transform, false);
        text.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = cardText;
        text.GetComponent<HighContrastText>().HasVideo = true;


        GetComponent<Image>().color = new Color(1, 1, 1, 0);

        gameObject.name += ": " + cardFace.name;

        flipCard();

        _init = true;
    }

    public void flipCard()
    {
        if(DO_NOT)
            return;

        if (state == VIRADA_BAIXO) {
            state = VIRADA_CIMA;
            //GetComponent<Image>().sprite = cardBack;

        } else if (state == VIRADA_CIMA) {
            state = VIRADA_BAIXO;

            //GetComponent<Image>().sprite = cardFace;

            if (_init){
               string objectName = CardsDescription.GetCardText(gameObject.name);
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
        GetComponent<Image>().sprite = cardBack;

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
            ReadText(cardDescription);
        }
        //else
        //{
        //    string objectName = CardsDescription.GetCardText(gameObject.name);
        //    ReadAndDebugCardText(objectName);
        //}
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

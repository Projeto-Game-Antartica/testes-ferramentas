using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardDesafioPaleo : AbstractScreenReader, ISelectHandler {

    public const int VIRADA_BAIXO = 0;
    public const int VIRADA_CIMA  = 1;
    public const int solo_2  = 2;
    public const int solo_3  = 3;
    public const int fim_fossil  = 99;
    public const int limpo  = 999;

    private float perde = 33;

    public static bool DO_NOT = false;

    public int state { get; set; }
    public int cardValue { get; set; }
    public bool initialized { get; set; }
    public bool isText { get; set; }   
    public Sprite cardBack;
    public Sprite cardFace;

    private GameObject desafioManagerPaleo;

    public DesafioManagerPaleo DesafioGame;

    public Sprite[] BGImage_Solo1;

    public Image BGImage;

    private bool _init = false;

    public GameObject cardImage;

    public string missionName;

    private void Start()
    {
        state = VIRADA_BAIXO;
        initialized = false;
        desafioManagerPaleo = GameObject.FindGameObjectWithTag("GameController");

        //StartCoroutine(showCards());
    }

    private void warningMessage(string message) {
        //Debug.Log(message);
        desafioManagerPaleo.GetComponent<DesafioManagerPaleo>().ShowOkDialog(message);
    }

    public void setupGraphics(int choice)
    {
        //cardBack = desafioManagerPaleo.GetComponent<DesafioManagerPaleo>().getCardBack();

            Debug.Log("teste solo");
            cardFace = desafioManagerPaleo.GetComponent<DesafioManagerPaleo>().getCardFace(cardValue);
            Debug.Log("teste solo: " +cardFace.name);
            BGImage_Solo1[3] = cardFace;
            //cardText = null;
            //gameObject.name += ": " + cardFace.name;
            Debug.Log("log: " +gameObject.name);
            //isText = false;
          
        //flipCard();

        _init = true;
    }

    public void flipCard()
    {
        desafioManagerPaleo.GetComponent<DesafioManagerPaleo>().VerItem();

        if(!desafioManagerPaleo.GetComponent<DesafioManagerPaleo>().Itens.Contains("luva"))
            {
             desafioManagerPaleo.GetComponent<DesafioManagerPaleo>().lifeExpController.HPImage.fillAmount += -perde / 100;
              Debug.Log("Perde Vida");
             if(desafioManagerPaleo.GetComponent<DesafioManagerPaleo>().lifeExpController.HPImage.fillAmount <= 0.1)
             {
                desafioManagerPaleo.GetComponent<DesafioManagerPaleo>().LoseImage.SetActive(true);
                desafioManagerPaleo.GetComponent<DesafioManagerPaleo>().resetButton.Select();
                desafioManagerPaleo.GetComponent<DesafioManagerPaleo>().lifeExpController.AddEXP(0.0001f); // jogou um minijogo
                desafioManagerPaleo.GetComponent<DesafioManagerPaleo>().LoseText.text = "Você zerou seus pontos de saúde. Aguarde uma pontuação mínima para retornar ao desafio.";
			 }
            }

        if(state == fim_fossil && (desafioManagerPaleo.GetComponent<DesafioManagerPaleo>().Itens.Contains("martelo") || desafioManagerPaleo.GetComponent<DesafioManagerPaleo>().Itens.Contains("talhadeira")))
        {
            desafioManagerPaleo.GetComponent<DesafioManagerPaleo>().quebra_fossil = desafioManagerPaleo.GetComponent<DesafioManagerPaleo>().quebra_fossil + 1;

            if(desafioManagerPaleo.GetComponent<DesafioManagerPaleo>().quebra_fossil >= 3)
            {
                desafioManagerPaleo.GetComponent<DesafioManagerPaleo>().LoseImage.SetActive(true);
                desafioManagerPaleo.GetComponent<DesafioManagerPaleo>().resetButton.Select();
                desafioManagerPaleo.GetComponent<DesafioManagerPaleo>().lifeExpController.AddEXP(0.0001f); // jogou um minijogo

                desafioManagerPaleo.GetComponent<DesafioManagerPaleo>().LoseText.text = "Fóssil destruído, procure outro afloramento.";
                ReadText("Fóssil destruído, procure outro afloramento.");

                desafioManagerPaleo.GetComponent<DesafioManagerPaleo>().audioSource.PlayOneShot(desafioManagerPaleo.GetComponent<DesafioManagerPaleo>().som_quebra_fossil);
            }
        }

        if (desafioManagerPaleo.GetComponent<DesafioManagerPaleo>().Itens.Contains("martelo") && desafioManagerPaleo.GetComponent<DesafioManagerPaleo>().Itens.Contains("talhadeira"))
        {
                      
            if (state == VIRADA_BAIXO && !DO_NOT)
            {
                GetComponent<Image>().sprite = BGImage_Solo1[1];
                state = VIRADA_CIMA;
                desafioManagerPaleo.GetComponent<DesafioManagerPaleo>().audioSource.PlayOneShot(desafioManagerPaleo.GetComponent<DesafioManagerPaleo>().som_quebra_solo1);

            }
            else if (state == VIRADA_CIMA && !DO_NOT)
            {
                GetComponent<Image>().sprite = BGImage_Solo1[2];
                state = solo_2;

                desafioManagerPaleo.GetComponent<DesafioManagerPaleo>().audioSource.PlayOneShot(desafioManagerPaleo.GetComponent<DesafioManagerPaleo>().som_quebra_solo2);
            }
            else if (state == solo_2 && !DO_NOT && BGImage_Solo1[3] != null)
            {
                state = fim_fossil;
                GetComponent<Image>().sprite = BGImage_Solo1[3];   
                GetComponent<Image>().color = new Color(1, 1, 0, 1);
                desafioManagerPaleo.GetComponent<DesafioManagerPaleo>().acha_fossil = desafioManagerPaleo.GetComponent<DesafioManagerPaleo>().acha_fossil + 1;

		    }
            else if (state == solo_2 && !DO_NOT && BGImage_Solo1[3] == null)
            {
                GetComponent<Image>().sprite = BGImage_Solo1[4];  
                state = solo_3;
                desafioManagerPaleo.GetComponent<DesafioManagerPaleo>().audioSource.PlayOneShot(desafioManagerPaleo.GetComponent<DesafioManagerPaleo>().som_quebra_solo3);
		    }
        }

        if(state == fim_fossil && desafioManagerPaleo.GetComponent<DesafioManagerPaleo>().Itens.Contains("pincel") && !desafioManagerPaleo.GetComponent<DesafioManagerPaleo>().Itens.Contains("martelo") && !desafioManagerPaleo.GetComponent<DesafioManagerPaleo>().Itens.Contains("talhadeira"))
        {
            desafioManagerPaleo.GetComponent<DesafioManagerPaleo>().limpa_fossil = desafioManagerPaleo.GetComponent<DesafioManagerPaleo>().limpa_fossil + 1;
            state = limpo;
            GetComponent<Image>().sprite = BGImage_Solo1[3];
            GetComponent<Image>().color = new Color(1, 1, 1, 1);

                DesafioGame.verificaAcha_Limpa();
            
        }

        if(desafioManagerPaleo.GetComponent<DesafioManagerPaleo>().acha_fossil == 4)
        {
            if (desafioManagerPaleo.GetComponent<DesafioManagerPaleo>().encontro)
            {
                desafioManagerPaleo.GetComponent<DesafioManagerPaleo>().audioSource.PlayOneShot(desafioManagerPaleo.GetComponent<DesafioManagerPaleo>().correctAudio);
                warningMessage("Parabéns, fóssil encontrado. Agora você precisa limpá-lo.");
                desafioManagerPaleo.GetComponent<DesafioManagerPaleo>().encontro = false;
            }

        }
    }

    public void turnCardDown()
    {

        GetComponent<Image>().sprite = cardBack;
        Debug.Log("setou cartas");

        state = VIRADA_BAIXO;
        DO_NOT = false;
    }

    public void OnSelect(BaseEventData eventData)
    {
        //Debug.Log(state);

        if (state == VIRADA_BAIXO || state == VIRADA_CIMA && !isText)
        {
            //Debug.Log(gameObject.name.Substring(0, gameObject.name.IndexOf(":")));
            //ReadText(gameObject.name.Substring(0, gameObject.name.IndexOf(":")));
        }
        else
        {
            string objectName = CardsDescriptionPaleo.GetCardDescriptionPaleo(gameObject.name);
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

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

    public GameObject memoryCardBackImage;
    public GameObject memoryCardBackText;

    public GameObject cardImage;

    public string missionName;
    
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
    //    if (!isText) state = 0;
    //    turnCardDown();
    //}

    public void setupGraphics(int choice)
    {
        cardBack = memoryManager.GetComponent<MemoryManager>().getCardBack();

        if (choice == MemoryManager.CARDFACE)
        {
            Debug.Log("card face");
            Debug.Log("card value: " +cardValue);
            cardFace = memoryManager.GetComponent<MemoryManager>().getCardFace(cardValue);
            cardText = null;
            gameObject.name += ": " + cardFace.name;
            isText = false;

            //GetComponent<Image>().color = new Color(1, 1, 1, 0);

            cardImage = Instantiate(memoryCardBackImage, transform, false);

            cardImage.GetComponentsInChildren<Image>()[1].sprite = cardFace;
            cardImage.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = cardFace.name;
        }
        else if (choice == MemoryManager.CARDTEXT)
        {
            cardText = memoryManager.GetComponent<MemoryManager>().getCardText(cardValue);
            cardFace = null;
            gameObject.name += ": " + cardText.name;
            isText = true;

            //GameObject obj = Instantiate(memoryCardBackText, transform, false);
            // create prefab
            GetComponent<Image>().color = new Color(1, 1, 1, 0);
            GameObject obj = Instantiate(memoryCardBackText, BGImage.transform, false);
            // set the card to prefab's child due the selectable
            transform.SetParent(obj.transform, true);
            // add the corresponding text

            switch(missionName)
            {
                case "baleias":
                    obj.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = CardsDescription.GetCardText(cardText.name);
                    break;
                case "paleo":
                    obj.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = CardsDescriptionPaleo.GetCardDescriptionPaleo(cardText.name);
                    break;
                default:
                    Debug.Log("check mission name");
                    break;
            }
        }
        //state = 1;

        //flipCard();

        _init = true;
    }

    public void flipCard()
    {
        //if (isText)
        //{
        //    state = 1;
        //    GetComponent<Image>().sprite = cardText;
        //}

        // change the state of card
        if (state == VIRADA_BAIXO && !DO_NOT)
            state = VIRADA_CIMA;
        else if (state == VIRADA_CIMA && !DO_NOT)
            state = VIRADA_BAIXO;

        // set the sprites according to state
        if (state == VIRADA_BAIXO && !DO_NOT)
        {
            if (isText)
                GetComponent<Image>().sprite = cardText;
            else
            { 
                GetComponent<Image>().sprite = cardBack;
                GetComponent<Image>().color = new Color(1, 1, 1, 1);
                cardImage.SetActive(false);
            }
        }
        else if (state == VIRADA_CIMA && !DO_NOT)
        {
            if (cardText == null)
            {
                GetComponent<Image>().sprite = cardFace;
                GetComponent<Image>().color = new Color(1, 1, 1, 0);
                cardImage.SetActive(true);
            }
            else
                GetComponent<Image>().sprite = cardText;

            if (_init)
            {
                string objectName;

                switch (missionName)
                {
                    case "baleias":
                        objectName = CardsDescription.GetCardText(gameObject.name);
                        break;
                    case "paleo":
                        objectName = CardsDescriptionPaleo.GetCardDescriptionPaleo(gameObject.name);
                        break;
                    default:
                        objectName = "";
                        Debug.Log("check mission name");
                        break;
                }

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

        if (!isText)
        {
            GetComponent<Image>().sprite = cardBack;
            GetComponent<Image>().color = new Color(1, 1, 1, 1);
            cardImage.SetActive(false);
        }

        state = VIRADA_BAIXO;
        DO_NOT = false;
    }

    // wait seconds to play another "round"
    IEnumerator pause()
    {
        yield return new WaitForSeconds(2f);

        if (state == VIRADA_BAIXO)
        {
            if (isText)
                GetComponent<Image>().sprite = cardText;
            else
            {
                GetComponent<Image>().sprite = cardBack;
                GetComponent<Image>().color = new Color(1, 1, 1, 1);
                cardImage.SetActive(false);
            }
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
        string objectName = "";

        switch (missionName)
        {
            case "baleias":
                objectName = CardsDescription.GetCardText(gameObject.name);
                break;
            case "paleo":
                objectName = CardsDescriptionPaleo.GetCardDescriptionPaleo(gameObject.name);
                break;
            default:
                objectName = "";
                Debug.Log("check mission name");
                break;
        }

        Debug.Log(state);

        if ((state == VIRADA_BAIXO) && !isText) // carta virada pra baixo sem ser texto
        {
            Debug.Log(gameObject.name.Substring(0, gameObject.name.IndexOf(":")) + " de 18");
            ReadText(gameObject.name.Substring(0, gameObject.name.IndexOf(":")) + " de 18");
        }
        else if (state == 2) // ja foi encontrado o par da carta selecionada
        {
            Debug.Log("Carta já combinada.");
            ReadText("Carta já combinada.");
            
            ReadAndDebugCardText(objectName);
        }
        else // cartas de texto
        {
            ReadAndDebugCardText(objectName);
        }
    }

    public void ReadAndDebugCardText(string objectName)
    {
        string animalDescription = string.Empty;
        try
        {
            animalDescription = CardsDescription.GetCardAudioDescription(gameObject.name.Substring(gameObject.name.IndexOf(":") + 2).ToLower());
        }
        catch(System.Exception ex)
        {
            Debug.Log("its a text >>>> " + ex.StackTrace);
        }

        Debug.Log(animalDescription);

        // numero da carta + descrição ou numero da carta + nome do animal
        string resultText  = gameObject.name.Substring(0, gameObject.name.IndexOf(":")) + " de 18" + ": " + objectName;

        // numero da carta
        string resultImage = gameObject.name.Substring(0, gameObject.name.IndexOf(":")) + " de 18" + gameObject.name.Substring(gameObject.name.IndexOf(":"))
                            + objectName + ": " + animalDescription;

        Debug.Log(objectName != null ? resultText : resultImage);
        ReadText(objectName != null ? resultText : resultImage);
    }

    public void ChangeDisabledCardColor(Button button, Color color)
    {
        var newCardColor = button.colors;
        newCardColor.disabledColor = color;
        button.colors = newCardColor;
    }
}

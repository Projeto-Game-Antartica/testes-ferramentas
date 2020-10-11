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
    private Sprite cardFace;
    private Sprite cardText; 
    private GameObject cardImage;
    private GameObject hct;
    private GameObject memoryManager;
    private bool _init = false;

    public Sprite cardBg;
    public Image BGImage;
    public GameObject memoryCardBackImage;
    public GameObject highContrastText;
    public string missionName;

    private void Start()
    {
        Parameters.HIGH_CONTRAST = true;
        state = VIRADA_BAIXO;
        initialized = false;
        memoryManager = GameObject.FindGameObjectWithTag("GameController");
    }

    public void setupGraphics(int choice)
    {
        cardBack = memoryManager.GetComponent<MemoryManager>().getCardBack();

        hct = Instantiate(highContrastText, BGImage.transform, false);

        hct.GetComponent<HighContrastText>().HasVideo = true;

        // set image cards
        if (choice == MemoryManager.CARDFACE)
        {
            Debug.Log("card face");
            Debug.Log("card value: " +cardValue);

            cardFace = memoryManager.GetComponent<MemoryManager>().getCardFace(cardValue);
            cardText = null;

            gameObject.name += ": " + cardFace.name;
            isText = false;

            // instantiate cardImage prefab
            cardImage = Instantiate(memoryCardBackImage, transform, false);
            cardImage.GetComponent<Image>().sprite = cardFace;
            hct.GetComponentInChildren<TextMeshProUGUI>().text = cardFace.name;

            // seted by analysing prefab on scene
            hct.GetComponent<RectTransform>().offsetMin = new Vector2(18, 15); // new Vector2(left, bottom); 
            hct.GetComponent<RectTransform>().offsetMax = new Vector2(-17.5f, -120); // new Vector2(-right, -top);

            hct.SetActive(false);
        }
        else if (choice == MemoryManager.CARDTEXT)
        {
            cardText = memoryManager.GetComponent<MemoryManager>().getCardText(cardValue);
            cardFace = null;
            gameObject.name += ": " + cardText.name;
            isText = true;
            GetComponent<Image>().sprite = cardBg;

            // seted by analysing prefab on scene
            hct.GetComponent<RectTransform>().offsetMin = new Vector2(17.5f, 17.5f); // new Vector2(left, bottom); 
            hct.GetComponent<RectTransform>().offsetMax = new Vector2(-17.5f, -17.5f); // new Vector2(-right, -top);

            Button b = hct.AddComponent<Button>() as Button;

            // add button event from card
            b.onClick.AddListener(flipCard);

            // change navigation mode
            Navigation nav = new Navigation();
            nav.mode = Navigation.Mode.None;
            b.navigation = nav;

            // change colors
            var colors = GetComponent<Button>().colors;
            b.colors = colors;

            // add the corresponding text to compare with images
            switch (missionName)
            {
                case "baleias":
                    hct.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = CardsDescription.GetCardText(cardText.name);
                    break;
                case "paleo":
                    hct.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = CardsDescriptionPaleo.GetCardDescriptionPaleo(cardText.name);
                    break;
                default:
                    Debug.Log("check mission name");
                    break;
            }
        }

        // add text to last component on hierarchy
        hct.transform.SetAsLastSibling();

        // adjust text components
        hct.GetComponentInChildren<TextMeshProUGUI>().enableAutoSizing = true;
        hct.GetComponentInChildren<TextMeshProUGUI>().fontSizeMin = 10;
        hct.GetComponentInChildren<TextMeshProUGUI>().fontSizeMax = 24;
        hct.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;

        _init = true;
    }

    public void flipCard()
    {
        // change the state of card
        if (state == VIRADA_BAIXO && !DO_NOT)
            state = VIRADA_CIMA;
        else if (state == VIRADA_CIMA && !DO_NOT)
            state = VIRADA_BAIXO;

        // set the sprites according to state
        if (state == VIRADA_BAIXO && !DO_NOT)
        {
            // close the card
            if (!isText)
            {
                GetComponent<Image>().sprite = cardBack;
                cardImage.SetActive(false);
                hct.SetActive(false);
            }
        }
        else if (state == VIRADA_CIMA && !DO_NOT)
        {
            if (cardText == null)
            {
                GetComponent<Image>().sprite = cardBg;
                cardImage.SetActive(true);
                hct.SetActive(true);
            }

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
            hct.SetActive(false);
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
                GetComponent<Image>().sprite = cardBg;
            else
            {
                GetComponent<Image>().sprite = cardBack;
                GetComponent<Image>().color = new Color(1, 1, 1, 1);
                cardImage.SetActive(false);
                hct.SetActive(false);
            }
        }
        else if (state == VIRADA_CIMA)
        {
            if (cardText == null)
                GetComponent<Image>().sprite = cardFace;
            else
                GetComponent<Image>().sprite = cardBg;
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
            switch(missionName)
            {
                case "baleias":
                    animalDescription = CardsDescription.GetCardAudioDescription(gameObject.name.Substring(gameObject.name.IndexOf(":") + 2).ToLower());
                    break;
                case "paleo":
                    animalDescription = CardsDescriptionPaleo.GetCardAudioDescription(gameObject.name.Substring(gameObject.name.IndexOf(":") + 2).ToLower());
                    break;
                default:
                    break;
            }
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

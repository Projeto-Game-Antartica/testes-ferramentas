using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Regras : AbstractCardManager {

    private int likeCount = 0;
    private int cardCount = 0;

    private int rulesNumber = 15;
    private int totalCards = 30;
    private int cardsNumber;

    //private bool[] selectedCards;

    public AudioSource audioSource;

    public AudioClip closeClip;
    public AudioClip avisoClip;
    public AudioClip victoryClip;
    public AudioClip loseClip;

    public TextMeshProUGUI CardLeft;
    public TextMeshProUGUI CardCount;

    public GameObject winImage;
    public GameObject loseImage;

    public GameObject instruction_interface;

    public Button audioButton;
    public Button resetButton;
    public Button backButton;

    public Image heartImage;
    public Image starImage;
    public Image antarticaImage;

    public GameObject textPrefab;
    public GameObject confirmQuit;

    public LifeExpController lifeExpController;

    public MinijogoIconsController iconsController;

    private void Update()
    {
        if (Input.GetKeyDown(InputKeys.INSTRUCTIONS_KEY))
        {
            instruction_interface.SetActive(true);
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (instruction_interface.activeSelf)
            {
                audioSource.PlayOneShot(closeClip);
                instruction_interface.SetActive(false);
            }
            else
            {
                TryReturnToUshuaia();
            }
        }

        if(Input.GetKeyDown(InputKeys.PARAMETERS_KEY))
        {
            lifeExpController.ReadHPandEXP();
        }

        if (Input.GetKeyDown(InputKeys.MJMENU_KEY))
        {
            audioButton.Select();
        }

        if (Input.GetKeyDown(KeyCode.F6))
        {
            likeButton.Select();
        }

        if (Input.GetKeyDown(InputKeys.AUDIODESCRICAO_KEY))
        {
            // audiodescricao
        }

        if (Input.GetKeyDown(InputKeys.REPEAT_KEY))
        {
            ReadCard(cardIndex);
        }
    }

    // after instructions, initialize the game
    public void InitializeGame()
    {
        cardIndex = 0;

        //selectedCards = new bool[sprites.Length];

        // selectedCards starts with false value
        //for (int i = 0; i < selectedCards.Length; i++)
        //    selectedCards[i] = false;

        cardsNumber = sprites.Length;

        // current card initial settings
        currentImage.sprite = sprites[cardIndex];
        currentImage.color = new Color(1, 1, 1, 0);
        currentImage.name = sprites[cardIndex].name;
        //cardName.text = currentImage.name;
        Instantiate(textPrefab, currentImage.transform, false);
        currentImage.GetComponentInChildren<TextMeshProUGUI>().text = RegrasText.GetRegra(cardIndex);

        ReadCard(cardIndex);

        // next card initial settings
        nextImage.GetComponentInChildren<Image>().sprite = sprites[cardIndex + 1];
        nextImage.color = new Color(1, 1, 1, 0);
        //nextImage.name = sprites[cardIndex + 1].name;
        Instantiate(textPrefab, nextImage.transform, false);
        nextImage.GetComponentInChildren<TextMeshProUGUI>().text = RegrasText.GetRegra(cardIndex+1);

        initialPosition = currentImage.transform.parent.position;

        resetButton.interactable = true;
        backButton.interactable = true;

        likeButton.Select();
    }

    public override void CheckDislike()
    {
        cardCount++;

        //if (cardCount == totalCards || cardCount == cardsNumber)
        //    cardCount = 0;
        
        CardLeft.text = "Regras restantes: " + (cardsNumber - cardCount);
        
        NextCard();
    }

    public override void CheckLike()
    {
        cardCount++;
        likeCount++;

        //if (cardCount == totalCards || cardCount == cardsNumber)
        //    cardCount = 0;

        CardLeft.text = "Regras restantes: " + (cardsNumber - cardCount);
        CardCount.text = "Regras escolhidas: " + likeCount + "/" + rulesNumber;

        cardsPoints(currentImage.name);

        // card is selected
        //if(cardIndex >= 0 && cardIndex < totalCards)
        //    selectedCards[cardIndex] = true;

        NextCard();

        Debug.Log(likeCount);

        if (likeCount >= rulesNumber)
        {
            StartCoroutine(EndGame(true));
        }
    }

    //private int GetCardsLeft()
    //{
    //    int result = 0;

    //    for (int i = 0; i < selectedCards.Length; i++)
    //        if (selectedCards[i] == false)
    //            result++;

    //    return result - 1;
    //}

    // set the experience points
    public void cardsPoints(string cardName)
    {
        switch (cardName.ToLower())
        {
            case "regra1":
                iconsController.AddPoints(0f, 0.04f, 0.5f);
                break;
            case "regra2":
                iconsController.AddPoints(0f, 0.04f, 0.5f);
                break;
            case "regra3":
                iconsController.AddPoints(0.08f, 0.04f, 0f);
                break;
            case "regra4":
                iconsController.AddPoints(0.06f, 0.05f, 0f);
                break;
            case "regra5":
                iconsController.AddPoints(0f, 0.05f, 0f);
                break;
            case "regra6":
                iconsController.AddPoints(0f, 0.05f, 0f);
                break;
            case "regra7":
                iconsController.AddPoints(0f, 0.05f, 0f);
                break;
            case "regra8":
                iconsController.AddPoints(0.08f, 0.04f, 0f);
                break;
            case "regra9":
                iconsController.AddPoints(0.08f, 0.03f, 0f);
                break;
            case "regra10":
                iconsController.AddPoints(0.08f, 0.03f, 0f);
                break;
            case "regra11":
                iconsController.AddPoints(0.08f, 0.03f, 0f);
                break;
            case "regra12":
                iconsController.AddPoints(0.0f, 0.05f, 0f);
                break;
            case "regra13":
                iconsController.AddPoints(0.08f, 0.03f, 0);
                break;
            case "regra14":
                iconsController.AddPoints(0f, 0.05f, 0f);
                break;
            case "regra15":
                iconsController.AddPoints(0f, 0.05f, 0f);
                break;
            case "regra16":
                iconsController.AddPoints(0f, 0.05f, 0f);
                break;
            case "regra17":
                iconsController.AddPoints(0f, 0.05f, 0f);
                break;
            case "regra18":
                iconsController.AddPoints(0.08f, 0.04f, 0f);
                break;
            case "regra19":
                iconsController.AddPoints(0.06f, 0f, 0f);
                break;
            case "regra20":
                iconsController.AddPoints(0f, 0.05f, 0f);
                break;
            case "regra21":
                iconsController.AddPoints(0f, 0.04f, 0f);
                break;
            case "regra22":
                iconsController.AddPoints(0.08f, 0f, 0f);
                break;
            case "regra23":
                iconsController.AddPoints(0.08f, 0f, 0f);
                break;
            case "regra24":
                iconsController.AddPoints(0f, 0.05f, 0f);
                break;
            case "regra25":
                iconsController.AddPoints(0.08f, 0f, 0f);
                break;
            case "regra26":
                iconsController.AddPoints(0f, 0.03f, 0f);
                break;
            case "regra27":
                iconsController.AddPoints(0.06f, 0f, 0f);
                break;
            case "regra28":
                iconsController.AddPoints(0f, 0.03f, 0f);
                break;
            case "regra29":
                iconsController.AddPoints(0f, 0.03f, 0f);
                break;
            case "regra30":
                iconsController.AddPoints(0.05f, 0.04f, 0f);
                break;
            default:
                break;
        }
    }

    public new void NextCard()
    {
        if (cardIndex == -1)
            StartCoroutine(EndGame(true));

        cardIndex++;

        // find next not selected card (if card was selected, its index is true)
        //while (selectedCards[cardIndex] == true)
        //{
        //    cardIndex++;

        //    // check if cardIndex have finished
        //    if (cardIndex > sprites.Length - 1)
        //        cardIndex = 0;
        //}

        ReadCard(cardIndex);

        if (minijogosDicas.hints.Length > 0)
            minijogosDicas.SetHintByIndex(cardIndex);

        if (cardIndex < sprites.Length)
        {
            currentImage.sprite = nextImage.sprite;
            currentImage.name = sprites[cardIndex].name;
            currentImage.GetComponentInChildren<TextMeshProUGUI>().text = RegrasText.GetRegra(cardIndex);
            cardName.text = currentImage.name;

            //Debug.Log(cardName.text);

            if (cardIndex < sprites.Length - 1)
            {
                //nextImage.sprite = sprites[cardIndex + 1];
                //nextImage.name = sprites[cardIndex + 1].name;
                nextImage.GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
            else
            {
                Debug.Log("fim das cartas... Começando de novo");
                cardIndex = -1;
                //nextImage.sprite = sprites[cardIndex+1];
                //nextImage.name = sprites[cardIndex+1].name;
                nextImage.GetComponentInChildren<TextMeshProUGUI>().text = "";

                //cardsNumber = GetCardsLeft();
                //Debug.Log(cardsNumber);
            }
        }

        ResetPosition();

        likeButton.Select();
    }

    public void ReadCard(int index)
    {
        Debug.Log("Nova Regra: " + RegrasText.GetRegra(index));
        ReadText("Nova Regra: " + RegrasText.GetRegra(index));
    }

    public IEnumerator EndGame(bool win)
    {
        if (win)
        {
            likeButton.interactable = false;
            dislikeButton.interactable = false;

            winImage.SetActive(true);

            PlayerPreferences.M002_Regras = true;

            audioSource.PlayOneShot(victoryClip);

            yield return new WaitWhile(() => audioSource.isPlaying);
            
            ReadText("Parabéns, você conseguiu mais alguns itens necessários para sua aventura na antártica!");

            lifeExpController.AddEXP(PlayerPreferences.XPwinPuzzle); // finalizou o minijogo
            lifeExpController.AddEXP(3*PlayerPreferences.XPwinItem); // ganhou o item

            // add the heart points in hp points
            lifeExpController.AddHP(5000f / heartImage.fillAmount);
            // add the antartica and star points to xp points
            lifeExpController.AddEXP(5000f / ((0.6f * starImage.fillAmount) + (0.4f * antarticaImage.fillAmount)));
        }
        else
        {
            loseImage.SetActive(true);

            //ReadText(ReadableTexts.instance.GetReadableText(ReadableTexts.key_m004_memoria_derrota, LocalizationManager.instance.GetLozalization()));

            audioSource.PlayOneShot(loseClip);

            yield return new WaitWhile(() => audioSource.isPlaying);

            ReadText("Infelizmente você não conseguiu finalizar o minijogo com êxito. Tente novamente.");

            resetButton.Select();

            lifeExpController.AddEXP(PlayerPreferences.XPlosePuzzle); // jogou um minijogo
        }

        StartCoroutine(ReturnToUshuaiaCoroutine());
    }

    public void ReturnToUshuaia()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(ScenesNames.M002Ushuaia);
    }

    public void ResetScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(ScenesNames.M002Regras);
    }

    public IEnumerator ReturnToUshuaiaCoroutine()
    {
        yield return new WaitForSeconds(4f);

        UnityEngine.SceneManagement.SceneManager.LoadScene(ScenesNames.M002Ushuaia);
    }

    public void TryReturnToUshuaia()
    {
        audioSource.PlayOneShot(avisoClip);

        confirmQuit.SetActive(true);

        ReadText(ReadableTexts.instance.GetReadableText(ReadableTexts.key_gameplay_aviso_botoes, LocalizationManager.instance.GetLozalization()));

        ReadText(confirmQuit.GetComponentInChildren<TMPro.TextMeshProUGUI>().text);

        confirmQuit.GetComponentInChildren<Button>().Select();
    }
}

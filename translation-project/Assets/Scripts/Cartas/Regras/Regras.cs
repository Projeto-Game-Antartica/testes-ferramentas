using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Regras : AbstractCardManager {

    private int likeCount = 0;
    private int cardCount = 0;

    private int rulesNumber = 15;
    private int cardsNumber;

    public TextMeshProUGUI CardLeft;
    public TextMeshProUGUI CardCount;

    public GameObject winImage;

    public GameObject instruction_interface;

    public Button resetButton;
    public Button backButton;

    public GameObject textPrefab;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            instruction_interface.SetActive(true);
        }

        if(Input.GetKey(KeyCode.Escape))
        {
            instruction_interface.SetActive(false);
        }
    }
    // after instructions, initialize the game
    public void InitializeGame()
    {
        cardIndex = 0;

        cardsNumber = sprites.Length;

        // current card initial settings
        currentImage.sprite = sprites[cardIndex];
        currentImage.color = new Color(1, 1, 1, 0);
        currentImage.name = sprites[cardIndex].name;
        //cardName.text = currentImage.name;
        Instantiate(textPrefab, currentImage.transform, false);
        currentImage.GetComponentInChildren<TextMeshProUGUI>().text = RegrasText.GetRegra(cardIndex);

        // next card initial settings
        nextImage.GetComponentInChildren<Image>().sprite = sprites[cardIndex + 1];
        nextImage.color = new Color(1, 1, 1, 0);
        //nextImage.name = sprites[cardIndex + 1].name;
        Instantiate(textPrefab, nextImage.transform, false);
        nextImage.GetComponentInChildren<TextMeshProUGUI>().text = RegrasText.GetRegra(cardIndex+1);

        initialPosition = currentImage.transform.parent.position;

        resetButton.interactable = true;
        backButton.interactable = true;
    }

    public override void CheckDislike()
    {
        cardCount++;
        if (cardCount == cardsNumber)
            cardCount = 0;

        CardLeft.text = "Regras restantes: " + (cardsNumber - cardCount);

        NextCard();
    }

    public override void CheckLike()
    {
        cardCount++;
        likeCount++;

        
        CardLeft.text = "Regras restantes: " + (cardsNumber - cardCount);
        CardCount.text = "Regras escolhidas: " + likeCount + "/" + rulesNumber;

        NextCard();

        if (likeCount >= rulesNumber)
        {
            likeButton.interactable = false;
            dislikeButton.interactable = false;

            winImage.SetActive(true);

            StartCoroutine(ReturnToUshuaiaCoroutine());
        }
    }

    // set the experience points
    public void cardsPoints(string cardName)
    {
        switch (cardName.ToLower())
        {
            case "regra1":
                break;
            case "regra2":
                break;
            case "regra3":
                break;
            case "regra4":
                break;
            case "regra5":
                break;
            case "regra6":
                break;
            case "regra7":
                break;
            case "regra8":
                break;
            case "regra9":
                break;
            case "regra10":
                break;
            case "regra11":
                break;
            case "regra12":
                break;
            case "regra13":
                break;
            case "regra14":
                break;
            case "regra15":
                break;
            case "regra16":
                break;
            case "regra17":
                break;
            case "regra18":
                break;
            case "regra19":
                break;
            case "regra20":
                break;
            case "regra21":
                break;
            case "regra22":
                break;
            case "regra23":
                break;
            case "regra24":
                break;
            case "regra25":
                break;
            case "regra26":
                break;
            case "regra27":
                break;
            case "regra28":
                break;
            case "regra29":
                break;
            case "regra30":
                break;
            default:
                break;
        }
    }

    public new void NextCard()
    {
        cardIndex++;

        if (minijogosDicas.hints.Length > 0)
            minijogosDicas.SetHintByIndex(cardIndex);

        if (cardIndex < sprites.Length)
        {
            currentImage.sprite = nextImage.sprite;
            currentImage.name = sprites[cardIndex].name;
            currentImage.GetComponentInChildren<TextMeshProUGUI>().text = RegrasText.GetRegra(cardIndex);
            cardName.text = currentImage.name;

            Debug.Log(cardName.text);

            if (cardIndex < sprites.Length - 1)
            {
                nextImage.sprite = sprites[cardIndex + 1];
                nextImage.name = sprites[cardIndex + 1].name;
                nextImage.GetComponentInChildren<TextMeshProUGUI>().text = RegrasText.GetRegra(cardIndex + 1);
            }
            else
            {
                Debug.Log("fim das cartas... Começando de novo");
                cardIndex = -1;
                nextImage.sprite = sprites[cardIndex+1];
                nextImage.name = sprites[cardIndex+1].name;
                nextImage.GetComponentInChildren<TextMeshProUGUI>().text = RegrasText.GetRegra(cardIndex+1);
            }
        }

        ResetPosition();
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
        yield return new WaitForSeconds(7f);

        UnityEngine.SceneManagement.SceneManager.LoadScene(ScenesNames.M002Ushuaia);
    }
}

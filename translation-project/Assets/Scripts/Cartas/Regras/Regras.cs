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

    // Use this for initialization
    void Start () {
        cardIndex = 0;
        
        cardsNumber = sprites.Length;

        currentCard.sprite = sprites[cardIndex];
        currentCard.name = sprites[cardIndex].name;
        //cardName.text = currentCard.name;

        nextCard.GetComponentInChildren<Image>().sprite = sprites[cardIndex + 1];
        //nextCard.name = sprites[cardIndex + 1].name;

        initialPosition = currentCard.transform.parent.position;
    }

    public override void CheckDislike()
    {
        cardCount++;

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

}

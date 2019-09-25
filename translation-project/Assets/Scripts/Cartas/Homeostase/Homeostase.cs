using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Homeostase : AbstractCardManager
{

    public Image kcalBar;

    public Transform alimentos;

    //private int cardIndex;

    private float kcal;

    private readonly int MAX_KCAL = 2000;

    public Image[] alimentosCesta;

    private int alimentosCestaIndex = 0;

    // Use this for initialization
    void Start()
    {
        cardIndex = 0;

        kcalBar.fillAmount = 0f;

        currentCard.sprite = sprites[cardIndex];
        currentCard.name = sprites[cardIndex].name;
        cardName.text = currentCard.name;

        nextCard.GetComponentInChildren<Image>().sprite = sprites[cardIndex + 1];
        nextCard.name = sprites[cardIndex + 1].name;

        initialPosition = currentCard.transform.parent.position;
    }

    override public void CheckLike()
    {
        // do something
        Transform cardImage = currentCard.GetComponentInChildren<Image>().transform;

        Instantiate(cardImage, currentCard.transform.position, Quaternion.identity, alimentos);

        if (alimentosCestaIndex < 20)
        {
            alimentosCesta[alimentosCestaIndex].color = new Color(1, 1, 1, 1);
            alimentosCesta[alimentosCestaIndex].sprite = currentCard.GetComponentInChildren<Image>().sprite;
            alimentosCesta[alimentosCestaIndex].preserveAspect = true;
            alimentosCestaIndex++;
        }


        switch (currentCard.name.ToLower())
        {
            case "abacate":
                kcal += 160;
                break;
            case "ameixa seca":
                kcal += 240;
                break;
            case "amendoas":
                kcal += 579;
                break;
            case "banana":
                kcal += 98;
                break;
            case "barrinha de cereal":
                kcal += 86;
                break;
            case "batata doce":
                kcal += 86;
                break;
            case "cenoura":
                kcal += 36;
                break;
            case "chocolate":
                kcal += 139;
                break;
            case "figo":
                kcal += 249;
                break;
            case "agua":
                kcal += 0;
                break;
            case "laranja":
                kcal += 47;
                break;
            case "leite de soja":
                kcal += 82;
                break;
            case "leite desnatado":
                kcal += 63;
                break;
            case "maca":
                kcal += 52;
                break;
            case "melancia":
                kcal += 30;
                break;
            case "pao":
                kcal += 126.5f;
                break;
            case "queijo cheddar":
                kcal += 402.66f;
                break;
            case "queijo mussarela":
                kcal += 318;
                break;
            case "semente de abobora":
                kcal += 559;
                break;
            case "suco laranja":
                kcal += 54.45f;
                break;
            default:
                kcal += 0;
                break;
        }

        CheckCalories(kcal);
        NextCard();
    }

    override public void CheckDislike()
    {
        // do something else
        NextCard();
    }

    public void CheckCalories(float kcal)
    {
        // normalize
        kcalBar.fillAmount = kcal / MAX_KCAL;

        if (kcalBar.fillAmount == 1)
            Debug.Log("Atingido o máximo de calorias");
    }

    public void RemoverAlimentoCesta(int index)
    {

    }
}
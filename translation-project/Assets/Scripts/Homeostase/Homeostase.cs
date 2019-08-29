using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Homeostase : MonoBehaviour {

    public Vector3 initialPosition;

    public Sprite[] sprites;

    public Image currentCard;
    public Image nextCard;

    public TMPro.TextMeshProUGUI cardName;

    public Image kcalBar;

    private int cardIndex;

    private float kcal;

    private readonly int MAX_KCAL = 2000;

	// Use this for initialization
	void Start () {
        cardIndex = 0;

        kcalBar.fillAmount = 0f;

        currentCard.sprite = sprites[cardIndex];
        currentCard.name = sprites[cardIndex].name;
        cardName.text = currentCard.name;

        nextCard.GetComponentInChildren<Image>().sprite = sprites[cardIndex + 1];
        nextCard.name = sprites[cardIndex + 1].name;

        initialPosition = currentCard.transform.parent.position;
	}

    public void SwipePositive()
    {
        currentCard.transform.parent.DOMoveX(400, 1);
        currentCard.transform.parent.DOMoveY(-300, 2);
        currentCard.transform.parent.DORotate(new Vector3(0, 0, -45), 2);
    }

    public void SwipeNegative()
    {
        StartCoroutine(SwipeNegativeCoroutine());
    }

    public void SwipePositiveScaled()
    {
        StartCoroutine(SwipePositiveCoroutine());
    }

    public IEnumerator SwipeNegativeCoroutine()
    {
        currentCard.transform.parent.DOMoveX(-400, 1);
        currentCard.transform.parent.DOMoveY(-300, 2);
        currentCard.transform.parent.DORotate(new Vector3(0, 0, 45), 2);

        yield return new WaitForSeconds(1f);

        CheckDislike();
    }
    
    public IEnumerator SwipePositiveCoroutine()
    {
        int delta = Random.Range(0, 80);
        currentCard.transform.parent.DOMoveX(120 + delta, 1);
        currentCard.transform.parent.DORotate(new Vector3(0, 0, -45), 2);
        currentCard.transform.parent.DOScale(new Vector3(0.4f, 0.4f), 2);

        yield return new WaitForSeconds(2f);

        CheckLike();
    }

    public void ResetPosition()
    {
        currentCard.transform.parent.position = initialPosition;
        currentCard.transform.parent.rotation = Quaternion.identity;
        currentCard.transform.parent.DOScale(Vector3.one, 0);
    }

    public void CheckLike()
    {
        // do something
        switch(currentCard.name.ToLower())
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

    public void CheckDislike()
    {
        // do something else
        NextCard();
    }

    private void NextCard()
    {
        cardIndex++;

        if (cardIndex < sprites.Length)
        {
            currentCard.sprite = nextCard.sprite;
            currentCard.name = sprites[cardIndex].name;
            cardName.text = currentCard.name;

            if (cardIndex < sprites.Length - 1)
            {
                nextCard.sprite = sprites[cardIndex + 1];
                nextCard.name = sprites[cardIndex + 1].name;
            }
        }
        else
        {
            Debug.Log("fim das cartas");
        }

        ResetPosition();
    }

    public void CheckCalories(float kcal)
    {
        // normalize
        kcalBar.fillAmount = kcal / MAX_KCAL;

        if (kcalBar.fillAmount == 1)
            Debug.Log("Atingido o máximo de calorias");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public abstract class AbstractCardManager : MonoBehaviour {

    public Vector3 initialPosition;

    public Sprite[] sprites;

    public Image currentCard;
    public Image nextCard;

    public Button likeButton;
    public Button dislikeButton;

    public TMPro.TextMeshProUGUI cardName;
    
    public int cardIndex;

    public abstract void CheckLike();

    public abstract void CheckDislike();

    // Use this for initialization
    void Start()
    {
        currentCard.sprite = sprites[cardIndex];
        currentCard.name = sprites[cardIndex].name;
        cardName.text = currentCard.name;

        nextCard.GetComponentInChildren<Image>().sprite = sprites[cardIndex + 1];
        nextCard.name = sprites[cardIndex + 1].name;

        initialPosition = currentCard.transform.parent.position;
    }

    public void SwipeNegative()
    {
        SetButtonsInteractable(false);
        StartCoroutine(SwipeNegativeCoroutine());
    }

    public void SwipePositive()
    {
        SetButtonsInteractable(false);
        StartCoroutine(SwipePositiveCoroutine());
    }

    public void SwipePositiveScaled()
    {
        SetButtonsInteractable(false);
        StartCoroutine(SwipePositiveScaledCoroutine());
    }

    public IEnumerator SwipePositiveCoroutine()
    {
        currentCard.transform.parent.DOMoveX(400, 1);
        currentCard.transform.parent.DOMoveY(-300, 2);
        currentCard.transform.parent.DORotate(new Vector3(0, 0, -45), 2);

        yield return new WaitForSeconds(2f);

        CheckLike();
    }

    public IEnumerator SwipeNegativeCoroutine()
    {
        currentCard.transform.parent.DOMoveX(-400, 1);
        currentCard.transform.parent.DOMoveY(-300, 2);
        currentCard.transform.parent.DORotate(new Vector3(0, 0, 45), 2);

        yield return new WaitForSeconds(2f);

        CheckDislike();
    }

    public IEnumerator SwipePositiveScaledCoroutine()
    {
        int delta = Random.Range(0, 90);
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

    public void NextCard()
    {
        Debug.Log("cardIndex: "+cardIndex);
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
            else
            {
                nextCard.sprite = sprites[0];
                nextCard.name = sprites[0].name;
            }
        }
        else
        {
            Debug.Log("fim das cartas");
            cardIndex = 0;
        }

        ResetPosition();
        SetButtonsInteractable(true);
    }

    public void SetButtonsInteractable(bool value)
    {
        likeButton.interactable = value;
        dislikeButton.interactable = value;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public abstract class AbstractCardManager : AbstractScreenReader {

    public Vector3 initialPosition;

    public Sprite[] sprites;

    public Image currentCard;
    public Image nextCard;

    public Button likeButton;
    public Button dislikeButton;

    public bool isDone;

    public TMPro.TextMeshProUGUI cardName;
    
    public int cardIndex;

    public MinijogosDicas minijogosDicas;

    public abstract void CheckLike();

    public abstract void CheckDislike();

    public void SwipeNegative()
    {
        likeButton.interactable = false;
        dislikeButton.interactable = false;

        StartCoroutine(SwipeNegativeCoroutine());
    }

    public void SwipePositive()
    {
        likeButton.interactable = false;
        dislikeButton.interactable = false;

        StartCoroutine(SwipePositiveCoroutine());
    }

    public void SwipePositiveScaled()
    {
        likeButton.interactable = false;
        dislikeButton.interactable = false;

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

        if (!isDone)
            likeButton.interactable = true;

        dislikeButton.interactable = true;
    }

    public void NextCard()
    {
        cardIndex++;

        minijogosDicas.SetHintByIndex(cardIndex);

        if (cardIndex < sprites.Length)
        {
            currentCard.sprite = nextCard.sprite;
            currentCard.name = sprites[cardIndex].name;
            cardName.text = currentCard.name;

            Debug.Log(cardName.text);

            if (cardIndex < sprites.Length - 1)
            {
                nextCard.sprite = sprites[cardIndex + 1];
                nextCard.name = sprites[cardIndex + 1].name;
            }
            else
            {
                Debug.Log("fim das cartas... Começando de novo");
                cardIndex = 0;
                nextCard.sprite = sprites[cardIndex];
                nextCard.name = sprites[cardIndex].name;
            }
        }

        ResetPosition();
    }
}

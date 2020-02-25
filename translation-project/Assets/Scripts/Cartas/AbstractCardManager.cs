using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public abstract class AbstractCardManager : AbstractScreenReader {

    public Vector3 initialPosition;

    public Sprite[] sprites;

    public Image currentImage;
    public Image nextImage;

    public Button likeButton;
    public Button dislikeButton;

    public bool isDone;

    public TMPro.TextMeshProUGUI cardName;
    
    public int cardIndex;

    public MinijogosDicas minijogosDicas;

    public abstract void CheckLike();

    public abstract void CheckDislike();

    protected virtual void beforePositiveSwipe() {
        return;
    }
    protected virtual void beforeNegativeSwipe() {
        return;
    }

    public void SwipeNegative()
    {
        beforeNegativeSwipe();
        likeButton.interactable = false;
        dislikeButton.interactable = false;

        StartCoroutine(SwipeNegativeCoroutine());
    }

    public void SwipePositive()
    {
        beforePositiveSwipe();
        likeButton.interactable = false;
        dislikeButton.interactable = false;

        StartCoroutine(SwipePositiveCoroutine());
    }

    public void SwipePositiveScaled()
    {
        beforePositiveSwipe();
        likeButton.interactable = false;
        dislikeButton.interactable = false;

        StartCoroutine(SwipePositiveScaledCoroutine());
    }

    public IEnumerator SwipePositiveCoroutine()
    {
        currentImage.transform.parent.DOMoveX(400, 1);
        currentImage.transform.parent.DOMoveY(-300, 2);
        currentImage.transform.parent.DORotate(new Vector3(0, 0, -45), 2);

        yield return new WaitForSeconds(2f);

        CheckLike();
    }

    public IEnumerator SwipeNegativeCoroutine()
    {
        currentImage.transform.parent.DOMoveX(-400, 1);
        currentImage.transform.parent.DOMoveY(-300, 2);
        currentImage.transform.parent.DORotate(new Vector3(0, 0, 45), 2);

        yield return new WaitForSeconds(2f);

        CheckDislike();
    }

    public IEnumerator SwipePositiveScaledCoroutine()
    {
        int delta = Random.Range(0, 90);
        currentImage.transform.parent.DOMoveX(120 + delta, 1);
        currentImage.transform.parent.DOMoveY(3, 1);
        currentImage.transform.parent.DORotate(new Vector3(0, 0, -45), 2);
        currentImage.transform.parent.DOScale(new Vector3(0.4f, 0.4f), 2);

        yield return new WaitForSeconds(2f);

        CheckLike();
    }

    public void ResetPosition()
    {
        currentImage.transform.parent.position = initialPosition;
        currentImage.transform.parent.rotation = Quaternion.identity;
        currentImage.transform.parent.DOScale(Vector3.one, 0);

        if (!isDone)
        {
            likeButton.interactable = true;
            dislikeButton.interactable = true;
        }
    }

    public void NextCard()
    {
        cardIndex++;
        
        if (cardIndex < sprites.Length)
        {
            currentImage.sprite = nextImage.sprite;
            currentImage.name = sprites[cardIndex].name;
            cardName.text = currentImage.name;

            Debug.Log(cardName.text);
            ReadText(cardName.text);

            if (cardIndex < sprites.Length - 1)
            {
                nextImage.sprite = sprites[cardIndex + 1];
                nextImage.name = sprites[cardIndex + 1].name;
            }
            else
            {
                Debug.Log("fim das cartas... Começando de novo");
                cardIndex = -1;
                nextImage.sprite = sprites[cardIndex+1];
                nextImage.name = sprites[cardIndex+1].name;
            }
        }

        // read the hint
        if (minijogosDicas.hints.Length > 0)
            minijogosDicas.SetHintByIndex(cardIndex);

        ResetPosition();
    }
}

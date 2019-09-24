using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;

public class Homeostase : MonoBehaviour {

    public Vector3 initialPosition;

    public Sprite[] sprites;

    public Image currentCard;
    public Image nextCard;

    public TMPro.TextMeshProUGUI cardName;

    public Image kcalBar;
<<<<<<< Updated upstream:translation-project/Assets/Scripts/Homeostase/Homeostase.cs

    private int cardIndex;

=======
    public Transform alimentos;
    //private int cardIndex;
>>>>>>> Stashed changes:translation-project/Assets/Scripts/Cartas/Homeostase/Homeostase.cs
    private float kcal;
    private readonly int MAX_KCAL = 2000;
<<<<<<< Updated upstream:translation-project/Assets/Scripts/Homeostase/Homeostase.cs

=======
    public Image[] alimentosCesta;
    private int alimentosCestaIndex = 20;

    public GameObject alimentoCestaPrefab;

    private List<GameObject> alimentosCestaList;
    
>>>>>>> Stashed changes:translation-project/Assets/Scripts/Cartas/Homeostase/Homeostase.cs
	// Use this for initialization
	void Start ()
    {
        alimentosCestaList = new List<GameObject>();

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

<<<<<<< Updated upstream:translation-project/Assets/Scripts/Homeostase/Homeostase.cs
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
=======
        //Instantiate(cardImage, currentCard.transform.position, Quaternion.identity, alimentos);
        var alimentoCesta = Instantiate(alimentoCestaPrefab, currentCard.transform.position, Quaternion.identity, alimentos);
        alimentoCesta.name = currentCard.name;
        alimentoCesta.GetComponent<Image>().sprite = currentCard.GetComponentInChildren<Image>().sprite;
        alimentoCesta.GetComponent<Image>().preserveAspect = true;

        alimentosCestaList.Add(alimentoCesta);
        
        for (int i = 0; i < alimentosCestaIndex; i++)
        {
                Debug.Log(i + " " + alimentosCesta[i].GetComponentInChildren<AlimentosInventarioController>().initialized);
            if (!alimentosCesta[i].GetComponentInChildren<AlimentosInventarioController>().initialized)
            {
                alimentosCesta[i].GetComponentsInChildren<Image>()[1].color = new Color(1, 1, 1, 1);
                alimentosCesta[i].GetComponentsInChildren<Image>()[1].sprite = currentCard.GetComponentInChildren<Image>().sprite;
                alimentosCesta[i].GetComponentsInChildren<Image>()[1].preserveAspect = true;
                alimentosCesta[i].GetComponentInChildren<Button>().interactable = true;
                alimentosCesta[i].GetComponentInChildren<AlimentosInventarioController>().initialized = true;
                alimentosCesta[i].gameObject.name = currentCard.name;

                Debug.Log("adicionado na pos: " + i);
                // exit for loop
                i = alimentosCestaIndex + 1;
            }
        }

        CheckCalories(currentCard.name, true);
        NextCard();
    }

    override public void CheckDislike()
    {
        // do something else
        NextCard();
    }
    
    // true to add calories, false to remove calories
    public void CheckCalories(string cardName, bool add)
    {
        switch (cardName.ToLower())
>>>>>>> Stashed changes:translation-project/Assets/Scripts/Cartas/Homeostase/Homeostase.cs
        {
            case "abacate":
                if (add) kcal += 160;
                else kcal -= 160;
                break;
            case "ameixa seca":
                if (add) kcal += 240;
                else kcal -= 240;
                break;
            case "amendoas":
                if (add) kcal += 579;
                else kcal -= 579;
                break;
            case "banana":
                if (add) kcal += 98;
                else kcal -= 98;
                break;
            case "barrinha de cereal":
                if (add) kcal += 86;
                else kcal -= 86;
                break;
            case "batata doce":
                if (add) kcal += 86;
                else kcal -= 86;
                break;
            case "cenoura":
                if (add) kcal += 36;
                else kcal -= 36;
                break;
            case "chocolate":
                if (add) kcal += 139;
                else kcal -= 139;
                break;
            case "figo":
                if (add) kcal += 249;
                else kcal -= 249;
                break;
            case "agua":
                if (add) kcal += 0;
                else kcal -= 0;
                break;
            case "laranja":
                if (add) kcal += 47;
                else kcal -= 47;
                break;
            case "leite de soja":
                if (add) kcal += 82;
                else kcal -= 82;
                break;
            case "leite desnatado":
                if (add) kcal += 63;
                else kcal -= 63;
                break;
            case "maca":
                if (add) kcal += 52;
                else kcal -= 52;
                break;
            case "melancia":
                if (add) kcal += 30;
                else kcal -= 30;
                break;
            case "pao":
                if (add) kcal += 126.5f;
                else kcal -= 126.5f;
                break;
            case "queijo cheddar":
                if (add) kcal += 402.66f;
                else kcal -= 402.66f;
                break;
            case "queijo mussarela":
                if (add) kcal += 318;
                else kcal -= 318;
                break;
            case "semente de abobora":
                if (add) kcal += 559;
                else kcal -= 559;
                break;
            case "suco laranja":
                if (add) kcal += 54.45f;
                else kcal -= 54.45f;
                break;
            default:
                kcal += 0;
                break;
        }

<<<<<<< Updated upstream:translation-project/Assets/Scripts/Homeostase/Homeostase.cs
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
=======
>>>>>>> Stashed changes:translation-project/Assets/Scripts/Cartas/Homeostase/Homeostase.cs
        // normalize
        kcalBar.fillAmount = kcal / MAX_KCAL;

        if (kcalBar.fillAmount == 1)
            Debug.Log("Atingido o máximo de calorias");
    }
    
    public void RemoverAlimentoCesta(int index)
    {
        alimentosCesta[index].GetComponentsInChildren<Image>()[1].sprite = null;
        alimentosCesta[index].GetComponentsInChildren<Image>()[1].color = new Color(1, 1, 1, 0);
        alimentosCesta[index].GetComponentInChildren<Button>().interactable = false;
        alimentosCesta[index].GetComponentInChildren<AlimentosInventarioController>().initialized = false;
        
        try
        {
            var result = alimentosCestaList.Find(x => x.name.Contains(alimentosCesta[index].gameObject.name));
            Debug.Log(alimentosCesta[index].gameObject.name);
            result.GetComponent<Image>().enabled = false;

            CheckCalories(result.name, false);
            alimentosCestaList.Remove(result);
        }
        catch (Exception ex)
        {
            Debug.Log("Não encontrado na cesta. Stacktrace => " + ex.StackTrace);
        }

        //ShiftArray(index);
    }

    public void ShiftArray(int index)
    {
        for (int i = index - 1; i < alimentosCestaIndex-1; i--)
        {
            alimentosCesta[i] = alimentosCesta[i + 1];
        }
    }
}

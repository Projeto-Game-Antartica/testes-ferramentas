using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Vegetacao : AbstractCardManager
{
    //public Image kcalBar;

    public Transform alimentos;

    private int cardIndex;

    private float kcal;

    private readonly int MAX_KCAL = 2000;

    public Image[] alimentosCesta;

    private int alimentosCestaIndex = 20;

    public GameObject alimentoCestaPrefab;

    private List<GameObject> alimentosCestaList;

    public Button satisfeitoButton;

    public Button resetButton;
    public Button backButton;
    public GameObject confirmQuit;

    public GameObject instruction_interface;

    //Lucas code

    String[] tiposVegetais = new String[]{
        "planta", 
        "fungo", 
        "alga"
    };

    public Image algaImg, plantaImg, fungoImg;

    private int cardsCount; //Contador do número de cards que ja foram passados

    private void updateCurrentCard(int cardIndex) {
        currentImage.sprite = sprites[cardIndex];
        currentImage.name = sprites[cardIndex].name;
        cardName.text = currentImage.name;//.Substring(0,5);
    }

    private void updateNextCard(int cardIndex){
        nextImage.GetComponentInChildren<Image>().sprite = sprites[cardIndex];
        nextImage.name = sprites[cardIndex].name;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            instruction_interface.SetActive(true);
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            instruction_interface.SetActive(false);
        }
    }

    // initialize after button click on instruction
    public void Initialize()
    {
        sprites = Shuffle<Sprite>(sprites);

        alimentosCestaList = new List<GameObject>();

        algaImg.fillAmount = 1;
        plantaImg.fillAmount = 1;
        fungoImg.fillAmount = 1;

        cardIndex = 0;
        isDone = false;
        cardsCount = 0;

        //kcalBar.fillAmount = 0f;

        // currentImage.sprite = sprites[cardIndex];
        // currentImage.name = sprites[cardIndex].name;
        // cardName.text = currentImage.name.Substring(0,5);
        updateCurrentCard(cardIndex);


        Debug.Log(cardName.text);

        //nextImage.GetComponentInChildren<Image>().sprite = sprites[cardIndex + 1];
        //nextImage.name = sprites[cardIndex + 1].name;
        updateNextCard(cardIndex + 1);

        initialPosition = currentImage.transform.parent.position;

        // set initialized from alimentos on inventory to false
        for (int i = 0; i < alimentosCesta.Length; i++)
        {
            alimentosCesta[i].GetComponentInChildren<AlimentosInventarioController>().initialized = false;
        }

        // show first hint
        minijogosDicas.SetHintByIndex(cardIndex);

        resetButton.interactable = true;
        backButton.interactable = true;
    }

    override public void CheckLike()
    {
        // do something
        Transform cardImage = currentImage.GetComponentInChildren<Image>().transform;

        //Instantiate(cardImage, currentImage.transform.position, Quaternion.identity, alimentos);

        // var alimentoCesta = Instantiate(alimentoCestaPrefab, currentImage.transform.position, Quaternion.identity, alimentos);
        // alimentoCesta.name = currentImage.name;
        // alimentoCesta.GetComponent<Image>().sprite = currentImage.GetComponentInChildren<Image>().sprite;
        // alimentoCesta.GetComponent<Image>().preserveAspect = true;

        // alimentosCestaList.Add(alimentoCesta);

        // for (int i = 0; i < alimentosCestaIndex; i++)
        // {
        //     //Debug.Log(i + " " + alimentosCesta[i].GetComponentInChildren<AlimentosInventarioController>().initialized);
        //     if (!alimentosCesta[i].GetComponentInChildren<AlimentosInventarioController>().initialized)
        //     {
        //         alimentosCesta[i].GetComponentsInChildren<Image>()[1].color = new Color(1, 1, 1, 1);
        //         alimentosCesta[i].GetComponentsInChildren<Image>()[1].sprite = currentImage.GetComponentInChildren<Image>().sprite;
        //         alimentosCesta[i].GetComponentsInChildren<Image>()[1].preserveAspect = true;
        //         alimentosCesta[i].GetComponentInChildren<Button>().interactable = true;
        //         alimentosCesta[i].GetComponentInChildren<AlimentosInventarioController>().initialized = true;
        //         alimentosCesta[i].gameObject.name = currentImage.name;

        //         //Debug.Log("adicionado na pos: " + i);
        //         // exit for loop
        //         i = alimentosCestaIndex + 1;
        //     }
        // }

        CheckCalories(currentImage.name, true);
        NextCard();
    }

    override public void CheckDislike()
    {
        // do something else
        NextCard();
    }

    public void CheckCalories(string cardName, bool add)
    {
        switch (cardName.ToLower())
        {
            case "abacate":
                if (add)
                    kcal += 160;
                else
                    kcal -= 160;
                break;
            case "ameixa seca":
                if (add)
                    kcal += 240;
                else
                    kcal -= 240;
                break;
            case "amendoas":
                if (add)
                    kcal += 579;
                else
                    kcal -= 579;
                break;
            case "banana":
                if (add)
                    kcal += 98;
                else
                    kcal -= 98;
                break;
            case "barrinha de cereal":
                if (add)
                    kcal += 86;
                else
                    kcal -= 86;
                break;
            case "batata doce":
                if (add)
                    kcal += 86;
                else
                    kcal -= 86;
                break;
            case "cenoura":
                if (add)
                    kcal += 36;
                else
                    kcal -= 36;
                break;
            case "chocolate":
                if (add)
                    kcal += 139;
                else
                    kcal -= 139;
                break;
            case "figo":
                if (add)
                    kcal += 249;
                else
                    kcal -= 249;
                break;
            case "agua":
                if (add)
                    kcal += 0;
                else
                    kcal -= 0;
                break;
            case "laranja":
                if (add)
                    kcal += 47;
                else
                    kcal -= 47;
                break;
            case "leite de soja":
                if (add)
                    kcal += 82;
                else
                    kcal -= 82;
                break;
            case "leite desnatado":
                if (add)
                    kcal += 63;
                else
                    kcal -= 63;
                break;
            case "maca":
                if (add)
                    kcal += 52;
                else
                    kcal -= 52;
                break;
            case "melancia":
                if (add)
                    kcal += 30;
                else
                    kcal -= 30;
                break;
            case "pao":
                if (add)
                    kcal += 126.5f;
                else
                    kcal -= 126.5f;
                break;
            case "queijo cheddar":
                if (add)
                    kcal += 402.66f;
                else
                    kcal -= 402.66f;
                break;
            case "queijo mussarela":
                if (add)
                    kcal += 318;
                else
                    kcal -= 318;
                break;
            case "semente de abobora":
                if (add)
                    kcal += 559;
                else
                    kcal -= 559;
                break;
            case "suco laranja":
                if (add)
                    kcal += 54.45f;
                else
                    kcal -= 54.45f;
                break;
            default:
                    kcal += 0;
                break;
        }

        // normalize
        //kcalBar.fillAmount = kcal / MAX_KCAL;

        Debug.Log("Você está levando " + kcal + "/" + MAX_KCAL + " kcal");

        // cesta cheia, não pode colocar mais comida.
        // if (kcalBar.fillAmount == 1)
        // {
        //     isDone = true;
        //     //likeButton.interactable = false;
        //     Debug.Log("Atingido o máximo de calorias");

        //     satisfeitoButton.interactable = true;
        // }
        // else
        //     satisfeitoButton.interactable = false;
    }

    public void NextCard() {
        cardIndex++;

        if(minijogosDicas.hints.Length > 0)
            minijogosDicas.SetHintByIndex(cardIndex);

        if (cardIndex < sprites.Length) {
            // currentImage.sprite = nextImage.sprite;
            // currentImage.name = sprites[cardIndex].name;
            // cardName.text = currentImage.name;
            updateCurrentCard(cardIndex);

            Debug.Log(cardName.text);

            if (cardIndex < sprites.Length - 1) {
                updateNextCard(cardIndex + 1);
                //nextImage.sprite = sprites[cardIndex + 1];
                //nextImage.name = sprites[cardIndex + 1].name;
            } else {
                Debug.Log("fim das cartas... Começando de novo");
                cardIndex = -1;
                updateNextCard(cardIndex + 1);
                //nextImage.sprite = sprites[cardIndex + 1];
                //nextImage.name = sprites[cardIndex + 1].name;
            }
        }

        ResetPosition();
    }

    public void ConfirmRemover() {
        Debug.Log(EventSystem.current.currentSelectedGameObject.transform.parent.name);
    }

    public void RemoverAlimentoCesta(int index) {
        alimentosCesta[index].GetComponentsInChildren<Image>()[1].sprite = null;
        alimentosCesta[index].GetComponentsInChildren<Image>()[1].color = new Color(1, 1, 1, 0);
        alimentosCesta[index].GetComponentInChildren<Button>().interactable = false;
        alimentosCesta[index].GetComponentInChildren<AlimentosInventarioController>().initialized = false;

        try
        {
            Debug.Log("alimento: " + alimentosCesta[index].gameObject.name);
            var result = alimentosCestaList.Find(x => x.name.Contains(alimentosCesta[index].gameObject.name));
            Debug.Log("resultado da lista: " + result.name);
            result.GetComponent<Image>().enabled = false;

            CheckCalories(result.name, false);

            alimentosCestaList.Remove(result);
        }
        catch (Exception ex)
        {
            Debug.Log("Não encontrado na cesta. Stacktrace => " + ex.StackTrace);
        }

        // pode escolher outro alimento para colocar na cesta
        // if(kcalBar.fillAmount < 1)
        // {
        //     isDone = false;
        //     likeButton.interactable = true;
        //     dislikeButton.interactable = true;
        // }

        //ShiftArray(index);
    }

    public void ShiftArray(int index)
    {
        for (int i = index - 1; i < alimentosCestaIndex - 1; i--)
        {
            alimentosCesta[i] = alimentosCesta[i + 1];
        }
    }

    
	/// <summary>
	/// Shuffle the array.
	/// </summary>
	/// <typeparam name="T">Array element type.</typeparam>
	/// <param name="array">Array to shuffle.</param>
	public T[] Shuffle<T>(T[] array)
	{
		var random = new System.Random();
		for (int i = array.Length; i > 1; i--)
		{
			// Pick random element to swap.
			int j = random.Next(i); // 0 <= j <= i-1
			// Swap.
			T tmp = array[j];
			array[j] = array[i - 1];
			array[i - 1] = tmp;
		}
		return array;
	}

    public void ResetScene()
    {
        SceneManager.LoadScene(ScenesNames.M002Homeostase);
    }

    public void ReturnToUshuaia()
    {
        SceneManager.LoadScene(ScenesNames.M002CasaUshuaia);
    }
}
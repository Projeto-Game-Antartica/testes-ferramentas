using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ClassificaCard : AbstractScreenReader//, ISelectHandler
{

    public const int VIRADA_BAIXO = 0;
    public const int VIRADA_CIMA = 1;

    public int clique = 0;

    public static bool DO_NOT = false;

    public bool added { get; set; }

    public int state { get; set; }
    public int cardValue { get; set; }
    public bool initialized { get; set; }

    //private Sprite cardBack;
    //public Sprite cardFace;

    public ClassificaManager classificaManager;

    public Image BGImage;

    private bool _init = false;

    public GameObject proccessTextPrefab;

    private void Start()
    {
        state = VIRADA_BAIXO;
        initialized = false;
        added = false;
        //StartCoroutine(showCards());
    }

    public IEnumerator showCards()
    {
        yield return new WaitForSeconds(0.5f);

        flipCard();
    }

    public void setupGraphics()
    {

        //GetComponent<Image>().color = new Color(1, 1, 1, 0);

        //gameObject.name += ": " + cardFace.name;

        //flipCard();

        _init = true;
    }

    public void cliques()
    {
     clique = clique + 1;
     Debug.Log("cliques:" +clique);
     GetComponent<Image>().color = new Color(1, 1, 1, 0);
	}

    public void flipCard()
    {
        Debug.Log("Clique");
        if (state == VIRADA_BAIXO && !DO_NOT)
        {
            state = VIRADA_CIMA;
            Debug.Log("virou");
            GetComponent<Image>().color = new Color(1, 1, 1, 1);
            //GetComponent<Image>().color = new Color(1, 1, 1, 0);
        }
        else if (state == VIRADA_CIMA && !DO_NOT)
        {
            state = VIRADA_BAIXO;
            GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }
        /*if (state == VIRADA_BAIXO && !DO_NOT)
        {

            //GetComponent<Image>().sprite = cardBack;
            //ADD COLOR
            //GetComponent<Image>().color = new Color(1, 1, 1, 1);

        }
        else if (state == VIRADA_CIMA && !DO_NOT)
        {
        state = VIRADA_CIMA;

            //GetComponent<Image>().sprite = cardFace;
            GetComponent<Image>().color = new Color(1, 1, 1, 1);


            //if (_init)
            //{
            //    string objectName = CardsDescription.GetCardText(gameObject.name);

            //    ReadAndDebugCardText(objectName);
            //}
        }*/
    }

    public void falseCheck()
    {
        StartCoroutine(pause());
    }

    public void turnCardDown()
    {
        //GetComponent<Image>().sprite = cardBack;

        //ADD COLOR

        DO_NOT = false;
    }

    IEnumerator pause()
    {
        yield return new WaitForSeconds(2f);

        if (state == VIRADA_BAIXO)
        {
            //GetComponent<Image>().sprite = cardBack;

            //ADD COLOR


        }

        DO_NOT = false;
    }

    /*public void OnSelect(BaseEventData eventData)
    {
        //Debug.Log(state);

        if (state == VIRADA_BAIXO || state == VIRADA_CIMA)
        {
            //Debug.Log(gameObject.name.Substring(0, gameObject.name.IndexOf(":")));
            ReadText(gameObject.name.Substring(0, gameObject.name.IndexOf(":")));
        }
        //else
        //{
        //    string objectName = CardsDescription.GetCardText(gameObject.name);
        //    ReadAndDebugCardText(objectName);
        //}
    }*/

    public void ReadAndDebugCardText(string objectName)
    {
        // numero da carta + descrição ou numero da carta + nome do animal
        //Debug.Log(objectName != null ? (gameObject.name.Substring(0, gameObject.name.IndexOf(":")) + ": " + objectName) : gameObject.name);
        ReadText(objectName != null ? (gameObject.name.Substring(0, gameObject.name.IndexOf(":")) + ": " + objectName) : gameObject.name);
    }

    public void CreateText(string cardName)
    {
        //GameObject text = new GameObject();
        GameObject text = Instantiate(proccessTextPrefab);
        text.transform.SetParent(transform, false);

        text.GetComponent<TMPro.TextMeshProUGUI>().text = EinsteinCardContent.GetText(cardName);
    }
}

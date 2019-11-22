using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;


public class Acampamentoo : AbstractCardManager
{

    public Vector3 initialPosition;

    public Sprite[] sprites;

    public Image currentCard;
    public Image nextCard;

    public TMPro.TextMeshProUGUI cardName;

    public Image kcalBar;

    public Image fill;

    public Image fills;

    public Image fillm;

    private int cardIndex;

    private float coracao;

    private float estrela;

    private float mapa;

    private readonly int MAX_COR = 100;
    private readonly int MAX_EST = 100;
    private readonly int MAX_MAP = 100;
    // Use this for initialization
    void Start()
    {
        cardIndex = 0;

        fill.fillAmount = 0.5f;
        fills.fillAmount = 0.5f;
        fillm.fillAmount = 0.5f;

        currentCard.sprite = sprites[cardIndex];
        currentCard.name = sprites[cardIndex].name;
        cardName.text = currentCard.name;

        Debug.Log(cardName.text);

        nextCard.GetComponentInChildren<Image>().sprite = sprites[cardIndex + 1];
        nextCard.name = sprites[cardIndex + 1].name;

        initialPosition = currentCard.transform.parent.position;
    }

    // public void SwipePositive()
    //{
    //    currentCard.transform.parent.DOMoveX(400, 1);
    //    currentCard.transform.parent.DOMoveY(-300, 2);
    //    currentCard.transform.parent.DORotate(new Vector3(0, 0, -45), 2);
    // }

    // public void SwipeNegative()
    // {
    //     StartCoroutine(SwipeNegativeCoroutine());
    // }

    //public void SwipePositiveScaled()
    //{
    //    StartCoroutine(SwipePositiveCoroutine());
    //}

    //public IEnumerator SwipeNegativeCoroutine()
    //{
    //    currentCard.transform.parent.DOMoveX(-400, 1);
    //    currentCard.transform.parent.DOMoveY(-300, 2);
    //    currentCard.transform.parent.DORotate(new Vector3(0, 0, 45), 2);

    //    yield return new WaitForSeconds(1f);

    //    CheckDislike();
    //}

    // public IEnumerator SwipePositiveCoroutine()
    // {
    //     int delta = Random.Range(0, 80);
    //     currentCard.transform.parent.DOMoveX(120 + delta, 1);
    //     currentCard.transform.parent.DORotate(new Vector3(0, 0, -45), 2);
    //     currentCard.transform.parent.DOScale(new Vector3(0.4f, 0.4f), 2);

    //   yield return new WaitForSeconds(2f);

    // CheckLike();
    // }

    /* public void ResetPosition()
     {
         currentCard.transform.parent.position = initialPosition;
         currentCard.transform.parent.rotation = Quaternion.identity;
         currentCard.transform.parent.DOScale(Vector3.one, 0);
     }*/

    public override void CheckLike()
    {
        // do something
        switch (currentCard.name.ToLower())
        {
            case "abridor de latas":
                estrela += -1;
                coracao += 0;
                mapa += 1;
                break;
            case "bandeira":
                estrela += 3;
                coracao += 0;
                mapa += -1;
                break;
            case "barraca deposito":
                estrela += 3;
                coracao += 2;
                mapa += 3;
                break;
            case "barraca individual":
                coracao += 1;
                estrela += 3;
                mapa += 2;
                break;
            case "barraca polar haven":
                coracao += 1;
                estrela += 3;
                mapa += 3;
                break;
            case "benjamin T":
                coracao += -1;
                estrela += -1;
                mapa += 0;
                break;
            case "cadeiras de praia":
                coracao += 0;
                estrela += -2;
                mapa += -1;
                break;
            case "cafeteira":
                coracao += -3;
                estrela += -2;
                mapa += -3;
                break;
            case "caixas de suprimento":
                coracao += 2;
                estrela += 3;
                mapa += 1;
                break;
            case "capa de chuva":
                coracao += 0;
                estrela += -1;
                mapa += 0;
                break;
            case "carreta de carga":
                coracao += -1;
                estrela += 3;
                mapa += -1;
                break;
            case "celular":
                coracao += 0;
                estrela += 2;
                mapa += 0;
                break;
            case "chinelo":
                coracao += 0;
                estrela += -2;
                mapa += 0;
                break;
            case "combustivel":
                coracao += 2;
                estrela += 4;
                mapa += -2;
                break;
            case "copos descartaveis":
                coracao += -3;
                estrela += -1;
                mapa += -3;
                break;
            case "detergente":
                coracao += 3;
                estrela += -1;
                mapa += -3;
                break;
            case "espelho":
                coracao += 0;
                estrela += -1;
                mapa += 0;
                break;
            case "espetos":
                coracao += -2;
                estrela += -2;
                mapa += -2;
                break;
            case "esponja":
                coracao += 2;
                estrela += -1;
                mapa += 0;
                break;
            case "estação meteorológica":
                coracao += 0;
                estrela += 3;
                mapa += 0;
                break;
            case "fita adesiva":
                coracao += 0;
                estrela += 1;
                mapa += -1;
                break;
            case "garrafa témica":
                coracao += 2;
                estrela += 4;
                mapa += 2;
                break;
            case "gerador":
                coracao += 0;
                estrela += 3;
                mapa += -1;
                break;
            case "guarda-sol":
                coracao += 0;
                estrela += -2;
                mapa += -2;
                break;
            case "guardanapos":
                coracao += 2;
                estrela += -1;
                mapa += -2;
                break;
            case "gás":
                coracao += -1;
                estrela += -2;
                mapa += -2;
                break;
            case "lanterna e pilhas":
                coracao += 1;
                estrela += 2;
                mapa += -2;
                break;
            case "lençois e cobertas":
                coracao += -1;
                estrela += -1;
                mapa += 0;
                break;
            case "luminária":
                coracao += 1;
                estrela += 2;
                mapa += 1;
                break;
            case "luvas de protrção":
                coracao += 2;
                estrela += 4;
                mapa += 3;
                break;
            case "martelete":
                coracao += 2;
                estrela += 4;
                mapa += -1;
                break;
            case "martelo":
                coracao += 2;
                estrela += 4;
                mapa += -1;
                break;
            case "mesa":
                coracao += -1;
                estrela += -2;
                mapa += 0;
                break;
            case "panela":
                coracao += -2;
                estrela += -2;
                mapa += 0;
                break;
            case "papel aluminio":
                coracao += 2;
                estrela += 2;
                mapa += 2;
                break;
            case "papel higiênico":
                coracao += 2;
                estrela += 2;
                mapa += -1;
                break;
            case "pincel":
                coracao += 2;
                estrela += 4;
                mapa += 2;
                break;
            case "pratos":
                coracao += 1;
                estrela += -1;
                mapa += 0;
                break;
            case "protetor solar":
                coracao += -2;
                estrela += 1;
                mapa += -1;
                break;
            case "quadriciclo":
                coracao += -1;
                estrela += 3;
                mapa += -1;
                break;
            case "repelente":
                coracao += 0;
                estrela += -2;
                mapa += -3;
                break;
            case "sabonete":
                coracao += 2;
                estrela += -1;
                mapa += -1;
                break;
            case "saco de dormir":
                coracao += -1;
                estrela += 3;
                mapa += 1;
                break;
            case "saco plástico":
                coracao += 2;
                estrela += 3;
                mapa += 2;
                break;
            case "sacos para lixo":
                coracao += 3;
                estrela += 2;
                mapa += 2;
                break;
            case "talhadeira":
                coracao += 2;
                estrela += 4;
                mapa += -1;
                break;
            case "talheres":
                coracao += 0;
                estrela += -1;
                mapa += 0;
                break;
            case "talheres descatavéis":
                coracao += 2;
                estrela += 4;
                mapa += -1;
                break;
            case "tesoura":
                coracao += 0;
                estrela += 1;
                mapa += 1;
                break;
            case "toalhas":
                coracao += 1;
                estrela += -2;
                mapa += 0;
                break;
            case "tonel":
                coracao += 2;
                estrela += 3;
                mapa += 3;
                break;
            case "travesseiros":
                coracao += 0;
                estrela += -1;
                mapa += 0;
                break;
            case "ventilador pequeno":
                coracao += 0;
                estrela += -1;
                mapa += 0;
                break;
            case "óculos de proteção":
                coracao += 2;
                estrela += 4;
                mapa += 2;
                break;

            default:
                coracao += 0;
                estrela += 0;
                mapa += 0;
                break;
        }

        CheckCalories(coracao, estrela, mapa);
        NextCard();
    }

    public override void CheckDislike()
    {
        switch (currentCard.name.ToLower())
        {
            case "abridor de latas":
                estrela -= -1;
                coracao -= 0;
                mapa -= 1;
                break;
            case "bandeira":
                estrela -= 3;
                coracao -= 0;
                mapa -= -1;
                break;
            case "barraca deposito":
                estrela -= 3;
                coracao -= 2;
                mapa -= 3;
                break;
            case "barraca individual":
                coracao -= 1;
                estrela -= 3;
                mapa -= 2;
                break;
            case "barraca polar haven":
                coracao -= 1;
                estrela -= 3;
                mapa -= 3;
                break;
            case "benjamin T":
                coracao -= -1;
                estrela -= -1;
                mapa -= 0;
                break;
            case "cadeiras de praia":
                coracao -= 0;
                estrela -= -2;
                mapa -= -1;
                break;
            case "cafeteira":
                coracao -= -3;
                estrela -= -2;
                mapa -= -3;
                break;
            case "caixas de suprimento":
                coracao -= 2;
                estrela -= 3;
                mapa -= 1;
                break;
            case "capa de chuva":
                coracao -= 0;
                estrela -= -1;
                mapa -= 0;
                break;
            case "carreta de carga":
                coracao -= -1;
                estrela -= 3;
                mapa -= -1;
                break;
            case "celular":
                coracao -= 0;
                estrela -= 2;
                mapa -= 0;
                break;
            case "chinelo":
                coracao -= 0;
                estrela -= -2;
                mapa -= 0;
                break;
            case "combustivel":
                coracao -= 2;
                estrela -= 4;
                mapa -= -2;
                break;
            case "copos descartaveis":
                coracao -= -3;
                estrela -= -1;
                mapa -= -3;
                break;
            case "detergente":
                coracao -= 3;
                estrela -= -1;
                mapa -= -3;
                break;
            case "espelho":
                coracao -= 0;
                estrela -= -1;
                mapa -= 0;
                break;
            case "espetos":
                coracao -= -2;
                estrela -= -2;
                mapa -= -2;
                break;
            case "esponja":
                coracao -= 2;
                estrela -= -1;
                mapa -= 0;
                break;
            case "estação meteorológica":
                coracao -= 0;
                estrela -= 3;
                mapa -= 0;
                break;
            case "fita adesiva":
                coracao -= 0;
                estrela -= 1;
                mapa -= -1;
                break;
            case "garrafa témica":
                coracao -= 2;
                estrela -= 4;
                mapa -= 2;
                break;
            case "gerador":
                coracao -= 0;
                estrela -= 3;
                mapa -= -1;
                break;
            case "guarda-sol":
                coracao -= 0;
                estrela -= -2;
                mapa -= -2;
                break;
            case "guardanapos":
                coracao -= 2;
                estrela -= -1;
                mapa -= -2;
                break;
            case "gás":
                coracao -= -1;
                estrela -= -2;
                mapa -= -2;
                break;
            case "lanterna e pilhas":
                coracao -= 1;
                estrela -= 2;
                mapa -= -2;
                break;
            case "lençois e cobertas":
                coracao -= -1;
                estrela -= -1;
                mapa -= 0;
                break;
            case "luminária":
                coracao -= 1;
                estrela -= 2;
                mapa -= 1;
                break;
            case "luvas de protrção":
                coracao -= 2;
                estrela -= 4;
                mapa -= 3;
                break;
            case "martelete":
                coracao -= 2;
                estrela -= 4;
                mapa -= -1;
                break;
            case "martelo":
                coracao -= 2;
                estrela -= 4;
                mapa -= -1;
                break;
            case "mesa":
                coracao -= -1;
                estrela -= -2;
                mapa -= 0;
                break;
            case "panela":
                coracao -= -2;
                estrela -= -2;
                mapa -= 0;
                break;
            case "papel aluminio":
                coracao -= 2;
                estrela -= 2;
                mapa -= 2;
                break;
            case "papel higiênico":
                coracao -= 2;
                estrela -= 2;
                mapa -= -1;
                break;
            case "pincel":
                coracao -= 2;
                estrela -= 4;
                mapa -= 2;
                break;
            case "pratos":
                coracao -= 1;
                estrela -= -1;
                mapa -= 0;
                break;
            case "protetor solar":
                coracao -= -2;
                estrela -= 1;
                mapa -= -1;
                break;
            case "quadriciclo":
                coracao -= -1;
                estrela -= 3;
                mapa -= -1;
                break;
            case "repelente":
                coracao -= 0;
                estrela -= -2;
                mapa -= -3;
                break;
            case "sabonete":
                coracao -= 2;
                estrela -= -1;
                mapa -= -1;
                break;
            case "saco de dormir":
                coracao -= -1;
                estrela -= 3;
                mapa -= 1;
                break;
            case "saco plástico":
                coracao -= 2;
                estrela -= 3;
                mapa -= 2;
                break;
            case "sacos para lixo":
                coracao -= 3;
                estrela -= 2;
                mapa -= 2;
                break;
            case "talhadeira":
                coracao -= 2;
                estrela -= 4;
                mapa -= -1;
                break;
            case "talheres":
                coracao -= 0;
                estrela -= -1;
                mapa -= 0;
                break;
            case "talheres descatavéis":
                coracao -= 2;
                estrela -= 4;
                mapa -= -1;
                break;
            case "tesoura":
                coracao -= 0;
                estrela -= 1;
                mapa -= 1;
                break;
            case "toalhas":
                coracao -= 1;
                estrela -= -2;
                mapa -= 0;
                break;
            case "tonel":
                coracao -= 2;
                estrela -= 3;
                mapa -= 3;
                break;
            case "travesseiros":
                coracao -= 0;
                estrela -= -1;
                mapa -= 0;
                break;
            case "ventilador pequeno":
                coracao -= 0;
                estrela -= -1;
                mapa -= 0;
                break;
            case "óculos de proteção":
                coracao -= 2;
                estrela -= 4;
                mapa -= 2;
                break;

            default:
                coracao -= 0;
                estrela -= 0;
                mapa -= 0;
                break;
        }

        CheckCalories(coracao, estrela, mapa);
        NextCard();
    }

    /* private void NextCard()
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
    }*/

    public void CheckCalories(float coracao, float estrela, float mapa)
    {
        // normalize
        fill.fillAmount += coracao / MAX_COR;

        fills.fillAmount += estrela / MAX_EST;

        fillm.fillAmount += mapa / MAX_MAP;

  

        if (fill.fillAmount == 0)
            Debug.Log("Zerou um marcador!");

        if (fills.fillAmount == 0)
            Debug.Log("Zerou um marcador!");

        if (fillm.fillAmount == 0)
            Debug.Log("Zerou um marcador!");

        if (fill.fillAmount <= 0.2)
            minijogosDicas.SetHintByIndex(0);
        Debug.Log("Cuidado com a limpeza do acampamento!!");
            
        if (fills.fillAmount <= 0.2)
            Debug.Log("Cuidado com seus pontos de experiência!!");

        if (fillm.fillAmount <= 0.2)
            Debug.Log("A Antártica não pode sofrer mais danos!");
    }


}
 
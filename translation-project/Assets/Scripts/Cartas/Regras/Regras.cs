using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Regras : AbstractCardManager {


    // Use this for initialization
    void Start () {
        cardIndex = 0;
        
        currentCard.sprite = sprites[cardIndex];
        currentCard.name = sprites[cardIndex].name;
        cardName.text = currentCard.name;

        nextCard.GetComponentInChildren<Image>().sprite = sprites[cardIndex + 1];
        nextCard.name = sprites[cardIndex + 1].name;

        initialPosition = currentCard.transform.parent.position;
    }

    public override void CheckDislike()
    {
        NextCard();
    }

    public override void CheckLike()
    {
        NextCard();
    }

}

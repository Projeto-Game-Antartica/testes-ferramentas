using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnalysisVegScreen : MonoBehaviour
{
    public Sprite[] VegSprites = new Sprite[9];

    public Image CurrentImage;

    private VegType correctAnswer;

    enum VegType {
        Angiosperma,
        Briofita,
        Liquen
    }

    private List<int> doneVegs = new List<int>();

    System.Random rnd = new System.Random();

    // public T[] ShuffleArray<T[]>(T[] array) {
    //     System.Random rnd = new System.Random();
    //     T[] randomArray = array.OrderBy(x => rnd.Next()).ToArray();
    //     return randomArray;
    // }

    public void ResetScreen() {
        //Get Random Sprite
        int randIndex = rnd.Next(VegSprites.Length);
        while(doneVegs.Contains(randIndex))
            randIndex = rnd.Next(VegSprites.Length);
        Sprite randomSprite = VegSprites[randIndex];
        CurrentImage.sprite = randomSprite;
        Debug.Log(randomSprite .name);
        Must set the correct answer routines 
        call the next screens
    }


    // Start is called before the first frame update
    void Start()
    {
        ResetScreen();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

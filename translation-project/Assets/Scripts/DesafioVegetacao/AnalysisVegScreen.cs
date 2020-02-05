using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnalysisVegScreen : MonoBehaviour
{
    public Sprite[] VegSprites = new Sprite[9];

    public DesafioVeg GameScreen;

    public Image CurrentImage;

    private VegType correctAnswer;

    public ButtonGroup Options;

    enum VegType {
        Angiosperma,
        Briofita,
        Liquen
    }

    private List<int> doneVegs = new List<int>();
    private int harvNumber = 1;

    public TMPro.TMP_InputField HarvestNumber;

    public GameObject WinScreen;

    System.Random rnd = new System.Random();

    // public T[] ShuffleArray<T[]>(T[] array) {
    //     System.Random rnd = new System.Random();
    //     T[] randomArray = array.OrderBy(x => rnd.Next()).ToArray();
    //     return randomArray;
    // }

    public void OnConfirmClick() {
        
        int selectedIndex = Options.GetSelectedIndex();

        if(selectedIndex < 0)
            return;

        VegType selectedAnswer = (VegType)selectedIndex;

        harvNumber++;

        if(selectedAnswer == correctAnswer) {
            Debug.Log("Resposta Certa!");
        } else {
            Debug.Log("Resposta Errada!");
        }

        if(harvNumber <= 3) {
            GameScreen.ResetHarvestScreen();
            GameScreen.ShowOkDialog("Parabéns, vegetação classificada. Realize nova coleta.", GameScreen.ShowHarvestScreen);
        } else {
            WinScreen.SetActive(true);
        }
    }

    public void ResetScreen() {
        HarvestNumber.text = harvNumber.ToString();
        Options.ClearSelection();

        //Get Random Sprite
        int randIndex = rnd.Next(VegSprites.Length);
        while(doneVegs.Contains(randIndex))
            randIndex = rnd.Next(VegSprites.Length);
        doneVegs.Add(randIndex);
        Sprite randomSprite = VegSprites[randIndex];
        CurrentImage.sprite = randomSprite;

        //Set correct answer
        if(randomSprite.name.StartsWith("M")) //Musgo = Briofita
            correctAnswer = VegType.Briofita;
        else if(randomSprite.name.StartsWith("L")) //Liquen
            correctAnswer = VegType.Liquen;
        else if(randomSprite.name.StartsWith("D")) //Deschampista = Angiosperma
            correctAnswer = VegType.Angiosperma;
        else
            throw new NotImplementedException("Tipo nao identificado " + randomSprite.name);
    }


    // Start is called before the first frame update
    void Start()
    {
        ResetScreen();
    }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }
}

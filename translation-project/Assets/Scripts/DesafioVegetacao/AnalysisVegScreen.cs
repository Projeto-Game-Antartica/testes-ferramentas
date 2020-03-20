using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class AnalysisVegScreen : MonoBehaviour
{
    public Sprite[] VegSprites = new Sprite[9];

    public DesafioVeg GameScreen;

    public Image CurrentImage;

    private VegType correctAnswer;

    public ButtonGroup Options;

    public Button ConfirmButton;

    public Button stdButton;

    enum VegType {
        Angiosperma,
        Briofita,
        Liquen
    }

    private List<int> doneVegs = new List<int>();
    private int harvNumber = 1;

    public TMPro.TMP_InputField HarvestNumber, HarvestDateTime;

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

        ConfirmButton.interactable = false;

        VegType selectedAnswer = (VegType)selectedIndex;

        harvNumber++;

        if(selectedAnswer == correctAnswer) {
            Debug.Log("Resposta Certa!");
            Options.SetButtonColor(selectedIndex, Color.green);           
        } else {
            Debug.Log("Resposta Errada!");
            Options.SetButtonColor(selectedIndex, Color.red);
        }

        if(harvNumber <= 3) {
            DoAfter(3, resetHarvestAndShowDialog);
        } else {
            DoAfter(3, showWinScreen);
        }
    }

    private void resetHarvestAndShowDialog() {
        GameScreen.ResetHarvestScreen();
        GameScreen.OkDialogBox.Show("Parabéns, vegetação classificada. Realize nova coleta.", GameScreen.ShowHarvestScreen);
        //GameScreen.ShowOkDialog("Parabéns, vegetação classificada. Realize nova coleta.", GameScreen.ShowHarvestScreen);
    }

    private void showWinScreen() {
        PlayerPreferences.M010_Desafio_Done = true;
        WinScreen.SetActive(true);
        DoAfter(5, ReturnToCamp);
    }

    public void ResetScreen() {
        HarvestNumber.text = harvNumber.ToString();
        Options.ClearSelection();

        DateTime currentDT = DateTime.Now;

        HarvestDateTime.text = currentDT.ToString("dd/MM/yyyy | HH:mm");

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

        ConfirmButton.interactable = true;
    }


    // Start is called before the first frame update
    void Start()
    {
        ResetScreen();
        stdButton.Select();
    }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }

    public void DoAfter(int secs, UnityAction action) {
        StartCoroutine(DoAfterCoroutine(secs, action));
    }

    public IEnumerator DoAfterCoroutine(int secs, UnityAction action) {
        yield return new WaitForSeconds(secs);
        action();
    }

    //Volta para o acampamento
    public void ReturnToCamp() {
        UnityEngine.SceneManagement.SceneManager.LoadScene(ScenesNames.M010Camp);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class AnalysisVegScreen : AbstractScreenReader
{
    private Sprite[] VegSprites;
    private string spriteDescription;

    private string currentDescription;

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

    public TMPro.TMP_Text GameCommandsText;

    string gameCommands = @"1- Para acessar as duas áreas da tela (imagem da vegetação e opções de resposta): tecla F6 
2- Navegação nas opções de resposta: teclas direcionais (direita e esquerda)
3- Selecionar opção: tecla enter";

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

        currentDescription = ReadableTexts.instance.GetReadableText("m010_desafio_win", LocalizationManager.instance.GetLozalization());

        ReadText(currentDescription);

        DoAfter(5, ReturnToCamp);
    }

    public void ResetScreen() {
        currentDescription = ReadableTexts.instance.GetReadableText("m010_desafio_catalog", LocalizationManager.instance.GetLozalization());

        //Check number of resources available
        UnityEngine.Object[] spriteAssets = Resources.LoadAll("coleta_veg", typeof(Sprite));
        int nResources = spriteAssets.Length;

        //Get Random Sprite
        int randIndex = rnd.Next(nResources);
        while(doneVegs.Contains(randIndex))
            randIndex = rnd.Next(nResources);
        doneVegs.Add(randIndex);
        
        Sprite randomSprite = Resources.Load<Sprite>("coleta_veg/" + randIndex);
        CurrentImage.sprite = randomSprite;

        string[] spriteInfo = Resources.Load<TextAsset>("coleta_veg/" + randIndex).text.Split('\t');
        spriteDescription = spriteInfo[1];

        if(spriteInfo[0] == "Musgo")
            correctAnswer = VegType.Briofita;
        else if(spriteInfo[0] == "Angiospermas")
            correctAnswer = VegType.Angiosperma;
        else if(spriteInfo[0] == "Liquen")
            correctAnswer = VegType.Liquen;
        else
            throw new NotImplementedException("Unknown vegetable: " + spriteInfo[0]);   


        // VegSprites = new Sprite[spriteAssets.Length];
        // foreach(UnityEngine.Object s in spriteAssets) {
        //     int index = Int32.Parse(s.name);
        //     VegSprites[index] = s as Sprite;
        // }
        
        // Resources.Load

        // //Load card descriptions and correct answers
        // UnityEngine.Object[] descTexts = Resources.LoadAll("coleta_veg", typeof(TextAsset));
        // spriteDescription = new string[descTexts.Length];
        // correctAnswer = new VegType[descTexts.Length];
        // foreach(UnityEngine.Object desc in descTexts) {
        //     int index = Int32.Parse(desc.name);
        //     string[] descSplit = (desc as TextAsset).text.Split('\t');
        //     cardDescriptions[index] = descSplit[1];
        //     if(descSplit[0] == "alga")
        //         correctAnswer[index] = Vegetais.Alga;
        //     else if(descSplit[0] == "planta")
        //         correctAnswer[index] = Vegetais.Planta;
        //     else if(descSplit[0] == "fungo")
        //         correctAnswer[index] = Vegetais.Fungo;
        //     else
        //         throw new NotImplementedException("Unknown vegetable: " + descSplit[0]);      
        // }

        GameCommandsText.text = gameCommands;
        HarvestNumber.text = harvNumber.ToString();
        Options.ClearSelection();

        DateTime currentDT = DateTime.Now;

        HarvestDateTime.text = currentDT.ToString("dd/MM/yyyy | HH:mm");

        // //Get Random Sprite
        // int randIndex = rnd.Next(VegSprites.Length);
        // while(doneVegs.Contains(randIndex))
        //     randIndex = rnd.Next(VegSprites.Length);
        // doneVegs.Add(randIndex);
        // Sprite randomSprite = VegSprites[randIndex];
        // CurrentImage.sprite = randomSprite;

        //Set correct answer
        // if(randomSprite.name.StartsWith("M")) //Musgo = Briofita
        //     correctAnswer = VegType.Briofita;
        // else if(randomSprite.name.StartsWith("L")) //Liquen
        //     correctAnswer = VegType.Liquen;
        // else if(randomSprite.name.StartsWith("D")) //Deschampista = Angiosperma
        //     correctAnswer = VegType.Angiosperma;
        // else
        //     throw new NotImplementedException("Tipo nao identificado " + randomSprite.name);
        

        ConfirmButton.interactable = true;

        ReadText(currentDescription);
        ReadText(spriteDescription);
    }


    // Start is called before the first frame update
    void Start()
    {
        ResetScreen();
        stdButton.Select();
    }

    // // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(InputKeys.REPEAT_KEY) && currentDescription != null) {
            ReadText(currentDescription);
            ReadText(spriteDescription);
        }   
    }

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

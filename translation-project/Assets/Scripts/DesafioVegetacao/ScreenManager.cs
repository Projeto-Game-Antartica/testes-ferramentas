using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenManager : AbstractScreenReader
{


    public GameObject EsquadroCort, Esquadro, Faca, Espatula, EspatulaPlanta, 
        Planta, PlantaFora, Pote, PoteFull, Saco, SacoFull, HarvestCompleted, AnalysisScreen, HarvestScreen;

    
    public Button BackButton, ResetButton;

    private int gameState = 0;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Started.");
    }

    // Update is called once per frame
    void Update()
    {
        switch(gameState) {
            case 0:
                //Initial state, do nothing
                break;

            case 1:
                EsquadroCort.SetActive(false);
                Esquadro.SetActive(true);
                break;

            case 2:
                Faca.SetActive(true);
                break;
            
            case 3:
                Faca.SetActive(false);
                EspatulaPlanta.SetActive(true);
                break;

            case 4:
                Espatula.SetActive(true);
                EspatulaPlanta.SetActive(false);
                PlantaFora.SetActive(true);
                Planta.SetActive(false);
                Pote.SetActive(true);
                break;

            case 5:
                PlantaFora.SetActive(false);
                Pote.SetActive(false);
                PoteFull.SetActive(true);
                break;

            case 6:
                Espatula.SetActive(false);
                Saco.SetActive(true);
                break;

            case 7:
                Saco.SetActive(false);
                PoteFull.SetActive(false);
                SacoFull.SetActive(true);
                HarvestCompleted.SetActive(true);
                break;

            case 8:
                HarvestScreen.SetActive(false);
                AnalysisScreen.SetActive(true);
                break;
        
            default:
                break;

        }
    }



    //Stores the states where each equipment has a valid action
    private List<List<int>> equipmentsActionStates = new List<List<int>> {
        new List<int>{1}, //Faca
        new List<int>{2,4}, //Espatula
        new List<int>{0}, //Quadrante
        new List<int>{3,6}, //Pote
        new List<int>{5}  //Saco
    };

    public void OnEquipmentClick(int eqValue) {
        if(equipmentsActionStates[eqValue].Contains(gameState))
            gameState++;
        else
            showTip();
    }

    public void OnHarvestCompleteOkClick() {
        gameState++;
    }
 
    private string[] tips = new string[] {
        "Você deve usar o quadrante",
        "Você deve usar a faca",
        "Você deve usar a espatula",
        "Você deve usar o pote",
        "Você deve usar a espatula",
        "Você deve usar o saco",
        "Você deve usar o pote",
    };

    private void showTip() {
        if (gameState < tips.Length)
            Debug.Log(tips[gameState]);
    }

    public void InitializeGame()
    {

        BackButton.interactable = true;
        ResetButton.interactable = true;

    }

    public void ReturnToShip()
    {
        //if (!PlayerPreferences.M009_Memoria) lifeExpController.RemoveEXP(0.0001f); // saiu sem concluir o minijogo
        //UnityEngine.SceneManagement.SceneManager.LoadScene(ScenesNames.M009Camp);
    }

    public void ResetScene()
    {
        //UnityEngine.SceneManagement.SceneManager.LoadScene(ScenesNames.M009Desafio);
    }




}

﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DesafioVeg : MonoBehaviour
{

    //Scenario
    public GameObject Plant, PlantDetached, BowlFull, BagFull, FramePlaced, BowlPlaced, OkDialog;

    Action okDialogCallback;

    //Tools
    public GameObject[] Tools = new GameObject[6];
    private int currentToolIndex = -1;

    private enum Tool {
        Frame,
        Knife,
        Spatula,
        SpatulaWithPlant,
        Bowl,
        Bag,
        None=-1
    }

    public GameObject[] Grid = new GameObject[16];

    private int selectedGridIndex = -1;

    private int plantIndex = -1;
    private int bowlIndex = -1;

    private GameState currentGameState = GameState.Initial;

    private enum GameState {
        Initial,
        PlantFixed,
        FramePlaced,
        PlantDetached,
        SpatulaWithPlant,
        BowlPlaced,
        PlantInBowl,
        BowlInBag
    }



    // Start is called before the first frame update
    void Start() {
        //changeTool(1);

        plantIndex = 0; //Must start at random
        changeGameState(GameState.PlantFixed);
    }

    // Update is called once per frame
    void Update() {

        if(selectedGridIndex > -1 && currentToolIndex > -1) {
            Tools[currentToolIndex].transform.SetParent(Grid[selectedGridIndex].transform, false);
        }

    }

    private void changeTool(int toolIndex) {
        currentToolIndex = toolIndex;
        
        foreach(GameObject tool in Tools)
            tool.SetActive(false); 

        if(toolIndex > -1) {
            //Tools[toolIndex].transform.SetParent(Grid[selectedGridIndex].transform, false);
            Tools[toolIndex].SetActive(true);
        }
    }

    private void changeTool(Tool tool) {
        changeTool((int)tool);
    }

    //Function to attempt choose other tool. Depending on game state it will suceed
    public void AttemptChangeTool(int tool) {
        //Later must restrict this
        if((Tool)tool == Tool.Spatula && (currentGameState == GameState.SpatulaWithPlant || currentGameState == GameState.BowlPlaced)) {
            tool = (int)Tool.SpatulaWithPlant;
        }

        if((Tool)tool == Tool.Frame && currentGameState >= GameState.FramePlaced)
            tool = -1;

        if((Tool)tool == Tool.Bowl && bowlIndex > -1)
            tool = -1;

        changeTool(tool);
    }

    private void warningMessage(string message) {
        Debug.Log(message);
    }

    public void OnOkDialogClick() {
        if(okDialogCallback != null)
            okDialogCallback();
    }

    public void ShowOkDialog(string message) {
         ShowOkDialog(message, null);
    }

    public void ShowOkDialog(string message, Action onOkClick) {
        okDialogCallback = onOkClick;
        OkDialog.GetComponentsInChildren<TMPro.TextMeshProUGUI>()[0].text = message;
        OkDialog.SetActive(true);
    }

    private void takeAction(int gridIndex) {
        //warningMessage("GRIDCLICK" + gridIndex);

        switch(currentGameState) {
            case GameState.PlantFixed:
                if(currentToolIndex == (int)Tool.Frame) {
                    if(plantIndex == selectedGridIndex) {
                        changeGameState(GameState.FramePlaced);
                        changeTool(-1);
                    }                    
                } else {
                    warningMessage("Você precisa colocar o quadrante antes!");
                }
                break;

            case GameState.FramePlaced:
                if(currentToolIndex == (int)Tool.Knife)
                    if(plantIndex == selectedGridIndex) {
                        changeGameState(GameState.PlantDetached);
                    }
                else
                    warningMessage("Você precisa cortar a vegetação!");
                break;

            case GameState.PlantDetached:
                if(currentToolIndex == (int)Tool.Spatula) {
                    if(plantIndex == selectedGridIndex) {
                        changeGameState(GameState.SpatulaWithPlant);
                        changeTool(Tool.SpatulaWithPlant);
                    }
                } else
                    warningMessage("Você precisa pegar a vegetação!");
                break;

            case GameState.SpatulaWithPlant:
                if(currentToolIndex == (int)Tool.Bowl) {
                    if(plantIndex != selectedGridIndex) {
                        bowlIndex = selectedGridIndex;
                        changeGameState(GameState.BowlPlaced);
                        changeTool(-1);
                    }

                } else
                    warningMessage("Você precisa posicionar o potinho!");
                break;

            case GameState.BowlPlaced:
                if(currentToolIndex == (int)Tool.SpatulaWithPlant) {
                    if(selectedGridIndex == bowlIndex) {
                        changeGameState(GameState.PlantInBowl);
                        changeTool(Tool.Spatula);
                    }
                } else
                    warningMessage("Você precisa colocar a vegetação no potinho!");
                break;

            case GameState.PlantInBowl:
                if(currentToolIndex == (int)Tool.Bag) {
                    if(selectedGridIndex == bowlIndex) {
                        changeGameState(GameState.BowlInBag);
                        changeTool(-1);
                        finishHarvest();
                    }
                } else
                    warningMessage("Você precisa colocar o potinho no saco!");
                break;

            case GameState.BowlInBag:
                break;

            default:
                break;
        }
    }

    private void finishHarvest() {
        warningMessage("Você concluiu a colheita!");
    }

    private void changeGameState(GameState state) {
        Plant.SetActive(false);  
        PlantDetached.SetActive(false); 
        BowlFull.SetActive(false); 
        BagFull.SetActive(false);
        FramePlaced.SetActive(false);
        BowlPlaced.SetActive(false);

        currentGameState = state;

        switch(currentGameState) {
            case GameState.Initial:
                break;

            case GameState.PlantFixed:
                Plant.SetActive(true);
                Plant.transform.SetParent(Grid[plantIndex].transform, false);
                break;

            case GameState.FramePlaced:
                FramePlaced.SetActive(true);
                Plant.SetActive(true);
                FramePlaced.transform.SetParent(Grid[plantIndex].transform, false);
                Plant.transform.SetParent(Grid[plantIndex].transform, false);
                FramePlaced.transform.SetSiblingIndex(0);
                break;

            case GameState.PlantDetached:
                FramePlaced.SetActive(true);
                PlantDetached .SetActive(true);
                FramePlaced.transform.SetParent(Grid[plantIndex].transform, false);
                PlantDetached.transform.SetParent(Grid[plantIndex].transform, false);
                break;

            case GameState.SpatulaWithPlant:
                FramePlaced.SetActive(true);
                FramePlaced.transform.SetParent(Grid[plantIndex].transform, false);
                break;

            case GameState.BowlPlaced:
                FramePlaced.SetActive(true);
                FramePlaced.transform.SetParent(Grid[plantIndex].transform, false);
                BowlPlaced.SetActive(true);
                BowlPlaced.transform.SetParent(Grid[bowlIndex].transform, false);
                break;

            case GameState.PlantInBowl:
                FramePlaced.SetActive(true);
                FramePlaced.transform.SetParent(Grid[plantIndex].transform, false);
                BowlFull.SetActive(true);
                BowlFull.transform.SetParent(Grid[bowlIndex].transform, false);
                break;

            case GameState.BowlInBag:
                FramePlaced.SetActive(true);
                FramePlaced.transform.SetParent(Grid[plantIndex].transform, false);
                BagFull.SetActive(true);
                BagFull.transform.SetParent(Grid[bowlIndex].transform, false);
                break;

            default:
                break;

        }
    }


    public void OnTestButtonClick() {
        //Plant.SetActive(false);
        //PlantDetached.SetActive(true);
        //Grid[0].
        //Plant.transform.SetParent(Grid[1].transform, false);
        //changeTool(currentToolIndex + 1);
        //changeGameState(GameState.BowlInBag);
        ShowOkDialog("Oiiii", finishHarvest);
    }

    public void OnGridClick(int gridIndex) {
        takeAction(gridIndex);
    }

    public void OnGridEnter(int gridIndex) {
        selectGrid(gridIndex);
    }

    public void OnGridExit() {
        //deselectGrid();
    }

    private void selectGrid(int gridIndex) {
        selectedGridIndex = gridIndex;
    }

    private void deselectGrid() {
        selectedGridIndex = -1;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesafioVeg : MonoBehaviour
{

    //Scenario
    public GameObject Plant, PlantDetached, BowlFull, BagFull, FramePlaced, BowlPlaced;

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

    private int plantIndex = 1;
    private int bowlIndex = 8;
    private bool bowlPlaced = false;
    private bool framePlaced = false;

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
        changeTool(1);
        changeGameState(GameState.Initial);
    }

    // Update is called once per frame
    void Update() {

        if(selectedGridIndex >= 0) {
            Tools[currentToolIndex].transform.SetParent(Grid[selectedGridIndex].transform, false);
        }

    }

    private void changeTool(int toolIndex) {
        currentToolIndex = toolIndex;
        
        foreach(GameObject tool in Tools)
            tool.SetActive(false); 

        if(toolIndex > -1)
            Tools[toolIndex].SetActive(true);
    }

    private void changeTool(Tool tool) {
        changeTool((int)tool);
    }

    //Function to attempt choose other tool. Depending on game state it will suceed
    public void AttemptChangeTool(int tool) {
        //Later must restrict this
        changeTool(tool);
    }

    private void warningMessage(string message) {
        Debug.Log(message);
    }

    private void takeAction(int gridIndex) {
        warningMessage("" + gridIndex);
    }

    must discover why other events are triggering on click

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
        changeGameState(GameState.BowlInBag);
    }

    public void OnGridClick(int gridIndex) {
        takeAction(gridIndex);
    }

    public void OnGridEnter(int gridIndex) {
        selectGrid(gridIndex);
    }

    public void OnGridExit() {
        deselectGrid();
    }

    private void selectGrid(int gridIndex) {
        selectedGridIndex = gridIndex;
    }

    private void deselectGrid() {
        selectedGridIndex = -1;
    }
}

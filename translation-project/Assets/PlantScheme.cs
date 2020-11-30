using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlantScheme : AbstractScreenReader
{
    GameObject canvasGo;
    bool allowHide = false;

    string[,] tableContent = {
        {"", "Briófitas", "Pteridófitas", "Gimnospermas", "Angiospermas"},
        {"Possui vasos?", "NÃO", "SIM", "SIM", "SIM"},
        {"Produz semente?", "NÃO", "NÃO", "SIM", "SIM"},
        {"Possui flor?", "NÃO", "NÃO", "NÃO (POSSUI ESTRÓBILOS)", "SIM"},
        {"Produz fruto?", "NÃO", "NÃO", "NÃO", "SIM"},
        {"Fase dominante?", "GAMETÓFITO", "ESPORÓFITO", "ESPORÓFITO", "ESPORÓFITO"},
        {"Dependência da água para a reprodução?", "SIM", "SIM", "NÃO", "NÃO"},
        {"Estrutura", "Rizoide, cauloide e filoide", "Raiz, caule e folhas", "Raiz, caule, folhas, estróbilos e sementes", "Raiz, caule, folhas, flores, sementes e frutos"}
    };
    int[] selectedCell = {0, 0};
    bool cellChanged  = false;

    void Start() {
        canvasGo = transform.GetChild(0).gameObject;
    }   


    void Update()
    {       
        if (Input.GetMouseButtonUp(0) || Input.GetKeyDown(KeyCode.Space)|| Input.GetKeyDown(KeyCode.Return)) {
            if(allowHide) Hide();
        }

        //Math.Clamp is used to clip the position values to constrained inside the screen limits.
        if(canvasGo.activeSelf) {

            if (Input.GetKeyDown(KeyCode.UpArrow)) {
                selectedCell[0] = Mathf.Clamp(selectedCell[0] - 1, 0, 7);
                cellChanged = true;
            }            

            if (Input.GetKeyDown(KeyCode.DownArrow)) {
                selectedCell[0] = Mathf.Clamp(selectedCell[0] + 1, 0, 7);
                cellChanged = true;
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                selectedCell[1] = Mathf.Clamp(selectedCell[1] - 1, 0, 4);
                cellChanged = true;
            }

            if (Input.GetKeyDown(KeyCode.RightArrow)) {
                selectedCell[1] = Mathf.Clamp(selectedCell[1] + 1, 0, 4);
                cellChanged = true;
            }

            if(cellChanged) {
                cellChanged = false;
                string cellContent = tableContent[selectedCell[0], selectedCell[1]];
                //Debug.Log(cellContent);
                ReadText(cellContent);
            }
        }


    }

    public void Show() {
        canvasGo.SetActive(true);
        //Must generate a delay, otherwise the screen will show and imediatelly hide
        ReadText("Grupo de Plantas");
        StartCoroutine(allowHideAfterDelay(0.1f));   
    }

    //This delay is necessary otherwise this window will open and close imediatelly due to two key captures.
    IEnumerator allowHideAfterDelay(float secs) {
        yield return new WaitForSeconds(secs);
        allowHide = true;
    }
    public void Hide() {
        canvasGo.SetActive(false);
        allowHide = false;
    }
}

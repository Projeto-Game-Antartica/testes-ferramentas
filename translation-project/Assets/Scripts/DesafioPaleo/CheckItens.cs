using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;

public class CheckItens : DragAndDropController
{
    private readonly string instructions = "Início do jogo. Minijogo da teia alimentar. Descrição...";

    public TMPro.TextMeshProUGUI attemptsText;
    private int attempts = 3;
    private int total = 0;

    public Button confirmaButton;

    public GameObject LoseImage;

    // count the correct/wrong drops
    private int correctAnswer = 0;
    private int wrongAnswer = 0;
    private bool WIN = false;

    public GameObject[] Celulas;

    public AudioClip correctClip;
    public AudioClip wrongClip;

    public MinijogosDicas dicas;

    public LifeExpController lifeExpController;

    public GameObject WinImage;
    public TMPro.TextMeshProUGUI WinText;

    private List<GameObject> draggedItems; //guarda itens
    private List<GameObject> draggedCell; //guarda itens
    private List<GameObject> draggedLocal; //guarda local 
    private List<GameObject> remover; //itens para remover

    private List<DragAndDropCell.DropEventDescriptor> passar_itens;

    public Button audioButton;
    public GameObject firstItem;

    enum Cells
    {
        bentosCell = 1, avesMarinhasCell, pinguinsCell, baleiasCell, krillCell,
        cefalopodesCell, focasCell, peixesCell, protozariosCell, cetaceosCell,
        zooplanctonsCell, bacteriasCell, algasCell
    };

    private void Start()
    {

        

        //confirmaButton.interactable = false;

        draggedItems = new List<GameObject>();
        draggedCell = new List<GameObject>();
        draggedLocal = new List<GameObject>();
        remover = new List<GameObject>();

        passar_itens = new List<DragAndDropCell.DropEventDescriptor>();

        //sReadText(instructions);

        NRO_CELLS = 12;

        Debug.Log(cells.Length);
        foreach(GameObject g in cells)
        {
            Debug.Log(g.name);
        }

        //audioSource = GetComponent<AudioSource>();

        // start afther time seconds and repeat at repeatRate rate
        //InvokeRepeating("CallHintMethod", dicas.time, dicas.repeatRate);
    }

    
    private void Update()
    {

        if(Input.GetKeyDown(KeyCode.Space))
        {
            try
            {
                if (EventSystem.current.currentSelectedGameObject.GetComponentInChildren<DragAndDropItem>().gameObject.tag.Equals("item"))
                {
                    OnButtonClick();
                    isPositioning = true;
                }
            } catch (Exception e)
            {
                Debug.Log("Não é um item. Stacktrace >>" + e.StackTrace);
            }
        }

        if ((Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow) ||
                                    Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow)))
        {
            GameObject nextCell = EventSystem.current.currentSelectedGameObject.gameObject;

            if (currentItem != null)
            {
                DragAndDropItem.icon.GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(nextCell.transform.position);
            }

            if (isCell)
            {
                nextCell.GetComponent<Selectable>().Select();
                Debug.Log("Célula " + ReturnCellNumber(nextCell.name));
                Debug.Log(ReturnCellInfo(nextCell.name));
                //ReadText(nextCell.name);
                ReadText("Célula " + ReturnCellNumber(nextCell.name));
                ReadText(ReturnCellInfo(nextCell.name));   
            }
            else
            {
                nextCell.GetComponent<Selectable>().Select();
                Debug.Log(nextCell.GetComponentInChildren<DragAndDropItem>().gameObject.name);
                ReadText(nextCell.GetComponentInChildren<DragAndDropItem>().gameObject.name);
            }
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            try
            {
                DragAndDropCell.DropEventDescriptor desc = new DragAndDropCell.DropEventDescriptor();
                currentCell = EventSystem.current.currentSelectedGameObject.GetComponent<DragAndDropCell>();

                desc.item = currentItem;
                desc.sourceCell = sourceCell;
                desc.destinationCell = currentCell;
                currentCell.SendRequest(desc);                      // Send drop request
                StartCoroutine(currentCell.NotifyOnDragEnd(desc));  // Send notification after drop will be finished

                if (desc.permission == true)
                {
                    currentCell.PlaceItem(currentItem);
                }

                currentCell.UpdateMyItem();
                currentCell.UpdateBackgroundState();
                sourceCell.UpdateMyItem();
                sourceCell.UpdateBackgroundState();

                // go to item
                firstItem.GetComponent<Selectable>().Select();

                ResetConditions();

                isPositioning = false;
            }
            catch(Exception e)
            {
                Debug.Log("Não é uma célula. Stacktrace >>" + e.StackTrace);
            }
        }

        if(Input.GetKeyDown(KeyCode.F6) && !isPositioning)
        {
            isCell = !isCell;

            if (isCell)
            {
                ReadText("Células");
                Debug.Log("Células");
                cells[0].GetComponent<Selectable>().Select();
            }
            else
            {
                ReadText("Itens");
                Debug.Log("Itens");
                firstItem.GetComponent<Selectable>().Select();
            }
        }
    }

    public void CallHintMethod()
    {
        dicas.StartHints();
    }
    
    public void GoWrapper()
    {

        StartCoroutine(GoCoroutine());               

        //Debug.Log("teste: " + transform.GetComponent("Era1"));

    }

    public IEnumerator GoCoroutine()
    {
        int contador = 0;

        int cont_erro = 0;

        foreach (GameObject g in draggedItems)
        {
            string nome = g.name; 

            Debug.Log("cont: " + contador + " g.name: " + g.name + "Cell: " + draggedCell[contador].name);
            Debug.Log("Nome: " +nome + "cell: " + g.name);

            if (nome.Contains(draggedCell[contador].name))
            {
                Debug.Log("Acerto");
                correctAnswer++;
            }
            else
            {
                Debug.Log("Devolver: " + contador);
                passar_itens[contador].sourceCell.PlaceItem(passar_itens[contador].item);
                //draggedCell.RemoveAt(contador);
                remover.Add(g);
                //draggedLocal.RemoveAt(contador);

                cont_erro++;
            }
            contador++;                  
        }
        
        if (cont_erro > 0)
            {
                wrongAnswer++;
                attemptsText.text = "Tentativas restantes: " + wrongAnswer + "/" + attempts;
                confirmaButton.interactable = false;
            }
        
        foreach (GameObject re in remover)
        {
            Debug.Log("antes: " + re);
            // remove o item que foi dropado
            draggedItems.Remove(re);
            //Debug.Log("depois: " + re);

            // re nao esta nessas duas listas
            //draggedCell.Remove(re);
            //draggedLocal.Remove(re);
            total--;
            Debug.Log("Total: " + total);
        }

        // clean all lists
        remover.Clear();
        draggedCell.Clear();
        draggedItems.Clear();
        draggedLocal.Clear();
        passar_itens.Clear();
        
        

        yield return new WaitForSeconds(1.2f);
    }

    public void EndGame(bool win)
    {
        if (win)
        {
            WinImage.SetActive(true);
            //WinImage.GetComponentInChildren<Button>().Select();

            lifeExpController.AddEXP(0.001f); // finalizou o minijogo
            lifeExpController.AddEXP(0.0002f); // ganhou o item
        }
        else
        {
            LoseImage.SetActive(true);
            lifeExpController.AddEXP(0.0001f); // jogou um minijogo
        }

        //StartCoroutine(ReturnToUshuaiaCoroutine()); // volta para o navio perdendo ou ganhando o minijogo
    }

    public override void OnSimpleDragAndDropEvent(DragAndDropCell.DropEventDescriptor desc)
    {
        // Get control unit of source cell
        CheckItens sourceSheet = desc.sourceCell.GetComponentInParent<CheckItens>();
        // Get control unit of destination cell
        CheckItens destinationSheet = desc.destinationCell.GetComponentInParent<CheckItens>();
        switch (desc.triggerType)                                               // What type event is?
        {
            case DragAndDropCell.TriggerType.DropRequest:                       // Request for item drag (note: do not destroy item on request)
                Debug.Log("teste: " + sourceSheet.name);
                Debug.Log("Request " + desc.item.name + " from " + sourceSheet.name + " to " + destinationSheet.name);
                break;
            case DragAndDropCell.TriggerType.DropEventEnd:                      // Drop event completed (successful or not)
                if (desc.permission == true)                                    // If drop successful (was permitted before)
                {
                    Debug.Log("Successful drop " + desc.item.name + " from " + sourceSheet.name + " to " + destinationSheet.name);
                    //Debug.Log("ITEM: " + desc.item.name + ". DESTINATION CELL: " + desc.destinationCell.name);
                    Debug.Log("RS: " + desc.item.gameObject.name);
                    //draggedItems.Add(desc.item.gameObject);
                    Debug.Log("ITEM: " + desc.destinationCell.gameObject.name);
                    //draggedCell.Add(desc.destinationCell.gameObject);

                    passar_itens.Add(desc);

                    
                    guarda(desc.item.gameObject, desc.destinationCell.gameObject, desc.sourceCell.gameObject, desc);

                    //PlayAudioClip(correctClip);
                    //correctAnswer++;
                    //if (correctAnswer >= NRO_CELLS) WIN = true;
                    //CheckEndGame();
                }
                else                                                            // If drop unsuccessful (was denied before)
                {
                    Debug.Log("Denied drop " + desc.item.name + " from " + sourceSheet.name + " to " + destinationSheet.name);

                    //PlayAudioClip(wrongClip);
                    //wrongAnswer++;
                }
                break;
            case DragAndDropCell.TriggerType.ItemAdded:                         // New item is added from application
                Debug.Log("Item " + desc.item.name + " added into " + destinationSheet.name);
                break;
            case DragAndDropCell.TriggerType.ItemWillBeDestroyed:               // Called before item be destructed (can not be canceled)
                Debug.Log("Item " + desc.item.name + " will be destroyed from " + sourceSheet.name);
                break;
            default:
                Debug.Log("Unknown drag and drop event");
                break;
        }
    }

    public void guarda(GameObject item, GameObject cell, GameObject local, DragAndDropCell.DropEventDescriptor passa)
    {
        Debug.Log("Guardo item: " + item.name);
        draggedItems.Add(item);

        Debug.Log("Guardo Cell: " + cell.name);
        draggedCell.Add(cell);

        Debug.Log("Guardo local: " + local.name);
        draggedLocal.Add(local);

        total++;
        Debug.Log(total);

    }
    
    public void CheckEndGame()
    {
        if (WIN)
        {
            lifeExpController.AddEXP(0.001f); // concluiu o minijogo
            lifeExpController.AddEXP(0.0002f); // ganhou um item
            Debug.Log("Wrong answers count: " + wrongAnswer);
            WinImage.SetActive(true);

            if (!PlayerPreferences.M004_Memoria)
                WinText.text = "Parabéns, você ganhou a lente zoom para realizar a missão, mas ainda falta um item.";
            else
                WinText.text = "Parabéns, você ganhou a lente zoom. Agora você já tem os itens necessários para realizar a missão.";

            PlayerPreferences.M004_TeiaAlimentar = true;


            StartCoroutine(ReturnToShipCoroutine());
        }
    }

    public int ReturnCellNumber(string name)
    {
        switch(name)
        {
            case "bentosCell":
                return (int)Cells.bentosCell;
            case "avesmarinhasCell":
                return (int)Cells.avesMarinhasCell;
            case "pinguinsCell":
                return (int)Cells.pinguinsCell;
            case "baleiasCell":
                return (int)Cells.baleiasCell;
            case "krillCell":
                return (int)Cells.krillCell;
            case "cefalopodesCell":
                return (int)Cells.cefalopodesCell;
            case "focasCell":
                return (int)Cells.focasCell;
            case "peixesCell":
                return (int)Cells.peixesCell;
            case "protozoariosCell":
                return (int)Cells.protozariosCell;
            case "cetaceosCell":
                return (int)Cells.cetaceosCell;
            case "zooplanctonsCell":
                return (int)Cells.zooplanctonsCell;
            case "bacteriasCell":
                return (int)Cells.bacteriasCell;
            case "algasCell":
                return (int)Cells.algasCell;
            default:
                return -1;

        }   
    }

    public string ReturnCellInfo(string name)
    {
        switch (name)
        {
            case "bentosCell":
                return "serve de alimento para a célula " + (int)Cells.peixesCell;
            case "avesmarinhasCell":
                return "não serve de alimento para nenhuma célula";
            case "pinguinsCell":
                return "serve de alimento para as células " + (int)Cells.focasCell + " e " + (int)Cells.cetaceosCell;
            case "baleiasCell":
                return "não serve de alimento para nenhuma célula";
            case "krillCell":
                return "serve de alimento para as células as células " + (int)Cells.baleiasCell + " e " + (int)Cells.avesMarinhasCell + " e " + (int)Cells.pinguinsCell + " e " + (int)Cells.cefalopodesCell;
            case "cefalopodesCell":
                return "serve de alimento para as células " + (int)Cells.pinguinsCell + " e " + (int)Cells.cetaceosCell;
            case "focasCell":
                return "serve de alimento para a célula " + (int)Cells.cetaceosCell;
            case "peixesCell":
                return "serve de alimento para as células " + (int)Cells.avesMarinhasCell + " e " + (int)Cells.cetaceosCell;
            case "protozoariosCell":
                return "serve de alimento para as células " + (int)Cells.krillCell + " e " + (int)Cells.zooplanctonsCell + " e " + (int)Cells.bentosCell;
            case "cetaceosCell":
                return  "serve de alimento para a célula " + (int)Cells.bentosCell;
            case "zooplanctonsCell":
                return  "serve de alimento para a célula " + (int)Cells.bentosCell;
            case "bacteriasCell":
                return  "serve de alimento para as células " + (int)Cells.zooplanctonsCell + " e " + (int)Cells.protozariosCell;
            case "algasCell":
                return  "serve de alimento para as células " + (int)Cells.bacteriasCell + " e " + (int)Cells.bentosCell + " e " + (int)Cells.protozariosCell;
            default:
                return null;

        }
    }
    
    public bool CheckAnswer(string currentCellname, string currentItemname)
    {
        Debug.Log("cu cell name: " + currentCellname);
        Debug.Log("cu item name: " + currentItemname);
        Debug.Log(currentCellname.Equals(currentItemname + "Cell") ? true : false);
        return currentCellname.Equals(currentItemname + "Cell") ? true : false;
    }

    public IEnumerator ReturnToShipCoroutine()
    {
        yield return new WaitForSeconds(7f);

        SceneManager.LoadScene(ScenesNames.M004Ship);
    }
}

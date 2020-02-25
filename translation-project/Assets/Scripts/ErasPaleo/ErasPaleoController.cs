using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;

public class ErasPaleoController : DragAndDropController
{
    private readonly string instructions = "Início do jogo. Minijogo da teia alimentar. Descrição...";

    public AudioClip victoryClip;
    public AudioClip loseAudio;
    public AudioClip selectAnimal;

    public Button resetButton;

    public TMPro.TextMeshProUGUI attemptsText;
    private int attempts = 3;
    private int total = 0;

    public GameObject[] items;

    public Button confirmaButton;

    public GameObject LoseImage;

    // count the correct/wrong drops
    private int correctAnswer = 0;
    private int wrongAnswer = 0;
    private bool WIN = false;

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
    //public GameObject firstItem;

    enum Cells
    {
        bentosCell = 1, avesMarinhasCell, pinguinsCell, baleiasCell, krillCell,
        cefalopodesCell, focasCell, peixesCell, protozariosCell, cetaceosCell,
        zooplanctonsCell, bacteriasCell, algasCell
    };

    private void Start()
    {

        confirmaButton.interactable = false;

        draggedItems = new List<GameObject>();
        draggedCell = new List<GameObject>();
        draggedLocal = new List<GameObject>();
        remover = new List<GameObject>();

        passar_itens = new List<DragAndDropCell.DropEventDescriptor>();

        ReadText(instructions);

        audioSource = GetComponent<AudioSource>();

        // start afther time seconds and repeat at repeatRate rate
        //InvokeRepeating("CallHintMethod", dicas.time, dicas.repeatRate);
    }

    
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            //RemoveFirstItem();
            if (WinImage.activeSelf)
                WinImage.SetActive(false);
            else
                audioButton.Select();
        }

        if(Input.GetKeyDown(InputKeys.PARAMETERS_KEY))
        {
            lifeExpController.ReadHPandEXP();
            ReadText("Tentativas restantes: " + (attempts - wrongAnswer));
            Debug.Log("Tentativas restantes: " + (attempts - wrongAnswer));
        }

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

                ReadCell(nextCell);
            }
            else
            {
                nextCell.GetComponent<Selectable>().Select();

                ReadCell(nextCell);
            }
        }

        if (Input.GetKeyDown(KeyCode.Return))
            {
                if (!isPositioning)
                {
                    try
                    {
                        if (EventSystem.current.currentSelectedGameObject.GetComponentInChildren<DragAndDropItem>().gameObject.tag.Equals("item"))
                        {
                            OnButtonClick();
                            audioSource.PlayOneShot(selectAnimal);
                            isPositioning = true;
                            ReadCell(EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>().gameObject);
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.Log("Não é um item. Stacktrace >>" + e.StackTrace);
                    }
                }
                else
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

                        try
                        {
                            // go to item
                            items[FindNextItem()].GetComponent<Selectable>().Select();
                            ReadCell(items[FindNextItem()]);
                        }
                        catch (Exception ex)
                        {
                            Debug.Log("Não há mais itens. StackTrace >> " + ex.StackTrace);
                        }

                        audioSource.PlayOneShot(selectAnimal);

                        ResetConditions();

                        desc.item.ResetConditions();
                        
                        isPositioning = false;

                    }
                    catch (Exception e)
                    {
                        Debug.Log("Não é uma célula. Stacktrace >>" + e.StackTrace);
                    }
                }
            }

        if (Input.GetKeyDown(KeyCode.F5))
        {
            dicas.ShowHint();
        }

        if(Input.GetKeyDown(KeyCode.F6) && !isPositioning)
        {
            isCell = !isCell;

            if (isCell)
            {
                ReadText("Células");
                Debug.Log("Células");
                cells[FindNextEmptyCell()].GetComponent<Selectable>().Select();
                ReadCell(cells[FindNextEmptyCell()]);
            }
            else
            {
                if(total == 24)
                {
                    confirmaButton.interactable = true;
                    confirmaButton.GetComponent<Selectable>().Select();
                    ReadText("Confirmar escolhas.");
                    Debug.Log("Confirmar escolhas.");

                }
                else
                {
                ReadText("Itens");
                Debug.Log("Itens");
                items[FindNextItem()].GetComponent<Selectable>().Select();
                ReadCell(items[FindNextItem()]);
                }
            }

            /*if(total == 24)
            {
                confirmaButton.interactable = true;
                confirmaButton.GetComponent<Selectable>().Select();

                if(GetComponentInChildren<DragAndDropItem>().gameObject.name == "Confirmar")
                {
                    Debug.Log("Confirmar escolhas.");
                    ReadText("Confirmar escolhas.");               
			    }

            }*/
        }
    }

    public void CallHintMethod()
    {
        dicas.StartHints();
    }
    
    public void GoWrapper()
    {
        StartCoroutine(GoCoroutine());               
    }

    public IEnumerator GoCoroutine()
    {

        int contador = 0;

        int cont_erro = 0;

        foreach (GameObject g in draggedItems)
        {
            string nome1 = g.name + "_1";
            string nome2 = g.name + "_2";
            string nome3 = g.name + "_3";
            string nome4 = g.name + "_4";
            string nome5 = g.name + "_5";
            string nome6 = g.name + "_6";
            string nome7 = g.name + "_7";
            string nome8 = g.name + "_8";
            string nome9 = g.name + "_9";

            Debug.Log("cont: " + contador + " g.name: " + g.name + "Cell: " + draggedCell[contador].name);
            Debug.Log("Nome1: " +nome1 + "cell: " + g.name);

            if (nome1.Contains(draggedCell[contador].name) || nome2.Contains(draggedCell[contador].name) || nome3.Contains(draggedCell[contador].name)
            || nome4.Contains(draggedCell[contador].name) || nome5.Contains(draggedCell[contador].name) || nome6.Contains(draggedCell[contador].name)
            || nome7.Contains(draggedCell[contador].name) || nome8.Contains(draggedCell[contador].name) || nome9.Contains(draggedCell[contador].name))
            {
                Debug.Log("Acerto");
                correctAnswer++;
                Debug.Log("ACERTOS:" +correctAnswer);
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
                wrongAnswer = wrongAnswer + 1;
                attemptsText.text = "Tentativas restantes: " + wrongAnswer + "/" + attempts;
                confirmaButton.interactable = false;
                audioSource.PlayOneShot(wrongClip);
            }
        
        foreach (GameObject re in remover)
        {
            Debug.Log("antes: " + re);
            // remove o item que foi dropado
            draggedItems.Remove(re);

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

        if(correctAnswer == 24)
            {
                EndGame(true);
			}
        if(wrongAnswer >= 3)
            {
                EndGame(false);
			}
    }

    public void EndGame(bool win)
    {
        if (win)
        {
            PlayerPreferences.M009_Eras = true;

            audioSource.PlayOneShot(victoryClip);

            //yield return new WaitWhile(() => audioSource.isPlaying);

            WinImage.SetActive(true);
            //WinImage.GetComponentInChildren<Button>().Select();

            lifeExpController.AddEXP(PlayerPreferences.XPwinPuzzle); // finalizou o minijogo
            lifeExpController.AddEXP(3*PlayerPreferences.XPwinItem); // ganhou o item  

            dicas.dicas.SetActive(false);
            
        }
        else
        {
            LoseImage.SetActive(true);

            audioSource.PlayOneShot(loseAudio);

            //yield return new WaitWhile(() => audioSource.isPlaying);

            resetButton.Select();

            lifeExpController.AddEXP(PlayerPreferences.XPlosePuzzle); // jogou um minijogo

            dicas.dicas.SetActive(false);
            
        }

        StartCoroutine(ReturnToCampCoroutine()); // volta para o navio perdendo ou ganhando o minijogo
    }

    public override void OnSimpleDragAndDropEvent(DragAndDropCell.DropEventDescriptor desc)
    {
        // Get control unit of source cell
        ErasPaleoController sourceSheet = desc.sourceCell.GetComponentInParent<ErasPaleoController>();
        // Get control unit of destination cell
        ErasPaleoController destinationSheet = desc.destinationCell.GetComponentInParent<ErasPaleoController>();

        //desc.permission = false;

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

                    audioSource.PlayOneShot(wrongClip);
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

        if(!draggedItems.Contains(item) || draggedItems == null)
            {
                total++;
                Debug.Log("não tem: " +total);
                //break;
           }
        else
           {
                Debug.Log("já tem: " +total);
                //break;
           }

        Debug.Log("Guardo item: " + item.name);
        draggedItems.Add(item);

        Debug.Log("Guardo Cell: " + cell.GetComponentInChildren<DragAndDropItem>().name);
        draggedCell.Add(cell);

        Debug.Log("Guardo local: " + local.name);
        draggedLocal.Add(local); 

        if(total == 24)
        {
            confirmaButton.interactable = true;
            confirmaButton.GetComponent<Selectable>().Select();  
            Debug.Log("Confirmar escolhas.");
            ReadText("Confirmar escolhas.");               
        }

    }

    public int ReturnCellNumber(string name)
    {
        if(name.Contains("_1"))
             return 1;
        else if(name.Contains("_2"))
            return 2;
        else if(name.Contains("_3"))
            return 3;
        else if(name.Contains("_4"))
            return 4;
        else if(name.Contains("_5"))
            return 5;
        else if(name.Contains("_6"))
            return 6;
        else if(name.Contains("_7"))
            return 7;
        else if(name.Contains("_8"))
            return 8;
        else if(name.Contains("_9"))
            return 9;
        else 
            return -1; 
    }

    public string ReturnCellInfo(string name)
    {
        switch (name)
        {
            case "formacao_terraEra1":
                return "Formação da terra";
            case "AlgasEra1":
                return "Algas";
            case "rochas_antigasEra1":
                return "Rochas antigas";
            case "bacteriasEra1":
                return "Bactérias";
            case "pre_cambrianoEra1":
                return "Pré-Cambriano";
            case "protozoarioEra2":
                return "Protozoários";
            case "esponjasEra2":
                return "Esponjas";
            case "rochas_sedimentaresEra2":
                return "Rochas sedimentares";
            case "peixesEra2":
                return "Peixes";
            case "plantas_terrestresEra2":
                return  "Plantas terrestres";
            case "anfibiosEra2":
                return  "Anfíbios";
            case "insetosEra2":
                return  "Insetos";
            case "repteisEra2":
                return  "Répteis";
            case "paleozoicoEra2":
                return  "Paleozóico";
            case "mamiferos_pequenosEra3":
                return  "Mamíferos pequenos";
            case "sementesEra3":
                return  "Sementes";
            case "dinossaurosEra3":
                return  "Dinossauros";
            case "passarosEra3":
                return  "Pássaros";
            case "floresEra3":
                return  "Flores";
            case "himalaiaEra3":
                return  "Himalaia";
            case "mesozoicoEra3":
                return  "Mesozóico";
            case "mamiferosgigantesEra4":
                return  "Mamíferos gigantes";
            case "macacosEra4":
                return  "Macacos";
            case "alpesEra4":
                return  "Alpes";
            case "ceno_terEra4":
                return  "Cenozóico (Terciário)";
            case "idades_glaciaisEra5":
                return  "Idades glaciais";
            case "mamutesEra5":
                return  "Mamutes";
            case "humanosEra5":
                return  "Humanos";
            case "ceno_quaEra5":
                return  "Cenozóico (Quaternário)";
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

    public IEnumerator ReturnToCampCoroutine()
    {
        yield return new WaitForSeconds(4f);

        UnityEngine.SceneManagement.SceneManager.LoadScene(ScenesNames.M009Camp);
    }

    public void SelectFirstItem()
    {
        items[0].GetComponent<Selectable>().Select();

        ReadCell(items[0]);
    }

    public int FindNextItem()
    {
        int index = 0;

        while(items[index].GetComponentInChildren<DragAndDropItem>() == null)
        {
            Debug.Log("vazio");
            index++;
        }

        return index;
    }

    void ReadCell(GameObject nextCell)
    {
        if(nextCell.name.Contains("Era1"))
        {
            // navegando pelas celulas vazias
            if (nextCell.GetComponentInChildren<DragAndDropItem>() == null)
            {
                Debug.Log("Era 1, Célula " + ReturnCellNumber(nextCell.name) + " " + ReturnCellInfo(nextCell.name));
                ReadText("Era 1, Célula " + ReturnCellNumber(nextCell.name) + " " + ReturnCellInfo(nextCell.name));
            }
            else
            {
                // se tiver o component DragAndDropItem significa que o item foi dropado na celula
                Debug.Log("Era 1, Célula " + ReturnCellNumber(nextCell.name) + " preenchida com o item: "
                    + ReturnCellInfo(nextCell.GetComponentInChildren<DragAndDropItem>().gameObject.name));
                ReadText("Era 1, Célula " + ReturnCellNumber(nextCell.name) + " preenchida com o item: "
                    + ReturnCellInfo(nextCell.GetComponentInChildren<DragAndDropItem>().gameObject.name));
            }
        }

        else if(nextCell.name.Contains("Era2"))
        {
            // navegando pelas celulas vazias
            if (nextCell.GetComponentInChildren<DragAndDropItem>() == null)
            {
                Debug.Log("Era 2, Célula " + ReturnCellNumber(nextCell.name) + " " + ReturnCellInfo(nextCell.name));
                ReadText("Era 2, Célula " + ReturnCellNumber(nextCell.name) + " " + ReturnCellInfo(nextCell.name));
            }
            else
            {
                // se tiver o component DragAndDropItem significa que o item foi dropado na celula
                Debug.Log("Era 2, Célula " + ReturnCellNumber(nextCell.name) + " preenchida com o item: "
                    + ReturnCellInfo(nextCell.GetComponentInChildren<DragAndDropItem>().gameObject.name));
                ReadText("Era 2, Célula " + ReturnCellNumber(nextCell.name) + " preenchida com o item: "
                    + ReturnCellInfo(nextCell.GetComponentInChildren<DragAndDropItem>().gameObject.name));
            }
        }

        else if(nextCell.name.Contains("Era3"))
        {
            // navegando pelas celulas vazias
            if (nextCell.GetComponentInChildren<DragAndDropItem>() == null)
            {
                Debug.Log("Era 3, Célula " + ReturnCellNumber(nextCell.name) + " " + ReturnCellInfo(nextCell.name));
                ReadText("Era 3, Célula " + ReturnCellNumber(nextCell.name) + " " + ReturnCellInfo(nextCell.name));
            }
            else
            {
                // se tiver o component DragAndDropItem significa que o item foi dropado na celula
                Debug.Log("Era 3, Célula " + ReturnCellNumber(nextCell.name) + " preenchida com o item: "
                    + ReturnCellInfo(nextCell.GetComponentInChildren<DragAndDropItem>().gameObject.name));
                ReadText("Era 3, Célula " + ReturnCellNumber(nextCell.name) + " preenchida com o item: "
                    + ReturnCellInfo(nextCell.GetComponentInChildren<DragAndDropItem>().gameObject.name));
            }
        }

        else if(nextCell.name.Contains("Era4"))
        {
            // navegando pelas celulas vazias
            if (nextCell.GetComponentInChildren<DragAndDropItem>() == null)
            {
                Debug.Log("Era 4, Célula " + ReturnCellNumber(nextCell.name) + " " + ReturnCellInfo(nextCell.name));
                ReadText("Era 4, Célula " + ReturnCellNumber(nextCell.name) + " " + ReturnCellInfo(nextCell.name));
            }
            else
            {
                // se tiver o component DragAndDropItem significa que o item foi dropado na celula
                Debug.Log("Era 4, Célula " + ReturnCellNumber(nextCell.name) + " preenchida com o item: "
                    + ReturnCellInfo(nextCell.GetComponentInChildren<DragAndDropItem>().gameObject.name));
                ReadText("Era 4, Célula " + ReturnCellNumber(nextCell.name) + " preenchida com o item: "
                    + ReturnCellInfo(nextCell.GetComponentInChildren<DragAndDropItem>().gameObject.name));
            }
        }

        else if(nextCell.name.Contains("Era5"))
        {
            // navegando pelas celulas vazias
            if (nextCell.GetComponentInChildren<DragAndDropItem>() == null)
            {
                Debug.Log("Era 5, Célula " + ReturnCellNumber(nextCell.name) + " " + ReturnCellInfo(nextCell.name));
                ReadText("Era 5, Célula " + ReturnCellNumber(nextCell.name) + " " + ReturnCellInfo(nextCell.name));
            }
            else
            {
                // se tiver o component DragAndDropItem significa que o item foi dropado na celula
                Debug.Log("Era 5, Célula " + ReturnCellNumber(nextCell.name) + " preenchida com o item: "
                    + ReturnCellInfo(nextCell.GetComponentInChildren<DragAndDropItem>().gameObject.name));
                ReadText("Era 5, Célula " + ReturnCellNumber(nextCell.name) + " preenchida com o item: "
                    + ReturnCellInfo(nextCell.GetComponentInChildren<DragAndDropItem>().gameObject.name));
            }
        }

        else
        {
                
            if (nextCell.GetComponentInChildren<DragAndDropItem>() == null)
            {
                // caso que o jogador estara navegando com o item selecionado nas ceulas
                // entao é preciso ler a celula e seu numero para auxiliar o usuario a escolher o posicionamento correto
                if (nextCell.gameObject.name.Contains("Cell"))
                {
                    Debug.Log("Célula com o item: " + ReturnCellNumber(nextCell.name));
                    ReadText("Célula com o item: " + ReturnCellNumber(nextCell.name));
                }
                else
                {
                    if(nextCell.GetComponent<Button>() != null)
                    {
                        if(nextCell.GetComponent<Button>().name == "Confirmar")
                        {
                            Debug.Log("Confirmar escolhas.");
                            ReadText("Confirmar escolhas.");
                        }
			        }
                    else
                    {
                    Debug.Log("Célula vazia");
                    ReadText("Célula vazia");
                    }
                }
        }
            else // navegando por itens ainda com conteudo
            {
                Debug.Log("Célula com o item: " +ReturnCellInfo(nextCell.GetComponentInChildren<DragAndDropItem>().gameObject.name));
                ReadText("Célula com o item: " +ReturnCellInfo(nextCell.GetComponentInChildren<DragAndDropItem>().gameObject.name));
            }
        }
    }

    /*void ReadItem(GameObject nextCell)
    {
        // navegando por itens vazios
        if (nextCell.GetComponentInChildren<DragAndDropItem>() == null)
        {
            // caso que o jogador estara navegando com o item selecionado nas ceulas
            // entao é preciso ler a celula e seu numero para auxiliar o usuario a escolher o posicionamento correto
            if (nextCell.gameObject.name.Contains("Cell"))
            {
                Debug.Log("Célula com o item: " + ReturnCellNumber(nextCell.name));
                ReadText("Célula com o item: " + ReturnCellNumber(nextCell.name));
            }
            else
            {
                Debug.Log("Célula vazia");
                ReadText("Célula vazia");
            }
        }
        else // navegando por itens ainda com conteudo
        {
            Debug.Log("Célula com o item: " +ReturnCellInfo(nextCell.GetComponentInChildren<DragAndDropItem>().gameObject.name));
            ReadText("Célula com o item: " +ReturnCellInfo(nextCell.GetComponentInChildren<DragAndDropItem>().gameObject.name));
        }
    }*/
}

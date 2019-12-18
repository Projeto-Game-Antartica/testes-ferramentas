using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;

public class TeiaAlimentarController : DragAndDropController
{
    //private readonly string instructions = "Início do jogo. Minijogo da teia alimentar. Descrição...";

    // count the correct/wrong drops
    private int correctAnswer = 0;
    private int wrongAnswer = 0;
    private bool WIN = false;

    public bool started = false;
    
    public AudioClip correctClip;
    public AudioClip wrongClip;
    public AudioClip victoryClip;
    public AudioClip selectTeia;

    public LifeExpController lifeExpController;

    public GameObject WinImage;
    public TMPro.TextMeshProUGUI WinText;

    // itens da teia alimentar (parte de baixo)
    public GameObject[] items;

    enum Cells
    {
        bentosCell = 1, avesMarinhasCell, pinguinsCell, baleiasCell, krillCell,
        cefalopodesCell, focasCell, peixesCell, protozariosCell, cetaceosCell,
        zooplanctonsCell, bacteriasCell, algasCell
    };

    private void Start()
    {
        //ReadText(instructions);

        NRO_CELLS = 12;

        //Debug.Log(cells.Length);
        //foreach(GameObject g in cells)
        //{
        //    Debug.Log(g.name);
        //}

        audioSource = GetComponent<AudioSource>();
    }

    
    private void Update()
    {
        if(started)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (!isPositioning)
                {
                    try
                    {
                        if (EventSystem.current.currentSelectedGameObject.GetComponentInChildren<DragAndDropItem>().gameObject.tag.Equals("item"))
                        {
                            OnButtonClick();
                            audioSource.PlayOneShot(selectTeia);
                            isPositioning = true;
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
                            ReadItem(items[FindNextItem()]);
                        }
                        catch (Exception ex)
                        {
                            Debug.Log("Não há mais itens. StackTrace >> " + ex.StackTrace);
                        }

                        audioSource.PlayOneShot(selectTeia);

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

            if ((Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow) ||
                                        Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow)))
            {
                GameObject nextCell = EventSystem.current.currentSelectedGameObject.gameObject;

                if (currentItem != null)
                {
                    //Debug.Log(nextCell.name);
                    Debug.Log(DragAndDropItem.icon.name);
                    DragAndDropItem.icon.GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(nextCell.transform.position);
                }

                if (isCell)
                {
                    // navegando pelas celulas
                    nextCell.GetComponent<Selectable>().Select();

                    ReadCell(nextCell);
                }
                else
                {
                    // navegando pelos itens
                    nextCell.GetComponent<Selectable>().Select();

                    ReadItem(nextCell);
                }
            }

            if (Input.GetKeyDown(KeyCode.F6) && !isPositioning)
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
                    ReadText("Itens");
                    Debug.Log("Itens");
                    items[FindNextItem()].GetComponent<Selectable>().Select();
                    ReadItem(items[FindNextItem()]);
                }
            }
        }
        
    }

    public void SelectFirstItem()
    {
        items[0].GetComponent<Selectable>().Select();

        ReadItem(items[0]);
    }
    
    void ReadCell(GameObject nextCell)
    {
        // navegando pelas celulas vazias
        if (nextCell.GetComponentInChildren<DragAndDropItem>() == null)
        {
            Debug.Log("Célula " + ReturnCellNumber(nextCell.name) + " " + ReturnCellInfo(nextCell.name));
            ReadText("Célula " + ReturnCellNumber(nextCell.name) + " " + ReturnCellInfo(nextCell.name));
        }
        else
        {
            // se tiver o component DragAndDropItem significa que o item foi dropado na celula
            Debug.Log("Célula " + ReturnCellNumber(nextCell.name) + " preenchida com o item: "
                + nextCell.GetComponentInChildren<DragAndDropItem>().gameObject.name + " e " + ReturnCellInfo(nextCell.name));
            ReadText("Célula " + ReturnCellNumber(nextCell.name) + " preenchida com o item: "
                + nextCell.GetComponentInChildren<DragAndDropItem>().gameObject.name + " e " + ReturnCellInfo(nextCell.name));
        }
    }

    void ReadItem(GameObject nextCell)
    {
        // navegando por itens vazios
        if (nextCell.GetComponentInChildren<DragAndDropItem>() == null)
        {
            // caso que o jogador estara navegando com o item selecionado nas ceulas
            // entao é preciso ler a celula e seu numero para auxiliar o usuario a escolher o posicionamento correto
            if (nextCell.gameObject.name.Contains("Cell"))
            {
                Debug.Log("Célula " + ReturnCellNumber(nextCell.name));
                ReadText("Célula " + ReturnCellNumber(nextCell.name));
            }
            else
            {
                Debug.Log("Item vazio");
                ReadText("Item vazio");
            }
        }
        else // navegando por itens ainda com conteudo
        {
            Debug.Log(nextCell.GetComponentInChildren<DragAndDropItem>().gameObject.name);
            ReadText(nextCell.GetComponentInChildren<DragAndDropItem>().gameObject.name);
        }
    }

    // return the index of cell that has a item to be dropped.
    public int FindNextItem()
    {
        int index = 0;

        while(items[index].GetComponentInChildren<DragAndDropItem>() == null)
        {
            index++;
        }

        return index;
    }

    public override void OnSimpleDragAndDropEvent(DragAndDropCell.DropEventDescriptor desc)
    {
        // Get control unit of source cell
        TeiaAlimentarController sourceSheet = desc.sourceCell.GetComponentInParent<TeiaAlimentarController>();
        // Get control unit of destination cell
        TeiaAlimentarController destinationSheet = desc.destinationCell.GetComponentInParent<TeiaAlimentarController>();
        switch (desc.triggerType)                                               // What type event is?
        {
            case DragAndDropCell.TriggerType.DropRequest:                       // Request for item drag (note: do not destroy item on request)
                Debug.Log("Request " + desc.item.name + " from " + sourceSheet.name + " to " + destinationSheet.name);
                break;
            case DragAndDropCell.TriggerType.DropEventEnd:                      // Drop event completed (successful or not)
                if (desc.permission == true)                                    // If drop successful (was permitted before)
                {
                    Debug.Log("Successful drop " + desc.item.name + " from " + sourceSheet.name + " to " + destinationSheet.name);
                    //Debug.Log("ITEM: " + desc.item.name + ". DESTINATION CELL: " + desc.destinationCell.name);
                    audioSource.PlayOneShot(correctClip);
                    correctAnswer++;
                    if (correctAnswer >= NRO_CELLS) WIN = true;
                    StartCoroutine(CheckEndGame());
                }
                else                                                            // If drop unsuccessful (was denied before)
                {
                    Debug.Log("Denied drop " + desc.item.name + " from " + sourceSheet.name + " to " + destinationSheet.name);
                    audioSource.PlayOneShot(wrongClip);
                    wrongAnswer++;
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
    
    public IEnumerator CheckEndGame()
    {
        if (WIN)
        {
            lifeExpController.AddEXP(0.001f); // concluiu o minijogo
            lifeExpController.AddEXP(0.0002f); // ganhou um item
            Debug.Log("Wrong answers count: " + wrongAnswer);
            WinImage.SetActive(true);

            ReadText(ReadableTexts.instance.GetReadableText(ReadableTexts.key_m004_teia_vitoria, LocalizationManager.instance.GetLozalization()));


            if (!PlayerPreferences.M004_Memoria)
            {
                WinText.text = "Parabéns, você ganhou a lente zoom para realizar a missão, mas ainda falta um item.";

                audioSource.PlayOneShot(victoryClip);
                yield return new WaitWhile(() => audioSource.isPlaying);

                ReadText("Parabéns, você ganhou a lente zoom para realizar a missão, mas ainda falta um item.");
            }
            else
            {
                WinText.text = "Parabéns, você ganhou a lente zoom. Agora você já tem os itens necessários para realizar a missão.";

                audioSource.PlayOneShot(victoryClip);
                yield return new WaitWhile(() => audioSource.isPlaying);

                ReadText("Parabéns, você ganhou a lente zoom. Agora você já tem os itens necessários para realizar a missão.");

            }

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
                return "Krill: serve de alimento para as células as células " + (int)Cells.baleiasCell + " e " + (int)Cells.avesMarinhasCell + " e " + (int)Cells.pinguinsCell + " e " + (int)Cells.cefalopodesCell;
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
                return  "serve de alimento para as células " + (int)Cells.bentosCell + " e " + (int)Cells.peixesCell;
            case "bacteriasCell":
                return  "serve de alimento para as células " + (int)Cells.zooplanctonsCell + " e " + (int)Cells.protozariosCell;
            case "algasCell":
                return  "serve de alimento para as células " + (int)Cells.bacteriasCell + " e " + (int)Cells.bentosCell + " e " + (int)Cells.protozariosCell + " e " + (int)Cells.zooplanctonsCell;
            default:
                return null;

        }
    }
    
    public bool CheckAnswer(string currentCellname, string currentItemname)
    {
        return currentCellname.Equals(currentItemname + "Cell") ? true : false;
    }

    public IEnumerator ReturnToShipCoroutine()
    {
        yield return new WaitForSeconds(4f);

        SceneManager.LoadScene(ScenesNames.M004Ship);
    }
}

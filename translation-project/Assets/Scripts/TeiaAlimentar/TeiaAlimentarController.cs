using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;

public class TeiaAlimentarController : AbstractScreenReader
{

    // count the correct/wrong drops
    private int correctAnswer    = 0;
    private int wrongAnswer      = 0;

    // teia alimentar settings.
    public const int NRO_CELLS = 12;
    private bool WIN = false;
    public AudioClip correctClip;
    public AudioClip wrongClip;
    public AudioSource audioSource;

    // set on inspector
    public GameObject WinImage;
    public GameObject firstItem;
    public Button audioButton;

    // cells
    public GameObject[] cells;
    public static int cellIndex = 0;
    public bool selected = false;
    public bool isCell = false;
    public bool isPositioning = false;

    enum Cells
    {
        bentosCell = 1, avesMarinhasCell = 2, pinguinsCell = 3, baleiasCell = 4, krillCell = 5,
        cefalopodesCell = 6, focasCell = 7, peixesCell = 8, protozariosCell = 9, cetaceosCell = 10,
        zooplanctonsCell = 11, bacteriasCell = 12, algasCell = 13
    };


    // drag and drop via keyboard settings
    DragAndDropItem currentItem;
    DragAndDropCell currentCell;
    DragAndDropCell sourceCell;

    private void Start()
    {
        Debug.Log(cells.Length);
        foreach(GameObject g in cells)
        {
            Debug.Log(g.name);
        }

        audioSource = GetComponent<AudioSource>();
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

        if(Input.GetKeyDown(KeyCode.Space))
        {
            try
            {
                if (EventSystem.current.currentSelectedGameObject.GetComponentInChildren<DragAndDropItem>().gameObject.tag.Equals("item"))
                {
                    isPositioning = true;
                    OnButtonClick();
                }
            } catch (Exception e)
            {
                Debug.Log("Não é um item. Stacktrace >>" + e.StackTrace);
            }
        }

        if (currentItem != null && (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow) ||
                                    Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow)))
        {
            GameObject nextCell = EventSystem.current.currentSelectedGameObject.gameObject;
            DragAndDropItem.icon.GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(nextCell.transform.position);
            nextCell.GetComponent<Selectable>().Select();
            ReadText("Elemento ");
            ReadText(nextCell.name);
        }

        //if (Input.GetKeyDown(KeyCode.F1))
        //{
        //    if (currentItem != null)
        //    {
        //        GameObject nextCell = GetNextCell();
        //        DragAndDropItem.icon.GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(nextCell.transform.position);
        //        nextCell.GetComponent<Selectable>().Select();
        //    }
        //}

        //if (Input.GetKeyDown(KeyCode.F2))
        //{
        //    if (currentItem != null)
        //    {
        //        GameObject previousCell = GetPreviousCell();
        //        DragAndDropItem.icon.GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(previousCell.transform.position);
        //        previousCell.GetComponent<Selectable>().Select();
        //    }
        //}

        if (Input.GetKeyDown(KeyCode.Return))
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

        if(Input.GetKeyDown(KeyCode.F6) && !isPositioning)
        {
            if(isCell)
            {
                cells[0].GetComponent<Selectable>().Select();
            }
            else
            {
                firstItem.GetComponent<Selectable>().Select();
            }

            isCell = !isCell;
        }
    }
    
    public GameObject GetNextCell()
    {
        cellIndex++;
        if (cellIndex > NRO_CELLS) cellIndex = 0;
        return cells[cellIndex];
    }

    public GameObject GetPreviousCell()
    {
        cellIndex--;
        if (cellIndex < 0) cellIndex = NRO_CELLS;
        return cells[cellIndex];
    }

    void OnSimpleDragAndDropEvent(DragAndDropCell.DropEventDescriptor desc)
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
                    PlayAudioClip(correctClip);
                    correctAnswer++;
                    if (correctAnswer >= NRO_CELLS) WIN = true;
                    CheckEndGame();
                }
                else                                                            // If drop unsuccessful (was denied before)
                {
                    Debug.Log("Denied drop " + desc.item.name + " from " + sourceSheet.name + " to " + destinationSheet.name);
                    PlayAudioClip(wrongClip);
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

    /// <summary>
    /// Add item in first free cell
    /// </summary>
    /// <param name="item"> new item </param>
    public void AddItemInFreeCell(DragAndDropItem item)
    {
        foreach (DragAndDropCell cell in GetComponentsInChildren<DragAndDropCell>())
        {
            if (cell != null)
            {
                if (cell.GetItem() == null)
                {
                    cell.AddItem(Instantiate(item.gameObject).GetComponent<DragAndDropItem>());
                    break;
                }
            }
        }
    }

    /// <summary>
    /// Remove item from first not empty cell
    /// </summary>
    public void RemoveFirstItem()
    {
        foreach (DragAndDropCell cell in GetComponentsInChildren<DragAndDropCell>())
        {
            if (cell != null)
            {
                if (cell.GetItem() != null)
                {
                    cell.RemoveItem();
                    break;
                }
            }
        }
    }

    public void CheckEndGame()
    {
        if(WIN)
        {
            Debug.Log("Wrong answers count: " + wrongAnswer);
            WinImage.SetActive(true);
        }
    }

    public void PlayAudioClip(AudioClip audioClip)
    {
        if (!audioSource.isPlaying)
            audioSource.PlayOneShot(audioClip);
    }

    public void OnButtonClick()
    {
        sourceCell = EventSystem.current.currentSelectedGameObject.GetComponentInParent<DragAndDropCell>();
        currentItem = sourceCell.GetComponentInChildren<DragAndDropItem>();

        currentItem = currentItem.KeyboardDrag();

        cells[0].GetComponent<Selectable>().Select();
    }

    public void ResetConditions()
    {
        currentCell = null;
        currentItem = null;
        sourceCell = null;
    }
}

using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PinguimController : DragAndDropController {
    
    public GameObject pinguim_adelia;
    public GameObject pinguim_antartico;
    public GameObject pinguim_papua;

    private Animator pinguim_adeliaAnimator;
    private Animator pinguim_antarticoAnimator;
    private Animator pinguim_papuaAnimator;

    public Image fillImage;

    public static bool adeliaFinished;
    public static bool antarticoFinished;
    public static bool papuaFinished;
    
    private List<GameObject> draggedItems;

    public float verticalLength;
    public float horizontalLenght;

    private const int DOWN   = 0;
    private const int UP     = 1;
    private const int RIGHT  = 2;
    private const int LEFT   = 3;
    private const int RANDOM = 4;

    public GameObject WinImage;

    private void Start()
    {
        draggedItems = new List<GameObject>();

        adeliaFinished = false;
        antarticoFinished = false;
        papuaFinished = false;
        
        pinguim_adeliaAnimator = pinguim_adelia.GetComponent<Animator>();
        pinguim_antarticoAnimator = pinguim_antartico.GetComponent<Animator>();
        pinguim_papuaAnimator = pinguim_papua.GetComponent<Animator>();

        pinguim_adeliaAnimator.SetBool("isMoving", false);
        pinguim_antarticoAnimator.SetBool("isMoving", false);
        pinguim_papuaAnimator.SetBool("isMoving", false);

        fillImage.fillAmount = 1f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            try
            {
                if (EventSystem.current.currentSelectedGameObject.GetComponentInChildren<DragAndDropItem>().gameObject.tag.Equals("item"))
                {
                    OnButtonClick();
                }
            }
            catch (Exception e)
            {
                Debug.Log("Não é um item. Stacktrace >>" + e.StackTrace);
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
                //firstItem.GetComponent<Selectable>().Select();

                //ResetConditions();
            }
            catch (Exception e)
            {
                Debug.Log("Não é uma célula. Stacktrace >>" + e.StackTrace);
            }
        }

        if ((Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow) ||
                            Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow)))
        {
            GameObject nextCell = EventSystem.current.currentSelectedGameObject.gameObject;

            try
            {
                if (currentItem != null)
                {
                    DragAndDropItem.icon.GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(nextCell.transform.position);
                }

                if (isCell)
                {
                    nextCell.GetComponent<Selectable>().Select();
                }
                else
                {
                    nextCell.GetComponent<Selectable>().Select();
                    Debug.Log(nextCell.GetComponentInChildren<DragAndDropItem>().gameObject.name);
                    ReadText(nextCell.GetComponentInChildren<DragAndDropItem>().gameObject.name);
                }
            }
            catch (Exception e)
            {
                Debug.Log("null exception >> " + e.StackTrace);
            }
        }

        if (Input.GetKeyDown(KeyCode.F6))
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

        if (adeliaFinished && antarticoFinished && papuaFinished)
            WinImage.SetActive(true);
    }

    /// <summary>
    /// Operate all drag and drop requests and events from children cells
    /// </summary>
    /// <param name="desc"> request or event descriptor </param>
    public override void OnSimpleDragAndDropEvent(DragAndDropCell.DropEventDescriptor desc)
    {
        // Get control unit of source cell
        PinguimController sourceSheet = desc.sourceCell.GetComponentInParent<PinguimController>();
        // Get control unit of destination cell
        PinguimController destinationSheet = desc.destinationCell.GetComponentInParent<PinguimController>();
        switch (desc.triggerType)                                               // What type event is?
        {
            case DragAndDropCell.TriggerType.DropRequest:                       // Request for item drag (note: do not destroy item on request)
                Debug.Log("Request " + desc.item.name + " from " + sourceSheet.name + " to " + destinationSheet.name);
                break;
            case DragAndDropCell.TriggerType.DropEventEnd:                      // Drop event completed (successful or not)
                if (desc.permission == true)                                    // If drop successful (was permitted before)
                {
                    Debug.Log("Successful drop " + desc.item.name + " from " + sourceSheet.name + " to " + destinationSheet.name);
                    draggedItems.Add(desc.item.gameObject);
                }
                else                                                            // If drop unsuccessful (was denied before)
                {
                    Debug.Log("Denied drop " + desc.item.name + " from " + sourceSheet.name + " to " + destinationSheet.name);
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

    public void GoWrapper()
    {
        StartCoroutine(GoCoroutine());
    }
    
    public IEnumerator GoCoroutine()
    {
        foreach (GameObject g in draggedItems)
        {
            pinguim_adeliaAnimator.SetBool("isMoving", true);
            pinguim_antarticoAnimator.SetBool("isMoving", true);
            pinguim_papuaAnimator.SetBool("isMoving", true);

            switch(g.name)
            {
                case "down-item":
                    Debug.Log("Moving down");
                    goDown(); // the selected go in this direction
                    goRandomDirection(); // the others go in random direction
                    break;
                case "up-item":
                    Debug.Log("Moving up");
                    goUp(); // the selected go in this direction
                    goRandomDirection(); // the others go in random direction
                    break;
                case "right-item":
                    Debug.Log("Moving right");
                    goRight(); // the selected go in this direction
                    goRandomDirection(); // the others go in random direction
                    break;
                case "left-item":
                    Debug.Log("Moving left");
                    goLeft(); // the selected go in this direction
                    goRandomDirection(); // the others go in random direction
                    break;
                default:
                    Debug.Log("movimento nao reconhecido.");
                    break;
            }
            yield return new WaitForSeconds(1.2f);
        }

        RemoveAllItems();
        draggedItems.Clear();
        UpdateBackgroundState();

        pinguim_adeliaAnimator.SetBool("isMoving", false);
        pinguim_antarticoAnimator.SetBool("isMoving", false);
        pinguim_papuaAnimator.SetBool("isMoving", false);
    }

    public void goUp()
    {
        if (pinguim_adelia.transform.GetChild(0).gameObject.activeSelf && !adeliaFinished)
        {
            pinguim_adelia.transform.DOBlendableMoveBy(new Vector3(0, verticalLength), 1);
        }

        if (pinguim_papua.transform.GetChild(0).gameObject.activeSelf && !papuaFinished)
        {
            pinguim_papua.transform.DOBlendableMoveBy(new Vector3(0, verticalLength), 1);
        }

        if (pinguim_antartico.transform.GetChild(0).gameObject.activeSelf && !antarticoFinished)
        {
            pinguim_antartico.transform.DOBlendableMoveBy(new Vector3(0, verticalLength), 1);
        }
    }

    public void goDown()
    {
        if (pinguim_adelia.transform.GetChild(0).gameObject.activeSelf && !adeliaFinished)
        {
            pinguim_adelia.transform.DOBlendableMoveBy(new Vector3(0, -verticalLength), 1);
        }

        if (pinguim_papua.transform.GetChild(0).gameObject.activeSelf && !papuaFinished)
        {
            pinguim_papua.transform.DOBlendableMoveBy(new Vector3(0, -verticalLength), 1);
        }

        if (pinguim_antartico.transform.GetChild(0).gameObject.activeSelf && !antarticoFinished)
        {
            pinguim_antartico.transform.DOBlendableMoveBy(new Vector3(0, -verticalLength), 1);
        }
    }

    public void goRight()
    {
        if (pinguim_adelia.transform.GetChild(0).gameObject.activeSelf && !adeliaFinished)
        {
            FlipPinguim(pinguim_adelia.name, false);
            pinguim_adelia.transform.DOBlendableMoveBy(new Vector3(horizontalLenght, 0), 1);
        }

        if (pinguim_papua.transform.GetChild(0).gameObject.activeSelf && !papuaFinished)
        {
            FlipPinguim(pinguim_papua.name, false);
            pinguim_papua.transform.DOBlendableMoveBy(new Vector3(horizontalLenght, 0), 1);
        }

        if (pinguim_antartico.transform.GetChild(0).gameObject.activeSelf && !antarticoFinished)
        {
            FlipPinguim(pinguim_antartico.name, false);
            pinguim_antartico.transform.DOBlendableMoveBy(new Vector3(horizontalLenght, 0), 1);
        }
    }

    public void goLeft()
    {
        if (pinguim_adelia.transform.GetChild(0).gameObject.activeSelf && !adeliaFinished)
        {
            FlipPinguim(pinguim_adelia.name, true);
            pinguim_adelia.transform.DOBlendableMoveBy(new Vector3(-horizontalLenght, 0), 1);
        }

        if (pinguim_papua.transform.GetChild(0).gameObject.activeSelf && !papuaFinished)
        {
            FlipPinguim(pinguim_papua.name, true);
            pinguim_papua.transform.DOBlendableMoveBy(new Vector3(-horizontalLenght, 0), 1);
        }

        if (pinguim_antartico.transform.GetChild(0).gameObject.activeSelf && !antarticoFinished)
        {
            FlipPinguim(pinguim_antartico.name, true);
            pinguim_antartico.transform.DOBlendableMoveBy(new Vector3(-horizontalLenght, 0), 1);
        }
    }

    public void goRandomDirection()
    {
        int random = UnityEngine.Random.Range(1, 4);

        switch (random)
        {
            case 1: // go up
                if (!pinguim_adelia.transform.GetChild(0).gameObject.activeSelf && !adeliaFinished)
                {
                    pinguim_adelia.transform.DOBlendableMoveBy(new Vector3(0, verticalLength), 1);
                }

                if (!pinguim_papua.transform.GetChild(0).gameObject.activeSelf && !papuaFinished)
                {
                    pinguim_papua.transform.DOBlendableMoveBy(new Vector3(0, verticalLength), 1);
                }

                if (!pinguim_antartico.transform.GetChild(0).gameObject.activeSelf && !antarticoFinished)
                {
                    pinguim_antartico.transform.DOBlendableMoveBy(new Vector3(0, verticalLength), 1);
                }
                break;
            case 2: // go right
                if (!pinguim_adelia.transform.GetChild(0).gameObject.activeSelf && !adeliaFinished)
                {
                    FlipPinguim(pinguim_adelia.name, false);
                    pinguim_adelia.transform.DOBlendableMoveBy(new Vector3(horizontalLenght, 0), 1);
                }

                if (!pinguim_papua.transform.GetChild(0).gameObject.activeSelf && !papuaFinished)
                {
                    FlipPinguim(pinguim_papua.name, false);
                    pinguim_papua.transform.DOBlendableMoveBy(new Vector3(horizontalLenght, 0), 1);
                }

                if (!pinguim_antartico.transform.GetChild(0).gameObject.activeSelf && !antarticoFinished)
                {
                    FlipPinguim(pinguim_antartico.name, false);
                    pinguim_antartico.transform.DOBlendableMoveBy(new Vector3(horizontalLenght, 0), 1);
                }
                break;
            case 3: // go left
                if (!pinguim_adelia.transform.GetChild(0).gameObject.activeSelf && !adeliaFinished)
                {
                    FlipPinguim(pinguim_adelia.name, true);
                    pinguim_adelia.transform.DOBlendableMoveBy(new Vector3(-horizontalLenght, 0), 1);
                }

                if (!pinguim_papua.transform.GetChild(0).gameObject.activeSelf && !papuaFinished)
                {
                    FlipPinguim(pinguim_papua.name, true);
                    pinguim_papua.transform.DOBlendableMoveBy(new Vector3(-horizontalLenght, 0), 1);
                }

                if (!pinguim_antartico.transform.GetChild(0).gameObject.activeSelf && !antarticoFinished)
                {
                    FlipPinguim(pinguim_antartico.name, true);
                    pinguim_antartico.transform.DOBlendableMoveBy(new Vector3(-horizontalLenght, 0), 1);
                }
                break;
            case 4: // go down
                if (!pinguim_adelia.transform.GetChild(0).gameObject.activeSelf && !adeliaFinished)
                {
                    pinguim_adelia.transform.DOBlendableMoveBy(new Vector3(0, -verticalLength), 1);
                }

                if (!pinguim_papua.transform.GetChild(0).gameObject.activeSelf && !papuaFinished)
                {
                    pinguim_papua.transform.DOBlendableMoveBy(new Vector3(0, -verticalLength), 1);
                }

                if (!pinguim_antartico.transform.GetChild(0).gameObject.activeSelf && !antarticoFinished)
                {
                    pinguim_antartico.transform.DOBlendableMoveBy(new Vector3(0, -verticalLength), 1);
                }
                break;
        }
    }

    public void RemoveAllItems()
    {
        foreach(GameObject g in cells)
        {
            if(g.GetComponentInChildren<DragAndDropItem>() != null)
            {
                Destroy(g.transform.GetChild(0).gameObject);
                g.GetComponent<DragAndDropCell>().UpdateBackgroundState();
            }
        }
    }

    public void UpdateBackgroundState()
    {
        foreach (GameObject g in cells)
        {
            g.GetComponent<DragAndDropCell>().UpdateBackgroundState();
        }
    }

    public void SelectPinguin(string name)
    {
        switch (name)
        {
            case "adelia":
                pinguim_adelia.transform.GetChild(0).gameObject.SetActive(true);
                pinguim_antartico.transform.GetChild(0).gameObject.SetActive(false);
                pinguim_papua.transform.GetChild(0).gameObject.SetActive(false);
                break;
            case "antartico":
                pinguim_adelia.transform.GetChild(0).gameObject.SetActive(false);
                pinguim_antartico.transform.GetChild(0).gameObject.SetActive(true);
                pinguim_papua.transform.GetChild(0).gameObject.SetActive(false);
                break;
            case "papua":
                pinguim_adelia.transform.GetChild(0).gameObject.SetActive(false);
                pinguim_antartico.transform.GetChild(0).gameObject.SetActive(false);
                pinguim_papua.transform.GetChild(0).gameObject.SetActive(true);
                break;
            default:
                pinguim_adelia.transform.GetChild(0).gameObject.SetActive(false);
                pinguim_antartico.transform.GetChild(0).gameObject.SetActive(false);
                pinguim_papua.transform.GetChild(0).gameObject.SetActive(false);
                break;
        }
    }

    public void LoseHP()
    {
        if (fillImage.fillAmount < 0) fillImage.fillAmount = 0;
        fillImage.fillAmount -= 0.1f;
    }

    public void FlipPinguim(string name, bool left)
    {
        switch(name)
        {
            case "pinguim_adelia":
                pinguim_adelia.GetComponent<SpriteRenderer>().flipX = left;
                break;
            case "pinguim_antartico":
                pinguim_antartico.GetComponent<SpriteRenderer>().flipX = left;
                break;
            case "pinguim_papua":
                pinguim_papua.GetComponent<SpriteRenderer>().flipX = left;
                break;
        }
    }
}

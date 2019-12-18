using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class DragAndDropController : AbstractScreenReader
{
    // settings.
    public int NRO_CELLS;
    public AudioSource audioSource;
    
    // cells
    public GameObject[] cells;
    public static int cellIndex = 0;
    protected bool selected = false;
    protected bool isCell = false;
    protected bool isPositioning = false;

    // drag and drop via keyboard settings
    protected DragAndDropItem currentItem;
    protected DragAndDropCell currentCell;
    protected DragAndDropCell sourceCell;

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

    public abstract void OnSimpleDragAndDropEvent(DragAndDropCell.DropEventDescriptor desc);

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

    public void OnButtonClick()
    {
        sourceCell = EventSystem.current.currentSelectedGameObject.GetComponentInParent<DragAndDropCell>();
        currentItem = sourceCell.GetComponentInChildren<DragAndDropItem>();

        currentItem = currentItem.KeyboardDrag();

        cells[FindNextEmptyCell()].GetComponent<Selectable>().Select();
    }

    public void ResetConditions()
    {
        currentCell = null;
        currentItem = null;
        sourceCell = null;
    }

    public GameObject ReturnFirstCell()
    {
        return cells[0];
    }

    public GameObject ReturnNextEmptyCell()
    {
        return cells[FindNextEmptyCell()];
    }

    // return the index of cell that is empty.
    public int FindNextEmptyCell()
    {
        int index = 0;

        while (cells[index].GetComponentInChildren<DragAndDropItem>() != null)
        {
            index++;
        }

        return index;
    }
}

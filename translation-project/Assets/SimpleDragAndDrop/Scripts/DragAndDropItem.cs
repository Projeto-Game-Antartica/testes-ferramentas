using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// Drag and Drop item.
/// </summary>
public class DragAndDropItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	public static bool dragDisabled = false;										// Drag start global disable

	public static DragAndDropItem draggedItem;                                      // Item that is dragged now
	public static GameObject icon;                                                  // Icon of dragged item
    //public static GameObject iconChild;
	public static DragAndDropCell sourceCell;                                       // From this cell dragged item is

	public delegate void DragEvent(DragAndDropItem item);
	public static event DragEvent OnItemDragStartEvent;                             // Drag start event
	public static event DragEvent OnItemDragEndEvent;                               // Drag end event

	private static Canvas canvas;                                                   // Canvas for item drag operation
	private static string canvasName = "DragAndDropCanvas";                   		// Name of canvas
	private static int canvasSortOrder = 100;                                       // Sort order for canvas

    public TeiaAlimentarController teiaAlimentarController;
    public PinguimController pinguimController;
    
    /// <summary>
    /// Awake this instance.
    /// </summary>
    void Awake()
	{
		if (canvas == null)
		{
			GameObject canvasObj = new GameObject(canvasName);
			canvas = canvasObj.AddComponent<Canvas>();
			canvas.renderMode = RenderMode.ScreenSpaceOverlay;
			canvas.sortingOrder = canvasSortOrder;
		}
	}

    private void Update()
    {
        if (icon != null)
        {
            //float vertical = Input.GetAxis("Vertical") * speed;
            //float horizontal = Input.GetAxis("Horizontal") * rotationSpeed;

            //// Make it move 10 meters per second instead of 10 meters per frame...
            //vertical *= Time.deltaTime;
            //horizontal *= Time.deltaTime;

            //// Move translation along the object's z-axis
            //icon.transform.Translate(horizontal, vertical, 0);

            //if(Input.GetKeyDown(KeyCode.Space))
            //{
            //    icon.GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(teiaAlimentarController.GetNextCell().transform.position);
            //}

            if (Input.GetKeyDown(KeyCode.Return))
            {
                ResetConditions();
            }
        }
    }

    /// <summary>
    /// This item started to drag.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
	{
		if (dragDisabled == false)
		{
			sourceCell = GetCell();                       							// Remember source cell
			draggedItem = this;                                             		// Set as dragged item
			// Create item's icon
			icon = new GameObject();
			icon.transform.SetParent(canvas.transform);
			icon.name = "Icon";
			Image myImage = GetComponent<Image>();
			myImage.raycastTarget = false;                                        	// Disable icon's raycast for correct drop handling
			Image iconImage = icon.AddComponent<Image>();
			iconImage.raycastTarget = false;
			iconImage.sprite = myImage.sprite;
			RectTransform iconRect = icon.GetComponent<RectTransform>();
			// Set icon's dimensions
			RectTransform myRect = GetComponent<RectTransform>();
			iconRect.pivot = new Vector2(0.5f, 0.5f);
			iconRect.anchorMin = new Vector2(0.5f, 0.5f);
			iconRect.anchorMax = new Vector2(0.5f, 0.5f);
			iconRect.sizeDelta = new Vector2(myRect.rect.width, myRect.rect.height);

            // Create icon's child for text
            //iconChild = new GameObject();
            //iconChild.transform.SetParent(icon.transform);
            //iconChild.name = "IconChild";
            //TextMeshProUGUI myText = GetComponentInChildren<TextMeshProUGUI>();
            //myText.raycastTarget = false;
            //TextMeshProUGUI iconText = iconChild.AddComponent<TextMeshProUGUI>();
            //iconText.raycastTarget = false;
            //iconText.text = myText.text;
            //iconText.font = Resources.Load<TMP_FontAsset>("Swiss 721 SDF");
            //iconText.alignment = TextAlignmentOptions.Center;
            //iconText.fontStyle = FontStyles.Bold;
            //iconText.color = Color.black;
            //RectTransform iconChildRect = iconChild.GetComponent<RectTransform>();
            //iconChildRect.pivot = new Vector2(0.5f, 0.5f);
            //iconChildRect.anchorMin = new Vector2(0.5f, 0.5f);
            //iconChildRect.anchorMax = new Vector2(0.5f, 0.5f);
            //iconChildRect.sizeDelta = new Vector2(myRect.rect.width, myRect.rect.height);


            if (OnItemDragStartEvent != null)
			{
				OnItemDragStartEvent(this);                                			// Notify all items about drag start for raycast disabling
			}
		}
	}

	/// <summary>
	/// Every frame on this item drag.
	/// </summary>
	/// <param name="data"></param>
	public void OnDrag(PointerEventData data)
	{
		if (icon != null)
		{
			icon.transform.position = Input.mousePosition;                          // Item's icon follows to cursor in screen pixels
		}
	}

	/// <summary>
	/// This item is dropped.
	/// </summary>
	/// <param name="eventData"></param>
	public void OnEndDrag(PointerEventData eventData)
	{
		ResetConditions();
	}

	/// <summary>
	/// Resets all temporary conditions.
	/// </summary>
	public void ResetConditions()
	{
		if (icon != null)
		{
			Destroy(icon);                                                          // Destroy icon on item drop
		}
		if (OnItemDragEndEvent != null)
		{
			OnItemDragEndEvent(this);                                       		// Notify all cells about item drag end
		}
		draggedItem = null;
		icon = null;
		sourceCell = null;
	}

	/// <summary>
	/// Enable item's raycast.
	/// </summary>
	/// <param name="condition"> true - enable, false - disable </param>
	public void MakeRaycast(bool condition)
	{
		Image image = GetComponent<Image>();
		if (image != null)
		{
			image.raycastTarget = condition;
		}
	}

	/// <summary>
	/// Gets DaD cell which contains this item.
	/// </summary>
	/// <returns>The cell.</returns>
	public DragAndDropCell GetCell()
	{
		return GetComponentInParent<DragAndDropCell>();
	}

	/// <summary>
	/// Raises the disable event.
	/// </summary>
	void OnDisable()
	{
		ResetConditions();
	}

    public DragAndDropItem KeyboardDrag()
    {
        if (dragDisabled == false)
        {
            sourceCell = GetCell();                                                 // Remember source cell
            draggedItem = this;                                                     // Set as dragged item
                                                                                    // Create item's icon
            icon = new GameObject();
            icon.transform.SetParent(canvas.transform);
            icon.name = EventSystem.current.currentSelectedGameObject.name;
            Image myImage = GetComponent<Image>();
            myImage.raycastTarget = false;                                          // Disable icon's raycast for correct drop handling
            Image iconImage = icon.AddComponent<Image>();
            iconImage.raycastTarget = false;
            iconImage.sprite = myImage.sprite;
            RectTransform iconRect = icon.GetComponent<RectTransform>();
            // Set icon's dimensions
            RectTransform myRect = GetComponent<RectTransform>();
            iconRect.pivot = new Vector2(0.5f, 0.5f);
            iconRect.anchorMin = new Vector2(0.5f, 0.5f);
            iconRect.anchorMax = new Vector2(0.5f, 0.5f);
            iconRect.sizeDelta = new Vector2(myRect.rect.width, myRect.rect.height);

            //icon.transform.position = teiaAlimentarController.cells[0].transform.position;
            TeiaAlimentarController.cellIndex = 0;
            //icon.GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(teiaAlimentarController.cells[0].transform.position);

            if (teiaAlimentarController != null)
            {
                icon.GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(teiaAlimentarController.ReturnFirstCell().transform.position);
            }
            else if (pinguimController != null)
            {
                icon.GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(pinguimController.ReturnFirstCell().transform.position);
            }

            Debug.Log(icon.name);
            Debug.Log(draggedItem.name);
            if (OnItemDragStartEvent != null)
            {
                OnItemDragStartEvent(this);                                         // Notify all items about drag start for raycast disabling
            }
        }

        return draggedItem;
    }
}

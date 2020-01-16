using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PinguimCell : AbstractScreenReader, ISelectHandler
{
    public void OnSelect(BaseEventData eventData)
    {
        if (transform.childCount > 0)
        {
            Debug.Log(gameObject.name + " preenchida com a seta para " + GetComponentInChildren<DragAndDropItem>().name);
            ReadText(gameObject.name + " preenchida com a seta para " + GetComponentInChildren<DragAndDropItem>().name);
        }
        else
        {
            Debug.Log(gameObject.name);
            ReadText(gameObject.name);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TeiaShowImage : MonoBehaviour, ISelectHandler, IPointerEnterHandler, IDeselectHandler, IPointerExitHandler {

    GameObject tooltip;

    private void Start()
    {
        tooltip = new GameObject();

        tooltip = Instantiate(tooltip, transform, false);
        tooltip.AddComponent<Image>();
        tooltip.SetActive(false);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        // activate the tooltip gameobject
        tooltip.SetActive(true);
        
        // add image or sprite
        //tooltip.GetComponent<Image>().sprite = "";
    }

    public void OnSelect(BaseEventData eventData)
    {
        // activate the tooltip gameobject
        tooltip.SetActive(true);

        // add image or sprite
        //tooltip.GetComponent<Image>().sprite = "";
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.SetActive(false);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        tooltip.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

// attach this script to button
// change the text color when selected or clicked
// the sprite is changed via editor
public class ButtonTextColor : MonoBehaviour, ISelectHandler, IPointerEnterHandler, IPointerClickHandler, IDeselectHandler, IPointerExitHandler {
    
    private Color color;
    private Color normalColor;
    private TextMeshProUGUI text;
    
    // Use this for initialization
    void Awake ()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();

        normalColor = text.color;

        // dark blue
        color = new Color(0, 0.3607843f, 0.6235294f, 1);

        //Debug.Log(text.name);
	}

    public void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log("OnPointerClick");
        ChangeButtonColor(true);
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("OnPointerEnter");
        ChangeButtonColor(true);
    }

    public void OnSelect(BaseEventData eventData)
    {
        //Debug.Log("OnSelect");
        ChangeButtonColor(true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        //Debug.Log("OnDeselect");
        ChangeButtonColor(false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("OnPointerExit");
        ChangeButtonColor(false);
    }

    public void ChangeButtonColor(bool value)
    {
        if (Parameters.BUTTONCONTRAST)
        {
            //Debug.Log("ChangeButtonColor");
            if(value)
                text.color = color;
            else
                text.color = normalColor;
        }
    }
}

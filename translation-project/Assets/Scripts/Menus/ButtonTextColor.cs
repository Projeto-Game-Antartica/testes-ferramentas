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
    private TextMeshProUGUI text;
    public static bool ButtonContrast;
    
    // Use this for initialization
    void Awake () {
        text = GetComponentInChildren<TextMeshProUGUI>();

        // dark blue
        color = new Color(0, 0.3607843f, 0.6235294f, 1);
	}

    public void OnPointerClick(PointerEventData eventData)
    {
        if(!ButtonContrast) text.color = color;
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!ButtonContrast) text.color = color;
    }

    public void OnSelect(BaseEventData eventData)
    {
        if(!ButtonContrast) text.color = color;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if(!ButtonContrast) text.color = Color.white;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(!ButtonContrast) text.color = Color.white;
    }

    public void ChangeButtonColor(bool value)
    {
        if (!ButtonContrast)
        {
            if(value)
                text.color = color;
            else
                text.color = Color.white;
        }
    }
}

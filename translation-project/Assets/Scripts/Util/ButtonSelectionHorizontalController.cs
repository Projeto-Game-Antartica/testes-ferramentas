using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.ScrollSnaps;
using UnityEngine.EventSystems;


public class ButtonSelectionHorizontalController : MonoBehaviour
{
    [SerializeField]
    private float m_lerpTime;
    private DirectionalScrollSnap m_scrollRect;
    [SerializeField]
    private Button[] m_buttons;
    private int m_index;
    private float m_horizontalPosition;
    private bool m_left;
    private bool m_right;
    public Selectable[] m_selectables;

    public void Start()
    {

    if(m_buttons.Length  > 1)
    {
        m_scrollRect = GetComponent<DirectionalScrollSnap>();
        //m_buttons = GetComponentsInChildren<Button>();
        m_buttons[m_index].Select();
        m_horizontalPosition = ((float)m_index / (m_buttons.Length - 1));
    }
    else if(m_selectables.Length > 1)
    {
        m_scrollRect = GetComponent<DirectionalScrollSnap>();
        //m_selectables = GetComponentsInChildren<Button>();
        m_selectables[m_index].Select();
        m_horizontalPosition = ((float)m_index / (m_selectables.Length - 1));
	}
    }
    
    public void Update()
    {
        m_left = Input.GetKeyDown(KeyCode.LeftArrow);
        m_right = Input.GetKeyDown(KeyCode.RightArrow);

        //Debug.Log("TESTEEEE:" + EventSystem.current.currentSelectedGameObject.gameObject);

        if (m_left ^ m_right )
        {
            if(m_buttons.Length  > 1)
            {
                if (m_left)
                    m_index = Mathf.Clamp(m_index - 1, 0, m_buttons.Length - 1);
                else
                    m_index = Mathf.Clamp(m_index + 1, 0, m_buttons.Length - 1);

                m_buttons[m_index].Select();
                m_horizontalPosition = ((float)m_index / (m_buttons.Length - 1));
            }

            else if(m_selectables.Length  >  1 && EventSystem.current.currentSelectedGameObject.gameObject.name.Contains("Animal"))
            {
                if (m_left)
                    m_index = Mathf.Clamp(m_index - 1, 0, m_selectables.Length - 1);
                else
                    m_index = Mathf.Clamp(m_index + 1, 0, m_selectables.Length - 1);

                m_selectables[m_index].Select();
                m_horizontalPosition = ((float)m_index / (m_selectables.Length - 1));
			}
        }

        m_scrollRect.horizontalNormalizedPosition = Mathf.Lerp(m_scrollRect.horizontalNormalizedPosition, m_horizontalPosition, Time.deltaTime / m_lerpTime);
    }
}

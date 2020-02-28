using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonGroup : MonoBehaviour
{

    private Button[] buttons;

    private int selectedIndex;

    public int GetSelectedIndex() => selectedIndex;

    public void ClearSelection() {
        selectedIndex = -1;
        foreach(Button button in GetComponentsInChildren<Button>())
            SetButtonColor(button, Color.white);
    }

    private void selectButton(int buttonIndex) {
        ClearSelection();
        selectedIndex = buttonIndex;
        SetButtonColor(buttonIndex, Color.yellow);
    }

    private void onButtonClick(int i) {
        selectButton(i);
    }

    public void SetButtonColor(int buttonIndex, Color color) {
        SetButtonColor(buttons[buttonIndex], color); 
    }

    public void SetButtonColor(Button button, Color color) {
        button.GetComponent<Image>().color = color;
    }

    // Start is called before the first frame update
    void Start()
    {
        buttons = GetComponentsInChildren<Button>();

        for(int i = 0; i < buttons.Length; i++) {

            int temp = i;
            buttons[i].onClick.AddListener(()=>{onButtonClick(temp);});
        }

        ClearSelection();
    }

}

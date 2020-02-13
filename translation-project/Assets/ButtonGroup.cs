using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonGroup : MonoBehaviour
{

    private Button[] buttons;

    private int selectedIndex = -1;

    public int GetSelectedIndex() => selectedIndex;

    public void ClearSelection() {
        selectedIndex = -1;
        foreach(Button button in GetComponentsInChildren<Button>())
            button.GetComponent<Image>().color = Color.white;
    }

    private void onButtonClick(int i) {
        selectedIndex = i;
        updatedButtonsColor(i);
    }

    private void updatedButtonsColor(int buttonIndex) {
        for(int i = 0; i < buttons.Length; i++) {
            if(i == buttonIndex)
                buttons[i].GetComponent<Image>().color = Color.yellow;
            else
                buttons[i].GetComponent<Image>().color = Color.white;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        buttons = GetComponentsInChildren<Button>();

        for(int i = 0; i < buttons.Length; i++) {

            int temp = i;
            buttons[i].onClick.AddListener(()=>{onButtonClick(temp);});
        }
    }

    // Update is called once per frame
    // void Update()
    // {
    //     string debugStr = "";
    //     foreach (Button but in buttons)
    //         debugStr += but.IsPressed();
    //     Debug.Log(debugStr);
    // }
}

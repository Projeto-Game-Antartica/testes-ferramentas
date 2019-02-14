using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SlidingBlock : MonoBehaviour {

    public Button backButton;
    
    public void ReadButton(Button button)
    {
        TolkUtil.Speak(button.GetComponentInChildren<Text>().text);
    }

    public void ReadButtonTextMesh(Button button)
    {
        TolkUtil.Speak(button.GetComponentInChildren<TextMeshProUGUI>().text);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            backButton.Select();
        }
    }
}

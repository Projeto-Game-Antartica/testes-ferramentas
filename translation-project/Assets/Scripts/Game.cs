using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DavyKager;
using TMPro;

public class Game : MonoBehaviour {

    private TextMeshProUGUI textField;
    private InputField inputField;
    
	// Use this for initialization
	void Start () {

        textField  = GameObject.Find("ExampleText").GetComponent<TextMeshProUGUI>();
        inputField = GameObject.Find("InputField").GetComponent<InputField>();
        
        //TolkUtil.Load();

        //Tolk.Speak("Pressione Q para ouvir as instruções novamente." +
        //       "Pressione Control+E para a leitura do texto estático.");
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.E)) {
            ReadableTexts.ReadText(textField.text);
        }

        if(Input.GetKeyDown(KeyCode.Q))
        {
            ReadableTexts.ReadText("Pressione Q para ouvir as instruções novamente." +
                "Pressione Control+E para a leitura do texto estático.");
        }
	}

    public void OnButtonPress()
    {
        Tolk.Speak(inputField.text);
    }
}

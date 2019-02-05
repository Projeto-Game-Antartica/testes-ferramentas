using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContentPanelController : MonoBehaviour {

    private const string instructions = "Painel de catalogo das baleias. Neste painel aparece a foto da baleia que" +
        "foi tirada agora, a data e hora, a organização e a sua localização com latitude, longitude e também um mapa." +
        "Há dois botões ao final, um para catalogar esta foto e outra para descartá-la, caso deseje tirar outra.";

    public Button firstButton;
    
	// Update is called once per frame
	void Update () {

		if(Input.GetKeyDown(KeyCode.F2))
        {
            SpeakInstructions();
        }
	}

    public static void SpeakInstructions()
    {
        TolkUtil.Speak(instructions);
    }
}

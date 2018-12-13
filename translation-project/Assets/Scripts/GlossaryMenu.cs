using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;

public class GlossaryMenu : MonoBehaviour {

    private Button ptlibrasButton;
    private Button enButton;
    private Button soundButton;
    private Button backButton;
    private EventSystem system;

    private const string glossaryText = "Glossário. Utilize as setas cima ou baixo ou a tecla TAB para navegação" +
                                        "e a tecla enter para selecionar os itens.";

    // Use this for initialization
    void Start () {
        ptlibrasButton = GameObject.Find("PtbrLibrasButton").GetComponent<Button>();
        enButton = GameObject.Find("EnButton").GetComponent<Button>();
        soundButton = GameObject.Find("SoundButton").GetComponent<Button>();
        backButton = GameObject.Find("BackButton").GetComponent<Button>();

        system = EventSystem.current;

        TolkUtil.Instructions();
        TolkUtil.Speak(glossaryText);

        //TolkUtil.Load();

        ptlibrasButton.Select();
    }
	
    
    public void LoadGlossary()
    {
        SceneManager.LoadScene("GlossaryScene");
    }


    public void ButtonPtLibrasAudio()
    {
        TolkUtil.Speak(ptlibrasButton.GetComponentInChildren<TextMeshProUGUI>().text);
    }

    public void ButtonEnAudio()
    {
        TolkUtil.Speak(enButton.GetComponentInChildren<TextMeshProUGUI>().text);
    }

    public void ButtonSoundAudio()
    {
        TolkUtil.Speak(soundButton.GetComponentInChildren<TextMeshProUGUI>().text);
    }

    public void ButtonBackAudio()
    {
        TolkUtil.Speak(backButton.GetComponentInChildren<TextMeshProUGUI>().text);
    }

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            TolkUtil.Speak(glossaryText);
        }

        // Navegação dos itens selecionáveis através do TAB
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Selectable next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();

            if (next != null)
            {

                InputField inputfield = next.GetComponent<InputField>();
                if (inputfield != null) inputfield.OnPointerClick(new PointerEventData(system));  //if it's an input field, also set the text caret

                system.SetSelectedGameObject(next.gameObject, new BaseEventData(system));
            }
            else //Here is the navigating back part
            {
                next = Selectable.allSelectables[0];
                system.SetSelectedGameObject(next.gameObject, new BaseEventData(system));
            }

        }
    }
}

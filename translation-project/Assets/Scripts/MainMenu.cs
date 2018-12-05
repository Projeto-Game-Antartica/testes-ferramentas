using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using DavyKager;
using TMPro;

public class MainMenu : MonoBehaviour
{
    private Button playButton;
    private Button quitButton;
    private Button optionButton;
    private EventSystem system;

    private const string menuText = "Menu principal do jogo. Utilize as setas cima ou baixo ou a tecla TAB para navegação" +
                                    "a tecla enter para selecionar os itens.";

    void Start()
    {
        playButton = GameObject.Find("PlayButton").GetComponent<Button>();
        quitButton = GameObject.Find("QuitButton").GetComponent<Button>();
        optionButton = GameObject.Find("OptionButton").GetComponent<Button>();
        system = EventSystem.current;

        TolkUtil.Load();
        Debug.Log("Tolk loaded");

        TolkUtil.Instructions();
        TolkUtil.Speak(menuText);

        playButton.Select();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("QuizScene");
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        TolkUtil.Unload();
        Application.Quit();
    }

    public void ButtonPlayAudio()
    {
        Debug.Log("PlayButton");
        TolkUtil.Speak(playButton.GetComponentInChildren<TextMeshProUGUI>().text);
    }

    public void ButtonOptionAudio()
    {
        Debug.Log("OptionButton");
        TolkUtil.Speak(optionButton.GetComponentInChildren<TextMeshProUGUI>().text);
    }

    public void ButtonQuitAudio()
    {
        Debug.Log("QuitButton");
        TolkUtil.Speak(quitButton.GetComponentInChildren<TextMeshProUGUI>().text);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1))
        {
            TolkUtil.Speak(menuText);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GlossaryMenu : MonoBehaviour {

    private Button glossaryButton;
    private Button soundButton;
    private Button backButton;

    private const string glossaryText = "Glossário. Utilize as setas cima ou baixo ou a tecla TAB para navegação" +
                                        "e a tecla enter para selecionar os itens.";

    // Use this for initialization
    void Start () {
        glossaryButton  = GameObject.Find("glossaryButton").GetComponent<Button>();
        soundButton     = GameObject.Find("SoundButton").GetComponent<Button>();
        backButton      = GameObject.Find("BackButton").GetComponent<Button>();
        
        TolkUtil.Instructions();
        TolkUtil.Speak(glossaryText);

        //TolkUtil.Load();

        glossaryButton.Select();
    }
    
    public void LoadGlossary()
    {
        SceneManager.LoadScene("GlossaryScene");
    }

    public void LoadSoundGlossary()
    {
        SceneManager.LoadScene("GlossarySoundScene");
    }

    public void ButtonPtLibrasAudio()
    {
        TolkUtil.Speak(glossaryButton.GetComponentInChildren<TextMeshProUGUI>().text);
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
    }
}

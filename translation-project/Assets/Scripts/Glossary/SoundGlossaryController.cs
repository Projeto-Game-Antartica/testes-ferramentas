using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class SoundGlossaryController : MonoBehaviour {


    // nome do arquivo JSON
    private const string dataFilename = "glossary.json";
    private const string dictionaryText = "";

    // Classe C# para mapear o JSON
    DataArray loadedData;

    public Transform contentPanel;
    public SimpleObjectPool buttonObjectPool;
    private List<SoundButton> buttonList;

    // lista contendo as palavras em portugues
    private List<string> keys_ptbr;

    // list contendo as palavras em inglês
    private List<string> keys_en;

    // hashmap contendo a palavra em portugues que o leva ao seu audio
    private Dictionary<string, string> audio_ptbr;

    // hashmap com keys em ingles
    private Dictionary<string, string> audio_en;

    private AudioSource audioSource;

    void Start()
    {
        LoadDictionary();
        Button button = GameObject.Find("ButtonA").GetComponent<Button>();
        TolkUtil.Speak(dictionaryText);
        button.Select();
    }

    public void LoadDictionary()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, dataFilename);
        
        keys_ptbr   = new List<string>();
        keys_en     = new List<string>();
        buttonList  = new List<SoundButton>();
        audio_ptbr  = new Dictionary<string, string>();
        audio_en    = new Dictionary<string, string>();

        if (File.Exists(filePath))
        {
            // leitura do JSON
            string dataAsJson = File.ReadAllText(filePath);
            loadedData = JsonUtility.FromJson<DataArray>(dataAsJson);

            if (LocalizationManager.instance.GetLozalization().Equals("locales_ptbr.json"))
            {
                // carga das listas / dicionários com o conteúdo do JSON em portugues
                for (int i = 0; i < loadedData.items.Length; i++)
                {
                    keys_ptbr.Add(loadedData.items[i].key_ptbr);
                    audio_ptbr.Add(loadedData.items[i].key_ptbr, loadedData.items[i].audio_path);
                }
                AddButton(keys_ptbr);
        }
        else
        {
            // carga das listas/dicionários com o conteúdo do JSON em ingles
            for (int i = 0; i < loadedData.items.Length; i++)
            {
                keys_en.Add(loadedData.items[i].key_en);
                audio_en.Add(loadedData.items[i].key_en, loadedData.items[i].audio_path);
            }
            AddButton(keys_en);
        }
    }
        else
        {
            Debug.LogError("Cannot find file!");
        }
    }

    public void AddButton(List<string> list)
    {
        list.Sort();

        foreach (string key in list)
        {
            GameObject newButton = buttonObjectPool.GetObject();
            newButton.name = key + "Button";
            newButton.transform.SetParent(contentPanel);
            Debug.Log(key + " " + newButton);

            SoundButton soundButton = newButton.GetComponent<SoundButton>();
            soundButton.keyLabel.text = key;

            buttonList.Add(soundButton);
        }
    }

    public void ShowAllButtons()
    {
        foreach (SoundButton b in buttonList)
        {
            b.gameObject.SetActive(true);
        }

        buttonList[0].buttonComponent.Select();
    }

    public void ShowButtonStartingWithLetter(string letter)
    {
        bool first = true;
        int index = 0;

        foreach (SoundButton b in buttonList)
        {
            if (b.keyLabel.text.ToLower().StartsWith(letter))
            {
                if (first) index = buttonList.IndexOf(b);
                b.buttonComponent.gameObject.SetActive(true);
                first = false;
            }
            else
            {
                b.buttonComponent.gameObject.SetActive(false);
            }
        }

        buttonList[index].buttonComponent.Select();
    }

    public void RemoveAllButtons()
    {
        foreach (SoundButton b in buttonList)
        {
            b.buttonComponent.gameObject.SetActive(false);
        }
    }

    public void ReadLetterButtin(string letter)
    {
        TolkUtil.Speak(letter);
    }

    public AudioClip GetAudioClip(string key, string localization)
    {
        AudioClip audioClip;

        if(localization.Equals("locales_ptbr.json"))
            audioClip = Resources.Load<AudioClip>(audio_ptbr[key]);
        else
            audioClip = Resources.Load<AudioClip>(audio_en[key]);

        return audioClip;
    }

    public void BackButton()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void Update()
    {
    }
}

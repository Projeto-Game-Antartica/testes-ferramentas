﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System;

public class SoundGlossaryController : AbstractScreenReader {

    /*
     * nome do arquivo JSON
     */
    //private const string dataFilename = "glossary.json";
    private const string dataFilename = "soundglossary.json";

    /*
     * Classe C# para mapear o JSON
     */
    DataArray loadedData;

    /*
    * Variáveis para controle dos botões e sua navegação
    */
    [SerializeField]
    private float m_lerpTime;
    private List<Button> m_buttons;
    private int m_index = 0;
    private float m_verticalPosition;
    private bool m_up;
    private bool m_down;
    private List<SoundButton> buttonList;
    private Button buttonA;
    public ScrollRect m_scrollRect;
    public Transform contentPanel;
    public SimpleObjectPool buttonObjectPool;
    public Button backButton;

    /*
    * Variáveis para controle do glossário
    */
    private List<string> keys_ptbr; // lista contendo as palavras em portugues
    private List<string> keys_en; // list contendo as palavras em inglês
    private Dictionary<string, string> audio_ptbr; // hashmap contendo a palavra em portugues que o leva ao seu audio
    private Dictionary<string, string> audio_en; // hashmap com keys em ingles

    private ReadableTexts readableTexts;

    void Start()
    {
        LoadDictionary();
        readableTexts = GameObject.Find("ReadableTexts").GetComponent<ReadableTexts>();
        buttonA = GameObject.Find("ButtonA").GetComponent<Button>();
        ReadText(readableTexts.GetReadableText(ReadableTexts.key_soundglossary_instructions, LocalizationManager.instance.GetLozalization()));
        m_buttons[m_index].Select();
        m_verticalPosition = 1f - ((float)m_index / (m_buttons.Count - 1));
    }

    public void Update()
    {
        // scrollbar follows the selected button (accessibility ?)
        //if (SoundButton.contentButton)
        //{
        //    m_up = Input.GetKeyDown(KeyCode.UpArrow);
        //    m_down = Input.GetKeyDown(KeyCode.DownArrow);

        //    if (m_up ^ m_down)
        //    {
        //        if (m_up)
        //            m_index = Mathf.Clamp(m_index - 1, 0, m_buttons.Count - 1);
        //        else
        //            m_index = Mathf.Clamp(m_index + 1, 0, m_buttons.Count - 1);

        //        m_buttons[m_index].Select();
        //        m_verticalPosition = 1f - ((float)m_index / (m_buttons.Count - 1));
        //    }

        //    m_scrollRect.verticalNormalizedPosition = Mathf.Lerp(m_scrollRect.verticalNormalizedPosition,
        //    m_verticalPosition, Time.deltaTime / m_lerpTime);
        //}

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (SoundButton.IsAudioPlaying())
                SoundButton.audioSource.Stop();
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            ReadText(readableTexts.GetReadableText(ReadableTexts.key_soundglossary_instructions, LocalizationManager.instance.GetLozalization()));
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            ResetVerticalPositionScrollRect();
            buttonA.Select();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            backButton.Select();
        }
    }

    public void LoadDictionary()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, dataFilename);
        
        keys_ptbr   = new List<string>();
        keys_en     = new List<string>();
        buttonList  = new List<SoundButton>();
        audio_ptbr  = new Dictionary<string, string>();
        audio_en    = new Dictionary<string, string>();
        m_buttons   = new List<Button>();

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
            m_buttons.Add(soundButton.buttonComponent);
        }
    }

    public void ShowAllButtons()
    {
        ResetVerticalPositionScrollRect();
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
        ReadText(letter);
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

    public void ResetVerticalPositionScrollRect()
    {
        m_verticalPosition = 1f;
        m_index = 0;
    }
}

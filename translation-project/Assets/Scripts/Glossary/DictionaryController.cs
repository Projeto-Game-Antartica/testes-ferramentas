﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Video;
using UnityEngine.EventSystems;

public class DictionaryController : AbstractScreenReader {
    
    /*
     * nome do arquivo JSON
     */
    private const string dataFilename = "glossary.json";

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
    private int m_index;
    private float m_verticalPosition;
    private bool m_up;
    private bool m_down;
    private List<DictionaryButton> buttonList;
    private Button buttonA;
    public AudioSource contentAudioSource;
    public ScrollRect m_scrollRect;
    public Transform contentPanel;
    public SimpleObjectPool buttonObjectPool;
    public DescriptionContent descriptionContent;
    public Button audioButton;
    public Button backButton;

    /*
     * Variáveis para controle do glossário
     */
    private List<string> keys_ptbr; // lista contendo as palavras em portugues
    private List<string> keys_en; // list contendo as palavras em inglês
    private Dictionary<string, string> description_ptbr; // hashmap contendo a palavra em portugues que o leva a sua descrição em portugues   
    private Dictionary<string, string> description_ptbrimage; // hashmap contendo a palavra que o leva para a imagem
    private Dictionary<string, string> description_ptbrvideo; // hashmap contendo a palavra que o leva para o video em libras
    private Dictionary<string, string> description_en; // idem itens anteriores, porem para ingles sem video
    private Dictionary<string, string> description_enimage;
    private Dictionary<string, string> audio_ptbr; // hashmap contendo a palavra em portugues que o leva ao seu audio
    private Dictionary<string, string> audio_en; // hashmap com keys em ingles

    void Start()
    {
        LoadDictionary();
        buttonA = GameObject.Find("ButtonA").GetComponent<Button>();
        ReadText(ReadableTexts.instance.GetReadableText(ReadableTexts.key_glossary_instructions, LocalizationManager.instance.GetLozalization()));
        m_buttons[m_index].Select();
        m_verticalPosition = 1f - ((float)m_index / (m_buttons.Count - 1));
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            contentAudioSource.Stop();
        }

        //if (DictionaryButton.contentButton)
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

        //    m_scrollRect.verticalNormalizedPosition = Mathf.Lerp(m_scrollRect.verticalNormalizedPosition, m_verticalPosition, Time.deltaTime / m_lerpTime);
        //}

        if (Input.GetKeyDown(KeyCode.F1))
        {
            ReadContentText(ReadableTexts.instance.GetReadableText(ReadableTexts.key_glossary_instructions, LocalizationManager.instance.GetLozalization()));
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            ResetVerticalPositionScrollRect();
            buttonA.Select();
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            ReadContentText(descriptionContent.descriptionText.text);
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            backButton.Select();
        }
    }

    public void LoadDictionary()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, dataFilename);

        buttonList = new List<DictionaryButton>();

        keys_ptbr = new List<string>();
        keys_en = new List<string>();

        m_buttons = new List<Button>();

        // pt-br information
        description_ptbr = new Dictionary<string, string>();
        description_ptbrimage = new Dictionary<string, string>();
        description_ptbrvideo = new Dictionary<string, string>();
        audio_ptbr = new Dictionary<string, string>();

        // en information
        description_en = new Dictionary<string, string>();
        description_enimage = new Dictionary<string, string>();
        audio_en = new Dictionary<string, string>();

        if (File.Exists(filePath))
        {
            // leitura do JSON
            string dataAsJson = File.ReadAllText(filePath);
            loadedData = JsonUtility.FromJson<DataArray>(dataAsJson);

            if (LocalizationManager.instance.GetLozalization().Equals("locales_ptbr.json"))
            {
                // carga das listas/dicionários com o conteúdo do JSON em portugues
                for (int i = 0; i < loadedData.items.Length; i++)
                {
                    keys_ptbr.Add(loadedData.items[i].key_ptbr);
                    description_ptbr.Add(loadedData.items[i].key_ptbr, loadedData.items[i].description_ptbr);
                    description_ptbrimage.Add(loadedData.items[i].key_ptbr, loadedData.items[i].image_path);
                    description_ptbrvideo.Add(loadedData.items[i].key_ptbr, loadedData.items[i].video_path);
                    audio_ptbr.Add(loadedData.items[i].key_ptbr, loadedData.items[i].audio_path);
                }
                AddButtons(keys_ptbr);
        }
        else
        {
            // carga das listas/dicionários com o conteúdo do JSON em ingles
            for (int i = 0; i < loadedData.items.Length; i++)
            {
                keys_en.Add(loadedData.items[i].key_en);
                description_en.Add(loadedData.items[i].key_en, loadedData.items[i].description_en);
                description_enimage.Add(loadedData.items[i].key_en, loadedData.items[i].image_path);
                audio_en.Add(loadedData.items[i].key_en, loadedData.items[i].audio_path);
            }
            AddButtons(keys_en);
        }
    }
        else
        {
            Debug.LogError("Cannot find file!");
        }
    }

    public void AddButtons(List<string> list)
    {
        list.Sort();

        foreach (string key in list)
        {
            // create a new button
            GameObject newButton = buttonObjectPool.GetObject();
            newButton.name = key + "Button";
            newButton.transform.SetParent(contentPanel);
            Debug.Log(key + " " + newButton);

            DictionaryButton dictionaryButton = newButton.GetComponent<DictionaryButton>();
            dictionaryButton.keyLabel.text = key;

            buttonList.Add(dictionaryButton);
            m_buttons.Add(dictionaryButton.buttonComponent);
        }

        // set bool value to first and last item for audiodescription
        SetFirstElement(buttonList.First());
        SetLastElement(buttonList.Last());
    }

    public void ShowAllButtons()
    {
        RemoveDescriptionComponent();
        ResetVerticalPositionScrollRect();
        ResetFirstLastElements(buttonList);

        foreach (DictionaryButton b in buttonList)
        {
            b.gameObject.SetActive(true);
        }


        // set bool value to first and last item for audiodescription
        SetFirstElement(buttonList.First());
        SetLastElement(buttonList.Last());

        buttonList[0].buttonComponent.Select();
    }

    public void ShowButtonStartingWithLetter(string letter)
    {
        RemoveDescriptionComponent();
        ResetVerticalPositionScrollRect();
        ResetFirstLastElements(buttonList);

        List<DictionaryButton> list = new List<DictionaryButton>();
        
        foreach(DictionaryButton b in buttonList)
        {
            if (b.keyLabel.text.ToLower().StartsWith(letter))
            {
                list.Add(b);
                b.buttonComponent.gameObject.SetActive(true);
            }
            else
            {
                b.buttonComponent.gameObject.SetActive(false);
            }
        }

        SetFirstElement(list.First());
        SetLastElement(list.Last());
        list.First().buttonComponent.Select();
    }

    public void ShowDescriptionContent(string key)
    {
        Sprite image;
        VideoClip videoClip;

        RemoveAllButtons();

        descriptionContent.gameObject.SetActive(true);

        Debug.Log(key);

        // descricao em portugues
        if (LocalizationManager.instance.GetLozalization().Equals("locales_ptbr.json"))
        {
            image = Resources.Load<Sprite>(description_ptbrimage[key]);

            // video from url
            if (description_ptbrvideo[key].Contains("http"))
            {
                string url = description_ptbrvideo[key];
                Debug.Log(url);
                StartCoroutine(PlayVideo(descriptionContent.videoPlayer, true, url));
            }
            else // video from resources
            {
                videoClip = Resources.Load<VideoClip>(description_ptbrvideo[key]);
                descriptionContent.videoPlayer.clip = videoClip;
                StartCoroutine(PlayVideo(descriptionContent.videoPlayer, false, ""));
            }
            
        }
        else // descricao em ingles (nao ha videoplayer)
        {
            image = Resources.Load<Sprite>(description_enimage[key]);
            descriptionContent.videoPlayer.gameObject.SetActive(false);
        }

        descriptionContent.PlayAudioText.text = key + ": Ouvir som";
        descriptionContent.descriptionText.text = GetTextDescription(key, LocalizationManager.instance.GetLozalization());
        descriptionContent.image.sprite = image;

        // setting the key for audio button
        DictionaryButton.keyAudio = key;

        audioButton.Select();

        ReadContentText(descriptionContent.descriptionText.text);
    }

    public void ReadContentText(string content)
    {
        ReadText(content);
    }

    public IEnumerator PlayVideo(VideoPlayer videoPlayer, bool isUrl, string url)
    {
        if (!isUrl)
        {
            videoPlayer.source = VideoSource.VideoClip;
        }
        else
        {
            videoPlayer.source = VideoSource.Url;
            videoPlayer.url = url;
        }

        videoPlayer.Prepare();

        while (!videoPlayer.isPrepared)
        {
            Debug.Log("Preparing video...");
            yield return null;
        }

        Debug.Log("Prepared...");
        videoPlayer.GetComponent<RawImage>().texture = videoPlayer.texture;
        videoPlayer.Play();

        while (videoPlayer.isPlaying)
        {
            Debug.LogWarning("Video Time: " + Mathf.FloorToInt((float)videoPlayer.time));
            yield return null;
        }

        Debug.Log("Done Playing Video");
    }

    public void RemoveDescriptionComponent()
    {
        descriptionContent.gameObject.SetActive(false);
    }

    public void RemoveAllButtons()
    {
        foreach (DictionaryButton b in buttonList)
        {
            b.buttonComponent.gameObject.SetActive(false);
        }
    }

    public void ReadLetterButtin(string letter)
    {
        ReadText(letter);
    }

    public string GetTextDescription(string key, string localization)
    {
        string result;

        if (localization.Equals("locales_ptbr.json"))
            result = description_ptbr[key];
        else
            result = description_en[key];

        return result;
    }

    public List<string> GetAllTextStartingWithLetter(string letter)
    {
        List<string> result = new List<string>();

        if (LocalizationManager.instance.GetLozalization().Equals("locales_ptbr.json"))
        {
            result = keys_ptbr.Where(r => r.ToLower().StartsWith(letter)).ToList<string>();
        }
        else
        {
            result = keys_en.Where(r => r.ToLower().StartsWith(letter)).ToList<string>();
        } 
        return result;
    }

    public void BackButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MenuScene");
    }

    public void ResetVerticalPositionScrollRect()
    {
        m_verticalPosition = 1f;
        m_index = 0;
    }

    public AudioClip GetAudioClip(string key, string localization)
    {
        AudioClip audioClip;

        if (localization.Equals("locales_ptbr.json"))
            audioClip = Resources.Load<AudioClip>(audio_ptbr[key]);
        else
            audioClip = Resources.Load<AudioClip>(audio_en[key]);

        return audioClip;
    }


    public void SetFirstElement(DictionaryButton db)
    {
        db.first = true;
    }

    public void SetLastElement(DictionaryButton db)
    {
        db.last = true;
    }

    public void ResetFirstLastElements(List<DictionaryButton> list)
    {
        foreach(DictionaryButton db in list)
        {
            db.first = false;
            db.last = false;
        }
    }
}

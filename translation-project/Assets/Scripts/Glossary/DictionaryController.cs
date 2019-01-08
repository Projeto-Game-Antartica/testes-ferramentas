using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Video;

public class DictionaryController : MonoBehaviour {
    
    // nome do arquivo JSON
    private string dataFilename = "glossary.json";

    // Classe C# para mapear o JSON
    DataArray loadedData;

    public Transform contentPanel;
    public SimpleObjectPool buttonObjectPool;
    //public SimpleObjectPool textObjectPool;

    public DescriptionContent descriptionContent;
    private List<DictionaryButton> buttonList;

    // lista contendo as palavras em portugues
    private List<string> keys_ptbr;

    // list contendo as palavras em inglês
    private List<string> keys_en;

    // hashmap contendo a palavra em portugues que o leva a sua descrição em portugues
    private Dictionary<string, string> description_ptbr;

    // hashmap contendo a palavra que o leva para a imagem
    private Dictionary<string, string> description_ptbrimage;

    // hashmap contendo a palavra que o leva para o video em libras
    private Dictionary<string, string> description_ptbrvideo;

    // hashmaps com keys em ingles
    private Dictionary<string, string> description_en;
    private Dictionary<string, string> description_enimage;
    private Dictionary<string, string> description_envideo;

    private const string dictionaryText = "Glossário em Português-Brasil e Libras. As letras estão separadas em botões" +
        "onde há duas linhas contendo treze letras em ordem alfabética. Ao selecionar a letra, palavras iniciando com essa letra" +
        "irão aparecer em forma de botões.";

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

        buttonList = new List<DictionaryButton>();

        keys_ptbr = new List<string>();
        keys_en = new List<string>();

        // pt-br information
        description_ptbr = new Dictionary<string, string>();
        description_ptbrimage = new Dictionary<string, string>();
        description_ptbrvideo = new Dictionary<string, string>();

        // en information
        description_en = new Dictionary<string, string>();
        description_enimage = new Dictionary<string, string>();
        description_envideo = new Dictionary<string, string>();

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
                }
                AddButton(keys_ptbr);
            }
            else
            {
                // carga das listas/dicionários com o conteúdo do JSON em ingles
                for (int i = 0; i < loadedData.items.Length; i++)
                {
                    keys_en.Add(loadedData.items[i].key_en);
                    description_en.Add(loadedData.items[i].key_en, loadedData.items[i].description_en);
                    description_enimage.Add(loadedData.items[i].key_en, loadedData.items[i].image_path);
                    description_envideo.Add(loadedData.items[i].key_en, loadedData.items[i].video_path);
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

            DictionaryButton dictionaryButton = newButton.GetComponent<DictionaryButton>();
            dictionaryButton.keyLabel.text = key;

            buttonList.Add(dictionaryButton);
        }
    }

    public void ShowAllButtons()
    {
        RemoveDescriptionComponent();

        foreach (DictionaryButton b in buttonList)
        {
            b.gameObject.SetActive(true);
        }
    }

    public void ShowButtonStartingWithLetter(string letter)
    {
        RemoveDescriptionComponent();

        foreach(DictionaryButton b in buttonList)
        {
            if (b.keyLabel.text.ToLower().StartsWith(letter))
            {
                b.buttonComponent.gameObject.SetActive(true);
            }
            else
            {
                b.buttonComponent.gameObject.SetActive(false);
            }
        }
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
            videoClip = Resources.Load<VideoClip>(description_ptbrvideo[key]);

            descriptionContent.videoPlayer.clip = videoClip;
            StartCoroutine(PlayVideo(descriptionContent.videoPlayer));
        }
        else // descricao em ingles
        {
            image = Resources.Load<Sprite>(description_enimage[key]);
            descriptionContent.videoPlayer.gameObject.SetActive(false);
        }

        descriptionContent.descriptionText.text = GetTextDescription(key, LocalizationManager.instance.GetLozalization());
        descriptionContent.image.sprite = image;
        
        ReadContentText(descriptionContent.descriptionText.text);
    }

    public void ReadContentText(string content)
    {
        TolkUtil.Speak(content);
    }

    public IEnumerator PlayVideo(VideoPlayer videoPlayer)
    {
        videoPlayer.Prepare();

        while (!videoPlayer.isPrepared)
        {
            Debug.Log("Not prepared");
            yield return null;
        }

        Debug.Log("Prepared...");
        videoPlayer.GetComponent<RawImage>().texture = videoPlayer.texture;
        videoPlayer.Play();
        Debug.Log(videoPlayer.isPlaying);
    }

    public void RemoveDescriptionComponent()
    {
        //if (descriptionContent.gameObject != null)
        //{
        //    Debug.Log("RemoveDescriptionComponent");
        //    Debug.Log(descriptionContent.gameObject);
        //    if (descriptionContent.isActiveAndEnabled)
        //    {
        //        Destroy(descriptionContent.gameObject);
        //        Debug.Log("Object Destroyed");
        //    }
        //}

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
        TolkUtil.Speak(letter);
    }

    public string GetTextDescription(string key, string localization)
    {
        if (localization.Equals("locales_ptbr.json"))
            return description_ptbr[key];
        else
            return description_en[key];
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

    //public void ShowAllTextStartingWithLetter(string letter)
    //{
    //    int i = 0;
    //    descriptionText.text = "";
    //    foreach (string s in keys_ptbr.Where(r => r.ToLower().StartsWith(letter)).ToList())
    //    {
    //        descriptionText.text += "(" + i +  ") "+ s + ": " + description_ptbr[s] + '\n';
    //        i++;
    //    }
    //}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.IO;

public class DictionaryController : MonoBehaviour {
    
    // nome do arquivo JSON
    private string dataFilename = "glossary.json";

    // Classe C# para mapear o JSON
    DataArray loadedData;
        
    public  Text textArea;
    public Transform contentPanel;
    public SimpleObjectPool buttonObjectPool;

    // lista contendo as palavras em portugues
    private List<string> keys_ptbr;

    // list contendo as palavras em inglês
    private List<string> keys_en;

    // hashmap contendo a palavra em portugues que o leva a sua descrição em portugues
    private Dictionary<string, string> description_ptbr;

    void Start()
    {
        LoadDictionary();
    }

    public void LoadDictionary()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, dataFilename);

        keys_ptbr = new List<string>();
        keys_en = new List<string>();
        description_ptbr = new Dictionary<string, string>();

        if (File.Exists(filePath))
        {
            // leitura do JSON
            string dataAsJson = File.ReadAllText(filePath);
            loadedData = JsonUtility.FromJson<DataArray>(dataAsJson);

            // carga das listas/dicionários com o conteúdo do JSON
            for (int i = 0; i < loadedData.items.Length; i++)
            {
                keys_ptbr.Add(loadedData.items[i].key_ptbr);
                keys_en.Add(loadedData.items[i].key_en);
                description_ptbr.Add(loadedData.items[i].key_ptbr, loadedData.items[i].description_ptbr);
            }

            AddButton(keys_ptbr);
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
            newButton.transform.SetParent(contentPanel);

            DictionaryButton dictionaryButton = newButton.GetComponent<DictionaryButton>();
            dictionaryButton.keyLabel.text = key;
        }
    }

    public void PrintInTextArea(List<string> list)
    {
        int i = 0;
        list.Sort();

        foreach(string s in list)
        {
            textArea.text += "(" + i + ") " + s + '\n';
            i++;
        }
        
    }

    public string getTextDescription(string key)
    {
        return description_ptbr[key];
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

    public void ShowAllTextStartingWithLetter(string letter)
    {
        int i = 0;
        textArea.text = "";
        foreach (string s in keys_ptbr.Where(r => r.ToLower().StartsWith(letter)).ToList())
        {
            textArea.text += "(" + i +  ") "+ s + ": " + description_ptbr[s] + '\n';
            i++;
        }
    }
}

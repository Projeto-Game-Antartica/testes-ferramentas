using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

[Serializable]
public class DialogueDataArray
{
    public DialogueData[] items;
}

[Serializable]
public class DialogueData
{
    public string key;
    public string dialogue_text;
}


public class DialoguesVideo : MonoBehaviour
{
    /*
    * nome do arquivo JSON
    */
    private const string dataFilename = "dialoguesLibras.json";

    /*
    * Variáveis para controle do glossário
    */
    private List<string> keys;
    private Dictionary<string, string> dialogue_text;

    /*
    * Classe C# para mapear o JSON
    */
    DialogueDataArray loadedDialogueData;

    // Start is called before the first frame update
    void Start()
    {
        LoadDictionary();
    }

    public void LoadDictionary()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, dataFilename);

        keys = new List<string>();

        // pt-br information
        dialogue_text = new Dictionary<string, string>();

        if (File.Exists(filePath))
        {
            // leitura do JSON
            string dataAsJson = File.ReadAllText(filePath);
            loadedDialogueData = JsonUtility.FromJson<DialogueDataArray>(dataAsJson);

            // carga das listas/dicionários com o conteúdo do JSON
            for (int i = 0; i < loadedDialogueData.items.Length; i++)
            {
                keys.Add(loadedDialogueData.items[i].key);
                dialogue_text.Add(loadedDialogueData.items[i].key, loadedDialogueData.items[i].dialogue_text);
            }

            Debug.Log("loaded " + loadedDialogueData.items.Length + " items");
        }
        else
        {
            Debug.LogError("Cannot find file!");
        }
    }
}

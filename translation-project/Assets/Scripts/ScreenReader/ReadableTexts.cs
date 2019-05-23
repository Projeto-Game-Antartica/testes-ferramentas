using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/*
 * Class to encapsulate all readable (by screenreader) texts that will be used in the game.
 * format: mission_textname
 */
public class ReadableTexts : MonoBehaviour
{
    /*
    * nome do arquivo JSON
    */
    private const string dataFilename = "readabletexts.json";

    private Dictionary<string, string> readabletext_ptbr;
    private Dictionary<string, string> readabletext_en;

    TextDataArray loadedData;

    private void Awake()
    {
        LoadReadableTexts();
        DontDestroyOnLoad(this.gameObject);
    }

    // create a dictionary with the readable texts in ptbr and en
    public void LoadReadableTexts()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, dataFilename);
        

        // pt-br information
        readabletext_ptbr = new Dictionary<string, string>();
        // en information
        readabletext_en = new Dictionary<string, string>();
        if (File.Exists(filePath))
        {
            // leitura do JSON
            string dataAsJson = File.ReadAllText(filePath);
            loadedData = JsonUtility.FromJson<TextDataArray>(dataAsJson);

            // carga dos dicionários com o conteúdo do JSON
            for (int i = 0; i < loadedData.items.Length; i++)
            {
                readabletext_ptbr.Add(loadedData.items[i].key, loadedData.items[i].readabletext_ptbr);
                readabletext_en.Add(loadedData.items[i].key, loadedData.items[i].readabletext_en);
            }

            Debug.Log(readabletext_ptbr.Count);
        }
        else
        {
            Debug.LogError("Cannot find file!");
        }
    }

    public string GetReadableText(string key, string localization)
    {
        string result;

        if (localization.Equals("locales_ptbr.json"))
            result = readabletext_ptbr[key];
        else
            result = readabletext_en[key];

        return result;
    }

    /*
     * Keys to JSON readabletexts file
     */
    public static string key_languagemenu_instructions = "languagemenu_instructions";

    public static string key_mainmenu_instructions = "mainmenu_instructions";

    public static string key_optionmenu_instructions = "optionmenu_instructions";
    
    public static string key_playmenu_instructions = "playmenu_instructions";

    public static string key_glossary_instructions = "glossary_instructions";

    public static string key_soundglossary_instructions = "soundglossary_instructions";

    public static string key_foto_instructions = "foto_instructions";

    public static string key_foto_sceneDescription = "foto_sceneDescription";

    public static string key_foto_catalogDescription = "foto_catalogDescription";

    public static string key_quiz_instructions = "quiz_instructions ";
}

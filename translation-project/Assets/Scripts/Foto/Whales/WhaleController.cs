using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WhaleController : MonoBehaviour {

    /*
     * nome do arquivo JSON
     */
    private const string dataFilename = "whales.json";

    /*
     * Classe C# para mapear o JSON
     */
    WhaleArray whaleArray;

    private List<int> id_whale;
    private Dictionary<int, string> latitude;
    private Dictionary<int, string> longitude;
    private Dictionary<int, string> image;
    
    // Use this for initialization
    void Start ()
    {
        LoadWhales();
	}

    void LoadWhales()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, dataFilename);

        // id_whale -> latitude/longitude/image
        id_whale    = new List<int>();
        latitude    = new Dictionary<int, string>();
        longitude   = new Dictionary<int, string>();
        image       = new Dictionary<int, string>();

        if (File.Exists(filePath))
        {
            // leitura do JSON
            string dataAsJson = File.ReadAllText(filePath);
            whaleArray = JsonUtility.FromJson<WhaleArray>(dataAsJson);

            // carga das listas/dicionários
            for (int i = 0; i < whaleArray.items.Length; i++)
            {
                id_whale.Add(whaleArray.items[i].id_whale);
                latitude.Add(whaleArray.items[i].id_whale, whaleArray.items[i].latitude);
                longitude.Add(whaleArray.items[i].id_whale, whaleArray.items[i].longitude);
                image.Add(whaleArray.items[i].id_whale, whaleArray.items[i].image_path);
            }
        }
        else
        {
            Debug.LogError("Cannot find file!");
        }
    }

    public WhaleData getWhaleById(int id)
    {
        return new WhaleData
            {
                latitude = latitude[id],
                longitude = longitude[id],
                image_path = image[id]
            };
    }
}

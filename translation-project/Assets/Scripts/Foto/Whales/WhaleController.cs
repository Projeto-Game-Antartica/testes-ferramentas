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
    private Dictionary<int, string> whale_name;
    private Dictionary<int, string> latitude;
    private Dictionary<int, string> longitude;
    private Dictionary<int, string> image;
    private Dictionary<int, string> borda;
    private Dictionary<int, string> ponta;
    private Dictionary<int, string> entalhe;
    private Dictionary<int, string> manchas;
    private Dictionary<int, string> riscos;
    private Dictionary<int, string> marcas;
    private Dictionary<int, string> pigmentacao;
    private Dictionary<int, string> description;

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
        whale_name  = new Dictionary<int, string>();
        latitude    = new Dictionary<int, string>();
        longitude   = new Dictionary<int, string>();
        image       = new Dictionary<int, string>();
        borda       = new Dictionary<int, string>();
        ponta       = new Dictionary<int, string>();
        entalhe     = new Dictionary<int, string>();
        manchas     = new Dictionary<int, string>();
        riscos      = new Dictionary<int, string>();
        marcas      = new Dictionary<int, string>();
        pigmentacao = new Dictionary<int, string>();
        description = new Dictionary<int, string>();

        if (File.Exists(filePath))
        {
            // leitura do JSON
            string dataAsJson = File.ReadAllText(filePath);
            whaleArray = JsonUtility.FromJson<WhaleArray>(dataAsJson);

            // carga das listas/dicionários
            for (int i = 0; i < whaleArray.items.Length; i++)
            {
                id_whale.Add(whaleArray.items[i].id_whale);
                whale_name.Add(whaleArray.items[i].id_whale, whaleArray.items[i].whale_name);
                latitude.Add(whaleArray.items[i].id_whale, whaleArray.items[i].latitude);
                longitude.Add(whaleArray.items[i].id_whale, whaleArray.items[i].longitude);
                image.Add(whaleArray.items[i].id_whale, whaleArray.items[i].image_path);
                borda.Add(whaleArray.items[i].id_whale, whaleArray.items[i].borda);
                ponta.Add(whaleArray.items[i].id_whale, whaleArray.items[i].ponta);
                entalhe.Add(whaleArray.items[i].id_whale, whaleArray.items[i].entalhe);
                manchas.Add(whaleArray.items[i].id_whale, whaleArray.items[i].manchas);
                riscos.Add(whaleArray.items[i].id_whale, whaleArray.items[i].riscos);
                marcas.Add(whaleArray.items[i].id_whale, whaleArray.items[i].marcas);
                pigmentacao.Add(whaleArray.items[i].id_whale, whaleArray.items[i].pigmentacao);
                description.Add(whaleArray.items[i].id_whale, whaleArray.items[i].description);
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
            whale_name = whale_name[id],
            latitude = latitude[id],
            longitude = longitude[id],
            image_path = image[id],
            borda = borda[id],
            ponta = ponta[id],
            entalhe = entalhe[id],
            manchas = manchas[id],
            riscos = riscos[id],
            marcas = marcas[id],
            pigmentacao = pigmentacao[id],
            description = description[id]
        };
    }
}

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FossilController : MonoBehaviour
{

    /*
     * nome do arquivo JSON
     */
    private const string dataFilename = "fossil.json";

    /*
     * Classe C# para mapear o JSON
     */
    FossilArray fossilArray;

    private List<int> id_fossil;
    private Dictionary<int, string> caracteristica;
    private Dictionary<int, string> classificacao;
    private Dictionary<int, string> era;
    private Dictionary<int, string> description;
    private Dictionary<int, string> image_path;


    // Start is called before the first frame update
    void Start()
    {
        LoadFossil();
    }

    void LoadFossil()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, dataFilename);

        // id_whale -> latitude/longitude/image
        id_fossil    = new List<int>();
        caracteristica  = new Dictionary<int, string>();
        classificacao    = new Dictionary<int, string>();
        era   = new Dictionary<int, string>();
        description = new Dictionary<int, string>();
        image_path = new Dictionary<int, string>();



        if (File.Exists(filePath))
        {
            // leitura do JSON
            string dataAsJson = File.ReadAllText(filePath);
            fossilArray = JsonUtility.FromJson<FossilArray>(dataAsJson);
            //Debug.Log(fossilArray.items[0].id_fossil);


            // carga das listas/dicionários
            for (int i = 0; i < fossilArray.items.Length; i++)
            {
                id_fossil.Add(fossilArray.items[i].id_fossil);
                caracteristica.Add(fossilArray.items[i].id_fossil, fossilArray.items[i].caracteristica);
                classificacao.Add(fossilArray.items[i].id_fossil, fossilArray.items[i].classificacao);
                era.Add(fossilArray.items[i].id_fossil, fossilArray.items[i].era);
                image_path.Add(fossilArray.items[i].id_fossil, fossilArray.items[i].image_path);
            }
        }
        else
        {
            Debug.LogError("Cannot find file!");
        }
    }

    public FossilData getFossilById(int id)
    {
        return new FossilData
        {
            id_fossil = id_fossil[id],
            caracteristica = caracteristica[id],
            classificacao = classificacao[id],
            era = era[id],
            image_path = image_path[id]
        };
    }
}

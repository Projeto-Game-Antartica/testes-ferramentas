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

    public static ReadableTexts instance; 

    TextDataArray loadedData;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        LoadReadableTexts();
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

    // prejogo
    public static string key_prejogo_menu = "prejogo_menu"; 
    public static string key_prejogo_config = "prejogo_config"; 
    public static string key_prejogo_ajuda = "prejogo_ajuda"; 
    public static string key_prejogo_sons = "prejogo_sons"; 
    public static string key_prejogo_menu_carregamento = "prejogo_menu_carregamento";
    public static string key_prejogo_aviso = "prejogo_aviso";

    // gameplay
    public static string key_gameplay_ingamemenu = "gameplay_ingamemenu";
    public static string key_gameplay_aviso = "gameplay_aviso"; 
    public static string key_gameplay_aviso_botoes = "gameplay_aviso_botoes"; 

    // dialogo
    public static string key_dialogo_player = "dialogo_player"; 
    public static string key_dialogo_npc = "dialogo_npc";

    // M004 - Baleias
    public static string key_m004_navio_instrucao = "m004_navio_instrucao"; 
    public static string key_m004_navio = "m004_navio"; 
    public static string key_m004_memoria_instrucao = "m004_memoria_instrucao"; 
    public static string key_m004_memoria = "m004_memoria"; 
    public static string key_m004_memoria_derrota = "m004_memoria_derrota"; 
    public static string key_m004_memoria_vitoria = "m004_memoria_vitoria"; 
    public static string key_m004_fotoidentificacao_instrucao = "m004_fotoidentificacao_instrucao"; 
    public static string key_m004_fotoidentificacao = "m004_fotoidentificacao"; 
    public static string key_m004_fotoidentificacao_pigmentacao = "m004_fotoidentificacao_pigmentacao"; 
    public static string key_m004_fotoidentificacao_mancha = "m004_fotoidentificacao_mancha"; 
    public static string key_m004_fotoidentificacao_riscos = "m004_fotoidentificacao_riscos"; 
    public static string key_m004_fotoidentificacao_marcas = "m004_fotoidentificacao_marcas"; 
    public static string key_m004_fotoidentificacao_entalhe = "m004_fotoidentificacao_entalhe"; 
    public static string key_m004_fotoidentificacao_borda = "m004_fotoidentificacao_borda"; 
    public static string key_m004_fotoidentificacao_ponta = "m004_fotoidentificacao_ponta"; 
    public static string key_m004_fotoidentificacao_derrota = "m004_fotoidentificacao_derrota"; 
    public static string key_m004_fotoidentificacao_vitoria = "m004_fotoidentificacao_vitoria"; 
    public static string key_m004_teia_instrucao = "m004_teia_instrucao"; 
    public static string key_m004_teia = "m004_teia"; 
    public static string key_m004_teia_vitoria = "m004_teia_vitoria"; 
    public static string key_m004_teia_derrota = "m004_teia_derrota";
    public static string key_m004_desafio_instrucao = "m004_desafio_instrucao";
    public static string key_m004_desafio_camera = "m004_desafio_camera";
    public static string key_m004_desafio_catalogo = "m004_desafio_catalogo";
    public static string key_m004_desafio_aviso = "m004_desafio_aviso";
    public static string key_m004_desafio_sucesso = "m004_desafio_sucesso";
    public static string key_m004_navio_mapa = "m004_navio_mapa";

    // M002 - Itens
    public static string key_m002_itens_mapa = "m002_itens_mapa";
    public static string key_m002_instrucoes = "m002_instrucoes";
    public static string key_m002_processo_instrucao = "m002_processo_instrucao";
    public static string key_m002_processo = "m002_processo";
    public static string key_m002_processo_vitoria = "m002_processo_vitoria";
    public static string key_m002_processo_derrota = "m002_processo_derrota";
    public static string key_m002_trilha_instrucao = "m002_trilha_instrucao";
    public static string key_m002_trilha = "m002_trilha";
    public static string key_m002_trilha_vitoria = "m002_trilha_vitoria";
    public static string key_m002_trilha_derrota = "m002_trilha_derrota";
    public static string key_m002_regras_instrucao = "m002_regras_instrucao";
    public static string key_m002_regras = "m002_regras";
    public static string key_m002_regras_vitoria = "m002_regras_vitoria";
    public static string key_m002_homeostase_instrucao = "m002_homeostase_instrucao";
    public static string key_m002_homeostase = "m002_homeostase";
    public static string key_m002_homeostase_cesta = "m002_homeostase_cesta";
    public static string key_m002_homeostase2 = "m002_homeostase2";
    public static string key_m002_homeostase_vitoria = "m002_homeostase_vitoria";
    public static string key_m002_homeostase_derrota = "m002_homeostase_derrota";
    public static string key_m002_casinha_mapa = "m002_casinha_mapa";



    // old keys
    public static string key_languagemenu_instructions = "languagemenu_instructions";
    public static string key_mainmenu_instructions = "mainmenu_instructions";
    public static string key_optionmenu_instructions = "optionmenu_instructions";
    public static string key_playmenu_instructions = "playmenu_instructions";
    public static string key_glossary_instructions = "glossary_instructions";
    public static string key_soundglossary_instructions = "soundglossary_instructions";
    public static string key_navio_instructions = "navio_instructions";
    public static string key_foto_instructions = "foto_instructions";
    public static string key_foto_identification = "foto_identification";
    public static string key_foto_sceneDescription = "foto_sceneDescription";
    public static string key_foto_catalogDescription = "foto_catalogDescription";
    public static string key_quiz_instructions = "quiz_instructions";
}

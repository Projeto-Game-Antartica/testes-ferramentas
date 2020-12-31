using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

using UnityEngine.SceneManagement;

//using String.Normalize;

public class VideoPathFinder : MonoBehaviour
{
    /*
    * nome do arquivo JSON
    */
    private const string dataFilename = "videos_libras.json";
    private static Dictionary<string, List<HashSet<string>>> scenevideosTextsTokens = new Dictionary<string, List<HashSet<string>>>();
    private static Dictionary<string, List<string>> scenevideosPaths = new Dictionary<string, List<string>>();

    //private static List<HashSet<string>> videosTextsTokens = new List<HashSet<string>>();
    //private static List<string> videosPaths = new List<string>();

    private static VideoPathFinder instance;

    //private Dictionary<string, string> cellTexts = new Dictionary<string, string>();
    private static List<string> locationList = new List<string>();
    private static List<string> cellTextList = new List<string>();

    private static Matrix<double> corpusMatrix;

    private static Vocabulary vocabulary;

    private void loadJsonFile() {
        //This method loads a json file with video texts and paths and 
        //calculate text similarity using jaccard indexes over set of tokens

        string filePath = Path.Combine(Application.streamingAssetsPath, dataFilename);
        
        if (File.Exists(filePath)) {
            // leitura do JSON
            string dataAsJson = File.ReadAllText(filePath);
            VideosLibras loadedData = JsonUtility.FromJson<VideosLibras>(dataAsJson);

            foreach(SceneVideos scene in loadedData.scenes) {
                string sceneName = scene.scene_name;
                
                scenevideosTextsTokens[sceneName] = new List<HashSet<string>>();
                scenevideosPaths[sceneName] = new List<string>();

                foreach(VideoData video in scene.videos) {
                    scenevideosPaths[sceneName].Add(loadedData.abs_path + video.rel_path);
                    HashSet<string> textTokens = new HashSet<string>(tokenize(video.text));
                    scenevideosTextsTokens[sceneName].Add(textTokens);
                }
            }

            //Debug.Log(get_jaccard_index(videosTextsTokens[0], videosTextsTokens[0]));

        } else {
            Debug.LogError("ERROR: videos_libras.json NOT FOUND.");
        }
    }

    private static float get_jaccard_index(IEnumerable<string> set1, IEnumerable<string> set2) {
        int intersection_size = set1.Intersect(set2).Count();
        int union_size = set1.Union(set2).Count();
        return (float)intersection_size / (float)union_size;
    }

    private static string[] tokenize(string text) {
        //Remove punctuation from the chracteres and them split
        string textWoPunct = "";
        foreach(char c in text)
            if(!char.IsPunctuation(c))
               textWoPunct += c; 
        // = new string(text.ToCharArray().Where(c => !char.IsPunctuation(c)).ToArray());
        return textWoPunct.ToLower().Split(new char [] {' '});
    }

    private void loadDotProductMethod() {
        //Populate spreadsheet texts into the dictionary if the resource exists
        TextAsset csvFile = Resources.Load<TextAsset>("VideoLibras/" + SceneManager.GetActiveScene().name);
        if(csvFile == null) {
            Debug.Log("Resource for VideoLibras/" + SceneManager.GetActiveScene().name + " does not exists.");
            return;
        }

        string[,] csvGrid = getCSVGrid(csvFile.text);
        for(int i = 0; i <= csvGrid.GetUpperBound(0); i++) {
            for(int j = 0; j <= csvGrid.GetUpperBound(1); j++) {
                string cellText = csvGrid[i,j];
                if(cellText != null && cellText != "") {
                    string location = getColumnLettersByIndex(i) + (j+1);
                    //cellTexts.Add(location, cellText);
                    locationList.Add(location);
                    cellTextList.Add(cellText);
                    //Debug.Log(location + " " + cellText);
                }
            }
        }

        //Get corpus, vocabulary and corpus matrix
        string corpus = string.Join(" ", cellTextList);

        vocabulary = new Vocabulary(corpus);

        List<Vector<double>> textVectors = new List<Vector<double>>();

        foreach(string text in cellTextList)
            textVectors.Add(vocabulary.Vectorize(text));

        corpusMatrix = DenseMatrix.OfRowVectors(textVectors);
    }

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        loadJsonFile();

        //loadDotProductMethod() //Deprecated

        //Must create debug interface
    }

    // Update is called once per frame
    // void Update()
    // {
        
    // }


    string getColumnLettersByIndex(int columnIndex) {
        //Absolute: 24 -> B  27 -> E  55 -> a  Relative to 65: 0->A e 25->Z

        string columnLetters = "";

        while(columnIndex >= 0) {
            char letter = (char) (columnIndex % 26 + 65);
            columnIndex = columnIndex / 26 - 1;
            columnLetters = letter + columnLetters;
        }

        return columnLetters;
    }


    static string[,] getCSVGrid(string csvText) {
        //split the data on split line character
        string[] lines = csvText.Split("\n"[0]);

        // find the max number of columns
        int totalColumns = 0;
        for (int i = 0; i < lines.Length; i++)
        {
            string[] row = lines[i].Split(',');
            totalColumns = Mathf.Max(totalColumns, row.Length);
        }

        // creates new 2D string grid to output to
        string[,] outputGrid = new string[totalColumns + 1, lines.Length + 1];
        for (int y = 0; y < lines.Length; y++)
        {
            string[] row = lines[y].Split(',');
            for (int x = 0; x < row.Length; x++)
            {
                outputGrid[x, y] = row[x];
            }
        }

        return outputGrid;
    }

    //Find the closest text by computing its dot product with a matrix of sentence vectors
    //DEPRECTED: Although more efficient, this method is too complex for maintenance
    // public static string FindPath(string text) {
    //     Vector<double> textVector = vocabulary.Vectorize(text);
    //     Vector<double> similarities = corpusMatrix.Multiply(textVector);
    //     int argMax = similarities.MaximumIndex();
    //     Debug.Log("CHOSEN TEXT: " + locationList[argMax] + " - " + cellTextList[argMax]);
    //     return locationList[argMax];
    // }

    //This version uses a much more simple method just getting similarity score by set operations
    public static string FindPath(string text, string sceneName) {
        HashSet<string> text_tokens_set = new HashSet<string>(tokenize(text));
        float max_similarity = -1;
        int max_similarity_index = -1;
        for(int i = 0; i < scenevideosTextsTokens[sceneName].Count; i++) {
            //Calculate similarity between the text and all the texts attached to some video
            float sim = get_jaccard_index(scenevideosTextsTokens[sceneName][i], text_tokens_set);
            if(sim > max_similarity) {
                max_similarity = sim;
                max_similarity_index = i;
            }
        }

        //Print Debug
        // string debugStr = "PATH: " + videosPaths[max_similarity_index] + 
        //     "\nTOKENS_SIM: " + printSet(videosTextsTokens[max_similarity_index]) + 
        //     "\nTEXT: " + text;
        // Debug.Log(debugStr);
        
        return scenevideosPaths[sceneName][max_similarity_index];
    }

    private static string printSet(HashSet<string> ss) {
        string printStr = "[";
        foreach(string s in ss) {
            printStr += s + ", ";
        }
        printStr += "]";
        return printStr;
    }

    private static string vect2str(Vector<double> vect) {
        string textVectorString = "";
        foreach(double v in vect)
            textVectorString += v;
        return textVectorString;
    }
}

class Vocabulary {

    Dictionary<string, int> wordToInt;

    Vector<double> clearVector;

    public Vocabulary(string corpus) {
        wordToInt = new Dictionary<string, int>();

        HashSet<string> vocabularySet = new HashSet<string>();

        foreach(string token in tokenize(corpus))
            vocabularySet.Add(token);

        List<string> vocabulary = new List<string>(vocabularySet);

        clearVector = DenseVector.Create(vocabulary.Count, 0);

        for(int i = 0; i < vocabulary.Count; i++) {
            wordToInt[vocabulary[i]] = i;
            //Debug.Log(vocabulary[i] + " -> " + i);
            //clearVector[i] = -1.0;
        }
    }

    private string[] tokenize(string text) {
        //Remove punctuation from the chracteres and them split
        string textWoPunct = "";
        foreach(char c in text)
            if(!char.IsPunctuation(c))
               textWoPunct += c; 
        // = new string(text.ToCharArray().Where(c => !char.IsPunctuation(c)).ToArray());
        return textWoPunct.ToLower().Split(new char [] {' '});
    }

    public Vector<double> Vectorize(string text) {
        Vector<double> textVector = clearVector.Clone();
        foreach(string token in tokenize(text)) {
            if(wordToInt.ContainsKey(token))
                textVector.At(wordToInt[token], 1.0); 
            //else
                //Debug.Log("Token doesn't exist: " + token);
        }
        return textVector;
    }
}

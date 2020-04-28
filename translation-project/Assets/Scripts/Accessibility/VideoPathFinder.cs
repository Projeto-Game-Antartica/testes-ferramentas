using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

using UnityEngine.SceneManagement;

//using String.Normalize;

public class VideoPathFinder : MonoBehaviour
{
    private static VideoPathFinder instance;

    //private Dictionary<string, string> cellTexts = new Dictionary<string, string>();
    private static List<string> locationList = new List<string>();
    private static List<string> cellTextList = new List<string>();

    private static Matrix<double> corpusMatrix;

    private static Vocabulary vocabulary;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

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

        //Must create debug interface
    }

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

    // Update is called once per frame
    // void Update()
    // {
        
    // }

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
    public static string FindPath(string text) {
        Vector<double> textVector = vocabulary.Vectorize(text);
        Vector<double> similarities = corpusMatrix.Multiply(textVector);
        int argMax = similarities.MaximumIndex();
        Debug.Log("CHOSEN TEXT: " + locationList[argMax] + " - " + cellTextList[argMax]);
        return locationList[argMax];
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

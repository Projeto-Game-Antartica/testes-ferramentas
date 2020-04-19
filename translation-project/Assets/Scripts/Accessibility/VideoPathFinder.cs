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

    private Dictionary<string, string> cellTexts = new Dictionary<string, string>();

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        //Populate spreadsheet texts into the dictionary if the resource exists
        TextAsset csvFile = Resources.Load<TextAsset>("VideoLibras/" + SceneManager.GetActiveScene().name);
        if(csvFile == null) {
            Debug.Log("Resource for VideoLibras/" + SceneManager.GetActiveScene().name + " does not exists.");
        } else {
            string[,] csvGrid = getCSVGrid(csvFile.text);
            for(int i = 0; i <= csvGrid.GetUpperBound(0); i++) {
                for(int j = 0; j <= csvGrid.GetUpperBound(1); j++) {
                    string cellText = csvGrid[i,j];
                    if(cellText != null && cellText != "") {
                        string location = getColumnLettersByIndex(i) + (j+1);
                        cellTexts.Add(location, cellText);
                        Debug.Log(location + " " + cellText);
                    }
                }
            }
        }

        //https://numerics.mathdotnet.com/
        //Create set to store vocabulary then create object to transform vocabulary in vectors and methods to compare and return the closest


        // Debug.Log(getColumnLettersByIndex(0));
        // Debug.Log(getColumnLettersByIndex(10));
        // Debug.Log(getColumnLettersByIndex(24));
        // Debug.Log(getColumnLettersByIndex(25));
        // Debug.Log(getColumnLettersByIndex(26));
        // Debug.Log(getColumnLettersByIndex(27));
        // Debug.Log(getColumnLettersByIndex(55));
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
    void Update()
    {
        
    }

    // static string RemoveDiacritics(string text) 
    // {
    //     var normalizedString = text.Normalize(NormalizationForm.FormD);
    //     var stringBuilder = new StringBuilder();

    //     foreach (var c in normalizedString)
    //     {
    //         var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
    //         if (unicodeCategory != UnicodeCategory.NonSpacingMark)
    //         {
    //             stringBuilder.Append(c);
    //         }
    //     }

    //     return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
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

    private void openSpreadsheetData(string spreadsheetPath) {
        //Debug.Log(spreadsheetPath);


        // Excel.Workbook excelWorkbook = excelApp.Workbooks.Open(workbookPath, 0, false, 5, "", "", false, Excel.XlPlatform.xlWindows, "", true, false, 0, true, false, false);
        // Excel.Sheets excelSheets = excelWorkbook.Worksheets;
        // string currentSheet = "Sheet1";
        // Excel.Worksheet excelWorksheet = (Excel.Worksheet)excelSheets.get_Item(currentSheet);
        // var cell = (Excel.Range)excelWorksheet.Cells[10, 2];

        //Vector2 vect = Vector2();
        //Debug.Log(vect);

        Matrix<double> A = DenseMatrix.OfArray(new double[,] {
        {1,1,1,1},
        {1,2,3,4},
        {4,3,2,1}});
        Vector<double>[] nullspace = A.Kernel();

        // verify: the following should be approximately (0,0,0)
        Debug.Log((A * (2*nullspace[0] - 3*nullspace[1])));
    }

    public static string FindPath(string text) {

        Debug.Log(text);
        return "";
    }
}

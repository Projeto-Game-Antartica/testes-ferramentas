using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class DictionaryController : MonoBehaviour {

    private DictionaryData data;
    private List<string> list = new List<string>();
    public  Text textArea;
    
    void Start()
    {
        InsertData("a");
        InsertData("AA");
        InsertData("b");
        InsertData("c");
        InsertData("d");
        InsertData("e");
        InsertData("f");
        InsertData("g");
        InsertData("h");
        InsertData("i");
        InsertData("j");
        InsertData("k");
        InsertData("l");
        InsertData("m");
        InsertData("n");
        InsertData("o");
        InsertData("p");
        InsertData("q");
        InsertData("r");
        InsertData("s");
        InsertData("t");
        InsertData("u");
        InsertData("v");
        InsertData("w");
        InsertData("x");
        InsertData("y");
        InsertData("z");

        textArea.text = "";
        foreach (string s in list)
        {
            textArea.text += ">>  " + s + '\n';
        }
    }

    public void InsertData(string text)
    {
        list.Add(text);
    }

    public List<string> GetAllTextStartingWithLetter(string letter)
    {
        var result = list.Where(r => r.StartsWith(letter));

        return result.ToList();
    }

    public void ShowAllTextStartingWithLetter(string letter)
    {
        textArea.text = "";
        foreach (string s in list.Where(r => r.StartsWith(letter)).ToList())
        {
            textArea.text += ">>  " + s + '\n';

        }
    }
}

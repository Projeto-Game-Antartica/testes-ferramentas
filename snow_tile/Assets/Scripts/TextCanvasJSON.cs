using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class TextCanvasJSON : MonoBehaviour {
    
    public Text textUI;
    public string url;
    public string dataAsJson;
    // Update is called once per frame
    void Update () {
		
	}

    IEnumerator DownloadStreaming()
    {
       
        /*
        UnityEngine.Networking.UnityWebRequest reader = UnityEngine.Networking.UnityWebRequest.Get(url);
        reader.SendWebRequest();
        */


        WWW www = new WWW(url);
        yield return www;
        
        while (!www.isDone) {
            yield return www;
        }
        
        dataAsJson = www.text;

        // Pass the json to JsonUtility, and tell it to create a GameData object from it
        TextoCanvas loadedData = JsonUtility.FromJson<TextoCanvas>(dataAsJson);
        textUI.text = loadedData.text;


    }

    private void Start()
    {
        textUI = GetComponent<Text>();
        url = Application.streamingAssetsPath + "/texto.json";//Application.streamingAssetsPath + "/texto.json"

        StartCoroutine("DownloadStreaming");

    }



}

[Serializable]
public class TextoCanvas
{
    public string id;
    public string text;
}

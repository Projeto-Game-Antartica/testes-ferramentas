using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextMeshJSON : MonoBehaviour {

    public TextMesh textMesh;
    public string url;
    public string dataAsJson;

    // Use this for initialization
    void Start () {



        textMesh = GetComponent<TextMesh>();
        url = Application.streamingAssetsPath + "/texto.json";//Application.streamingAssetsPath + "/texto.json"

        StartCoroutine("DownloadStreaming");
    }
	
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

        while (!www.isDone)
        {
            yield return www;
        }

        dataAsJson = www.text;

        // Pass the json to JsonUtility, and tell it to create a GameData object from it
        TextoCanvas loadedData = JsonUtility.FromJson<TextoCanvas>(dataAsJson);
        textMesh.text = loadedData.text;


    }
}

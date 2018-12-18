using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DataController : MonoBehaviour
{
    public RoundData[] allRoundData;


    // Use this for initialization
    void Start()
    {
        TextAsset jsonTextFile = Resources.Load<TextAsset>("quizz"); //Carrega arquivo Resource/quizz.json
        allRoundData = new RoundData[1];
        allRoundData[0] = JsonUtility.FromJson<RoundData>(jsonTextFile.ToString());
        DontDestroyOnLoad(gameObject);
    }
    /*
    private void Awake()
    {
        Debug.Log(JsonUtility.ToJson(allRoundData[0], true));
    }
    */
    public RoundData GetCurrentRoundData()
    {
        return allRoundData[0];
    }

    // Update is called once per frame
    void Update()
    {

    }
}
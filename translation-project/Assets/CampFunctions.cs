using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CampFunctions : MonoBehaviour
{
    // Start is called before the first frame update
    public VIDE_Assign mentorDesafio;

    public SimpleCharacterController player;
    private GameObject mentor;

    public ChasingCamera chasingCamera;

    void Start()
    {
        Debug.Log("TESTE M10AMOSTRAS");
        //Debug.Log(PlayerPreferences.M010_Amostras);
        Debug.Log(PlayerPrefs.HasKey("M010_Amostras"));
        if(PlayerPreferences.M010_Amostras && PlayerPreferences.M010_Tipos) {
            mentorDesafio.overrideStartNode = 0;
            //PlayerPreferences.M010_Amostras = true;
            //PlayerPreferences.M010_Tipos = true;
            PlayerPrefs.SetInt("M010_Amostras", 1);
        }

        if (PlayerPrefs.GetInt("Saved_m010") == 1)
        {
            transform.position = player.GetPosition("m010");
            chasingCamera.SetCameraPosition(player.GetPosition("m010"));
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadScene(string scene) {
        SavePosition();
        SceneManager.LoadScene(scene);
    }

    public void SavePosition() {
        Vector3 positionSceneChange = new Vector2(player.gameObject.transform.position.x, 
            player.gameObject.transform.position.y);

        player.gameObject.GetComponent<SimpleCharacterController>().SavePosition(positionSceneChange, "m010");
    }
}

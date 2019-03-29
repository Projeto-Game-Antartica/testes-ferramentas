using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameOptions : MonoBehaviour {

    public Toggle toggle;

    private void Start()
    {
        toggle.isOn = Parameters.ACCESSIBILITY;    
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void SetAcessibility(bool acessibility)
    {
        Parameters.ACCESSIBILITY = acessibility;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameOptions : MonoBehaviour {

    public Toggle toggle;
    public Animator charAnimator;

    private void Start()
    {
        toggle.isOn = Parameters.ACCESSIBILITY;    
    }

    private void OnEnable()
    {
        // runs idle animation when the ingameoption is active
        charAnimator.SetFloat("Magnitude", 0);

        if (Parameters.HIGH_CONTRAST) HighContrastText.ChangeTextBackgroundColor();
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

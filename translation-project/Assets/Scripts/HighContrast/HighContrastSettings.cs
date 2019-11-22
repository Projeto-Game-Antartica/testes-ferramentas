using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighContrastSettings : MonoBehaviour {
    
    //private void OnEnable()
    //{
    //    SetHighAccessibility();
    //}
        
    public void SetHighAccessibility()
    {
        Parameters.HIGH_CONTRAST = !Parameters.HIGH_CONTRAST;

        // when high constrast is active, button text doesnt change his color
        ButtonTextColor.ButtonContrast = Parameters.HIGH_CONTRAST;

        Debug.Log(gameObject.name + " SetHighAccessibility");

        ChangeHighContrast();
    }

    public void ChangeHighContrast()
    {
        if (Parameters.HIGH_CONTRAST)
            HighContrastText.ChangeTextBackgroundColor();
        else
            HighContrastText.RestoreToDefault("average");
    }
}

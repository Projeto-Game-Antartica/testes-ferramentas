using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighContrastSettings : MonoBehaviour {
       
    public void SetHighAccessibility()
    {
        Parameters.HIGH_CONTRAST = !Parameters.HIGH_CONTRAST;

        // when high constrast is active, button text doesnt change his color
        ButtonTextColor.ButtonContrast = Parameters.HIGH_CONTRAST;

        Debug.Log(gameObject.name + " SetHighAccessibility");
    }
}

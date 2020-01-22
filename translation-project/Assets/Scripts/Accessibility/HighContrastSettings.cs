using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighContrastSettings : MonoBehaviour {

    public void SetHighAccessibility(bool isOn)
    {
        Parameters.HIGH_CONTRAST = isOn;

        // when high constrast is active, button text doesnt change his color
        Parameters.BUTTONCONTRAST = !Parameters.HIGH_CONTRAST;

        Debug.Log(gameObject.name + " SetHighAccessibility");
    }
}

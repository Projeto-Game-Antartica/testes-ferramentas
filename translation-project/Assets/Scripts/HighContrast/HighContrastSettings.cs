using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighContrastSettings : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        Parameters.HIGH_CONTRAST = true;
        SetHighAccessibility();
    }

    private void OnEnable()
    {
        SetHighAccessibility();
    }

    private void SetHighAccessibility()
    {
        if (Parameters.HIGH_CONTRAST)
            HighContrastText.ChangeTextBackgroundColor();
        else
            HighContrastText.RestoreToDefault("swiss");
    }
}

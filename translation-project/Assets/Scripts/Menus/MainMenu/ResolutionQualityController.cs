using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResolutionQualityController : AbstractScreenReader
{
    // Called on dropdown OnValueChange event
    public void SetResolution(int value)
    {
        switch (value)
        {
            case 0:
                Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
                break;
            case 1: // 4:3
                Debug.Log("1024x768");
                Screen.SetResolution(1024, 768, true);
                break;
            case 2: // 16:10
                Debug.Log("1280x720");
                Screen.SetResolution(1280,720, true);
                break;
            case 3: // 16:10
                Debug.Log("1440x900");
                Screen.SetResolution(1440, 900, true);
                break;
            default:
                Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
                break;
        }
    }

    // Called on dropdown OnValueChange event
    public void SetQuality(int value)
    {
        Debug.Log(value);
        switch (value)
        {
            case 0:
                QualitySettings.SetQualityLevel(QualitySettings.GetQualityLevel());
                break;
            case 1: // high
                QualitySettings.SetQualityLevel(3);
                break;
            case 2: // medium
                QualitySettings.SetQualityLevel(2);
                break;
            case 3: // low
                QualitySettings.SetQualityLevel(1);
                break;
            default:
                QualitySettings.SetQualityLevel(QualitySettings.GetQualityLevel());
                break;
        }
    }

    // read drop down
    public void ReadDropDown(TMP_Dropdown dropdown)
    {
        ReadText(dropdown.name + " " + dropdown.options[dropdown.value].text);
    }
}

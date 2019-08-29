using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeExpController : MonoBehaviour {

    public Image HPImage;
    public Image EXPImage;

	// Use this for initialization
	void Start ()
    {
        HPImage.fillAmount = PlayerPrefs.GetFloat("HealthPoints");
        EXPImage.fillAmount = PlayerPrefs.GetFloat("Experience");
    }

    public void AddHP(float points)
    {
        HPImage.fillAmount += points;
        PlayerPrefs.SetFloat("HealthPoints", HPImage.fillAmount);
    }

    public void AddEXP(float points)
    {
        EXPImage.fillAmount += points;
        PlayerPrefs.SetFloat("Experience", EXPImage.fillAmount);
    }

    public void RemoveHP(float points)
    {
        HPImage.fillAmount -= points;
        PlayerPrefs.SetFloat("HealthPoints", HPImage.fillAmount);
    }

    public void RemoveEXP(float points)
    {
        EXPImage.fillAmount -= points;
        PlayerPrefs.SetFloat("Experience", EXPImage.fillAmount);
    }
	
}

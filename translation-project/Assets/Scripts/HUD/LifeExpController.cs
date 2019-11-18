using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeExpController : AbstractScreenReader {

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

    public void ReadHPandEXP()
    {
        // hp and exp points are 0 to 1
        Debug.Log("Você tem " +HPImage.fillAmount * 100  + " pontos de vida.");
        ReadText("Você tem " +HPImage.fillAmount * 100  + " pontos de vida.");

        Debug.Log("Você tem " +EXPImage.fillAmount * 100  + " pontos de experiência.");
        ReadText("Você tem " +EXPImage.fillAmount * 100  + " pontos de experiência.");
    }
	
}

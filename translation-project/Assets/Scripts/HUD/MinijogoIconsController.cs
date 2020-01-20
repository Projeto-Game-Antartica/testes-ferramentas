using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinijogoIconsController : AbstractScreenReader {

    public Image HeartImage;
    public Image StarImage;
    public Image AntarticaImage;

	// Use this for initialization
	void Start ()
    {
        PlayerPrefs.SetFloat("MJHealthPoints", 0f);
        PlayerPrefs.SetFloat("MJExperience", 0f);
        PlayerPrefs.SetFloat("MJAntartica", 0f);

        HeartImage.fillAmount = PlayerPrefs.GetFloat("MJHealthPoints");
        StarImage.fillAmount = PlayerPrefs.GetFloat("MJExperience");
        AntarticaImage.fillAmount = PlayerPrefs.GetFloat("MJAntartica");
    }

    private void Update()
    {
        if(Input.GetKeyDown(InputKeys.PARAMETERS_KEY))
        {
            ReadHPandEXP();
        }
    }

    public void AddPoints(float heartPoints, float starPoints, float antarticaPoints)
    {
        HeartImage.fillAmount += heartPoints;
        PlayerPrefs.SetFloat("MJHealthPoints", HeartImage.fillAmount);

        StarImage.fillAmount += starPoints;
        PlayerPrefs.SetFloat("MJExperience", StarImage.fillAmount);

        AntarticaImage.fillAmount += antarticaPoints;
        PlayerPrefs.SetFloat("MJAntartica", AntarticaImage.fillAmount);
    }

    public void ReadHPandEXP()
    {
        // hp and exp points are 0 to 1
        Debug.Log("Você tem " + (HeartImage.fillAmount * 100).ToString("F")  + " pontos de vida no minijogo.");
        ReadText("Você tem " + (HeartImage.fillAmount * 100).ToString("F") + " pontos de vida no minijogo.");

        Debug.Log("Você tem " + (StarImage.fillAmount * 100).ToString("F")  + " pontos de experiência no minijogo.");
        ReadText("Você tem " + (StarImage.fillAmount * 100).ToString("F") + " pontos de experiência no minijogo.");

        Debug.Log("Você tem " + (AntarticaImage.fillAmount * 100).ToString("F")  + " pontos de cuidado com o meio ambiente no minijogo.");
        ReadText("Você tem " + (AntarticaImage.fillAmount * 100).ToString("F") + " pontos de cuidado com o meio ambiente no minijogo.");
    }
	
}

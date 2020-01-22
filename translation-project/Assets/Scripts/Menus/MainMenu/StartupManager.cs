using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartupManager : MonoBehaviour {

    // Use this for initialization
    private IEnumerator Start () {
        
        // accessibility parameters start disabled
        Parameters.ACCESSIBILITY = false;
        Parameters.HIGH_CONTRAST = false;
        Parameters.BOLD = false;

        // change button color
        Parameters.BUTTONCONTRAST = true;

        //TolkUtil.Load();

        yield return new WaitForSeconds(2f);

        while (!LocalizationManager.instance.GetIsReady())
        {
            Debug.Log("loading...");
            yield return null;
        }

        Debug.Log("loaded");
        SceneManager.LoadScene(ScenesNames.Login);
        //SceneManager.LoadScene(ScenesNames.Menu);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartupManager : MonoBehaviour {

    // Use this for initialization
    private IEnumerator Start () {
        
        // accessibility and high contrast functions inactive
        Parameters.ACCESSIBILITY = false;
        Parameters.HIGH_CONTRAST = false;

        yield return new WaitForSeconds(2f);

        while (!LocalizationManager.instance.GetIsReady())
        {
            Debug.Log("loading...");
            yield return null;
        }

        Debug.Log("loaded");
        //SceneManager.LoadScene(ScenesNames.Login);
        SceneManager.LoadScene(ScenesNames.Menu);
    }

}

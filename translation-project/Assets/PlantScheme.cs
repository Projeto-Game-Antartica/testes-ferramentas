using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlantScheme : MonoBehaviour
{
    GameObject canvasGo;
    bool allowHide = false;
    void Start() {
        canvasGo = transform.GetChild(0).gameObject;
    }    


    void Update()
    {       
        if (Input.GetMouseButtonUp(0) || Input.GetKeyDown(KeyCode.Space)|| Input.GetKeyDown(KeyCode.Return)) {
            if(allowHide) Hide();
        }
    }

    public void Show() {
        canvasGo.SetActive(true);
        //Must generate a delay, otherwise the screen will show and imediatelly hide
        StartCoroutine(allowHideAfterDelay(3));   
    }

    IEnumerator allowHideAfterDelay(int secs) {
        yield return new WaitForSeconds(secs);
        allowHide = true;
    }
    public void Hide() {
        canvasGo.SetActive(false);
        allowHide = false;
    }
}

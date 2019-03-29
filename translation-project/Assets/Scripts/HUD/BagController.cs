using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagController : MonoBehaviour {

    // bag transform
    public GameObject bagbar;
    private float count;
    private const float time = 0.05f;

    public void OnButtonClick()
    {
        StartCoroutine(HandleBagBar());
    }

    public IEnumerator HandleBagBar()
    {
        // mochila fechada
        if (bagbar.transform.localScale.x <= 0)
        {
            count = 0;
            while (bagbar.transform.localScale.x <= 1)
            {
                Debug.Log("openning");
                count += time;
                SetBarSize(count);
                yield return new WaitForSeconds(0.03f);
            }
        }
        else if (bagbar.transform.localScale.x >= 1) // mochila aberta
        {
            count = 1;

            while (bagbar.transform.localScale.x >= 0)
            {
                Debug.Log("closing");
                count -= time;
                SetBarSize(count);
                yield return new WaitForSeconds(0.03f);
            }
        }
    }

    // bar size from 0 to 1
    public void SetBarSize(float normalizedSize)
    {
        bagbar.transform.localScale = new Vector3(normalizedSize, 1, 1);
    }
}

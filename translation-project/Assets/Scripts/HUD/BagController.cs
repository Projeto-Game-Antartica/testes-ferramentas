using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagController : MonoBehaviour {

    // bag transform
    public GameObject bagbar;
    private float count;
    private const float time = 0.05f;

    public Image bar;
    public Image camera_inv;
    public Image lente_inv;
    public Image ponteiraButton;

    public void OnButtonClick()
    {
        //StartCoroutine(HandleBagBar());

        if (bar.fillAmount == 0)
            StartCoroutine(OpenBag());
        else if(bar.fillAmount == 1)
            StartCoroutine(CloseBag());

    }

    public IEnumerator OpenBag()
    {
        while (bar.fillAmount < 1)
        {
            Debug.Log("openning");
            bar.fillAmount += 0.1f;

            if (bar.fillAmount > 0.4f && bar.fillAmount < 0.6f) // its float numbers
                StartCoroutine(ShowInvItems());

            yield return new WaitForSeconds(0.03f);
        }

        ponteiraButton.fillAmount = 1;
    }

    public IEnumerator ShowInvItems()
    {
        while (camera_inv.fillAmount < 1)
        {
            camera_inv.fillAmount += 0.1f;
            lente_inv.fillAmount += 0.1f;

            yield return new WaitForSeconds(0.03f);
        }
    }

    public IEnumerator CloseBag()
    {
        ponteiraButton.fillAmount = 0;

        while (bar.fillAmount > 0)
        {
            Debug.Log("closing");

            bar.fillAmount -= 0.1f;

            if (bar.fillAmount > 0.5f && bar.fillAmount < 0.7f) // its float numbers
                StartCoroutine(CoverInvItems());

            yield return new WaitForSeconds(0.03f);
        }
    }

    public IEnumerator CoverInvItems()
    {
        while (camera_inv.fillAmount > 0)
        {
            camera_inv.fillAmount -= 0.1f;
            lente_inv.fillAmount -= 0.1f;

            yield return new WaitForSeconds(0.03f);
        }
    }

    //public IEnumerator HandleBagBar()
    //{
    //    // mochila fechada
    //    if (bagbar.transform.localScale.x <= 0)
    //    {
    //        count = 0;
    //        while (bagbar.transform.localScale.x <= 1)
    //        {
    //            Debug.Log("openning");
    //            count += time;
    //            SetBarSize(count);
    //            yield return new WaitForSeconds(0.03f);
    //        }
    //    }
    //    else if (bagbar.transform.localScale.x >= 1) // mochila aberta
    //    {
    //        count = 1;

    //        while (bagbar.transform.localScale.x >= 0)
    //        {
    //            Debug.Log("closing");
    //            count -= time;
    //            SetBarSize(count);
    //            yield return new WaitForSeconds(0.03f);
    //        }
    //    }
    //}

    // bar size from 0 to 1
    public void SetBarSize(float normalizedSize)
    {
        bagbar.transform.localScale = new Vector3(normalizedSize, 1, 1);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagController : MonoBehaviour
{
    public GameObject spots, itemSpot, bag;

    float spacing;

    float itemWidth;

    int bagSize;

    GameObject[] bagItems;

    public Sprite[] ItemsSprites;
    public string[] ItemsDescriptions;

    private bool opened { get{ return spots.active; } }

    // Start is called before the first frame update
    void Start()
    {
        return;
        bag = gameObject;
        spacing = spots.GetComponentInChildren<HorizontalLayoutGroup>().spacing;
        itemWidth = itemSpot.GetComponent<RectTransform>().sizeDelta.x;
        bagSize = ItemsSprites.Length;

        bagItems = new GameObject[bagSize];

        for(int i = 0; i < bagSize; i++) {
            bagItems[i] = addItemToBag(ItemsSprites[i], ItemsDescriptions[i]);
        }

        //Remove standardItem and its space
        Destroy(itemSpot);
        itemSpot = bagItems[0];
        increaseSpotsWidth(- itemWidth - spacing);

        EnableItemByIndex(0);
        EnableItemByIndex(3);

        //spots.GetComponent<Image>().fillAmount = 0;
    }

    private void setSpotsWidth(float newWidth) {
        //GetComponentInChildren<HorizontalLayoutGroup>().
            GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newWidth);
    }

    private float getSpotsWidth() {
        //return GetComponentInChildren<HorizontalLayoutGroup>().
        return GetComponent<RectTransform>().sizeDelta.x;
    }

    private void increaseSpotsWidth(float deltaWidth) {
        setSpotsWidth(getSpotsWidth() + deltaWidth);
    }

    private GameObject addItemToBag(Sprite sprite, string description) {
        increaseSpotsWidth(itemWidth + spacing);
        GameObject newItem = Instantiate(itemSpot, spots.transform); //Make copy of standard object
        newItem.GetComponent<Image>().sprite = sprite;
        newItem.GetComponent<Image>().color = new Color32(255,255,255,50); //Leave image translucide (disabled)
        return newItem;
    }

    // public void ClearBag() {
    //     foreach(Transform child in spots.transform) {
    //         Destroy(child.gameObject);
    //         increaseSpotsWidth(- itemWidth - spacing);
    //     }
    // }

    public void EnableItemByIndex(int index) {
        bagItems[index].GetComponent<Image>().color = new Color32(255,255,255,255);
    }

    private void openBag() {
        spots.SetActive(true);
    }

    private void closeBag() {
        spots.SetActive(false);
    }


    public void OpenOrCloseBag() {
        if(opened) 
            closeBag();
        else    
            openBag();

    }

    // private IEnumerator RollBag() {
    //     while (img.fillAmount < 1)
    //     {
    //         Debug.Log("opening");
    //         img.fillAmount += 0.05f;

    //         //if ((img.fillAmount > 0.4f && img.fillAmount < 0.6f) && op == BAG) // its float numbers
    //             //StartCoroutine(ShowInvItems());

    //         yield return new WaitForSeconds(time);
    //     }
    // }

    // public void HandleBagBar()
    // {
    //     if (bar.fillAmount == 0)
    //     {
    //         StartCoroutine(FillImage(bar, BAG));
    //         //audioSource.PlayOneShot(openBagClip);
    //     }
    //     else if (bar.fillAmount == 1)
    //     {
    //         StartCoroutine(UnfillImage(bar, BAG));
    //         //audioSource.PlayOneShot(closeBagClip);
    //     }
    // }

    // public void HandleInfoBar()
    // {
    //     // if (info.fillAmount == 0)
    //     // {
    //     //     StartCoroutine(FillImage(info, INFO));
    //     //     ReadText("Informações ativadas");
    //     //     ReadText(infoMissionName.GetComponentInChildren<TextMeshProUGUI>().text);
    //     //     ReadText(infoMissionDescription.GetComponentInChildren<TextMeshProUGUI>().text);
    //     // }
    //     // else if (info.fillAmount == 1)
    //     // {
    //     //     StartCoroutine(UnfillImage(info, INFO));
    //     //     ReadText("Informações desativadas");
    //     // }
    // }

    // public IEnumerator FillImage(Image img, int op)
    // {
    //     while (img.fillAmount < 1)
    //     {
    //         Debug.Log("opening");
    //         img.fillAmount += 0.05f;

    //         //if ((img.fillAmount > 0.4f && img.fillAmount < 0.6f) && op == BAG) // its float numbers
    //             //StartCoroutine(ShowInvItems());

    //         yield return new WaitForSeconds(time);
    //     }

    //     if (op == BAG) ponteiraButton.fillAmount = 1;
    //     else if (op == INFO)
    //     {
    //         infoMissionDescription.SetActive(true);
    //         infoMissionName.SetActive(true);
    //         infoMissionIcon.SetActive(true);
    //     }
    // }

    // public IEnumerator UnfillImage(Image img, int op)
    // {
    //     if (op == BAG) ponteiraButton.fillAmount = 0;
    //     else if (op == INFO)
    //     {
    //         infoMissionDescription.SetActive(false);
    //         infoMissionName.SetActive(false);
    //         infoMissionIcon.SetActive(false);
    //     }

    //     while (img.fillAmount > 0)
    //     {
    //         Debug.Log("closing");

    //         img.fillAmount -= 0.05f;

    //         if ((img.fillAmount > 0.5f && img.fillAmount < 0.7f) && op == BAG) // its float numbers
    //             StartCoroutine(CoverInvItems());

    //         yield return new WaitForSeconds(time);
    //     }
    // } 

}

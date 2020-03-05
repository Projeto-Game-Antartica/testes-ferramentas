using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagController : MonoBehaviour
{
    public GameObject spots, itemSpot, bag, ponteira;

    float spacing;

    float itemWidth;

    int bagSize;

    GameObject[] bagItems;

    public Sprite[] ItemsSprites;
    public string[] ItemsDescriptions;

    public AudioSource audioSource;

    public AudioClip openBagClip;
    public AudioClip closeBagClip;

    private bool opened { get{ return spots.active; } }

    // Start is called before the first frame update
    void Start()
    {
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
        if(bagItems.Length > 0) //If bag not empty
            itemSpot = bagItems[0];
        increaseSpotsWidth(- itemWidth - spacing);

        //Ensure bag is closed when start
        closeBagImmediate();
    }

    private void setSpotsWidth(float newWidth) {
        GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newWidth);
    }

    private float getSpotsWidth() {
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

    public void EnableItemByIndex(int index) {
        bagItems[index].GetComponent<Image>().color = new Color32(255,255,255,255);
    }

    public void OpenBag() {
        if(!opened) {
            StartCoroutine(rollBag());
            audioSource.PlayOneShot(openBagClip);
        }
    }

    public void OpenOrClose() {
        if(!opened)
            OpenBag();
        else
            CloseBag();
    }

    public void CloseBag() {
        if(opened) {
            audioSource.PlayOneShot(closeBagClip);
            StartCoroutine(unrollBag());
        }
    }

    private void closeBagImmediate() {
        //Necessary for stating closed
        ponteira.SetActive(false);
        spots.GetComponent<Image>().fillAmount = 0;
        foreach(GameObject item in bagItems)
            item.SetActive(false);
        spots.SetActive(false); 
    }

    private IEnumerator rollBag() {
        spots.SetActive(true);

        Image img = spots.GetComponent<Image>();

        while (img.fillAmount < 1)
        {
            img.fillAmount += 0.05f;

            yield return new WaitForSeconds(0.02f);
        }

        foreach(GameObject item in bagItems)
            item.SetActive(true);

        ponteira.SetActive(true);
    }

    private IEnumerator unrollBag() {
        ponteira.SetActive(false);

        foreach(GameObject item in bagItems)
            item.SetActive(false);

        Image img = spots.GetComponent<Image>();

        while (img.fillAmount > 0)
        {
            img.fillAmount -= 0.05f;

            yield return new WaitForSeconds(0.02f);
        }

        spots.SetActive(false);   
    }
}

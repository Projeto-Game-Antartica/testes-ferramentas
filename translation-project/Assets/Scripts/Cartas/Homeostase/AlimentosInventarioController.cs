using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AlimentosInventarioController : MonoBehaviour {

    public Homeostase homeostase;
    public Button dislikeButton;
    private int alimentoIndex;
    public GameObject confirmPanel;
    private GameObject selectedObject;

    public bool initialized;

    //   // Use this for initialization
    void Start()
    {
        initialized = false;
    }

    //// Update is called once per frame
    //void Update () {
    //       //if (confirmPanel.activeSelf)
    //       //    GetComponent<Selectable>().interactable = false;
    //       //else
    //       //    GetComponent<Selectable>().interactable = true;
    //}

    //   public void OnSelect(BaseEventData eventData)
    //   {
    //       selectedObject = EventSystem.current.currentSelectedGameObject;

    //       if (selectedObject.GetComponent<Image>() != null)
    //       {
    //           //dislikeButton.interactable = true;

    //           //confirmPanel.SetActive(true);
    //           //confirmPanel.GetComponentsInChildren<Image>()[1].sprite = selectedObject.GetComponent<Image>().sprite;


    //           //// get the integer from game object name
    //           //alimentoIndex = int.Parse(Regex.Match(EventSystem.current.currentSelectedGameObject.name, @"\d+").Value);
    //           //Debug.Log(alimentoIndex);
    //       }
    //   }

    public void RemoverAlimento(Transform alimento)
    {
        Debug.Log(alimento.name);
        int index = int.Parse(Regex.Match(alimento.name, @"\d+").Value);

        homeostase.RemoverAlimentoCesta(index);

        confirmPanel.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AlimentosInventarioController : MonoBehaviour {

    public Homeostase homeostase;
    private int alimentoIndex;
    private GameObject selectedObject;

    public GameObject confirmPanel;
    public TMPro.TextMeshProUGUI confirmText;
    public Image confirmImage;
    public Button removeButton;

    public bool initialized;

    public void RemoverAlimento(Transform alimento)
    {
        Debug.Log(alimento.name);

        int index = int.Parse(Regex.Match(alimento.name, @"\d+").Value);

        Debug.Log(index);

        confirmPanel.SetActive(true);

        confirmText.text = "Tem certeza que deseja remover o (a) " + alimento.GetComponent<Image>().sprite.name + " da cesta?";
        Debug.Log(alimento.GetComponent<Image>().sprite.name);
        confirmImage.sprite = alimento.GetComponent<Image>().sprite;
        
        // add onClick event do removerButton at runtime
        removeButton.onClick.AddListener(() => { ConfirmRemover(index); });
    }

    public void ConfirmRemover(int index)
    {
        Debug.Log("confirmar remover index: " + index);
        homeostase.RemoverAlimentoCesta(index);

        confirmPanel.SetActive(false);

        removeButton.onClick.RemoveAllListeners();
    }
}
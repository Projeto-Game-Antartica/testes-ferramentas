using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AlimentosInventarioController : AbstractScreenReader, ISelectHandler {

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

        Debug.Log(confirmText.text);
        ReadText(confirmText.text);

        //Debug.Log(alimento.GetComponent<Image>().sprite.name);

        confirmImage.sprite = alimento.GetComponent<Image>().sprite;

        confirmPanel.GetComponentInChildren<Button>().Select();
        
        // add onClick event do removerButton at runtime
        removeButton.onClick.AddListener(() => { ConfirmRemover(index); });
    }

    public void ConfirmRemover(int index)
    {
        Debug.Log("confirmar remover index: " + index);
        homeostase.RemoverAlimentoCesta(index);

        Debug.Log("Alimento removido com sucesso.");
        ReadText("Alimento removido com sucesso.");

        confirmPanel.SetActive(false);

        removeButton.onClick.RemoveAllListeners();

        GetComponent<Selectable>().Select();
    }

    public void OnSelect(BaseEventData eventData)
    {
        string name = transform.parent.name;

        if (!name.Contains("alimentoItem"))
        {
            Debug.Log(name);
            ReadText(name);
        }
        else
        {
            Debug.Log("Item vazio");
            ReadText("Item vazio");
        }
    }
}
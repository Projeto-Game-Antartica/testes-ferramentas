using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HomeostaseVitoria : MonoBehaviour {

    public TMPro.TextMeshProUGUI titleText;

    public GameObject algodaoCard;
    public GameObject fleeceCard;

    public GameObject winImage;
    public GameObject loseImage;

    public Button likeButton;

    public MinijogosDicas minijogoDicas;

    public string initialHint;

    public string algodaoHint;
    public string fleeceHint;

    public GameObject instruction_interface;

    private GameObject clickedCard = null;

    // Use this for initialization
    void Start () {

        minijogoDicas.ShowIsolatedHint(initialHint);
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            instruction_interface.SetActive(true);
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            instruction_interface.SetActive(false);
        }
    }

    public void OnCardClick()
    {
        clickedCard = EventSystem.current.currentSelectedGameObject.gameObject;

        likeButton.interactable = true;

        if (clickedCard.name.Equals(algodaoCard.name))
        {
            titleText.text = "Blusa de algodão";
            minijogoDicas.ShowIsolatedHint(algodaoHint);
        }
        else if (clickedCard.name.Equals(fleeceCard.name))
        {
            titleText.text = "Blusa de fleece";
            minijogoDicas.ShowIsolatedHint(fleeceHint);
        }
    }

    public void OnLikeClick()
    {
        if (clickedCard.name.Equals(algodaoCard.name))
        {
            // wrong choice
            loseImage.SetActive(true);
        }
        else if (clickedCard.name.Equals(fleeceCard.name))
        {
            // correct choice
            winImage.SetActive(true);
        }

        StartCoroutine(ReturnToUshuaiaCoroutine());
    }

    public IEnumerator ReturnToUshuaiaCoroutine()
    {
        yield return new WaitForSeconds(7f);

        UnityEngine.SceneManagement.SceneManager.LoadScene(ScenesNames.M002CasaUshuaia);
    }
}

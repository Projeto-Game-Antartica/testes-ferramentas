using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class HomeostaseVitoria : AbstractScreenReader {

    public TMPro.TextMeshProUGUI titleText;

    public GameObject algodaoCard;
    public GameObject fleeceCard;

    public GameObject winImage;
    public GameObject loseImage;

    public GameObject confirmQuit;

    public AudioSource audioSource;

    public AudioClip victoryClip;
    public AudioClip loseClip;
    public AudioClip avisoClip;
    public AudioClip closeClip;

    public Button likeButton;
    public Button audioButton;

    public MinijogosDicas minijogoDicas;

    public LifeExpController lifeExpController;

    public string initialHint;

    public string algodaoHint;
    public string fleeceHint;

    public Image heartImage;
    public Image starImage;
    public Image antarticaImage;

    public GameObject instruction_interface;

    private GameObject clickedCard = null;

    private bool isOnLikeButton;

    // Use this for initialization
    void Start ()
    {
        heartImage.fillAmount = PlayerPrefs.GetFloat("MJHealthPoints");
        starImage.fillAmount = PlayerPrefs.GetFloat("MJExperience");
        antarticaImage.fillAmount = PlayerPrefs.GetFloat("MJAntartica");

        isOnLikeButton = true;
        minijogoDicas.ShowIsolatedHint(initialHint);

        Debug.Log(initialHint);
        ReadText(initialHint);
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(InputKeys.INSTRUCTIONS_KEY))
        {
            instruction_interface.SetActive(true);
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            if (instruction_interface.activeSelf)
            {
                audioSource.PlayOneShot(closeClip);
                instruction_interface.SetActive(false);
            }
            else
            {
                TryReturnToCasaUshuaia();
            }
        }

        if (Input.GetKeyDown(InputKeys.PARAMETERS_KEY))
        {
            lifeExpController.ReadHPandEXP();
        }

        if (Input.GetKeyDown(InputKeys.MJMENU_KEY))
        {
            if(!isOnLikeButton)
            {
                audioButton.Select();
                isOnLikeButton = true;
            }
            else
            {
                fleeceCard.GetComponent<Button>().Select();
                isOnLikeButton = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.F6))
        {
            likeButton.Select();
        }

        if (Input.GetKeyDown(InputKeys.AUDIODESCRICAO_KEY))
        {
            // audiodescricao
        }

        if (Input.GetKeyDown(InputKeys.REPEAT_KEY))
        {
            //ReadCard(cardIndex);
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

        likeButton.Select();
    }

    public void OnLikeClick()
    {
        if (clickedCard.name.Equals(algodaoCard.name))
        {
            // wrong choice
            StartCoroutine(EndGame(false));
        }
        else if (clickedCard.name.Equals(fleeceCard.name))
        {
            // correct choice
            StartCoroutine(EndGame(true));
        }
    }

    public IEnumerator EndGame(bool win)
    {
        if (win)
        {
            winImage.SetActive(true);

            PlayerPreferences.M002_Homeostase = true;

            audioSource.PlayOneShot(victoryClip);

            yield return new WaitWhile(() => audioSource.isPlaying);

            ReadText("Parabéns, você conseguiu mais alguns itens necessários para sua aventura na antártica!");

            lifeExpController.AddEXP(PlayerPreferences.XPwinPuzzle); // finalizou o minijogo
            lifeExpController.AddEXP(3*PlayerPreferences.XPwinItem); // ganhou o item

            // add the heart points in hp points
            lifeExpController.AddHP(PlayerPreferences.calculateMJExperiencePoints(heartImage.fillAmount));
            // add the antartica and star points to xp points
            lifeExpController.AddEXP(PlayerPreferences.calculateMJExperiencePoints(starImage.fillAmount, antarticaImage.fillAmount));
        }
        else
        {
            loseImage.SetActive(true);

            audioSource.PlayOneShot(loseClip);

            yield return new WaitWhile(() => audioSource.isPlaying);

            ReadText("Infelizmente você não conseguiu finalizar o minijogo com êxito. Tente novamente.");

            lifeExpController.AddEXP(PlayerPreferences.XPlosePuzzle); // jogou um minijogo
        }

        StartCoroutine(ReturnToCasaUshuaiaCoroutine());
    }

    public void ReturnToUshuaia()
    {
        SceneManager.LoadScene(ScenesNames.M002CasaUshuaia);
    }

    public IEnumerator ReturnToCasaUshuaiaCoroutine()
    {
        yield return new WaitForSeconds(4f);

        SceneManager.LoadScene(ScenesNames.M002CasaUshuaia);
    }

    public void TryReturnToCasaUshuaia()
    {
        audioSource.PlayOneShot(avisoClip);

        confirmQuit.SetActive(true);

        ReadText(ReadableTexts.instance.GetReadableText(ReadableTexts.key_gameplay_aviso_botoes, LocalizationManager.instance.GetLozalization()));

        ReadText(confirmQuit.GetComponentInChildren<TMPro.TextMeshProUGUI>().text);

        confirmQuit.GetComponentInChildren<Button>().Select();
    }
}

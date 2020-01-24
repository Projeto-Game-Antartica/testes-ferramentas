using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CofreAnimation : AbstractScreenReader
{
    public AudioSource audioSource;
    public AudioClip cofreClip;

    public Animator animator;
    public SpriteRenderer ticketSprite;
    public CircleCollider2D circleCollider2D;

    public Image ticketPt1;
    public Image ticketPt2;
    public Button closeButton;
    public GameObject textPanel;

    private bool opened;

    int i = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        opened = false;

        animator.SetBool("open", false);
    }

    // Update is called once per frame
    void Update()
    {
        // condition to open the locker
        if (PlayerPreferences.M002_Homeostase && !opened)
        {
            StartCoroutine(OpenLocker());
        }
    }

    private IEnumerator OpenLocker()
    {
        opened = true;

        animator.SetBool("open", true);

        audioSource.PlayOneShot(cofreClip);

        yield return new WaitForSeconds(1);

        ticketSprite.enabled = true;
        circleCollider2D.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerPrefs.SetInt("M002_Ticketpt2", 1);

        closeButton.gameObject.SetActive(true);
        closeButton.Select();

        if (PlayerPrefs.GetInt("M002_Ticketpt1") == 1)
            ticketPt1.gameObject.SetActive(true);

        ReadText(textPanel.GetComponentInChildren<TMPro.TextMeshProUGUI>().text);

        ticketPt2.gameObject.SetActive(true);
        ticketSprite.enabled = false;
        textPanel.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ticketPt2.gameObject.SetActive(false);
        ticketPt1.gameObject.SetActive(false);
        closeButton.gameObject.SetActive(false);
        textPanel.SetActive(false);
    }
}

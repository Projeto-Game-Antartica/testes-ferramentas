using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CofreAnimation : MonoBehaviour
{
    public Animator animator;
    public SpriteRenderer ticketSprite;
    public CircleCollider2D circleCollider2D;

    public Image ticketPt1;
    public Image ticketPt2;
    public Button closeButton;

    int i = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        animator.SetBool("open", false);
    }

    // Update is called once per frame
    void Update()
    {
        // condition to open the locker
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(OpenLocker());
        }
    }

    private IEnumerator OpenLocker()
    {
        animator.SetBool("open", true);

        yield return new WaitForSeconds(1);

        ticketSprite.enabled = true;
        circleCollider2D.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        closeButton.gameObject.SetActive(true);
        closeButton.Select();

        if (PlayerPrefs.GetInt("Ticketpt1") == 1)
            ticketPt1.gameObject.SetActive(true);

        ticketPt2.gameObject.SetActive(true);
        ticketSprite.enabled = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ticketPt2.gameObject.SetActive(false);
        ticketPt1.gameObject.SetActive(false);
        closeButton.gameObject.SetActive(false);
    }
}

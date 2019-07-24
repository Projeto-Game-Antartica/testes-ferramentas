using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Minijogos_dicas : MonoBehaviour {

    public TextMeshProUGUI targetText;
    public string[] hints;

    private float waitTime = 5;

    public readonly float time = 5.0f;
    public readonly float repeatRate = 10.0f;

    public GameObject dicas;

    public void StartHints()
    {
        StartCoroutine(ShowHints());
    }

    private IEnumerator ShowHints()
    {
        if (!dicas.activeSelf)
        {
            // select a random number between 0 and hints.lenght-1
            int index = Random.Range(0, hints.Length - 1);

            dicas.SetActive(true);
            Debug.Log("showing hint number " + index);
            targetText.text = hints[index];

            yield return new WaitForSeconds(waitTime);

            dicas.SetActive(false);
            targetText.text = string.Empty;
        }
        else
            yield return null;
    }

    public string[] GetHints()
    {
        return hints;
    }
}

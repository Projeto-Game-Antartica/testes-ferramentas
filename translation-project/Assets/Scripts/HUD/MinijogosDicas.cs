using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MinijogosDicas : AbstractScreenReader {

    public TextMeshProUGUI targetText;
    public string[] hints;

    private float waitTime = 5;

    public readonly float time = 10.0f;
    public readonly float repeatRate = 10.0f;

    public bool isRandom;

    public GameObject dicas;

    public void StartHints()
    {
        if (isRandom)
            StartCoroutine(ShowHints());
    }

    private IEnumerator ShowHints()
    {
        if (!dicas.activeSelf)
        {
            // select a random number between 0 and hints.lenght-1
            int index = Random.Range(0, hints.Length - 1);

            dicas.SetActive(true);
            //Debug.Log("showing hint number " + index);
            ReadText("Nova dica: " + hints[index]);
            //Debug.Log("Nova dica: " + hints[index]);
            targetText.text = hints[index];

            yield return new WaitForSeconds(waitTime);

            dicas.SetActive(false);
            targetText.text = string.Empty;
        }
        else
            yield return null;
    }

    public void SetHintByIndex(int index)
    {
        if (!dicas.activeSelf)
            dicas.SetActive(true);

        Debug.Log("showing hint number " + index);

        ReadText("Nova dica: " + hints[index]);
        Debug.Log("Nova dica: " + hints[index]);
        targetText.text = hints[index];

        if (index == -1)
            dicas.SetActive(false);
    }

    public string[] GetHints()
    {
        return hints;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class MinijogosDicas : AbstractScreenReader {

    public TextMeshProUGUI targetText;
    public string[] hints;

    private int[] hintIndexes;
    private int index = 0;
    
    private float waitTime = 5;

    public readonly float time = 10.0f;
    public readonly float repeatRate = 10.0f;

    public bool isRandom;

    public GameObject dicas;

    private void Start()
    {
        hintIndexes = new int[hints.Length];

        for (int i = 0; i < hints.Length; i++)
            hintIndexes[i] = i;
        
        // randomize the indexes
        System.Random r = new System.Random();
        hintIndexes = hintIndexes.OrderBy(x => r.Next()).ToArray();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
        {
            if (dicas.activeSelf)
                ReadText("Dica: " + targetText.text);
        }
    }
    
    public void ShowHint()
    {
        dicas.SetActive(true);

        targetText.text = hints[hintIndexes[index]];
        index++;

        if (index == hints.Length)
            index = 0;

        ReadText("Dica: " + targetText.text);

        // lose exp points
        // TO DO
    }

    
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

            Debug.Log(hints.Length);

            dicas.SetActive(true);
            //Debug.Log("showing hint number " + index);
            ReadText(hints[index]);
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
        //Debug.Log("Nova dica: " + hints[index]);
        targetText.text = hints[index];

        if (index == -1)
            dicas.SetActive(false);
    }

    public void ReadCurrentHint()
    {
        if (dicas.activeSelf)
        {
            ReadText(targetText.text);
        }
    }

    public string[] GetHints()
    {
        return hints;
    }

    public void ShowIsolatedHint(string hint)
    {
        dicas.SetActive(true);
        targetText.text = hint;
    }

    public void SupressDicas()
    {
        dicas.SetActive(false);
    }
}

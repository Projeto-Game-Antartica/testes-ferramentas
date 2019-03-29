using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FotoidentificacaoController : MonoBehaviour {

    /*
     * Questions
     */
    string[] questions = new string[5] 
    {
        "1. Qual baleia abaixo tem a borda igual a da baleia fotografada?",
        "2. Qual baleia abaixo tem a ponta igual a da baleia fotografada?",
        "3. Qual baleia abaixo tem o entalhe igual a da baleia fotografada?",
        "4. Qual baleia abaixo tem manchas iguais a da baleia fotografada?",
        "5. Identifique a porcentagem de pigmentação branca da baleia fotografada."
    };

    /*
     * Game Parameters
     */
    private float round;
    private bool isCorrect = true;
    private int index = 0;
    public GameObject option1;
    public GameObject option2;
    public GameObject option3;
    public Text questionText;

    public WhaleController whaleController;

    private void Start()
    {
        
    }

    public void RoundSettings(int index)
    {
        if (index < 5)
        {
            questionText.text = questions[index];
        }
        else
        {
            // TODO
        }
    }

    public void CheckAnswer(GameObject option)
    {
        if(isCorrect)
        {
            index++;
            RoundSettings(index);
        }
        else
        {
            ChangeBackgroundColor(option, Color.red);
        }
    }
    
    public void ChangeBackgroundColor(GameObject option, Color color)
    {
        option.GetComponent<Image>().color = color;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDMJController : AbstractScreenReader
{
    public GameObject acessoTeclado;
    public TMPro.TextMeshProUGUI descricaoText;

    private void Update()
    {
        if(Input.GetKeyDown(InputKeys.REPEAT_KEY))
        {
            acessoTeclado.SetActive(true);
            ReadText(descricaoText.text);
            acessoTeclado.GetComponentInChildren<Button>().Select();
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (acessoTeclado.activeSelf)
                acessoTeclado.SetActive(false);
        }
    }

}

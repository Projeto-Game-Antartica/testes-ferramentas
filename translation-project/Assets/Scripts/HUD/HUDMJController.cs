using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDMJController : AbstractScreenReader
{
    public GameObject acessoTeclado;
    public TMPro.TextMeshProUGUI descricaoText;

    public TMPro.TextMeshProUGUI playerName;

    private void Start()
    {
        playerName.text = PlayerPreferences.PlayerName.ToUpper();
    }

    private void Update()
    {
        if(Input.GetKeyDown(InputKeys.ACESSOTECLADO_KEY))
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

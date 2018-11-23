using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LocalizedTextMeshPRO : MonoBehaviour {

    public string key;

	// Awake para que seja traduzido e escrito antes da leitura da tela
	void Awake () {
        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();
        text.text = LocalizationManager.instance.GetLocalizedValue(key);
	}
}

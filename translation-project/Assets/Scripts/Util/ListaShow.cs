using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListaShow : AbstractScreenReader
{
    GameObject canvasGo;
    bool allowHide = false;

    // Start is called before the first frame update
    void Start()
    {
        canvasGo = transform.gameObject;
        allowHide = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            if (allowHide) Hide();
        }
    }

    public void Hide()
    {
        canvasGo.SetActive(false);
        allowHide = false;
    }
}

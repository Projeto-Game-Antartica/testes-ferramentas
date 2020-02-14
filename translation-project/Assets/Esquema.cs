using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Esquema : MonoBehaviour
{
    public GameObject esquemaObject;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Teste() {
        Debug.Log("TESTE_ESQUEMA");
        esquemaObject.SetActive(true);
    }
}

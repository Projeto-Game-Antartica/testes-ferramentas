using UnityEngine.UI;
using UnityEngine;

public class AlimentosCestaController : MonoBehaviour {
    
    // Update is called once per frame
    void Update () {
        if (!GetComponent<Image>().enabled)
            Destroy(gameObject);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DavyKager;

public class TolkUtil : MonoBehaviour {

    public static void Load()
    {
        Debug.Log("Inicializando...");
        Tolk.Load();
        Debug.Log("Procurando por leitores de tela no dispositivo...");

        string name = Tolk.DetectScreenReader();

        if (name == null) Tolk.TrySAPI(true);

        if (name != null)
        {
            Debug.Log("O leitor ativo é: " + name);
        }
        else
        {
            Debug.Log("Nenhum leitor está rodando");
        }
    }

    public static void Unload()
    {
       if (Tolk.IsLoaded())
       {
            Tolk.Unload();
            Debug.Log("Tolk unloaded.");
       }
       else
       {
            Debug.Log("Tolk isnt running.");
       }
    }
}

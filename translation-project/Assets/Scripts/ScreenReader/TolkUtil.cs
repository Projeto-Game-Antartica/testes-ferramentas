using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DavyKager;

public class TolkUtil : AbstractScreenReader {

    public static void Load()
    {
        
        if (!Tolk.IsLoaded())
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
                // caso não tenha SAPI
                Debug.Log("Nenhum leitor está rodando");
            }
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

    public static void Speak(string text)
    {
        if (!Tolk.IsSpeaking()) Tolk.Speak(text);
    }

    public static void SpeakAnyway(string text)
    {
        Tolk.Speak(text);
    }

    public static void Instructions()
    {
        const string instructions = "Utilize a tecla F1 para repetir as instruções à qualquer momento.";
        ReadText(instructions);
    }
}

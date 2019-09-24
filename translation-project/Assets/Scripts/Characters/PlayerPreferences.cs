using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPreferences {

    // M004
    public static bool M004_TeiaAlimentar = false;
    public static bool M004_FotoIdentificacao = false;
    public static bool M004_Memoria = false;

    public static bool finishedAllM004Games()
    {
        return M004_FotoIdentificacao && M004_Memoria && M004_Memoria;
    }
}   

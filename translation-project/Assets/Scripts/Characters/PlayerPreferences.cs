using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPreferences {

    // M004
    public static bool M004_TeiaAlimentar = false;
    public static bool M004_FotoIdentificacao = false;
    public static bool M004_Memoria = false;

    // M009
    public static bool M009_Memoria = false;
    public static bool M009_Eras = false;
    public static bool M009_Itens = false;

    public static bool finishedAllM004Games()
    {
        return M004_FotoIdentificacao && M004_Memoria && M004_TeiaAlimentar;
    }

    public static bool finishedAllM009Games()
    {
        return M009_Memoria && M009_Eras && M009_Itens;
    }
}   

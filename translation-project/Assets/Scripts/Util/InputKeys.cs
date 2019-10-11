using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputKeys : MonoBehaviour {

    /*
     * Classe para mapear as teclas de atalho do jogo.
     * Para utilizar nos scripts fazer a chamada da seguinte maneira:
     * if (InputKeys.INVENTORY_KEY) equivale a if (Input.GetKeyDown(Keycode.I))
     * 
     */

    // ### GETKEYDOWN ###

    // Acessar Inventário Logbook (HUD)
    public static bool INVENTORY_KEY = Input.GetKeyDown(KeyCode.I);

    // Quest (acessar nome da missão)
    public static bool QUEST_KEY = Input.GetKeyDown(KeyCode.Q);

    //Acesso às instruções 
    public static bool INSTRUCTIONS_KEY = Input.GetKeyDown(KeyCode.F1);

    // Acesso aos parâmetros: coração, estrela, meio ambiente; pontos e tentativas
    public static bool PARAMETERS_KEY = Input.GetKeyDown(KeyCode.A);

    // Pausar MJ e direciona para o Menu
    public static bool MJMENU_KEY = Input.GetKeyDown(KeyCode.P);
}

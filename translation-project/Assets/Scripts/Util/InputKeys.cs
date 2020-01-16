using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputKeys : MonoBehaviour {

    /*
     * Classe para mapear as teclas de atalho do jogo.
     * Para utilizar nos scripts fazer a chamada da seguinte maneira:
     * if (Input.GetKeyDown(InputKeys.INVENTORY_KEY)) equivale a if (Input.GetKeyDown(Keycode.I))
     * 
     */

    // ### GETKEYDOWN ###

    // Acessar Inventário Logbook (HUD)
    public static KeyCode INVENTORY_KEY = KeyCode.I;

    // Quest (acessar nome da missão)
    public static KeyCode QUEST_KEY = KeyCode.Q;

    //Acesso às instruções 
    public static KeyCode INSTRUCTIONS_KEY = KeyCode.F1;

    // Acesso aos parâmetros: coração, estrela, meio ambiente; pontos e tentativas
    public static KeyCode PARAMETERS_KEY = KeyCode.A;

    // Pausar MJ e direciona para o Menu
    public static KeyCode MJMENU_KEY = KeyCode.P;

    // repetir acesso via teclado
    public static KeyCode REPEAT_KEY = KeyCode.F2;

    // repetir audiodescricao
    public static KeyCode AUDIODESCRICAO_KEY = KeyCode.F3;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPreferences : MonoBehaviour {

    // M004 - Puzzles
    public static bool M004_TeiaAlimentar = false;
    public static bool M004_FotoIdentificacao = false;
    public static bool M004_Memoria = false;

    // M002 - Puzzles
    public static bool M002_Homeostase = false;
    public static bool M002_Regras = false;
    public static bool M002_Pinguim = false;
    public static bool M002_ProcessoPesquisa = false;

    // M009
    public static bool M009_Memoria = false;

    // puzzles points
    public static float XPwinPuzzle = 0.004f;
    public static float XPwinItem = 0.00004f;
    public static float XPlosePuzzle = -0.002f;
    public static float XPwrongTry = -0.00004f;

    // mission points
    public static float XPwinMission = 0.006f;
    public static float HPwinMission = 0.006f;

    private void Start()
    {
        // set the parameter to show the instruction interface when loading the game
        PlayerPrefs.SetInt("InstructionInterface", 0);
        
        // set the saved position int to 0
        PlayerPrefs.SetInt("Saved", 0);

        // ingame player preferences
        PlayerPrefs.SetFloat("Experience", 0f);
        PlayerPrefs.SetFloat("HealthPoints", 1f);
        
        // set the dialogues to not read (0)
        // when the dialogue is read (1), the balloon change its color to green
        
        // M004
        PlayerPrefs.SetInt("M004_Mentor1_Dialogue", 0);
        PlayerPrefs.SetInt("M004_Mentor3_Dialogue", 0);
        // M002
        PlayerPrefs.SetInt("M002_Mentor1_Dialogue2", 0);
        PlayerPrefs.SetInt("M002_Mentor2_Dialogue2", 0);
        PlayerPrefs.SetInt("M002_Mentor3_Dialogue1", 0);
        PlayerPrefs.SetInt("M002_Mentor4_Dialogue1", 0);
        PlayerPrefs.SetInt("M002_Ticketpt1", 0);
        PlayerPrefs.SetInt("M002_Ticketpt2", 0);
        PlayerPrefs.SetInt("M002_Ticketpt3", 0);
    }

    public static bool finishedAllM004Games()
    {
        return M004_FotoIdentificacao && M004_Memoria && M004_TeiaAlimentar;
    }
}   

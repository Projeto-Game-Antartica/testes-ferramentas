using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPreferences : MonoBehaviour {

    public static string PlayerName = "";
    
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
    public static bool M009_Eras = false;
    public static bool M009_Itens = false;

    // M010
    public static bool M010_Amostras = false;
    public static bool M010_Tipos = false;
    public static bool M010_Mentor0_Talked = false;
    public static bool M010_Mentor3_Talked = false;
    public static bool M010_Desafio_Done = false;
    public static bool M010_Initialized = false;
  
    // puzzles points
    public static float XPwinPuzzle = 0.004f;
    public static float XPwinItem = 0.00004f;
    public static float XPlosePuzzle = -0.002f;
    public static float XPwrongTry = -0.00004f;

    // mission points
    public static float XPwinMission = 0.006f;
    public static float HPwinMission = 0.006f;

    private int firstRun = 0;

    private void Start()
    {
        //firstRun = PlayerPrefs.GetInt("firstRun", 0);
        //LoadAndSetPlayerPreferences(firstRun);
        LoadAndSetPlayerPreferences();
    }

    private void LoadAndSetPlayerPreferences()
    {
        // first time playing the game, initialize with default values
        //if(firstRun == 0)
        //{
            //firstRun = 1;
            
            // set the parameter to show the instruction interface when loading the game
            PlayerPrefs.SetInt("InstructionInterface", 0);

            PlayerPrefs.SetInt("UshuaiaMap", 0);
            PlayerPrefs.SetInt("CasaUshuaiaMap", 0);
            PlayerPrefs.SetInt("NavioMap", 0);

            // set the saved position int to 0
            PlayerPrefs.SetInt("Saved", 0);

            // volume settings
            PlayerPrefs.SetFloat("MusicVolume", 1f);

            // ingame player preferences
            PlayerPrefs.SetFloat("Experience", 0f);
            PlayerPrefs.SetFloat("HealthPoints", 1f);

            // initial position of each scenario starts unsaved
            PlayerPrefs.SetInt("Saved_M002", 0);
            PlayerPrefs.SetInt("Saved_M002_Casinha", 0);
            PlayerPrefs.SetInt("Saved_M004", 0);
            PlayerPrefs.SetInt("Saved_M009", 0);

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

            // M009
            PlayerPrefs.SetInt("M009_Mentor0_Dialogue1", 0);
            PlayerPrefs.SetInt("M009_Mentor3_Dialogue2", 0);
        //}
    }

    public static bool finishedAllM004Games()
    {
        return M004_FotoIdentificacao && M004_Memoria && M004_TeiaAlimentar;
    }
  
    public static bool finishedAllM009Games()
    {
        return M009_Memoria && M009_Eras && M009_Itens;
    }
  
    public static float calculateMJExperiencePoints(float expPoints, float antarticaPoints)
    {
        return (0.6f * expPoints + 0.4f * antarticaPoints) / 5000f;
    }

    public static float calculateMJExperiencePoints(float heartPoints)
    {
        return heartPoints / 5000f;
    }
}   

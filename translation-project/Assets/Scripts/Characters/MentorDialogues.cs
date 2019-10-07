﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MentorDialogues {

    /*  
     * Pattern of dialogues names on VIDE
     * Format: MISSIONNUMBER_MENTOR(NUMEROMENTOR)_DIALOGUE(NUMERODIALOGUE)
     */
    public const string sufixo = "_Dialogue";
    public const string prefixom002 = "M002_Mentor";
    public const string prefixom004 = "M004_Mentor";
    
    /* ####### */ 

    /*
     * M004 - Baleias 
     */
    public static string[] M004_Mentor0 =
    {
        prefixom004 + "0" + sufixo
    };
    public static string[] M004_Mentor1 =
    {
        prefixom004 + "1" + sufixo
    };
    public static string[] M004_Mentor2 = 
    {
        prefixom004 + "2" + sufixo
    };
    public static string[] M004_Mentor3 = 
    {
        prefixom004 + "3" + sufixo
    };
    public static string[] M004_Mentor4 = 
    {
        prefixom004 + "4" + sufixo
    };

    /*
    * M002 - Itens de Viagem
    */

    public static string[] M002_Mentor0 =
    {
        
    };
    public static string[] M002_Mentor1 =
    {
        prefixom002 + "1" + sufixo + "1", prefixom002 + "1" + sufixo + "2", prefixom002 + "1" + sufixo + "3",
        prefixom002 + "1" + sufixo + "4", prefixom002 + "1" + sufixo + "5"
    };
    public static string[] M002_Mentor2 =
    {
        prefixom002 + "2" + sufixo + "1", prefixom002 + "2" + sufixo + "2", prefixom002 + "2" + sufixo + "3",
        prefixom002 + "2" + sufixo + "4", prefixom002 + "2" + sufixo + "5"
    };
    public static string[] M002_Mentor3 =
    {
        prefixom002 + "3" + sufixo + "1", prefixom002 + "3" + sufixo + "2", prefixom002 + "3" + sufixo + "3",
        prefixom002 + "3" + sufixo + "4", prefixom002 + "3" + sufixo + "5"
    };
    public static string[] M002_Mentor4 =
    {
        prefixom002 + "4" + sufixo + "1", prefixom002 + "4" + sufixo + "2", prefixom002 + "4" + sufixo + "3",
        prefixom002 + "4" + sufixo + "4", prefixom002 + "4" + sufixo + "5"
    };

    /*
     * return the dialogue from mentor in mission with index in range 0 to vector.length
     */
    public static string GetDialogue(string mission, string mentor, int index)
    {
        switch (mission)
        {
            case "M002":
                switch (mentor)
                {
                    case "Mentor0":
                        return M002_Mentor0[index];
                    case "Mentor1":
                        return M002_Mentor1[index];
                    case "Mentor2":
                        return M002_Mentor2[index];
                    case "Mentor3":
                        return M002_Mentor3[index];
                    case "Mentor4":
                        return M002_Mentor4[index];
                    default:
                        return "Check mentor name.";
                }
            case "M004":
                switch (mentor)
                {
                    case "Mentor0":
                        return M004_Mentor0[index];
                    case "Mentor1":
                        return M004_Mentor1[index];
                    case "Mentor2":
                        return M004_Mentor2[index];
                    case "Mentor3":
                        return M004_Mentor3[index];
                    case "Mentor4":
                        return M004_Mentor4[index];
                    default:
                        return "Check mentor name.";
                }
            default:
                return "Check mission name.";
        }
    }

    // get the length of vector according to the mentor and mission
    public static int GetVectorLenght(string mission, string mentor)
    {
        switch (mission)
        {
            case "M002":
                switch (mentor)
                {
                    case "Mentor0":
                        return M002_Mentor0.Length;
                    case "Mentor1":
                        return M002_Mentor1.Length;
                    case "Mentor2":
                        return M002_Mentor2.Length;
                    case "Mentor3":
                        return M002_Mentor3.Length;
                    case "Mentor4":
                        return M002_Mentor4.Length;
                    default:
                        return -1;
                }
            case "M004":
                switch (mentor)
                {
                    case "Mentor0":
                        return M004_Mentor0.Length;
                    case "Mentor1":
                        return M004_Mentor1.Length;
                    case "Mentor2":
                        return M004_Mentor2.Length;
                    case "Mentor3":
                        return M004_Mentor3.Length;
                    case "Mentor4":
                        return M004_Mentor4.Length;
                    default:
                        return -1;
                }
            default:
                return -1;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMentorDialogues {
    // control the random dialogues
    public const string sufixo = "_MENTOR";

    /* M004 - Baleias 
     * Format: M004_RANDOM(NUMERODIALOGO)_MENTOR(NUMEROMENTOR)
     */

    public const string prefixom004 = "M004_RANDOM";

    public static string[] M004_Mentor0 = 
    {
        prefixom004 + "19" + sufixo + "0"
    };
    public static string[] M004_Mentor1 = 
    {
        prefixom004 + "1" + sufixo + "1", prefixom004 + "5" + sufixo + "1", prefixom004 + "9"+ sufixo + "1", prefixom004 + "13"+ sufixo + "1",
        prefixom004 + "17"+ sufixo + "1" //, prefixom004 + "21"+ sufixo + "1"
    };
    public static string[] M004_Mentor2 = 
    {
        prefixom004 + "2"+ sufixo + "2", prefixom004 + "6"+ sufixo + "2", prefixom004 + "10"+ sufixo + "2", prefixom004 + "14"+ sufixo + "2",
        prefixom004 + "18"+ sufixo + "2"
    };
    public static string[] M004_Mentor3 = 
    {
        prefixom004 +"3"+ sufixo + "3", prefixom004 +"7"+ sufixo + "3", prefixom004 + "11"+ sufixo + "3", prefixom004 + "15"+ sufixo + "3"
    };
    public static string[] M004_Mentor4 = 
    {
        prefixom004 +"4"+ sufixo + "4", prefixom004 + "8"+ sufixo + "4", prefixom004 + "12"+ sufixo + "4", prefixom004 + "16"+ sufixo + "4"
    };

    /*
     * return the dialogue from mentor with index in range 0 to vector.length
     */
    public static string GetRandomDialogue(string mentor, int index)
    {
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
    }

    // get the length of vector according to the mentor
    public static int GetVectorLenght(string mentor)
    {
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
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parameters {

    public const bool READ_TEXT_DEBUG_MODE = true;

    // GAME VERSION
    public static string VERSION = "v 1.0";
    
    // bool for enabling/disabling accessibility functions
    public static bool ACCESSIBILITY;
    public static bool HIGH_CONTRAST;
    public static bool BOLD;
    public static bool BUTTONCONTRAST;

    /* ## PHOTOIDENTIFICATION MISSION ## */
    // readonly parameters for camera limits
    public const float RIGHT_LIMIT    = 494f;
    public const float LEFT_LIMIT     = -494f;
    public const float UP_LIMIT       = 228f;
    public const float DOWN_LIMIT     = -228f;
    public const float Z_POSITION     = -20f;
    public const float MAX_ORTHOSIZE  = 200f;
    public const float MIN_ORTHOSIZE  = 100f;
    public const float ZOOM_SPEED     = 30f;
    
    // limits of the camera to check
    public static bool RIGHT_BORDER;
    public static bool LEFT_BORDER;
    public static bool UP_BORDER;
    public static bool DOWN_BORDER;

    // whale id
    public const int MIN_ID = 1;
    public const int MAX_ID = 9;
    public static int WHALE_ID;
    public static bool ISWHALEIDENTIFIED = false;

    // is whale on the camera focus?
    public static bool ISWHALEONCAMERA = false;

    // readonly parameters for photoidentification proccess
    public const int PIGMENTACAO = 0;
    public const int MANCHAS = 1;
    public const int RISCOS = 2;
    public const int MARCAS = 3;
    public const int BORDA = 4;
    public const int PONTAS = 5;
    public const int ENTALHE = 6;
    public static bool ISPIGMENTACAODONE = false;
    public static bool ISMANCHASDONE     = false;
    public static bool ISRISCOSDONE      = false;
    public static bool ISMARCASDONE      = false;
    public static bool ISBORDADONE       = false;
    public static bool ISPONTADONE       = false;
    public static bool ISENTALHEDONE     = false;

    /* #### */

    /* ## MEMORY GAME ## */
    public static int MEMORY_ROUNDINDEX = 0;
    /* #### */

    /* ## URLs ## */
    public const string DIALOGUE_PATH = @"http://acessivel.ufabc.edu.br/antartica/videos_antartica/";
    public const string VP8_TYPE = ".vp8";
}

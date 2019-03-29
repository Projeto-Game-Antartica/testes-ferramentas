using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parameters {

    // bool for enabling/disabling accessibility functions
    public static bool ACCESSIBILITY;
    

    /* ## PHOTOIDENTIFICATION MISSION ## */
    // readonly parameters for camera limits
    public static readonly float RIGHT_LIMIT    = 494f;
    public static readonly float LEFT_LIMIT     = -494f;
    public static readonly float UP_LIMIT       = 228f;
    public static readonly float DOWN_LIMIT     = -228f;
    public static readonly float Z_POSITION     = -20f;
    public static readonly float MAX_ORTHOSIZE  = 200f;
    public static readonly float MIN_ORTHOSIZE  = 170f;
    public static readonly float ZOOM_SPEED     = 30f;
    
    // limits of the camera to check
    public static bool RIGHT_BORDER;
    public static bool LEFT_BORDER;
    public static bool UP_BORDER;
    public static bool DOWN_BORDER;
    /* #### */


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parameters {

    // readonly parameters for camera limits
    public static readonly float RIGHT_LIMIT    = 247f;
    public static readonly float LEFT_LIMIT     = -247f;
    public static readonly float UP_LIMIT       = 113f;
    public static readonly float DOWN_LIMIT     = -113f;
    public static readonly float Z_POSITION     = -20f;
    public static readonly float MAX_ORTHOSIZE  = 100f;
    public static readonly float MIN_ORTHOSIZE  = 80f;

    // bool for enabling/disabling accessibility functions
    public static bool ACCESSIBILITY;

    // limits of the camera to check if got passed
    public static bool RIGHT_BORDER;
    public static bool LEFT_BORDER;
    public static bool UP_BORDER;
    public static bool DOWN_BORDER;
}

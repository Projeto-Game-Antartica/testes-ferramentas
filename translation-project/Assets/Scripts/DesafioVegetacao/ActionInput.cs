using UnityEngine;

public class ActionInput : Input {
    public static bool CaptureKey = true;

    public static bool GetKeyDown(KeyCode buttonName) {
        return CaptureKey && Input.GetKeyDown(buttonName);
    }

    public static bool GetKey(KeyCode buttonName) {
        return CaptureKey && Input.GetKey(buttonName);
    }
}
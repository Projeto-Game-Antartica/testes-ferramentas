using UnityEngine;
using UnityEngine.Events;


public class OkDialog : MonoBehaviour
{
    UnityAction okDialogCallback;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Return)) {
            OnOkDialogClick();
            Hide();
        }
    }

    public void Show(string message, UnityAction onOkClick) {
        okDialogCallback = onOkClick;
        gameObject.GetComponentsInChildren<TMPro.TextMeshProUGUI>()[0].text = message;
        gameObject.SetActive(true);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }

    public void OnOkDialogClick() {
        if(okDialogCallback != null)
            okDialogCallback();
    }

    public void Show(string message) {
         Show(message, null);
    }

    // public void ShowOkDialog(string message, Action onOkClick) {
    //     okDialogCallback = onOkClick;
    //     OkDialog.GetComponentsInChildren<TMPro.TextMeshProUGUI>()[0].text = message;
    //     OkDialog.SetActive(true);
    // }
}

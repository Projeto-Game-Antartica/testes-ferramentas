using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FossilImages : MonoBehaviour {

    public Image panelContentImage;

    public Image WinImage;
    public Image LoseImage;

    public void SetPhotographedWhaleImage(string image_path)
    {
        if (panelContentImage != null)
            panelContentImage.sprite = Resources.Load<Sprite>(image_path);

        if (WinImage != null)
            WinImage.sprite = Resources.Load<Sprite>(image_path);

        if (LoseImage != null)
            LoseImage.sprite = Resources.Load<Sprite>(image_path);


    }
}

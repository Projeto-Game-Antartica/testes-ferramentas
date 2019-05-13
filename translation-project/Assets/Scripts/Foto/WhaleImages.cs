using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WhaleImages : MonoBehaviour {

    public Image panelContentImage;
    public Image panelFotoidentificacaoImage;
    public Image panelWhalesCatalogImage;

    public void SetPhotographedWhaleImage(string image_path)
    {
        panelContentImage.sprite = Resources.Load<Sprite>(image_path);
        panelFotoidentificacaoImage.sprite = Resources.Load<Sprite>(image_path);
        panelWhalesCatalogImage.sprite = Resources.Load<Sprite>(image_path);
    }
}

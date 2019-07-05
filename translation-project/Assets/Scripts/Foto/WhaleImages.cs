using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WhaleImages : MonoBehaviour {

    public Image panelContentImage;
    public Image panelContentAumentadoImage;
    public Image panelFotoidentificacaoImage;
    public Image panelFotoidentificacaoAumentadoImage;
    public Image panelWhalesCatalogImage;

    public void SetPhotographedWhaleImage(string image_path)
    {
        if (panelContentImage != null)
            panelContentImage.sprite = Resources.Load<Sprite>(image_path);
        if (panelContentAumentadoImage != null)
            panelContentAumentadoImage.sprite = Resources.Load<Sprite>(image_path);
        if (panelFotoidentificacaoImage != null)
            panelFotoidentificacaoImage.sprite = Resources.Load<Sprite>(image_path);
        if (panelFotoidentificacaoAumentadoImage != null)
            panelFotoidentificacaoAumentadoImage.sprite = Resources.Load<Sprite>(image_path);
        if (panelWhalesCatalogImage != null)
            panelWhalesCatalogImage.sprite = Resources.Load<Sprite>(image_path);
    }
}

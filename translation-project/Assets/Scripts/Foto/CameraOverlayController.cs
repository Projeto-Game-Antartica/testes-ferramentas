using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class CameraOverlayController : MonoBehaviour {

    public SpriteRenderer spriteRenderer;

    public AudioSource audioSource;

    public AudioClip photoAudioClip;
    public AudioClip loadingAudioClip;
    public AudioClip turningOffAudioClip;

    public GameObject panelContent;
    public GameObject panelImage;

    new public Camera camera;

    public Button buttonPanelContent;

    private const float ZOOM_SPEED = 10f;

    // Update is called once per frame
    void Update () {
		if(Input.GetKeyDown(KeyCode.F))
        {
            if (!spriteRenderer.enabled)
                audioSource.PlayOneShot(loadingAudioClip);
            else
                audioSource.PlayOneShot(turningOffAudioClip);

            spriteRenderer.enabled = !spriteRenderer.enabled;
        }

        if(Input.GetKeyDown(KeyCode.Return) && spriteRenderer.enabled && !panelContent.activeSelf)
        {
            audioSource.PlayOneShot(photoAudioClip);
            spriteRenderer.enabled = false;
            panelContent.SetActive(true);
            //StartCoroutine(captureScreenshot());
        }

        if(Input.GetKeyDown(KeyCode.Space) && panelContent.activeSelf)
        {
            panelContent.SetActive(false);
        }

        if (spriteRenderer.enabled)
            HandleCameraZoom();
        else
            camera.orthographicSize = Parameters.MAX_ORTHOSIZE;
	}


    private void HandleCameraZoom()
    {
        if (Input.GetKey(KeyCode.KeypadMinus))
        {
            if (camera.orthographicSize >= Parameters.MAX_ORTHOSIZE)
                camera.orthographicSize = Parameters.MAX_ORTHOSIZE;
            else
                camera.orthographicSize += ZOOM_SPEED * Time.deltaTime;

        }

        if (Input.GetKey(KeyCode.KeypadPlus))
        {
            if (camera.orthographicSize <= Parameters.MIN_ORTHOSIZE)
                camera.orthographicSize = Parameters.MIN_ORTHOSIZE;
            else
                camera.orthographicSize -= ZOOM_SPEED * Time.deltaTime;
        }

        if(Input.mouseScrollDelta.y > 0)
        {
            if (camera.orthographicSize <= Parameters.MIN_ORTHOSIZE)
                camera.orthographicSize = Parameters.MIN_ORTHOSIZE;
            else
                camera.orthographicSize -= ZOOM_SPEED * Time.deltaTime;
        }

        if (Input.mouseScrollDelta.y < 0)
        {
            if (camera.orthographicSize >= Parameters.MAX_ORTHOSIZE)
                camera.orthographicSize = Parameters.MAX_ORTHOSIZE;
            else
                camera.orthographicSize += ZOOM_SPEED * Time.deltaTime;
        }
    }

    public IEnumerator captureScreenshot()
    {
        yield return new WaitForEndOfFrame();
        
        string path = Application.persistentDataPath + "/whale-screenshot.png";

        Texture2D screenImage = new Texture2D(Screen.width, Screen.height);

        //Get Image from screen
        screenImage.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);

        //Wait for a long time
        for (int i = 0; i < 15; i++)
        {
            yield return null;
        }

        screenImage.Apply();

        //Wait for a long time
        for (int i = 0; i < 15; i++)
        {
            yield return null;
        }

        //Convert to png(Expensive)
        byte[] imageBytes = screenImage.EncodeToPNG();

        //Wait for a long time
        for (int i = 0; i < 15; i++)
        {
            yield return null;
        }

        //Create new thread then save image to file
        new System.Threading.Thread(() =>
        {
            System.Threading.Thread.Sleep(100);
            File.WriteAllBytes(path, imageBytes);
        }).Start();

        //Wait for a long time
        for (int i=0; i < 15; i++)
        {
            yield return null;
        }

        // activate the content panel and speak the instructions (accessibility only)
        panelContent.SetActive(true);
        if (Parameters.ACCESSIBILITY) ContentPanelController.SpeakInstructions();
        buttonPanelContent.Select();

        // set the screenshot on panel image
        Image image = panelImage.GetComponent<Image>();
        image.sprite = LoadPNG(path);
    }

    public Sprite LoadPNG(string filePath)
    {

        Texture2D tex = null;
        byte[] fileData;

        if (File.Exists(filePath))
        {
            fileData = File.ReadAllBytes(filePath);
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
        }

        Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f,0.5f));
        return sprite;
    }
}

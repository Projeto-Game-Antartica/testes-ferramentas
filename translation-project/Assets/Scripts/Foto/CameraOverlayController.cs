using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class CameraOverlayController : MonoBehaviour {

    // object containing all sprites for camera overlay
    public GameObject cameraOverlaySprites;

    // Audio components
    public AudioSource audioSource;
    public AudioClip photoAudioClip;
    public AudioClip loadingAudioClip;
    public AudioClip turningOffAudioClip;
    
    // content panel objects
    public GameObject panelContent;
    public Image panelImage;
    public Button buttonPanelContent;
    public Text date;
    public Text latitude;
    public Text longitude;

    // catalog panel objects
    public Image catalogImage;

    // whale controller script
    public WhaleController whaleController;

    // range for whales id according to json
    private const int min_id = 1;
    private const int max_id = 9;
    public static int randomID;

    // camera
    new public Camera camera;

    // instructions texts
    private ReadableTexts readableTexts;

    private void Start()
    {
        readableTexts = GameObject.Find("ReadableTexts").GetComponent<ReadableTexts>();
    }

    // Update is called once per frame
    void Update () {

        if(Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("pressed");
            gameObject.SetActive(true);
        }
        // open the overlay
		if(Input.GetKeyDown(KeyCode.F) && !panelContent.activeSelf)
        {
            if (!cameraOverlaySprites.activeSelf)
                audioSource.PlayOneShot(loadingAudioClip);
            else
                audioSource.PlayOneShot(turningOffAudioClip);

            //spriteRenderer.enabled = !spriteRenderer.enabled;

            if (cameraOverlaySprites.activeSelf)
                cameraOverlaySprites.SetActive(false);
            else
                cameraOverlaySprites.SetActive(true);
        }

        // take the photo
        if(Input.GetKeyDown(KeyCode.Space) && cameraOverlaySprites.activeSelf && !panelContent.activeSelf)
        {
            audioSource.PlayOneShot(photoAudioClip);
            cameraOverlaySprites.SetActive(false); ;
            //StartCoroutine(captureScreenshot());

            // open the content panel (without screenshot)
            StartCoroutine(GetWhaleInfo());
            panelContent.SetActive(true);
            if (Parameters.ACCESSIBILITY) panelContent.GetComponent<ContentPanelController>().ReadInstructions();
            buttonPanelContent.Select();

            ReadableTexts.ReadText(readableTexts.GetReadableText(ReadableTexts.key_foto_catalogDescription, LocalizationManager.instance.GetLozalization()));
        }

        // close the catalog panel
        if(Input.GetKeyDown(KeyCode.Escape) && panelContent.activeSelf)
        {
            panelContent.SetActive(false);
            ReadableTexts.ReadText("Catálogo fechado");
        }

        // repeat instructions
        if(Input.GetKeyDown(KeyCode.F1) && panelContent.activeSelf)
        {
            ReadableTexts.ReadText(readableTexts.GetReadableText(ReadableTexts.key_foto_catalogDescription, LocalizationManager.instance.GetLozalization()));
        }

        // zoom only when overlay is enabled
        if (cameraOverlaySprites.activeSelf)
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
                camera.orthographicSize += Parameters.ZOOM_SPEED * Time.deltaTime;

        }

        if (Input.GetKey(KeyCode.KeypadPlus))
        {
            if (camera.orthographicSize <= Parameters.MIN_ORTHOSIZE)
                camera.orthographicSize = Parameters.MIN_ORTHOSIZE;
            else
                camera.orthographicSize -= Parameters.ZOOM_SPEED * Time.deltaTime;
        }

        if(Input.mouseScrollDelta.y > 0)
        {
            if (camera.orthographicSize <= Parameters.MIN_ORTHOSIZE)
                camera.orthographicSize = Parameters.MIN_ORTHOSIZE;
            else
                camera.orthographicSize -= Parameters.ZOOM_SPEED * Time.deltaTime;
        }

        if (Input.mouseScrollDelta.y < 0)
        {
            if (camera.orthographicSize >= Parameters.MAX_ORTHOSIZE)
                camera.orthographicSize = Parameters.MAX_ORTHOSIZE;
            else
                camera.orthographicSize += Parameters.ZOOM_SPEED * Time.deltaTime;
        }
    }

    // retrieve whale info according to WhaleData class
    public IEnumerator GetWhaleInfo()
    {
        yield return new WaitForEndOfFrame();

        // get and random id
        randomID = Random.Range(min_id, max_id);

        // get the whale from random id
        WhaleData whale = whaleController.getWhaleById(randomID);

        // set the date, image, latitude and longitude to the content panel
        panelImage.sprite = Resources.Load<Sprite>(whale.image_path);
        date.text = System.DateTime.Now.ToString();
        latitude.text = whale.latitude;
        longitude.text = whale.longitude;

        // set the image to the catalog panel
        catalogImage.sprite = panelImage.sprite;
    }

    // screenshot coroutine
    public IEnumerator captureScreenshot()
    {
        yield return new WaitForEndOfFrame();

        // just a simple way to not override any screenshot. System.DateTime.Now format = "MM/DD/YYYY HH:MM:SS AM/PM"
        //string path = Application.persistentDataPath + "/"+ System.DateTime.Now + "whale-screenshot.png";
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
        if (Parameters.ACCESSIBILITY) panelContent.GetComponent<ContentPanelController>().ReadInstructions();
        buttonPanelContent.Select();

        // set the screenshot on panel image
        panelImage.sprite = LoadPNG(path);
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

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CameraOverlayMissionController : AbstractScreenReader {
    
    // object containing all sprites for camera overlay
    public GameObject cameraOverlaySprites;

    // Audio components
    public AudioSource audioSource;
    public AudioClip photoAudioClip;
    public AudioClip loadingAudioClip;
    public AudioClip turningOffAudioClip;
    public AudioClip zoomInClip;
    public AudioClip zoomOutClip;
    public AudioClip avisoClip;

    // content panel objects
    public GameObject panelInstruction;
    public GameObject panelContent;
    public GameObject warningInterface;
    public Image panelImage;
    public Button saveButton;
    public Button cadastrarButton;
    public Button okButton;
    public TextMeshProUGUI date;
    public TextMeshProUGUI latitude;
    public TextMeshProUGUI longitude;
    public TMP_InputField nameInputField;

    public Sprite cenario;

    public Button backButton;

    // catalog panel objects
    //public Image catalogImage;

    // whale controller script
    public WhaleController whaleController;
    public WhaleImages whaleImages;

    // camera
    public new Camera camera;

    // instructions texts
    private ReadableTexts readableTexts;

    // array containing the 8 indexes for photo
    // sort the array for randomization
    private int[] whaleIndexes = { 0, 1, 2, 3, 4, 5, 6, 7};
    private int index;

    // feedback texts
    private const string NEGATIVE_FB = "A fotografia não ficou muito legal. Tente novamente.";
    private const string POSITIVE_FB = "A fotografia ficou ótima!";
    
    private void Start()
    {
        readableTexts = GameObject.FindGameObjectWithTag("Accessibility").GetComponent<ReadableTexts>();

        // randomize the indexes
        System.Random r = new System.Random();
        whaleIndexes = whaleIndexes.OrderBy(x => r.Next()).ToArray();

        index = 0;

        Parameters.WHALE_ID = -1;
    }

    // Update is called once per frame
    void Update ()
    {
  //      // open the overlay
		//if(Input.GetKeyDown(KeyCode.F) && !panelContent.activeSelf && !panelInstruction.activeSelf)
  //      {
  //          if (cameraOverlaySprites.activeSelf)
  //          {
  //              cameraOverlaySprites.SetActive(false);
  //              ReadText("Camera fechada");
  //          }
  //          else
  //          {
  //              cameraOverlaySprites.SetActive(true);
  //              ReadText("Camera aberta");
  //          }

  //          if (!cameraOverlaySprites.activeSelf && !audioSource.isPlaying)
  //              audioSource.PlayOneShot(loadingAudioClip);
  //          else if (!audioSource.isPlaying)
  //              audioSource.PlayOneShot(turningOffAudioClip);
  //      }

        // take the photo
        if(Input.GetKeyDown(KeyCode.Space) && cameraOverlaySprites.activeSelf && !panelContent.activeSelf)
        {
            HandlePhoto();
        }

        // close the catalog panel
        if(Input.GetKeyDown(KeyCode.Escape) && panelContent.activeSelf)
        {
            panelContent.SetActive(false);
            ReadText("Catálogo fechado");
        }

        // repeat instructions
        if(Input.GetKeyDown(KeyCode.F1) && panelContent.activeSelf)
        {
            ReadText(readableTexts.GetReadableText(ReadableTexts.key_foto_catalogDescription, LocalizationManager.instance.GetLozalization()));
        }

        // zoom only when overlay is enabled
        if (cameraOverlaySprites.activeSelf)
            HandleCameraZoom();
        else
            camera.orthographicSize = Parameters.MAX_ORTHOSIZE;
	}

    public void Initialize()
    {
        panelInstruction.SetActive(false);
        backButton.interactable = true;
        //resetButton.interactable = true;
        ReadText("Início do jogo. Pressione F3 para repetir a descrição do cenário.");
        //ReadText(readableTexts.GetReadableText(ReadableTexts.key_foto_sceneDescription, LocalizationManager.instance.GetLozalization()));
    }

    public void HandlePhoto()
    {
        // whale is not identified yet
        Parameters.ISWHALEIDENTIFIED = false;

        audioSource.PlayOneShot(photoAudioClip);
        //cameraOverlaySprites.SetActive(false);

        // reset the panel
        whaleImages.SetPhotographedWhaleImage(null);

        saveButton.interactable = false;
        cadastrarButton.interactable = false;

        nameInputField.text = null;
        nameInputField.interactable = false;

        warningInterface.SetActive(false);
        panelContent.SetActive(true);

        // show a whale image
        if (Parameters.ISWHALEONCAMERA)
        {
            StartCoroutine(GetWhaleInfo());
            // positive feedback
            ReadText(POSITIVE_FB);
        }
        else
        {
            Parameters.WHALE_ID = -1;

            // whale is not on the camera, take a screenshot
            //StartCoroutine(captureScreenshot());
            warningInterface.SetActive(true);

            warningInterface.GetComponentInChildren<Button>().Select();

            Debug.Log(warningInterface.GetComponentInChildren<Button>().name);

            audioSource.PlayOneShot(avisoClip);

            // set the screenshot on panel image
            panelImage.sprite = cenario;

            // negative feedback
            ReadText(NEGATIVE_FB);
        }

        if (Parameters.ACCESSIBILITY)
            panelContent.GetComponent<ContentPanelController>().ReadInstructions();

        saveButton.Select();

        ReadText(readableTexts.GetReadableText(ReadableTexts.key_foto_catalogDescription, LocalizationManager.instance.GetLozalization()));
    }

    private void HandleCameraZoom()
    {
        if (Input.GetKey(KeyCode.KeypadPlus) || Input.GetKey(KeyCode.Equals) || Input.mouseScrollDelta.y > 0)
        {
            ZoomIn();
        }

        if (Input.GetKey(KeyCode.KeypadMinus) || Input.GetKey(KeyCode.Minus) || Input.mouseScrollDelta.y < 0)
        {
            ZoomOut();
        }
    }

    public void ZoomIn()
    {
        if (camera.orthographicSize <= Parameters.MIN_ORTHOSIZE)
            camera.orthographicSize = Parameters.MIN_ORTHOSIZE;
        else
        {
            if (!audioSource.isPlaying) audioSource.PlayOneShot(zoomInClip);
            camera.orthographicSize -= Parameters.ZOOM_SPEED * Time.deltaTime;
        }
    }

    public void ZoomOut()
    {
        if (camera.orthographicSize >= Parameters.MAX_ORTHOSIZE)
            camera.orthographicSize = Parameters.MAX_ORTHOSIZE;
        else
        {
            camera.orthographicSize += Parameters.ZOOM_SPEED * Time.deltaTime;
            if (!audioSource.isPlaying) audioSource.PlayOneShot(zoomOutClip);
        }
    }

    // retrieve whale info according to WhaleData class
    public IEnumerator GetWhaleInfo()
    {
        yield return new WaitForEndOfFrame();

        // get and random id
        //Parameters.WHALE_ID = Random.Range(Parameters.MIN_ID, Parameters.MAX_ID);
        //Parameters.WHALE_ID = 1;

        // get an whale index starting from 1
        Parameters.WHALE_ID = whaleIndexes[index];

        // get the whale from random id
        WhaleData whale = whaleController.getWhaleById(Parameters.WHALE_ID);

        Debug.Log(whale.image_path);

        // set the image to all panels
        //panelImage.sprite = Resources.Load<Sprite>(whale.image_path);
        whaleImages.SetPhotographedWhaleImage(whale.image_path);


        // set the date, latitude and longitude to the content panel
        date.text = System.DateTime.Now.ToString();
        latitude.text = whale.latitude;
        longitude.text = whale.longitude;

        //whale.identified = true;
        Debug.Log(whale.whale_name);
        Debug.Log(whale.identified);

        // set the image to the catalog panel
        //catalogImage.sprite = panelImage.sprite;

        // increment whale index
        index++;
        if (index >= whaleIndexes.Length)
            index = 0;

        if(!whale.whale_name.Equals(""))
            cadastrarButton.interactable = false;
        else
            cadastrarButton.interactable = true;
        //Debug.Log(index);

        panelContent.GetComponent<ContentPanelMissionController>().ReadWhaleInfo(whale);
    }

    // screenshot coroutine
    public IEnumerator captureScreenshot()
    {
        yield return new WaitForEndOfFrame();

        Parameters.WHALE_ID = -1;

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

        // after the screenshot
        // activate the content panel and speak the instructions (accessibility only)
        // set the button inactive
        panelContent.SetActive(true);
        if (Parameters.ACCESSIBILITY) panelContent.GetComponent<ContentPanelMissionController>().ReadInstructions();
        saveButton.Select();

        // set the screenshot on panel image
        panelImage.sprite = null;
        panelImage.sprite = LoadPNG(path);

        nameInputField.text = null;
        nameInputField.interactable = false;

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

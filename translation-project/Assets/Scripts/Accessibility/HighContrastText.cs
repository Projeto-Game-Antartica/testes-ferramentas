using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.SceneManagement;

public class HighContrastText : AbstractScreenReader {

    /*
     * Hexadecimal colors:
     *  white  = #FFFFFF
     *  black  = #000000
     *  yellow = #FFFF00
     *  
     *  
     * Change the background color of text using TextMeshProUGUI
     * UI Text need to be TMPro
     * Attach this script to a HighContrastText prefab
     */
     
    private TMP_FontAsset arialFont;

    private TextMeshProUGUI text;
    private Color originalTextColor;
    private TMP_FontAsset originalFont;
    private Image bgImage;

    public bool HasVideo;

    private void Start()
    {
        arialFont = Resources.Load<TMP_FontAsset>("Fonts/ARIAL SDF");

        text = GetComponentInChildren<TextMeshProUGUI>();
        originalTextColor = text.color;
        originalFont = text.font;
        bgImage = GetComponent<Image>();
        //Debug.Log(bgImage.name);


        //Events for Libra Videos
        if(HasVideo) {
            EventTrigger trigger = gameObject.AddComponent<EventTrigger>() as EventTrigger;
            
            //Mouse Enter Event
            EventTrigger.Entry pEnter = new EventTrigger.Entry();
            pEnter.eventID = EventTriggerType.PointerEnter;
            pEnter.callback.AddListener( (eventData) => { setVideo(); } );
            trigger.triggers.Add(pEnter);

            //Mouse Exit Event            
            EventTrigger.Entry pExit = new EventTrigger.Entry();
            pExit.eventID = EventTriggerType.PointerExit;
            pExit.callback.AddListener( (eventData) => { clearVideo(); } );
            trigger.triggers.Add(pExit);

            //Mouse Click Event 
            //Solution to keep propagating events as per https://forum.unity.com/threads/how-does-ui-event-bubbles.514319/           
            EventTrigger.Entry pClick = new EventTrigger.Entry();
            pClick.eventID = EventTriggerType.PointerClick;
            pClick.callback.AddListener( (eventData) => { 
                //return;
                ExecuteEvents.ExecuteHierarchy(transform.parent.gameObject, eventData, ExecuteEvents.pointerClickHandler); 
                
                } );
            trigger.triggers.Add(pClick);
        }
    }

    // need to find a better way to do that. Its not efficient
    private void Update()
    {
        ChangeHighContrast();
        SetToBold();
    }

    public void ChangeHighContrast()
    {
        if (Parameters.HIGH_CONTRAST)
        {
            //Debug.Log("ChangeHighContrast");
            var tempColor = bgImage.color;
            tempColor = Color.black;
            tempColor.a = 1f;
            bgImage.color = tempColor;

            //text.font = arialFont;

            if(!Parameters.BUTTONCONTRAST)
                text.color = Color.white;
        }
        else
        {
            var tempColor = bgImage.color;
            tempColor.a = 0f;
            bgImage.color = tempColor;

            text.font = originalFont;

            if (!Parameters.BUTTONCONTRAST)
                text.color = originalTextColor;
        }
    }

    public void SetToBold()
    {
        string tmp = text.text;

        if (!Parameters.BOLD)
        {
            Parameters.BOLD = false;

            if (text.text.Contains("<b>"))
            {
                tmp = tmp.Replace("<b>", "");
                tmp = tmp.Replace("</b>", "");

                text.text = tmp;

                //ReadText("Texto em negrito desativado");
            }
        }
        else
        {
            Parameters.BOLD = true;

            if (!text.text.Contains("<b>"))
            {
                text.text = "<b>" + tmp + "</b>";
                //ReadText("Texto em negrito ativado");
            }

        }
    }

    private void setVideo() {
        //Debug.Log("Set Video!");
        if(Parameters.LIBRAS_ENABLED && text.text != "") {
            string videoPath = VideoPathFinder.FindPath(text.text, SceneManager.GetActiveScene().name);
            LibraseContraste.SetLibrasVideoPath(videoPath);
        }
    }

    private void clearVideo() {
        //Debug.Log("Clear Video!");
        if(Parameters.LIBRAS_ENABLED)   
            LibraseContraste.SetLibrasVideoPath("");
    }
}

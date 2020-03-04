/*
 *  This is script is only meant to be demonstrate various ways of handling data to create a Dialogue/UI Manager
 *  VIDE doesn't focus on the actual interface, but rather on the system and the data handling
 *  This script is basically handling the node data from nodeData in its own, customized way
 *  Creating a customized in-game Dialogue/UI manager is up to you
 *  Of course, you can absolutely use this script as a start point by adding, modifying, optimizing, or simplifying it to your needs.
 *  If you are experiencing strange behaviours or have any issues or questions, don't hesitate on contacting me at https://videdialogues.wordpress.com/contact/
 *  Need help programming your own UI Manager? Check out the scripting tutorial: https://videdialogues.wordpress.com/tutorial/
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using VIDE_Data; //<--- Import to use easily call VD class
using UnityEngine.SceneManagement;
using TMPro;

public class VIDEUIManager : AbstractScreenReader
{

    //This script will handle everything related to dialogue interface
    //It will use the VD class to load dialogues and retrieve node data

    #region VARS

    //These are the references to UI components and containers in the scene
    public GameObject dialogueContainer;
    public GameObject NPC_Container;
    public GameObject playerContainer;
    public GameObject itemPopUp;

    public TextMeshProUGUI NPC_Text;
    public TextMeshProUGUI NPC_label;
    public Image NPCSprite;
    public GameObject playerChoicePrefab;
    public GameObject playerChoiceHolder;
    public Image playerSprite;
    public TextMeshProUGUI playerLabel;

    public TextMeshProUGUI NPC_Text2;

    public Image xpIcon;

    public AudioClip xpClip;
    public AudioClip dialogueBlipClip;
    public AudioClip avisoClip;

    public AudioSource audioSource;

    bool dialoguePaused = false; //Custom variable to prevent the manager from calling VD.Next
    bool animatingText = false; //Will help us know when text is currently being animated

    //Reference to the player script
    //public VIDEPlayer player;
    public SimpleCharacterController player;

    //We'll be using this to store references of the current player choices
    private List<TextMeshProUGUI> currentChoices = new List<TextMeshProUGUI>();

    //With this we can start a coroutine and stop it. Used to animate text
    IEnumerator NPC_TextAnimator;

    public GameObject warningInterface;

    public GameObject AlertDialog;
    private string url;

    // to get mentor position
    private GameObject mentor;

    public LifeExpController lifeExpController;
    private bool flagEXP;
    private bool flagRead;

    public GameObject listaItem;

    public GameObject textPanel;
    public Image ticket_pt1;
    public Image ticket_pt2;
    public Image ticket_pt3;
    public Image ticket;
    public Button close;

    #endregion

    #region MAIN

    void Awake()
    {
        // VD.LoadDialogues(); //Load all dialogues to memory so that we dont spend time doing so later
        //An alternative to this can be preloading dialogues from the VIDE_Assign component!

        //Loads the saved state of VIDE_Assigns and dialogues.
        VD.LoadState("VIDEDEMOScene1", true);
    }

    //This begins the dialogue and progresses through it (Called by VIDEDemoPlayer.cs)
    public void Interact(VIDE_Assign dialogue)
    {
        //Sometimes, we might want to check the ExtraVariables and VAs before moving forward
        //We might want to modify the dialogue or perhaps go to another node, or dont start the dialogue at all
        //In such cases, the function will return true
        var doNotInteract = PreConditions(dialogue);
        if (doNotInteract)
        {
            Debug.Log("Não interagir....");
            return;
        }

        if (!VD.isActive)
        {
            Debug.Log("inicio de diálogo");
            mentor = dialogue.gameObject;
            Begin(dialogue);
        }
        else
        {
            Debug.Log("Próximo nó.");
            CallNext();
        }
    }

    //This begins the conversation
    void Begin(VIDE_Assign dialogue)
    {
        flagEXP = true;

        //Let's reset the NPC text variables
        NPC_Text.text = "";
        NPC_label.text = "";
        playerLabel.text = "";

        //First step is to call BeginDialogue, passing the required VIDE_Assign component 
        //This will store the first Node data in VD.nodeData
        //But before we do so, let's subscribe to certain events that will allow us to easily
        //Handle the node-changes
        VD.OnActionNode += ActionHandler;
        VD.OnNodeChange += UpdateUI;
        VD.OnEnd += EndDialogue;

        VD.BeginDialogue(dialogue); //Begins dialogue, will call the first OnNodeChange

        dialogueContainer.SetActive(true); //Let's make our dialogue container visible

        // accessibility
        ReadText("Início do diálogo.");
        Debug.Log("Início do diálogo.");
    }

    //Calls next node in the dialogue
    public void CallNext()
    {
        //Let's not go forward if text is currently being animated, but let's speed it up.
        if (animatingText) { CutTextAnim(); return; }

        if (!dialoguePaused) //Only if
        {
            VD.Next(); //We call the next node and populate nodeData with new data. Will fire OnNodeChange.
        }
        else
        {
            //Disable item popup and disable pause
            if (itemPopUp.activeSelf)
            {
                dialoguePaused = false;
                itemPopUp.SetActive(false);
            }
        }
    }

    //Input related stuff (scroll through player choices and update highlight)
    void Update()
    {
        //Lets just store the Node Data variable for the sake of fewer words
        var data = VD.nodeData;

        if (VD.isActive) //If there is a dialogue active
        {
            //Scroll through Player dialogue options if dialogue is not paused and we are on a player node
            //For player nodes, NodeData.commentIndex is the index of the picked choice
            if (!data.pausedAction && data.isPlayer)
            {
                if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
                {
                    if (data.commentIndex < currentChoices.Count - 1)
                    {
                        data.commentIndex++;

                        // Read the option selected after changing index
                        //ReadText("Opção " + data.commentIndex + " de " + (currentChoices.Count-1).ToString() + " " + data.comments[data.commentIndex]);
                        Debug.Log(data.comments[data.commentIndex]);
                        
                    }
                }
                if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                {
                    if (data.commentIndex > 0)
                    {
                        data.commentIndex--;
                        // Read the option selected after changing index
                        //ReadText("Opção " + data.commentIndex + " de " + (currentChoices.Count - 1).ToString() + " " + data.comments[data.commentIndex]);
                        Debug.Log(data.comments[data.commentIndex]);
                    }
                }

                //Color the Player options. Blue for the selected one
                for (int i = 0; i < currentChoices.Count; i++)
                {
                    //currentChoices[i].color = Color.white;
                    //Debug.Log(currentChoices[i].gameObject.GetComponentInParent<Image>().name);
                    // desabilita a imagem de fundo do texto
                    currentChoices[i].gameObject.GetComponentInParent<Image>().enabled = false;
                    if (i == data.commentIndex)
                    {
                        //currentChoices[i].color = Color.yellow;
                        // habilita a imagem do fundo do texto que foi selecionado
                        currentChoices[i].gameObject.GetComponentInParent<Image>().enabled = true;
                        Debug.Log(playerLabel.text + ": Opção " + (data.commentIndex+1) + " de " + (currentChoices.Count).ToString() + " " + data.comments[data.commentIndex]);
                        ReadText(playerLabel.text + ": Opção " + (data.commentIndex+1) + " de " + (currentChoices.Count).ToString() + " " + data.comments[data.commentIndex]);
                    }
                }
            }
             
            if (data.extraVars.ContainsKey("LoadScene"))
            {
                //SceneManager.LoadScene((string)data.extraVars["LoadScene"], LoadSceneMode.Single);
                LoadSceneWithDelay((string)data.extraVars["LoadScene"], 3f);
            }

            if(data.extraVars.ContainsKey("SavePosition"))
            {
                Vector3 positionSceneChange;
                float xdif = player.transform.position.x - mentor.gameObject.transform.position.x;

                // afasta um pouco o mentor depois que volta a cena pois, caso contrário, iria iniciar o dialogo novamente (devido ao colisor)
                if(xdif > 0)
                    positionSceneChange = new Vector2(player.gameObject.transform.position.x + 10.0f, player.gameObject.transform.position.y);
                else
                    positionSceneChange = new Vector2(player.gameObject.transform.position.x - 10.0f, player.gameObject.transform.position.y);

                
                Debug.Log(positionSceneChange);
                string missionNumber = player.gameObject.GetComponent<SimpleCharacterController>().missionNumber;

                player.gameObject.GetComponent<SimpleCharacterController>().SavePosition(positionSceneChange, missionNumber);
            }

            if(data.extraVars.ContainsKey("OpenURL"))
            {
                AlertDialog.SetActive(true);
                url = (string)data.extraVars["OpenURL"];
                //Debug.Log(url);
            }

            if(data.extraVars.ContainsKey("AddEXP"))
            {
                AddExperience();

                string dialogueName = (string)data.extraVars["AddEXP"];

                Debug.Log(dialogueName);

                // set the dialogue to read (whitout puzzle)
                if (PlayerPrefs.HasKey(dialogueName))
                {
                    Debug.Log(dialogueName + " atualizado.");
                    PlayerPrefs.SetInt(dialogueName, 1);
                }
                else
                {
                    Debug.Log("Player prefs dont have key >> " + dialogueName);
                }

                //The third component on hierarchy is the dialogue/minijogo balloon
                //Update 18/02: This should only change when the player successfully finishes the minigame
                //mentor.gameObject.GetComponentsInChildren<SpriteRenderer>()[2].color = new Color(0.4f, 1, 0.4f);
            }

            if(data.extraVars.ContainsKey("SetDoneBalloon")) {
                mentor.gameObject.GetComponentInChildren<DialogMentorBalloon>().SetDone();
            }

            if(data.extraVars.ContainsKey("OpenLista"))
            {
                listaItem.SetActive(true);
            }

            if(data.extraVars.ContainsKey("Ticket"))
            {
                close.gameObject.SetActive(true);
                textPanel.SetActive(true);
                close.Select();

                string result = "";

                switch ((string)data.extraVars["Ticket"])
                {
                    case "pt1":
                        ticket_pt1.gameObject.SetActive(true);

                        result = "Parabéns! Você adquiriu a parte 1 de 3 da passagem de embarque";

                        PlayerPrefs.SetInt("M002_Ticketpt1", 1);

                        if (PlayerPrefs.GetInt("M002_Ticketpt2") == 1)
                            ticket_pt2.gameObject.SetActive(true);

                        if (PlayerPrefs.GetInt("M002_Ticketpt3") == 1)
                            ticket_pt3.gameObject.SetActive(true);
                        break;
                    case "pt2":
                        ticket_pt2.gameObject.SetActive(true);

                        result = "Parabéns! Você adquiriu a parte 2 de 3 da passagem de embarque";

                        PlayerPrefs.SetInt("M002_Ticketpt2", 1);

                        if (PlayerPrefs.GetInt("M002_Ticketpt1") == 1)
                            ticket_pt1.gameObject.SetActive(true);

                        if (PlayerPrefs.GetInt("M002_Ticketpt3") == 1)
                            ticket_pt3.gameObject.SetActive(true);
                        break;
                    case "inteiro":
                        PlayerPrefs.SetInt("M002_Ticketpt3", 1);
                        ticket_pt3.gameObject.SetActive(true);

                        result = "Parabéns! Você adquiriu a parte 3 de 3 da passagem de embarque";

                        if (PlayerPrefs.GetInt("M002_Ticketpt1") == 1)
                            ticket_pt1.gameObject.SetActive(true);

                        if (PlayerPrefs.GetInt("M002_Ticketpt3") == 1)
                            ticket_pt2.gameObject.SetActive(true);
                        break;
                    default:
                        break;
                }

                textPanel.GetComponentInChildren<TextMeshProUGUI>().text = result;

                Debug.Log(result);
                ReadText(result);
            }

            if (data.extraVars.ContainsKey("CloseTicket"))
            {
                close.gameObject.SetActive(false);
                textPanel.SetActive(false);
                ticket_pt1.gameObject.SetActive(false);
                ticket_pt2.gameObject.SetActive(false);
                ticket_pt3.gameObject.SetActive(false);
            }

            if(data.extraVars.ContainsKey("CheckTicket"))
            {
                if(PlayerPrefs.GetInt("M002_Ticketpt1") == 1 && PlayerPrefs.GetInt("M002_Ticketpt2") == 1)
                {
                    CallNext();
                }
                else
                {
                    Debug.Log("Você não está apto...");
                    EndDialogue(data);

                    warningInterface.SetActive(true);
                    audioSource.PlayOneShot(avisoClip);
                    warningInterface.GetComponentInChildren<TextMeshProUGUI>().text = "Você ainda não está apto para viajar à Antártica.";
                    ReadText(warningInterface.GetComponentInChildren<TextMeshProUGUI>().text);
                }
            }

            if(data.extraVars.ContainsKey("ReadAudioDescription"))
            {
                ReadAudioDescription((string)data.extraVars["ReadAudioDescrpition"]);
            }
        }

        //Note you could also use Unity's Navi system
    }

    public void ReadAudioDescription(string type)
    {
        if(flagRead)
        {
            if (type.Equals("Player"))
                ReadText(ReadableTexts.instance.GetReadableText(ReadableTexts.key_dialogo_player, LocalizationManager.instance.GetLozalization()));
            else
                ReadText(ReadableTexts.instance.GetReadableText(ReadableTexts.key_dialogo_npc, LocalizationManager.instance.GetLozalization()));
            flagRead = false;
        }

    }

    public void AddExperience()
    {
        if (flagEXP)
        {
            audioSource.PlayOneShot(xpClip);

            StartCoroutine(HandlexpIcon());
            lifeExpController.AddEXP(0.02f);

            Debug.Log("exp gained");
            flagEXP = false;
        }
    }

    public void LoadSceneWithDelay(string sceneName, float delay) {
        StartCoroutine(LoadSceneWithDelayCoroutine(sceneName, delay));
    }

    public IEnumerator LoadSceneWithDelayCoroutine(string sceneName, float delay) {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    public IEnumerator HandlexpIcon()
    {
        xpIcon.fillClockwise = true;

        while (xpIcon.fillAmount < 1f)
        {
            xpIcon.fillAmount += 0.05f;
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(1f);

        xpIcon.fillClockwise = false;
        while (xpIcon.fillAmount > 0)
        {
            xpIcon.fillAmount -= 0.05f;
            yield return new WaitForSeconds(0.05f);
        }
    }

    public void HandleAlertDialog(bool open)
    {
        if (open)
            Application.OpenURL(url);

        AlertDialog.SetActive(false);
        VD.Next();
    }

    //When we call VD.Next, nodeData will change. When it changes, OnNodeChange event will fire
    //We subscribed our UpdateUI method to the event in the Begin method
    //Here's where we update our UI
    void UpdateUI(VD.NodeData data)
    {
        //Reset some variables
        //Destroy the current choices
        foreach (TextMeshProUGUI op in currentChoices)
            Destroy(op.gameObject.transform.parent.parent.gameObject); // detroy o prefab (prefab > imagem > texto)
            //Destroy(op.gameObject);
        currentChoices = new List<TextMeshProUGUI>();
        //NPC_Text.text = "";
        NPC_Container.SetActive(false);
        playerContainer.SetActive(false);
        playerSprite.sprite = null;
        NPCSprite.sprite = null;

        //Look for dynamic text change in extraData
        PostConditions(data);

        //If this new Node is a Player Node, set the player choices offered by the node
        if (data.isPlayer)
        {
            //Set node sprite if there's any, otherwise try to use default sprite
            if (data.sprite != null)
                playerSprite.sprite = data.sprite;
            else if (VD.assigned.defaultPlayerSprite != null)
                playerSprite.sprite = VD.assigned.defaultPlayerSprite;

            SetOptions(data.comments);

            //If it has a tag, show it, otherwise let's use the alias we set in the VIDE Assign
            if (data.tag.Length > 0)
                playerLabel.text = data.tag;
            else
                playerLabel.text = player.playerName;

            //Sets the player container on
            playerContainer.SetActive(true);

        }
        else  //If it's an NPC Node, let's just update NPC's text and sprite
        {
            //Set node sprite if there's any, otherwise try to use default sprite
            if (data.sprite != null)
            {
                //dialogueContainer.GetComponent<Image>().enabled = true;
                

                //For NPC sprite, we'll first check if there's any "sprite" key
                //Such key is being used to apply the sprite only when at a certain comment index
                //Check CrazyCap dialogue for reference
                if (data.extraVars.ContainsKey("sprite"))
                {
                    if (data.commentIndex == (int)data.extraVars["sprite"])
                        NPCSprite.sprite = data.sprite;
                    else
                        NPCSprite.sprite = VD.assigned.defaultNPCSprite; //If not there yet, set default dialogue sprite
                }
                else //Otherwise use the node sprites
                {
                    NPCSprite.sprite = data.sprite;
                }
            } //or use the default sprite if there isnt a node sprite at all
            else if (VD.assigned.defaultNPCSprite != null)
                NPCSprite.sprite = VD.assigned.defaultNPCSprite;

            //This coroutine animates the NPC text instead of displaying it all at once
            NPC_TextAnimator = DrawText(data.comments[data.commentIndex], 0.02f);
            StartCoroutine(NPC_TextAnimator);

            //If it has a tag, show it, otherwise let's use the alias we set in the VIDE Assign
            if (data.tag.Length > 0)
                NPC_label.text = data.tag;
            else
                NPC_label.text = VD.assigned.alias;

            dialogueContainer.GetComponent<Image>().enabled = true;
            playerChoiceHolder.transform.parent.gameObject.SetActive(false);

            //Sets the NPC container on
            NPC_Container.SetActive(true);
           
        }
    }

    //This uses the returned string[] from nodeData.comments to create the UIs for each comment
    //It first cleans, then it instantiates new choices
    public void SetOptions(string[] choices)
    {
        playerChoiceHolder.transform.parent.gameObject.SetActive(true);
        dialogueContainer.GetComponent<Image>().enabled = false;

        //Create the choices. The prefab comes from a dummy gameobject in the scene
        //This is a generic way of doing it. You could instead have a fixed number of choices referenced.
        for (int i = 0; i < choices.Length; i++)
        {
            GameObject newOp = Instantiate(playerChoicePrefab.gameObject, playerChoicePrefab.transform.position, Quaternion.identity) as GameObject;
            //newOp.transform.SetParent(playerChoicePrefab.transform.parent, true);
            newOp.transform.SetParent(playerChoiceHolder.transform, true);
            
            newOp.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 20 - (20 * i));
            newOp.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            //newOp.GetComponent<UnityEngine.UI.Text>().text = choices[i];
            //newOp.GetComponent<TMPro.TextMeshProUGUI>().text = choices[i];

            // o texto de fato sera mostrado neste componente
            newOp.GetComponentsInChildren<TMPro.TextMeshProUGUI>()[1].text = (i+1) + ". " + choices[i];

            // esse texto serve para gerar o tamanho do gameobject para que ele se adeque ao layout
            newOp.GetComponent<TMPro.TextMeshProUGUI>().text = choices[i];

            NPC_Text2.text = NPC_Text.text;

            newOp.SetActive(true);

            // adiciono a lista o texto que eh utilizado
            currentChoices.Add(newOp.GetComponentsInChildren<TextMeshProUGUI>()[1]);
        }
    }

    //Unsuscribe from everything, disable UI, and end dialogue
    //Called automatically because we subscribed to the OnEnd event
    public void EndDialogue(VD.NodeData data)
    {
        //NPC_Text.text = string.Empty;
        CutTextAnim();
        CheckTasks();
        VD.OnActionNode -= ActionHandler;
        VD.OnNodeChange -= UpdateUI;
        VD.OnEnd -= EndDialogue;
        dialogueContainer.SetActive(false);
        VD.EndDialogue();

        //VD.SaveState("VIDEDEMOScene1", true); //Saves VIDE stuff related to EVs and override start nodes
        //QuestChartDemo.SaveProgress(); //saves OUR custom game data

        // accessibility
        ReadText("Fim do diálogo");
        Debug.Log("Fim do diálogo");
    }

    void OnDisable()
    {
        //If the script gets destroyed, let's make sure we force-end the dialogue to prevent errors
        //We do not save changes
        CheckTasks();
        VD.OnActionNode -= ActionHandler;
        VD.OnNodeChange -= UpdateUI;
        VD.OnEnd -= EndDialogue;
        if (dialogueContainer != null)
            dialogueContainer.SetActive(false);
        VD.EndDialogue();
    }

    #endregion

    #region DIALOGUE CONDITIONS 

    //DIALOGUE CONDITIONS --------------------------------------------

    //When this returns true, it means that we did something that alters the progression of the dialogue
    //And we don't want to call Next() this time
    bool PreConditions(VIDE_Assign dialogue)
    {
        var data = VD.nodeData;

        if (VD.isActive) //Stuff we check while the dialogue is active
        {
            //Check for extra variables
            //This one finds a key named "item" which has the value of the item thats gonna be given
            //If there's an 'item' key, then we will assume there's also an 'itemLine' key and use it
            if (!data.isPlayer)
            {
                if (data.extraVars.ContainsKey("item") && !data.dirty)
                {
                    if (data.commentIndex == (int)data.extraVars["itemLine"])
                    {
                        if (data.extraVars.ContainsKey("item++")) //If we have this key, we use it to increment the value of 'item' by 'item++'
                        {
                            Dictionary<string, object> newVars = data.extraVars; //Clone the current extraVars content
                            int newItem = (int)newVars["item"]; //Retrieve the value we want to change
                            newItem += (int)data.extraVars["item++"]; //Change it as we desire
                            newVars["item"] = newItem; //Set it back   
                            VD.SetExtraVariables(25, newVars); //Send newVars through UpdateExtraVariable method
                        }

                        //If it's CrazyCap, check his stock before continuing
                        //If out of stock, change override start node
                        if (VD.assigned.alias == "CrazyCap")
                            if ((int)data.extraVars["item"] + 1 >= player.demo_Items.Count)
                                VD.assigned.overrideStartNode = 28;


                        if (!player.demo_ItemInventory.Contains(player.demo_Items[(int)data.extraVars["item"]]))
                        {
                            GiveItem((int)data.extraVars["item"]);
                            return true;
                        }
                    }
                }
            }
            else
            {
                if (data.extraVars.ContainsKey("outCondition"))
                {
                    if (data.extraVars.ContainsKey("condInfo"))
                    {
                        int[] nodeIDs = VD.ToIntArray((string)data.extraVars["outCondition"]);
                        if (VD.assigned.interactionCount < nodeIDs.Length)
                            VD.SetNode(nodeIDs[VD.assigned.interactionCount]);
                        else
                            VD.SetNode(nodeIDs[nodeIDs.Length - 1]);
                        return true;
                    }
                }

            }
        }
        else //Stuff we do right before the dialogue begins
        {
            //Get the item from CrazyCap to trigger this one on Charlie
            if (dialogue.alias == "Charlie")
            {
                if (player.demo_ItemInventory.Count > 0 && dialogue.overrideStartNode == -1)
                {
                    dialogue.overrideStartNode = 16;
                    return false;
                }
            }
        }
        return false;
    }

    //Conditions we check after VD.Next was called but before we update the UI
    void PostConditions(VD.NodeData data)
    {
        //Don't conduct extra variable actions if we are waiting on a paused action
        if (data.pausedAction) return;

        if (!data.isPlayer) //For player nodes
        {
            //Replace [WORDS]
            ReplaceWord(data);

            //Checks for extraData that concerns font size (CrazyCap node 2)
            if (data.extraData[data.commentIndex].Contains("fs"))
            {
                //int fSize = 14;
                int fSize = 16;

                string[] fontSize = data.extraData[data.commentIndex].Split(","[0]);
                int.TryParse(fontSize[1], out fSize);
                NPC_Text.fontSize = fSize;
            }
            else
            {
                //NPC_Text.fontSize = 14;
                NPC_Text.fontSize = 16;
            }
        }
    }

    //This will replace any "[NAME]" with the name of the gameobject holding the VIDE_Assign
    //Will also replace [WEAPON] with a different variable
    void ReplaceWord(VD.NodeData data)
    {
        if (data.comments[data.commentIndex].Contains("[NAME]"))
            data.comments[data.commentIndex] = data.comments[data.commentIndex].Replace("[NAME]", VD.assigned.gameObject.name);

        if (data.comments[data.commentIndex].Contains("[WEAPON]"))
            data.comments[data.commentIndex] = data.comments[data.commentIndex].Replace("[WEAPON]", player.demo_ItemInventory[0].ToLower());
    }

    #endregion

    #region EVENTS AND HANDLERS

    //Just so we know when we finished loading all dialogues, then we unsubscribe
    void OnLoadedAction()
    {
        Debug.Log("Finished loading all dialogues");
        VD.OnLoaded -= OnLoadedAction;
    }

    //Another way to handle Action Nodes is to listen to the OnActionNode event, which sends the ID of the action node
    void ActionHandler(int actionNodeID)
    {
        //Debug.Log("ACTION TRIGGERED: " + actionNodeID.ToString());
    }

    //Adds item to demo inventory, shows item popup, and pauses dialogue
    void GiveItem(int itemIndex)
    {
        player.demo_ItemInventory.Add(player.demo_Items[itemIndex]);
        itemPopUp.SetActive(true);
        string text = "You've got a <color=yellow>" + player.demo_Items[itemIndex] + "</color>!";
        itemPopUp.transform.GetChild(0).GetComponent<Text>().text = text;
        dialoguePaused = true;
    }

    IEnumerator DrawText(string text, float time)
    {
        // while drwaing play the audio clip clip
        audioSource.PlayOneShot(dialogueBlipClip);
        audioSource.loop = true;

        animatingText = true;

        string[] words = text.Split(' ');

        char[] letters = text.ToCharArray();

        NPC_Text.text = "";
        NPC_Text2.text = "";

        for(int i = 0; i<letters.Length; i++)
        {
            NPC_Text.text += letters[i];
            yield return new WaitForSeconds(time);
        }
        //for (int i = 0; i < words.Length; i++)
        //{
        //    string word = words[i];
        //    if (i != words.Length - 1) word += " ";

        //    string previousText = NPC_Text.text;

        //    float lastHeight = NPC_Text.preferredHeight;
        //    NPC_Text.text += word;
        //    if (NPC_Text.preferredHeight > lastHeight)
        //    {
        //        previousText += System.Environment.NewLine;
        //    }

        //    for (int j = 0; j < word.Length; j++)
        //    {
        //        NPC_Text.text = previousText + word.Substring(0, j + 1);
        //        yield return new WaitForSeconds(time);
        //    }
        //}
        NPC_Text.text = text;

        // Make screenreader read the text. Reads after animation
        Debug.Log(NPC_label.text);
        Debug.Log(NPC_Text.text);
        ReadText(NPC_label.text);
        ReadText(NPC_Text.text);

        // stop when finishes
        audioSource.Stop();
        audioSource.loop = false;

        animatingText = false;
    }

    void CutTextAnim()
    {
        StopCoroutine(NPC_TextAnimator);
        NPC_Text.text = VD.nodeData.comments[VD.nodeData.commentIndex]; //Now just copy full text		

        // Make screenreader read the text after cutting the animation
        ReadText(NPC_label.text);
        ReadText(NPC_Text.text);

        // stop if cut animation
        audioSource.Stop();
        audioSource.loop = false;

        animatingText = false;
    }

    //Check task progression
    void CheckTasks()
    {
        if (player.demo_ItemInventory.Count == 5)
            QuestChartDemo.SetQuest(2, false);

        QuestChartDemo.CheckTaskCompletion(VD.nodeData);
    }

    #endregion

    //Utility note: If you're on MonoDevelop. Go to Tools > Options > General and enable code folding.
    //That way you can exapnd and collapse the regions and methods

}

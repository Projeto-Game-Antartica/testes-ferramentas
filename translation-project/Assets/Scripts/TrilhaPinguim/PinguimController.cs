using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PinguimController : DragAndDropController {
    
    public GameObject pinguim_adelia;
    public GameObject pinguim_antartico;
    public GameObject pinguim_papua;

    public Button adeliaButton;
    public Button antarticoButton;
    public Button papuaButton;

    public Image adeliaIcon;
    public Image antarticoIcon;
    public Image papuaIcon;

    private Animator pinguim_adeliaAnimator;
    private Animator pinguim_antarticoAnimator;
    private Animator pinguim_papuaAnimator;

    public Image timer;

    public bool adeliaFinished;
    public bool antarticoFinished;
    public bool papuaFinished;
    
    private List<GameObject> draggedItems;

    public float verticalLength;
    public float horizontalLenght;

    private const int DOWN   = 0;
    private const int UP     = 1;
    private const int RIGHT  = 2;
    private const int LEFT   = 3;
    private const int RANDOM = 4;

    public LifeExpController lifeExpController;

    public GameObject WinImage;
    public GameObject LoseImage;
    public GameObject confirmQuit;

    public AudioClip closeClip;
    public AudioClip avisoClip;
    public AudioClip victoryClip;
    public AudioClip loseClip;
    public AudioClip pinguimAndandoClip;

    public GameObject instruction_interface;

    public Selectable firstItem;

    public Button resetButton;
    public Button audioButton;

    public void initializeGame()
    {
        draggedItems = new List<GameObject>();

        adeliaFinished = false;
        antarticoFinished = false;
        papuaFinished = false;

        pinguim_adeliaAnimator = pinguim_adelia.GetComponent<Animator>();
        pinguim_antarticoAnimator = pinguim_antartico.GetComponent<Animator>();
        pinguim_papuaAnimator = pinguim_papua.GetComponent<Animator>();

        pinguim_adeliaAnimator.SetBool("isMoving", false);
        pinguim_antarticoAnimator.SetBool("isMoving", false);
        pinguim_papuaAnimator.SetBool("isMoving", false);

        timer.fillAmount = 1f;

        resetButton.interactable = true;

        firstItem.Select();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            instruction_interface.SetActive(true);
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            if (instruction_interface.activeSelf)
            {
                instruction_interface.SetActive(false);
                audioSource.PlayOneShot(closeClip);
            }
            else
            {
                TryReturnToUshuaia();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {

        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (!isPositioning)
            {
                try
                {
                    if (EventSystem.current.currentSelectedGameObject.GetComponentInChildren<DragAndDropItem>().gameObject.tag.Equals("item"))
                    {
                        OnButtonClick();
                        isPositioning = true;
                    }
                }
                catch (Exception e)
                {
                    Debug.Log("Não é um item. Stacktrace >>" + e.StackTrace);
                }
            }
            else
            {
                try
                {
                    DragAndDropCell.DropEventDescriptor desc = new DragAndDropCell.DropEventDescriptor();
                    currentCell = EventSystem.current.currentSelectedGameObject.GetComponent<DragAndDropCell>();

                    desc.item = currentItem;
                    desc.sourceCell = sourceCell;
                    desc.destinationCell = currentCell;
                    currentCell.SendRequest(desc);                      // Send drop request
                    StartCoroutine(currentCell.NotifyOnDragEnd(desc));  // Send notification after drop will be finished

                    if (desc.permission == true)
                    {
                        currentCell.PlaceItem(currentItem);
                    }

                    currentCell.UpdateMyItem();
                    currentCell.UpdateBackgroundState();
                    sourceCell.UpdateMyItem();
                    sourceCell.UpdateBackgroundState();

                    // go to item
                    //firstItem.GetComponent<Selectable>().Select();

                    ResetConditions();
                    desc.item.ResetConditions();

                    isPositioning = false;
                    firstItem.Select();
                }
                catch (Exception e)
                {
                    Debug.Log("Não é uma célula. Stacktrace >>" + e.StackTrace);
                }
            }
        }

        if ((Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow) ||
                            Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow)))
        {
            GameObject nextCell = EventSystem.current.currentSelectedGameObject.gameObject;

            try
            {
                if (currentItem != null)
                {
                    DragAndDropItem.icon.GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(nextCell.transform.position);
                }

                if (isCell)
                {
                    nextCell.GetComponent<Selectable>().Select();
                }
                else
                {
                    nextCell.GetComponent<Selectable>().Select();
                    Debug.Log(nextCell.GetComponentInChildren<DragAndDropItem>().gameObject.name);
                    ReadText(nextCell.GetComponentInChildren<DragAndDropItem>().gameObject.name);
                }
            }
            catch (Exception e)
            {
                Debug.Log("null exception >> " + e.StackTrace);
            }
        }

        if(Input.GetKeyDown(InputKeys.MJMENU_KEY))
        {
            audioButton.Select();
        }

        if (Input.GetKeyDown(KeyCode.F6))
        {
            isCell = !isCell;

            if (isCell)
            {
                ReadText("Células");
                Debug.Log("Células");
                cells[0].GetComponent<Selectable>().Select();
            }
            else
            {
                ReadText("Itens");
                Debug.Log("Itens");
                firstItem.GetComponent<Selectable>().Select();
            }
        }

        if (Input.GetKeyDown(InputKeys.PARAMETERS_KEY))
        {
            lifeExpController.ReadHPandEXP();
            //ReadText("Você ainda tem " + (timer.fillAmount * 10f) + " segundos restantes");
            //Debug.Log("Você ainda tem " + (timer.fillAmount * 10f) + " segundos restantes");
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            // audiodescricao
        }

        // loses the game
        if (timer.fillAmount <= 0f)
            StartCoroutine(EndGame(false));

        // wins the game
        if (adeliaFinished && antarticoFinished && papuaFinished)
        {
            StartCoroutine(EndGame(true));
        }
    }

    /// <summary>
    /// Operate all drag and drop requests and events from children cells
    /// </summary>
    /// <param name="desc"> request or event descriptor </param>
    public override void OnSimpleDragAndDropEvent(DragAndDropCell.DropEventDescriptor desc)
    {
        // Get control unit of source cell
        PinguimController sourceSheet = desc.sourceCell.GetComponentInParent<PinguimController>();
        // Get control unit of destination cell
        PinguimController destinationSheet = desc.destinationCell.GetComponentInParent<PinguimController>();
        switch (desc.triggerType)                                               // What type event is?
        {
            case DragAndDropCell.TriggerType.DropRequest:                       // Request for item drag (note: do not destroy item on request)
                Debug.Log("Request " + desc.item.name + " from " + sourceSheet.name + " to " + destinationSheet.name);
                break;
            case DragAndDropCell.TriggerType.DropEventEnd:                      // Drop event completed (successful or not)
                if (desc.permission == true)                                    // If drop successful (was permitted before)
                {
                    Debug.Log("Successful drop " + desc.item.name + " from " + sourceSheet.name + " to " + destinationSheet.name);
                    draggedItems.Add(desc.item.gameObject);
                }
                else                                                            // If drop unsuccessful (was denied before)
                {
                    Debug.Log("Denied drop " + desc.item.name + " from " + sourceSheet.name + " to " + destinationSheet.name);
                }
                break;
            case DragAndDropCell.TriggerType.ItemAdded:                         // New item is added from application
                Debug.Log("Item " + desc.item.name + " added into " + destinationSheet.name);
                break;
            case DragAndDropCell.TriggerType.ItemWillBeDestroyed:               // Called before item be destructed (can not be canceled)
                Debug.Log("Item " + desc.item.name + " will be destroyed from " + sourceSheet.name);
                break;
            default:
                Debug.Log("Unknown drag and drop event");
                break;
        }
    }

    public void GoWrapper()
    {
        StartCoroutine(GoCoroutine());
    }
    
    public IEnumerator GoCoroutine()
    {
        audioSource.PlayOneShot(pinguimAndandoClip);
        foreach (GameObject g in draggedItems)
        {
            CountTime(0.01f);  

            if (pinguim_adelia.activeSelf) pinguim_adeliaAnimator.SetBool("isMoving", true);
            if (pinguim_antartico.activeSelf) pinguim_antarticoAnimator.SetBool("isMoving", true);
            if (pinguim_papua.activeSelf) pinguim_papuaAnimator.SetBool("isMoving", true);

            switch(g.name)
            {
                case "baixo":
                    Debug.Log("Moving down");
                    goDown(); // the selected go in this direction
                    goRandomDirection(); // the others go in random direction
                    break;
                case "cima":
                    Debug.Log("Moving up");
                    goUp(); // the selected go in this direction
                    goRandomDirection(); // the others go in random direction
                    break;
                case "direita":
                    Debug.Log("Moving right");
                    goRight(); // the selected go in this direction
                    goRandomDirection(); // the others go in random direction
                    break;
                case "esquerda":
                    Debug.Log("Moving left");
                    goLeft(); // the selected go in this direction
                    goRandomDirection(); // the others go in random direction
                    break;
                default:
                    Debug.Log("movimento nao reconhecido.");
                    break;
            }
            yield return new WaitForSeconds(1.2f);
        }

        RemoveAllItems();
        draggedItems.Clear();
        UpdateBackgroundState();


        if (pinguim_adelia.activeSelf)
            pinguim_adeliaAnimator.SetBool("isMoving", false);
        if (pinguim_antartico.activeSelf)
            pinguim_antarticoAnimator.SetBool("isMoving", false);
        if (pinguim_papua.activeSelf)
            pinguim_papuaAnimator.SetBool("isMoving", false);

        if (audioSource.isPlaying)
            audioSource.Stop();
    }

    public void goUp()
    {
        if (pinguim_adelia.transform.GetChild(0).gameObject.activeSelf && !adeliaFinished)
        {
            pinguim_adelia.transform.DOBlendableMoveBy(new Vector3(0, verticalLength), 1);
        }

        if (pinguim_papua.transform.GetChild(0).gameObject.activeSelf && !papuaFinished)
        {
            pinguim_papua.transform.DOBlendableMoveBy(new Vector3(0, verticalLength), 1);
        }

        if (pinguim_antartico.transform.GetChild(0).gameObject.activeSelf && !antarticoFinished)
        {
            pinguim_antartico.transform.DOBlendableMoveBy(new Vector3(0, verticalLength), 1);
        }
    }

    public void goDown()
    {
        if (pinguim_adelia.transform.GetChild(0).gameObject.activeSelf && !adeliaFinished)
        {
            pinguim_adelia.transform.DOBlendableMoveBy(new Vector3(0, -verticalLength), 1);
        }

        if (pinguim_papua.transform.GetChild(0).gameObject.activeSelf && !papuaFinished)
        {
            pinguim_papua.transform.DOBlendableMoveBy(new Vector3(0, -verticalLength), 1);
        }

        if (pinguim_antartico.transform.GetChild(0).gameObject.activeSelf && !antarticoFinished)
        {
            pinguim_antartico.transform.DOBlendableMoveBy(new Vector3(0, -verticalLength), 1);
        }
    }

    public void goRight()
    {
        if (pinguim_adelia.transform.GetChild(0).gameObject.activeSelf && !adeliaFinished)
        {
            FlipPinguim(pinguim_adelia.name, false);
            pinguim_adelia.transform.DOBlendableMoveBy(new Vector3(horizontalLenght, 0), 1);
        }

        if (pinguim_papua.transform.GetChild(0).gameObject.activeSelf && !papuaFinished)
        {
            FlipPinguim(pinguim_papua.name, false);
            pinguim_papua.transform.DOBlendableMoveBy(new Vector3(horizontalLenght, 0), 1);
        }

        if (pinguim_antartico.transform.GetChild(0).gameObject.activeSelf && !antarticoFinished)
        {
            FlipPinguim(pinguim_antartico.name, false);
            pinguim_antartico.transform.DOBlendableMoveBy(new Vector3(horizontalLenght, 0), 1);
        }
    }

    public void goLeft()
    {
        if (pinguim_adelia.transform.GetChild(0).gameObject.activeSelf && !adeliaFinished)
        {
            FlipPinguim(pinguim_adelia.name, true);
            pinguim_adelia.transform.DOBlendableMoveBy(new Vector3(-horizontalLenght, 0), 1);
        }

        if (pinguim_papua.transform.GetChild(0).gameObject.activeSelf && !papuaFinished)
        {
            FlipPinguim(pinguim_papua.name, true);
            pinguim_papua.transform.DOBlendableMoveBy(new Vector3(-horizontalLenght, 0), 1);
        }

        if (pinguim_antartico.transform.GetChild(0).gameObject.activeSelf && !antarticoFinished)
        {
            FlipPinguim(pinguim_antartico.name, true);
            pinguim_antartico.transform.DOBlendableMoveBy(new Vector3(-horizontalLenght, 0), 1);
        }
    }

    public void goRandomDirection()
    {
        int random = UnityEngine.Random.Range(1, 4);

        switch (random)
        {
            case 1: // go up
                if (!pinguim_adelia.transform.GetChild(0).gameObject.activeSelf && !adeliaFinished)
                {
                    pinguim_adelia.transform.DOBlendableMoveBy(new Vector3(0, verticalLength), 1);
                }

                if (!pinguim_papua.transform.GetChild(0).gameObject.activeSelf && !papuaFinished)
                {
                    pinguim_papua.transform.DOBlendableMoveBy(new Vector3(0, verticalLength), 1);
                }

                if (!pinguim_antartico.transform.GetChild(0).gameObject.activeSelf && !antarticoFinished)
                {
                    pinguim_antartico.transform.DOBlendableMoveBy(new Vector3(0, verticalLength), 1);
                }
                break;
            case 2: // go right
                if (!pinguim_adelia.transform.GetChild(0).gameObject.activeSelf && !adeliaFinished)
                {
                    FlipPinguim(pinguim_adelia.name, false);
                    pinguim_adelia.transform.DOBlendableMoveBy(new Vector3(horizontalLenght, 0), 1);
                }

                if (!pinguim_papua.transform.GetChild(0).gameObject.activeSelf && !papuaFinished)
                {
                    FlipPinguim(pinguim_papua.name, false);
                    pinguim_papua.transform.DOBlendableMoveBy(new Vector3(horizontalLenght, 0), 1);
                }

                if (!pinguim_antartico.transform.GetChild(0).gameObject.activeSelf && !antarticoFinished)
                {
                    FlipPinguim(pinguim_antartico.name, false);
                    pinguim_antartico.transform.DOBlendableMoveBy(new Vector3(horizontalLenght, 0), 1);
                }
                break;
            case 3: // go left
                if (!pinguim_adelia.transform.GetChild(0).gameObject.activeSelf && !adeliaFinished)
                {
                    FlipPinguim(pinguim_adelia.name, true);
                    pinguim_adelia.transform.DOBlendableMoveBy(new Vector3(-horizontalLenght, 0), 1);
                }

                if (!pinguim_papua.transform.GetChild(0).gameObject.activeSelf && !papuaFinished)
                {
                    FlipPinguim(pinguim_papua.name, true);
                    pinguim_papua.transform.DOBlendableMoveBy(new Vector3(-horizontalLenght, 0), 1);
                }

                if (!pinguim_antartico.transform.GetChild(0).gameObject.activeSelf && !antarticoFinished)
                {
                    FlipPinguim(pinguim_antartico.name, true);
                    pinguim_antartico.transform.DOBlendableMoveBy(new Vector3(-horizontalLenght, 0), 1);
                }
                break;
            case 4: // go down
                if (!pinguim_adelia.transform.GetChild(0).gameObject.activeSelf && !adeliaFinished)
                {
                    pinguim_adelia.transform.DOBlendableMoveBy(new Vector3(0, -verticalLength), 1);
                }

                if (!pinguim_papua.transform.GetChild(0).gameObject.activeSelf && !papuaFinished)
                {
                    pinguim_papua.transform.DOBlendableMoveBy(new Vector3(0, -verticalLength), 1);
                }

                if (!pinguim_antartico.transform.GetChild(0).gameObject.activeSelf && !antarticoFinished)
                {
                    pinguim_antartico.transform.DOBlendableMoveBy(new Vector3(0, -verticalLength), 1);
                }
                break;
        }
    }

    public void RemoveAllItems()
    {
        foreach(GameObject g in cells)
        {
            if(g.GetComponentInChildren<DragAndDropItem>() != null)
            {
                Destroy(g.transform.GetChild(0).gameObject);
                g.GetComponent<DragAndDropCell>().UpdateBackgroundState();
            }
        }
    }

    public void UpdateBackgroundState()
    {
        foreach (GameObject g in cells)
        {
            g.GetComponent<DragAndDropCell>().UpdateBackgroundState();
        }
    }

    public void SelectPinguin(string name)
    {
        switch (name)
        {
            case "adelia":
                Debug.Log("Pinguim Adelia selecionado");
                ReadText("Pinguim Adelia selecionado");
                pinguim_adelia.transform.GetChild(0).gameObject.SetActive(true);
                pinguim_antartico.transform.GetChild(0).gameObject.SetActive(false);
                pinguim_papua.transform.GetChild(0).gameObject.SetActive(false);
                break;
            case "antartico":
                Debug.Log("Pinguim Antártico selecionado");
                ReadText("Pinguim Antártico selecionado");
                pinguim_adelia.transform.GetChild(0).gameObject.SetActive(false);
                pinguim_antartico.transform.GetChild(0).gameObject.SetActive(true);
                pinguim_papua.transform.GetChild(0).gameObject.SetActive(false);
                break;
            case "papua":
                Debug.Log("Pinguim Papua selecionado");
                ReadText("Pinguim Papua selecionado");
                pinguim_adelia.transform.GetChild(0).gameObject.SetActive(false);
                pinguim_antartico.transform.GetChild(0).gameObject.SetActive(false);
                pinguim_papua.transform.GetChild(0).gameObject.SetActive(true);
                break;
            default:
                pinguim_adelia.transform.GetChild(0).gameObject.SetActive(false);
                pinguim_antartico.transform.GetChild(0).gameObject.SetActive(false);
                pinguim_papua.transform.GetChild(0).gameObject.SetActive(false);
                break;
        }
    }

    public void CountTime(float value)
    {
        if (timer.fillAmount < 0)
        {
            timer.fillAmount = 0;
            LoseImage.SetActive(true);
        }
        else
            timer.fillAmount -= value;
    }

    public void FlipPinguim(string name, bool left)
    {
        switch(name)
        {
            case "pinguim_adelia":
                pinguim_adelia.GetComponent<SpriteRenderer>().flipX = left;
                break;
            case "pinguim_antartico":
                pinguim_antartico.GetComponent<SpriteRenderer>().flipX = left;
                break;
            case "pinguim_papua":
                pinguim_papua.GetComponent<SpriteRenderer>().flipX = left;
                break;
        }
    }

    public IEnumerator EndGame(bool win)
    {
        if (win)
        {
            PlayerPreferences.M002_Pinguim = true;

            WinImage.SetActive(true);
            //WinImage.GetComponentInChildren<Button>().Select();

            //ReadText(ReadableTexts.instance.GetReadableText(ReadableTexts.key_m004_memoria_vitoria, LocalizationManager.instance.GetLozalization()));
            
            audioSource.PlayOneShot(victoryClip);

            yield return new WaitWhile(() => audioSource.isPlaying);

            ReadText("Parabéns, você conseguiu mais alguns dos itens necessários para sua aventura na Antártica!");

            lifeExpController.AddEXP(PlayerPreferences.XPwinPuzzle); // finalizou o minijogo
            lifeExpController.AddEXP(4*PlayerPreferences.XPwinItem); // ganhou o item
        }
        else
        {
            LoseImage.SetActive(true);

            //ReadText(ReadableTexts.instance.GetReadableText(ReadableTexts.key_m004_memoria_derrota, LocalizationManager.instance.GetLozalization()));

            audioSource.PlayOneShot(loseClip);

            yield return new WaitWhile(() => audioSource.isPlaying);

            ReadText("Infelizmente você não conseguiu finalizar o minijogo com êxito. Tente novamente.");
            resetButton.Select();
            lifeExpController.AddEXP(PlayerPreferences.XPlosePuzzle); // jogou um minijogo
        }

        StartCoroutine(ReturnToUshuaiaCoroutine()); // volta para o navio perdendo ou ganhando o minijogo
    }

    public void TryReturnToUshuaia()
    {
        confirmQuit.SetActive(true);

        ReadText(ReadableTexts.instance.GetReadableText(ReadableTexts.key_gameplay_aviso_botoes, LocalizationManager.instance.GetLozalization()));

        audioSource.PlayOneShot(avisoClip);

        ReadText(confirmQuit.GetComponentInChildren<TMPro.TextMeshProUGUI>().text);
        confirmQuit.GetComponentInChildren<Button>().Select();
    }

    public void ReturnToUshuaia()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(ScenesNames.M002Ushuaia);
    }

    public void ResetScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(ScenesNames.M002Pinguim);
    }

    public IEnumerator ReturnToUshuaiaCoroutine()
    {
        yield return new WaitForSeconds(7f);

        UnityEngine.SceneManagement.SceneManager.LoadScene(ScenesNames.M002Ushuaia);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using Random=UnityEngine.Random;
using UnityEngine.EventSystems;

public class DesafioManagerPaleo : AbstractScreenReader {

    private readonly string instructions = "Início do jogo. Mini jogo de memória. Descrição..";
    
    protected int selectedArea = 1;

    protected bool isEquipamento = false;

    protected bool isEquipado = false;

    protected bool acabou = false;

    public TMPro.TextMeshProUGUI kitValor;

    private float ganha = 33;

    public FossilImages fossilImages;

    public FossilController fossilController;

    public ClassificaManager classificaManager;
    public FossilData fossilData;

    public int num_fossil;

    public int vida;

    public int acha_fossil;

    public int quebra_fossil;
    public int limpa_fossil;
    
    public GameObject ClassificaFossil;

    //public CheckItens CheckItens;

    public List<string> Itens = new List<string>();

    private string item00;
    private string item01;
    private string item02;
    private string item03;

    public Sprite[] fossil0;
    public Sprite[] fossil1;
    public Sprite[] fossil2;
    public Sprite[] fossil3;
    public Sprite[] fossil4;
    public Sprite[] fossil5;
    public Sprite[] fossil6;
    public Sprite[] fossil7;
    public Sprite[] fossil8;
    public Sprite[] fossil9;
    public Sprite[] fossil10;
    public Sprite[] fossil11;
    public Sprite[] fossil12;
    public Sprite[] fossil13;
    public Sprite[] fossil14;
    public Sprite[] fossil15;
    public Sprite[] fossil16;
    public Sprite[] fossil17;
    public Sprite[] fossil18;
    public Sprite[] fossil19;
    public Sprite[] fossil20;
    public Sprite[] fossil21;
    public Sprite[] fossil22;
    public Sprite[] fossil23;
    public Sprite[] fossil24;
    public Sprite[] fossil25;
    public Sprite[] fossil26;

    public int[] posicoes = new int[] {0,1,2,4,5,6,8,9,10};
    private int sorteio;

    public Sprite cardBack;

    public GameObject[] cards;

    public GameObject[] pega_item;

    public GameObject[] equipamentos;


    public Button backButton;
    public Button resetButton;
    public Button audioButton;
    
    public Button usarKit;

    public int[] index; 
    private int matches = 6;
    private int miss = 0;

    private bool init;

    public static int CARDFACE = 1;

    public TMPro.TextMeshProUGUI missText;
    public TMPro.TextMeshProUGUI matchesText;

    public AudioSource audioSource;

    public AudioClip correctAudio;
    public AudioClip wrongAudio;

    private List<int> c;

    public GameObject WinImage;
    public TMPro.TextMeshProUGUI WinText;
    public GameObject LoseImage;
    public TMPro.TextMeshProUGUI LoseText;


    public GameObject BigImage1;
    public GameObject BigImage2;

    // hint settings
    public MinijogosDicas dicas;

    public LifeExpController lifeExpController;

    public GameObject instructionInterface;

    private bool _first;

    private enum Operation { correct, confirm, wrong }

    public string missionName;
    
    private void Start()
    {
        kitValor.text = "1";

        lifeExpController.GetComponent<LifeExpController>().HPImage.fillAmount = 1;

        //sorteio = Random.Range(0, 9);
        sorteio = 0;

        num_fossil = Random.Range(0, 27);

        Debug.Log("sorteio: " +sorteio);

        backButton.interactable = false;
        resetButton.interactable = false;

        init = false;
        _first = true;  
        
        fossilData = fossilController.getFossilById(num_fossil);
        
        fossilImages.SetPhotographedWhaleImage(fossilData.image_path);

        ReadText(instructions);

        audioSource = GetComponent<AudioSource>();

        if (instructionInterface.activeSelf)
            instructionInterface.GetComponentInChildren<Button>().Select();
    }

    // Update is called once per frame
    void Update () {

        if ((Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow) ||
                                    Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow)))
        {
            GameObject nextCell = EventSystem.current.currentSelectedGameObject.gameObject;

            if (selectedArea == 2)
            {
                nextCell.GetComponent<Selectable>().Select();

                ReadCamp(nextCell);
            }
            else if(selectedArea == 0)
            {
                nextCell.GetComponent<Selectable>().Select();

                ReadEquipamento(nextCell);
            }

            else if(selectedArea == 1)
            {
                nextCell.GetComponent<Selectable>().Select();

                ReadEquipados(nextCell);
            }
        }

        if(Input.GetKeyDown(KeyCode.Return)) {
            Debug.Log("ENTER CLICADO");  

            if(selectedArea == 0)
            {
                GameObject currentTool = EventSystem.current.currentSelectedGameObject.gameObject;

                foreach(GameObject equipSpot in pega_item)
                {
                    if(equipSpot.transform.childCount == 0)
                    {   
                        /*if(currentTool.GetComponent<Button>().gameObject.name == "usaKit")
                            {
                            
							}*/
                        if(currentTool.GetComponentInChildren<DragAndDropItem>().gameObject.name == "kit")
                        {
                            Debug.Log("O kit não pode ser equipado.");
                            break;
                        }
                        else
                        {
                            Debug.Log("O item "+ currentTool.GetComponentInChildren<DragAndDropItem>().gameObject.name +" foi equipado.");
                            ReadText("O item "+ currentTool.GetComponentInChildren<DragAndDropItem>().gameObject.name +" foi equipado.");
                            currentTool.transform.GetChild(0).SetParent(equipSpot.transform, false);                   
                            break;
                        }
                    }
                }
            }

            else if(selectedArea == 1)
            {
                GameObject currentTool = EventSystem.current.currentSelectedGameObject.gameObject;

                foreach(GameObject desequipSpot in equipamentos)
                {
                    if(desequipSpot.transform.childCount == 0)
                    {   Debug.Log("O item "+ currentTool.GetComponentInChildren<DragAndDropItem>().gameObject.name +" foi desequipado.");
                        ReadText("O item "+ currentTool.GetComponentInChildren<DragAndDropItem>().gameObject.name +" foi desequipado.");
                        currentTool.transform.GetChild(0).SetParent(desequipSpot.transform, false);                   
                        break;
                    }
                }
			}
		}


        if (!_first && (Input.GetMouseButtonUp(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)) && !Card.DO_NOT)
        {
            Debug.Log("aqui: " +pega_item[0]);
            Debug.Log("Checando cartas....");
            //checkCards();

            
        }

        if(Input.GetKeyDown(KeyCode.P))
        {
            audioButton.Select();
        }

        if(Input.GetKeyDown(KeyCode.F6))
        {
            selectedArea = (selectedArea + 1) % 3;

            if (selectedArea == 2)
            {
                cards[0].GetComponent<Button>().Select();
                ReadText("Campo");
                Debug.Log("Campo");
                ReadCamp(cards[0]);
            }
            else if(selectedArea == 1)
            {
                ReadText("Itens equipado");
                Debug.Log("Itens equipado");
                pega_item[0].GetComponent<Selectable>().Select();
                ReadEquipados(pega_item[0]);  
                
            }
            else
            {
                ReadText("Itens");
                Debug.Log("Itens");
                equipamentos[0].GetComponent<Selectable>().Select();
                ReadEquipamento(equipamentos[0]);
			}
        }

        if (c != null && c.Count >= 2)
        {
            Card.DO_NOT = true;
            //cancelarButton.interactable = true;
            //confirmarButton.interactable = true;
            Debug.Log(c.Count);
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (!instructionInterface.activeSelf)
            {
                instructionInterface.SetActive(true);
                instructionInterface.GetComponentInChildren<Button>().Select();
            }
            else
                instructionInterface.SetActive(false);
        }

        //else
        //{
        //    confirmarButton.interactable = false;
        //    cancelarButton.interactable = false;

        //}

        //Debug.Log(Card.DO_NOT);
    }

    public void initializeGame()
    {

        backButton.interactable = true;
        resetButton.interactable = true;

        if (!init)
            initializeCards();
        
        // start afther dicas.time seconds and repeat at dicas.repeatRate rate
        InvokeRepeating("CallHintMethod", dicas.time, dicas.repeatRate);
        
        StartCoroutine(showCards());

    }

    public void CallHintMethod()
    {
        dicas.StartHints();
    }

    public void initializeCards()
    {
    switch (sorteio)
        {
            case 0:
                cards[0].GetComponent<CardDesafioPaleo>().cardValue = 1;
                cards[1].GetComponent<CardDesafioPaleo>().cardValue = 2;
                cards[4].GetComponent<CardDesafioPaleo>().cardValue = 3;
                cards[5].GetComponent<CardDesafioPaleo>().cardValue = 4;

                cards[0].GetComponent<CardDesafioPaleo>().initialized = true;
                cards[1].GetComponent<CardDesafioPaleo>().initialized = true;
                cards[4].GetComponent<CardDesafioPaleo>().initialized = true;
                cards[5].GetComponent<CardDesafioPaleo>().initialized = true;

                cards[0].GetComponent<CardDesafioPaleo>().setupGraphics(1);
                cards[1].GetComponent<CardDesafioPaleo>().setupGraphics(1);
                cards[4].GetComponent<CardDesafioPaleo>().setupGraphics(1);
                cards[5].GetComponent<CardDesafioPaleo>().setupGraphics(1);
                cards[6].GetComponent<CardDesafioPaleo>().setupGraphics(0);           
               
                break;
            
            case 1:
                cards[1].GetComponent<CardDesafioPaleo>().cardValue = 1;
                cards[2].GetComponent<CardDesafioPaleo>().cardValue = 2;
                cards[5].GetComponent<CardDesafioPaleo>().cardValue = 3;
                cards[6].GetComponent<CardDesafioPaleo>().cardValue = 4;

                cards[1].GetComponent<CardDesafioPaleo>().initialized = true;
                cards[2].GetComponent<CardDesafioPaleo>().initialized = true;
                cards[5].GetComponent<CardDesafioPaleo>().initialized = true;
                cards[6].GetComponent<CardDesafioPaleo>().initialized = true;

                cards[1].GetComponent<CardDesafioPaleo>().setupGraphics(1);
                cards[2].GetComponent<CardDesafioPaleo>().setupGraphics(1);
                cards[5].GetComponent<CardDesafioPaleo>().setupGraphics(1);
                cards[6].GetComponent<CardDesafioPaleo>().setupGraphics(1);
                cards[7].GetComponent<CardDesafioPaleo>().setupGraphics(0);  
                break;

            case 2:
                cards[2].GetComponent<CardDesafioPaleo>().cardValue = 1;
                cards[3].GetComponent<CardDesafioPaleo>().cardValue = 2;
                cards[6].GetComponent<CardDesafioPaleo>().cardValue = 3;
                cards[7].GetComponent<CardDesafioPaleo>().cardValue = 4;

                cards[2].GetComponent<CardDesafioPaleo>().initialized = true;
                cards[3].GetComponent<CardDesafioPaleo>().initialized = true;
                cards[6].GetComponent<CardDesafioPaleo>().initialized = true;
                cards[7].GetComponent<CardDesafioPaleo>().initialized = true;

                cards[2].GetComponent<CardDesafioPaleo>().setupGraphics(1);
                cards[3].GetComponent<CardDesafioPaleo>().setupGraphics(1);
                cards[6].GetComponent<CardDesafioPaleo>().setupGraphics(1);
                cards[7].GetComponent<CardDesafioPaleo>().setupGraphics(1);
                cards[8].GetComponent<CardDesafioPaleo>().setupGraphics(0);  
                break;

            case 3:
                cards[4].GetComponent<CardDesafioPaleo>().cardValue = 1;
                cards[5].GetComponent<CardDesafioPaleo>().cardValue = 2;
                cards[8].GetComponent<CardDesafioPaleo>().cardValue = 3;
                cards[9].GetComponent<CardDesafioPaleo>().cardValue = 4;

                cards[4].GetComponent<CardDesafioPaleo>().initialized = true;
                cards[5].GetComponent<CardDesafioPaleo>().initialized = true;
                cards[8].GetComponent<CardDesafioPaleo>().initialized = true;
                cards[9].GetComponent<CardDesafioPaleo>().initialized = true;

                cards[4].GetComponent<CardDesafioPaleo>().setupGraphics(1);
                cards[5].GetComponent<CardDesafioPaleo>().setupGraphics(1);
                cards[8].GetComponent<CardDesafioPaleo>().setupGraphics(1);
                cards[9].GetComponent<CardDesafioPaleo>().setupGraphics(1);
                cards[10].GetComponent<CardDesafioPaleo>().setupGraphics(0);
                break;

            case 4:
                cards[5].GetComponent<CardDesafioPaleo>().cardValue = 1;
                cards[6].GetComponent<CardDesafioPaleo>().cardValue = 2;
                cards[9].GetComponent<CardDesafioPaleo>().cardValue = 3;
                cards[10].GetComponent<CardDesafioPaleo>().cardValue = 4;

                cards[5].GetComponent<CardDesafioPaleo>().initialized = true;
                cards[6].GetComponent<CardDesafioPaleo>().initialized = true;
                cards[9].GetComponent<CardDesafioPaleo>().initialized = true;
                cards[10].GetComponent<CardDesafioPaleo>().initialized = true;

                cards[5].GetComponent<CardDesafioPaleo>().setupGraphics(1);
                cards[6].GetComponent<CardDesafioPaleo>().setupGraphics(1);
                cards[9].GetComponent<CardDesafioPaleo>().setupGraphics(1);
                cards[10].GetComponent<CardDesafioPaleo>().setupGraphics(1);
                cards[11].GetComponent<CardDesafioPaleo>().setupGraphics(0);
                break;

            case 5:
                cards[6].GetComponent<CardDesafioPaleo>().cardValue = 1;
                cards[7].GetComponent<CardDesafioPaleo>().cardValue = 2;
                cards[10].GetComponent<CardDesafioPaleo>().cardValue = 3;
                cards[11].GetComponent<CardDesafioPaleo>().cardValue = 4;

                cards[6].GetComponent<CardDesafioPaleo>().initialized = true;
                cards[7].GetComponent<CardDesafioPaleo>().initialized = true;
                cards[10].GetComponent<CardDesafioPaleo>().initialized = true;
                cards[11].GetComponent<CardDesafioPaleo>().initialized = true;

                cards[6].GetComponent<CardDesafioPaleo>().setupGraphics(1);
                cards[7].GetComponent<CardDesafioPaleo>().setupGraphics(1);
                cards[10].GetComponent<CardDesafioPaleo>().setupGraphics(1);
                cards[11].GetComponent<CardDesafioPaleo>().setupGraphics(1);
                cards[12].GetComponent<CardDesafioPaleo>().setupGraphics(0);
                break;

            case 6:
                cards[8].GetComponent<CardDesafioPaleo>().cardValue = 1;
                cards[9].GetComponent<CardDesafioPaleo>().cardValue = 2;
                cards[12].GetComponent<CardDesafioPaleo>().cardValue = 3;
                cards[13].GetComponent<CardDesafioPaleo>().cardValue = 4;

                cards[8].GetComponent<CardDesafioPaleo>().initialized = true;
                cards[9].GetComponent<CardDesafioPaleo>().initialized = true;
                cards[12].GetComponent<CardDesafioPaleo>().initialized = true;
                cards[13].GetComponent<CardDesafioPaleo>().initialized = true;

                cards[8].GetComponent<CardDesafioPaleo>().setupGraphics(1);
                cards[9].GetComponent<CardDesafioPaleo>().setupGraphics(1);
                cards[12].GetComponent<CardDesafioPaleo>().setupGraphics(1);
                cards[13].GetComponent<CardDesafioPaleo>().setupGraphics(1);
                cards[14].GetComponent<CardDesafioPaleo>().setupGraphics(0);
                break;

            case 7:
                cards[9].GetComponent<CardDesafioPaleo>().cardValue = 1;
                cards[10].GetComponent<CardDesafioPaleo>().cardValue = 2;
                cards[13].GetComponent<CardDesafioPaleo>().cardValue = 3;
                cards[14].GetComponent<CardDesafioPaleo>().cardValue = 4;

                cards[9].GetComponent<CardDesafioPaleo>().initialized = true;
                cards[10].GetComponent<CardDesafioPaleo>().initialized = true;
                cards[13].GetComponent<CardDesafioPaleo>().initialized = true;
                cards[14].GetComponent<CardDesafioPaleo>().initialized = true;

                cards[9].GetComponent<CardDesafioPaleo>().setupGraphics(1);
                cards[10].GetComponent<CardDesafioPaleo>().setupGraphics(1);
                cards[13].GetComponent<CardDesafioPaleo>().setupGraphics(1);
                cards[14].GetComponent<CardDesafioPaleo>().setupGraphics(1);
                cards[15].GetComponent<CardDesafioPaleo>().setupGraphics(0);
                break;

            case 8:
                cards[10].GetComponent<CardDesafioPaleo>().cardValue = 1;
                cards[11].GetComponent<CardDesafioPaleo>().cardValue = 2;
                cards[14].GetComponent<CardDesafioPaleo>().cardValue = 3;
                cards[15].GetComponent<CardDesafioPaleo>().cardValue = 4;

                cards[10].GetComponent<CardDesafioPaleo>().initialized = true;
                cards[11].GetComponent<CardDesafioPaleo>().initialized = true;
                cards[14].GetComponent<CardDesafioPaleo>().initialized = true;
                cards[15].GetComponent<CardDesafioPaleo>().initialized = true;

                cards[10].GetComponent<CardDesafioPaleo>().setupGraphics(1);
                cards[11].GetComponent<CardDesafioPaleo>().setupGraphics(1);
                cards[14].GetComponent<CardDesafioPaleo>().setupGraphics(1);
                cards[15].GetComponent<CardDesafioPaleo>().setupGraphics(1);
                cards[16].GetComponent<CardDesafioPaleo>().setupGraphics(0);
                break;
        }
       
    }

    public Sprite getCardBack()
    {
        return cardBack;
    }

    public Sprite getCardFace(int i)
    {
        switch (num_fossil)
            {
                case 0:
                    Debug.Log("entrou 0");
                    return Parameters.MEMORY_ROUNDINDEX == 0 ? fossil0[i-1] : null;
                    break;
                case 1:
                    Debug.Log("entrou 1");
                    return Parameters.MEMORY_ROUNDINDEX == 0 ? fossil1[i-1] : null;
                    break;
                case 2:
                    Debug.Log("entrou 2");
                    return Parameters.MEMORY_ROUNDINDEX == 0 ? fossil2[i-1] : null;
                    break;
                case 3:
                    Debug.Log("entrou 3");
                    return Parameters.MEMORY_ROUNDINDEX == 0 ? fossil3[i-1] : null;
                    break;
                case 4:
                    Debug.Log("entrou 4");
                    return Parameters.MEMORY_ROUNDINDEX == 0 ? fossil4[i-1] : null;
                    break;
                case 5:
                    Debug.Log("entrou 5");
                    return Parameters.MEMORY_ROUNDINDEX == 0 ? fossil5[i-1] : null;
                    break;
                case 6:
                    Debug.Log("entrou 6");
                    return Parameters.MEMORY_ROUNDINDEX == 0 ? fossil6[i-1] : null;
                    break;
                case 7:
                    Debug.Log("entrou 7");
                    return Parameters.MEMORY_ROUNDINDEX == 0 ? fossil7[i-1] : null;
                    break;
                case 8:
                    Debug.Log("entrou 8");
                    return Parameters.MEMORY_ROUNDINDEX == 0 ? fossil8[i-1] : null;
                    break;
                case 9:
                    Debug.Log("entrou 9");
                    return Parameters.MEMORY_ROUNDINDEX == 0 ? fossil9[i-1] : null;
                    break;
                case 10:
                    Debug.Log("entrou 10");
                    return Parameters.MEMORY_ROUNDINDEX == 0 ? fossil10[i-1] : null;
                    break;
                case 11:
                    Debug.Log("entrou 11");
                    return Parameters.MEMORY_ROUNDINDEX == 0 ? fossil11[i-1] : null;
                    break;
                case 12:
                    Debug.Log("entrou 12");
                    return Parameters.MEMORY_ROUNDINDEX == 0 ? fossil12[i-1] : null;
                    break;
                case 13:
                    Debug.Log("entrou 13");
                    return Parameters.MEMORY_ROUNDINDEX == 0 ? fossil13[i-1] : null;
                    break;
                case 14:
                    Debug.Log("entrou 14");
                    return Parameters.MEMORY_ROUNDINDEX == 0 ? fossil14[i-1] : null;
                    break;
                case 15:
                    Debug.Log("entrou 15");
                    return Parameters.MEMORY_ROUNDINDEX == 0 ? fossil15[i-1] : null;
                    break;
                case 16:
                    Debug.Log("entrou 16");
                    return Parameters.MEMORY_ROUNDINDEX == 0 ? fossil16[i-1] : null;
                    break;
                case 17:
                    Debug.Log("entrou 17");
                    return Parameters.MEMORY_ROUNDINDEX == 0 ? fossil17[i-1] : null;
                    break;
                case 18:
                    Debug.Log("entrou 18");
                    return Parameters.MEMORY_ROUNDINDEX == 0 ? fossil18[i-1] : null;
                    break;
                case 19:
                    Debug.Log("entrou 19");
                    return Parameters.MEMORY_ROUNDINDEX == 0 ? fossil19[i-1] : null;
                    break;
                case 20:
                    Debug.Log("entrou 20");
                    return Parameters.MEMORY_ROUNDINDEX == 0 ? fossil20[i-1] : null;
                    break;
                case 21:
                    Debug.Log("entrou 21");
                    return Parameters.MEMORY_ROUNDINDEX == 0 ? fossil21[i-1] : null;
                    break;
                case 22:
                    Debug.Log("entrou 22");
                    return Parameters.MEMORY_ROUNDINDEX == 0 ? fossil22[i-1] : null;
                    break;
                case 23:
                    Debug.Log("entrou 23");
                    return Parameters.MEMORY_ROUNDINDEX == 0 ? fossil23[i-1] : null;
                    break;
                case 24:
                    Debug.Log("entrou 24");
                    return Parameters.MEMORY_ROUNDINDEX == 0 ? fossil24[i-1] : null;
                    break;
                case 25:
                    Debug.Log("entrou 25");
                    return Parameters.MEMORY_ROUNDINDEX == 0 ? fossil25[i-1] : null;
                    break;
                case 26:
                    Debug.Log("entrou 26");
                    return Parameters.MEMORY_ROUNDINDEX == 0 ? fossil26[i-1] : null;
                    break;
                default:
                   return null;
                    break;
            }
    }

    /*public Sprite getCardText(int i)
    {
        return Parameters.MEMORY_ROUNDINDEX == 0 ? fossil1[i - 1] : null; //tirar
    }*/

    /*void checkCards()
    {
        c = new List<int>();

        for (int i = 0; i < cards.Length; i++)
        {
            if (cards[i].GetComponent<CardDesafioPaleo>().state == Card.VIRADA_CIMA && !_first)
            {
                Debug.Log("carta adicionada >> " + cards[i]);
                c.Add(i);
                Debug.Log("após adicionar carta >> " + c.Count);
            }
        }
    }*/

    public void EndGame(bool win)
    {
        BigImage1.SetActive(false);
        BigImage2.SetActive(false);

        if (win)
        {
            WinImage.SetActive(true);
            //WinImage.GetComponentInChildren<Button>().Select();

            if (!PlayerPreferences.M004_TeiaAlimentar)
                WinText.text = "Parabéns, você ganhou a câmera fotográfica para realizar a missão, mas ainda falta um item.";
            else
                WinText.text = "Parabéns, você ganhou a câmera fotográfica. Agora você já tem os itens necessários para realizar a missão.";

            lifeExpController.AddEXP(0.001f); // finalizou o minijogo
            lifeExpController.AddEXP(0.0002f); // ganhou o item
        }
        else
        {
            LoseImage.SetActive(true);
            resetButton.Select();
            lifeExpController.AddEXP(0.0001f); // jogou um minijogo
        }

        StartCoroutine(ReturnToCampCoroutine()); // volta para o navio perdendo ou ganhando o minijogo
    }

    public void ReturnToShip()
    {
        if (!PlayerPreferences.M009_Memoria) lifeExpController.RemoveEXP(0.0001f); // saiu sem concluir o minijogo
        UnityEngine.SceneManagement.SceneManager.LoadScene(ScenesNames.M009Camp);
    }

    public void ResetScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(ScenesNames.M009Desafio);
    }

    public IEnumerator ReadCards()
    {
        GameObject[] tmpCards = cards;
        
        // ordenar o array baseado no numero da carta (posicao)
        Array.Sort(tmpCards, delegate (GameObject g1, GameObject g2)
        {
            return int.Parse(new string(g1.name.Where(char.IsDigit).ToArray())).CompareTo(
                int.Parse(new string(g2.name.Where(char.IsDigit).ToArray())));
            //return string.Join(string.Empty, Regex.Matches(g1.gameObject.name, @"\d+").OfType<Match>().Select(m => m.Value)).CompareTo(
            //    string.Join(string.Empty, Regex.Matches(g2.gameObject.name, @"\d+").OfType<Match>().Select(m => m.Value)));
        });

        // imprime e le o conteudo a cada meio segundo (tempo que as cartas ficarão abertas no início)
        for (int i = 0; i < tmpCards.Length; i++)
        {
            string objectName;

            switch (missionName)
            {
                case "baleias":
                    objectName = CardsDescription.GetCardText(tmpCards[i].name);
                    break;
                case "paleo":
                    objectName = CardsDescriptionPaleo.GetCardDescriptionPaleo(tmpCards[i].name);
                    break;
                default:
                    objectName = "";
                    Debug.Log("check mission name");
                    break;
            }

            //Debug.Log(objectName != null ? (tmpCards[i].name.Substring(0, tmpCards[i].name.IndexOf(":")) + ": " + objectName) : tmpCards[i].gameObject.name);

            //tmpCards[i].GetComponent<Button>().Select();
            yield return new WaitForSeconds(0.5f);
        }
    }

    public IEnumerator showCards()
    {
        for (int i = 0; i < cards.Length; i++)
        {
            //if (!cards[i].GetComponent<Card>().isText)
            cards[i].GetComponent<CardDesafioPaleo>().state = Card.VIRADA_BAIXO;

            cards[i].GetComponent<CardDesafioPaleo>().turnCardDown();
        }

        yield return new WaitForSeconds(4);

        _first = false;
    }

    public IEnumerator ReturnToCampCoroutine()
    {
        yield return new WaitForSeconds(7f);

        UnityEngine.SceneManagement.SceneManager.LoadScene(ScenesNames.M009Camp);
    }

    public void VerItem()
    {
        if(!pega_item[0].GetComponentInChildren<DragAndDropItem>() || !pega_item[1].GetComponentInChildren<DragAndDropItem>()|| !pega_item[2].GetComponentInChildren<DragAndDropItem>()|| !pega_item[3].GetComponentInChildren<DragAndDropItem>())
        {
            Itens.Remove(item00);
            Itens.Remove(item01);
            Itens.Remove(item02);
            Itens.Remove(item03);
            Itens.Clear();
            item00 = "";
            item01 = "";
            item02 = "";
            item03 = "";
            Debug.Log("nao tem nada00: " +item00);
        }
        if(pega_item[0].GetComponentInChildren<DragAndDropItem>() || pega_item[1].GetComponentInChildren<DragAndDropItem>() || pega_item[2].GetComponentInChildren<DragAndDropItem>() || pega_item[3].GetComponentInChildren<DragAndDropItem>())
        {   
            if(pega_item[0].GetComponentInChildren<DragAndDropItem>())
            {
                Itens.Add(pega_item[0].GetComponentInChildren<DragAndDropItem>().name);
                item00 = pega_item[0].GetComponentInChildren<DragAndDropItem>().name;
                Debug.Log("tem item: " +item00);
            }
            if(pega_item[1].GetComponentInChildren<DragAndDropItem>())
            {
                Itens.Add(pega_item[1].GetComponentInChildren<DragAndDropItem>().name);
                item01 = pega_item[1].GetComponentInChildren<DragAndDropItem>().name;
                Debug.Log("tem item: " +item01);
            }
            if(pega_item[2].GetComponentInChildren<DragAndDropItem>())
            {
                Itens.Add(pega_item[2].GetComponentInChildren<DragAndDropItem>().name);
                item02 = pega_item[2].GetComponentInChildren<DragAndDropItem>().name;
                Debug.Log("tem item: " +item02);
            }
            if(pega_item[3].GetComponentInChildren<DragAndDropItem>())
            {
                Itens.Add(pega_item[3].GetComponentInChildren<DragAndDropItem>().name);
                item02 = pega_item[3].GetComponentInChildren<DragAndDropItem>().name;
                Debug.Log("tem item: " +item03);
            }
        }
	}

    public void usaKit()
    {
        int valor = Convert.ToInt32(kitValor.text);

        if(valor > 0)
        {
            lifeExpController.GetComponent<LifeExpController>().HPImage.fillAmount += ganha / 100;
            Debug.Log("Tem kit saúde");
            Debug.Log(lifeExpController.GetComponent<LifeExpController>().HPImage.fillAmount);
            valor--;
            kitValor.text = valor.ToString();
            Debug.Log("Você usou um Kit de saúde, restam: " + kitValor.text + " kits.");
            ReadText("Você usou um Kit de saúde, restam: " + kitValor.text + " kits."); 
		}  
        else
        {
            Debug.Log("Você não possui mais nenhum kit de saúde.");
            ReadText("Você não possui mais nenhum kit de saúde.");
		}
	}

    void ReadEquipamento(GameObject nextCell)
    {
        
        // navegando por itens vazios
        if (nextCell.GetComponentInChildren<DragAndDropItem>() == null)
        {
            if(nextCell.GetComponent<Button>() != null)
            {
                Debug.Log("Usar kit de saúde.");
                ReadText("Usar kit de saúde.");
            }
            else
            {
                Debug.Log("Item vazio");
                ReadText("Item vazio");
            }
        }
        else if (nextCell.GetComponentInChildren<DragAndDropItem>() != null)  // navegando por itens ainda com conteudo
        {
            if(nextCell.GetComponentInChildren<DragAndDropItem>().gameObject.name == "kit")
            {
                Debug.Log("Item equipado: Kit de saúde. Quantidade: " + kitValor.text);
                ReadText("Item equipado: Kit de saúde. Quantidade: " + kitValor.text);                
			}
            else
            {
            Debug.Log("Item:" + nextCell.GetComponentInChildren<DragAndDropItem>().gameObject.name);
            ReadText("Item:" + nextCell.GetComponentInChildren<DragAndDropItem>().gameObject.name);
            }
        }
    }

    void ReadEquipados(GameObject nextCell)
    {
        // navegando por itens vazios
        if (nextCell.GetComponentInChildren<DragAndDropItem>() == null)
        {
                Debug.Log("Nenhum item equipado.");
                ReadText("Nenhum item equipado.");
        }
        else // navegando por itens ainda com conteudo
        {
            Debug.Log("Item equipado:" + nextCell.GetComponentInChildren<DragAndDropItem>().gameObject.name);
            ReadText("Item equipado:" + nextCell.GetComponentInChildren<DragAndDropItem>().gameObject.name);
        }
    }

    void ReadCamp(GameObject nextCell)
    {
        string verifica = nextCell.GetComponent<Image>().sprite.name;

        int index = Array.IndexOf(cards,nextCell) + 1;   

        if(verifica.Contains("solo0"))
        {
            Debug.Log("Solo " + index + " na profundidade 0");
            ReadText("Solo " + index + " na profundidade 0");
        }
        else if(verifica.Contains("solo1"))
        {
            Debug.Log("Solo " + index + " na profundidade 1");
            ReadText("Solo " + index + " na profundidade 1");
        }

        else if(verifica.Contains("solo2"))
        {
            Debug.Log("Solo " + index + " na profundidade 2");
            ReadText("Solo " + index + " na profundidade 2");
        }

        else if(verifica.Contains("solo3"))
        {
            Debug.Log("Solo " + index + " na profundidade 3");
            ReadText("Solo " + index + " na profundidade 3");
        }

        else
        {
            Debug.Log("Parte de fóssil encontrada");
            ReadText("Parte de fóssil encontrada");
        }
        
    }

}
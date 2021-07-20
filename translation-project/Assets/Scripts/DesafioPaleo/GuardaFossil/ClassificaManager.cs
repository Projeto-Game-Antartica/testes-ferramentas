using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ClassificaManager : AbstractScreenReader
{
    public FossilData fossilData;
    public FossilController fossilController;

    public DesafioManagerPaleo desafioManagerPaleo;

    public CardDesafioPaleo cards_Wall;

    private int id_fossil;
    private string caracteristica;
    private string classificacao;
    private string era;
    private string image_path;

    public Button guardarButton;

    public FossilImages fossilImages;


    private readonly string instructions = "Início do jogo. Mini jogo de memória. Descrição..";

    public GameObject[] cards;

    public Button[] Botao;

    //public Button backButton;
    //public Button resetButton;
    public Button audioButton;
    public Button confirmarButton;
    public Button cancelButton;

    private bool init;

    private bool verificado = true;

    private AudioSource audioSource;

    public AudioClip correctAudio;
    public AudioClip wrongAudio;

    private List<int> c;

    public GameObject WinImage;
    public GameObject LoseImage;

    //public TMPro.TMP_Dropdown processDropDown;

    // hint settings
    public MinijogosDicas dicas;

    public LifeExpController lifeExpController;

    public GameObject instruction_interface;

    private enum Operation { correct, wrong }

    private int attempts = 3;
    private int tries = 0;

    private int harvNumber = 1;

    public TMPro.TextMeshProUGUI attemptsText;

    public void Start()
    {
        guardarButton.interactable = false;

        //desafioManagerPaleo = GameObject.FindGameObjectWithTag("GameController");

        //resetButton.interactable = false;

        init = false;

        ReadText(instructions);

        audioSource = GetComponent<AudioSource>();

        initializeGame();
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetMouseButtonUp(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)) && !ClassificaCard.DO_NOT)
        {
            //Debug.Log("Checando cartas....");
            Debug.Log("CHECK CARD");
            checkCards(); 
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            audioButton.Select();
        }

        if (Input.GetKeyDown(KeyCode.F6))
        {
            cards[0].GetComponent<Button>().Select();
        }

        if (c != null && c.Count >= 3)
        {
            ClassificaCard.DO_NOT = true;
            confirmarButton.interactable = true;
            cancelButton.interactable = true;

            if(verificado)
            {
                confirmarButton.GetComponent<Selectable>().Select();
                verificado = false;
            }

            //Debug.Log(c.Count);
        }
        else if (c != null && c.Count > 0)
        {
            //processDropDown.interactable = false;
            cancelButton.interactable = true;
        }
        else
        {
            confirmarButton.interactable = false;
            cancelButton.interactable = false;
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            instruction_interface.SetActive(true);
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            instruction_interface.SetActive(false);
        }
    }

    public void initializeGame()
    {
        if (!init)
            initializeCards();

        c = new List<int>();

        //backButton.interactable = true;
        //resetButton.interactable = true;

        id_fossil = fossilData.id_fossil;
        caracteristica = fossilData.caracteristica;
        classificacao = fossilData.classificacao;
        era = fossilData.era;

        Debug.Log("asd asd asd " + caracteristica);

        fossilImages.SetPhotographedWhaleImage(fossilData.image_path);
    }

    public void CallHintMethod()
    {
        dicas.StartHints();
    }

    public void initializeCards()
    {
            Debug.Log("teste");
            Debug.Log("numero = " +desafioManagerPaleo.GetComponent<DesafioManagerPaleo>().num_fossil);
            fossilData = fossilController.getFossilById(desafioManagerPaleo.GetComponent<DesafioManagerPaleo>().num_fossil);


        for (int i = 0; i < cards.Length; i++)
        {
            Debug.Log("valor:" + i);
            bool test = false;
            int choice = 0;

            cards[i].GetComponent<ClassificaCard>().cardValue = i;
            cards[i].GetComponent<ClassificaCard>().initialized = true;

            cards[i].GetComponent<ClassificaCard>().setupGraphics();
            //cards.state = cards.VIRADA_BAIXO;
            
        }
        
        if (!init)
        {
            init = true;
            cards[0].GetComponent<Button>().Select();
            desafioManagerPaleo.GetComponent<DesafioManagerPaleo>().ReadCaracteristicas(cards[0]);
            StartCoroutine(ReadCards());
        }
    }

    void checkCards()
    {
        for (int i = 0; i < cards.Length; i++)
        {
            var card = cards[i].GetComponent<ClassificaCard>();
            //card.state = ClassificaCard.VIRADA_BAIXO;
            if (card.state == ClassificaCard.VIRADA_CIMA && !card.added)
            {
                Debug.Log("carta adicionada >> " + cards[i]);
                c.Add(i);

                Debug.Log("num:" +i);

                card.BGImage.color = new Color(1, 1, 0, 1);

                //card.BGImage.color = GetColor(GetDropDownValue());
                card.added = true;

               if(i == 0)
               {
                    Botao[3].interactable = false;
                    Botao[6].interactable = false;
               }
                else if(i == 3)
               {
                    Botao[0].interactable = false;
                    Botao[6].interactable = false;
               }
                else if(i == 6)
               {
                    Botao[0].interactable = false;
                    Botao[3].interactable = false;
               }

               if(i == 1)
               {
                    Botao[4].interactable = false;
                    Botao[7].interactable = false;
               }
                else if(i == 4)
               {
                    Botao[1].interactable = false;
                    Botao[7].interactable = false;
               }
                else if(i == 7)
               {
                    Botao[1].interactable = false;
                    Botao[4].interactable = false;
               }

               if(i == 2)
               {
                    Botao[5].interactable = false;
                    Botao[8].interactable = false;
               }
                else if(i == 5)
               {
                    Botao[2].interactable = false;
                    Botao[8].interactable = false;
               }
                else if(i == 8)
               {
                    Botao[2].interactable = false;
                    Botao[5].interactable = false;
               }
            }
        }

        //Debug.Log("Após checar cartas >> " + c.Count);

        //if (c.Count == GetRemainingOptions(GetDropDownValue()))
        //    cardComparison(c);
    }

    public void verifica()
    {
    verificado = false;

        for(int i=0; i <= 2; i++)
        {
            var card = cards[c[i]].GetComponent<ClassificaCard>();

            if(cards[c[i]].name == "Carta1")
            {
                if(fossilData.caracteristica == "Concreção")
                    {
                        Debug.Log("Acerto");
                        card.BGImage.color = new Color(0, 1, 0, 1); 
                    }
                    else
                        card.BGImage.color =new Color(1, 0, 0, 1);         
            }
            if(cards[c[i]].name == "Carta2")
            {
                if(fossilData.classificacao == "Invertebrado")
                    {
                        Debug.Log("Acerto");
                        card.BGImage.color = new Color(0, 1, 0, 1); 
                    } 
                    else
                        card.BGImage.color =new Color(1, 0, 0, 1);
		    }
            if(cards[c[i]].name == "Carta3")
            {
                if(fossilData.era == "Cenozóico")
                    {
                        Debug.Log("Acerto");
                        card.BGImage.color = new Color(0, 1, 0, 1); 
                    }
                    else
                        card.BGImage.color =new Color(1, 0, 0, 1);
		    }
            if(cards[c[i]].name == "Carta4")
            {
                if(fossilData.caracteristica == "Fora")
                    {
                        Debug.Log("Acerto");
                        card.BGImage.color = new Color(0, 1, 0, 1); 
                    }
                    else
                        card.BGImage.color =new Color(1, 0, 0, 1);
		    }
            if(cards[c[i]].name == "Carta5")
            {
                if(fossilData.classificacao == "Vertebrado")
                    {
                        Debug.Log("Acerto");
                        card.BGImage.color = new Color(0, 1, 0, 1); 
                    }
                    else
                        card.BGImage.color =new Color(1, 0, 0, 1);
		    }
            if(cards[c[i]].name == "Carta6")
            {
                  if(fossilData.era == "Mesozóico")
                    {
                        Debug.Log("Acerto");
                        card.BGImage.color = new Color(0, 1, 0, 1); 
                    }
                    else
                        card.BGImage.color =new Color(1, 0, 0, 1);
		    }
            if(cards[c[i]].name == "Carta7")
            {
                  if(fossilData.caracteristica == "Molde")
                    {
                        Debug.Log("Acerto");
                        card.BGImage.color = new Color(0, 1, 0, 1); 
                    }
                    else
                        card.BGImage.color =new Color(1, 0, 0, 1);
		    }
            if(cards[c[i]].name == "Carta8")
            {
                  if(fossilData.classificacao == "Vegetal")
                    {
                        Debug.Log("Acerto");
                        card.BGImage.color = new Color(0, 1, 0, 1); 
                    }
                    else
                        card.BGImage.color =new Color(1, 0, 0, 1);
		    }
            if(cards[c[i]].name == "Carta9")
            {
                  if(fossilData.era == "Paleozóico")
                    {
                        Debug.Log("Acerto");
                        card.BGImage.color = new Color(0, 1, 0, 1); 
                    }
                    else
                        card.BGImage.color =new Color(1, 0, 0, 1);
		    }
        }

        Debug.Log(fossilData.id_fossil);
        Debug.Log(fossilData.caracteristica);
        Debug.Log(fossilData.classificacao);
        Debug.Log(fossilData.era);

         //Debug.Log(fossilData.description);
         cancelButton.interactable = false;
         guardarButton.interactable = true;

         guardarButton.GetComponent<Selectable>().Select();

	}

    public void Cancel()
    {
        //int dropDownValue = GetDropDownValue();

        verificado = true;
        
        for (int i = 0; i < c.Count; i++)
        {
            var card = cards[c[i]].GetComponent<ClassificaCard>();

            cards[c[i]].GetComponent<ClassificaCard>().state = ClassificaCard.VIRADA_BAIXO;
            cards[c[i]].GetComponent<ClassificaCard>().turnCardDown();
            //cards[c[i]].GetComponent<ClassificaCard>().BGImage.color = GetColor(-1);
            cards[c[i]].GetComponent<ClassificaCard>().added = false;
            card.BGImage.color = new Color(0, 0, 0, 0);

        }

        c.Clear();
        cards[0].GetComponent<Button>().Select();
        //processDropDown.interactable = true;

        guardarButton.interactable = false;

        confirmarButton.interactable = false;

        for (int i = 0; i < 9; i++)
        {
            Botao[i].interactable = true;
        }
    }

    public void EndGame()
    {   
        desafioManagerPaleo.audioSource.PlayOneShot(desafioManagerPaleo.GetComponent<DesafioManagerPaleo>().guarda_saco);

        harvNumber++;
        
        if(harvNumber <= 3) {
            DoAfter(3, resetHarvestAndShowDialog);
        } else {
            DoAfter(3, showWinScreen);
        }

    }

    private void resetHarvestAndShowDialog() {
        //turnCardDown
        resetScene();
        desafioManagerPaleo.ShowOkDialog("Parabéns, fóssil classificado. Realize uma nova escavação e classificação.", desafioManagerPaleo.ShowHarvestScreen);             
    }

    private void showWinScreen() {
        WinImage.SetActive(true);
        lifeExpController.AddEXP(PlayerPreferences.XPwinPuzzle); // finalizou o minijogo
        lifeExpController.AddEXP(3*PlayerPreferences.XPwinItem); // ganhou o item  

        ReadText(ReadableTexts.instance.GetReadableText(ReadableTexts.key_m009_desafio_vitoria, LocalizationManager.instance.GetLozalization()));

        PlayerPreferences.M009_Desafio_Done = true;
 
        DoAfter(5, ReturnToCamp);
    }

    public void DoAfter(int secs, UnityAction action) {
        StartCoroutine(DoAfterCoroutine(secs, action));
    }

    public IEnumerator DoAfterCoroutine(int secs, UnityAction action) {
        yield return new WaitForSeconds(secs);
        action();
    }

    public void ReturnToCamp()
    {
        //if (!PlayerPreferences.M004_Memoria) lifeExpController.RemoveEXP(0.0001f); // saiu sem concluir o minijogo
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
            string objectName = CardsDescription.GetCardText(tmpCards[i].name);
            //Debug.Log(objectName != null ? (tmpCards[i].name.Substring(0, tmpCards[i].name.IndexOf(":")) + ": " + objectName) : tmpCards[i].gameObject.name);

            //tmpCards[i].GetComponent<Button>().Select();
            yield return new WaitForSeconds(0.5f);
        }
    }

    public IEnumerator ReturnToCampCoroutine()
    {
        yield return new WaitForSeconds(7f);

        UnityEngine.SceneManagement.SceneManager.LoadScene(ScenesNames.M009Desafio);
    }

    public void resetScene()
    {
        Cancel();

        guardarButton.interactable = false;
        confirmarButton.interactable = false;
        cancelButton.interactable = false;
	}
}

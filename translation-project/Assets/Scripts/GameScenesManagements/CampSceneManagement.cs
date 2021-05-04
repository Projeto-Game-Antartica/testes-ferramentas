using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

// attached to the character
public class CampSceneManagement : AbstractScreenReader {

    private bool isTrigger;
    public SimpleCharacterController character;
    public ChasingCamera chasingCamera;

    public MentorController mentorController;

    private Vector2 positionSceneChange;

    private Collider2D colliderControl = null;

    public GameObject warningInterface;
    public TextMeshProUGUI warningText;

    public GameObject dialogueInterface;
    public TextMeshProUGUI dialogueInterfaceText;
    public TextMeshProUGUI dialogueInterfacePlayerName;

    // instruction settings
    public GameObject instructionInterface;
    public TextMeshProUGUI missionName;
    public TextMeshProUGUI descriptionText;

    //public string sceneDescription;

    public AudioSource audioSource;
    public AudioClip warningClip;

    public BagController Bag;

    private string missionNumber = "M009";

    //private string initialInstruction = "Conheça o navio e converse com os pesquisadores para novos desafios.";

    public void Start()
    {
        isTrigger = false;

        //Flag to set itens to true for debug
        bool debugItens = true;
        if(debugItens) {
            PlayerPreferences.M009_Memoria = true;
            PlayerPreferences.M009_Eras = true;
            PlayerPreferences.M009_Itens = true;
        }
        
        
        if(PlayerPreferences.M009_Memoria) {
            Bag.EnableItemByIndex(4);
        }

        if(PlayerPreferences.M009_Eras) {
            Bag.EnableItemByIndex(0);
            Bag.EnableItemByIndex(2);
            Bag.EnableItemByIndex(1);
        }

        if(PlayerPreferences.M009_Itens) {
            Bag.EnableItemByIndex(3);
            Bag.EnableItemByIndex(5);
        }

        if (PlayerPrefs.GetInt("Saved_"+missionNumber) == 1)
        {
            transform.position = character.GetPosition(missionNumber);
            chasingCamera.SetCameraPosition(character.GetPosition(missionNumber));
            Debug.Log(transform.position);
        }

        //Show instructions in case game has not yet been initialized
        instructionInterface.SetActive(!PlayerPreferences.M009_Initialized);

    }

    private void Update()
    {
        if (isTrigger && Input.GetKeyDown(KeyCode.Return))
        {
            positionSceneChange = new Vector3(transform.position.x, transform.position.y);
            // save the position when loading another scene
            character.SavePosition(positionSceneChange, missionNumber);


            if (colliderControl.name.Equals("Figurante") && PlayerPreferences.finishedAllM009Games())
                SceneManager.LoadScene(ScenesNames.M009Desafio);
        }

        //if (Input.GetKeyDown(KeyCode.F2))
        //{
        //    if (!instructionInterface.activeSelf)
        //    {
        //        ReadSceneDescription();
        //    }
        //}
    }

    public void InitMission() {
        PlayerPreferences.M009_Initialized = true;
        instructionInterface.SetActive(false);
    }

    //public void ReadSceneDescription()
    //{
    //    ReadText(sceneDescription);
    //    Debug.Log(sceneDescription);
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("scene-trigger");
        
        //if (collision.name.Equals("cabine principal"))
        //{
        //    warningInterface.SetActive(true);
        //    warningText.text = "Pressione E para entrar no passadiço do navio.";
        //}
        //else 
       
       /*if(collision.name.Equals("Mentor4"))
        {
            Debug.Log("trombo");
            mentorController.GetComponent<MentorController>().GetDialogue("M009", "Mentor4", 0)
            if (PlayerPreferences.finishedAllM009Games())
            {
                warningInterface.SetActive(true);

                ReadText(ReadableTexts.instance.GetReadableText(ReadableTexts.key_gameplay_aviso, LocalizationManager.instance.GetLozalization()));

                warningText.text = "Parabéns!! Você já tem tudo o que precisa para fotografar caudas de baleias jubarte e contribuir com "
                    + "as pesquisas da Ciência Cidadã. Pressione ENTER para iniciar.";

                audioSource.PlayOneShot(warningClip);

                Debug.Log(warningText.text);
                ReadText(warningText.text);
            }
            else
            {
                dialogueInterface.SetActive(true);
                dialogueInterfacePlayerName.text = "Turistas";
                
                dialogueInterfaceText.text = "Olá, nós também somos turistas e estamos participando do Ciência Cidadã de " +
                    "identificação de baleias. Para você participar, você ainda precisa conquistar a " +
                    (PlayerPreferences.M004_Memoria == false ? "Câmera fotográfica " : "") +
                    (PlayerPreferences.M004_TeiaAlimentar == false ? "Lente Zoom; " : "") +
                    (PlayerPreferences.M004_FotoIdentificacao == false ? " E conhecer sobre o processo de fotoidentificação de baleias; " : "") +
                    "Volte aqui depois para concluir a missão!!";

                Debug.Log(dialogueInterfaceText.text);
                ReadText(dialogueInterfaceText.text);

            }

        }*/

        Debug.Log("TRIGGER>>>>>> " + collision.name);

        if(PlayerPreferences.finishedAllM009Games() && collision.name.Equals("Mentor4"))
        {
            //mentorController.AddComponent<VIDE_Assign>();
            
            //mentorController.GetComponent<VIDE_Assign>().enabled = true;
            mentorController.GetComponent<MentorController>().GetDialogue("M009", "Mentor4", 0); // dialogo que leva para missao

            
        
		}
        else if (collision.name.Equals("Mentor4"))
           {
            mentorController.GetComponent<MentorController>().GetDialogue("M009", "Mentor4", 1); // dialogo que leva para missao
             //   dialogueInterface.SetActive(true);
             //   dialogueInterfacePlayerName.text = "Turistas";
                
               // dialogueInterfaceText.text = "Olá, nós também somos turistas e estamos participando do Ciência Cidadã de " +
                 //   "identificação de baleias. Para você participar, você ainda precisa conquistar a " +
                   // (PlayerPreferences.M004_Memoria == false ? "Câmera fotográfica " : "") +
                    //(PlayerPreferences.M004_TeiaAlimentar == false ? "Lente Zoom; " : "") +
                    //(PlayerPreferences.M004_FotoIdentificacao == false ? " E conhecer sobre o processo de fotoidentificação de baleias; " : "") +
                    //"Volte aqui depois para concluir a missão!!";

                //Debug.Log(dialogueInterfaceText.text);
                //ReadText(dialogueInterfaceText.text);  
		   }

        isTrigger = true;
        colliderControl = collision;
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        warningInterface.SetActive(false);
        dialogueInterface.SetActive(false);

        isTrigger = false;

        colliderControl = null;
    }
}

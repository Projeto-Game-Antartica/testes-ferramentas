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

    //private string initialInstruction = "Conheça o navio e converse com os pesquisadores para novos desafios.";

    public void Start()
    {
        isTrigger = false;

        //InitialInstruction();

        if (PlayerPrefs.GetInt("Saved") == 1)
        {
            transform.position = character.GetPosition();
            chasingCamera.SetCameraPosition(character.GetPosition());
            Debug.Log(transform.position);
        }

        Debug.Log(SceneManager.GetActiveScene().name);
    }

    private void Update()
    {
        if (isTrigger && Input.GetKeyDown(KeyCode.Return))
        {
            positionSceneChange = new Vector3(transform.position.x, transform.position.y);
            // save the position when loading another scene
            character.SavePosition(positionSceneChange);


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

        isTrigger = true;
        colliderControl = collision;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        warningInterface.SetActive(false);
        dialogueInterface.SetActive(false);

        isTrigger = false;

        colliderControl = null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

// attached to the character
public class ShipSceneManagement : AbstractScreenReader {

    private bool isTrigger;
    public SimpleCharacterController character;
    public ChasingCamera chasingCamera;

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
    public AudioClip closeClip;

    public BagController Bag;

    private string missionNumber = "M004";

    public void Start()
    {
        isTrigger = false;

        if (PlayerPrefs.GetInt("Saved_"+missionNumber) == 1)
        {
            transform.position = character.GetPosition(missionNumber);
            chasingCamera.SetCameraPosition(character.GetPosition(missionNumber));
            Debug.Log(transform.position);
        }


        if (PlayerPreferences.M004_Memoria)
            Bag.EnableItemByIndex(0); // camera fotografica

        if (PlayerPreferences.M004_TeiaAlimentar)
            Bag.EnableItemByIndex(1); // lente zoom

    }

    private void Update()
    {
        if (isTrigger && Input.GetKeyDown(KeyCode.Return))
        {
            positionSceneChange = new Vector3(transform.position.x, transform.position.y);
            // save the position when loading another scene
            character.SavePosition(positionSceneChange, missionNumber);


            if (colliderControl.name.Equals("Figurante") && PlayerPreferences.finishedAllM004Games())
                SceneManager.LoadScene(ScenesNames.M004TailMission);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name.Equals("Figurante"))
        {
            if (PlayerPreferences.finishedAllM004Games())
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

        }

        isTrigger = true;
        colliderControl = collision;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (warningInterface.activeSelf)
            audioSource.PlayOneShot(closeClip);

        warningInterface.SetActive(false);
        dialogueInterface.SetActive(false);

        isTrigger = false;

        colliderControl = null;
    }
}

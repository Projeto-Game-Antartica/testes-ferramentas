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

            //if (colliderControl.name.Equals("cabine principal"))
            //    SceneManager.LoadScene("ShipInsideScene");
            //else 
            if (colliderControl.name.Equals("Figurante") && PlayerPreferences.finishedAllM004Games())
                SceneManager.LoadScene(ScenesNames.M004TailMission);
        }

        //if (character.isWalking && warningText.text == initialInstruction)
        //{
        //    warningInterface.SetActive(false);
        //    ReadText("Painel de instruções iniciais fechado.");
        //}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("scene-trigger");
        
        //if (collision.name.Equals("cabine principal"))
        //{
        //    warningInterface.SetActive(true);
        //    warningText.text = "Pressione E para entrar no passadiço do navio.";
        //}
        //else 
        if(collision.name.Equals("Figurante"))
        {
            if (PlayerPreferences.finishedAllM004Games())
            {
                warningInterface.SetActive(true);
                warningText.text = "Parabéns!! Você já tem tudo o que precisa para fotografar caudas de baleias jubarte e contribuir com "
                    + "as pesquisas da Ciência Cidadã. Pressione ENTER para iniciar.";
            }
            else
            {
                dialogueInterface.SetActive(true);
                dialogueInterfacePlayerName.text = "Turistas";
                
                dialogueInterfaceText.text = "Olá, nós também somos turistas e estamos participando do Ciência Cidadã de " +
                    "identificação de baleias.Para você participar, você ainda precisa" + "Finalize os seguintes minijogos: " +
                    (PlayerPreferences.M004_FotoIdentificacao == false ? "Fotoidentificação de baleias; " : "") +
                    (PlayerPreferences.M004_Memoria == false ? "Animais antárticos; " : "") +
                    (PlayerPreferences.M004_TeiaAlimentar == false ? "Teia Alimentar; " : "") +
                    "Volte aqui depois para concluir a missão!!";
            }

        }

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

    //private void InitialInstruction()
    //{
    //    ReadText("Painel de instruções iniciais aberto");
    //    warningInterface.SetActive(true);
    //    warningText.text = initialInstruction;
    //    ReadText(warningText.text);
    //}
}

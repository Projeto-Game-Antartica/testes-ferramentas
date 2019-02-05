using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*
 * Based on https://github.com/SebLague/Programming-Practice
 * Adapted for accessibility purposes
 */

public class Block : MonoBehaviour, ISelectHandler, IDeselectHandler {

    public event System.Action<Block> OnBlockPressed;
    public event System.Action OnFinishedMoving;

    public Vector2Int coord;
    Vector2Int startingCoord;
    Texture2D image;

    public bool selected = false;

    public AudioSource audioSource;
    public AudioClip selectAudioClip;
    

    public void Init(Vector2Int startingCoord, Texture2D image)
    {
        this.startingCoord = startingCoord;
        coord = startingCoord;

        audioSource = gameObject.AddComponent<AudioSource>();
        selectAudioClip = Resources.Load<AudioClip>("Audio/selecting");

        GetComponent<MeshRenderer>().material = Resources.Load<Material>("Block");
        GetComponent<MeshRenderer>().material.mainTexture = image;
        
        this.image = image;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return) && selected)
        {
            OnBlockPressed(this);
        }
    }

    public void MoveToPosition(Vector2 target, float duration)
    {
        StartCoroutine(AnimateMove(target, duration));
    }

    private void OnMouseDown()
    {
        if(OnBlockPressed != null)
        {
            OnBlockPressed(this);
        }
    }

    private void OnMouseOver()
    {
        ChangeMaterial("BlockHighlited");
    }

    private void OnMouseExit()
    {
        ChangeMaterial("Block");
    }

    void ISelectHandler.OnSelect(BaseEventData eventData)
    {
        selected = true;

        audioSource.PlayOneShot(selectAudioClip);

        ChangeMaterial("BlockHighlited");
    }

    void IDeselectHandler.OnDeselect(BaseEventData eventData)
    {
        selected = false;
        ChangeMaterial("Block");
    }

    void ChangeMaterial(string materialName)
    {
        GetComponent<MeshRenderer>().material = Resources.Load<Material>(materialName);
        GetComponent<MeshRenderer>().material.mainTexture = image;
    }

    IEnumerator AnimateMove(Vector2 target, float duration)
    {
        Vector2 initialPos = transform.position;
        float percent = 0;

        while (percent < 1)
        {
            percent += Time.deltaTime / duration;
            transform.position = Vector2.Lerp(initialPos, target, percent);
            yield return null;
        }

        if(OnFinishedMoving != null)
        {
            OnFinishedMoving();
        }
    }

    public bool IsAtStartingCoord()
    {
        return coord == startingCoord;
    }
}

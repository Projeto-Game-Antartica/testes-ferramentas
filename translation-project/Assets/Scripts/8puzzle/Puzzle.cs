    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*
 * Based on https://github.com/SebLague/Programming-Practice
 * Adapted for accessibility purposes
 */

public class Puzzle : MonoBehaviour {

    public Texture2D image;
    public int blocksPerLine = 3;
    public int shuffleLenght = 20;
    public float defaultMoveDuration = 0.2f;
    public float shuffleMoveDuration = 0.1f;

    enum PuzzleState { Solved, Shuffling, Playing };
    PuzzleState state;

    Block emptyBlock;
    Block[,] blocks;
    Queue<Block> inputs;
    bool blockIsMoving;
    int shuffleMovesRemaining;
    Vector2Int prevShuffleOffset;

    private AudioSource audioSource;
    private AudioClip slidingAudioClip;
    private AudioClip wrongAudioClip;
    private AudioClip completedAudioClip;

    public Text countText;
    private int movementCount = 0;

    public Transform blockTransform;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        slidingAudioClip = Resources.Load<AudioClip>("Audio/slide-right");
        wrongAudioClip = Resources.Load<AudioClip>("Audio/wrong");
        completedAudioClip = Resources.Load<AudioClip>("Audio/completed");
        
        CreatePuzzle();

        blockTransform.localScale = new Vector3(15, 15, 1);
    }

    private void Update()
    {
        if (state == PuzzleState.Solved && Input.GetKeyDown(KeyCode.Space))
        {
            StartShuffle();
        }
    }

    void CreatePuzzle()
    {
        blocks = new Block[blocksPerLine, blocksPerLine];
        Texture2D[,] imageSlices = ImageSlicer.GetSlices(image, blocksPerLine);

        // depending on camera size (due to canvas)
        float blockSeparation = 6.3f;
        Vector3 Scale = new Vector3(5f, 5f, 0);
        Vector2 Position = new Vector2(0f, 3f);

        for(int i=0; i<blocksPerLine; i++)
        {
            for(int j=0; j <blocksPerLine; j++)
            {
                GameObject blockObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
                blockObject.transform.localScale += Scale;
                blockObject.transform.position = -Position * (blocksPerLine - 1)  + new Vector2(blockSeparation * j, blockSeparation * i);
                blockObject.transform.parent = transform;

                Block block = blockObject.AddComponent<Block>();
                blockObject.AddComponent<Selectable>();
                blockObject.name = "Block" + i + j;
                block.OnBlockPressed += PlayerMoveBlockInput;
                block.OnFinishedMoving += OnBlockFinishedMoving;
                block.Init(new Vector2Int(j, i), imageSlices[j, i]);
                blocks[j, i] = block;

                if(i==0 && j == blocksPerLine - 1)
                {
                    emptyBlock = block;
                }
                
                if(i==blocksPerLine - 1 && j == 0)
                {
                    block.GetComponent<Selectable>().Select();
                }
            }
        }

        //Camera.main.orthographicSize = blocksPerLine;
        inputs = new Queue<Block>();
    }

    void PlayerMoveBlockInput(Block blockToMove)
    {
        if (state == PuzzleState.Playing)
        {
            inputs.Enqueue(blockToMove);
            MakeNextPlayerMove();
        }
    }

    void MakeNextPlayerMove()
    {
        Debug.Log(inputs.Count);
        while (inputs.Count > 0 && !blockIsMoving)
        {
            MoveBlock(inputs.Dequeue(), defaultMoveDuration, false);
        }
    }

    void MoveBlock(Block blockToMove, float duration, bool isShuffle)
    {
        if((blockToMove.coord - emptyBlock.coord).sqrMagnitude == 1)
        {
            if (!isShuffle)
            {
                movementCount++;
                countText.text = movementCount.ToString();
            }

            if (!audioSource.isPlaying) audioSource.PlayOneShot(slidingAudioClip);

            blocks[blockToMove.coord.x, blockToMove.coord.y] = emptyBlock;
            blocks[emptyBlock.coord.x, emptyBlock.coord.y] = blockToMove;

            Vector2Int targetCoord = emptyBlock.coord;
            emptyBlock.coord = blockToMove.coord;
            blockToMove.coord = targetCoord;

            Vector2 targetPosition = emptyBlock.transform.position;
            emptyBlock.transform.position = blockToMove.transform.position;
            blockToMove.MoveToPosition(targetPosition, duration);
            blockIsMoving = true;
        }
        else
        {
            if (!audioSource.isPlaying) audioSource.PlayOneShot(wrongAudioClip);
        }
    }

    void OnBlockFinishedMoving()
    {
        blockIsMoving = false;
        CheckIfSolved();

        if(state == PuzzleState.Playing)
        {
            MakeNextPlayerMove();
        }
        else if(state == PuzzleState.Shuffling)
        {
            if (shuffleMovesRemaining > 0)
            {
                MakeNextShuffleMove();
            }
            else
            {
                state = PuzzleState.Playing;
            }
        }
    }

    void StartShuffle()
    {
        state = PuzzleState.Shuffling;
        shuffleMovesRemaining = shuffleLenght;
        emptyBlock.gameObject.SetActive(false);
        MakeNextShuffleMove();
    }

    void MakeNextShuffleMove()
    {
        Vector2Int[] offsets = { new Vector2Int(1, 0), new Vector2Int(-1, 0), new Vector2Int(0, 1), new Vector2Int(0, -1) };
        int randomIndex = Random.Range(0, offsets.Length);

        for(int i=0; i < offsets.Length; i++)
        {
            Vector2Int offset = offsets[(randomIndex + i) % offsets.Length];
            if (offset != prevShuffleOffset * -1)
            {
                Vector2Int moveBlockCoord = emptyBlock.coord + offset;

                if(moveBlockCoord.x >= 0 && moveBlockCoord.x < blocksPerLine && moveBlockCoord.y >= 0 && moveBlockCoord.y < blocksPerLine)
                {
                    MoveBlock(blocks[moveBlockCoord.x, moveBlockCoord.y], shuffleMoveDuration, true);
                    shuffleMovesRemaining--;
                    prevShuffleOffset = offset;
                    break;
                }
            }
        }
    }

    void CheckIfSolved()
    {
        foreach(Block block in blocks)
        {
            if(!block.IsAtStartingCoord())
            {
                return;
            }
        }

        state = PuzzleState.Solved;
        emptyBlock.gameObject.SetActive(true);
        audioSource.PlayOneShot(completedAudioClip);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

public class Board : MonoBehaviour
{
    // Height and width of game board
    public int width;
    public int height;
    public GameObject tilePrefab;
    public GameObject[] blocks;
    private BackgroundTile[,] allTiles;

    private static Block[,] _blocksGrid;
    [SerializeField] private List<Block> blocksList;
    [SerializeField] private Transform blocksHolder; 

    private int _blockLayerId = 8;

    private Coroutine _blockCoroutine;

    private Score score;

    void Start()
    {
        Input.multiTouchEnabled = false;

        _blockLayerId = LayerMask.NameToLayer("block");

        allTiles = new BackgroundTile [width, height];

        _blocksGrid = new Block[width, height];

        FillGrid();
    }

    Vector3 oldpos = Vector3.zero;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) DebugGrid();

        if (!Input.GetMouseButtonDown(0)) return;
        if (_blockCoroutine != null) return;

        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        oldpos = pos;
        var hit = Physics2D.Raycast(pos, Vector2.zero);
        if (hit.collider && hit.collider.gameObject.layer == _blockLayerId)
        {
            _blockCoroutine = StartCoroutine(MovingBlock(hit.collider.gameObject));
        }
    }

    private void SetUp()
    {
    }

    IEnumerator MovingBlock(GameObject block)
    {
        var startingPos = block.transform.position;
        var blockComponent = block.GetComponent<Block>();

        ClearPositions(blockComponent);

        var moveSuccessful = false;
        while (Input.GetMouseButton(0))
        {
            // current new position of a parent block
            var newPos = block.transform.position;

            var newX = Mathf.RoundToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition).x);

            if (newX >= width) newX = width - 1;
            else if (newX < 0) newX = 0;

            newPos.x = newX;

            if (IsMoveValid(blockComponent, newX, Mathf.RoundToInt(newPos.y)))
            {
                // move successful
                moveSuccessful = true;
                block.transform.position = newPos;
            }
            else
            {
                moveSuccessful = false;
            }

            yield return null;
        }


        moveSuccessful = startingPos != block.transform.position;


        if (moveSuccessful)
        {
            // update grid
            SetBlockCoordinatesToGrid(blockComponent);

            ApplyGravity();

            CheckLines();

            NextTurn();
        }

        _blockCoroutine = null;
    }

    private void FillGrid()
    {
        Array.Clear(_blocksGrid, 0, _blocksGrid.Length);

        foreach (Block block in blocksList)
        {
            SetBlockCoordinatesToGrid(block);
        }
    }

    void SetBlockCoordinatesToGrid(Block block)
    {
        ClearPositions(block);

        foreach (Transform subBlock in block.transform)
        {
            var pos = subBlock.transform.position;
            var coordinates = new Vector2Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));
            block.blockPositions.Add(coordinates);
            _blocksGrid[coordinates.x, coordinates.y] = block;
        }
    }

    private void ClearPositions(Block block)
    {
        // clear positions on grid from block's vector array
        foreach (var position in block.blockPositions)
        {
            _blocksGrid[position.x, position.y] = null;
        }

        block.blockPositions.Clear();
    }

    private void ApplyGravity()
    {
        while (true)
        {
            var blockFell = false;
            foreach (var block in blocksList)
            {
                var canFall = true;
                foreach (var position in block.blockPositions)
                {
                    if (position.y == 0)
                    {
                        // block already at y = 0
                        canFall = false;
                        break;
                    }

                    if (_blocksGrid[position.x, position.y - 1] != null)
                    {
                        canFall = false;
                        break;
                    }
                }

                // continue to next block
                if (!canFall) continue;

                blockFell = true;
                // move block down
                block.transform.position += Vector3.down;
                // clear pos array, and grid
                SetBlockCoordinatesToGrid(block);
            }

            if (blockFell)
            {
                continue;
            }

            break;
        }
    }

    private void CheckLines()
    {
        for (int y = 0; y < height; y++)
        {
            var lineFound = true;
            for (int x = 0; x < width; x++)
            {
                // TODO match block types later
                if (_blocksGrid[x, y] == null) lineFound = false;
            }

            if (lineFound)
            {
                print("lineFound");
                for (int x = 0; x < width; x++)
                {
                    Destroy(_blocksGrid[x, y].gameObject.transform.parent.gameObject);
                    Destroy(_blocksGrid[x, y].gameObject);
                    _blocksGrid[x, y] = null;
                    score = FindObjectOfType<Score>();
                    score.UpdScore(1);
                }

                ApplyGravity();
                break;
            }
        }
    }

    void DebugGrid()
    {
        StringBuilder stringBuilder = new StringBuilder();
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                stringBuilder.Append(_blocksGrid[x, y] == null ? " . " : "X ");
            }

            stringBuilder.Append("\n");
        }

        Debug.Log(stringBuilder.ToString());
    }

    private bool IsMoveValid(Block block, int newX, int y)
    {
        for (int x = newX; x < newX + block.size; x++)
        {
            if (x >= width) return false;
            if (_blocksGrid[x, y] != null) return false;
        }

        return true;
    }

    private void NextTurn()
    {
        foreach (var block in blocksList)
        {
            block.transform.position += Vector3.up;
            SetBlockCoordinatesToGrid(block);
        }

        var rand = new System.Random();
        var index = rand.Next(0, blocks.Length - 1);

        var blockToSpawn = blocks[index];

        var spawnedBlock = Instantiate(blockToSpawn, Vector3.zero, Quaternion.identity, blocksHolder);
        var spawnedBlockComponent = spawnedBlock.GetComponent<Block>(); // TODO remove

        SetBlockCoordinatesToGrid(spawnedBlockComponent);

        ApplyGravity();
        CheckLines();
    }
}
using System;
using System.Collections;
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

    private static Transform[,] blocksGrid;
    [SerializeField] private Transform blocksHolder;

    private int _blockLayerId = 8;

    private Coroutine _blockCoroutine;

    private Score score;

    void Start()
    {
        _blockLayerId = LayerMask.NameToLayer("block");

        allTiles = new BackgroundTile [width, height];

        blocksGrid = new Transform[width, height];

        FillGrid();
    }

    Vector3 oldpos = Vector3.zero;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) DebugGrid();
        Debug.DrawRay(oldpos, Camera.main.transform.forward * 20, Color.white, 10f);

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
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector2 tempPos = new Vector2(i, j);
                GameObject backgroundTile = Instantiate(tilePrefab, tempPos, Quaternion.identity) as GameObject;
                backgroundTile.transform.parent = this.transform;
                backgroundTile.name = "( " + i + ", " + j + " )";
                // fill board with blocks
                int blockToUse = Random.Range(0, blocks.Length);
                GameObject block = Instantiate(blocks[blockToUse], tempPos, Quaternion.identity);
                block.transform.parent = this.transform;
                block.name = "block: " + "( " + i + ", " + j + " )";
            }
        }
    }

    IEnumerator MovingBlock(GameObject block)
    {
        var startingPos = block.transform.position;
        var blockComponent = block.GetComponent<Block>();

        ClearPositions(ref blockComponent);

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
            SetBlockCoordinatesToGrid(block.transform, ref blockComponent);

            ApplyGravity();

            CheckLines();

            NextTurn();
        }

        _blockCoroutine = null;
    }

    private void ClearPositions(ref Block block)
    {
        // clear positions on grid from block's vector array
        foreach (var position in block.blockPositions)
        {
            blocksGrid[position.x, position.y] = null;
        }

        block.blockPositions.Clear();
    }

    private void FillGrid()
    {
        Array.Clear(blocksGrid, 0, blocksGrid.Length);

        foreach (Transform blockTransform in blocksHolder)
        {
            var block = blockTransform.GetComponent<Block>();
            SetBlockCoordinatesToGrid(blockTransform, ref block);
        }
    }

    void SetBlockCoordinatesToGrid(Transform parentBlock, ref Block block)
    {
        ClearPositions(ref block);

        if (block.debugger)
        {
            Debug.LogError("qwe");
        }

        foreach (Transform subBlock in parentBlock)
        {
            var pos = subBlock.transform.position;
            var coordinates = new Vector2Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));
            block.blockPositions.Add(coordinates);
            blocksGrid[coordinates.x, coordinates.y] = subBlock;
            if (block.debugger)
            {
                Debug.LogError(block.transform.position + " UwU ");
            }
        }
    }

    private void ApplyGravity()
    {
        while (true)
        {
//            print("ApplyGravity();");
            var blockFell = false;
            foreach (Transform block in blocksHolder)
            {
                var blockComponent = block.GetComponent<Block>();
                var canFall = true;
                foreach (var position in blockComponent.blockPositions)
                {
                    if (position.y == 0)
                    {
                        // block already at y = 0
                        canFall = false;
                        break;
                    }

                    if (blocksGrid[position.x, position.y - 1] != null)
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
                ClearPositions(ref blockComponent);
                SetBlockCoordinatesToGrid(block, ref blockComponent);
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
                if (blocksGrid[x, y] == null) lineFound = false;
            }

            if (lineFound)
            {
                print("lineFound");
                for (int x = 0; x < width; x++)
                {
                    Destroy(blocksGrid[x, y].gameObject.transform.parent.gameObject);
                    Destroy(blocksGrid[x, y].gameObject);
                    blocksGrid[x, y] = null;
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
                stringBuilder.Append(blocksGrid[x, y] == null ? " . " : "X ");
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
            if (blocksGrid[x, y] != null) return false;
        }

        return true;
    }

    private void NextTurn()
    {
        foreach (Transform block in blocksHolder)
        {
            block.transform.position += Vector3.up;
            var blockComponent = block.GetComponent<Block>();
            SetBlockCoordinatesToGrid(block, ref blockComponent);
        }

        var rand = new System.Random();
        var index = rand.Next(0, blocks.Length - 1);

        var blockToSpawn = blocks[index];

        var spawnedBlock = Instantiate(blockToSpawn, Vector3.zero, Quaternion.identity, blocksHolder);
        var spawnedBlockComponent = spawnedBlock.GetComponent<Block>();

        SetBlockCoordinatesToGrid(spawnedBlock.transform, ref spawnedBlockComponent);
        
        ApplyGravity();
        CheckLines();
    }
}
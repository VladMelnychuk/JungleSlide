using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityRandom = UnityEngine.Random;

public class Board : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height;

    [SerializeField] private Transform blocksHolder;
    public List<Block> activeBlocksList = new List<Block>();

    private static Block[,] _grid;

    [SerializeField] private int blockLayerId = 8;

    private void Start()
    {
        blockLayerId = LayerMask.NameToLayer("block");
        _grid = new Block[width, height];
        FillGrid();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) DebugGrid();

        if (!Input.GetMouseButtonDown(0)) return;

        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var hit = Physics2D.Raycast(pos, Vector2.zero);
        if (hit.collider && hit.collider.gameObject.layer == blockLayerId)
        {
            StartCoroutine(MovingBlock(hit.collider.gameObject));
        }
    }

    #region BlockMoving

    private IEnumerator MovingBlock(GameObject blockGameObject)
    {
        var gameObjectPos = blockGameObject.transform.position;
        var blockInitPosition = new Vector2Int(Mathf.RoundToInt(gameObjectPos.x), Mathf.RoundToInt(gameObjectPos.y));

        var block = _grid[blockInitPosition.x, blockInitPosition.y];
        Debug.LogError("hit " + blockGameObject.name + " at " + blockInitPosition.x + " : " + blockInitPosition.y);

        var borders = GetBorders(block);

        while (Input.GetMouseButton(0))
        {
            var newX = Mathf.RoundToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition).x);

            if (newX >= borders.x && newX <= borders.y)
            {
                // can move
                var newPos = new Vector3(newX, block.gridPosition.y, 0);
                block.transform.position = newPos;
            }

            yield return null;
        }

        if (gameObjectPos != block.transform.position)
        {
            // move completed
            MoveBlockInGrid(block, Vector3ToVector2Int(block.transform.position));
            ApplyGravity();
            CheckLines();
        }
    }

    private void MoveBlockInGrid(Block block, Vector2Int newPos)
    {
        // set old positions to null aka remove from grid
        RemoveBlockFromGrid(block);

        // set new position
        AddBlockToGrid(block, newPos);

        block.gridPosition.x = newPos.x;
        block.gridPosition.y = newPos.y;

        // TODO move block in UI
        block.transform.position = new Vector3(block.gridPosition.x, block.gridPosition.y, 0);
    }

    private Vector2Int GetBorders(Block block)
    {
        // x = left, y = right
        var borders = new Vector2Int(0, width - block.size);

        // get left border
        for (int left = block.gridPosition.x; left >= 0; left--)
        {
            if (_grid[left, block.gridPosition.y] == null) continue;
            if (_grid[left, block.gridPosition.y] != block)
            {
                borders.x = left + 1;
                break;
            }
        }

        // get right border
        for (int right = block.size + block.gridPosition.x - 1; right < width; right++)
        {
            if (_grid[right, block.gridPosition.y] == null) continue;
            if (_grid[right, block.gridPosition.y] != block)
            {
                borders.y = right - block.size;
                break;
            }
        }

        return borders;
    }

    #endregion

    private void AddBlockToGrid(Block block, Vector2Int newPos)
    {
        for (int i = newPos.x; i < block.size + newPos.x; i++)
        {
            _grid[i, newPos.y] = block;
        }

        block.gridPosition = Vector3ToVector2Int(block.transform.position);
    }

    private void RemoveBlockFromGrid(Block block)
    {
        for (int i = block.gridPosition.x; i < block.size + block.gridPosition.x; i++)
        {
            _grid[i, block.gridPosition.y] = null;
        }
    }

    private void FillGrid()
    {
        foreach (Transform blockTransform in blocksHolder)
        {
            activeBlocksList.Add(blockTransform.GetComponent<Block>());
        }

        foreach (var block in activeBlocksList)
        {
            AddBlockToGrid(block, block.gridPosition);
        }
    }

    private void ApplyGravity()
    {
        while (true)
        {
            var blockFell = false;
            foreach (var block in activeBlocksList)
            {
                var blockPos = block.gridPosition;

                // block already at y = 0    
                if (blockPos.y == 0) continue;

                bool canFall = true;

                for (int i = 0; i < block.size; i++)
                {
                    if (_grid[blockPos.x, blockPos.y - 1] != null)
                    {
                        canFall = false;
                        break;
                    }
                }

                if (canFall)
                {
                    blockFell = true;
                    var newPos = block.gridPosition;
                    newPos.y -= 1;
                    MoveBlockInGrid(block, newPos);
                }
            }

            if (blockFell) continue;

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
                // TODO match block types
                if (_grid[x, y] == null) lineFound = false;
            }

            if (lineFound)
            {
                print("lineFound");

                var xIndex = 0;

                while (xIndex < width)
                {
                    var block = _grid[xIndex, y];
                    RemoveBlockFromGrid(block);
                    xIndex += block.size;

                    // TODO Object pool
                    Destroy(block.gameObject);
                }

                ApplyGravity();
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
                stringBuilder.Append(_grid[x, y] == null ? " . " : "X ");
            }

            stringBuilder.Append("\n");
        }

        Debug.Log(stringBuilder.ToString());
    }

    private Vector2Int Vector3ToVector2Int(Vector3 vector3)
    {
        return new Vector2Int(Mathf.RoundToInt(vector3.x), Mathf.RoundToInt(vector3.y));
    }
}
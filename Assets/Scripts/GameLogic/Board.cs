﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using DG.Tweening;
using UnityRandom = UnityEngine.Random;

public class Board : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height;

    [SerializeField] private Transform blocksHolder;
//    public List<Block> activeBlocksList = new List<Block>();

    private static Block[,] _grid;

    [SerializeField] private int blockLayerId = 8;
    [SerializeField] private Block[] blocks;
    private DictionaryObjectPool _objectpool; 
    
    //Score
    private Score score;
    private void ObjectPoolSetup()
    {
        _objectpool = new DictionaryObjectPool();
        foreach (var block in blocks)
        {
            var blockGameObject = block.gameObject;
            _objectpool.AddObjectPool(blockGameObject.name, blockGameObject, blocksHolder, 20);
        }

        // generating level
        var block1 = _objectpool["1x1_Air"].Spawn(Vector3.zero);
        var blockComponent = block1.GetComponent<Block>();
        blockComponent.gridPosition = Vector2Int.zero;
        AddBlockToGrid(blockComponent, Vector2Int.zero);
    }

    private void Start()
    {
        _grid = new Block[width, height];
        
        blockLayerId = LayerMask.NameToLayer("block");
        
        ObjectPoolSetup();
    }

    private void Update()
    {
        print(_grid[5, 0]);
        if (PauseController.IsPaused) return;
        
        if (Input.GetKeyDown(KeyCode.Space)) DebugGrid();

        if (!Input.GetMouseButtonDown(0)) return;

        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var hit = Physics2D.Raycast(pos, Vector2.zero);
        if (hit.collider && hit.collider.gameObject.layer == blockLayerId)
        {
            StartCoroutine(MovingBlock(hit.collider.gameObject));
        }
    }

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

            NextTurn();
        }
    }

    #region Grid Logic

    private void MoveBlockInGrid(Block block, Vector2Int newPos)
    {

        if (newPos.y >= height)
        {
            Debug.LogError("Game Over");
            return;
        }
        
        // set old positions to null aka remove from grid
        RemoveBlockFromGrid(block);

        // set new position
        AddBlockToGrid(block, newPos);

        block.gridPosition = newPos;

        // TODO move block in UI
//        block.transform.position = new Vector3(block.gridPosition.x, block.gridPosition.y, 0);
        block.transform.DOMove(new Vector3(block.gridPosition.x, block.gridPosition.y, 0), .3f);
    }

    private void AddBlockToGrid(Block block, Vector2Int newPos)
    {
        for (int i = newPos.x; i < newPos.x + block.size; i++)
        {
            _grid[i, newPos.y] = block;
        }
    }

    private void RemoveBlockFromGrid(Block block)
    {
        for (int i = block.gridPosition.x; i < block.gridPosition.x + block.size; i++)
        {
            _grid[i, block.gridPosition.y] = null;
        }
    }

    #endregion

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
    
    #region Game Logic

    private void ApplyGravity()
    {
        while (true)
        {
            var blockFell = false;

            for (int y = height - 1; y > 0; y--)
            {
                for (int x = 0; x < width;)
                {
                    if (_grid[x, y] == null)
                    {
                        x += 1;
                        continue;
                    }

                    var block = _grid[x, y];
                    var blockPos = block.gridPosition;
                    var newY = blockPos.y - 1;
                    x += block.size;

                    bool canFall = true;
                    
                    for (int i = 0; i < block.size; i++)
                    {
                        if (_grid[blockPos.x + i, newY] != null)
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
            }
            
            if (!blockFell) break;
        }
    }

    private void CheckBlockStatus() {}

    private void UpdateScore(int scorenum)
    {
        score = FindObjectOfType<Score>();
        score.UpdScore(scorenum);
    }
    private void CheckLines()
    {
        for (int y = 0; y < height; y++)
        {
            var lineFound = true;
            for (int x = 0; x < width; x++)
            {
                // TODO match block types
                if (_grid[x, y] == null)
                {
                    lineFound = false;
                    break;
                }
            }

            if (lineFound)
            {
                print("lineFound");
                
                DebugGrid();

                var xIndex = 0;

                while (xIndex < width)
                {
                    var block = _grid[xIndex, y];
                    RemoveBlockFromGrid(block);
                    xIndex += block.size;
                    
                    //Score Update
                    UpdateScore(1*xIndex);
                    
                    // TODO Object pool
                    block.Despawn();
                    
                }

                ApplyGravity();
            }
        }
    }

    private void NextTurn()
    {
        for (int y = (height - 1); y >= 0; y--)
        {
            for (int x = 0; x < width;)
            {
                if (_grid[x, y] != null)
                {
                    var block = _grid[x, y];
                    x += block.size;
                    var newPos = block.gridPosition;
                    newPos.y += 1;
                    MoveBlockInGrid(block, newPos);
                }
                else x++;
            }
        }

        DebugGrid();

        var rand = new System.Random();
        var index = rand.Next(0, blocks.Length - 1);

        var blockToSpawn = blocks[index];

        var spawnedBlock = _objectpool[blockToSpawn.gameObject.name].Spawn(Vector3.zero);
        
        var spawnedBlockComponent = spawnedBlock.GetComponent<Block>();

        AddBlockToGrid(spawnedBlockComponent, spawnedBlockComponent.gridPosition);

        ApplyGravity();
        CheckLines();
    }

    #endregion


    private void DebugGrid()
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

    private static Vector2Int Vector3ToVector2Int(Vector3 vector3)
    {
        return new Vector2Int(Mathf.RoundToInt(vector3.x), Mathf.RoundToInt(vector3.y));
    }
}
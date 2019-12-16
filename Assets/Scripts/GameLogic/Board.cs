using System;
using System.Collections;
using System.IO;
using UnityEngine;
using System.Text;
using DG.Tweening;
using Newtonsoft.Json.Linq;
using UnityEngine.Networking;
using Random = System.Random;
using UnityRandom = UnityEngine.Random;

public class Board : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height;

    [SerializeField] private Transform blocksHolder;

    private static Block[,] _grid;

    [SerializeField] private int blockLayerId = 8;
    [SerializeField] private Block[] blocks;
    private DictionaryObjectPool _objectpool;

    private Random rand = new System.Random();

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
    }

    private IEnumerator GenerateLevel(string lvlName)
    {
        var path = Path.Combine(Application.streamingAssetsPath, "Levels", lvlName + ".json");

        var req = UnityWebRequest.Get(path);

        yield return req.SendWebRequest();

        if (req.isNetworkError)
        {
            Debug.LogError("Error: " + req.error);
        }
        else
        {
            var jObject = JObject.Parse(req.downloadHandler.text);

            if (!(jObject[lvlName] is JArray blocksToSpawn)) yield break;

            foreach (var block in blocksToSpawn)
            {
                var pos = new Vector3(float.Parse(block["x"].ToString()), float.Parse(block["y"].ToString()));
                var spawnedBlock = _objectpool[block["name"].ToString()].Spawn(pos, Quaternion.identity);
                var spawnedBlockComponent = spawnedBlock.GetComponent<Block>();
                AddBlockToGrid(spawnedBlockComponent, Vector3ToVector2Int(pos));
            }
        }
    }

    private void Start()
    {
        _grid = new Block[width, height];

        blockLayerId = LayerMask.NameToLayer("block");

        ObjectPoolSetup();
        StartCoroutine(GenerateLevel(LevelLoader.CurrentLevelName));
    }

    private void Update()
    {
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
            StartCoroutine(CompleteMove(block));
        }
    }

    private IEnumerator CompleteMove(Block block)
    {
        const float delay = .1f;
        MoveBlockInGrid(block, Vector3ToVector2Int(block.transform.position));
        ApplyGravity();
        yield return new WaitForSeconds(delay);
        CheckLines();
        yield return new WaitForSeconds(delay);
        NextTurn();
    }

    #region Grid Logic

    private void MoveBlockInGrid(Block block, Vector2Int newPos)
    {
        if (newPos.y >= height)
        {
            Debug.LogError("Game Over");
            block.Despawn();
            return;
        }

        // set old positions to null aka remove from grid
        RemoveBlockFromGrid(block);

        // set new position
        AddBlockToGrid(block, newPos);

        block.gridPosition = newPos;

        // TODO move block in UI
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
        for (var right = block.size + block.gridPosition.x - 1; right < width; right++)
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

    private void CheckBlockStatus()
    {
    }

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

            if (!lineFound) continue;

            print("lineFound");

            var xIndex = 0;

            Tweener tweener = null;

            while (xIndex < width)
            {
                var block = _grid[xIndex, y];
                RemoveBlockFromGrid(block);
                xIndex += block.size;

                //Score Update
                UpdateScore(1 * xIndex);

                tweener = block.transform.DOShakePosition(.1f, .1f, 5);
                tweener.onComplete += () => { block.Despawn(); };
            }


            if (tweener != null)
            {
                tweener.onComplete += ApplyGravity;
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

        SpawnNewBlock();
        ApplyGravity();
        CheckLines();
    }

    private void SpawnNewBlock()
    {
        var maxLimit = blocks.Length - 1;
        var spawnPosition = 0;

        while (spawnPosition < width - 1)
        {
            var chance = rand.Next(1, 3);
            if (chance >= 3) continue;

            var block = blocks[rand.Next(0, maxLimit)];
            if (block.size + spawnPosition < width)
            {
                // good, spawn
                var pos = Vector3.right * spawnPosition;
                var spawnedBlock = _objectpool[block.gameObject.name].Spawn(pos, Quaternion.identity);
                var blockComponent = spawnedBlock.GetComponent<Block>();
                AddBlockToGrid(blockComponent, Vector3ToVector2Int(pos));

                spawnPosition += block.size;
            }
            else
            {
                spawnPosition += 1;
            }
        }
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
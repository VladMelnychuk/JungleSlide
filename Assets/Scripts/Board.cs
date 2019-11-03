using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

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

	private int blockLayerID = 8;
    
    void Start()
    {
	    blockLayerID = LayerMask.NameToLayer("block");
	    allTiles = new BackgroundTile [width, height];
	    blocksGrid = new Transform[width, height];

//     	SetUp();
		FillGrid();
    }

    Vector3 oldpos = Vector3.zero;
    void Update()
    {
	    if (Input.GetKeyDown(KeyCode.Space)) DebugGrid();
	    Debug.DrawRay(oldpos, Camera.main.transform.forward * 20, Color.white, 10f);
	    if (Input.GetMouseButtonDown(0))
	    {
		    var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		    oldpos = pos;
		    var hit = Physics2D.Raycast(pos, Vector2.zero);
		    if (hit.collider && hit.collider.gameObject.layer == blockLayerID)
		    {
			    print("Hit: " + hit.collider.gameObject.name);
			    StartCoroutine(MovingBlock(hit.collider.gameObject));
		    }
		    else
		    {
			    print("missed");
		    }
	    }
    }
    
    private void SetUp(){

    	 for (int i = 0; i < width; i++){
    	 	for (int j = 0; j < height; j++){
    	 		Vector2 tempPos = new Vector2(i, j);
    	 		GameObject backgroundTile = Instantiate(tilePrefab, tempPos, Quaternion.identity) as GameObject ;
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
	    var blockComponent = block.GetComponent<Block>();
	    foreach (var oldPosition in blockComponent.blockPositions)
	    {
		    // set old position to null
		    blocksGrid[oldPosition.x, oldPosition.y] = null;
	    }
	    
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
	    
	    if (moveSuccessful)
	    {
		    // update grid
		    SetBlockCoordinatesToGrid(block.transform, blockComponent);
		    EndTurn();
	    }
	    
    }

    void FillGrid()
    {
	    foreach (Transform blockTransform in blocksHolder)
	    {
		    var block = blockTransform.GetComponent<Block>(); 
		    SetBlockCoordinatesToGrid(blockTransform, block);
	    }
    }

    void SetBlockCoordinatesToGrid(Transform parentBlock, Block block)
    {
	    block.blockPositions.Clear();
	    foreach (Transform subBlock in parentBlock)
	    {
		    var pos = subBlock.transform.position;
		    var coordinates = new Vector2Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));
		    block.blockPositions.Add(coordinates);
		    blocksGrid[coordinates.x, coordinates.y] = subBlock;
	    }
    }

    void EndTurn()
    {
	    ApplyGravity();
//	    CheckLines();
    }

    void ApplyGravity()
    {
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

			    if (blocksGrid[position.x, position.y - 1] != null) canFall = false;
		    }

		    if (canFall)
		    {
			    blockFell = true;
			    // move block down
			    block.transform.position += Vector3.down;
			    ClearBlockCoordinatesOnGrid(blockComponent.blockPositions);
			    blockComponent.blockPositions.Clear();
			    SetBlockCoordinatesToGrid(block, blockComponent);
		    }
	    }

	    if (blockFell)
	    {
		    CheckLines();
	    }
    }

    private static void ClearBlockCoordinatesOnGrid(List<Vector2Int> oldCoordinates)
    {
	    foreach (var oldCoordinate in oldCoordinates)
	    {
		    blocksGrid[oldCoordinate.x, oldCoordinate.y] = null;
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
			    // woala
			    for (int x = 0; x < width; x++)
			    {
				    Destroy(blocksGrid[x, y].gameObject);
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
		    if (blocksGrid[x, y] != null) return false;
	    }
	    return true;
    }
    
}

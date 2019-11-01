using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
	[SerializeField] private GameObject blocksHolder;

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
	    Debug.DrawRay(oldpos, Camera.main.transform.forward * 20, Color.white, 10f);
	    if (Input.GetMouseButtonDown(0))
	    {
		    var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		    oldpos = pos;
		    var hit = Physics2D.Raycast(pos, Vector2.zero);
		    if (hit.collider && hit.collider.gameObject.layer == blockLayerID)
		    {
			    print(hit.collider.gameObject.name);
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
	    var moveSuccesful = false;
	    while (Input.GetMouseButton(0))
	    {
		    // get edges
		    var newPos = block.transform.position;
		    var newX = Mathf.RoundToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition).x);

		    var leftEdge = newPos.y;
		    
		    if (newX >= width) newX = width - 1;
		    else if (newX < 0) newX = 0;
		    
		    newPos.x = newX;
		    int y = Mathf.RoundToInt(newPos.y);

		    if (blocksGrid[newX, y] == null)
		    {
			    // move successful
			    moveSuccesful = true;
			    block.transform.position = newPos;
		    }
		    else
		    {
			    moveSuccesful = false;
		    }
		    
		    yield return null;
	    }
	    
	    if (moveSuccesful)
	    {
		    foreach (var oldPosition in blockComponent.blockPositions)
		    {
			    // set old position to null
			    blocksGrid[oldPosition.x, oldPosition.y] = null;
		    }
		    FillBlockCoordinates(block.transform);
	    }
	    
    }

    void FillGrid()
    {
	    foreach (Transform blockTransform in blocksHolder.transform)
	    {
		    FillBlockCoordinates(blockTransform);
	    }
    }

    void FillBlockCoordinates(Transform parentBlock)
    {
	    var block = parentBlock.GetComponent<Block>();
	    foreach (Transform subBlock in parentBlock)
	    {
		    var pos = subBlock.transform.position;
		    var coordinates = new Vector2Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));
		    block.blockPositions.Add(coordinates);
		    Debug.Log(subBlock.gameObject.name + " : " + coordinates);
		    blocksGrid[coordinates.x, coordinates.y] = subBlock;
	    }
    }

    void EndTurn()
    {
	    foreach (Transform blockParent in blocksHolder.transform)
	    {
		    foreach (Transform block in blockParent)
		    {
			    
		    }
	    }
    }

    void CheckLines()
    {
	    var lineFound = false;
	    for (int i = 0; i < width; i++)
	    {
		    lineFound = true;
		    for (int j = 0; j < height; j++)
		    {
			    if (blocksGrid[i, j] == null)
			    {
				    lineFound = false;
				    break;
			    }

			    if (lineFound)
			    {
				    // pop
				    Debug.LogError("pop");
			    }
		    }
	    }
    }
    
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    // Height and width of game board
	public int width;
	public int height;
	public GameObject tilePrefab;
	public GameObject[] blocks;
	private BackgroundTile[,] allTiles;
    // Start is called before the first frame update
    void Start()
    {
     	allTiles = new BackgroundTile [width, height];

     	SetUp();
    }

    // Update is called once per frame
    void Update()
    {
        
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
}

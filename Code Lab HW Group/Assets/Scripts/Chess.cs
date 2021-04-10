﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Chess : MonoBehaviour
{
    // the width and height of the chess board
    public int width = 7;
    public int height = 7;

    private float offsetX = 3;
    private float offsetY = 3;
    
    private int[,] grid;

    // player 1 and 2 take turns to place the chess piece
    private bool blackTurn = false;

    private List<GameObject> spawnedPieces = new List<GameObject>();

    public GameObject blackPrefab, whitePrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        
        // initialize an empty 2D array with no chess pieces
        grid = new int[width, height];

        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                grid[x, y] = 0;
            }
        }
    }



    // Update is called once per frame
    void Update()
    {
        // If you press space, it reloads the scene.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {

            // get the position of the cursor when click on the mouse
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition, Camera.MonoOrStereoscopicEye.Mono);

            Debug.Log(worldPoint);
            
            // round the position of the cursor into integers
            int gridX = Convert.ToInt32(Math.Round(worldPoint.x)) + Convert.ToInt32(offsetX);
            int gridY = Convert.ToInt32(Math.Round(worldPoint.y)) + Convert.ToInt32(offsetY);

            // if the position is on the chess board and there is no chess piece in the grid
            if (gridX >= 0 && gridX < 7 && gridY >= 0 && gridY < 7 && grid[gridX, gridY] == 0)
            {
                // place a chess piece according to the turn
                if (blackTurn)
                {
                    grid[gridX, gridY] = 1;
                    blackTurn = false;
                }
                else
                {
                    grid[gridX, gridY] = 2;
                    blackTurn = true;
                }

                Debug.Log(grid[gridX, gridY]);
                
                UpdateDisplay();
            }
        }
    }

    // if a space is 1, it contains a black piece
    public bool ContainsBlack(int x, int y)
    {
        return grid[x, y] == 1;
    }

    // if a space is 2, it contains a white piece
    public bool ContainsWhite(int x, int y)
    {
        return grid[x, y] == 2;
    }
    
    private void UpdateDisplay()
    {
        foreach (var piece in spawnedPieces)
        {
            Destroy(piece);
        }

        spawnedPieces.Clear();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (ContainsBlack(x, y))
                {
                    var blackPiece = Instantiate(blackPrefab);
                    blackPiece.transform.position = new Vector3(x - offsetX, y - offsetY);
                    spawnedPieces.Add(blackPiece);
                }

                if (ContainsWhite(x, y))
                {
                    var whitePiece = Instantiate(whitePrefab);
                    whitePiece.transform.position = new Vector3(x - offsetX, y - offsetY);
                    spawnedPieces.Add(whitePiece);
                }
            }
        }
    }
    
    
    
    
}
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Chess : MonoBehaviour
{
    // the width and height of the chess board
    public int width = 7;
    public int height = 7;

    private float offsetX = 3;
    private float offsetY = 3;

    private int[,] grid;

    public Text display;

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
                //if someone has won dont place pieces
                if (BlackWin() || WhiteWin()) return;
                
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

                //Debug.Log(grid[gridX, gridY]);

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
        //Destroy and recreated the spawned pieces so that the board can update
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

        //display the winner as Black
        if (BlackWin())
        {
            display.text = "Black Wins!";
            display.color = Color.black;
        }
        //display the winner as White
        else if (WhiteWin())
        {
            display.text = "White Wins!";
            display.color = Color.white;
        }
        //display nothing
        else
        {
            display.text = "";
        }
    }
    
    //black wins
    public bool BlackWin()
    {
        return FiveInARow() == 1;
    }

    //white wins
    public bool WhiteWin()
    {
        return FiveInARow() == 2;
    }

    
    //this integer checks in you have five pieces of the same color in a row. checking each array location seperately
    private int FiveInARow (){
        for ( var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                //as long as the node is at least 5 away from the top of the board
                if(y <= height - 5)
                    //check if all 5 vertical pieces are the same
                    if (grid[x, y] != 0 && grid[x, y] == grid[x, y + 1] && grid[x, y] == grid[x, y + 2] &&
                        grid[x, y] == grid[x, y + 3] && grid[x, y] == grid[x, y + 4])
                        return grid[x, y];
                
                //as long as the node is at least 5 away from the far right side
                if(x <= width - 5)
                    //check if all 5 horizontal pieces are the same
                    if (grid[x, y] != 0 && grid[x, y] == grid[x + 1, y] && grid[x, y] == grid[x + 2, y] &&
                        grid[x, y] == grid[x + 3, y] && grid[x, y] == grid[x + 4, y])
                        return grid[x, y];
                
                //as long as the node is both 5 away from the top and right
                if(x <= width - 5 && y <= height - 5)
                    //check if the upward right diagonal is equal
                    if (grid[x, y] != 0 && grid[x, y] == grid[x + 1, y + 1] && grid[x, y] == grid[x + 2, y + 2] &&
                        grid[x, y] == grid[x + 3, y + 3] && grid[x, y] == grid[x + 4, y + 4])
                        return grid[x, y];
                    
                //as long as the node is both 5 away from the top and left
                if(x >= width - 2 && y <= height - 5)
                    //check if the upward left diagonal is all the same
                    if (grid[x, y] != 0 && grid[x, y] == grid[x - 1, y + 1] && grid[x, y] == grid[x - 2, y + 2] &&
                        grid[x, y] == grid[x - 3, y + 3] && grid[x, y] == grid[x - 4, y + 4])
                        return grid[x, y];
            }
        }

        //return 0 if none of them work
        return 0;
    }
}

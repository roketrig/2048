using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBoard : MonoBehaviour
{
    public Tile tilePrefab;
    public TileState[] tileStates;
    private TileGrid grid;
    private List<Tile> tiles;
    
    private bool _waitForAniamtion;
    private void Awake()
    {
        grid = GetComponentInChildren<TileGrid>();
        tiles = new List<Tile>(16);
    }

    private void Start()
    {
        CreateTile();
        CreateTile();
    }

    private void CreateTile()
    {
        Tile tile = Instantiate(tilePrefab, grid.transform);
        tile.SetState(tileStates[0],2);
        tile.Spawn(grid.GetRandomEmptyCell());
        tiles.Add(tile);
    }

    private void Update()
    {
        if (!_waitForAniamtion)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                MoveTiles(Vector2Int.up,0,1,1,1);
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                MoveTiles(Vector2Int.down,0,1,grid.height-2,-1);
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                MoveTiles(Vector2Int.left,1,1,0,1);
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                MoveTiles(Vector2Int.right,grid.width-2,-1,0,1);
            }
        }
    }

    private void MoveTiles(Vector2Int direction, int startX,int endX ,int startY , int endY)
    {
        bool change  = false;
        for (int x = startX; x >= 0 && x < grid.width; x += endX)
        {
            for (int y = startY; y >= 0 && y < grid.height; y += endY)
            {
                TileCell cell = grid.GetCell(x, y);
                if (cell.occupied)
                {
                    change |= MoveTile(cell.tile, direction);
                    
                }
            }

            if (change)
            {
                StartCoroutine(WaitForAnimation());
            }
        }
    }

    private bool MoveTile(Tile tile, Vector2Int direction)
    {
        TileCell newCell = null;
        TileCell fullOrNot = grid.GetPosCell(tile.cell, direction);

        while (fullOrNot != null)
        {
            if (fullOrNot.occupied)
            {
                //TODO
                break;
            }
            newCell = fullOrNot;
            fullOrNot = grid.GetPosCell(fullOrNot, direction);
        }

        if (newCell != null)
        {
            tile.MoveTo(newCell);
            return true;
        }
        return false;
    }

    private IEnumerator WaitForAnimation()
    {
        _waitForAniamtion = true;
        yield return new WaitForSeconds(0.1f);
        _waitForAniamtion = false;
        
        //Tile ayarı
        // GameOver ekranı
    }
}

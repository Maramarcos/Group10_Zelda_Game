using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private int _chunkID;
    private int _mapID;
    private int tileX;
    private int tileY;

    private TileEnum tileType;
    private SpriteRenderer spriteRenderer;
    private Collider colliderRef;
    private bool startInit = false;
    private bool postInit = false;


    
    void Update()
    {
        if(startInit)
        {
            spriteRenderer.sprite = World.sprites[(int)tileType];

            startInit = false;
            postInit = true;
        }
    }


    public void SetupTile(TileEnum tileType, SpriteRenderer spriteRenderer, Collider collider, int _mapID, int _chunkID)
    {
        this.tileType = tileType;
        this.spriteRenderer = spriteRenderer;
        this.colliderRef = collider;
        this._mapID = _mapID;
        this._chunkID = _chunkID;
    }

    //For mapEditing.
    private void OnMouseDown()
    {
        tileType = MapEditor.selectedTileType;
        World.worldData.GetMap(_mapID).GetChunk(_chunkID).SetTile(tileX, tileY, tileType);        
        spriteRenderer.sprite = World.sprites[(int)tileType];
    }

    //Used by Chunk.cs when making tile
    public void SetXY(int x, int y)
    {
        this.tileX = x;
        this.tileY = y;
        startInit = true;
    }
}

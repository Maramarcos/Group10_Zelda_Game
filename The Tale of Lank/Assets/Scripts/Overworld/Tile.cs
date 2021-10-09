using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private int _chunkID;
    private int _mapID;
    private int tileIndex;

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


    public void SetupTile(TileEnum tileType, SpriteRenderer spriteRenderer, Collider collider, int _mapID, int _chunkID, int tileIndex)
    {
        this.tileType = tileType;
        this.spriteRenderer = spriteRenderer;
        this.colliderRef = collider;
        this._mapID = _mapID;
        this._chunkID = _chunkID;
        this.tileIndex = tileIndex;
        this.spriteRenderer.sprite = spriteRenderer.sprite = World.sprites[(int)tileType];
    }

    //For mapEditing.
    private void OnMouseEnter()
    {
        if(Input.GetMouseButton(0))
        {
            tileType = MapEditor.selectedTileType;
            World.worldData.GetMap(_mapID).GetChunk(_chunkID).SetTile(tileIndex, tileType);
            spriteRenderer.sprite = World.sprites[(int)tileType];
        }
        
    }

    
}

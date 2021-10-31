using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private int _chunkID;
    private int _mapID;
    private int tileIndex;
    private TileInformation tileInfo;
    private SpriteRenderer spriteRenderer;
    private Collider colliderRef;
    private bool startInit = false;
    private bool postInit = false;


    
    void Update()
    {
        //Load the sprite if havn't already
        if(startInit)
        {
            spriteRenderer.sprite = World.tileSets[(int) tileInfo.GetCategory()][tileInfo.GetTileset()]
                .sprites[tileInfo.GetIndex()];

            startInit = false;
            postInit = true;
        }
    }


    public void SetupTile(TileInformation tileInfo, SpriteRenderer spriteRenderer, Collider collider, int _mapID, int _chunkID, int tileIndex)
    {
        this.tileInfo = tileInfo;
        this.spriteRenderer = spriteRenderer;
        this.colliderRef = collider;
        this._mapID = _mapID;
        this._chunkID = _chunkID;
        this.tileIndex = tileIndex;
        this.spriteRenderer.sprite = spriteRenderer.sprite;
        //Debug.Log(tileInfo.GetCategory() + ", "+tileInfo.GetTileset() +", "+ tileInfo.GetIndex());
        this.spriteRenderer.sprite = World.tileSets[(int) tileInfo.GetCategory()][tileInfo.GetTileset()]
            .sprites[tileInfo.GetIndex()];

        
        //Load Collision
        GameObject go;   
        switch (tileInfo.GetCollisionType())
        {
            case TileCollisionEnum.square:
                go = (GameObject)Instantiate(Resources.Load("Prefabs/SquareCollider"));
                go.transform.parent = this.transform;
                return;
            case TileCollisionEnum.triangle0:
                go = (GameObject)Instantiate(Resources.Load("Prefabs/TriangleCollider"));
                go.transform.parent = this.transform;
                go.transform.eulerAngles.Set(0,0,0);
                return;
            case TileCollisionEnum.triangle90:
                go = (GameObject)Instantiate(Resources.Load("Prefabs/TriangleCollider"));
                go.transform.parent = this.transform;
                go.transform.Rotate(0,0,90);
                return;
            case TileCollisionEnum.triangle180:
                go = (GameObject)Instantiate(Resources.Load("Prefabs/TriangleCollider"));
                go.transform.parent = this.transform;
                go.transform.Rotate(0,0,180);
                return;
            case TileCollisionEnum.triangle270:
                go = (GameObject)Instantiate(Resources.Load("Prefabs/TriangleCollider"));
                go.transform.parent = this.transform;
                go.transform.Rotate(0,0,270);
                return;
        }
    }

    //For mapEditing.
    private void OnMouseOver()
    {
        //Change tile under mouse if left click is held and not on top of any UI elements
        if(Input.GetMouseButton(0) && !MapEditor.mapEditor.mainPanelClickBlocker.IsMouseOver())
        {
            tileInfo = MapEditor.selectedTileType;
            World.worldData.GetMap(_mapID).GetChunk(_chunkID).SetTile(tileIndex, tileInfo);
            spriteRenderer.sprite = World.tileSets[(int) tileInfo.GetCategory()][tileInfo.GetTileset()]
                .sprites[tileInfo.GetIndex()];
        }
        
    }

    
    
}

using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private int _chunkID;
    private string mapName;
    private int tileIndex;
    private TileInformation tileInfo;
    private SpriteRenderer spriteRenderer;
    private GameObject collision;
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


    public void SetupTile(TileInformation tileInfo, SpriteRenderer spriteRenderer, Collider collider, string mapName, int _chunkID, int tileIndex)
    {
        this.tileInfo = tileInfo;
        this.spriteRenderer = spriteRenderer;
        this.mapName = mapName;
        this._chunkID = _chunkID;
        this.tileIndex = tileIndex;
        this.spriteRenderer.sprite = spriteRenderer.sprite;
        //Debug.Log(tileInfo.GetCategory() + ", "+tileInfo.GetTileset() +", "+ tileInfo.GetIndex());
        this.spriteRenderer.sprite = World.tileSets[(int) tileInfo.GetCategory()][tileInfo.GetTileset()]
            .sprites[tileInfo.GetIndex()];
        LoadCollision();
    }

    //For mapEditing.
    private void OnMouseOver()
    {
        //Don't do anything if editing is disabled
        if (MapEditor.editingMode == MapEditorMode.TileEditing && !MapEditor.mapEditor.IsMouseOverUI())
        {
            //Change tile under mouse if left click is held and not on top of any UI elements
            if(Input.GetMouseButton(0))
            {
                tileInfo = MapEditor.selectedTileType;
                World.worldData.GetMap(mapName).GetChunk(_chunkID).SetTile(tileIndex, tileInfo);
                spriteRenderer.sprite = World.tileSets[(int) tileInfo.GetCategory()][tileInfo.GetTileset()]
                    .sprites[tileInfo.GetIndex()];
                DestroyImmediate(collision);
                LoadCollision();
            }
        }
        
        

    }

    private void LoadCollision()
    {
        //Load Collision
        GameObject go = null;   
        switch (tileInfo.GetCollisionType())
        {
            case TileCollisionEnum.square:
                go = (GameObject)Instantiate(Resources.Load("Prefabs/SquareCollider"));
                go.transform.parent = this.transform;
                go.transform.localPosition = Vector3.zero;
                break;
            case TileCollisionEnum.triangle0:
                go = (GameObject)Instantiate(Resources.Load("Prefabs/TriangleCollider"));
                go.transform.parent = this.transform;
                go.transform.eulerAngles.Set(0,0,0);
                go.transform.localPosition = Vector3.zero;
                break;
            case TileCollisionEnum.triangle90:
                go = (GameObject)Instantiate(Resources.Load("Prefabs/TriangleCollider"));
                go.transform.parent = this.transform;
                go.transform.Rotate(0,0,90);
                go.transform.localPosition = Vector3.zero;
                break;
            case TileCollisionEnum.triangle180:
                go = (GameObject)Instantiate(Resources.Load("Prefabs/TriangleCollider"));
                go.transform.parent = this.transform;
                go.transform.Rotate(0,0,180);
                go.transform.localPosition = Vector3.zero;
                break;
            case TileCollisionEnum.triangle270:
                go = (GameObject)Instantiate(Resources.Load("Prefabs/TriangleCollider"));
                go.transform.parent = this.transform;
                go.transform.Rotate(0,0,270);
                go.transform.localPosition = Vector3.zero;
                break;
            case TileCollisionEnum.water:
                go = (GameObject)Instantiate(Resources.Load("Prefabs/SquareCollider"));
                go.transform.parent = this.transform;
                go.transform.localPosition = Vector3.zero;
                break;
            case TileCollisionEnum.allowArrow:
                go = (GameObject)Instantiate(Resources.Load("Prefabs/SquareCollider"));
                go.transform.parent = this.transform;
                go.transform.localPosition = Vector3.zero;
                break;
        }

        if (go != null)
        {
            if (tileInfo.GetCollisionType() == TileCollisionEnum.water)
            {
                go.tag = "Water";                
            }
            else if (tileInfo.GetCollisionType() == TileCollisionEnum.allowArrow)
            {
                go.tag = "AllowArrow";
            }
            collision = go;
        }
    }

    public void UnsavedTileChange(TileInformation tmpTileInfo)
    {
        this.spriteRenderer.sprite = World.tileSets[(int) tmpTileInfo.GetCategory()][tmpTileInfo.GetTileset()]
            .sprites[tmpTileInfo.GetIndex()];
        
        

        //Load new Collision
        Destroy(collision);
        GameObject go = null;   
        switch (tmpTileInfo.GetCollisionType())
        {
            case TileCollisionEnum.square:
                go = (GameObject)Instantiate(Resources.Load("Prefabs/SquareCollider"));
                go.transform.parent = this.transform;
                go.transform.localPosition = Vector3.zero;
                break;
            case TileCollisionEnum.triangle0:
                go = (GameObject)Instantiate(Resources.Load("Prefabs/TriangleCollider"));
                go.transform.parent = this.transform;
                go.transform.eulerAngles.Set(0,0,0);
                go.transform.localPosition = Vector3.zero;
                break;
            case TileCollisionEnum.triangle90:
                go = (GameObject)Instantiate(Resources.Load("Prefabs/TriangleCollider"));
                go.transform.parent = this.transform;
                go.transform.Rotate(0,0,90);
                go.transform.localPosition = Vector3.zero;
                break;
            case TileCollisionEnum.triangle180:
                go = (GameObject)Instantiate(Resources.Load("Prefabs/TriangleCollider"));
                go.transform.parent = this.transform;
                go.transform.Rotate(0,0,180);
                go.transform.localPosition = Vector3.zero;
                break;
            case TileCollisionEnum.triangle270:
                go = (GameObject)Instantiate(Resources.Load("Prefabs/TriangleCollider"));
                go.transform.parent = this.transform;
                go.transform.Rotate(0,0,270);
                go.transform.localPosition = Vector3.zero;
                break;
            case TileCollisionEnum.water:
                go = (GameObject)Instantiate(Resources.Load("Prefabs/SquareCollider"));
                go.transform.parent = this.transform;
                go.transform.localPosition = Vector3.zero;
                break;
            case TileCollisionEnum.allowArrow:
                go = (GameObject)Instantiate(Resources.Load("Prefabs/SquareCollider"));
                go.transform.parent = this.transform;
                go.transform.localPosition = Vector3.zero;
                break;
        }

        if (go != null)
        {
            if (tmpTileInfo.GetCollisionType() == TileCollisionEnum.water)
            {
                go.tag = "Water";                
            }
            else if (tmpTileInfo.GetCollisionType() == TileCollisionEnum.allowArrow)
            {
                go.tag = "AllowArrow";
            }
            collision = go;
        }
    }
    
}

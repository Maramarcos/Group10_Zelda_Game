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

        
        GameObject go;   
        switch (tileType.GetCollisionType())
        {
            case TileCollisionEnum.square:
                go = (GameObject)Instantiate(Resources.Load("Prefabs/SquareCollider"));
                go.transform.parent = this.transform;
                return;
            case TileCollisionEnum.triangle0:
                go = (GameObject)Instantiate(Resources.Load("Prefabs/TriangleCollider"));
                go.transform.parent = this.transform;
                go.transform.eulerAngles.Set(0,0,0);
                Debug.Log(tileType + " to triangle rotated @" + new Vector3(0,0,0));
                return;
            case TileCollisionEnum.triangle90:
                go = (GameObject)Instantiate(Resources.Load("Prefabs/TriangleCollider"));
                go.transform.parent = this.transform;
                go.transform.Rotate(0,0,90);
                Debug.Log(tileType + " to triangle rotated @" + new Vector3(0,0,90));
                return;
            case TileCollisionEnum.triangle180:
                go = (GameObject)Instantiate(Resources.Load("Prefabs/TriangleCollider"));
                go.transform.parent = this.transform;
                go.transform.Rotate(0,0,180);
                Debug.Log(tileType + " to triangle rotated @" + new Vector3(0,0,180));
                return;
            case TileCollisionEnum.triangle270:
                go = (GameObject)Instantiate(Resources.Load("Prefabs/TriangleCollider"));
                go.transform.parent = this.transform;
                go.transform.Rotate(0,0,270);
                Debug.Log(tileType + " to triangle rotated @" + new Vector3(0,0,270));
                return;
        }
    }

    //For mapEditing.
    private void OnMouseOver()
    {
        if(Input.GetMouseButton(0))
        {
            tileType = MapEditor.selectedTileType;
            World.worldData.GetMap(_mapID).GetChunk(_chunkID).SetTile(tileIndex, tileType);
            spriteRenderer.sprite = World.sprites[(int)tileType];
        }
        
    }

    
    
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class Chunk : MonoBehaviour
{
    private int _mapID;
    private int _chunkID;
    private bool startInit = false;
    private Tile[] tiles;

    public void Update()
    {
        //Need to wait for parameters. Start starts too early, so do in Update()
        if(startInit)
        {
            tiles = new Tile[ChunkData.tileHeight * ChunkData.tileWidth];

            int i = 0;
            //Iterate through all tiles in tile data and create appropiate gameObjects
            foreach (TileEnum tData in World.worldData.GetMap(_mapID).GetChunk(_chunkID).GetTiles())
            {
                GameObject go = new GameObject();
                tiles[i] = go.AddComponent<Tile>();
                
                //Add spriteRender and BoxCollider to TileObject
                //TODO: Different tiles need different collisions. Implement Later
                tiles[i].SetupTile(tData, tiles[i].gameObject.AddComponent<SpriteRenderer>(), tiles[i].gameObject.AddComponent<BoxCollider>(), _mapID, _chunkID);
                tiles[i].transform.parent = this.transform;
                i++;
            }            

            i = 0;
            //Move tiles to proper positions
            for(int y = 0; y < ChunkData.tileHeight; y++)
            {
                for(int x = 0; x < ChunkData.tileWidth; x++)
                {
                    tiles[i].transform.localPosition = new Vector3(x, y);
                    tiles[i].SetXY(x,y);
                    tiles[i].name="("+x+", " + y + ")";
                    i++;
                }
            }

            startInit = false;
        }
        
    }

    //Used by Map.cs when making chunk
    //Set and signal init
    public void SetID(int _mapID, int _chunkID)
    {
        this._chunkID = _chunkID;
        startInit = true;
    }
}
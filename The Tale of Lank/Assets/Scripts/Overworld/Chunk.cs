using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class Chunk : MonoBehaviour
{
    private string mapName;
    private int _chunkID;
    private bool startInit = false;
    
    //Are these used anywhere?
    private Tile[] tiles;
    private Warp[] warps;
    private Grid[] tileGrids;

    private bool gridEnabled = false;

    public void ToggleGrid()
    {
        float gridSpacing;
        if (gridEnabled)
        {
            gridEnabled = false;
            gridSpacing = 1;
        }
        else
        {
            gridEnabled = true;
            gridSpacing = 1.1f;
        }

        //Move tiles to new positions
        int i = 0;
        for(int y = 0; y < ChunkData.tileHeight; y++)
        {
            for(int x = 0; x < ChunkData.tileWidth; x++)
            {
                tiles[i].transform.localPosition = new Vector3(x * gridSpacing, y * gridSpacing);
                i++;
            }
        }
    }
    public void Update()
    {
        //Need to wait for parameters. Start starts too early, so do in Update()
        if(startInit)
        {
            tiles = new Tile[ChunkData.tileHeight * ChunkData.tileWidth];

            int i = 0;
            //Iterate through all tiles in tile data and create appropiate gameObjects
            foreach (TileInformation tData in World.worldData.GetMap(mapName).GetChunk(_chunkID).GetTiles())
            {
                GameObject go = new GameObject();
                tiles[i] = go.AddComponent<Tile>();
                
                //TODO: I don't thiunk that this is used anymore
                BoxCollider mapEditorCollider = tiles[i].gameObject.AddComponent<BoxCollider>();
                tiles[i].tag = "EditorCollider";
                mapEditorCollider.isTrigger = true;
                
    
                
                tiles[i].SetupTile(tData, tiles[i].gameObject.AddComponent<SpriteRenderer>(), mapEditorCollider, mapName, _chunkID, i);
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
                    tiles[i].name="("+x+", " + y + ")";
                    
                    i++;
                }
            }

            //Load Warps
            if (World.worldData.GetMap(mapName).GetChunk(_chunkID).GetWarps() != null)
            {
                
                try
                {
                    warps = new Warp[World.worldData.GetMap(mapName).GetChunk(_chunkID).GetWarps().GetFlatLength()];
                }
                catch (Exception ex)
                {
                    World.worldData.GetMap(mapName).GetChunk(_chunkID).ResetWarps();
                    MapEditor.SaveMap();
                }
                
                
                
                Flat2DArray<WarpData> wDatas = World.worldData.GetMap(mapName).GetChunk(_chunkID).GetWarps();

                i = 0;
                for (int x = 0; x < wDatas.GetLength(0); x++)
                {
                    for (int y = 0; y < wDatas.GetLength(1); y++)
                    {
                        GameObject go = new GameObject();
                        go.transform.parent = this.transform;
                        go.transform.localPosition = new Vector3(y, x);
                        go.name = "Warp: ("+ x + ", " + y + ")";
                        Warp warp = go.AddComponent<Warp>();
                        BoxCollider2D mapEditorCollider = go.AddComponent<BoxCollider2D>();
                        warp.tag = "EditorCollider";
                        mapEditorCollider.isTrigger = true;
                        warp.InitWarpData(wDatas[y,x], mapName, _chunkID, i);
                        warps[i] = warp;
                        i++;
                    }
                }
            }
            else
            {
                World.worldData.GetMap(mapName).GetChunk(_chunkID).ResetWarps();
                MapEditor.SaveMap();
            }

            startInit = false;
        }

    }

    //Used by Map.cs when making chunk
    //Set and signal init
    public void SetID(string mapName, int _chunkID)
    {
        this.mapName = mapName;
        this._chunkID = _chunkID;
        startInit = true;
    }

    public Tile GetTileByCoord(Vector2Int coord)
    {
        Debug.Log("Called GetTileByCoord()");
        for (int i = 0; i < tiles.Length; i++)
        {
            Vector2Int cCoord = new Vector2Int((int)tiles[i].transform.localPosition.x, (int)tiles[i].transform.localPosition.y);
            if (cCoord == coord)
            {
                return tiles[i];
            }
        }

        return null;
    }
}
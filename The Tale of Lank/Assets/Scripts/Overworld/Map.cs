using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Serialization;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Map : MonoBehaviour
{
    private string mapName;
    private Chunk[] chunks;
    private bool startInit = false; //Start is called before init paramaters, so need to run init after getting parameters

    private bool tileToggle = false;
    private bool chunkToggle = false;

    public void ToggleChunkGrid()
    {
        float modifier;
        if (chunkToggle)
        {
            chunkToggle = false;
            modifier = 1;
            
            //Add 0.1f for if tile grid is also on
            if (tileToggle)
            {
                modifier += 0.1f;
            }
        }
        else
        {
            chunkToggle = true;
            modifier = 1.1f;
            
            //Add 0.1f for if tile grid is also on
            if (tileToggle)
            {
                modifier += 0.1f;
            }
        }
        
        //Move chunks to new positions
        int j = 0;
        for (int y = 0; y < World.worldData.GetMap(mapName).GetChunkLength(1); y++)
        {
            for (int x = 0; x < World.worldData.GetMap(mapName).GetChunkLength(0); x++)
            {
                chunks[j].transform.localPosition = new Vector3(x * ChunkData.tileWidth * modifier, y * ChunkData.tileHeight * modifier);
                j++; 
            }
        }
    }
    
    public void ToggleTileGrid()
    {
        for(int i = 0; i < chunks.Length; i++)
        {
            chunks[i].ToggleGrid();
        }

        float modifier;
        if (tileToggle)
        {
            tileToggle = false;
            modifier = 1;
            
            //Add 0.1f for if chunk grid is also on
            if (chunkToggle)
            {
                modifier += 0.1f;
            }
        }
        else
        {
            tileToggle = true;
            modifier = 1.1f;
            
            //Add 0.1f for if chunk grid is also on
            if (chunkToggle)
            {
                modifier += 0.1f;
            }
        }

        

        //Move chunks to new positions
        int j = 0;
        for (int y = 0; y < World.worldData.GetMap(mapName).GetChunkLength(1); y++)
        {
            for (int x = 0; x < World.worldData.GetMap(mapName).GetChunkLength(0); x++)
            {
                chunks[j].transform.localPosition = new Vector3(x * ChunkData.tileWidth * modifier, y * ChunkData.tileHeight * modifier);
                j++; 
            }
        }
    }
    
    void Update()
    {
        if(startInit)
        {
            chunks = new Chunk[World.worldData.GetMap(mapName).ChunkCount];

            int i = 0;
            //Create chunk objects based on save data
            foreach (ChunkData cData in World.worldData.GetMap(mapName).GetChunks())
            {
                GameObject go = new GameObject();
                chunks[i] = go.AddComponent<Chunk>();
                chunks[i].SetID(mapName, cData.chunkID);
                chunks[i].name = cData.name;
                chunks[i].transform.parent = this.transform;
                i++;
            }

            //Move chunks to proper positions
            i = 0;
            for (int y = 0; y < World.worldData.GetMap(mapName).GetChunkLength(1); y++)
            {
                for (int x = 0; x < World.worldData.GetMap(mapName).GetChunkLength(0); x++)
                {
                    chunks[i].transform.localPosition = new Vector3(x * ChunkData.tileWidth, y * ChunkData.tileHeight);
                    i++; 
                }
            }

            startInit = false;
        }

    }

    public void SetMapName(string mapName)
    {
        this.mapName = mapName;
        startInit = true;
    }

    
    //Destory children when this is destroyed
    public void OnDestroy()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        
        Destroy(this.gameObject);
    }

}

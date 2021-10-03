using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    private int _mapID;
    private Chunk[] chunks;
    private bool startInit = false; //Start is called before init paramaters, so need to run init after getting parameters

    
    void Update()
    {
        if(startInit)
        {
            chunks = new Chunk[World.worldData.GetMap(_mapID).ChunkCount];

            int i = 0;
            //Create chunk objects based on save data
            foreach (ChunkData cData in World.worldData.GetMap(_mapID).GetChunks())
            {
                GameObject go = new GameObject();
                chunks[i] = go.AddComponent<Chunk>();
                chunks[i].SetID(_mapID, cData.chunkID);
                chunks[i].name = cData.name;
                chunks[i].transform.parent = this.transform;
                i++;
            }

            //Move chunks to proper positions
            i = 0;
            for (int y = 0; y < World.worldData.GetMap(_mapID).GetChunkLength(1); y++)
            {
                for (int x = 0; x < World.worldData.GetMap(_mapID).GetChunkLength(0); x++)
                {
                    chunks[i].transform.localPosition = new Vector3(x * ChunkData.tileWidth, y * ChunkData.tileHeight);
                    i++; 
                }
            }

            startInit = false;
        }
    }

    public void SetID(int _mapID)
    {
        this._mapID = _mapID;
        startInit = true;
    }
}

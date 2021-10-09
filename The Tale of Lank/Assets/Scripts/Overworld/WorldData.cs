using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

//Documentation in readme.md in Overworld Folder

[Serializable]
public struct WorldData
{
    [SerializeField]
    private MapData[] maps;

    public MapData GetMap(int _mapID) 
    {
        for(int i = 0; i < maps.Length; i++)
        {
            if(maps[i]._mapID == _mapID)
            {
                return maps[i];
            }
        }

        throw new Exception("Error: Invalid _mapID requested.");
    }

    public readonly MapData[] GetMaps()
    {
        return maps;
    }

    public int MapCount
    {
        get
        {
            return maps.Length;
        }
    }

    //DebugOnly Constructor
    public WorldData(MapData[] maps)
    {
        this.maps = maps;
    }
}

[Serializable]
public struct MapData
{
    [SerializeField]
    private Flat2DArray<ChunkData> chunks;

    [SerializeField]
    public int _mapID;

    public int mapID
    {
        get
        {
            return _mapID;
        }
    }

    
    public int ChunkCount
    {
        get
        {
            return chunks.GetFlatLength();
        }
    }

    public ChunkData GetChunk(int _chunkID)
    {
        for (int i = 0; i < chunks.GetFlatLength(); i++)
        {
            if (chunks[i].chunkID == _chunkID)
            {
                return chunks[i];
            }
        }

        throw new Exception("Error: Invalid _chunkID requested.");
    }

    public readonly Flat2DArray<ChunkData> GetChunks()
    {
        return chunks;
    }

    [SerializeField]
    private string mapName;

    public string name
    {
        get
        {
            return mapName;
        }
    }

    public int GetChunkLength(int dim)
    {
        return chunks.GetLength(dim);
    }

    //DebugOnly Constructor
    public MapData(Flat2DArray<ChunkData> chunks, int _mapID, string mapName)
    {
        this._mapID = _mapID;
        this.chunks = chunks;
        this.mapName = mapName;
    }
}

[Serializable]
public struct ChunkData
{
    //Amount of tiles per chunk
    [NonSerialized]
    public readonly static int tileWidth = 16;
    [NonSerialized]
    public readonly static int tileHeight = 11;

    [SerializeField]
    private Flat2DArray<TileEnum> tiles;

    [SerializeField]
    private int _chunkID;

    public int chunkID
    {
        get
        {
            return _chunkID;
        }
    }

    public TileEnum GetTile(int tileIndex)
    {
        return tiles[tileIndex];
    }

    public void SetTile(int tileIndex, TileEnum tile)
    {
        tiles[tileIndex] = tile;
    }

    [SerializeField]
    private string chunkName;

    public string name 
    { 
        get 
        { 
            return chunkName; 
        }
    }

    public readonly Flat2DArray<TileEnum> GetTiles()
    {
        return tiles;
    }
    
    //DebugOnly Constructor
    public ChunkData(Flat2DArray<TileEnum> tiles, int _chunkID, string chunkName)
    {
        this._chunkID = _chunkID;
        this.tiles = tiles;
        this.chunkName = chunkName;
    }
}
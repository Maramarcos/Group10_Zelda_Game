using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Object = System.Object;
using Random = System.Random;

//Documentation in readme.md in Overworld Folder

[Serializable]
public struct WorldData
{
    [SerializeField]
    private MapData[] maps;

    public MapData GetMap(string mapName)
    {
        for (int i = 0; i < maps.Length; i++)
        {
            if (maps[i].name == mapName)
            {
                return maps[i];
            }
        }

        throw new Exception("Requested map '" + mapName + "' does not exist.");
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
    
    //Map Editor
    public void ChangeMapDimension(string mapName, int x, int y)
    {
        for (int i = 0; i < maps.Length; i++)
        {
            Debug.Log(maps[i].name + " VS" + mapName);
            if (maps[i].name == mapName)
            {
                Debug.Log("Found!");
                maps[i].ChangeChunkSize(x, y);
            }
        }
    }
    public void AddNewMap()
    {
        //Can't have duplicate names. Will cause errors. Generate new name
        Random random = new Random();
        string tmpName = "Map: ";
        bool nameExists;
        do
        {   //Check every randomly generated name. Add another number to the name if it already exists 
            tmpName += random.Next(9);
            
            nameExists = false;
            for (int i = 0; i < maps.Length; i++)
            {
                if (maps[i].name == tmpName)
                {
                    nameExists = true;
                }
            }
        } while (nameExists);
        //Found valid name

        
        //Add Map. Create tmp array with increased array size, add new map, and swap arrays
        MapData mData = new MapData(new Flat2DArray<ChunkData>(0, 0), tmpName);
        MapData[] tmpMaps = new MapData[maps.Length + 1];
        for (int i = 0; i < maps.Length; i++)
        {
            tmpMaps[i] = maps[i];
        }
        tmpMaps[tmpMaps.Length - 1] = mData;
        maps = tmpMaps;

    }
}

[Serializable]
public struct MapData
{
    [SerializeField]
    private Flat2DArray<ChunkData> chunks;
    
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
    public MapData(Flat2DArray<ChunkData> chunks, string mapName)
    {
        this.chunks = chunks;
        this.mapName = mapName;
    }
    
    //MapEditor
    public void ChangeChunkSize(int x, int y)
    {
        //Create new larger/smaller array
        Flat2DArray<ChunkData> tmpArray = new Flat2DArray<ChunkData>(x, y);
        
        
        
        //Create empty tile data
        for (int i = 0; i < tmpArray.GetFlatLength(); i++)
        {
            tmpArray[i] = new ChunkData(new Flat2DArray<TileInformation>(ChunkData.tileWidth, ChunkData.tileHeight), i,
                "DebugChunk:" + i);
        }

        //Copy over old array where possible
        for (int i = 0; i < tmpArray.GetLength(0); i++)
        {
            for (int j = 0; j < tmpArray.GetLength(1); j++)
            {
                //Try catch for cases where chunks[i,j] is out of bounds. 
                try
                {
                    tmpArray[i, j] = chunks[i, j];
                }
                catch(Exception ex)
                {
                    //Don't care about the error. Just let it fail and go onto the next one
                }
            }
        }
        chunks = tmpArray;
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
    private Flat2DArray<TileInformation> tiles;


    [SerializeField]
    private int _chunkID;

    public int chunkID
    {
        get
        {
            return _chunkID;
        }
    }

    public TileInformation GetTile(int tileIndex)
    {
        return tiles[tileIndex];
    }

    public void SetTile(int tileIndex, TileInformation tile)
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

    public readonly Flat2DArray<TileInformation> GetTiles()
    {
        return tiles;
    }
    
    //DebugOnly Constructor
    public ChunkData(Flat2DArray<TileInformation> tiles, int _chunkID, string chunkName)
    {
        this._chunkID = _chunkID;
        this.tiles = tiles;
        this.chunkName = chunkName;
    }
}
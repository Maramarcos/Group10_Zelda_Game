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
public class WorldData
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

    public MapData[] GetMaps()
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
            if (maps[i].name == mapName)
            {
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

    public void DeleteMap(string mapName)
    {
        //Find index of map
        int mapIndex = -1;
        
        for (int i = 0; i < maps.Length; i++)
        {
            if (maps[i].name == mapName)
            {
                mapIndex = i;
                break;
            }
        }

        if (mapIndex == -1)
        {
            throw new Exception("Error: Map '" + mapName + "'doesn't exist!");
            return;
        }
        
        //Shift left all slots over to delete targeted map
        for (int i = mapIndex; i < maps.Length-1; i++)
        {
            maps[i] = maps[i + 1];
        }
        
        //Shrinkened array & swip
        MapData[] tmpMaps = new MapData[maps.Length - 1];
        for (int i = 0; i < tmpMaps.Length; i++)
        {
            tmpMaps[i] = maps[i];
        }
        maps = tmpMaps;
    }

    public void RenameMap(string curMapName, string newMapName)
    {
        for (int i = 0; i < maps.Length; i++)
        {
            if (maps[i].name == curMapName)
            {
                maps[i].SetName(newMapName);
                break;
            }
        }
    }
}

[Serializable]
public class MapData
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

    public Flat2DArray<ChunkData> GetChunks()
    {
        return chunks;
    }

    public void SetName(string newName)
    {
        mapName = newName;
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
                "DebugChunk:" + i, new Flat2DArray<WarpData>(ChunkData.tileWidth, ChunkData.tileHeight));
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
public class ChunkData
{
    //Amount of tiles per chunk
    [NonSerialized]
    public static readonly int tileWidth = 16;
    [NonSerialized]
    public static readonly int tileHeight = 11;

    [SerializeField]
    private Flat2DArray<TileInformation> tiles;

    [SerializeField]
    private Flat2DArray<WarpData> warps;


    [SerializeField]
    private int _chunkID;

    public int chunkID
    {
        get
        {
            return _chunkID;
        }
    }

    public void SetWarp(int warpIndex, WarpData warpData)
    {
        if (warpIndex >= warps.GetFlatLength())
        {
            warps = new Flat2DArray<WarpData>(tileWidth, tileHeight);
        }
        warps[warpIndex] = warpData;
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

    public Flat2DArray<TileInformation> GetTiles()
    {
        return tiles;
    }

    public Flat2DArray<WarpData> GetWarps()
    {
        return warps;
    }

    //Hold over from old map format
    public void ResetWarps()
    {
        warps = new Flat2DArray<WarpData>(tileWidth,tileHeight);
        for (int i = 0; i < warps.GetFlatLength(); i++)
        {
            warps[i] = new WarpData(new Vector2(0,0), "null");
        }
    }

    
    //DebugOnly Constructor
    public ChunkData(Flat2DArray<TileInformation> tiles, int _chunkID, string chunkName, Flat2DArray<WarpData> warps)
    {
        this._chunkID = _chunkID;
        this.tiles = tiles;
        this.chunkName = chunkName;
        this.warps = warps;
    }
}
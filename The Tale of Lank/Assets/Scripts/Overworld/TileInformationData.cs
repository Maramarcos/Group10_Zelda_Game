using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

//Classes/Structs in this file are exclusivly for saving the data for the tilesets

//MasterTilesetData is the parent object of all tileset data information.
//Tilesets are seperated into categories. Each category has multiple tilesets
//Each tileset has multiple tiles.
[Serializable]
public struct MasterTilesetData
{
    [SerializeField]
    private TilesetCategoryData[] tilesetsCategorys;

    public int GetLength()
    {
        return tilesetsCategorys.Length;
    }

    public readonly TilesetCategoryData[] GetCategorys()
    {
        return tilesetsCategorys;
    }

    public TilesetCategoryData GetCategory(int i)
    {
        return tilesetsCategorys[i];
    }
    
    //DEBUG ONLY
    public MasterTilesetData(TilesetCategoryData[] tilesetCategoryData)
    {
        this.tilesetsCategorys = tilesetCategoryData;
    }
}

[Serializable]
public struct TilesetCategoryData
{
    [SerializeField]
    private TilesetData[] tilesetDatas;

    public readonly TilesetData[] GetTilesetData()
    {
        return tilesetDatas;
    }
    
    //DEBUG ONLY
    public TilesetCategoryData(TilesetData[] tilesetDates)
    {
        tilesetDatas = tilesetDates;
    }

    public int GetLength()
    {
        return tilesetDatas.Length;
    }

    public TilesetData GetTilesetData(int i)
    {
        return tilesetDatas[i];
    }
    
}


[Serializable]
public struct TilesetData
{
    [SerializeField]
    private string name;
    [SerializeField]
    private TileData[] tileDatas;

    public TileCollisionEnum GetCollisionType(int _index)
    {
        if (_index >= tileDatas.Length)
        {
            TileData[] tmpArray = new TileData[_index+1];
            for (int i = 0; i < tileDatas.Length; i++)
            {
                tmpArray[i] = tileDatas[i];
            }

            tileDatas = tmpArray;
        }
        
        return tileDatas[_index].GetCollisionType();
        
        
        //Fallback
        return TileCollisionEnum.none;
    }

    public void SetCollisionType(TileCollisionEnum collisionEnum, int _index)
    {
        tileDatas[_index].SetCollision(collisionEnum);
    }

    public TilesetData(string name, TileData[] tileDatas)
    {
        this.name = name;
        this.tileDatas = tileDatas;
    }

    public string GetName()
    {
        return name;
    }
    
}

[Serializable]
public struct TileData
{
    [SerializeField]
    private TileCollisionEnum collisionType;
    [SerializeField]
    private int visualLayer;

    public TileCollisionEnum GetCollisionType()
    {
        return collisionType;
    }

    public void SetCollision(TileCollisionEnum collisionType)
    {
        this.collisionType = collisionType;
    }

    public void SetVisualLayer(int visualLayer)
    {
        this.visualLayer = visualLayer;
    }

    public TileData(TileCollisionEnum tileCollisionEnum, int visualLayer)
    {
        this.collisionType = tileCollisionEnum;
        this.visualLayer = visualLayer;
    }
    
    
}

[Serializable]
public enum Category
{
    Exterior = 0,
    Interior = 1
}


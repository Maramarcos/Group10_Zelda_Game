using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = System.Random;

//Information for a given tile.
[Serializable]
public struct TileInformation
{
    [SerializeField]
    private Category category;
    [SerializeField]
    private int tileSetIndex;
    [SerializeField]
    private int tileIndex;

    public TileCollisionEnum GetCollisionType()
    {
        return World.masterTilesetData.GetCategorys()[(int)category].GetTilesetData()[tileSetIndex]
            .GetCollisionType(tileIndex);
    }

    public Category GetCategory()
    {
        return category;
    }

    public int GetTileset()
    {
        return tileSetIndex;
    }

    public int GetIndex()
    {
        return tileIndex;
    }

    public TileInformation(Category category, int tileSetIndex, int tileIndex)
    {
        this.category = category;
        this.tileSetIndex = tileSetIndex;
        this.tileIndex = tileIndex;
    }
    

}

//All the sprites for the given tileset
public struct TileSet
{
    public TileSet(Sprite[] sprites)
    {
        this.sprites = sprites;
    }

    public Sprite[] sprites;
}

//Collision types
public enum TileCollisionEnum
{
    none,
    triangle0,
    triangle90,
    triangle180,
    triangle270,
    square,
    water
}
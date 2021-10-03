﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Tile Enums are equal to the tile ID. TileIDs will change overtime as the tileset is changed and new features are added
public enum TileEnum
{
    none = 0,
    mountain_grass = 113,
    mountain_grass_n = 105,
    mountain_grass_e = 114,
    mountain_grass_s = 121,
    mountain_grass_w = 112,
    mountain_grass_ne = 106,
    mountain_grass_se = 122,
    mountain_grass_sw = 120,
    mountain_grass_nw = 104,
    water,
    water_edge_n,
    water_edge_e,
    water_edge_s,
    water_edge_w,
    water_edge_ne,
    water_edge_se,        
    water_edge_sw,
    water_edge_nw,
    mountain_inner_corner_sw = 116,
    mountain_inner_corner_se = 137,
    grass = 1,
    sand = 292,
    grass_sand_edge_n = 281,
    grass_sand_edge_e = 288,
    grass_sand_edge_s = 297,
    grass_sand_edge_w = 290,
    grass_sand_edge_ne = 282,
    grass_sand_edge_se = 298,
    grass_sand_edge_sw = 296,
    grass_sand_edge_nw = 280,
    rock_sand = 226,
    rock_grass = 224,
    tree = 4,    
    stairs = 175,
    staircase,
    cave_enterance_b = 167,
    cave_enterance_t = 159
}

//TODO: Implement
public static class TileEnumExtensions
{
    public static bool IsAnimated(this TileEnum tile)
    {
        return false;
    }

    public static int GetAnimationFrame(this TileEnum tile, int frame)
    {
        return 0;
    }

}
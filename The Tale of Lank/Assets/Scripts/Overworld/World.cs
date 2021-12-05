using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class World : MonoBehaviour
{
    public static WorldData worldData;
    public static MasterTilesetData masterTilesetData;
    public static TileSet[][] tileSets;
    public string filepath; //Set with unity editor for now.

    public MasterTilesetData tmpData;
    public bool printMapJson = false;
    public static World thisWorld;

    public static Map[] maps;

    public static string activeMap;

    public static Map GetMapByName(string mapName)
    {
        for (int i = 0; i < maps.Length; i++)
        {
            if (maps[i].name == mapName)
            {
                return maps[i];
            }
        }

        return null;
    }
    

    public static void ToggleTileGrid()
    {
        for(int i = 0; i < maps.Length; i++)
        {
            maps[i].ToggleTileGrid();
        }
    }

    public static void ToggleChunkGrid()
    {
        for (int i = 0; i < maps.Length; i++)
        {
            maps[i].ToggleChunkGrid();
        }
    }
    public static void SetActiveMap(string mapName)
    {
        PlayerMovement.player.ForceStopInput();
        for (int i = 0; i < maps.Length; i++)
        {
            if (maps[i].name == mapName)
            {
                activeMap = mapName;
                maps[i].gameObject.SetActive(true);
            }
            else
            {
                maps[i].gameObject.SetActive(false);
            }
        }

        if (mapName != activeMap)
        {
            PlayerMovement.player.ForceStartInput();
            throw new Exception("Map Name: '" + mapName + "' does not exist. Couldn't load new map");
        }
        PlayerMovement.player.ForceStartInput();
    }
    
    public static void ReloadMapData()
    {
        for (int i = maps.Length - 1; i >= 0; i--)
        {
            Destroy(maps[i]);
        }
        LoadMapData();
        
        //Tell MapEditor to Reset the MapDropdown
        MapEditor.mapEditor.ReloadMapDropdown();
    }

    private static void LoadMapData()
    {
        //Load map information
        string loadFile = System.IO.File.ReadAllText(Application.dataPath + "/" + "debugMapData" + ".json");
        //Debug.Log(loadFile);
        worldData = JsonUtility.FromJson<WorldData>(loadFile);

        //Setup map data from map info
        maps = new Map[worldData.MapCount];
        int i = 0;        
        foreach(MapData mData in worldData.GetMaps())
        {
            //Create map object and store the reference in maps
            maps[i] = new GameObject().AddComponent<Map>();
            maps[i].SetMapName(mData.name);
            maps[i].name = mData.name;            

            maps[i].transform.parent = thisWorld.transform;
            i++;
        }
    }
    
    void Start()
    {
        //Load tileset information
        string loadFile = System.IO.File.ReadAllText(Application.dataPath + "/" + "debugTilesetData" + ".json");
        masterTilesetData = JsonUtility.FromJson<MasterTilesetData>(loadFile);

        //Load tileset sprites
        tileSets = new TileSet[masterTilesetData.GetLength()][];
        int i = 0;
        foreach (TilesetCategoryData cData in masterTilesetData.GetCategorys())
        {
            tileSets[i] = new TileSet[cData.GetLength()];
            int j = 0;
            foreach (TilesetData tData in cData.GetTilesetData())
            {
                
                //Debug.Log("Textures/Tileset/" +(Category)i + "/" + (Category)i + j);
                Resources.LoadAll<Sprite>("Textures/Tileset/" + (Category) i + "/" + (Category) i + j);
                tileSets[i][j] = new TileSet(Resources.LoadAll<Sprite>("Textures/Tileset/" + (Category)i + "/" + (Category)i + j));
                j++;
            }

            i++;
        }

        



        thisWorld = this;
        LoadMapData();
        MapEditor.mapEditor.NotifyTilesetLoaded();
        SetActiveMap(maps[0].name);
        MapEditor.mapEditor.ReloadMapDropdown();

    }

    
    void Update()
    {
        if(printMapJson)
        {
            Debug.Log(JsonUtility.ToJson(worldData));
            printMapJson = false;
        }
        tmpData = masterTilesetData;

    }

    
}

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

    public static Map[] maps;
    
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
        
        
        
        
        
        //Load map information
        loadFile = System.IO.File.ReadAllText(Application.dataPath + "/" + "debugMapData" + ".json");
        //Debug.Log(loadFile);
        worldData = JsonUtility.FromJson<WorldData>(loadFile);

        //Setup map data from map info
        maps = new Map[worldData.MapCount];
        i = 0;        
        foreach(MapData mData in worldData.GetMaps())
        {
            //Create map object and store the reference in maps
            maps[i] = Instantiate(new GameObject()).AddComponent<Map>();
            maps[i].SetID(mData._mapID);
            maps[i].name = mData.name;            

            maps[i].transform.parent = this.transform;
            i++;
        }
        
        MapEditor.mapEditor.NotifyTilesetLoaded();
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

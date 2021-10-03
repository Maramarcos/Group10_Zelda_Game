using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Not proper implemented yet. Need to fix Save+Load 1st

public class MapEditor : MonoBehaviour
{
    public static TileEnum selectedTileType;
    public TileEnum tmpEnum;

    public bool ForceSave = false;

    public static void SaveMap()
    {
        string tmp = JsonUtility.ToJson(World.worldData);
        System.IO.File.WriteAllText(Application.dataPath + "/" + "debugMapData" + ".json", tmp);
        Debug.Log("Wrote to: " + Application.dataPath + "/" + "debugMapData" + ".json");
    }
    
    void Start()
    {
        
    }

    
    void Update()
    {
        selectedTileType = tmpEnum;
        if(ForceSave)
        {
            ForceSave = false;
            SaveMap();
        }
    }
}

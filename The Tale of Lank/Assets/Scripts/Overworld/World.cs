using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class World : MonoBehaviour
{
    public static WorldData worldData;
    public static Sprite[] sprites;
    public string filepath; //Set with unity editor for now.

    public bool printMapJson = false;

    public static Map[] maps;
    
    void Start()
    {
        //Load sprites
        sprites = Resources.LoadAll<Sprite>(filepath);

        string loadFile = System.IO.File.ReadAllText(Application.dataPath + "/" + "debugMapData" + ".json");
        //Debug.Log(loadFile);
        worldData = JsonUtility.FromJson<WorldData>(loadFile);

        maps = new Map[worldData.MapCount];

        int i = 0;        
        foreach(MapData mData in worldData.GetMaps())
        {
            //Create map object and store the reference in maps
            maps[i] = Instantiate(new GameObject()).AddComponent<Map>();
            maps[i].SetID(mData._mapID);
            maps[i].name = mData.name;            

            maps[i].transform.parent = this.transform;
            i++;
        }
        
    }

    
    void Update()
    {
        if(printMapJson)
        {
            Debug.Log(JsonUtility.ToJson(worldData));
            printMapJson = false;
        }
    }

}

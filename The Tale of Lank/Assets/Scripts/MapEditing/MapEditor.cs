using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MapEditor : MonoBehaviour
{
    public static TileInformation selectedTileType;
    public static MapEditor mapEditor;
    public Image selectedImage;
    public ClickThroughPreventer mainPanelClickBlocker;
    public Transform tileSetScrollView;

    public Dropdown tileSetDropdown;
    public Dropdown collisionDropdown;

    public bool ForceSave = false; //Debug

    private List<GameObject> tileButtons; //For selecting current tile

    private Dictionary<int, Vector2Int> dropboxToTileSet; //Convert the dropbox values into usable data (category, and index) for loading.

    //Called through button on UI
    public static void SaveMap()
    {
        string tmp = JsonUtility.ToJson(World.worldData);
        System.IO.File.WriteAllText(Application.dataPath + "/" + "debugMapData" + ".json", tmp);
        Debug.Log("Wrote to: " + Application.dataPath + "/" + "debugMapData" + ".json");
    }
    
    void Start()
    {
        /*
        //DEBUG
        List<TilesetCategoryData> tcData = new List<TilesetCategoryData>();
        tcData.Add(new TilesetCategoryData(new TilesetData[2]{new TilesetData("alpha", new List<TileData>(){new TileData(),new TileData(),new TileData(),new TileData()}), new TilesetData("beta", new List<TileData>(){new TileData(),new TileData()})}));
        tcData.Add(new TilesetCategoryData(new TilesetData[2]{new TilesetData("delta", null), new TilesetData("gamma", null)}));
        MasterTilesetData mData = new MasterTilesetData(tcData.ToArray());
        Debug.Log(mData.GetCategorys().Length);
        string tmp = JsonUtility.ToJson(mData);
        System.IO.File.WriteAllText(Application.dataPath + "/" + "debugTilesetData" + ".json", tmp);
        */
        
        //Init variables
        dropboxToTileSet = new Dictionary<int, Vector2Int>();
        tileButtons = new List<GameObject>();
        mapEditor = this;
        InitCollisionDropdownUI();
    }

    //Can't do stuff with the tilesets before the tilesets have loaded. This is called by World when the tilesets have been loaded
    public void NotifyTilesetLoaded()
    {
        //Load default tileset
        LoadTilePallet(Category.Exterior, 0);
        InitTileSetDropdownUI();
    }

    
    void Update()
    {
        if(ForceSave)
        {
            ForceSave = false;
            SaveMap();
        }
    }

    public void SetSelectedTile(TileInformation tileInfo)
    {
        selectedTileType = tileInfo;
        selectedImage.sprite = World.tileSets[(int) tileInfo.GetCategory()][tileInfo.GetTileset()]
            .sprites[tileInfo.GetIndex()];
        
        //Set ui tile info to represent newly selected tile
        collisionDropdown.SetValueWithoutNotify((int)tileInfo.GetCollisionType());
    }

    private void InitCollisionDropdownUI()
    {
        //Init Collision Types. Get the name of each enum and add to dropdown
        List<Dropdown.OptionData> opl = new List<Dropdown.OptionData>();
        foreach (String cType in Enum.GetNames((typeof(TileCollisionEnum))))
        {
            opl.Add(new Dropdown.OptionData(cType));
        }

        collisionDropdown.options = opl;
    }

    //Inits the dropdown with the names for each tileset
    private void InitTileSetDropdownUI()
    {
        List<Dropdown.OptionData> opl = new List<Dropdown.OptionData>();     
        //Iterate through each tileset in each category
        int i = 0;
        for (int category = 0; category < World.masterTilesetData.GetCategorys().Length; category++)
        {
            for (int tileset = 0; tileset < World.masterTilesetData.GetCategorys()[category].GetLength(); tileset++)
            {
                dropboxToTileSet.Add(i, new Vector2Int(category, tileset));
                opl.Add(new Dropdown.OptionData(World.masterTilesetData.GetCategorys()[category].GetTilesetData()[tileset].GetName()));
                i++;
            }
        }
        tileSetDropdown.options = opl;
    }
    
    //Changes the loaded tilepalett in the UI for map editing
    private void LoadTilePallet(Category tileSetCategory, int tileSetIndex)
    {
        //Remove old button objects before creating new ones
        if (tileButtons.Count != 0)
        {
            foreach (GameObject obj in tileButtons)
            {
                Destroy(obj);
            }
        }

        //Load tileset in a MxN grid to match the base tileset image
        int width = 0;
        int height = 0;
        int i = 0;
        foreach (Sprite sprite in World.tileSets[(int)tileSetCategory][tileSetIndex].sprites)
        {
            //Set the default selected tile to the 1st tile in the tileset
            if (i == 0)
            {
                selectedImage.sprite = World.tileSets[(int) tileSetCategory][tileSetIndex]
                    .sprites[0]; 
            }
            
            //Reset width when reached edge and goto next row
            if (width == 8)
            {
                width = 0;
                height++;
            }
            
            //Create the tile button for selecting the current tile.
            GameObject go = (GameObject)Instantiate(Resources.Load("Prefabs/TilePalettButton"));
            //go.transform.parent = tileSetScrollView;
            go.transform.SetParent(tileSetScrollView);
            go.transform.localPosition = new Vector3(20+(width*44), -20-(height*44), 0);
            go.GetComponent<Image>().sprite = sprite;
            go.GetComponent<TileButton>().SetTileInfo(new TileInformation(tileSetCategory, tileSetIndex, i));
            tileButtons.Add(go);
            width++;
            i++;
        }
        
        

    }

    //Called when TileSet Dropbox changes
    public void OnTilesetChange()
    {
        Vector2Int newTileset = dropboxToTileSet[tileSetDropdown.value];
        LoadTilePallet((Category)newTileset.x, newTileset.y);
    }

    //Note: Collision is not refreshed. Must stop and start unity again for now
    //Called when CollisionType Dropbox changes
    public void OnCollisionChange()
    {
        //Set the new collision type in loaded tilesetData
        World.masterTilesetData.GetCategorys()[(int) selectedTileType.GetCategory()].GetTilesetData()[
            selectedTileType.GetTileset()].SetCollisionType((TileCollisionEnum)collisionDropdown.value, selectedTileType.GetIndex());
        
        //Save the collision type
        string tmp = JsonUtility.ToJson(World.masterTilesetData);
        System.IO.File.WriteAllText(Application.dataPath + "/" + "debugTilesetData" + ".json", tmp);

    }
    
    
}

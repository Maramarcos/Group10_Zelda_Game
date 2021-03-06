using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;


public class MapEditor : MonoBehaviour
{
    public static TileInformation selectedTileType;
    public static MapEditor mapEditor;
    
    public Image selectedImage;
    public ClickThroughPreventer mainPanelClickBlocker;
    public ClickThroughPreventer mapPanelClickBlocker;
    public ClickThroughPreventer warpPanelClickBlocker;

    public InputField newMapName;
    
    public Transform tileSetScrollView;

    public Dropdown tileSetDropdown;
    public Dropdown collisionDropdown;
    public Canvas editingCanvas;

    public InputField xChunks;
    public InputField yChunks;

    public bool toggleEditing;

    public Dropdown currentMapDropdown;

    public GameObject selectedWarpIndicator;

    public static MapEditorMode editingMode = MapEditorMode.TileEditing;

    private Warp selectedWarpObject;
    private WarpData selectedWarpData;
    
    public bool ForceSave = false; //Debug

    private List<GameObject> tileButtons; //For selecting current tile

    private Dictionary<int, Vector2Int> dropboxToTileSet; //Convert the dropbox values into usable data (category, and index) for loading.

    public InputField warpXDestination;
    public InputField warpYDestination;
    public InputField warpMapDestination;
    public Toggle warpEditingMode;
    public Toggle tileEditingMode;



    public bool IsMouseOverUI()
    {
        if (mainPanelClickBlocker.mouseOver || mapPanelClickBlocker.mouseOver || warpPanelClickBlocker.mouseOver)
        {
            return true;
        }

        return false;
    }
    
    //Called through button on UI
    public static void SaveMap()
    {
        string tmp = JsonUtility.ToJson(World.worldData);
        System.IO.File.WriteAllText(Application.dataPath + "/" + "debugMapData" + ".json", tmp);
        Debug.Log("Wrote to: " + Application.dataPath + "/" + "debugMapData" + ".json");
    }

    public void ToggleEditing()
    {
        if (editingMode != MapEditorMode.Disabled)
        { 
            editingMode = MapEditorMode.Disabled;
            editingCanvas.enabled = false;
        }
        else
        {
            editingMode = MapEditorMode.TileEditing;
            editingCanvas.enabled = true;
        }
    }
    void Start()
    {
        ToggleEditing(); //Turn off map editor by default
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

    public void EnableTileMode()
    {
        editingMode = MapEditorMode.TileEditing;
    }

    public void EnableWarpMode()
    {
        editingMode = MapEditorMode.WarpEditing;
    }

    
    void Update()
    {
        if(ForceSave)
        {
            ForceSave = false;
            SaveMap();
        }

        if (toggleEditing)
        {
            toggleEditing = false;
            ToggleEditing();
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

    public void CreateNewMap()
    {
        World.worldData.AddNewMap();
        //Save Changes
        string tmp = JsonUtility.ToJson(World.worldData);
        System.IO.File.WriteAllText(Application.dataPath + "/" + "debugMapData" + ".json", tmp);
        Debug.Log("Wrote to: " + Application.dataPath + "/" + "debugMapData" + ".json");
        World.ReloadMapData();

    }


    public void ChangeMapSize()
    {
        int x;
        int y;
        if (int.TryParse(xChunks.text, out x) && int.TryParse(yChunks.text, out y))
        {
            World.worldData.ChangeMapDimension(currentMapDropdown.options[currentMapDropdown.value].text, x, y);
        }
    }

    public void ReloadMapDropdown()
    {
        List<Dropdown.OptionData> opl = new List<Dropdown.OptionData>();     
        //Iterate through each tileset in each category
        for (int i = 0; i < World.worldData.MapCount; i++)
        {
            opl.Add(new Dropdown.OptionData(World.worldData.GetMaps()[i].name));
        }
        currentMapDropdown.options = opl;
    }

    public void ChangeMap()
    {
        World.SetActiveMap(currentMapDropdown.options[currentMapDropdown.value].text);
    }

    public void UpdateWarpModView(WarpData selectedWarpData, Warp selectedWarpObject)
    {
        this.selectedWarpData = selectedWarpData;
        this.selectedWarpObject = selectedWarpObject;
        this.selectedWarpIndicator.transform.position = selectedWarpObject.transform.position;
        UpdateWarpUI();
    }

    private void UpdateWarpUI()
    {
        warpXDestination.text = selectedWarpData.GetTargetLocation().x.ToString();
        warpYDestination.text = selectedWarpData.GetTargetLocation().y.ToString();
        warpMapDestination.text = selectedWarpData.GetTargetMap();
    }

    //Doesn't save to file. Only to instance
    public void ApplyWarpChanges()
    {
        int x;
        int y;
        if (int.TryParse(warpXDestination.text, out x) && int.TryParse(warpYDestination.text, out y))
        {
            selectedWarpData = new WarpData(new Vector2(x, y), warpMapDestination.text);
        }
        selectedWarpObject.ApplyChanges(selectedWarpData);
        
    }

    public void DeleteMap()
    {
        string deleteTarget = currentMapDropdown.options[currentMapDropdown.value].text;
        
        World.worldData.DeleteMap(deleteTarget);
        
        //Save Changes
        string tmp = JsonUtility.ToJson(World.worldData);
        System.IO.File.WriteAllText(Application.dataPath + "/" + "debugMapData" + ".json", tmp);
        Debug.Log("Wrote to: " + Application.dataPath + "/" + "debugMapData" + ".json");
        World.ReloadMapData();
    }

    public void ChangeMapName()
    {
        World.worldData.RenameMap(currentMapDropdown.options[currentMapDropdown.value].text, newMapName.text);
    }

    public void ToggleTileGrid()
    {
        World.ToggleTileGrid();

    }

    public void ToggleChunkGrid()
    {
        World.ToggleChunkGrid();
    }
    
}

public enum MapEditorMode
{
    Disabled,
    TileEditing,
    WarpEditing,
    EnemyEditing,
    ItemEditing


}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Warp : MonoBehaviour
{
    private WarpData warpData;
    
    private int _chunkID;
    private string mapName;
    private int warpIndex;
    
    private GameObject collision;


    public void InitWarpData(WarpData warpData, string mapName, int _chunkID, int warpIndex)
    {
        this.warpData = warpData;
        this._chunkID = _chunkID;
        this.mapName = mapName;
        this.warpIndex = warpIndex;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Only warp if player touches warp.
        if (other.gameObject.CompareTag("Player") && warpData != null && !warpData.GetTargetMap().Equals("") && !warpData.GetTargetMap().Equals("null"))
        {
            PlayerMovement.player.WarpPlayer(warpData.GetTargetMap(), warpData.GetTargetLocation().x, warpData.GetTargetLocation().y);
        }
    }
    
    private void OnMouseUpAsButton()
    {
        //Only do work if in warp editing mode
        if (MapEditor.editingMode == MapEditorMode.WarpEditing && !MapEditor.mapEditor.IsMouseOverUI())
        {
            //Set the selected warp data in map editor to current warp
            MapEditor.mapEditor.UpdateWarpModView(warpData, this);
        }
        
    }

    public void ApplyChanges(WarpData warpData)
    {
        this.warpData = warpData;
        World.worldData.GetMap(mapName).GetChunk(_chunkID).SetWarp(warpIndex, warpData);

    }
}

[Serializable]
public class WarpData
{
    //targetXY is the pixel that the warp will warp the player to. Look at Player Position on Player in UnityInspector to know what to put for here.
    //mapName is which map it will warp the player to.
   
    [SerializeField]
    private Vector2 targetXY;
    [SerializeField]
    private string mapName;
    


    public Vector2 GetTargetLocation()
    {
        return targetXY;
    }

    public string GetTargetMap()
    {
        return mapName;
    }

    public WarpData(Vector2 targetXY, string mapName)
    {
        this.targetXY = targetXY;
        this.mapName = mapName;
    }
}
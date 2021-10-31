using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Tells mapeditor to change the selected tile to *this* tileInfo
public class TileButton : MonoBehaviour
{
    private TileInformation tileInfo;

    public void SetTileInfo(TileInformation tileInfo)
    {
        this.tileInfo = tileInfo;
    }

    public void OnClick()
    {
        MapEditor.mapEditor.SetSelectedTile(tileInfo);
    }
    
}

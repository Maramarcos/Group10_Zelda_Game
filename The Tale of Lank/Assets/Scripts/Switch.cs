using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    public TileInformation[] from;
    public TileInformation[] to;
    public TileObjectReference[] targetStringReference;
    public string targetMap;
    private Tile[] targets;
    
    private BoxCollider2D collision;
    private SpriteRenderer spriteRenderer;


    public float cooldown;

    private float timeRemaining;

    private bool active;

    // Start is called before the first frame update
    void Start()
    {
        active = false;
        timeRemaining = cooldown;
        this.collision = GetComponent<BoxCollider2D>();
        this.spriteRenderer = GetComponent<SpriteRenderer>();

        this.tag = "Switch";
        this.collision.gameObject.tag = "Switch";
        targets = new Tile[targetStringReference.Length];

    }

    public void Init()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (timeRemaining>0)
        {
            timeRemaining -= 1;            
        }

        if (World.activeMap != targetMap)
        {
            collision.enabled = false;
            spriteRenderer.enabled = false;

        }
        else
        {
            collision.enabled = true;
            spriteRenderer.enabled = true;

        }
    }

    public void Toggle()
    {
        Debug.Log("Called Switch!");
        if (timeRemaining == 0)
        {
            timeRemaining = cooldown;

            for (int i = 0; i < targets.Length; i++)
            {
                //Get tile reference. Need to to this onDemand because we can't predict when a given map, chunk, tile will be load/unloaded during void Start()
                Map map = World.GetMapByName(targetMap);
                Chunk chunk = null;
                Tile tile = null;
                if (map != null)
                {
                    chunk = map.GetChunkByName(targetStringReference[i].chunkName);
                }
                else
                {
                    Debug.Log("Map: " + map);
                }

                if (chunk != null)
                {
                    tile = chunk.GetTileByCoord(targetStringReference[i].tilePos);
                }
                else
                {
                    Debug.Log("Chunk: " + chunk);
                }
                
                if (active)
                {
                    tile.UnsavedTileChange(from[i]);
                }
                else
                {
                    tile.UnsavedTileChange(to[i]);
                }                
            }

            active = !active;
        }
    }
    
    
}

[Serializable]
public struct TileObjectReference
{
    [SerializeField]
    public string chunkName;
    [SerializeField]
    public Vector2Int tilePos;
}

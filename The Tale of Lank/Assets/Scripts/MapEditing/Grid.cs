using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    private string mapName;

    private Renderer myRenderer;
    // Start is called before the first frame update
    void Start()
    {
        //myRenderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (World.activeMap != mapName)
        {
            //myRenderer.enabled = false;
        }
        else
        {
            //myRenderer.enabled = true;
        }
    }

    public void Setup(string mapName)
    {
        this.mapName = mapName;
    }
}

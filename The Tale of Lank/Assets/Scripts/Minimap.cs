using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    public string targetMap;
    public Camera me;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (World.activeMap != targetMap)
        {
            me.enabled = false;
        }
        else
        {
            me.enabled = true;
        }
    }
}

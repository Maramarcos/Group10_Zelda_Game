using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sign : MonoBehaviour
{
    // Start is called before the first frame update
    public string text;
    public string targetWorld;

    private BoxCollider2D collision;
    
    void Start()
    {
        this.collision = GetComponent<BoxCollider2D>();
        this.tag = "Sign";
        this.collision.gameObject.tag = "Sign";
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (World.activeMap != targetWorld)
        {
            collision.enabled = false;
        }
        else
        {
            collision.enabled = true;
        }
    }

    public void ShowText()
    {
        TextBox.textBox.ShowText(text);
        Debug.Log(text);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBox : MonoBehaviour
{
    // Start is called before the first frame update
    public Canvas frame;
    public Text text;
    public static TextBox textBox;
    void Start()
    {
        textBox = this;
        frame.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (frame.enabled && Input.GetButtonDown("attack"))
        {
            frame.enabled = false;
        }
    }

    public void ShowText(string msg)
    {
        text.text = msg;
        frame.enabled = true;
    }
}

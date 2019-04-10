using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    public float charge = 1.0f;

    // start is called before the first frame update
    void Start()
    {
        UpdateColor();
    }

    public void UpdateColor()
    {
        // + blue, - red, 0 black
        Color color = charge > 0 ? Color.blue : charge < 0 ? Color.red : Color.black;
        GetComponent<Renderer>().material.color = color;
    }
}

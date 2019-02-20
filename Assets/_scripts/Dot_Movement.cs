using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot_Movement : MonoBehaviour
{
    private bool is_held;

    void start()
    {
        is_held = false;
    }

    public void hold()
    {
        is_held = true;
    }

    public void drop()
    {
        is_held = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(is_held)
        {
            float xpos = Mathf.RoundToInt( (Input.mousePosition.x/Screen.width) * 18f );
            float ypos = Mathf.RoundToInt( (Input.mousePosition.y/Screen.height) * 10f );
            transform.position = new Vector3(xpos , ypos, 0);
        }
    }
}

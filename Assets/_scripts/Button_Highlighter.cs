using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button_Highlighter : MonoBehaviour
{
    SpriteRenderer child_rend;
    Color on;
    Color off;

    private void Awake()
    {
        child_rend = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        child_rend.color = new Color(0, 0, 0, 0);
        // "#d8f4f2"
        on = new Color(1.0f, 0.512f, 0.48f, 0.5f);
        off = new Color(1.0f, 0.512f, 0.48f, 0f);
    }

    private void OnMouseDown()
    {
        child_rend.color = off;
    }

    private void OnMouseEnter()
    {
        
        child_rend.color = on;
        
    }

    private void OnMouseExit()
    {
        
        child_rend.color = off;
        
    }
}

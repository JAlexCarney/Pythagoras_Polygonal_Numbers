using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_Select_Button : MonoBehaviour
{
    public int lvl_ID;
    public Sprite playable;
    public Sprite locked;
    public Sprite completed;
    public GameObject handler_object;
    private Level_Handler lvl_handler;
    private SpriteRenderer s_rend;
    private SpriteRenderer child_rend;
    private bool is_locked;

    public void Unlock_Level()
    {
        is_locked = false;
        s_rend.sprite = playable;
    }

    public void Complete_Level()
    {
        s_rend.sprite = completed;
    }
    
    private void Awake()
    {
        lvl_handler = handler_object.GetComponent<Level_Handler>();
        s_rend = GetComponent<SpriteRenderer>();
        s_rend.sprite = locked;
        is_locked = true;
        child_rend = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        child_rend.color = new Color(0, 0, 0, 0);
    }
    
    private void OnMouseDown()
    {
        if (!is_locked)
        {
            // enter lvl
            GetComponent<AudioSource>().Play();
            lvl_handler.Start_Opening_Speech(lvl_ID);
        }
    }

    private void OnMouseEnter()
    {
        if (!is_locked)
        {
            // "#d8f4f2"
            child_rend.color = new Color(1.0f, 0.512f, 0.48f, 0.5f);
        } 
    }

    private void OnMouseExit()
    {
        if (!is_locked)
        {
            child_rend.color = new Color(1.0f, 0.512f, 0.48f, 0f); ;
        }
    }
}

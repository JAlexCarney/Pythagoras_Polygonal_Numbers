using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot_Handler : MonoBehaviour
{
    public GameObject dot_prefab;

    private bool is_playing;
    private bool is_holding_dot;
    private int held_dot_index;
    private GameObject[] dots;
    private Vector2[] dot_positions;
    
    private void Start()
    {
        is_holding_dot = false;
        held_dot_index = -1;
        is_playing = false;
    }

    public void play()
    {
        is_playing = true;
    }

    public void stop()
    {
        is_playing = false;
    }

    public void place_dots(Vector2[] points)
    {
        dots = new GameObject[points.Length];
        for (int i = 0; i < points.Length; i++)
        {
            GameObject new_dot = Instantiate(dot_prefab);
            new_dot.transform.position = new Vector3(points[i].x, points[i].y, 0);
            dots[i] = new_dot;
            dot_positions = points;
        }
    }

    public void Remove_Dots()
    {
        foreach (GameObject dot in dots)
        {
            Destroy(dot);
        }
    }

    public Vector2[] Get_Dot_Positions()
    {
        return dot_positions;
    }

    // Update is called once per frame
    void Update()
    {
        if (is_playing)
        { 
            if (Input.GetButtonDown("Fire1"))
            {
                if (!is_holding_dot)
                {
                    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

                    RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
                    if (hit.collider != null)
                    {
                        for (int i = 0; i < dots.Length; i++)
                        {
                            if (hit.collider.gameObject == dots[i])
                            {
                                is_holding_dot = true;
                                held_dot_index = i;
                                transform.GetChild(2).GetComponent<AudioSource>().Play();
                                dots[i].GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.512f, 0.48f, 1f);
                            }
                        }
                    }
                }
            }
            if (is_holding_dot && Input.GetButtonUp("Fire1"))
            {
                is_holding_dot = false;
                dot_positions[held_dot_index].x = dots[held_dot_index].transform.position.x;
                dot_positions[held_dot_index].y = dots[held_dot_index].transform.position.y;
                dots[held_dot_index].GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 1f);
                transform.GetChild(1).GetComponent<AudioSource>().Play();
            }
            if (is_holding_dot)
            {
                float xpos = Mathf.RoundToInt((Input.mousePosition.x / Screen.width) * 18f);
                float ypos = Mathf.RoundToInt((Input.mousePosition.y / Screen.height) * 10f);
                bool valid_place = true;
                if (xpos <= 0 || xpos >= 18 || ypos <= 0 || ypos >= 10 || (xpos >= 15 && ypos <= 2) || (xpos >= 15 && ypos >= 7))
                {
                    valid_place = false;
                }
                foreach (GameObject dot in dots)
                {
                    Vector3 dot_pos = dot.transform.position;
                    if (dot != dots[held_dot_index] && dot_pos.x == xpos && dot_pos.y == ypos)
                    {
                        valid_place = false;
                    }
                }
                if(valid_place)
                    dots[held_dot_index].transform.position = new Vector3(xpos, ypos, 0);
            }
        }
    }
}

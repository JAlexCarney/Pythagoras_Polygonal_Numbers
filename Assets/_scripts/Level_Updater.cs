using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_Updater : MonoBehaviour
{
    public List<GameObject> children;

    public void Unlock_lvl(int lvl_ID)
    {
        foreach (var child in children)
        {

            Level_Select_Button lvl_select_button = child.GetComponent<Level_Select_Button>();
            if (lvl_ID == lvl_select_button.lvl_ID)
            {
                lvl_select_button.Unlock_Level();
            }
        }
        Debug.Log("Unlocked lvl " + lvl_ID);
    }

    public void Complete_lvl(int lvl_ID)
    {
        foreach (var child in children)
        {
            Level_Select_Button lvl_select_button = child.GetComponent<Level_Select_Button>();
            if (lvl_ID == lvl_select_button.lvl_ID)
            {
                lvl_select_button.Complete_Level();
            }
        }
        Debug.Log("Completed lvl " + lvl_ID);
    }
}

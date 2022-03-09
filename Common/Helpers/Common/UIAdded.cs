using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UIAdded
{
    public static List<GameObject> listUIAddeds;
    
    public static void AddUI()
    {
        listUIAddeds = new List<GameObject>();
        listUIAddeds.Add(UIHandler.Instance.popUpExit);
        listUIAddeds.Add(UIHandler.Instance.guideBoard);
        listUIAddeds.Add(UIHandler.Instance.listAudioBoard);
    }

    public static bool CheckEnaleOneOfUI()
    {
        foreach (GameObject obj in listUIAddeds)
        {
            if (obj.activeSelf == true)
            {
                return true;
            }
        }
        return false;
    }
}

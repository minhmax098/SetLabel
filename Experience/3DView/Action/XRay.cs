using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRay : MonoBehaviour
{
    private static XRay instance;
    public  static XRay Instance 
    { 
        get 
        { 
            if (instance == null)
                instance = FindObjectOfType<XRay>();
            return instance; 
        } 
    }

    // public void ChangeMaterial(Material material)
    // {
    //     int childCount = GameObjectManager.Instance.CurrentObject.transform.childCount;
    //     if (childCount > 0)
    //     {
    //         for (int i = 0; i < childCount; i++)
    //         {
    //             GameObjectManager.Instance.CurrentObject.transform.GetChild(i).gameObject.GetComponent<MeshRenderer>().material = material;
    //         }
    //         GameObjectManager.Instance.CurrentObject.GetComponent<MeshRenderer>().material = material;
    //     }
    // }

    public void ChangeMaterial(Material material, GameObject obj)
    {
        int childCount = obj.transform.childCount;
        if (childCount > 0)
        {
            for (int i = 0; i < childCount; i++)
            {
                ChangeMaterial(material, obj.transform.GetChild(i).gameObject);
            }
        }
        else
        {
            obj.GetComponent<MeshRenderer>().material = material;
            return;
        }
    }
    
}

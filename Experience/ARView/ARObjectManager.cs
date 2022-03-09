using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARObjectManager : MonoBehaviour
{
    private static ARObjectManager instance;
    public static ARObjectManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ARObjectManager>();
            }
            return instance;
        }
    }
    public GameObject CurrentObject { get; set; }

    public void InstantiateObject(Vector3 position, Quaternion rotation)
    {
        CurrentObject = Instantiate(Resources.Load(PathResource.MODEL_PATH) as GameObject, position, rotation);
        CurrentObject.transform.localScale *= ModelConfig.scaleFactorInARMode;
        if (CurrentObject.GetComponent<ARAnchor>() == null)
        {
            CurrentObject.AddComponent<ARAnchor>();
        }
    }
}

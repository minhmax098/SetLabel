using System.Security;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameObjectManager : MonoBehaviour
{ 
    private static GameObjectManager instance;
    public static GameObjectManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameObjectManager>();
            }
            return instance;
        }
    }

    public OrganInfor OriginOrganData { get; set; }
    public Material OriginOrganMaterial { get; set; }

    public GameObject OriginObject { get; set; }
    public GameObject CurrentObject { get; set; }
    // public List<GameObject> ListChildCurentObject { get; set; }

    // save selected object 
    public List<GameObject> ListSelectedObject { get; set; }

    // save origin local position
    public List<List<GameObject>> ListChildOfSelectedObject { get; set; }
    public List<List<Vector3>> ListOriginLocalPositionOfChild {get; set;}
    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;


        // ListChildCurentObject = new List<GameObject>();

        // selected object
        ListSelectedObject = new List<GameObject>();

        // save origin local position
        // ListLocalPositionChildCurentObject = new List<Vector3>();
        ListChildOfSelectedObject = new List<List<GameObject>>();
        ListOriginLocalPositionOfChild = new List<List<Vector3>>();

        LoadDataOrgan();
        InitObject();
        OriginOrganMaterial = CurrentObject.GetComponent<Renderer>().materials[0];
    }

    void Update()
    {

    }

    public void LoadDataOrgan()
    {
        OriginOrganData = new OrganInfor("", "Brain_Model2", "");
    }

    public void InitObject()
    {
        GameObject prefabMainOrgan = Resources.Load(PathResource.PRE_PATH_MODEL + OriginOrganData.Name, typeof(GameObject)) as GameObject;
        OriginObject = Instantiate(prefabMainOrgan, prefabMainOrgan.transform.position, prefabMainOrgan.transform.rotation) as GameObject;
        CurrentObject = OriginObject;
        SetListSelectedObject();
        SetListOriginLocalPositionOfChild();
    }
    public void SetListOriginLocalPositionOfChild()
    {
        List<Vector3> ListOriginLocalPostion = new List<Vector3>();
        List<GameObject> ListChildCurrentObject = new List<GameObject>();
        int childCount = CurrentObject.transform.childCount;
        if (childCount > 0)
        {
            for (int i = 0; i < childCount; i++)
            {
                ListChildCurrentObject.Add(CurrentObject.transform.GetChild(i).gameObject);
                ListOriginLocalPostion.Add(CurrentObject.transform.GetChild(i).transform.localPosition);
            }
        }
        else
        {
            ListChildCurrentObject = null;
            ListChildCurrentObject = null;
        }
        ListChildOfSelectedObject.Add(ListChildCurrentObject);
        ListOriginLocalPositionOfChild.Add(ListOriginLocalPostion);
    }

    public void SetListSelectedObject()
    {
        ListSelectedObject.Add(CurrentObject);
    }

    public void SelectDoubleTouchObject(GameObject newGameObject)
    {
        CurrentObject = newGameObject;
        SetListSelectedObject();
        SetListOriginLocalPositionOfChild();
    }

    // back select in Node Tree
    public void UpdateCurrentObject(int indexElementOfNewObject, GameObject newGameObject)
    {
        CurrentObject = newGameObject;
        Helper.RemoveElementOfListFromIndexToEnd(ListSelectedObject, indexElementOfNewObject);
        Helper.RemoveElementOfListInListObjectFromIndexToEnd(ListChildOfSelectedObject, indexElementOfNewObject);
        Helper.RemoveElementOfListInListVector3FromIndexToEnd(ListOriginLocalPositionOfChild, indexElementOfNewObject);
    }
}

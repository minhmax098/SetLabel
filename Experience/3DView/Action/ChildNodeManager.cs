using System.Linq.Expressions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class ChildNodeManager : MonoBehaviour
{
    Vector3 positionCamera;
    private static ChildNodeManager instance;
    public static ChildNodeManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ChildNodeManager>();
            }
            return instance;
        }
    }

    void Start()
    {
        positionCamera = Camera.main.transform.position;
    }

    int nodeIndex = 0;
    public Transform rootTreeTransform;

    public void CreateChildNodeUI(string name)
    {
        GameObject childNode = Instantiate(Resources.Load(PathResource.MODEL_ITEM_BODY_TREE) as GameObject);
        childNode.transform.SetParent(rootTreeTransform, false);
        childNode.transform.GetChild(1).GetComponent<Text>().text = name;
        nodeIndex++;
        childNode.name = nodeIndex.ToString();
        childNode.transform.GetComponent<Button>().onClick.AddListener(delegate { HandleEventClickNodeTree(childNode);});

        Debug.Log("before list local position " + GameObjectManager.Instance.ListOriginLocalPositionOfChild.Count);
        Debug.Log("before list child object " + GameObjectManager.Instance.ListChildOfSelectedObject.Count);
        Debug.Log("before list select object " + GameObjectManager.Instance.ListSelectedObject.Count);
        Debug.Log("before list end select object " + GameObjectManager.Instance.ListSelectedObject[GameObjectManager.Instance.ListSelectedObject.Count-1]);
        // Debug.Log("before selected object " + selectedObject); 
    }
    public void DisplaySelectedObject(GameObject selectedObject)
    {
        foreach (Transform child in GameObjectManager.Instance.CurrentObject.transform)
        {
            if (child.gameObject != selectedObject)
            {
                child.gameObject.SetActive(false);
            }
        }
        // move camera center
        StartCoroutine(Helper.MoveObject(Camera.main.gameObject, new Vector3(selectedObject.transform.position.x, selectedObject.transform.position.y, -7f)));
    }

    public void RemoveItem(int indexElement)
    {
        for (int i = indexElement; i < rootTreeTransform.transform.childCount; i++)
        {
            Destroy(rootTreeTransform.transform.GetChild(i).gameObject);
        }
    }

    void HandleEventClickNodeTree(GameObject childNode)
    {
        int indexSelectedObject = Convert.ToInt32(childNode.name);
        SelectPreviousNodeTree(indexSelectedObject);
    }

    public void SelectPreviousNodeTree(int indexSelectedObject)
    {
        UIHandler.Instance.TurnOffAudio();
        UIHandler.Instance.isClickedBtnAudio = false;
        UIHandler.Instance.isAudioPlaying = false;
        
        if (indexSelectedObject == nodeIndex)
        {
            return;
        }

        nodeIndex = indexSelectedObject;
        Debug.Log("index " + indexSelectedObject);
        GameObject selectedObject = GameObjectManager.Instance.ListSelectedObject[indexSelectedObject];
        Debug.Log("select: " + GameObjectManager.Instance.ListSelectedObject.Count);


        for (int indexList = indexSelectedObject; indexList < GameObjectManager.Instance.ListSelectedObject.Count; indexList++)
        {
            if (GameObjectManager.Instance.ListChildOfSelectedObject[indexList] != null)
            {
                int childCount = GameObjectManager.Instance.ListChildOfSelectedObject[indexList].Count;
                if (childCount > 0)
                {
                    for (int indexChild = 0; indexChild < childCount; indexChild++)
                    {
                        GameObjectManager.Instance.ListChildOfSelectedObject[indexList][indexChild].transform.localPosition = GameObjectManager.Instance.ListOriginLocalPositionOfChild[indexList][indexChild];
                        GameObjectManager.Instance.ListChildOfSelectedObject[indexList][indexChild].gameObject.SetActive(true);
                    }
                }
            }
        }

        RemoveItem(indexSelectedObject + 1);
        StartCoroutine(Helper.MoveObject(Camera.main.gameObject, positionCamera));

        GameObjectManager.Instance.UpdateCurrentObject(indexSelectedObject + 1, selectedObject);

        Debug.Log("end list local position " + GameObjectManager.Instance.ListOriginLocalPositionOfChild.Count);
        Debug.Log("end list child object " + GameObjectManager.Instance.ListChildOfSelectedObject.Count);
        Debug.Log("end list select object " + GameObjectManager.Instance.ListSelectedObject.Count);
        Debug.Log("end list end select object " + GameObjectManager.Instance.ListSelectedObject[GameObjectManager.Instance.ListSelectedObject.Count-1]);
        Debug.Log("end selected object " + selectedObject);        
    }
}

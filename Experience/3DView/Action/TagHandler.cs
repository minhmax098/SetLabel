using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TagHandler : MonoBehaviour
{
    private static TagHandler instance;
    public static TagHandler Instance
    {
        get {
            if (instance == null)
            {
                instance = FindObjectOfType<TagHandler>();
            }
            return instance;
        }
    }

    public List<GameObject> addedTags = new List<GameObject>();


    void Start()
    {
        // initAtlas();
        // loadTags();
    }
    void Update()
    {

    }
    
    public void AddTag(GameObject tag)
    {
        addedTags.Add(tag);
    }

    public void DeleteTags()
    {
        addedTags.Clear();
    }

    // public void adjustTag(int idx, Tag tag)
    // {
    //     addedTags[idx].transform.GetChild(0).transform.position = tag.point.coordinate;
    //     addedTags[idx].transform.GetChild(0).transform.eulerAngles = tag.point.direction;
    //     addedTags[idx].transform.GetChild(0).gameObject.GetComponent<LineRenderer>().SetVertexCount(2);
    //     addedTags[idx].transform.GetChild(0).gameObject.GetComponent<LineRenderer>().SetPosition(0,tag.point.coordinate);
    //     addedTags[idx].transform.GetChild(0).gameObject.GetComponent<LineRenderer>().SetPosition(1,tag.tag);
    //     addedTags[idx].transform.GetChild(0).gameObject.GetComponent<LineRenderer>().SetWidth(0.01f, 0.01f);

    //     addedTags[idx].transform.GetChild(1).transform.position = tag.tag;

    //     float scaleX = (tag.name.Length/9f);
    //     int scaleY = (int)(tag.name.Length / 18);
    //     if (scaleX > 2)
    //         scaleX = 2;
    //     if (scaleY * 18 < tag.name.Length)
    //         scaleY++;
    //     addedTags[idx].transform.GetChild(1).gameObject.GetComponent<TMPro.TextMeshPro>().SetText(tag.name);
    //     addedTags[idx].transform.GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(  scaleX * addedTags[idx].transform.GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta[0], 
    //                                                                                                             scaleY * addedTags[idx].transform.GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta[1]);
    //     addedTags[idx].transform.GetChild(2).transform.localScale = new Vector3(scaleX * addedTags[idx].transform.GetChild(2).transform.localScale.x, 
    //                                                                             scaleY * addedTags[idx].transform.GetChild(2).transform.localScale.y, 
    //                                                                             addedTags[idx].transform.GetChild(2).transform.localScale.z);
    //     addedTags[idx].transform.GetChild(2).transform.position = tag.tag;
    // }

    public void OnMove()
    {
        foreach(GameObject addedTag in addedTags)
        {
            denoteTag(addedTag);
            moveTag(addedTag);
        }
    }
    
    public void denoteTag(GameObject addedTag)
    {
        if (addedTag.transform.GetChild(1).transform.position.z > 1f)  // (+) is back
        {
            addedTag.transform.GetChild(0).gameObject.GetComponent<LineRenderer>().enabled = false;
            addedTag.transform.GetChild(1).gameObject.GetComponent<MeshRenderer>().enabled = false;
            addedTag.transform.GetChild(2).gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
        else{
            addedTag.transform.GetChild(0).gameObject.GetComponent<LineRenderer>().enabled = true;
            addedTag.transform.GetChild(1).gameObject.GetComponent<MeshRenderer>().enabled = true;
            addedTag.transform.GetChild(2).gameObject.GetComponent<MeshRenderer>().enabled = true;
        }
    }

    

    public void moveTag(GameObject addedTag)
    {
        addedTag.transform.GetChild(1).transform.LookAt(addedTag.transform.GetChild(1).position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
        addedTag.transform.GetChild(2).transform.LookAt(addedTag.transform.GetChild(2).position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
    }
}

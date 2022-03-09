using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro;

public class Label : MonoBehaviour
{
    private static Label instance; 
    public static Label Instance
    {
        get 
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Label>(); 
            }
            return instance; 
        }
    }

    private int childCount;
    private GameObject activeObject;

    private GameObject rootObject;

    private bool isFirstCall = true;
    private float c = 4f;

    private Vector3 objectCenter;

    private List<GameObject> labelObjects = new List<GameObject>(); 

    public void CreateLabel()
    {
        
        activeObject = GameObjectManager.Instance.CurrentObject;
        Debug.Log("ACTIVE OBJECT: " + activeObject);
        childCount = activeObject.transform.childCount;

        rootObject = GameObjectManager.Instance.CurrentObject.transform.GetChild(0).gameObject;
        // posittion 
        objectCenter = CalculateCentroid(activeObject);

        Debug.Log("CENTER POINT: " + objectCenter);
        Debug.Log("PONS CENTER: " + rootObject.transform.position);

        // childCount = GameObjectManager.Instance.CurrentObject.transform.childCount;
        Debug.Log("NUMBER OF CHILD: " + childCount);
        for(int i = 0; i < childCount; i++)
        {
            GameObject childObject = activeObject.transform.GetChild(i).gameObject;
            // GameObject labelObject = Instantiate(Resources.Load("Model/Lessons/LabelManager_1") as GameObject);
            GameObject labelObject = Instantiate(Resources.Load("Model/Lessons/tag") as GameObject);
            TagHandler.Instance.AddTag(labelObject);

            
            labelObject.transform.SetParent(childObject.transform, false);
            labelObject.transform.localPosition = new Vector3(0, 0, 0);
            Debug.Log("TEST GLOBAL POSITION: " + labelObject.transform.position.x);
            // SetLabel(childObject, rootObject, labelObject);

            SetLabel_2(childObject, activeObject, objectCenter, labelObject);
            labelObjects.Add(labelObject); 
        }
    }

    public static Bounds GetRenderBounds(GameObject go)
    {
        var totalBounds = new Bounds();
        totalBounds.SetMinMax(Vector3.one * Mathf.Infinity, -Vector3.one * Mathf.Infinity);
        foreach (var renderer in go.GetComponentsInChildren<Renderer>()){
            var bounds = renderer.bounds;
            var totalMin = totalBounds.min;
            totalMin.x = Mathf.Min(totalMin.x, bounds.min.x);
            totalMin.y = Mathf.Min(totalMin.y, bounds.min.y);
            totalMin.z = Mathf.Min(totalMin.z, bounds.min.z);

            var totalMax = totalBounds.max;
            totalMax.x = Mathf.Max(totalMax.x, bounds.max.x);
            totalMax.y = Mathf.Max(totalMax.y, bounds.max.y);
            totalMax.z = Mathf.Max(totalMax.z, bounds.max.z);

            totalBounds.SetMinMax(totalMin, totalMax);
        }
        return totalBounds;
    }

    private Vector3 CalculateCentroid(GameObject obj){
        Transform[] children;
        Vector3 centroid = new Vector3(0, 0, 0);

        children = obj.GetComponentsInChildren<Transform>(true);

        Debug.Log("LIST ALL CHILDREN !!!");
        foreach (var ob in children)
        {
            if(ob != obj.transform){
                centroid += ob.transform.position;
                Debug.Log("CENTROID UPDATED: " + centroid);
                Debug.Log(ob.name);
            }  
        }
        Debug.Log("CHILDREND LENGTH: " + children.Length);
        centroid /= (children.Length - 1);
        Debug.Log("Centroid: " + centroid.x);
        return centroid;
    }

    public Bounds getParentBound(GameObject parentObject, Vector3 center)
    {
        foreach (Transform child in parentObject.transform){
            center += child.gameObject.GetComponent<Renderer>().bounds.center;
        }

        center /= parentObject.transform.childCount;
        
        Bounds bounds = new Bounds(center, Vector3.zero);
        foreach(Transform child in parentObject.transform){
            bounds.Encapsulate(child.gameObject.GetComponent<Renderer>().bounds);
        }
        return bounds;
    }

    public void SetLabel_2(GameObject currentObject, GameObject parentObject, Vector3 rootPosition, GameObject label){
        GameObject line = label.transform.GetChild(0).gameObject; 
        GameObject labelName = label.transform.GetChild(1).gameObject;
        labelName.GetComponent<TextMeshPro>().text = currentObject.name; 
        // Bounds parentBounds = parentObject.GetComponent<Renderer>().bounds;
        Bounds parentBounds = getParentBound(parentObject, rootPosition);
        Bounds objectBounds = currentObject.GetComponent<Renderer>().bounds;

        Debug.Log("PARENT OBJECT MAXMANIUDE : " + parentBounds.max.magnitude);
        Debug.Log("CURRENT OBJECT MAXMANIUDE : " + objectBounds.max.magnitude);

        Vector3 dir = rootPosition - currentObject.transform.position;
        labelName.transform.localPosition = -c * parentBounds.max.magnitude * label.transform.InverseTransformPoint(dir).normalized;
        Debug.Log("Label name localPosition: " + labelName.transform.localPosition.x);
        Debug.Log("Label name position: " + labelName.transform.position.x);
        line.GetComponent<LineRenderer>().useWorldSpace = false;

        line.GetComponent<LineRenderer>().SetVertexCount(2);
        line.GetComponent<LineRenderer>().SetPosition(0, objectBounds.center);
        line.GetComponent<LineRenderer>().SetPosition(1, labelName.transform.localPosition);
        line.GetComponent<LineRenderer>().SetColors(Color.white, Color.black);
    }


    public void SetLabel(GameObject currentObject, GameObject root, GameObject label)
    {
        GameObject line = label.transform.GetChild(0).gameObject; 
        GameObject labelName = label.transform.GetChild(1).gameObject;
        // GameObject labelPanel = label.transform.GetChild(2).gameObject;

        // labelName.transform.GetChild(0).gameObject.GetComponent<Text>().text = currentObject.name;
        labelName.GetComponent<TextMeshPro>().text = currentObject.name; 
        // Bounds objectBounds = GetRenderBounds(currentObject);
        Bounds objectBounds = currentObject.GetComponent<Renderer>().bounds;

        Vector3 dir = root.transform.position - currentObject.transform.position;

        
        // labelName.transform.localPosition = new Vector3(currentObject.transform.localPosition.x, currentObject.transform.localPosition.y, currentObject.transform.localPosition.z); 
        //labelName.transform.localPosition = new Vector3(objectBounds.size.x, objectBounds.size.y , objectBounds.size.z); 
        // labelName.transform.localPosition = new Vector3(-c * objectBounds.size.x, - c * objectBounds.size.y , c * objectBounds.size.z); 
        labelName.transform.localPosition = -c * objectBounds.size.x * label.transform.InverseTransformPoint(dir).normalized;
        // labelPanel.transform.localPosition = -c * objectBounds.size.y * label.transform.InverseTransformPoint(dir).normalized;

        

        Debug.Log("Label name localPosition: " + labelName.transform.localPosition.x);
        Debug.Log("Label name position: " + labelName.transform.position.x);
        line.GetComponent<LineRenderer>().useWorldSpace = false;
        
        // line.GetComponent<LineRenderer>().SetColors(Color.white, Color.white);
        // Material whiteDiffuseMat = new Material(Shader.Find("Unlit/Texture"));
        // line.GetComponent<LineRenderer>().material = whiteDiffuseMat;

        line.GetComponent<LineRenderer>().SetVertexCount(2);
        line.GetComponent<LineRenderer>().SetPosition(0, objectBounds.center);
        // line.GetComponent<LineRenderer>().SetPosition(0, new Vector3(0, 0, 0));
        line.GetComponent<LineRenderer>().SetPosition(1, labelName.transform.localPosition);
    }

    public void displayLabel()
    {
        Debug.Log("DISPLAY LABELS !!!");
        if (isFirstCall){
            CreateLabel();
            isFirstCall = false;
        }
        else
        {
            foreach(GameObject label in labelObjects)
            {
                label.SetActive(true);
            }
        }
       
    }

    public void hideLabel()
    {
        Debug.Log("HIDE LABELS !!!");
        foreach(GameObject label in labelObjects)
        {
            label.SetActive(false);
        }
    }

}

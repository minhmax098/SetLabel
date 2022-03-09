using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Separate : MonoBehaviour
{
    private static Separate instance;
    public static Separate Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<Separate>();
            return instance;
        }
    }

    private float radius = 8f;  // 0.008f
    private float c = 8f;
    private int childCount;
    private Vector3 centerPosition;
    private float angle;
    private Vector3 targetPosition;

    public void SeparateOrganModel()
    {
        childCount = GameObjectManager.Instance.CurrentObject.transform.childCount;
        centerPosition = GameObjectManager.Instance.CurrentObject.transform.GetChild(0).localPosition; /// position of Pons w.r.t brain model
        Debug.Log("Center POS: " + centerPosition );
        Debug.Log(childCount);
        angle = (float)(360 / (childCount - 1));
        // Move the other object 
        // for (int index = 1; index < childCount; index++)
        // {
        //     targetPosition = RandomCircle(centerPosition, angle * index);
        //     Debug.Log("Target Position: " + targetPosition);
        //     StartCoroutine(MoveObjectWithLocalPosition(GameObjectManager.Instance.CurrentObject.transform.GetChild(index).gameObject, targetPosition));
        // }
        centerPosition = CalculateCentroid(GameObjectManager.Instance.CurrentObject);
        Debug.Log("Center POS RECALULATED !!!: " + centerPosition);

        for (int i =1; i < childCount; i++){
            targetPosition = computeTargetPosition(centerPosition, GameObjectManager.Instance.CurrentObject.transform.GetChild(i).gameObject);
            StartCoroutine(MoveObjectWithLocalPosition(GameObjectManager.Instance.CurrentObject.transform.GetChild(i).gameObject, targetPosition));
        }
    }


    private Vector3 CalculateCentroid(GameObject parentObject){
        Vector3 centroid = new Vector3(0, 0, 0);

        foreach (Transform child in parentObject.transform){
            centroid += child.gameObject.transform.position;
        }
        centroid /= parentObject.transform.childCount;
        Debug.Log("Centroid: " + centroid.x);
        return centroid;
    }

    public Vector3 RandomCircle(Vector3 center, float angle)
    {
        Vector3 pos;
        pos.x = center.x;
        // pos.x = center.x + radius * Mathf.Sin(angle * Mathf.Deg2Rad);
        pos.y = center.y + radius * Mathf.Sin(angle * Mathf.Deg2Rad);
        // pos.y = center.y;
        // pos.z = center.z;
        pos.z = center.z + radius * Mathf.Cos(angle * Mathf.Deg2Rad);
        return pos;
    }

    public Vector3 computeTargetPosition(Vector3 center, GameObject moveObject){
        Vector3 dir  =  moveObject.transform.localPosition - center;
        return dir.normalized * c;
    }

    public IEnumerator MoveObjectWithLocalPosition(GameObject moveObject, Vector3 targetPosition)
    {
        float timeSinceStarted = 0f;
        while (true)
        {
            timeSinceStarted += Time.deltaTime;
            moveObject.transform.localPosition = Vector3.Lerp(moveObject.transform.localPosition, targetPosition, timeSinceStarted);
            if (moveObject.transform.localPosition == targetPosition)
            {
                yield break;
            }
            yield return null;
        }
    }
    public void BackPositionOfChildrenOrgan()
    {
        int childCount = GameObjectManager.Instance.CurrentObject.transform.childCount;
        if (childCount < 0)
        {
            return;
        }
        int indexList = GameObjectManager.Instance.ListSelectedObject.IndexOf(GameObjectManager.Instance.CurrentObject);
        for (int indexChild = 0; indexChild < childCount; indexChild++)
        {
            targetPosition = GameObjectManager.Instance.ListOriginLocalPositionOfChild[indexList][indexChild];
            StartCoroutine(MoveObjectWithLocalPosition(GameObjectManager.Instance.ListChildOfSelectedObject[indexList][indexChild], targetPosition));
            // Main.currentObject.currentListChildOrgan[i].SetActive(true);
        }
        
    }
}

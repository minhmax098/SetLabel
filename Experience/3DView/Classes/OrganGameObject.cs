// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class OrganGameObject
// {
//     public GameObject MainObject { get; set; }
//     public int ChildCount { get; set; }
//     public Material OriginMaterial { get; set; }
//     public List<LocalTransform> originListChildOrganTranform;

//     public Organ(GameObject instance)
//     {
//         MainObject = instance;
//         ChildCount = instance.transform.childCount;
//         originListChildOrganTranform = new List<LocalTransform>();
//         currentListChildOrgan = new List<GameObject>();
//         OriginMaterial = MainObject.GetComponent<Renderer>().materials[0];
//         SetCurrentListChildOrgan();
//         SetOriginListChildOrganTranform();
//         // if (originListChildOrganTranform != null)
//         //     Debug.Log("originList[0]" + originListChildOrganTranform[0]);
//         // if (currentListChildOrgan != null)
//         //     Debug.Log("current[0] " + currentListChildOrgan[0].name + currentListChildOrgan[0].transform.position);
//     }

//     public void SetOriginListChildOrganTranform()
//     {
//         if (ChildCount > 0)
//         {
//             foreach (Transform child in MainObject.transform)
//             {
//                 originListChildOrganTranform.Add(new LocalTransform(
//                     child.localPosition, 
//                     child.localScale,
//                     child.localRotation));
//             }
//         }
//     }

//     public void SetCurrentListChildOrgan()
//     {
//         if (ChildCount > 0)
//         {
//             for (int i = 0; i < ChildCount; i++)
//             {
//                 currentListChildOrgan.Add(MainObject.transform.GetChild(i).gameObject);
//             }
//         }
//     }

//     public List<GameObject> GetCurrentListChildOrgan()
//     {
//         return currentListChildOrgan;
//     }

//     public List<LocalTransform> GetOriginListChildOrganTransform()
//     {
//         return originListChildOrganTranform;
//     }
// }
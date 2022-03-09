using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideShowTag : MonoBehaviour
{
    
   void OnBecameInvisible()
    {
        Debug.Log("inVISIBLE");
        // this.gameObject.transform.GetComponent<MeshRenderer>().enabled = false;
        this.gameObject.transform.parent.transform.GetChild(0).gameObject.GetComponent<LineRenderer>().enabled = false;
        // this.gameObject.transform.parent.transform.GetChild(1).gameObject.GetComponent<MeshRenderer>().enabled = false;
        this.gameObject.transform.parent.transform.GetChild(2).gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

    // ...and enable it again when it becomes visible.
    void OnBecameVisible()
    {
        Debug.Log("VISIBLE");
        // this.gameObject.transform.GetComponent<MeshRenderer>().enabled = true;
        this.gameObject.transform.parent.transform.GetChild(0).gameObject.GetComponent<LineRenderer>().enabled = true;
        // this.gameObject.transform.parent.transform.GetChild(1).gameObject.GetComponent<MeshRenderer>().enabled = true;
        this.gameObject.transform.parent.transform.GetChild(2).gameObject.GetComponent<MeshRenderer>().enabled = true;
    }
}

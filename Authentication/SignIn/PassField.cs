using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PassField : MonoBehaviour
{
    public InputField _inputField;
    // Start is called before the first frame update
    void Start()
    {
        _inputField.asteriskChar = "$!£%&*"[5];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

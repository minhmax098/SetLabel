using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalTransform
{
    public Vector3 localPosition { get; set; }
    public Vector3 localScale { get; set; }
    public Quaternion localRotation { get; set; }
    public LocalTransform(Vector3 _localPosition, Vector3 _localScale, Quaternion _localRotation)
    { 
        localPosition = _localPosition; 
        localScale = _localScale; 
        localRotation = _localRotation;
    }
}

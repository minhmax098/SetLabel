using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ObjectManager
{
    public static GameObject CurrentModelObject { get; set; }
    public static bool IsRotating { get; set; }
    public static bool IsMoving { get; set; }
    public static bool IsSeparate { get; set; }
    public static bool IsXRay { get; set; }
    public static bool IsLabel { get; set; }
    public static bool IsAnimate { get; set; }
    public static bool IsPickup { get; set; }
    public static bool IsInfo2 { get; set; }

    public static Model DataOrgan { get; set; }

    public static void InitModel(
        GameObject _current_model_object,
        bool _is_rotating = false,
        bool _is_moving = false,
        bool _is_separate = false,
        bool _is_xray = false, 
        bool _is_label = false,
        bool _is_animate = false, 
        bool _is_pickup = false, 
        bool _is_info2 = false
        // Model dataOrgan
        ) 
        {
            CurrentModelObject = _current_model_object;
            IsRotating = _is_rotating;
            IsMoving = _is_moving;
            IsSeparate = _is_separate;
            IsXRay = _is_xray;
            IsLabel = _is_label;
            IsAnimate = _is_animate; 
            IsPickup = _is_pickup; 
            IsInfo2 = _is_info2; 
            // DataOrgan = dataOrgan;
        }
    // public static void InitModel(GameObject currentModelObject, bool isRotating, bool isMoving, bool isSeparate, bool isXRay, bool isLabel, bool isAnimate, bool isPickup, bool isInfo2) 
    // {
    //     CurrentModelObject = currentModelObject;
    //     IsRotating = isRotating;
    //     IsMoving = isMoving;
    //     IsSeparate = isSeparate;
    //     IsXRay = isXRay;
    //     IsLabel = isLabel;
    //     IsAnimate = isAnimate;
    //     IsPickup = isPickup;
    //     IsInfo2 = isInfo2;
    // }
}

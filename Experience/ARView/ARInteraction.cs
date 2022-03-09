using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARInteraction : MonoBehaviour
{
    private const int ONE_FINGER_TOUCH = 1;
    private const int TWO_FINGER_TOUCH = 2;
    private Touch singleTouch;
    private Touch firstFingerTouch;
    private Touch secondFingerTouch;
    private float originalDistanceDelta;
    private Vector3 originalLocalScaleOfCurrentObject;
    float currentDistanceDelta;
    private float scaleFactor;
    bool isDraggingWithLongTouch = false;
    private bool isLongTouch = false;
    private float touchDuration = 0.0f;
    private Vector3 originalLocalScaleOfSelectedObject;
    private GameObject currentSelectedObject;
    private Vector3 mOffset;
    private float mZCoord;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (ARObjectManager.Instance.CurrentObject == null || Helper.IsPointerOverUIObject())
        {
            return;
        }
        switch (Input.touchCount)
        {
            case ONE_FINGER_TOUCH:
                singleTouch = Input.GetTouch(0);
                if (singleTouch.tapCount == 1)
                {
                    HandleSingleTouch(singleTouch);
                }
                else if (singleTouch.tapCount == 2)
                {

                }
                break;
            case TWO_FINGER_TOUCH:
                firstFingerTouch = Input.GetTouch(0);
                secondFingerTouch = Input.GetTouch(1);
                ScaleCurrentObject(firstFingerTouch, secondFingerTouch);
                break;
            default:
                break;
        }
        // if (Input.touchCount < 1)
        // {
        //     return;
        // }
        // if (Input.touchCount == 1)
        // {
        //     touch = Input.GetTouch(0);
        //     if (touch.tapCount == 1)  // single touch
        //     {
        //         HandleSingleTouch(touch);
        //     }
        //     else if (touch.tapCount == 2)
        //     {
        //         touch = Input.touches[0];
        //         if (touch.phase == TouchPhase.Ended) // double touch
        //         {
        //             HandleDoupleTouch(touch);
        //         }
        //     }
        // }
        // else if (Input.touchCount == 2) // touch at same time
        // {
        //     touchZero = Input.GetTouch(0);
        //     touchOne = Input.GetTouch(1);
        //     HandleSimultaneousTouch(touchZero, touchOne);
        // }
    }

    private void HandleSingleTouch(Touch singleTouch)
    {
        switch (singleTouch.phase)
        {
            case TouchPhase.Began:
                {
                    currentSelectedObject = Helper.GetOrganObjectByTouchInARMode(singleTouch.position);
                    isDraggingWithLongTouch = currentSelectedObject != null;
                    if (isDraggingWithLongTouch)
                    {
                        Debug.Log($"sonvo selected object {currentSelectedObject.name}");
                        mZCoord = Camera.main.WorldToScreenPoint(currentSelectedObject.transform.position).z;
                        mOffset = currentSelectedObject.transform.position - GetTouchPositionAsWorldPoint(singleTouch);
                    }
                    break;
                }
            case TouchPhase.Stationary:
                {
                    // if (isDraggingWithLongTouch && !isLongTouch && UIHandler.Instance.isClickedBtnHold)
                    if (isDraggingWithLongTouch && !isLongTouch)
                    {
                        touchDuration += Time.deltaTime;
                        if (touchDuration > ModelConfig.longTouchThreshold)
                        {
                            OnLongTouchInvoke();
                        }
                    }
                    break;
                }
            case TouchPhase.Moved:
                {
                    if (isLongTouch)
                    {
                        DragCurrentObject(singleTouch);
                    }
                    else
                    {
                        RotateCurrentObject(singleTouch);
                    }
                    break;
                }
            case TouchPhase.Ended:
                {
                    ResetLongTouch();
                    break;
                }

            case TouchPhase.Canceled:
                {
                    ResetLongTouch();
                    break;
                }
        }
    }

    void ResetLongTouch()
    {
        touchDuration = 0f;
        isLongTouch = false;
        isDraggingWithLongTouch = false;
        currentSelectedObject = null;
    }
    private Vector3 GetTouchPositionAsWorldPoint(Touch touch)
    {
        Vector3 touchPoint = touch.position;
        touchPoint.z = mZCoord;
        return Camera.main.ScreenToWorldPoint(touchPoint);
    }

    private void ScaleCurrentObject(Touch firstFingerTouch, Touch secondFingerTouch)
    {
        if (firstFingerTouch.phase == TouchPhase.Began || secondFingerTouch.phase == TouchPhase.Began)
        {
            originalDistanceDelta = Vector2.Distance(firstFingerTouch.position, secondFingerTouch.position);
            originalLocalScaleOfCurrentObject = ARObjectManager.Instance.CurrentObject.transform.localScale;
        }
        else if (firstFingerTouch.phase == TouchPhase.Moved || secondFingerTouch.phase == TouchPhase.Moved)
        {
            currentDistanceDelta = Vector2.Distance(firstFingerTouch.position, secondFingerTouch.position);
            scaleFactor = currentDistanceDelta / originalDistanceDelta;
            ARObjectManager.Instance.CurrentObject.transform.localScale = originalLocalScaleOfCurrentObject * scaleFactor;
        }
    }

    private void RotateCurrentObject(Touch touch)
    {
        // ARObjectManager.Instance.CurrentObject.transform.Rotate(touch.deltaPosition.y * rotationRate, -touch.deltaPosition.x * ModelConfig.rotationRate, 0, Space.World);
        ARObjectManager.Instance.CurrentObject.transform.rotation *= Quaternion.Euler(new Vector3(0, -touch.deltaPosition.x * ModelConfig.rotationSpeed, 0));
    }
    private void OnLongTouchInvoke()
    {
        StartCoroutine(HightlightSelectedObject());
        isLongTouch = true;
    }
    private IEnumerator HightlightSelectedObject()
    {
        originalLocalScaleOfSelectedObject = currentSelectedObject.transform.localScale;
        currentSelectedObject.transform.localScale = originalLocalScaleOfSelectedObject * ModelConfig.scaleFactorForHightlightingSelectedObject;
        yield return new WaitForSeconds(ModelConfig.durationForHightlightingSelectedObject);
        currentSelectedObject.transform.localScale = originalLocalScaleOfSelectedObject;
    }
    private void DragCurrentObject(Touch touch)
    {
        if (currentSelectedObject != null)
        {
            currentSelectedObject.transform.position = GetTouchPositionAsWorldPoint(touch) + mOffset;
        }
    }
}

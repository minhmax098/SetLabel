using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interaction : MonoBehaviour
{
    private float rotationRate = 0.08f;
    Touch touch;
    Touch touchZero;
    Touch touchOne;
    float originDelta;
    Vector3 originScale;
    float currentDelta;
    float scaleFactor;
    Vector2 originPosition;
    float touchDuration = 0.0f;
    const float LONG_TOUCH_THRESHOLD = 1f;
    bool isMovingByLongTouch = false;
    bool isLongTouch = false;
    Vector3 originScaleSelected;
    GameObject currentSelectedObject;
    private Vector3 mOffset;
    private float mZCoord;

    void Start()
    {

    }

    void Update()
    {

        if (Input.touchCount < 1)
        {
            return;
        }
        // if (Helper.IsPointerOverUIObject())
        // {
        //     return;
        // }
        if (UIAdded.CheckEnaleOneOfUI())
        {
            return;
        }

        if (Input.touchCount == 1)
        {
            touch = Input.GetTouch(0);
            if (touch.tapCount == 1)                 // single touch
            {
                HandleSingleTouch(touch);
            }
            else if (touch.tapCount == 2)
            {
                touch = Input.touches[0];
                if (touch.phase == TouchPhase.Ended) // double touch same location
                {
                    HandleDoupleTouch(touch);
                }
            }
        }
        else if (Input.touches.Length == 2)          // touch at same time
        {
            touchZero = Input.GetTouch(0);
            touchOne = Input.GetTouch(1);
            HandleSimultaneousTouch(touchZero, touchOne);
        }
    }


    private void HandleSingleTouch(Touch touch)
    {
        switch (touch.phase)
        {
            case TouchPhase.Began:
                {
                    currentSelectedObject = Helper.GetChildOrganOnTouchByTag(touch.position);
                    isMovingByLongTouch = currentSelectedObject != null;
                    if (currentSelectedObject != null)
                    {
                        mZCoord = Camera.main.WorldToScreenPoint(currentSelectedObject.transform.position).z;
                        mOffset = currentSelectedObject.transform.position - GetTouchPositionAsWorldPoint(touch);
                    }
                    break;
                }

            case TouchPhase.Stationary:
                {
                    if (isMovingByLongTouch && !isLongTouch && UIHandler.Instance.isClickedBtnHold)
                    {
                        touchDuration += Time.deltaTime;
                        if (touchDuration > LONG_TOUCH_THRESHOLD)
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
                        Drag(touch);
                    }
                    else
                    {
                        Rotate(touch);
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
        isMovingByLongTouch = false;
        currentSelectedObject = null;
    }

    void OnLongTouchInvoke()
    {
        StartCoroutine(HightLightObject());
        isLongTouch = true;
    }

    IEnumerator HightLightObject()
    {
        originScaleSelected = currentSelectedObject.transform.localScale;
        currentSelectedObject.transform.localScale = originScaleSelected * 1.5f;
        yield return new WaitForSeconds(0.12f);
        currentSelectedObject.transform.localScale = originScaleSelected;
    }

    private void Rotate(Touch touch)
    {
        // GameObjectManager.Instance.CurrentObject.transform.Rotate(touch.deltaPosition.y * rotationRate, -touch.deltaPosition.x * rotationRate, 0, Space.World);
        GameObjectManager.Instance.OriginObject.transform.Rotate(touch.deltaPosition.y * rotationRate, -touch.deltaPosition.x * rotationRate, 0, Space.World);

    }

    private Vector3 GetTouchPositionAsWorldPoint(Touch touch)
    {
        Vector3 touchPoint = touch.position;
        touchPoint.z = mZCoord;
        return Camera.main.ScreenToWorldPoint(touchPoint);
    }

    private void Drag(Touch touch)
    {
        if (currentSelectedObject != null)
        {
            currentSelectedObject.transform.position = GetTouchPositionAsWorldPoint(touch) + mOffset;
        }
    }

    private void HandleSimultaneousTouch(Touch touchZero, Touch touchOne)
    {
        if (touchZero.phase == TouchPhase.Began || touchOne.phase == TouchPhase.Began)
        {
            originDelta = Vector2.Distance(touchZero.position, touchOne.position);
            originScale = GameObjectManager.Instance.OriginObject.transform.localScale;
        }
        else if (touchZero.phase == TouchPhase.Moved || touchOne.phase == TouchPhase.Moved)
        {
            currentDelta = Vector2.Distance(touchZero.position, touchOne.position);
            scaleFactor = currentDelta / originDelta;
            GameObjectManager.Instance.OriginObject.transform.localScale = originScale * scaleFactor;
        }
    }

    private void HandleDoupleTouch(Touch touch)
    {
        UIHandler.Instance.isClickedBtnHold = false;

        UIHandler.Instance.TurnOffAudio();
        UIHandler.Instance.isClickedBtnAudio = false;
        UIHandler.Instance.isAudioPlaying = false;

        UIHandler.Instance.isClickedBtnSeparate = false;
        // Separate.Instance.BackPositionOfChildrenOrgan();

        GameObject selectedObject = Helper.GetChildOrganOnTouchByTag(touch.position);

        if (selectedObject == null || selectedObject == GameObjectManager.Instance.CurrentObject || GameObjectManager.Instance.CurrentObject.transform.childCount <= 0)
        {
            return;
        }

        // Display object selected
        ChildNodeManager.Instance.DisplaySelectedObject(selectedObject);

        // Change current object
        GameObjectManager.Instance.SelectDoubleTouchObject(selectedObject);

        // Append child node UI in tree
        ChildNodeManager.Instance.CreateChildNodeUI(selectedObject.name);

    }
}

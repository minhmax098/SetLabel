using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum UIState
{
    Starting,
    Detecting,
    Controlling
}
public class ARUIManager : MonoBehaviour
{
    public GameObject actionControlPanel;
    public GameObject introPanel;
    public GameObject introText;
    public GameObject guideText;
    public GameObject arPointer;
    public GameObject btnExit;
    // public UIState State = { get; Set;} = UIState.Starting;

    // Start is called before the first frame update
    void Start()
    {
        Screen.orientation = ScreenOrientation.Landscape;
        AllowInteractingObject(false);
        AllowPlacingObject(false);
        InitEvent();
    }

    void Update()
    {
        if (Input.touchCount == 1)
        {
            if (IsPlacingObject() && (!Helper.IsPointerOverUIObject()))
            {
                if (Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    PlaceObject();
                }
            }
        }
    }
    void InitEvent()
    {
        ARPointerManager.onActivePointer += OnActivePointer;
        ARPointerManager.onInactivePointer += OnInactivePointer;

        btnExit.GetComponent<Button>().onClick.AddListener(HandlerBtnExit);
    }
    void AllowInteractingObject(bool isReadyToControl)
    {
        actionControlPanel.SetActive(isReadyToControl);
        introPanel.SetActive(!isReadyToControl);
    }
    void AllowPlacingObject(bool isReadyToPlaceObject)
    {
        guideText.SetActive(isReadyToPlaceObject);
        introText.SetActive(!isReadyToPlaceObject);
    }
    void OnInactivePointer()
    {
        AllowPlacingObject(false);
        arPointer.SetActive(false);
    }

    void OnActivePointer(Pose pose)
    {
        AllowPlacingObject(true);
        arPointer.transform.position = pose.position;
        arPointer.transform.rotation = pose.rotation;
        if (!arPointer.activeInHierarchy)
        {
            arPointer.SetActive(true);
        }
    }
    void PlaceObject()
    {
        AllowInteractingObject(true);
        arPointer.SetActive(false);
        ARObjectManager.Instance.InstantiateObject(arPointer.transform.position, arPointer.transform.rotation);
    }

    bool IsPlacingObject()
    {
        return guideText.activeInHierarchy;
    }

    void HandlerBtnExit()
    {
        SceneManager.LoadScene("3DView");
    }
}

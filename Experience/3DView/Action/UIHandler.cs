using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;


public class UIHandler : MonoBehaviour
{
    private static UIHandler instance;
    public static UIHandler Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<UIHandler>();
            return instance;
        }
    }
    public GameObject btnLabel;
    public GameObject btnHold;
    public GameObject btnXRay;
    public GameObject btnSeparate;
    public GameObject btnAudio;
    public GameObject btnAR;
    public GameObject btnExit;
    public GameObject btnGuide;
    public GameObject btnMenu;
    public GameObject btnControlAudio;
    public GameObject btnExitAudio;
    public GameObject guideBoard;
    public GameObject btnExitGuideBoard;
    // list audio
    public GameObject listAudioBoard;
    public GameObject txtNameOrgan;
    // pop up exit lesson
    public GameObject popUpExit;
    public GameObject btnContinue;
    public GameObject btnExitLesson;
    public GameObject btnExitPopup;
    public GameObject panelAudio;
    public GameObject itemStartTree;
    public Material transparentMaterial;
    public bool isClickedBtnHold = false;
    public bool isClickedBtnXray = false;
    public bool isClickedBtnAudio = false;
    public bool isAudioPlaying = false;
    public bool isClickedBtnSeparate = false;
    public bool isClickedBtnMenu = false;

    public bool isClickedBtnLabel = false;
    public float timeCurrentPause = 0f;


    private GameObject activeObject;

    void Start()
    {
        InitEvents();
        DisplayPanel();    
        UIAdded.AddUI();
    }

    void OnMove(){
        Debug.Log("MOVING");
    }

    void Update()
    {
        DisplayUI();
        
        if (TagHandler.Instance.addedTags.Count > 0)
        {
            TagHandler.Instance.OnMove();
        }
    }

    void DisplayUI()
    {
        if (GameObjectManager.Instance.CurrentObject != null)
        {
            // Enable button feature
            if (GameObjectManager.Instance.CurrentObject.GetComponent<AudioSource>() == null)
            {
                btnAudio.GetComponent<Button>().interactable = false;
            }
            else
            {
                btnAudio.GetComponent<Button>().interactable = true;
            }

            // Enable button feature
            if (GameObjectManager.Instance.CurrentObject.transform.childCount <= 0)
            {
                btnSeparate.GetComponent<Button>().interactable = false;
                btnHold.GetComponent<Button>().interactable = false;
            }
            else
            {
                btnSeparate.GetComponent<Button>().interactable = true;
                btnHold.GetComponent<Button>().interactable = true;

            }
            // Display infor organ
            itemStartTree.transform.GetChild(1).gameObject.GetComponent<Text>().text = GameObjectManager.Instance.OriginOrganData.Name;
        }

        // button Separate
        if (isClickedBtnSeparate)
        {
            btnSeparate.GetComponent<Image>().sprite = Resources.Load<Sprite>(PathResource.SEPARATE_CLICKED_IMAGE);
        }
        else
        {
            btnSeparate.GetComponent<Image>().sprite = Resources.Load<Sprite>(PathResource.SEPARATE_UNCLICK_IMAGE);
        }

        // button Hold
        if (isClickedBtnHold)
        {
            btnHold.GetComponent<Image>().sprite = Resources.Load<Sprite>(PathResource.HOLD_CLICKED_IMAGE);
        }
        else
        {
            btnHold.GetComponent<Image>().sprite = Resources.Load<Sprite>(PathResource.HOLD_UNCLICK_IMAGE);

        }

        // button XRay
        if (isClickedBtnXray)
        {
            btnXRay.GetComponent<Image>().sprite = Resources.Load<Sprite>(PathResource.XRAY_CLICKED_IMAGE);
        }
        else
        {
            btnXRay.GetComponent<Image>().sprite = Resources.Load<Sprite>(PathResource.XRAY_UNCLICK_IMAGE);
        }

        // button Menu list audio
        if (isClickedBtnMenu)
        {
            btnMenu.GetComponent<Image>().sprite = Resources.Load<Sprite>(PathResource.MENU_CLICKED_IMAGE);
        }
        else
        {
            btnMenu.GetComponent<Image>().sprite = Resources.Load<Sprite>(PathResource.MENU_UNCLICK_IMAGE);
        }

        // button Label 
        if (isClickedBtnLabel)
        {
            Debug.Log("BUTTON LABEL IS CLICKED !!!");
            btnLabel.GetComponent<Image>().sprite = Resources.Load<Sprite>(PathResource.LABEL_CLICKED_IMAGE);
        }
        else
        {
            Debug.Log("BTn LABEL NOT CLICKED !!!");
            btnLabel.GetComponent<Image>().sprite = Resources.Load<Sprite>(PathResource.LABEL_UNCLICK_IMAGE);
            // TagHandler.Instance.DeleteTags();
        }
        

    }

    void DisplayPanel()
    {
        panelAudio.SetActive(false);

    }

    void DisplayPanelAudio(bool isClickedBtnAudio)
    {
        panelAudio.SetActive(isClickedBtnAudio);
        btnAudio.SetActive(!isClickedBtnAudio);
    }

    void InitEvents()
    {
        btnHold.GetComponent<Button>().onClick.AddListener(HandlerBtnHold);
        btnXRay.GetComponent<Button>().onClick.AddListener(HandlerBtnXray);
        btnAR.GetComponent<Button>().onClick.AddListener(HandlerBtnAR);
        btnSeparate.GetComponent<Button>().onClick.AddListener(HandlerBtnSeparate);
        btnAudio.GetComponent<Button>().onClick.AddListener(HandlerBtnAudio);
        btnGuide.GetComponent<Button>().onClick.AddListener(HandlerBtnGuide);
        btnExit.GetComponent<Button>().onClick.AddListener(HandlerBtnExit);
        btnMenu.GetComponent<Button>().onClick.AddListener(HandlerBtnMenu);
        itemStartTree.GetComponent<Button>().onClick.AddListener(HandlerBtnHomeTree);

        btnLabel.GetComponent<Button>().onClick.AddListener(HandlerBtnLabel);

        btnControlAudio.GetComponent<Button>().onClick.AddListener(ControlAudio);
        btnExitAudio.GetComponent<Button>().onClick.AddListener(TurnOffAudio); 

        btnExitLesson.GetComponent<Button>().onClick.AddListener(ExitLesson);
        btnContinue.GetComponent<Button>().onClick.AddListener(ContinueLesson);
        btnExitPopup.GetComponent<Button>().onClick.AddListener(ContinueLesson);  
    }

    void HandlerBtnHold()
    {
        if (isClickedBtnHold)
        {
            isClickedBtnHold = false;
        }
        else
        {
            isClickedBtnHold = true;  
        }
    }

    void HandlerBtnXray()
    {
        if (isClickedBtnXray)
        {
            XRay.Instance.ChangeMaterial(GameObjectManager.Instance.OriginOrganMaterial, GameObjectManager.Instance.OriginObject);

            isClickedBtnXray = false;
        }
        else
        {
            XRay.Instance.ChangeMaterial(transparentMaterial, GameObjectManager.Instance.OriginObject);

            isClickedBtnXray = true;
        }
    }

    void HandlerBtnAR()
    {
        SceneManager.LoadScene("ARView");
    }

    void HandlerBtnExit()
    {
        popUpExit.SetActive(true);
    }

    void ExitLesson()
    {
        if (PlayerPrefs.GetString("user_email") != ""){
            SceneManager.LoadScene(SceneConfig.lesson); 
        }
        else SceneManager.LoadScene(SceneConfig.lesson_nosignin); 
        // SceneManager.LoadScene(SceneName.LESSON_DESCRIPTION);
    }

    void ContinueLesson()
    {
        popUpExit.SetActive(false);
    }
    void HandlerBtnSeparate()
    {
        if (isClickedBtnSeparate)
        {
            Separate.Instance.BackPositionOfChildrenOrgan();

            isClickedBtnSeparate = false;
        }
        else
        {
            Separate.Instance.SeparateOrganModel();

            isClickedBtnSeparate = true;   
        }
    }

    public void HandlerBtnAudio()
    {
        if (isClickedBtnAudio)
        {
            Audio.Instance.ActionAudio(GameObjectManager.Instance.CurrentObject, Audio.ActionAudioControl.Stop);
            isAudioPlaying = false;
            isClickedBtnAudio = false;
        }
        else
        {
            Audio.Instance.ActionAudio(GameObjectManager.Instance.CurrentObject, Audio.ActionAudioControl.Start);
            isAudioPlaying = true;
            isClickedBtnAudio = true;
                
        }
        DisplayPanelAudio(isClickedBtnAudio);
    }

    void ControlAudio()
    {
        if (isAudioPlaying)
        {
            Audio.Instance.ActionAudio(GameObjectManager.Instance.CurrentObject, Audio.ActionAudioControl.Pause);
            btnControlAudio.GetComponent<Image>().sprite = Resources.Load<Sprite>(PathResource.AUDIO_PLAY_IMAGE);
            isAudioPlaying = false;
        }
        else
        {
            Audio.Instance.ActionAudio(GameObjectManager.Instance.CurrentObject, Audio.ActionAudioControl.Unpause);
            btnControlAudio.GetComponent<Image>().sprite = Resources.Load<Sprite>(PathResource.AUDIO_PAUSE_IMAGE);
            isAudioPlaying = true;
        }
    }

    public void TurnOffAudio()
    {
        Audio.Instance.ActionAudio(GameObjectManager.Instance.CurrentObject, Audio.ActionAudioControl.Stop);
        isAudioPlaying = false;
        isClickedBtnAudio = false;
        btnControlAudio.GetComponent<Image>().sprite = Resources.Load<Sprite>(PathResource.AUDIO_PAUSE_IMAGE);
        DisplayPanelAudio(isClickedBtnAudio);
    }

    void HandlerBtnHomeTree()
    {
        int indexSelectedObject = 0;
        ChildNodeManager.Instance.SelectPreviousNodeTree(indexSelectedObject);
    }

    void HandlerBtnGuide()
    {
        guideBoard.SetActive(true);
        btnExitGuideBoard.GetComponent<Button>().onClick.AddListener(HiddenGuideBoard);
    }

    void HiddenGuideBoard()
    {
        guideBoard.SetActive(false);
    }

    void HandlerBtnMenu()
    {
        if (isClickedBtnMenu)
        {
            listAudioBoard.SetActive(false);
            isClickedBtnMenu = false;
        }
        else
        {
            listAudioBoard.SetActive(true);
            if (Audio.Instance.contentListAudio.transform.childCount <= 0)
            {
                
                Audio.Instance.GetAllAudioOfOrgan(GameObjectManager.Instance.OriginObject);
                txtNameOrgan.GetComponent<Text>().text = GameObjectManager.Instance.OriginOrganData.Name;
            }
            
            isClickedBtnMenu = true;
        }
        
    }


    void HandlerBtnLabel(){
        if(isClickedBtnLabel)
        {
            Label.Instance.hideLabel();
            isClickedBtnLabel = false;
        }
        else
        {
            Label.Instance.displayLabel();
            isClickedBtnLabel = true;
        }
    }
}

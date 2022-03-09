using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LeftPanelHandler : MonoBehaviour
{
    [SerializeField]
    Animator statusAnimator;
    private GameObject meetingManager;
    private GameObject signOutBtn;
    void Start()
    {
        InitUI();
        // InitEvent();
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    void InitUI() 
    {
        meetingManager = GameObject.Find("MeetingManager");
        
        signOutBtn = GameObject.Find("BtnSignOut");
        if (signOutBtn != null) signOutBtn.GetComponent<Button>().onClick.AddListener(SignOut);

    }

    public void SignOut(){
        Debug.Log("NACRIEMA");
        PlayerPrefs.SetString("user_name", "");
        PlayerPrefs.SetString("user_email", "");
        SceneManager.LoadScene(SceneConfig.home_nosignin);
    }
    
    // void InitEvent() 
    // {
    //     meetingManager.GetComponent<Button>().onClick.AddListener(LoadMeetingJoiningRoom);
    // }

    // void LoadMeetingJoiningRoom() 
    // {
    //     SceneManager.LoadScene(SceneConfig.meetingJoin);
    // }

    public void ShowLeftPanel() 
    {
        statusAnimator.SetBool(AnimatorConfig.showLeftPanel, true);
    }

    public void HideLeftPanel() 
    {
        statusAnimator.SetBool(AnimatorConfig.showLeftPanel, false);
    }
}

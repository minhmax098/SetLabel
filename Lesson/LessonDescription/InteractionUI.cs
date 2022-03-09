using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
namespace LessonDescription
{
public class InteractionUI : MonoBehaviour
{
    // public GameObject waitingScreen;
    private GameObject startLessonBtn; 
    private GameObject startMeetingBtn; 
    private GameObject backToHomeBtn;
    private static InteractionUI instance; 
    public static InteractionUI Instance
    {
        get 
        {
            if(instance == null)
            {
                instance = FindObjectOfType<InteractionUI>(); 
            }
            return instance;
        }
    }

    public void onClickItemLesson (int lessonId)
    {
        LessonManager.InitLesson(lessonId);

        SceneNameManager.setPrevScene(SceneConfig.lesson_nosignin);
        if (PlayerPrefs.GetString("user_email") != ""){
            SceneManager.LoadScene(SceneConfig.lesson); 
        }
        else SceneManager.LoadScene(SceneConfig.lesson_nosignin); 
    }
    void Start()
    {
        // waitingScreen.SetActive(false);
        InitUI(); 
        SetActions(); 
    }

    void Update()
    {
        // if (startLessonBtn.GetComponent<Button>() == true)
        // {
        //     Debug.Log("start Lesson: "); 
        //     PlayerPrefs.SetString("user_input", ""); 

        //     StartCoroutine(LoadAsynchronously(SceneConfig.start3Dview)); 
        //     // if (PlayerPrefs.GetString("user_email") != "")
        //     // {
        //     //     SceneManager.LoadScene(SceneConfig.); 
        //     // }
        //     // else SceneManager.LoadScene(SceneConfig.lesson_nosignin); 
        // }
    }

    // public IEnumerator LoadAsynchronously(string sceneName)
    // {
    //     AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
    //     waitingScreen.SetActive(true);
    //     while(!operation.isDone)
    //     {
    //         yield return new WaitForSeconds(.2f);
    //     }
    // }
    void InitUI()
    {
        backToHomeBtn = GameObject.Find("BtnMenu"); 
        startLessonBtn = GameObject.Find("StartLessonBtn");
    }
    void SetActions()
    {
        backToHomeBtn.GetComponent<Button>().onClick.AddListener(BackToRenalSystem); 
        startLessonBtn.GetComponent<Button>().onClick.AddListener(Start3DView); 
    }
    void BackToRenalSystem()
    {
        SceneManager.LoadScene(SceneNameManager.prevScene);
        // SceneManager.LoadScene(SceneConfig.listOrgan); 
    }
    void Start3DView()
    {
        SceneManager.LoadScene(SceneConfig.start3Dview); 
    }
}
}


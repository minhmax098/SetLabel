using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems; 

namespace ListOrgan 
{
    public class InteractionUI : MonoBehaviour
    {
        private GameObject backToHomeBtn; 
        private string emailCheck;
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

            SceneNameManager.setPrevScene(SceneConfig.listOrgan);
            if (PlayerPrefs.GetString("user_email") != ""){
                SceneManager.LoadScene(SceneConfig.lesson); 
            }
            else SceneManager.LoadScene(SceneConfig.lesson_nosignin); 
        }

        void Start()
        {
            InitUI(); 
            SetActions(); 
        }
        void InitUI()
        {
            backToHomeBtn = GameObject.Find("BackBtn"); 
        }
        void SetActions()
        {
            backToHomeBtn.GetComponent<Button>().onClick.AddListener(BackToHome); 

        }
        void BackToHome()
        {
            emailCheck = PlayerPrefs.GetString("user_email");
            if (emailCheck == "")
            {
                SceneManager.LoadScene(SceneConfig.home_nosignin); 
            }
            else SceneManager.LoadScene(SceneConfig.home_user); 
            
        } 
    }
}

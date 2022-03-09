using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems; 

public class InteractionUISignIn : MonoBehaviour
{
    private GameObject backToHomeBtn; 
    private GameObject nextForgotPassBtn; 
    private GameObject nextSignUpBtn; 
    void Start()
    {
        InitUI(); 
        SetActions();
    }

    void Update()
    {
        
    }
    void InitUI()
    {
        backToHomeBtn = GameObject.Find("BackBtn"); 
        nextForgotPassBtn = GameObject.Find("ForgotPass"); 
        nextSignUpBtn = GameObject.Find("SignUp"); 

    }
    void SetActions()
    {
        backToHomeBtn.GetComponent<Button>().onClick.AddListener(BackToHomeNoSignIn); 
        nextForgotPassBtn.GetComponent<Button>().onClick.AddListener(NextToForgotPass); 
        nextSignUpBtn.GetComponent<Button>().onClick.AddListener(NextToSignUp); 
    }

    void BackToHomeNoSignIn()
    {
        SceneManager.LoadScene(SceneConfig.home_nosignin); 
    }
    void NextToForgotPass()
    {
        SceneManager.LoadScene(SceneConfig.forgotPass); 
    }
    void NextToSignUp()
    {
        SceneManager.LoadScene(SceneConfig.signUp); 
    }
}

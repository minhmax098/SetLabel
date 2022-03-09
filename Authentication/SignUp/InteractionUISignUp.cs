using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 

public class InteractionUISignUp : MonoBehaviour
{
    public GameObject switchToHomeNoSignIn;
    public GameObject switchToSignInBtn; 

    // Start is called before the first frame update
    void Start()
    {
        InitUI(); 
        SetActions(); 
    }
    void InitUI()
    {
        switchToHomeNoSignIn = GameObject.Find("BackBtn"); 
        switchToSignInBtn = GameObject.Find("SignIn"); 
    }   
    void SetActions()
    {
        switchToHomeNoSignIn.GetComponent<Button>().onClick.AddListener(SwitchToHomeNoSignIn); 
        switchToSignInBtn.GetComponent<Button>().onClick.AddListener(SwitchToSignIn); 
    }
    void SwitchToHomeNoSignIn()
    {
        SceneManager.LoadScene(SceneConfig.home_nosignin); 
    }
    void SwitchToSignIn()
    {
        SceneManager.LoadScene(SceneConfig.signIn); 
    }
}

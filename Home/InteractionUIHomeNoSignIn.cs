using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 

public class InteractionUIHomeNoSignIn : MonoBehaviour
{
    public GameObject nextToSignInBtn; 
    public GameObject nextToSignUpBtn; 
    // Start is called before the first frame update
    void Start()
    {
        InitUI(); 
        SetActions(); 
    }

    void InitUI()
    {
        nextToSignInBtn = GameObject.Find("Footer"); 
        nextToSignUpBtn = GameObject.Find("BtnSignUp"); 

    }
    void SetActions()
    {
        nextToSignInBtn.GetComponent<Button>().onClick.AddListener(NextToSignIn); 
        nextToSignUpBtn.GetComponent<Button>().onClick.AddListener(NextToSignUp); 
    }

    void NextToSignIn()
    {
        SceneManager.LoadScene(SceneConfig.signIn); 
    }
    void NextToSignUp()
    {
        SceneManager.LoadScene(SceneConfig.signUp); 
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 

public class InteractionUIPassReset : MonoBehaviour
{
    public GameObject nexttoSignInBtn; 
    public GameObject backtoResetPassBtn; 
    // Start is called before the first frame update
    void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait; 
        InitUI(); 
        SetActions(); 
    }

    // Update is called once per frame
    void InitUI()
    {
        nexttoSignInBtn = GameObject.Find("SignIn"); 
        backtoResetPassBtn = GameObject.Find("BtnBackResetPass"); 
    }
    void SetActions()
    {
        nexttoSignInBtn.GetComponent<Button>().onClick.AddListener(NextToSignIn); 
        backtoResetPassBtn.GetComponent<Button>().onClick.AddListener(BackToResetPass); 
    }
    void NextToSignIn()
    {
        SceneManager.LoadScene(SceneConfig.signIn); 
    }
    void BackToResetPass()
    {
        SceneManager.LoadScene(SceneConfig.resetpass); 
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
public class SplashScreen : MonoBehaviour
{
    void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait; 
        StartCoroutine(toMainMenu());
    }

    IEnumerator toMainMenu(){
        yield return new WaitForSeconds(0);
        if (PlayerPrefs.GetString("user_email") != ""){
            SceneManager.LoadScene(SceneConfig.home_user);
        }
        else SceneManager.LoadScene(SceneConfig.home_nosignin);
    }
}

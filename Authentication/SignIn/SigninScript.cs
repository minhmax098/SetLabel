using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement; 
using UnityEngine.Networking; 
using System.Text;

public class SigninScript : MonoBehaviour
{
    public GameObject loadingScreen; 
    public GameObject waitingScreen;
    public Slider slider; 
    public Button signInBtn; 
    public InputField userNameInput; 
    public InputField passwordInput; 
    public GameObject EmailWarning; 
    // public InputField Email; 
    public GameObject PassWarning; 
    public GameObject capcha; 
    public GameObject CapchaWarning; 
    public GameObject incorrectCapcha;
    private int invalidCount; 
    public GameObject UserPassWarning;

    public GameObject notificationBox; 

    private PopupSystem pop; 
    void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait; 
        signInBtn = transform.GetComponent<Button>();
        signInBtn.onClick.AddListener(ValidateSignin);

        EmailWarning.SetActive(false); 
        PassWarning.SetActive(false); 
        capcha.SetActive(false);  

        loadingScreen.SetActive(false); 
        waitingScreen.SetActive(false); 

        passwordInput.contentType = InputField.ContentType.Password;
        passwordInput.onValueChanged.AddListener(checkPassValid);

        userNameInput.keyboardType = TouchScreenKeyboardType.EmailAddress;
        userNameInput.onValueChanged.AddListener(checkUserNameValid);

        CapchaWarning.SetActive(false); 
        incorrectCapcha.SetActive(false); 
        // Warning; username or password is incorrect
        UserPassWarning.SetActive(false);
        notificationBox.SetActive(false); 
        pop = notificationBox.GetComponent<PopupSystem>();
    }
    


    void checkUserNameValid(string data)
    {
        if(data != "") EmailWarning.SetActive(false);
    }

    void checkPassValid(string data)
    {
        if(data != "") PassWarning.SetActive(false); 
    }

    private void ValidateSignin()
    {
        string email = userNameInput.text; 
        string pass = passwordInput.text; 
        bool check = false;

        if (email == "")
        {
            EmailWarning.SetActive(true);
            check = true;
            // Email.selectionColor = Color.red; 
            // return;
        }
        if (pass == "")
        {
            PassWarning.SetActive(true); 
            check = true;
            // return; 
        }
        // if (email != "")
        // {
        //     EmailWarning.SetActive(false);
        // }
        // if (pass != "")
        // {
        //     PassWarning.SetActive(false);
        // }

        if (invalidCount >= 5){
            string capchaString = capcha.transform.GetChild(0).GetComponent<InputField>().text;
            Debug.Log("TEST CAPCHA: ");
            Debug.Log(capchaString);
            if (capchaString == ""){
                incorrectCapcha.SetActive(false);
                CapchaWarning.SetActive(true);
                check = true;
            }
            if (capchaString != ""){
                CapchaWarning.SetActive(false);
                if (!check){
                    Debug.Log("TEST CAPCHA COMPARISION: ");
                    Debug.Log(capchaString);
                    Debug.Log(PlayerPrefs.GetString("capcha"));
                     
                    if (capchaString.Equals(PlayerPrefs.GetString("capcha"))){                    
                        incorrectCapcha.SetActive(false);
                        StartCoroutine(CallSignin(email, pass));
                    }
                    else{
                        Debug.Log(" CAPCHA !!");
                        incorrectCapcha.SetActive(true);
                        GenerateCapcha.genCapchaCode(6);
                        CapchaWarning.SetActive(false);
                    }
                }
            }

        }

        if (!check && invalidCount < 5) {
            StartCoroutine(CallSignin(email, pass));
        }
    }


    // public IEnumerator LoadAsynchronously(string sceneName)
    // {
    //     AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        
    //     loadingScreen.SetActive(true);
    //     while(!operation.isDone)
    //     {
    //         float progress = Mathf.Clamp01(operation.progress / .9f);
    //         Debug.Log(progress); 
    //         slider.value = progress; 
    //         yield return null; 
    //     }
    // }

    public IEnumerator LoadAsynchronously(string sceneName)
    {
        // float time = 0f;
        // float duration = 0.5f;

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        waitingScreen.SetActive(true);

        while(!operation.isDone)
        {
            // slider.value = time - duration * (int)Mathf.Floor(time/duration);
            // time += Time.deltaTime;
            yield return new WaitForSeconds(.2f);
        }
    }

    public IEnumerator WaitForAPIResponse(UnityWebRequest request)
    {
        // float time = 0f;
        // float duration = 0.5f;

        // loadingScreen.GetComponent<Image>().color = new Color32(0,0,0,183);
        waitingScreen.SetActive(true);
        Debug.Log("CALLING API: ");
        while (!request.isDone)
        {

            // slider.value = time - duration * (int)Mathf.Floor(time/duration);
            // time += Time.deltaTime;
            yield return new WaitForSeconds(.2f);
        }
    }

    // Signin API ~Task in C#
    public IEnumerator CallSignin(string Email, string Password)
    {
        string logindataJsonString = "{\"email\": \"" + Email + "\", \"password\": \"" + Password + "\"}";
        Debug.Log(logindataJsonString);

        var request = new UnityWebRequest("https://api.xrcommunity.org/v1/xap/user/login", "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(logindataJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        
        StartCoroutine(WaitForAPIResponse(request));
        yield return request.SendWebRequest();
        waitingScreen.SetActive(false); 

        if (request.error != null)
        {
            Debug.Log("Error: " + request.error);
            if (request.responseCode == 400)
            {
                // no found user, show message 
                // UserPassWarning.SetActive(true); 
                pop.PopUp("Username or password incorrect");
                invalidCount += 1; 
                Debug.Log(invalidCount);
                if (invalidCount == 5)
                {
                    capcha.SetActive(true); 
                }
            }
        }
        else
        {   
            Debug.Log("All OK");
            Debug.Log("Status Code: " + request.responseCode);
            string response = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);
            Debug.Log("LOGIN RESPOSE: " + response);
            LoginData userDetail = JsonUtility.FromJson<LoginData>(response);

            if( request.responseCode == 200)
            {
                PlayerPrefs.SetString("user_email", userDetail.data[0].user.email); 
                PlayerPrefs.SetString("user_name", userDetail.data[0].user.fullName); 
                PlayerPrefs.SetString("user_token", userDetail.data[0].token);

                // string token = PlayerPrefs.GetString("user_token");

                StartCoroutine(LoadAsynchronously(SceneConfig.home_user));
                // SceneManager.LoadScene(SceneConfig.home_user);
            }
            

        }
      
        

        // // Create a form, in which you send a request, similar to Json 
        // WWWForm form = new WWWForm();
        // form.AddField("email", Email); 
        // form.AddField("password", Password);
        // Debug.Log("email" + Email); 
        // Debug.Log("password" + Password); 
        // // Call api through UnityWebRequest, call post method
        // UnityWebRequest www = UnityWebRequest.Post("https://api.xrcommunity.org/xap/user/login", form); 
        // // request.contentType = "application/json"; 
        // // get back out response
        // yield return www.SendWebRequest();

        // if(www.error != null)
        // {
        //     Debug.Log("Error" + www.error); 
        // }
        // else 
        // {
        //     // Success 
        //     Debug.Log("Response"+ www.downloadHandler.text);
        //     // Get User info from response, email, name store in PlayerPrefs 
        //     UserDetail userDetail = JsonUtility.FromJson<UserDetail>(www.downloadHandler.text); 

        //     // check server messages  
        //     if(userDetail.code == 200)
        //     {
        //         // Get user info , store !!!!
        //     print("Name: " + userDetail.user.fullName);
        //     print("Email: " + userDetail.user.email); 
        //     print("Name: " + userDetail.token);
        //     print("Code: " + userDetail.code);

        //     PlayerPrefs.SetString("user_email", userDetail.user.email); 
        //     PlayerPrefs.SetString("user_name", userDetail.user.fullName); 
        //     PlayerPrefs.SetString("user_token", userDetail.token);

        //     string token = PlayerPrefs.GetString("user_token");

        //     SceneManager.LoadScene(SceneConfig.home_user); 
        //     }
        //     // Do cai SCHEME
        //     // else if (userDetail.code == 400)
        //     // {
        //     //     // no user found, show message
        //     //     UserPassWarning.SetActive(true); 
        //     //     invalidCount += 1; 
        //     //     if (invalidCount == 5)
        //     //     {
        //     //         capcha.SetActive(true); 
        //     //     }
        //     // }
        //     else if (userDetail.code == 400)
        //     {
        //         // no user found, show message
        //         UserPassWarning.SetActive(true); 
        //         invalidCount += 1; 
        //         if (invalidCount == 5)
        //         {
        //             capcha.SetActive(true); 
        //         }
        //     }
        // }
    }

}

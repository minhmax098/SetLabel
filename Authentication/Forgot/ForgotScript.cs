using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement; 
using UnityEngine.Networking; 
using System.Text;
using System.Text.RegularExpressions;

public class ForgotScript : MonoBehaviour
{
    public GameObject waitingScreen; 
    public Slider slider; 
    public Button nextBtn; 
    public InputField emailInput; 
    public GameObject EmailWarning; 
    public GameObject validateEmail; 

    public GameObject notificationBox; 

    private PopupSystem pop; 
    private Regex emailRgx = new Regex(@"^(([^<>()[\]\\.,;:\s@']+(\.[^<>()[\]\\.,;:\s@']+)*)|('.+'))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$");

    void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait; 
        nextBtn = transform.GetComponent<Button>(); 
        nextBtn.onClick.AddListener(ValidateForgotPass); 

        EmailWarning.SetActive(false); 
        validateEmail.SetActive(false); 
        notificationBox.SetActive(false);

        waitingScreen.SetActive(false);

        emailInput.keyboardType = TouchScreenKeyboardType.EmailAddress;
        emailInput.onValueChanged.AddListener(CheckEmailFormat);
        pop = notificationBox.GetComponent<PopupSystem>();
    }

    private void ValidateForgotPass()
    {
        string email = emailInput.text; 
        if (email == "")
        {
            EmailWarning.SetActive(true); 
            emailInput.selectionColor = Color.red; 
            return; 
        }
        if (email != "")
        {
            EmailWarning.SetActive(false); 
        }
        StartCoroutine(CallForgotPass(email)); 
    }

    private void CheckEmailFormat(string data){
        if (data == ""){
            EmailWarning.SetActive(false);
            validateEmail.SetActive(false);
        }
        else if (!emailRgx.IsMatch(data)){
            EmailWarning.SetActive(false);
            validateEmail.SetActive(true);
            nextBtn.interactable = false;
        }
        else {
            EmailWarning.SetActive(false);
            validateEmail.SetActive(false);
            nextBtn.interactable = true;
        }

    }

    public IEnumerator LoadAsynchronously (string sceneName)
   {
      AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
      waitingScreen.SetActive(true);
      while (!operation.isDone)
      {
         yield return new WaitForSeconds(.2f); 
      }
   }
   public IEnumerator WaitForAPIResponse(UnityWebRequest request)
   {
      waitingScreen.SetActive(true); 
      Debug.Log("calling API: "); 
      while(!request.isDone)
      {
         // slider.value = time - duration * (int)Mathf.Floor(time/duration);
         // time += Time.deltaTime; 
         yield return new WaitForSeconds(.2f);
      }
   }
    // Forgot pass API 
    public IEnumerator CallForgotPass(string Email)
    {
        string forgotpassdataJsonString = "{\"email\": \"" + Email + "\" }";
        Debug.Log(forgotpassdataJsonString); 

        var request = new UnityWebRequest("https://api.xrcommunity.org/v1/xap/user/forgotPassword", "POST"); 
        byte[] bodyRaw = Encoding.UTF8.GetBytes(forgotpassdataJsonString); 
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw); 
        request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer(); 
        request.SetRequestHeader("Content-Type", "application/json"); 

        StartCoroutine(WaitForAPIResponse(request));
        yield return request.SendWebRequest(); 
        waitingScreen.SetActive(false); 
        
        if (request.error != null)
        {
            string response = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);
            UserDetail userDetail = JsonUtility.FromJson<UserDetail>(response);
            Debug.Log("Error: " + request.error);
            if (request.responseCode == 400)
            {
                // gmail not found 
                // Debug.Log("Gmail not found"); 
                pop.PopUp(userDetail.message);
            }
            else if (request.responseCode == 500){
                // Debug.Log("Server error !!");
                pop.PopUp(userDetail.message); 
            }
        }
        else
        {   
            // gmail ok fine
            Debug.Log("All OK");
            Debug.Log("Status Code: " + request.responseCode);
            string response = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);
            UserDetail userDetail = JsonUtility.FromJson<UserDetail>(response);

            if( userDetail.code == 200)
            {
                PlayerPrefs.SetString("user_email_for_reset", Email);
                Debug.Log("SECRECT CODE: "); 
                Debug.Log(userDetail.secret);
                // string token = PlayerPrefs.GetString("user_token");
                SceneManager.LoadScene(SceneConfig.resetpass);
            }
        }
    }
}   

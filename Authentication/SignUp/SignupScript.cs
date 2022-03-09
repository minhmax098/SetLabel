using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking; 
using System.Text;
using System.Text.RegularExpressions;

public class SignupScript : MonoBehaviour
{
   public GameObject waitingScreen; 
   public Slider slider; 
   public Button signUpBtn;
   public InputField emailInput; 
   public InputField fullNameInput;
   public InputField passwordInput;
   public InputField confirmPasswordInput;

   public GameObject EmailWarning; 
   public GameObject validateEmail; 
   public GameObject FullnameWarning; 
   public GameObject PasswordWarning; 
   public GameObject PasswordWarning2; 
   public GameObject ConfirmPasswordWarning;
   public GameObject ConfirmPasswordWarning2;

   // public GameObject SignUpWarning; 
   public GameObject notificationBox; 

   private PopupSystem pop; 
   private Regex emailRgx = new Regex(@"^(([^<>()[\]\\.,;:\s@']+(\.[^<>()[\]\\.,;:\s@']+)*)|('.+'))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$");

   private Regex passRgx = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,20}$");
   
   void Start()
   {
      Screen.orientation = ScreenOrientation.Portrait; 
      signUpBtn = transform.GetComponent<Button>();
      signUpBtn.onClick.AddListener(ValidateSignup);

      EmailWarning.SetActive(false); 
      FullnameWarning.SetActive(false); 
      PasswordWarning.SetActive(false); 

      ConfirmPasswordWarning.SetActive(false);  
      PasswordWarning2.SetActive(false); 
      
      ConfirmPasswordWarning2.SetActive(false);  
      validateEmail.SetActive(false);

      waitingScreen.SetActive(false);

      emailInput.keyboardType = TouchScreenKeyboardType.EmailAddress;
      
      emailInput.onValueChanged.AddListener(CheckEmailFormat);
      // emailInput.onValueChanged.AddListener(CheckEmailValid);
      fullNameInput.onValueChanged.AddListener(CheckFullnameFormat);

      notificationBox.SetActive(false); 
      pop = notificationBox.GetComponent<PopupSystem>();

      passwordInput.onValueChanged.AddListener(CheckPasswordStrength);
      passwordInput.contentType = InputField.ContentType.Password;
      passwordInput.characterLimit = 20;
      // passwordInput.onValueChanged.AddListener(CheckPassValid); 
      
      confirmPasswordInput.onValueChanged.AddListener(CheckPasswordMatch);
      confirmPasswordInput.contentType = InputField.ContentType.Password; 
      confirmPasswordInput.characterLimit = 20; 
      // confirmPasswordInput.onValueChanged.AddListener(CheckConfirmPassValid);
   } 
   
   void CheckEmailValid(string data)
   {
      if (data != "") validateEmail.SetActive(false);
   }
   void CheckPassValid(string data)
   {
      if (data != "") 
      {
         PasswordWarning2.SetActive(false); 
      }
      else
      {
      PasswordWarning.SetActive(true);
      }
   }
   void CheckConfirmPassValid(string data)
   {
      if(data != "") ConfirmPasswordWarning2.SetActive(false);
   }

   private void CheckFullnameFormat(string data)
   {
      if (data == ""){
            FullnameWarning.SetActive(true);
            
      }
      else {
            FullnameWarning.SetActive(false);
      }
   }
   
   private void CheckPasswordMatch(string data){
        Debug.Log(passwordInput.text);
        if (data == ""){
            ConfirmPasswordWarning.SetActive(false);
            ConfirmPasswordWarning2.SetActive(false);
        }
        else if (!data.Equals(passwordInput.text)){
            ConfirmPasswordWarning2.SetActive(true);
            ConfirmPasswordWarning.SetActive(false);            
        }
        else {
            ConfirmPasswordWarning2.SetActive(false);
            ConfirmPasswordWarning.SetActive(false);
        }
    }
   
   private void CheckPasswordStrength(string data){
        if (data == ""){
            PasswordWarning.SetActive(false);
            PasswordWarning2.SetActive(false);
            // confirmPasswordInput.enabled = false;
        }
        else if (!passRgx.IsMatch(data)){
            PasswordWarning.SetActive(false);
            PasswordWarning2.SetActive(true);
            // confirmPasswordInput.enabled = false;
        }
        else {
            PasswordWarning.SetActive(false);
            PasswordWarning2.SetActive(false);
            // confirmPasswordInput.enabled = true;
        }
   }
   private void ValidateSignup()
   {
      string email = emailInput.text; 
      string fullName = fullNameInput.text; 
      string password = passwordInput.text; 
      string confirmPassword = confirmPasswordInput.text; 

      bool check = false; 

      if (email == "")
      {
         validateEmail.SetActive(false);
         EmailWarning.SetActive(true);
         check = true; 
         // emailInput.selectionColor = Color.red; 
         // return;
      }
      if (fullName == "")
      {
         FullnameWarning.SetActive(true);
         check = true;  
         // fullnameInput.selectionColor = Color.red; 
         // return;
      }
      if (password == "")
      {
         // Sua dung 1 dong ni thoi a
         // Do mi ko chiu do hieu code ta, them tum lum thu !!!!
         // May cai kia mi comment lai ta ko chiu trach nhiem dau, ta doc ko hieu
         PasswordWarning2.SetActive(false);
         PasswordWarning.SetActive(true);
         check = true; 
         // return;
      }
      if(confirmPassword == "")
      {
         ConfirmPasswordWarning2.SetActive(false);
         ConfirmPasswordWarning.SetActive(true);
         check = true; 
         // return;
      }
      
      if (email != "")
      {
         EmailWarning.SetActive(false);
      }
      if (fullName != "")
      {
         FullnameWarning.SetActive(false);
      }
      if (password != "")
      {
         PasswordWarning.SetActive(false);
      }
      if (confirmPassword != "")
      {
         ConfirmPasswordWarning.SetActive(false);
      }

      if (!check)
      {
         StartCoroutine(CallSignup(email, fullName, password, confirmPassword));
      }
   }

   private void CheckEmailFormat(string data){
        if (data == ""){
            EmailWarning.SetActive(false);
            validateEmail.SetActive(false);
        }
        else if (!emailRgx.IsMatch(data)){
            EmailWarning.SetActive(false);
            validateEmail.SetActive(true);
        }
        else {
            EmailWarning.SetActive(false);
            validateEmail.SetActive(false);
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

   // Sign up API
   public IEnumerator CallSignup(string Email, string Fullname, string Password, string ConfirmPassword)
   {
        string signupdataJsonString = "{\"email\": \"" + Email + "\", \"fullName\": \"" + Fullname + "\",\"password\": \"" + Password + "\",\"confirmPassword\": \"" + ConfirmPassword + "\" }";
        Debug.Log(signupdataJsonString);

        var request = new UnityWebRequest("https://api.xrcommunity.org/v1/xap/user/signup", "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(signupdataJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        StartCoroutine(WaitForAPIResponse(request));
        yield return request.SendWebRequest(); 
        waitingScreen.SetActive(false); 

        if (request.error != null)
        {
            string response = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);
            UserDetail userDetail = JsonUtility.FromJson<UserDetail>(response);
            // != 200 vo day het 
            Debug.Log("Error: " + request.error);
            if (request.responseCode == 400)
            {
               pop.PopUp(userDetail.message);
               // Debug.Log("Email format incorrect"); 
            }
        }
        else
        {
           // code 200
            Debug.Log("All OK");
            Debug.Log("Status Code: " + request.responseCode);
            string response = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data); 
            
            UserDetail userDetail = JsonUtility.FromJson<UserDetail>(response); 

            // PlayerPrefs.SetString("user_email", userDetail.user.email);
            // PlayerPrefs.SetString("user_fullName", userDetail.user.fullName); 
            // PlayerPrefs.SetString("user_password", userDetail.user.password); 
            // PlayerPrefs.SetString("user_confirmPassword", userDetail.user.confirmPassword); 


            SceneManager.LoadScene(SceneConfig.signIn); 
        }
      
   }

}
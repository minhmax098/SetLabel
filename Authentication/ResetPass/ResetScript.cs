using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using UnityEngine.UI; 
using UnityEngine.SceneManagement; 
using UnityEngine.Networking; 
using System.Text; 
using System.Text.RegularExpressions;

public class ResetScript : MonoBehaviour
{
    public GameObject waitingScreen;
    public Slider slider; 
    public Button resetPassBtn; 
    public InputField secretcodeInput; 
    public GameObject secretCodeWarning; 
    public InputField newpassInput; 
    public GameObject newPassWarning; 
    public GameObject newPassWarning2; 
    public InputField confirmpassInput; 
    public GameObject confirmPassWarning; 
    public GameObject confirmPassWarning2; 

    public GameObject resetPassWarning; 
    public GameObject notificationBox; 
    private PopupSystem pop; 

    private string email;
    private Regex rgx;

    void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait; 
        resetPassBtn = transform.GetComponent<Button>();
        resetPassBtn.onClick.AddListener(ValidateResetPass); 

        // set show info warning. 
        secretCodeWarning.SetActive(false); 
        newPassWarning.SetActive(false); 
        newPassWarning2.SetActive(false);
        confirmPassWarning.SetActive(false);
        confirmPassWarning2.SetActive(false);

        waitingScreen.SetActive(false);

        notificationBox.SetActive(false);
        pop = notificationBox.GetComponent<PopupSystem>();

        // secretcodeInput.characterValidation = InputField.CharacterValidation.Integer;
        // secretcodeInput.characterLimit = 6;
        secretcodeInput.keyboardType = TouchScreenKeyboardType.NumberPad;

        resetPassWarning.SetActive(false);
        email = PlayerPrefs.GetString("user_email_for_reset");
        Debug.Log("TEST EMAIL IN PREVIOUS SCENE");
        Debug.Log(email);

        // confirmpassInput.enabled = false;

        rgx = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,20}$");
        newpassInput.onValueChanged.AddListener(CheckPasswordStrength);
        newpassInput.contentType = InputField.ContentType.Password;
        newpassInput.characterLimit = 20;
        
        secretcodeInput.onValueChanged.AddListener(CheckSecretCodeInput);
        newpassInput.onValueChanged.AddListener(CheckNewPassValid);

        // passwordField.ForceLabelUpdate();
        confirmpassInput.onValueChanged.AddListener(CheckPasswordMatch);
        confirmpassInput.contentType = InputField.ContentType.Password;
        confirmpassInput.characterLimit = 20;

        confirmpassInput.onValueChanged.AddListener(CheckConfirmPassValid);

    }
    void CheckSecretCodeInput(string data)
    {
        if (data != "") secretCodeWarning.SetActive(false);
    }

    void CheckNewPassValid(string data)
    {
        if (data != "") newPassWarning.SetActive(false);
    }

    void CheckConfirmPassValid(string data)
    {
        if(data != "") confirmPassWarning.SetActive(false);
    }

    public void CheckPasswordMatch(string data){
        Debug.Log(newpassInput.text);
        if (data == ""){
            confirmPassWarning.SetActive(false);
            confirmPassWarning2.SetActive(false);
        }
        else if (!data.Equals(newpassInput.text)){
            confirmPassWarning2.SetActive(true);
            confirmPassWarning.SetActive(false);            
        }
        else {
            confirmPassWarning2.SetActive(false);
            confirmPassWarning.SetActive(false);
        }
    }

    public void CheckPasswordStrength(string data){
        if (data == ""){
            newPassWarning.SetActive(false);
            newPassWarning2.SetActive(false);
            // confirmpassInput.enabled = false;
        }
        else if (!rgx.IsMatch(data)){
            newPassWarning.SetActive(false);
            newPassWarning2.SetActive(true);
            // confirmpassInput.enabled = false;
        }
        else {
            newPassWarning.SetActive(false);
            newPassWarning2.SetActive(false);
            // confirmpassInput.enabled = true;
        }
    }

    private void ValidateResetPass()
    {
        string secretcode = secretcodeInput.text; 
        string newpass = newpassInput.text; 
        string confirmpass = confirmpassInput.text;

        bool check = false;  
        if (secretcode == "")
        {
            secretCodeWarning.SetActive(true); 
            check = true; 
        }
         if (newpass == "")
        {
            newPassWarning.SetActive(true); 
            newPassWarning2.SetActive(false); 
            check = true; 
        }
         if (confirmpass == "")
        {
            confirmPassWarning2.SetActive(false); 
            confirmPassWarning.SetActive(true); 
            check = true; 
        }

        if (secretcode != "")
        {
            secretCodeWarning.SetActive(false); 
            
        }
         if (newpass != "")
        {
            newPassWarning.SetActive(false); 
            
        }
         if (confirmpass != "")
        {
            confirmPassWarning.SetActive(false); 
        }
        
        if (!check && !newPassWarning2.activeSelf && !confirmPassWarning2.activeSelf)
        {
            StartCoroutine(CallResetPass(secretcode, newpass, confirmpass)); 
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

    public IEnumerator CallResetPass(string Secretcode, string Newpass, string Confirmpass)
    {
        string logindataJsonString = "{\"email\": \"" + email + "\", \"secretCode\": \"" + Secretcode + "\", \"newPassword\": \"" + Newpass + "\",\"confirmPassword\": \"" + Confirmpass + "\"}";
        Debug.Log(logindataJsonString);

        var request = new UnityWebRequest("https://api.xrcommunity.org/v1/xap/user/resetPassword", "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(logindataJsonString);
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
            Debug.Log(response);
            Debug.Log(userDetail.message);
            // != 200 vo day het
            Debug.Log("Error: " + request.error);
            if (request.responseCode == 400){
                // Debug.Log("The account is not correct, please check again!");
                pop.PopUp(userDetail.message);
            }

        }
        else
        {   
            // code 200 
            Debug.Log("All OK");
            Debug.Log("Status Code: " + request.responseCode);
            string response = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);
            UserDetail userDetail = JsonUtility.FromJson<UserDetail>(response);

            if(userDetail.code == 200){
                SceneManager.LoadScene(SceneConfig.passresetdone);
            }
            // {
            // PlayerPrefs.SetString("user_email", userDetail.user.email); 
            // PlayerPrefs.SetString("user_name", userDetail.user.fullName); 
            // PlayerPrefs.SetString("user_token", userDetail.token);
        }
    }
}

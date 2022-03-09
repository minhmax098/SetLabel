using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using System; 
using System.Text.RegularExpressions;
using UnityEngine.EventSystems; 
using UnityEngine.Networking;
using System.Threading.Tasks;


namespace ListOrgan {
public class LoadScene : MonoBehaviour, IEndDragHandler
{
    public GameObject contentForOrgansListLessons; 
    public GameObject searchBox; 
    public GameObject organTitle; 
    public Lesson[] organLesson; 
    // add 3 record
    public GameObject sumLesson; 
    public GameObject xBtn; 
    public GameObject searchBtn;

    private char[] charsToTrim = { '*', '.', ' '};

    public ScrollRect content;

    // Variable control content 
    private int offset = 0;  // This is curent page value (offset = 0.. pagesize-1)

    private int limit = 8;  // this is pageSize in API, control number of maximum item in each page , this is CONSTANT, UI dev decided !!!

    // Total page this is also a critical variable, will use it to conrol our API call this is equivalent to totalPage in API 
    private int totalPage;

    // public AllOrgans listLessons;

    async void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait; 
        searchBtn.SetActive(true);
        xBtn.SetActive(false); 
        xBtn.transform.GetComponent<Button>().onClick.AddListener(ClearInput); 

        Debug.Log("User clicked Organ Id: " + OrganManager.organId); 
        
        // add them to the list
        // searchBox.Select();
        // searchBox.ActivateInputField();
        // userInput = PlayerPrefs.GetString("user_input");
        // searchBox.text = userInput;

        AllOrgans listLessons;
        listLessons = await LoadData.Instance.GetListLessonsByOrgan(OrganManager.organId, "", offset, limit);
        totalPage = listLessons.meta.totalPage;

        // organLesson = listLessons.data;
        organTitle.gameObject.GetComponent<Text>().text = OrganManager.organName;

        loadLessons(listLessons.data, listLessons.meta.totalElements, true); 
        
        
        searchBox.GetComponent<InputField>().onValueChanged.AddListener(UpdateList);
    }

    public async void OnEndDrag(PointerEventData eventData)
    {
        AllOrgans listLessons;
        Debug.Log("Stopped draging: " + this.name);

        // if (content.verticalNormalizedPosition >= 0.95f) {
        //     Debug.Log("Current page: " + offset + "Total page; " + totalPage);
        //     if (offset > 0)
        //     {
        //         Debug.Log("Go to previous page");
        //         offset -= 1;

        //         organLesson = LoadData.Instance.GetListLessonsByOrgan(OrganManager.organId, "", offset, limit).data;
        //         loadLessons(organLesson);
        //     }
        // }
        if (content.verticalNormalizedPosition <= 0.05f){
            // Update current page 
            Debug.Log("Current page: " + offset + "Total page; " + totalPage);

            if (offset < totalPage - 1){
                Debug.Log("Go to next page");
                offset += 1;

                // Get current text inside Search box then pass ...
                listLessons = await LoadData.Instance.GetListLessonsByOrgan(OrganManager.organId, "", offset, limit);
                loadLessons(listLessons.data, listLessons.meta.totalElements, false);
            }     
        }
    }

    // void LateUpdate()
    // {
    //     searchBox.MoveTextEnd(true);
    // }

    async void UpdateList(string data)
    {
        Debug.Log("Update string !!!");
        string processedString = Regex.Replace(data, @"\s+", " ").ToLower().Trim(charsToTrim);

        Debug.Log(processedString);
        // processedString = data.Trim(charsToTrim).ToLower();
        if (processedString == "")
        {
            xBtn.SetActive(false); 
            searchBtn.SetActive(true);
        }
        else
        {
            xBtn.SetActive(true);
            searchBtn.SetActive(false); 
        }

        StopAllCoroutines();
        Debug.Log("CLEAR ALL COROUTINE !!!");

        offset = 0;
        // organLesson = LoadData.Instance.GetListLessonsByOrgan(OrganManager.organId, processedString, offset, limit).data;
        AllOrgans listLessons = await LoadData.Instance.GetListLessonsByOrgan(OrganManager.organId, processedString, offset, limit); 

        // Update totalPage too when user change the search input 
        // when change input, we must renew lessons
        // totalPage = organLesson.meta.totalPage;
        // loadLessons(organLesson.data, organLesson.meta.totalElements, true); 
        totalPage = listLessons.meta.totalPage;
        loadLessons(listLessons.data, listLessons.meta.totalElements, true);
    }

    async void ClearInput()
    {
        AllOrgans listLessons;
        searchBox.GetComponent<InputField>().SetTextWithoutNotify("");
        xBtn.SetActive(false); 
        searchBtn.SetActive(true); 

        offset = 0;
        listLessons = await LoadData.Instance.GetListLessonsByOrgan(OrganManager.organId, "", offset, limit);
        // organLesson = LoadData.Instance.GetListLessonsByOrgan(OrganManager.organId, "", offset, limit).data;
        loadLessons(listLessons.data, listLessons.meta.totalElements, true); 
    }

    void loadLessons(Lesson[] lessons, int totalLessons, bool isRenewLesssons)
    {
        if (isRenewLesssons)
        {
            foreach(Transform child in contentForOrgansListLessons.transform)
            {
                GameObject.Destroy(child.gameObject); 
            }
        }
        
        if (totalLessons == 0)
        {
            sumLesson.transform.GetChild(0).gameObject.GetComponent<Text>().text = $"No results found.";
        }
        else
        {
            sumLesson.transform.GetChild(0).gameObject.GetComponent<Text>().text = $"{totalLessons} lessons";
        }
        // // count the lessons
        // int count = 0; 
        foreach (Lesson lesson in lessons) 
        {
            // GameObject organLesson = Instantiate(Resources.Load(ItemConfig.totalLesson2) as GameObject, transform); 
            string imageUrl = String.Format("https://api.xrcommunity.org/v1/xap/stores{0}", lesson.lessonThumbnail);
            // UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUri);
            // yield return request.SendWebRequest();
            // while (!request.isDone)
            // {
            //     yield return null;
            // }
            // Van de no nam o day, cai IEnumerator, ko phai cai Task 
            // Day roi, co nguoi thac mac y

            // GetTexture for each image 
            GetTexture(imageUrl, (string error) => {
                Debug.Log("Error: " + error);
            }, (Texture2D texture) => {
                // Texture2D texture = ((DownloadHandlerTexture) request.downloadHandler).texture;
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(texture.width / 2, texture.height / 2));
                
                GameObject organLesson = Instantiate(Resources.Load(ItemConfig.totalLesson2) as GameObject, contentForOrgansListLessons.transform); 
                organLesson.name = lesson.lessonId.ToString(); 
                organLesson.transform.GetChild(1).gameObject.GetComponent<Text>().text = lesson.lessonTitle;
                organLesson.transform.GetChild(2).GetChild(0).gameObject.GetComponent<Text>().text = lesson.viewed.ToString(); 
                organLesson.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = sprite;
                organLesson.transform.GetChild(3).gameObject.GetComponent<Button>().image.color = lesson.isFavorate != 0 ? Color.red : Color.black;
                organLesson.transform.GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(() => InteractionUI.Instance.onClickItemLesson(lesson.lessonId));
            });
        }       
    }

    // Get Texture for each URL 
    void GetTexture(string url, Action<string> onError, Action<Texture2D> onSuccess){
        StartCoroutine(GetTextureCoroutine(url, onError, onSuccess));
    }

    IEnumerator GetTextureCoroutine(string url, Action<string> onError, Action<Texture2D> onSuccess){
        using (UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(url)){
            yield return unityWebRequest.SendWebRequest();

            if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError){
                onError(unityWebRequest.error);
            } else {
                DownloadHandlerTexture downloadHandlerTexture = unityWebRequest.downloadHandler as DownloadHandlerTexture;
                Debug.Log("DownloadHandlerTexture: " + downloadHandlerTexture);
                onSuccess(downloadHandlerTexture.texture);
            }
        }
    }
}
}

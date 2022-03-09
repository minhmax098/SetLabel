using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using System; 
using System.Text.RegularExpressions;
using UnityEngine.EventSystems; 

namespace XRLibrary
{
    public class LoadScene : MonoBehaviour, IEndDragHandler
    {
        public GameObject contentForOrgansListLessons; 
        public InputField searchBox; 
        public Lesson[] xrLibraryLesson; 

        // insert into 3 records
        public GameObject sumLesson; 
        public GameObject xBtn; 
        public GameObject searchBtn; 
        private string processedString; 
        private string userInput;
        private char[] charsToTrim = { '*', '.', ' '};

        public ScrollRect SR;

        private int offset = 0;
        private int limit = 20;
        private int totalPage;

        public AllXRLibrary listLessons;
        // private UnityWebRequest request;

        void Start()
        {
            Screen.orientation = ScreenOrientation.Portrait; 
            searchBtn.SetActive(true); 
            xBtn.SetActive(false);
            xBtn.transform.GetComponent<Button>().onClick.AddListener(ClearInput); 

            searchBox.Select();
            searchBox.ActivateInputField();
            userInput = PlayerPrefs.GetString("user_input");
            searchBox.text = userInput;


            searchBox.onValueChanged.AddListener(UpdateList); 
            listLessons = LoadData.Instance.GetListLessons(userInput, offset, limit); 
            totalPage = listLessons.meta.totalPage;
            
            Debug.Log("Length of listlesson: ");
            Debug.Log(listLessons.data.Length);
            
            loadLessons(listLessons.data, listLessons.meta.totalElements, true); 

            // yield return new WaitForEndOfFrame();
            // searchBox.MoveTextEnd(true);
            // searchBox.caretPosition = searchBox.text.Length;

        }

        public void OnEndDrag(PointerEventData eventData)
        {
            // if (SR.verticalNormalizedPosition >= 0.95f) {
            //     Debug.Log("Current page: " + offset + "Total page: " + totalPage);
            //     if (offset > 0)
            //     {
            //         Debug.Log("Go to previous page");
            //         offset -= 1;

            //         listLessons = LoadData.Instance.GetListLessons(searchBox.text, offset, limit); 

            //         // We not use this later
            //         loadLessons(listLessons.data, listLessons.meta.totalElements, false);
            //     }
            // }
            if (SR.verticalNormalizedPosition <= 0.05f){
                // Update current page 
                Debug.Log("Current page: " + offset + "Total page: " + totalPage);

                if (offset < totalPage - 1){
                    Debug.Log("Go to next page");
                    offset += 1;

                    // Get current text inside Search box then pass ...

                    listLessons = LoadData.Instance.GetListLessons(searchBox.text, offset, limit); 
                    loadLessons(listLessons.data, listLessons.meta.totalElements, false);
                }     
            }
        }

        void LateUpdate(){
            searchBox.MoveTextEnd(true);
        }

        void UpdateList(string data)
        {
            processedString = Regex.Replace(data, @"\s+", " ").ToLower().Trim(charsToTrim); 
            // processedString = data.Trim(charsToTrim).ToLower(); 
            if (processedString == "")
            {
                // When input is empty
                Debug.Log("Chuoi tim kiem rong");
                xBtn.SetActive(false); 
                searchBtn.SetActive(true); 
            }
            else 
            {
                // When input is not empty
                xBtn.SetActive(true); 
                searchBtn.SetActive(false); 
            }
            
            offset = 0;
            listLessons = LoadData.Instance.GetListLessons(processedString, offset, limit); 
            
            // Update totalPage too when user change the search input
            // When change input, we MUST RENEW lessons 
            totalPage = listLessons.meta.totalPage;
            loadLessons(listLessons.data, listLessons.meta.totalElements, true);
        }
        
        void ClearInput()
        {
            searchBox.GetComponent<InputField>().SetTextWithoutNotify(""); 
            xBtn.SetActive(false); 
            searchBtn.SetActive(true); 
            
            offset = 0; 
            listLessons = LoadData.Instance.GetListLessons("", offset, limit);
            // xrLibraryLesson = LoadData.Instance.GetListLessons("", offset, limit).data;

            // When clear input, we need RENEW LESSONS 
            loadLessons(listLessons.data, listLessons.meta.totalElements, true);
        }
        void loadLessons(Lesson[] lessons, int totalLessons, bool isRenewLessons)
        {
            if (isRenewLessons){
                // When need renew then we delete the old component FIRST
                 foreach(Transform child in contentForOrgansListLessons.transform){
                    GameObject.Destroy(child.gameObject);
                }
            }
           
            foreach(Lesson lesson in lessons)
            {

                // request = UnityWebRequestTexture.GetTexture(MediaUrl);
                GameObject xrLibraryLesson = Instantiate(Resources.Load(ItemConfig.totalLesson2) as GameObject, contentForOrgansListLessons.transform); 
                xrLibraryLesson.name = lesson.lessonId.ToString(); 
                xrLibraryLesson.transform.GetChild(1).gameObject.GetComponent<Text>().text = lesson.lessonTitle;
                xrLibraryLesson.transform.GetChild(2).GetChild(0).gameObject.GetComponent<Text>().text = lesson.viewed.ToString(); 
                xrLibraryLesson.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(lesson.lessonThumbnail.Split('.')[0].Substring(1));                
                xrLibraryLesson.transform.GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(() => InteractionUI.Instance.onClickItemLesson(lesson.lessonId));
            }
            if (totalLessons == 0)
            {
                sumLesson.transform.GetChild(0).gameObject.GetComponent<Text>().text = $"No results found."; 
            }
            else
            {
                sumLesson.transform.GetChild(0).gameObject.GetComponent<Text>().text = $"{totalLessons} lessons"; 
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System; 
using UnityEngine.Networking;

namespace Home {
    public class LoadScene : MonoBehaviour
    {
        public GameObject waitingScreen;
        public GameObject contentItemCategoryWithLesson;
        public GameObject contentItemCategory;
        
        public GameObject searchBox; //

        // add 3 record: searchBtn, xBtn, sumLesson
        public GameObject searchBtn; 
        public GameObject xBtn; 
        // private UnityWebRequest request;

        // Start is called before the first frame update
        void Start()
        {
            waitingScreen.SetActive(false);
            Screen.orientation = ScreenOrientation.Portrait; 
            // search record
            searchBtn.SetActive(true);
            xBtn.SetActive(false);
            xBtn.transform.GetComponent<Button>().onClick.AddListener(ClearInput); //

            // searchBox.GetComponent<InputField>().onValueChanged.AddListener(UpdateList); 

            LoadCategories();
            LessonByCategory();
        }

        // Update is called once per frame
        void Update()
        {
            if (searchBox.GetComponent<InputField>().isFocused == true){
                Debug.Log("Search box is focused !!!");
                PlayerPrefs.SetString("user_input", "");

                StartCoroutine(LoadAsynchronously(SceneConfig.xrLibrary));
            }
        }
        void LoadCategories() 
        {
            // Load from file json
            foreach (OrganForHome organ in LoadData.Instance.GetCategoryWithLesson().data)
            {
                GameObject categoryObject = Instantiate(Resources.Load(DemoConfig.demoItemCategoryPath) as GameObject);
                categoryObject.name = organ.organsId.ToString();
                categoryObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = organ.organsName;
                categoryObject.transform.parent = contentItemCategory.transform;
            }
        }

        // void UpdateList(string data)
        // {
        //     if(data == "")
        //     {
        //         xBtn.SetActive(false);
        //         searchBtn.SetActive(true);
        //     }
        //     else
        //     {
        //         // xBtn.SetActive(true);
        //         // searchBtn.SetActive(false);

        //         PlayerPrefs.SetString("user_input", data);
        //         SceneManager.LoadScene(SceneConfig.xrLibrary);

        //     }
        // }

        void ClearInput()
        {
            searchBox.GetComponent<InputField>().SetTextWithoutNotify(""); //
            xBtn.SetActive(false);
            searchBtn.SetActive(true);
        }

        void LessonByCategory() {

            // load from api

            // foreach(Category category in LoadData.Instance.GetCategories().data)
            // {
            //     string category_id = category.id;
            //     GameObject itemCategoryObject = Instantiate(Resources.Load(DemoConfig.demoItemCategoryWithLessonPath) as GameObject);
            //     itemCategoryObject.name = category.id.ToString();
            //     itemCategoryObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = category.name.ToUpper();
                
            //     itemCategoryObject.transform.parent = contentItemCategoryWithLesson.transform;
               
            //     GameObject subContent = itemCategoryObject.transform.GetChild(1).GetChild(0).GetChild(0).gameObject;
            //     var response = LoadData.Instance.GetLessonByCategory(category_id);
            //     if(response != null){
            //         foreach (Lesson lesson in LoadData.Instance.GetLessonByCategory(category_id).data){
            //             LoadLessons(subContent, lesson);
            //         }
            //     }
            // }


            //load from file json

            foreach (OrganForHome organ in LoadData.Instance.GetCategoryWithLesson().data)
            {
                GameObject itemCategoryObject = Instantiate(Resources.Load(DemoConfig.demoItemCategoryWithLessonPath) as GameObject);
                itemCategoryObject.name = organ.organsId.ToString();
                itemCategoryObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = organ.organsName;
                itemCategoryObject.transform.parent = contentItemCategoryWithLesson.transform;

                Button moreLessonBtn = itemCategoryObject.transform.GetChild(0).GetChild(1).gameObject.GetComponent<Button>();
                moreLessonBtn.onClick.AddListener(() => updateOrganManager(organ.organsId, organ.organsName));
         
                GameObject subContent = itemCategoryObject.transform.GetChild(1).GetChild(0).GetChild(0).gameObject;
                foreach (LessonForHome lesson in organ.listLesson)
                {
                    StartCoroutine(LoadLessons(subContent, lesson));
                }
            }
        }

        void updateOrganManager(int id, string name){
            OrganManager.InitOrgan(id, name);
            Debug.Log(id);

            // SceneNameManager.setPrevScene(SceneConfig.home_noSignIn);
            // SceneManager.LoadScene(SceneConfig.listOrgan);
            StartCoroutine(LoadAsynchronously(SceneConfig.listOrgan));
        }

        IEnumerator LoadLessons(GameObject parentObject, LessonForHome lesson) 
        {
            string imageUri = String.Format("https://api.xrcommunity.org/v1/xap/stores{0}", lesson.lessonThumbnail);
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUri);
            yield return request.SendWebRequest();

            while (!request.isDone)
            {
                yield return null;
            }
            
            Texture2D tex = ((DownloadHandlerTexture) request.downloadHandler).texture;
            Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(tex.width / 2, tex.height / 2));
            
            GameObject lessonObject = Instantiate(Resources.Load(DemoConfig.demoLessonObjectPath) as GameObject);
            lessonObject.name = lesson.lessonId.ToString();
            lessonObject.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Text>().text = lesson.lessonTitle;
            Debug.Log(lesson.lessonThumbnail.Split('.')[0].Substring(1));
            lessonObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite = sprite;

            // lessonObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(lesson.lessonThumbnail.Split('.')[0].Substring(1));
            // lessonObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Thumbnail/Lesson/Heart");

            lessonObject.transform.parent = parentObject.transform; 

            Button lessonBtn = lessonObject.GetComponent<Button>();
            lessonBtn.onClick.AddListener(() => InteractionUI.Instance.onClickItemLesson(lesson.lessonId));
        }

        public IEnumerator LoadAsynchronously(string sceneName)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
            waitingScreen.SetActive(true);
            while(!operation.isDone)
            {
                yield return new WaitForSeconds(.2f);
            }
        }

    }

   
}
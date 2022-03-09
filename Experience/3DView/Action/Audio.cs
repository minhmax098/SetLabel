using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Audio : MonoBehaviour
{
    private static Audio instance;
    public static Audio Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<Audio>();
            return instance;
        }
    }

    public GameObject timeCurrentAudio;
    public GameObject timeEndAudio;

    public GameObject sliderControlAudio;
    public Transform contentListAudio;

    public enum ActionAudioControl {
        Start,
        Pause,
        Unpause,
        Stop
    }
    AudioSource audioData;

    void Start()
    {
        
    }
    
    void Update()
    {
        if (audioData != null)
        {
            timeCurrentAudio.GetComponent<Text>().text = Helper.FormatTime(audioData.time);
            sliderControlAudio.GetComponent<Slider>().value = audioData.time;
        }
    }

    void ChangeAudioData(GameObject obj)
    {
        audioData = obj.GetComponent<AudioSource>();
        if (audioData != null)
        {
            timeEndAudio.GetComponent<Text>().text = Helper.FormatTime(audioData.clip.length);
            sliderControlAudio.GetComponent<Slider>().maxValue = audioData.clip.length;
        }
    }
    
    public void ActionAudio(GameObject controlGameObject, ActionAudioControl status)
    {

        ChangeAudioData(controlGameObject);
        
        switch (status)
        {
            case ActionAudioControl.Start:
            {
                audioData.Play();
                break;
            }
            case ActionAudioControl.Pause:
            {
                audioData.Pause();
                break;
            }
            case ActionAudioControl.Unpause:
            {
                audioData.UnPause();
                break;
            }
            case ActionAudioControl.Stop:
            {
                audioData.Stop();
                break;
            }
            default:
            {
                break;
            }
        }
    }

    public void GetAllAudioOfOrgan(GameObject obj)
    {
        int childCount = obj.transform.childCount;
        if (obj.GetComponent<AudioSource>() != null)
        {
            Debug.Log(obj.name + (obj.GetComponent<AudioSource>() == null));
            
            GameObject itemAudio = Instantiate(Resources.Load(PathResource.MODEL_ITEM_AUDIO) as GameObject);
            itemAudio.transform.SetParent(contentListAudio.transform, false);
            itemAudio.transform.GetChild(2).GetComponent<Text>().text = itemAudio.transform.GetChild(2).GetComponent<Text>().text + obj.name;
            itemAudio.name = obj.name;
            // itemAudio.transform.GetComponent<Button>().onClick.AddListener(delegate { HandleEventClickItemAudio(itemAudio); });

        }

        if (childCount > 0)
        {
            for (int i = 0; i < childCount; i++)
            {                
                GetAllAudioOfOrgan(obj.transform.GetChild(i).gameObject);
            }
        }
        else
        {
            return;
        }

    }

    void HandleEventClickItemAudio(GameObject itemAudio)
    {
        GameObject selected = GameObject.Find(itemAudio.name);
        Debug.Log(selected);
        // Display object selected
        ChildNodeManager.Instance.DisplaySelectedObject(selected);

        // Change current object
        GameObjectManager.Instance.SelectDoubleTouchObject(selected);
        
        // Append child node UI in tree
        ChildNodeManager.Instance.CreateChildNodeUI(selected.name);

        UIHandler.Instance.isClickedBtnAudio = true;
        UIHandler.Instance.HandlerBtnAudio();
    }
}

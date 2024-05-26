//System
using System.Collections;
using System.Collections.Generic;

//Unity
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//This script is for Generate Gukbab 
[DisallowMultipleComponent]
public class FireManager : MonoBehaviour
{
    //Count of Gukbab
    public int gukbabCount;

    //Timer Slider for Check Generate Gukbab
    [Header("타이머 슬라이더")]
    [SerializeField] private Slider timeSlider;

    //Usually Time for Cooking Gukbab
    [Header("국밥 요리에 걸리는 보통시간")]
    [SerializeField] private float usuallyCookingTime;

    //Slow Time for Cooking Gukbab
    [Header("국밥 요리에 걸리는 느린시간")]
    [SerializeField] private float slowCookingTime;

    //Current Time for Cooking Gukbab
    private float currentCookingTime;

    //Data
    private Data data;

    //Jumak Scene
    private JumakScene jumakScene;

    //TMP for fire Size
    [Header("불 크기를 나타낼 TMP")]
    [SerializeField] private TextMeshProUGUI fireSizeTMP;

    //Fire Image
    [Header("불 이미지")]
    [SerializeField] private Image fireImage;

    //Fire Image Decrease Rate
    [Header("불 이미지 감소 비율")]
    [SerializeField] private float decreaseRate;


    private void Start()
    {
        gukbabCount = 0;

        //Get Components
        data = DataManager.Instance.data;
        jumakScene = FindObjectOfType<JumakScene>();

        //Initialize Variables
        if (data.fireSize <= 50) currentCookingTime = slowCookingTime;
        else currentCookingTime = usuallyCookingTime;

    }

    private void Update()
    {
        //if jumakScene is started
        if (jumakScene.isStart)
        {
            //Fire System
            FireSystem();
        }
    }

    //This Method is to Generate Gukbab
    private void FireSystem()
    {
        //Rescale fire image
        fireImage.transform.localScale = Vector3.one * (data.fireSize / 100f);

        //Update fire size text
        fireSizeTMP.text = Mathf.FloorToInt(data.fireSize) + "%";

        //Timer Condition : current Cooking Time greater than 0, gukbab count less than 5
        if (currentCookingTime > 0f && gukbabCount < 5)
        {
            //Decrease Current Cooking Time
            currentCookingTime -= Time.deltaTime;

            //Decrease Fire Image Size
            data.fireSize -= decreaseRate * Time.deltaTime;
        }
        //Generate Gukbab Condition : cuurent Cooking Time below 0, gukbab count less than 5
        else if (currentCookingTime <= 0f && gukbabCount < 5)
        {
            //Increase Gukbab Count
            gukbabCount++;

            //Update currentCookingTime
            if (data.fireSize <= 50f) currentCookingTime = slowCookingTime;
            else currentCookingTime = usuallyCookingTime;
        }

        //Mediate fire size
        if (data.fireSize < 0f)
        {
            data.fireSize = 0f;
        }
        else if (data.fireSize > 100f)
        {
            data.fireSize = 100f;
        }

        //Update Time Slider Value
        if (data.fireSize <= 50) timeSlider.value = currentCookingTime / slowCookingTime;
        else timeSlider.value = currentCookingTime / slowCookingTime;
    }


}

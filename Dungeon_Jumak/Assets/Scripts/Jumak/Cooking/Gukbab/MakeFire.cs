//System
using System.Collections;
using System.Collections.Generic;

//Unity
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class MakeFire : MonoBehaviour
{
    //Touch Count
    [Header("������ Ŭ�� ��")]
    [SerializeField] private int touchCount = 0;

    //Fire Increase Rate
    [Header("�� ũ�� ����ġ")]
    [SerializeField] private float fireIncreaseRate = 5f;

    //Data
    private Data data;

    private void Start()
    {
        //Get Data
        data = DataManager.Instance.data;
    }

    void Update()
    {
        //Detect Touch
        if (Input.GetMouseButtonDown(0))
        {
            //add touch count
            touchCount++;

            //detect third touch, data.fireSize less than 100
            if (touchCount % 3 == 0 && data.fireSize <= 100)
            {
                GameManager.Sound.Play("[S] Make Fire", Define.Sound.Effect, false);

                //Increase Fire
                data.fireSize += fireIncreaseRate;

                //Initialize Touch Count
                touchCount = 0;
            }
        }
    }
}

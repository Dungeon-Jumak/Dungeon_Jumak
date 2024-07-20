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
    [Header("감지한 클릭 수")]
    [SerializeField] private int touchCount = 0;

    //Fire Increase Rate
    [Header("불 크기 증가치")]
    [SerializeField] private float fireIncreaseRate = 5f;

    [Header("파이어 매니저")]
    [SerializeField] private FireManager fireManager;

    void Update()
    {
        //Detect Touch
        if (Input.GetMouseButtonDown(0))
        {
            //add touch count
            touchCount++;

            //detect third touch, data.fireSize less than 100
            if (touchCount % 3 == 0 && fireManager.fireSize <= 100)
            {
                GameManager.Sound.Play("SFX/Jumak/[S] Make Fire", Define.Sound.Effect, false);

                //Increase Fire
                fireManager.fireSize += fireIncreaseRate;

                //Initialize Touch Count
                touchCount = 0;
            }
        }
    }
}

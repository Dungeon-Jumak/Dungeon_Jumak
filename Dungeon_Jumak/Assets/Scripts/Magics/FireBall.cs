using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class FireBall : MonoBehaviour
{
    public Image disable;

    private float coolTime = 1.0f;
    private float coolTime_max = 1.0f;
    private bool CoolCheck = false;

    private Data data;

    void Awake()
    {
        data = DataManager.Instance.data;
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (coolTime <= 0.0f)
        {
            CoolCheck = false;
        }
    }

    IEnumerator FireBallCoolTimeFunc()
    {
        while (coolTime > 0.0f)
        {
            coolTime -= 0.01f;

            disable.fillAmount = coolTime / coolTime_max;

            yield return new WaitForSeconds(0.01f);
        }
    }

    public void FireBallCoolStart()
    {

        if (CoolCheck == false)
        {
            coolTime = coolTime_max;
            disable.fillAmount = 1.0f; 

            StartCoroutine(FireBallCoolTimeFunc());

            FireBallMagic();
            CoolCheck = true;
        }
    }

    public void FireBallMagic()
    {
        data.monsterHP -= 0.5f;
    }
}

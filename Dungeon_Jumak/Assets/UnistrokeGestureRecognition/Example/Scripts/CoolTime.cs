using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Newtonsoft.Json.Bson;

public class CoolTime : MonoBehaviour
{
    private float coolTime = 1.0f;
    private float coolTime_max = 1.0f;
    private bool CoolCheck = false;
    public Image disable;


    IEnumerator CoolTimeFunc()
    {
        while (coolTime > 0.0f)
        {
            coolTime -= 0.01f;

            disable.fillAmount = coolTime / coolTime_max;

            yield return new WaitForSeconds(0.01f);
        }
    }

    private void Update()
    {
        if (coolTime <= 0.0f)
        {
            CoolCheck = false;
        }
    }

    public void CoolStart()
    {

        if (CoolCheck == false)
        {
            coolTime = coolTime_max;
            disable.fillAmount = 1.0f; 

            StartCoroutine(CoolTimeFunc());

            FireBallMagic();
            CoolCheck = true;
        }
    }

    public void FireBallMagic()
    {
        Debug.Log("µ¥¹ÌÁö : 0.5");
    }
}

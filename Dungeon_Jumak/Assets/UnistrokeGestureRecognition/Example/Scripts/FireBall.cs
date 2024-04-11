using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Newtonsoft.Json.Bson;

public class FireBall : MonoBehaviour
{
    public Image disable;
    public MonsterBuChu monsterBuChu;

    private float coolTime = 1.0f;
    private float coolTime_max = 1.0f;
    private bool CoolCheck = false;


    IEnumerator FireBallCoolTimeFunc()
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
        monsterBuChu.MonsterHPs -= 0.5f;
    }
}

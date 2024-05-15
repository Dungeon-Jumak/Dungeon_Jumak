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
    //private AudioManager audioManager;

    void Awake()
    {
        data = DataManager.Instance.data;
    }

    void Start()
    {
        //audioManager = FindObjectOfType<AudioManager>();
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

            StartCoroutine(FireBallMagic());
            CoolCheck = true;
        }
    }

    IEnumerator FireBallMagic()
    {
        /* if (data.isSound)
         {
             audioManager.Play("magicSketch");
             yield return new WaitForSeconds(1f);
             audioManager.Stop("magicSketch");

             audioManager.Play("fireBall");
         }*/
        DataManager.Instance.data.isSkillSuc = true;
        yield return null;
    }
}

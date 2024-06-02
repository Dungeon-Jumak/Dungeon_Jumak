using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Unity.VisualScripting;

public class MonsterBuChu : MonoBehaviour
{
    
    private Data data;

    //private AudioManager audioManager;
    void Awake()
    {
        data = DataManager.Instance.data;
    }

    private void Start()
    {
        //audioManager = FindObjectOfType<AudioManager>();
        StartCoroutine(StartBattle());
    }

    private void Update()
    {
        if (data.monsterHP <= 0)
        {
            data.monsterHP = 0;
            StopAllCoroutines();
        }
    }


    IEnumerator StartBattle()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            CallHpFunc();
        }
    }

    void CallHpFunc()
    {
        if (data.isSound)
            //audioManager.Play("damagedSound");
        data.playerHP -= 0.5f;
    }
}

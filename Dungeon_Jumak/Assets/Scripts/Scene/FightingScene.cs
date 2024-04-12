using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FightingScene : MonoBehaviour
{
    private Data data;

    void Awake()
    {
        data = DataManager.Instance.data;
    }

    void Update()
    {
        if(data.monsterHP == 0)
        {
            for (int i = 0; i < data.monsterSpawn.Length; i++)
            {
                data.monsterSpawn[i] = false;
            }

            if (data.isThirdMonster == true)
            {
                SceneManager.LoadScene("WaitingScene");
            }
            else
            {
                SceneManager.LoadScene("RunningScene");
            }
        }
        else if(data.playerHP == 0)
        {
            SceneManager.LoadScene("WaitingScene");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RunningScene : MonoBehaviour
{
    private Data data;

    void Awake()
    {
        data = DataManager.Instance.data;
    }

    void Update()
    {
        if(data.playerHP == 0)
        {
            SceneManager.LoadScene("WaitingScene");
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerController : MonoBehaviour
{
    public GameObject secTextObj;
    public GameObject minTextObj;

    [SerializeField]
    private float time = 120; // timer ÃÊ

    void Start()
    {
        DataManager.Instance.data.isTimerRun = true;
    }

    void Update()
    {
        countTime();
    }

    private void countTime()
    {
        //Timer running
        if (time > 0 && DataManager.Instance.data.isTimerRun == true)
        {
            //--Calculate time in scene--//
            time -= Time.deltaTime;

            int min = (int)time / 60;
            int sec = ((int)time - min * 60) % 60;

            secTextObj.GetComponent<TextMeshProUGUI>().text = sec.ToString("00");
            minTextObj.GetComponent<TextMeshProUGUI>().text = min.ToString("00");
        }
        //When Time end
        else if(time <= 0)
        {
            DataManager.Instance.data.isTimerRun = false;
        }
    }

    public void PauseTime()
    {
        DataManager.Instance.data.isTimerRun = false;
    }

    public void RestartTime()
    {
        DataManager.Instance.data.isTimerRun = true;
    }
}

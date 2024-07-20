//System
using System.Collections;
using System.Collections.Generic;

//Unity
using UnityEngine;
using UnityEngine.UI;

//TMPro
using TMPro;

[DisallowMultipleComponent]
public class DungeonScene : BaseScene
{
    [Header("타이머 슬라이더")]
    [SerializeField] private Slider timerSlider;

    [Header("경험치 슬라이더")]
    [SerializeField] private Slider xpSlider;

    [Header("경험치 텍스트")]
    [SerializeField] private TextMeshProUGUI xpText;

    [Header("레벨 텍스트")]
    [SerializeField] private TextMeshProUGUI levelText;

    [Header("던전 제한 시간")]
    [SerializeField] private float maxTimer;

    [Header("게임오버 팝업")]
    [SerializeField] private GameObject gameClear;

    [Header("타임오버 팝업")]
    [SerializeField] private GameObject timeOut;

    [Header("던전 배경 밤 레이어")]
    [SerializeField] private GameObject dunNightImg;

    [Header("헤더 밤 레이어")]
    [SerializeField] private GameObject headerNightImg;

    [SerializeField] private float currentTimer;

    private float lastXP;

    private bool isStop;

    private Data data;

    private void Start()
    {
        isStop = false;

        //Get Data
        data = DataManager.Instance.data;

        if (data.timeNum == 1)
        {
            dunNightImg.SetActive(true);
            headerNightImg.SetActive(true);
        }
        else
        {
            dunNightImg.SetActive(false);
            headerNightImg.SetActive(false);
        }

        //Init
        currentTimer = maxTimer;

        lastXP = data.curXP;

        //BGM
        GameManager.Sound.Play("BGM/[B] Dungeon Stage1", Define.Sound.Bgm);
    }

    private void Update()
    {
        if (currentTimer > 0 && !isStop)
        {
            //Timer
            currentTimer -= Time.deltaTime;
        }

        if (currentTimer <= 0 && !isStop)
        {
            isStop = true;
            TimeOut();
        }

        //Update Timer Value
        timerSlider.value = currentTimer / maxTimer;
        //Update Xp Value
        xpSlider.value = data.curXP / data.maxXP;

        LevelUp();
    }

    private void LevelUp()
    {
        if (data.curXP >= data.maxXP)
        {
            //Level Up
            data.curPlayerLV++;

            GameManager.Sound.Play("Dunjeon/[S] Level Up", Define.Sound.Effect, false);

            data.curXP = data.curXP - data.maxXP;

            //Update Max XP
            data.maxXP *= 5;
        }

        //if (lastXP < data.curXP)
        //{
        //    xpText.text = data.curXP.ToString() + " / " + data.maxXP.ToString();
        //}
    }

    public void TimeOut()
    {
        //Stop
        Time.timeScale = 0f;

        //Pop Up
        timeOut.SetActive(true);

        //Time System
        data.timeNum++;
        {
            if (data.timeNum >= data.time.Length)
            {
                data.timeNum = 0;

                //Increase Day Count
                data.day++;

                if (data.day > data.maxDay)
                {
                    //Init day
                    data.day = 1;

                    //Increase Season Number
                    data.seasonNum++;

                    //Init Season Number
                    if (data.seasonNum >= data.season.Length)
                    {
                        data.seasonNum = 0;

                        data.year++;
                    }
                }
            }
        }
    }

    public void GameClear()
    {
        //Stop
        Time.timeScale = 0f;

        //Pop Up
        gameClear.SetActive(true);

        //Time System
        data.timeNum++;
        {
            if (data.timeNum >= data.time.Length)
            {
                data.timeNum = 0;

                //Increase Day Count
                data.day++;

                if (data.day > data.maxDay)
                {
                    //Init day
                    data.day = 1;

                    //Increase Season Number
                    data.seasonNum++;

                    //Init Season Number
                    if (data.seasonNum >= data.season.Length)
                    {
                        data.seasonNum = 0;

                        data.year++;
                    }
                }
            }
        }
    }

    public void ConvertScene()
    {
        Time.timeScale = 1f;
        GameManager.Scene.LoadScene(Define.Scene.WaitingScene);
    }

    public override void Clear()
    {
        Debug.Log("DungeonScene Changed!");
    }

    public void ButtonClickSFX()
    {
        GameManager.Sound.Play("[S] Push Button", Define.Sound.Effect, false);
    }

    public void Pause()
    {
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
    }
}
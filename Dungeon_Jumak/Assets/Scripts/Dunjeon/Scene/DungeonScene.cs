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
    [Header("Ÿ�̸� �����̴�")]
    [SerializeField] private Slider timerSlider;

    [Header("����ġ �����̴�")]
    [SerializeField] private Slider xpSlider;

    [Header("����ġ �ؽ�Ʈ")]
    [SerializeField] private TextMeshProUGUI xpText;

    [Header("���� �ؽ�Ʈ")]
    [SerializeField] private TextMeshProUGUI levelText;

    [Header("���� ���� �ð�")]
    [SerializeField] private float maxTimer;

    [Header("���ӿ��� �˾�")]
    [SerializeField] private GameObject gameClear;

    private float currentTimer;

    private int lastLevel;

    private float lastXP;

    private Data data;

    private void Start()
    {
        //Init
        currentTimer = maxTimer;

        //Get Data
        data = DataManager.Instance.data;

        lastLevel = data.curPlayerLV;
        lastXP = data.curXP;
    }

    private void Update()
    {
        if(currentTimer <= 0)
        {
            currentTimer = 0f;

        }

        //Timer
        currentTimer -= Time.deltaTime;

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
            data.curXP = data.curXP - data.maxXP;

            //Update Max XP
            data.maxXP *= 2;
        }

        //when level update
        if (lastLevel < data.curPlayerLV)
        {
            lastLevel = data.curPlayerLV;
            levelText.text = "LV." + lastLevel.ToString();
        }

        if (lastXP < data.curXP)
        {
            xpText.text = data.curXP.ToString() + " / " + data.maxXP.ToString();
        }
    }

    public void GameClear()
    {
        //Stop
        Time.timeScale = 0f;

        //Pop Up
        gameClear.SetActive(true);
    }

    public void ConvertScene()
    {
        GameManager.Scene.LoadScene(Define.Scene.WaitingScene);
    }

    public override void Clear()
    {
        Debug.Log("DungeonScene Changed!");
    }
}

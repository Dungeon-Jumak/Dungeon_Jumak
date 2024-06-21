//System
using System.Collections;
using System.Collections.Generic;

//Unity
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class DungeonScene : MonoBehaviour
{
    [Header("타이머 슬라이더")]
    [SerializeField] private Slider timerSlider;

    [Header("던전 제한 시간")]
    [SerializeField] private float maxTimer;

    [Header("게임오버 팝업")]
    [SerializeField] private GameObject gameOverPopUp;

    [SerializeField]
    private float currentTimer;

    private void Start()
    {
        currentTimer = maxTimer;
    }

    private void Update()
    {
        currentTimer -= Time.deltaTime;

        timerSlider.value = currentTimer / maxTimer;
    }

    public void GameOver()
    {
        //Stop
        Time.timeScale = 0f;

        //Pop Up
        gameOverPopUp.SetActive(true);
    }

    public void ConvertScene()
    {
        GameManager.Scene.LoadScene(Define.Scene.WaitingScene);
    }


}

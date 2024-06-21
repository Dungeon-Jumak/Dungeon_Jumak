//System
using System.Collections;
using System.Collections.Generic;

//Unity
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class DungeonScene : MonoBehaviour
{
    [Header("Ÿ�̸� �����̴�")]
    [SerializeField] private Slider timerSlider;

    [Header("���� ���� �ð�")]
    [SerializeField] private float maxTimer;

    [Header("���ӿ��� �˾�")]
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

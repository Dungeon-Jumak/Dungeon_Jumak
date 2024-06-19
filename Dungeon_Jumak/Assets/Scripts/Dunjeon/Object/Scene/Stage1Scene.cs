using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Stage1Scene : BaseScene
{
    public GameObject resultPanel;
    public GameObject overPanel;

    public GameObject monsterPrefab;

    [SerializeField]
    private int numberOfMonsters = 4; // ������ ������ ��

    void Start()
    {
        SpawnMonsters();
    }

    void Update()
    {
        CheckGameResult();

        if(DataManager.Instance.data.isTimerRun == false)
        {
            overPanel.SetActive(true);
        }
    }

    //���� ���� ����
    void SpawnMonsters()
    {
        for (int i = 0; i < numberOfMonsters; i++)
        {
            Vector3 spawnPosition = new Vector3(
                Random.Range(-10f, 10f),
                Random.Range(-10f, 10f),
                0
            );
            Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);
        }
    }

    //���� ���� ��Ȳ Ȯ��
    private void CheckGameResult()
    {
        if (DataManager.Instance.data.ingredient[0] == 4)
        {
            resultPanel.SetActive(true);
        }
    }


    //Map Scene���� �̵�
    public void ConvertToMapScene()
    {
        GameManager.Scene.LoadScene(Define.Scene.Map);
    }

    //Waiting Scene���� �̵�
    public void ConvertToWaitingScene()
    {
        GameManager.Scene.LoadScene(Define.Scene.WaitingScene);
    }

    //��¥ �߰�
    public void AddDay()
    {
        DataManager.Instance.data.days++;
    }

    //Current Scene Settings
    protected override void Init()
    {
        SceneType = Define.Scene.Stage1;
    }

    //When convert to next scene
    public override void Clear()
    {
        Debug.Log("Stage1 Scene changed!");
    }
}

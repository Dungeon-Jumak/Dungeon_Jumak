using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DunjeonScene : MonoBehaviour
{
    public GameObject resultPanel;
    public GameObject monsterPrefab;

    public GameObject secTextObj;
    public GameObject minTextObj;

    private TextMeshProUGUI secText;
    private TextMeshProUGUI minText;

    [SerializeField]
    private float time = 120; // timer 초
    [SerializeField]
    private int numberOfMonsters = 4; // 스폰할 몬스터의 수

    [SerializeField]
    private Data data;

    void Start()
    {
        data = DataManager.Instance.data;

        secText = secTextObj.GetComponent<TextMeshProUGUI>();
        minText = minTextObj.GetComponent<TextMeshProUGUI>();

        SpawnMonsters();
    }

    void Update()
    {
        countTime();

        if (DataManager.Instance.data.ingredient[0] == 4)
        {
            resultPanel.SetActive(true);
        }
    }

    private void countTime()
    {
        if (time > 0)
        {
            time -= Time.deltaTime;

            int min = (int)time / 60;
            int sec = ((int)time - min * 60) % 60;

            secText.text = sec.ToString("00");
            minText.text = min.ToString("00");
        }
        else
        {
            resultPanel.SetActive(true);
        }
    }

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

    public void ChangeToMapScene()
    {
        GameManager.Scene.LoadScene(Define.Scene.Map);
    }

    public void AddDay()
    {
        DataManager.Instance.data.days++;
    }
}

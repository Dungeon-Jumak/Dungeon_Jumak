using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckMonster : MonoBehaviour
{
    public GameObject[] monsterPrefabs;
    public RectTransform canvasTransform;
    private Data data;
    void Awake()
    {
        data = DataManager.Instance.data;
    }
    void Start()
    {
        MonsterPrefabSpawn();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MonsterPrefabSpawn()
    {
        if (data.monsterSpawn[0] == true)
        {
            data.monsterHP = 2;
            Instantiate(monsterPrefabs[0], canvasTransform);
        }
    }
}

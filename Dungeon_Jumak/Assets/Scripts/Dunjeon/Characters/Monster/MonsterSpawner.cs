//System
using System.Collections;
using System.Collections.Generic;

//Unity
using UnityEngine;

[DisallowMultipleComponent]
public class MonsterSpawner : MonoBehaviour
{
    [Header("���� Ǯ �Ŵ��� ��ũ��Ʈ")]
    [SerializeField] private MonsterPoolManager poolManager;

    [Header("���� ���� �ð�")]
    [SerializeField] private float spawnDelay;

    [Header("���� ���� ����Ʈ���� �迭")]
    [SerializeField] private Transform[] spawnPoint;

    //Timer
    private float timer;

    //Level
    private int level;

    //Game Time
    private float gameTime;

    //Max Game Time
    private float maxGameTime = 2 * 10f;

    private void Awake()
    {
        //Load SpawnPoints
        spawnPoint = GetComponentsInChildren<Transform>();
    }

    private void Update()
    {
        //add timer
        timer += Time.deltaTime;

        //add gameTime
        gameTime += Time.deltaTime;

        //change level (10 seconds)
        level = Mathf.FloorToInt(gameTime / 10f);

        //Check game time greater than max game time
        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
        }

        if (timer > spawnDelay)
        {
            //Init Timer
            timer = 0;

            //Spawn
            Spawn();
        }
    }

    //Spawn Method
    private void Spawn()
    {
        //Pool Gets
        GameObject monster = poolManager.Get(Random.Range(0, 2));

        //Random Position
        monster.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
    }
}

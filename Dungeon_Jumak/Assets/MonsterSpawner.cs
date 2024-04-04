using UnityEngine;
using System.Collections;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject monsterPrefab; // 장애물 프리팹

    public float spawnDelay = 10f; // 장애물이 생성되는 시간 간격
    private float nextSpawnTime;

    void Update()
    {
        if (ShouldSpawn())
        {
            SpawnObstacle();
        }
    }

    bool ShouldSpawn()
    {
        return Time.time >= nextSpawnTime;
    }

    void SpawnObstacle()
    {
        nextSpawnTime = Time.time + spawnDelay;

        // 몬스터 생성
        Vector3 spawnPosition = new Vector3(0.0f, 7f, 0); // 중앙 레인
        Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);

        // 몬스터가 내려오는 텀을 랜덤하게 설정
        spawnDelay = Random.Range(1f, 10f); // 예시로 1초에서 3초 사이의 랜덤한 시간 설정
    }
}

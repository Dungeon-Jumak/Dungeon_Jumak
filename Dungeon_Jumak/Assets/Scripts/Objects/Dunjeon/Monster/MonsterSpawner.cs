using UnityEngine;
using System.Collections;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject monsterPrefab; // 장애물 프리팹
    public float spawnDelay = 10f; // 장애물이 생성되는 시간 간격

    private float nextSpawnTime;

    [SerializeField]
    private Data data;

    void Awake()
    {
        data = DataManager.Instance.data;
    }

    void Update()
    {
        if (ShouldSpawn() && data.isObstacle == false)
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

        if(data.isObstacle == false)
        {
            // 몬스터 생성
            Vector3 spawnPosition = new Vector3(28f, -0.7f, 13f); // 중앙 레인
            GameObject instance = Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);
            instance.transform.Rotate(0, 90, 0); // y축 기준으로 90도 회전

            data.isMonster = true;
        }

        // 몬스터가 내려오는 텀을 랜덤하게 설정
        spawnDelay = Random.Range(1f, 10f); // 예시로 1초에서 3초 사이의 랜덤한 시간 설정
    }
}

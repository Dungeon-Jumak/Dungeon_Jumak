using UnityEngine;
using System.Collections;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab; // 장애물 프리팹
    public GameObject recoveryPrefab; // 회복초 프리팹
    public float spawnDelay = 0.3f; // 장애물 생성 시간 간격

    private float nextSpawnTime;

    // 레인 위치 설정
    private Vector3[] lanePositions = {
        new Vector3(28f, -1.2f, 13.5f), // 왼쪽 레인
        new Vector3(28f, -1.2f, 13f), // 중앙 레인
        new Vector3(28f, -1.2f, 12.5f)  // 오른쪽 레인
    };

    [SerializeField]
    private Data data;

    void Awake()
    {
        data = DataManager.Instance.data;
    }

    void Update()
    {
        if (ShouldSpawn() && data.isMonster == false)
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

        // 랜덤한 레인 위치 선택
        int lane = Random.Range(0, lanePositions.Length);

        // 확률에 따라 프리팹 선택
        float chance = Random.Range(0f, 100f);
        GameObject prefabToSpawn = chance < 80 ? obstaclePrefab : recoveryPrefab;

        // 선택된 레인에 프리팹 생성
        Vector3 spawnPosition = lanePositions[lane] + transform.position;
        GameObject instance = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
        instance.transform.Rotate(0, 90, 0); // y축 기준으로 90도 회전

        // 장애물 생성 딜레이를 랜덤하게 설정
        spawnDelay = Random.Range(0.5f, 1.2f);
    }
}

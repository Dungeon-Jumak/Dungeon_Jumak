using UnityEngine;
using System.Collections;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab; // 장애물 프리팹

    public float spawnDelay = 2f; // 장애물이 생성되는 시간 간격
    private float nextSpawnTime;

    // 레인 별 위치 설정, 플레이어 스크립트에서 사용한 것과 동일하게 설정해야 합니다.
    private Vector3[] lanePositions = {
        new Vector3(-1.4f, 7f, 0), // 왼쪽 레인
        new Vector3(0.0f, 7f, 0), // 중앙 레인
        new Vector3(1.4f, 7f, 0)  // 오른쪽 레인
    };

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

        // 랜덤한 레인 위치 선택
        int lane = Random.Range(0, lanePositions.Length);

        // 선택된 레인에 장애물 생성
        Vector3 spawnPosition = lanePositions[lane] + transform.position; // transform.position을 추가하여, 스포너의 위치를 기준으로 함
        Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);

        // 장애물이 내려오는 텀을 랜덤하게 설정
        spawnDelay = Random.Range(1f, 6f); // 예시로 1초에서 3초 사이의 랜덤한 시간 설정
    }
}

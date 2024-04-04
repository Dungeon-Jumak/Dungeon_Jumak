using UnityEngine;
using System.Collections;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab; // ��ֹ� ������

    public float spawnDelay = 2f; // ��ֹ��� �����Ǵ� �ð� ����
    private float nextSpawnTime;

    // ���� �� ��ġ ����, �÷��̾� ��ũ��Ʈ���� ����� �Ͱ� �����ϰ� �����ؾ� �մϴ�.
    private Vector3[] lanePositions = {
        new Vector3(-1.4f, 7f, 0), // ���� ����
        new Vector3(0.0f, 7f, 0), // �߾� ����
        new Vector3(1.4f, 7f, 0)  // ������ ����
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

        // ������ ���� ��ġ ����
        int lane = Random.Range(0, lanePositions.Length);

        // ���õ� ���ο� ��ֹ� ����
        Vector3 spawnPosition = lanePositions[lane] + transform.position; // transform.position�� �߰��Ͽ�, �������� ��ġ�� �������� ��
        Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);

        // ��ֹ��� �������� ���� �����ϰ� ����
        spawnDelay = Random.Range(1f, 6f); // ���÷� 1�ʿ��� 3�� ������ ������ �ð� ����
    }
}

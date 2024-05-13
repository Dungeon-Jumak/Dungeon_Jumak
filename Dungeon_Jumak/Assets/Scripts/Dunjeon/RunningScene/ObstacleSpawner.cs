using UnityEngine;
using System.Collections;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab; // ��ֹ� ������
    public GameObject recoveryPrefab; // ȸ���� ������
    public float spawnDelay = 0.3f; // ��ֹ� ���� �ð� ����

    private float nextSpawnTime;

    // ���� ��ġ ����
    private Vector3[] lanePositions = {
        new Vector3(28f, -1.2f, 13.5f), // ���� ����
        new Vector3(28f, -1.2f, 13f), // �߾� ����
        new Vector3(28f, -1.2f, 12.5f)  // ������ ����
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

        // ������ ���� ��ġ ����
        int lane = Random.Range(0, lanePositions.Length);

        // Ȯ���� ���� ������ ����
        float chance = Random.Range(0f, 100f);
        GameObject prefabToSpawn = chance < 80 ? obstaclePrefab : recoveryPrefab;

        // ���õ� ���ο� ������ ����
        Vector3 spawnPosition = lanePositions[lane] + transform.position;
        GameObject instance = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
        instance.transform.Rotate(0, 90, 0); // y�� �������� 90�� ȸ��

        // ��ֹ� ���� �����̸� �����ϰ� ����
        spawnDelay = Random.Range(0.5f, 1.2f);
    }
}

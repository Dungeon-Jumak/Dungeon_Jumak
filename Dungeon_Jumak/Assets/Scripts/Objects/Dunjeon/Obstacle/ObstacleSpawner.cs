using UnityEngine;
using System.Collections;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab; // ��ֹ� ������
    public GameObject recoveryPrefab; // ȸ���� ������

    public float spawnDelay = 2f; // ��ֹ� ���� �ð� ����
    private float nextSpawnTime;

    // ���� ��ġ ����
    private Vector3[] lanePositions = {
        new Vector3(28f, -1.3f, 13.5f), // ���� ����
        new Vector3(28f, -1.3f, 13f), // �߾� ����
        new Vector3(28f, -1.3f, 12.5f)  // ������ ����
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

        // Ȯ���� ���� isObstacle ����
        data.isObstacle = prefabToSpawn == obstaclePrefab;

        // ��ֹ� ���� �����̸� �����ϰ� ����
        spawnDelay = Random.Range(1f, 6f);
    }
}

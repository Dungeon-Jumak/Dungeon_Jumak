using UnityEngine;
using System.Collections;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject monsterPrefab; // ��ֹ� ������
    public float spawnDelay = 10f; // ��ֹ��� �����Ǵ� �ð� ����

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

        // ���� ����
        Vector3 spawnPosition = new Vector3(0.0f, 7f, 0); // �߾� ����
        Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);
        data.isMonster = true;

        // ���Ͱ� �������� ���� �����ϰ� ����
        spawnDelay = Random.Range(1f, 10f); // ���÷� 1�ʿ��� 3�� ������ ������ �ð� ����
    }
}

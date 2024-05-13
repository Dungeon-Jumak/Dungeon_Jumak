using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject monsterPrefab; // ��ֹ� ������

    [SerializeField]
    private Data data;

    void Awake()
    {
        data = DataManager.Instance.data;
    }

    void Update()
    {
        if (data.isMonster)
        {
            SpawnObstacle();
        }
    }

    public void SpawnObstacle()
    {
        data.isMonster = false;

        Vector3 spawnPosition = new Vector3(28f, -0.7f, 13f); // �߾� ����
        Instantiate(monsterPrefab, spawnPosition, Quaternion.Euler(0, 90, 0)); // y�� �������� 90�� ȸ��
    }
}

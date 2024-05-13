using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject monsterPrefab; // 장애물 프리팹

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

        Vector3 spawnPosition = new Vector3(28f, -0.7f, 13f); // 중앙 레인
        Instantiate(monsterPrefab, spawnPosition, Quaternion.Euler(0, 90, 0)); // y축 기준으로 90도 회전
    }
}

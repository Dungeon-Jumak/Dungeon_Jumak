using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ProgressBarController : MonoBehaviour
{
    public GameObject progressBar;
    public GameObject monsterPrefab; // 몬스터 프리팹

    private Image progressImg;
    [SerializeField]
    private Data data;

    //---몬스터 생성 여부 변수---//
    private bool monsterSpawnedAt0_3 = false;
    private bool monsterSpawnedAt0_6 = false;
    private bool monsterSpawnedAt0_9 = false;

    void Awake()
    {
        data = DataManager.Instance.data;
    }

    private void Start()
    {
        progressImg = progressBar.GetComponent<Image>();
        StartCoroutine(UpdateProgressBar());
    }

    private IEnumerator UpdateProgressBar()
    {
        data.runningTime = 0f; // 현재 목표 값

        while (data.runningTime < 1.0f) //1.0이 될 때까지 반복
        {
            data.runningTime += 0.005f;
            progressImg.fillAmount = data.runningTime; // 이미지의 fillAmount 업데이트
            yield return new WaitForSeconds(0.2f);

            if (data.runningTime >= 0.29f && data.runningTime <= 0.31f && !monsterSpawnedAt0_3)
            {
                SpawnMonster();
                monsterSpawnedAt0_3 = true;
            }
            else if (data.runningTime >= 0.59f && data.runningTime <= 0.61f && !monsterSpawnedAt0_6)
            {
                SpawnMonster();
                monsterSpawnedAt0_6 = true;
            }
            else if (data.runningTime >= 0.89f && data.runningTime <= 0.91f && !monsterSpawnedAt0_9)
            {
                SpawnMonster();
                monsterSpawnedAt0_9 = true;
            }
        }
    }

    //---몬스터 spawn---//
    private void SpawnMonster()
    {
        Vector3 spawnPosition = new Vector3(28f, -0.7f, 13f); // 중앙 레인
        Instantiate(monsterPrefab, spawnPosition, Quaternion.Euler(0, 90, 0)); // y축 기준으로 90도 회전

        data.isMonster = true;
    }
}

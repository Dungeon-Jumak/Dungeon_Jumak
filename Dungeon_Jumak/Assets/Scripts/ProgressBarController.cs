using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ProgressBarController : MonoBehaviour
{
    public GameObject progressBar;
    public GameObject monsterPrefab; // ���� ������

    private Image progressImg;
    [SerializeField]
    private Data data;

    //---���� ���� ���� ����---//
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
        data.runningTime = 0f; // ���� ��ǥ ��

        while (data.runningTime < 1.0f) //1.0�� �� ������ �ݺ�
        {
            data.runningTime += 0.005f;
            progressImg.fillAmount = data.runningTime; // �̹����� fillAmount ������Ʈ
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

    //---���� spawn---//
    private void SpawnMonster()
    {
        Vector3 spawnPosition = new Vector3(28f, -0.7f, 13f); // �߾� ����
        Instantiate(monsterPrefab, spawnPosition, Quaternion.Euler(0, 90, 0)); // y�� �������� 90�� ȸ��

        data.isMonster = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumakManager : MonoBehaviour
{
    //---해금 할 단상 배열---//
    [SerializeField]
    private GameObject[] Dansangs;

    [SerializeField]
    private Data data;

    private void Start()
    {
        data = DataManager.Instance.data;
    }

    // Update is called once per frame
    void Update()
    {
        unlockTable();
    }

    //---게임 로드시 데이터 값에 따라 해금---//
    void unlockTable()
    {
        for (int i = 0; i < data.curUnlockLevel; i++)
        {
            if (Dansangs[i] != null) // 해당 게임 오브젝트가 파괴되지 않았는지 확인
            {
                Dansangs[i].SetActive(true);
            }
        }
    }
}

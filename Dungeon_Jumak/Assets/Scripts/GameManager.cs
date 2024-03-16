using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    Data data;

    //---해금 할 단상 배열---//
    [SerializeField]
    private GameObject[] Dansangs;

    // Start is called before the first frame update
    void Start()
    {
        data = DataManager.Instance.data;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(data.curUnlockLevel < data.maxUnlockLevel) 
            {
                data.curUnlockLevel++;
                data.maxSeatSize += 2;
                unlockTable();
            }

        }




    }

    //---게임 로드시 데이터 값에 따라 해금---//
    void unlockTable()
    {
        for (int i = 0; i < data.curUnlockLevel; i++)
        {
            Dansangs[i].SetActive(true);
        }
    }
}

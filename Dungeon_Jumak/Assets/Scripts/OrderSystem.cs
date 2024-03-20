using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderSystem : MonoBehaviour
{
    [SerializeField]
    private Transform[] tables;
    [SerializeField]
    private Data data;

    private void Start()
    {
        data = DataManager.Instance.data;
    }

    private void Update()
    {
        for (int i = 0; i < tables.Length; i++)
        {
            //---다 먹었으면 table을 초기화---//
            if (data.isFinEat[i])
            {
                data.isFinEat[i] = false;
                Debug.Log("국밥 파괴!");
                Destroy(tables[i].transform.GetChild(0).GetChild(0).gameObject);
                //아래에 코인 지급 추가 //
                DataManager.Instance.UpdateCoin();

            }

        }
    }


}

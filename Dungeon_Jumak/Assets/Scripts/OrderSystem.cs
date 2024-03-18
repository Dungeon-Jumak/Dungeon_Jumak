using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderSystem : MonoBehaviour
{
    [SerializeField]
    private Table[] tables;
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
                tables[i].isOnFood = false;
                data.isFinEat[i] = false;
                Destroy(tables[i].transform.GetChild(0).GetChild(0).gameObject);
                //아래에 코인 지급 추가 //

            }
            //---음식이 올라올 경우 데이터 값 변경---//
            if (tables[i].isOnFood)
                data.onTables[i] = true;

        }
    }


}

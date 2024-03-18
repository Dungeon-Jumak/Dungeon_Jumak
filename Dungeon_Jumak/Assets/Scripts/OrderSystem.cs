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
            //---�� �Ծ����� table�� �ʱ�ȭ---//
            if (data.isFinEat[i])
            {
                tables[i].isOnFood = false;
                data.isFinEat[i] = false;
                Destroy(tables[i].transform.GetChild(0).GetChild(0).gameObject);
                //�Ʒ��� ���� ���� �߰� //

            }
            //---������ �ö�� ��� ������ �� ����---//
            if (tables[i].isOnFood)
                data.onTables[i] = true;

        }
    }


}

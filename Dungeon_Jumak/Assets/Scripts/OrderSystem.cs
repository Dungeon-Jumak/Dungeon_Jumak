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
            //---�� �Ծ����� table�� �ʱ�ȭ---//
            if (data.isFinEat[i])
            {
                data.isFinEat[i] = false;
                Debug.Log("���� �ı�!");
                Destroy(tables[i].transform.GetChild(0).GetChild(0).gameObject);

                DataManager.Instance.UpdateCoin();//���� ����

            }

        }
    }


}

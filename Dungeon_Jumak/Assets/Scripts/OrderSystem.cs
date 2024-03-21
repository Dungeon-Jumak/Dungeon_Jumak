using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderSystem : MonoBehaviour
{
    [SerializeField]
    private Transform[] tables;
    [SerializeField]
    private Data data;
    [SerializeField]
    private AudioManager audioManager;
    [SerializeField]
    private string coinSound;

    private void Start()
    {
        data = DataManager.Instance.data;
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void Update()
    {
        for (int i = 0; i < tables.Length; i++)
        {
            //---�� �Ծ����� table�� �ʱ�ȭ---//
            if (data.isFinEat[i])
            {
                data.isFinEat[i] = false;
                //---���� ���� �ı� ���� ����---//
                if (tables[i].transform.GetChild(0).childCount != 0)
                    Destroy(tables[i].transform.GetChild(0).GetChild(0).gameObject);

                DataManager.Instance.UpdateCoin();//���� ����
                audioManager.Play(coinSound);
            }

        }
    }


}

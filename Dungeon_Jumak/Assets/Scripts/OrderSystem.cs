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
            //---다 먹었으면 table을 초기화---//
            if (data.isFinEat[i])
            {
                data.isFinEat[i] = false;
                //---국밥 이중 파괴 오류 수정---//
                if (tables[i].transform.GetChild(0).childCount != 0)
                    Destroy(tables[i].transform.GetChild(0).GetChild(0).gameObject);

                DataManager.Instance.UpdateCoin();//코인 지급
                audioManager.Play(coinSound);
            }

        }
    }


}

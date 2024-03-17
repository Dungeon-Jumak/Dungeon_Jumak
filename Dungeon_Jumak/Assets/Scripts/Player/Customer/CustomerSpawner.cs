using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    //---스폰 관련 변수---//
    [SerializeField]
    private GameObject[] customerPrefab; //손님 프리팹
    [SerializeField]
    private int customerNum;
    [SerializeField]
    private float delayTime; //스폰 딜레이 시간 = 10 - (maxSeatSize - curSeatSize)

    //---데이터---//
    [SerializeField]
    private Data data;

    private void Start()
    {
        data = DataManager.Instance.data;
        // --- 재귀 시작 --- //
        StartCoroutine(SpawnCustomer(delayTime));
    }

    // --- 코루틴 재귀 (delayTime마다 스폰) --- // 
    IEnumerator SpawnCustomer(float _delayTime)
    {
        float newDelayTime = _delayTime - (data.maxSeatSize - data.curSeatSize);
        Debug.Log("다음 손님이 오는 시간 : " + newDelayTime);

        yield return new WaitForSeconds(newDelayTime);

        var customer = ObjectPool.GetObject();
        if(data.curSeatSize < data.maxSeatSize) data.curSeatSize++; //현재 차있는 자리에 따라 스폰시간을 달리하기 위함

        StartCoroutine(SpawnCustomer(_delayTime));
    }


}

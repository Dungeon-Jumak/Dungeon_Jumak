using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    //---최소 스폰 시간과 최대 스폰 시간의 차이---//
    public int minMaxTerm; 

    //---스폰 관련 변수---//
    [SerializeField]
    private GameObject[] customerPrefab; //손님 프리팹
    [SerializeField]
    private int customerNum;

    [SerializeField]
    private int minDelayTime = 3; //최소 딜레이 시간
    [SerializeField]
    private int newMinDelayTime;  //업데이트 된 딜레이 시간

    //---데이터---//
    [SerializeField]
    private Data data;

    private void Start()
    {
        data = DataManager.Instance.data;
        minDelayTime = 3;

        // --- 재귀 시작 --- //
        StartCoroutine(SpawnCustomer());
    }

    // --- 코루틴 재귀 (delayTime마다 스폰) --- // 
    IEnumerator SpawnCustomer()
    {
        UpdateMinDelayTime();

        int newDelayTime = newMinDelayTime - (data.maxSeatSize - data.curSeatSize);
        int realDelayTime = Random.Range(newDelayTime, newDelayTime + minMaxTerm + 1);

        Debug.Log("다음 손님이 오는 시간 : " + realDelayTime);

        yield return new WaitForSeconds(realDelayTime);

        var customer = ObjectPool.GetObject();
        if(data.curSeatSize < data.maxSeatSize) data.curSeatSize++; //현재 차있는 자리에 따라 스폰시간을 달리하기 위함

        StartCoroutine(SpawnCustomer());
    }

    private void UpdateMinDelayTime()
    {
        newMinDelayTime = minDelayTime + data.maxSeatSize;
    }

}

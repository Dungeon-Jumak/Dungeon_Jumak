using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 일정 시간마다 손님을 스폰할 스크립트
/// </summary>
public class CustomerSpawner : MonoBehaviour
{
    [SerializeField, Header("최소 스폰 시간")]
    private float minDelayTime = 3f; //최소 딜레이 시간

    [SerializeField, Header("최대 시간과의 간격")]
    private float delayTerm; 

    [SerializeField]
    private GameObject[] customerPrefab; //손님 프리팹

    //---데이터---//
    private Data data;

    private void Start()
    {
        data = DataManager.Instance.data;
        // --- 재귀 시작 --- //
        StartCoroutine(SpawnCustomer());
    }

    // --- 손님 스폰 코루틴 재귀  --- // 
    IEnumerator SpawnCustomer()
    {
        // 최소 딜레이 시간과 간격 시간에서 랜덤으로 젠 시간 생성
        float realDelayTime = Random.Range(minDelayTime, minDelayTime + delayTerm + 1);

        //Debug.Log("다음 손님이 오는 시간 : " + realDelayTime);

        // 재 계산한 값만큼 대기
        yield return new WaitForSeconds(realDelayTime);

        // 오브젝트 풀링에서 풀을 가져옴
        var customer = ObjectPool.GetObject();

        // 손님의 HeadCount 증가
        if(data.customerHeadCount < data.maxSeatSize) data.customerHeadCount++;

        // 재귀
        StartCoroutine(SpawnCustomer());
    }
}

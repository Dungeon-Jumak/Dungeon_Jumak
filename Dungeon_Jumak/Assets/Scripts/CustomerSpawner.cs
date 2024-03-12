using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] customerPrefab; //손님 프리팹
    [SerializeField]
    private int customerNum;
    [SerializeField]
    private float SpawnDealy; //스폰 딜레이 시간

    private void Start()
    {
        // --- 재귀 시작 --- //
        StartCoroutine(SpawnCustomer(SpawnDealy));
    }

    // --- 코루틴 재귀 (delayTime마다 스폰) --- // 
    IEnumerator SpawnCustomer(float delayTime)
    {
        GameObject customerInstance = Instantiate(customerPrefab[customerNum], this.gameObject.transform);
        yield return new WaitForSeconds(SpawnDealy);
        StartCoroutine(SpawnCustomer(delayTime));
    }
}

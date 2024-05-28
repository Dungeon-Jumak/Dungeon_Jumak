// System
using System.Collections;
using System.Collections.Generic;

// Unity
using UnityEngine;

[DisallowMultipleComponent]
public class CustomerSpawner : MonoBehaviour
{
    //Min Spwan Delay
    [Header("�ּ� ���� �ð�")]
    [SerializeField] private float minDelayTime = 3f;

    //Term of min and max time
    [Header("�ִ� �ð����� ����")]
    [SerializeField] private float delayTerm; 

    //Data
    private Data data;

    private void Start()
    {
        //Get Data
        data = DataManager.Instance.data;

        //Start Coroutine
        StartCoroutine(SpawnCustomer());
    }

    //Recursion Coroutine for Spawn Customer
    IEnumerator SpawnCustomer()
    {
        //Update Random Delay Time
        float realDelayTime = Random.Range(minDelayTime, minDelayTime + delayTerm + 1);

        //Debug.Log("���� �մ��� ���� �ð� : " + realDelayTime);

        //Wait Delay Time
        yield return new WaitForSeconds(realDelayTime);

        //Object Pool -> Get Object
        var customer = ObjectPool.GetObject();

        // Increase headcount of customer
        if(data.customerHeadCount < data.maxSeatSize) data.customerHeadCount++;

        //Recursion
        StartCoroutine(SpawnCustomer());
    }
}

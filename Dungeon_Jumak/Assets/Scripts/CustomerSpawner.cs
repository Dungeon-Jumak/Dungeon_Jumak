using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] customerPrefab; //¼Õ´Ô ÇÁ¸®ÆÕ
    [SerializeField]
    private int customerNum;
    [SerializeField]
    private float SpawnDealy; //½ºÆù µô·¹ÀÌ ½Ã°£


    private void Start()
    {
        StartCoroutine(SpawnCustomer(SpawnDealy));
    }

    private void Update()
    {


    }

    IEnumerator SpawnCustomer(float delayTime)
    {
        GameObject customerInstance = Instantiate(customerPrefab[customerNum], this.gameObject.transform);
        yield return new WaitForSeconds(SpawnDealy);
        StartCoroutine(SpawnCustomer(delayTime));
    }
}

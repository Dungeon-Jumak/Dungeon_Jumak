using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] customerPrefab; //�մ� ������
    [SerializeField]
    private int customerNum;
    [SerializeField]
    private float SpawnDealy; //���� ������ �ð�

    private void Start()
    {
        // --- ��� ���� --- //
        StartCoroutine(SpawnCustomer(SpawnDealy));
    }

    // --- �ڷ�ƾ ��� (delayTime���� ����) --- // 
    IEnumerator SpawnCustomer(float delayTime)
    {
        GameObject customerInstance = Instantiate(customerPrefab[customerNum], this.gameObject.transform);
        yield return new WaitForSeconds(SpawnDealy);
        StartCoroutine(SpawnCustomer(delayTime));
    }
}

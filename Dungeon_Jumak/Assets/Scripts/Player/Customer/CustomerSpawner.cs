using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    //---�ּ� ���� �ð��� �ִ� ���� �ð��� ����---//
    public int minMaxTerm; 

    //---���� ���� ����---//
    [SerializeField]
    private GameObject[] customerPrefab; //�մ� ������
    [SerializeField]
    private int customerNum;
    [SerializeField]
    private int minDelayTime = 7;

    [SerializeField]
    private int maxSeatSize = 2;

    //---������---//
    [SerializeField]
    private Data data;

    private void Start()
    {
        data = DataManager.Instance.data;
        maxSeatSize = data.maxSeatSize;

        // --- ��� ���� --- //
        StartCoroutine(SpawnCustomer());
    }

    private void Update()
    {
        if (maxSeatSize < data.maxSeatSize)
        {
            minDelayTime += data.maxSeatSize - maxSeatSize;
            maxSeatSize = data.maxSeatSize;
        }
    }

    // --- �ڷ�ƾ ��� (delayTime���� ����) --- // 
    IEnumerator SpawnCustomer()
    {
        int newDelayTime = minDelayTime - (data.maxSeatSize - data.curSeatSize);
        int realDelayTime = Random.Range(newDelayTime, newDelayTime + minMaxTerm + 1);

        Debug.Log("���� �մ��� ���� �ð� : " + realDelayTime);

        yield return new WaitForSeconds(realDelayTime);

        var customer = ObjectPool.GetObject();
        if(data.curSeatSize < data.maxSeatSize) data.curSeatSize++; //���� ���ִ� �ڸ��� ���� �����ð��� �޸��ϱ� ����

        StartCoroutine(SpawnCustomer());
    }


}

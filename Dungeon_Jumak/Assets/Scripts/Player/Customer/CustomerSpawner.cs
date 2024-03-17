using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    //---���� ���� ����---//
    [SerializeField]
    private GameObject[] customerPrefab; //�մ� ������
    [SerializeField]
    private int customerNum;
    [SerializeField]
    private float delayTime; //���� ������ �ð� = 10 - (maxSeatSize - curSeatSize)

    //---������---//
    [SerializeField]
    private Data data;

    private void Start()
    {
        data = DataManager.Instance.data;
        // --- ��� ���� --- //
        StartCoroutine(SpawnCustomer(delayTime));
    }

    // --- �ڷ�ƾ ��� (delayTime���� ����) --- // 
    IEnumerator SpawnCustomer(float _delayTime)
    {
        float newDelayTime = _delayTime - (data.maxSeatSize - data.curSeatSize);
        Debug.Log("���� �մ��� ���� �ð� : " + newDelayTime);

        yield return new WaitForSeconds(newDelayTime);

        var customer = ObjectPool.GetObject();
        if(data.curSeatSize < data.maxSeatSize) data.curSeatSize++; //���� ���ִ� �ڸ��� ���� �����ð��� �޸��ϱ� ����

        StartCoroutine(SpawnCustomer(_delayTime));
    }


}

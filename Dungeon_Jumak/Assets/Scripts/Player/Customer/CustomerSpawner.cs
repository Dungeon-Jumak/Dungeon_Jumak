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
    private int minDelayTime = 3; //�ּ� ������ �ð�
    [SerializeField]
    private int newMinDelayTime;  //������Ʈ �� ������ �ð�

    //---������---//
    [SerializeField]
    private Data data;

    private void Start()
    {
        data = DataManager.Instance.data;
        minDelayTime = 3;

        // --- ��� ���� --- //
        StartCoroutine(SpawnCustomer());
    }

    // --- �ڷ�ƾ ��� (delayTime���� ����) --- // 
    IEnumerator SpawnCustomer()
    {
        UpdateMinDelayTime();

        int newDelayTime = newMinDelayTime - (data.maxSeatSize - data.curSeatSize);
        int realDelayTime = Random.Range(newDelayTime, newDelayTime + minMaxTerm + 1);

        Debug.Log("���� �մ��� ���� �ð� : " + realDelayTime);

        yield return new WaitForSeconds(realDelayTime);

        var customer = ObjectPool.GetObject();
        if(data.curSeatSize < data.maxSeatSize) data.curSeatSize++; //���� ���ִ� �ڸ��� ���� �����ð��� �޸��ϱ� ����

        StartCoroutine(SpawnCustomer());
    }

    private void UpdateMinDelayTime()
    {
        newMinDelayTime = minDelayTime + data.maxSeatSize;
    }

}

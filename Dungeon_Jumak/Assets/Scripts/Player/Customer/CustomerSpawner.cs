using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    //---�ּ� ���� �ð��� �ִ� ���� �ð��� ����---//
    public float minMaxTerm; 

    //---���� ���� ����---//
    [SerializeField]
    private GameObject[] customerPrefab; //�մ� ������
    [SerializeField]
    private int customerNum;

    [SerializeField]
    private float minDelayTime = 3f; //�ּ� ������ �ð�
    [SerializeField]
    private float newMinDelayTime;  //������Ʈ �� ������ �ð�

    //---������---//
    [SerializeField]
    private Data data;

    private void Start()
    {
        data = DataManager.Instance.data;

        // --- ��� ���� --- //
        StartCoroutine(SpawnCustomer());
    }

    // --- �ڷ�ƾ ��� (delayTime���� ����) --- // 
    IEnumerator SpawnCustomer()
    {
        UpdateMinDelayTime();

        float newDelayTime = newMinDelayTime - (data.maxSeatSize - data.curSeatSize);
        float realDelayTime = Random.Range(newDelayTime, newDelayTime + minMaxTerm + 1);

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

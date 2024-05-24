using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� �ð����� �մ��� ������ ��ũ��Ʈ
/// </summary>
public class CustomerSpawner : MonoBehaviour
{
    [SerializeField, Header("�ּ� ���� �ð�")]
    private float minDelayTime = 3f; //�ּ� ������ �ð�

    [SerializeField, Header("�ִ� �ð����� ����")]
    private float delayTerm; 

    [SerializeField]
    private GameObject[] customerPrefab; //�մ� ������

    //---������---//
    private Data data;

    private void Start()
    {
        data = DataManager.Instance.data;
        // --- ��� ���� --- //
        StartCoroutine(SpawnCustomer());
    }

    // --- �մ� ���� �ڷ�ƾ ���  --- // 
    IEnumerator SpawnCustomer()
    {
        // �ּ� ������ �ð��� ���� �ð����� �������� �� �ð� ����
        float realDelayTime = Random.Range(minDelayTime, minDelayTime + delayTerm + 1);

        //Debug.Log("���� �մ��� ���� �ð� : " + realDelayTime);

        // �� ����� ����ŭ ���
        yield return new WaitForSeconds(realDelayTime);

        // ������Ʈ Ǯ������ Ǯ�� ������
        var customer = ObjectPool.GetObject();

        // �մ��� HeadCount ����
        if(data.customerHeadCount < data.maxSeatSize) data.customerHeadCount++;

        // ���
        StartCoroutine(SpawnCustomer());
    }
}

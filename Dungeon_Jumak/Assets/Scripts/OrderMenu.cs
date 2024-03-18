using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderMenu : MonoBehaviour
{
    [Header("�޴� ��ǳ��")]
    [SerializeField]
    private GameObject[] speechBox;

    [Header("�޴� ���� Ȯ�� ����")]
    [SerializeField]
    private int percentOfMenu;
    [SerializeField]
    private int gukBabMax = 60;
    [SerializeField]
    private int riceJuiceMax = 80;
    [SerializeField]
    private int paJeonMax = 100;
    [SerializeField]
    private int min = 0;
    [SerializeField]
    private int max = 100;

    [Header("�޴� �ѹ���")]
    [SerializeField]
    private int gukBabNum = 0;
    [SerializeField]
    private int riceJuiceNum = 1;
    [SerializeField]
    private int paJeonNum = 2;

    [Header("���� ���� �� ����")]
    [SerializeField]
    private bool isOrder = false;
    [SerializeField]
    private bool isGukBab = false;
    [SerializeField]
    private bool isRiceJuice = false;
    [SerializeField]
    private bool isPaJeon = false;

    //---������---//
    [SerializeField]
    Data data;

    [SerializeField]
    private CustomerMovement customerMovement;

    void Start()
    {
        data = DataManager.Instance.data;

        //���� �޴� �ر� ������ ���� max���� �ٲ�
        switch (data.menuUnlockLevel)
        {
            case 1:
                max = 60; //����
                break;
            case 2:
                max = 80; //����
                break;
            case 3:
                max = 100; //����
                break;
        }


        customerMovement = GetComponent<CustomerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isOrder)
        {
            isOrder = true;


        }
    }

    //---�޴� �ֹ� ���� �Լ�---//
    public void OrderNewMenu()
    {
        int randNum = Random.Range(min, max);

        if (randNum >= min && randNum < gukBabMax)
            OrderGukBab();
        else if (randNum >= gukBabMax && randNum < riceJuiceMax)
            OrderRiceJuice();
        else if (randNum >= riceJuiceMax && randNum < paJeonMax)
            OrderPajeon();
       
    }

    //---���� �ֹ� �Լ�---// 
    void OrderGukBab()
    {
        transform.GetChild(gukBabNum).gameObject.SetActive(true);
        isGukBab = true;
    }

    //---���� �ֹ� �Լ�---//
    void OrderRiceJuice()
    {
        transform.GetChild(riceJuiceNum).gameObject.SetActive(true);
        isRiceJuice = true;
    }

    //---���� �ֹ� �Լ�---//
    void OrderPajeon()
    {
        transform.GetChild(paJeonNum).gameObject.SetActive(true);
        isRiceJuice = true;
    }
}

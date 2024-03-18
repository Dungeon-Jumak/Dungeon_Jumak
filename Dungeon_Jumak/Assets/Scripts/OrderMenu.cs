using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderMenu : MonoBehaviour
{
    [Header("메뉴 말풍선")]
    [SerializeField]
    private GameObject[] speechBox;

    [Header("메뉴 결정 확률 변수")]
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

    [Header("메뉴 넘버링")]
    [SerializeField]
    private int gukBabNum = 0;
    [SerializeField]
    private int riceJuiceNum = 1;
    [SerializeField]
    private int paJeonNum = 2;

    [Header("오더 관련 불 변수")]
    [SerializeField]
    private bool isOrder = false;
    [SerializeField]
    private bool isGukBab = false;
    [SerializeField]
    private bool isRiceJuice = false;
    [SerializeField]
    private bool isPaJeon = false;

    //---데이터---//
    [SerializeField]
    Data data;

    [SerializeField]
    private CustomerMovement customerMovement;

    void Start()
    {
        data = DataManager.Instance.data;

        //현재 메뉴 해금 레벨에 따라 max값을 바꿈
        switch (data.menuUnlockLevel)
        {
            case 1:
                max = 60; //국밥
                break;
            case 2:
                max = 80; //식혜
                break;
            case 3:
                max = 100; //파전
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

    //---메뉴 주문 결정 함수---//
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

    //---국밥 주문 함수---// 
    void OrderGukBab()
    {
        transform.GetChild(gukBabNum).gameObject.SetActive(true);
        isGukBab = true;
    }

    //---식혜 주문 함수---//
    void OrderRiceJuice()
    {
        transform.GetChild(riceJuiceNum).gameObject.SetActive(true);
        isRiceJuice = true;
    }

    //---파전 주문 함수---//
    void OrderPajeon()
    {
        transform.GetChild(paJeonNum).gameObject.SetActive(true);
        isRiceJuice = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class OrderMenu : MonoBehaviour
{
    public int menuNum;
    public bool isRun = true;

    [Header("메뉴 말풍선")]
    [SerializeField]
    private GameObject[] speechBox;

    [Header("메뉴 결정 확률 변수")]
    [SerializeField]
    private int percentOfMenu;
    [SerializeField]
    private int gukBabMax = 50;
    [SerializeField]
    private int riceJuiceMax = 70;
    [SerializeField]
    private int paJeonMax = 100;
    [SerializeField]
    private int min = 0;
    [SerializeField]
    private int max = 100;

    [Header("메뉴 넘버링")]
    [SerializeField]
    private int gukBabNum = 1;
    [SerializeField]
    private int riceJuiceNum = 2;
    [SerializeField]
    private int paJeonNum = 3;

    //---데이터---//
    [SerializeField]
    Data data;

    [SerializeField]
    private CustomerMovement customerMovement;

    void Start()
    {
        data = DataManager.Instance.data;
        customerMovement = GetComponent<CustomerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        //현재 메뉴 해금 레벨에 따라 max값을 바꿈
        switch (data.curMenuUnlockLevel)
        {
            case 1:
                max = 50; //국밥
                break;
            case 2:
                max = 70; //식혜
                break;
            case 3:
                max = 100; //파전
                break;
        }

        if (isRun)
        {
            //현재 자리에 음식이 올라갔다면
            if (data.onTables[customerMovement.seatIndex])
            {
                isRun = false;
                EatFood(data.menuNums[customerMovement.seatIndex] - 1);
            }
        }
    }

    //---메뉴 주문 결정 함수---//
    public void OrderNewMenu()
    {
        int randNum = Random.Range(min, max);

        if (randNum >= min && randNum < gukBabMax)
            data.menuNums[customerMovement.seatIndex] = gukBabNum;
        else if (randNum >= gukBabMax && randNum < riceJuiceMax)
            data.menuNums[customerMovement.seatIndex] = riceJuiceNum;
        else if (randNum >= riceJuiceMax && randNum < paJeonMax)
            data.menuNums[customerMovement.seatIndex] = paJeonNum;

        OrederSelectMenu(data.menuNums[customerMovement.seatIndex] - 1);

    }

    //주문 그림 보이게 하는 함수
    void OrederSelectMenu(int nums)
    {
        transform.GetChild(nums).gameObject.SetActive(true);
    }

    //말풍선 지우는 함수
    void EatFood(int nums)
    {
        transform.GetChild(nums).gameObject.SetActive(false);
        customerMovement.EatFood();
    }

    public void TimeOut()
    {
        transform.GetChild(data.menuNums[customerMovement.seatIndex] - 1).gameObject.SetActive(false);
        customerMovement.TimeOut();
    }

}

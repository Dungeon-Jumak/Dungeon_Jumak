using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class OrderMenu : MonoBehaviour
{
    public bool isEat;

    [Header("메뉴 결정 확률 변수")]
    [SerializeField]
    private int percentOfMenu;
    [SerializeField]
    private int gukBabMax;
    [SerializeField]
    private int paJeonMax;
    [SerializeField]
    private int riceJuiceMax;
    [SerializeField]
    private int min = 0;
    [SerializeField]
    private int max = 100;

    //---데이터---//
    [SerializeField]
    Data data;

    [SerializeField]
    private CustomerMovement customerMovement;

    void Start()
    {
        isEat = false;

        gukBabMax = 50;
        paJeonMax = 80;
        riceJuiceMax = 100;

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
                max = 80; //파전
                break;
            case 3:
                max = 100; //식혜
                break;
        }

        if (!isEat)
        {
            //현재 자리에 음식이 올라갔다면
            if (data.onTables[customerMovement.seatIndex])
            {
                isEat = true;
                EatFood(data.menuCategories[customerMovement.seatIndex]);
            }
        }
    }

    //---메뉴 주문 결정 함수---//
    public void OrderNewMenu()
    {
        int randNum = Random.Range(min, max); //랜덤 값

        //확률에 따라 카테고리 결정
        if (randNum >= min && randNum < gukBabMax)
            data.menuCategories[customerMovement.seatIndex] = "Gukbab";
        else if (randNum >= gukBabMax && randNum < paJeonMax)
            data.menuCategories[customerMovement.seatIndex] = "Pajeon";
        else if (randNum >= paJeonMax && randNum < riceJuiceMax)
            data.menuCategories[customerMovement.seatIndex] = "RiceJuice";

        OrederSelectMenu(data.menuCategories[customerMovement.seatIndex]);

    }

    //주문 그림 보이게 하는 함수
    void OrederSelectMenu(string _menuCategories)
    {
        switch (_menuCategories)
        {
            case "Gukbab":
                transform.GetChild(0).gameObject.SetActive(true);
                break;
            case "Pajeon":
                transform.GetChild(1).gameObject.SetActive(true);
                break;
            case "RiceJuice":
                transform.GetChild(2).gameObject.SetActive(true);
                break;
            default:
                Debug.Log("에러 : 없는 메뉴 카테고리를 선택했습니다");
                break;
        }

    }

    //말풍선 지우는 함수
    void EatFood(string _menuCategories)
    {

        switch (_menuCategories)
        {
            case "Gukbab":
                BubbleShadowController bsc0 = transform.GetChild(0).GetChild(1).GetComponent<BubbleShadowController>();
                bsc0.Initialize();
                transform.GetChild(0).gameObject.SetActive(false);
                break;
            case "Pajeon":
                BubbleShadowController bsc1 = transform.GetChild(1).GetChild(1).GetComponent<BubbleShadowController>();
                bsc1.Initialize();
                transform.GetChild(1).gameObject.SetActive(false);
                break;
            case "RiceJuice":
                BubbleShadowController bsc2 = transform.GetChild(2).GetChild(1).GetComponent<BubbleShadowController>();
                bsc2.Initialize();
                transform.GetChild(2).gameObject.SetActive(false);
                break;
            default:
                Debug.Log("에러 : EatFood(In OrderMenu)");
                break;
        }
        customerMovement.EatFood();
    }

    public void TimeOut()
    {
        switch (data.menuCategories[customerMovement.seatIndex])
        {
            case "Gukbab":
                transform.GetChild(0).gameObject.SetActive(false);
                break;
            case "Pajeon":
                transform.GetChild(1).gameObject.SetActive(false);
                break;
            case "RiceJuice":
                transform.GetChild(2).gameObject.SetActive(false);
                break;
            default:
                Debug.Log("에러 : Time Out(In OrderMenu)");
                break;
        }
        customerMovement.TimeOut();
    }

}

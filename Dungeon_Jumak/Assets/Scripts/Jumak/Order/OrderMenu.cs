using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class OrderMenu : MonoBehaviour
{
    //--- 음식 먹음을 감지하는 bool 변수 ---//
    public bool isEat;

    //--- 메뉴 결정 확률 변수 ---//
    [SerializeField]
    private int gukBabMax;          //--- 국밥 최대 확률 ---//
    [SerializeField]
    private int paJeonMax;          //--- 파전 최대 확률 ---//
    [SerializeField]
    private int riceJuiceMax;       //--- 식혜 최대 확률 ---//

    //--- 최소 확률 값과 최대 확률 값 변수 ---//
    private int min = 0;
    private int max = 100;

    //--- 데이터 ---//
    Data data;

    //--- CustomerMovement Componet ---//
    private CustomerMovement customerMovement;

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    void Start()
    {
        //--- Initialize Variables ---//
        isEat = false;

        gukBabMax = 50;
        paJeonMax = 80;
        riceJuiceMax = 100;

        //--- Get Component ---//
        data = DataManager.Instance.data;
        customerMovement = GetComponent<CustomerMovement>();
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    void Update()
    {
        //--- 현재 메뉴 해금 레벨에 따라 max값을 바꿈 ---//
        //*** 해금을 어떻게 추가 할 것인지에 대한 확실한 기획이 나오면 코드 변경 ***//
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

        //--- 음식을 먹고 있지 않을때 ---//
        if (!isEat)
        {
            //--- 현재 자리에 음식이 올라갔다면 ---//
            if (data.onTables[customerMovement.seatIndex])
            {
                //--- 중복 실행 방지 bool 변수 변환 ---//
                isEat = true;
                //--- EatFood 함수 실행 ---//
                EatFood(data.menuCategories[customerMovement.seatIndex]);
            }
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    //--- 메뉴의 카테고리를 결정하기 위한 함수 ---//
    public void OrderNewMenu()
    {
        //--- 음식의 랜덤 확률 변수를 갖고옴 ---//
        int randNum = Random.Range(min, max);

        //--- 확률에 따라 카테고리 결정 ---//
        if (randNum >= min && randNum < gukBabMax)
            data.menuCategories[customerMovement.seatIndex] = "Gukbab";
        else if (randNum >= gukBabMax && randNum < paJeonMax)
            data.menuCategories[customerMovement.seatIndex] = "Pajeon";
        else if (randNum >= paJeonMax && randNum < riceJuiceMax)
            data.menuCategories[customerMovement.seatIndex] = "RiceJuice";

        //--- 메뉴가 결정되면 해당되는 메뉴의 말풍선을 손님위에 띄우도록 함 ---//
        OrederSelectMenu(data.menuCategories[customerMovement.seatIndex]);

    }

    //--- 결정된 메뉴의 말풍선을 띄우기 위한 함수 ---//
    private void OrederSelectMenu(string _menuCategories)
    {
        //--- 메뉴 카테고리에 따라 Switch 문으로 분기를 나눔 ---//
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

    //--- 음식 먹음 신호를 감지하고 말풍선을 지우는 함수 ---//
    private void EatFood(string _menuCategories)
    {
        //--- 카테고리로 말풍선의 SetActive를 전환하는 분기문 ---//
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

        //--- CustomerMovement -> EatFood 메소드 실행 ---//
        customerMovement.EatFood();
    }

    //--- 시간이 다 되었을 때 말풍선을 끄고 손님이 돌아가게 하는 함수 ---//
    public void TimeOut()
    {
        //--- 카테고리에 따른 Switch 분기문 ---//
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

        //--- CustomerMovement -> TimeOut 메소드 실행 ---//
        customerMovement.TimeOut();
    }

}

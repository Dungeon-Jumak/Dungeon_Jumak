//System
using System.Collections;
using System.Collections.Generic;

//Unity
using UnityEngine;
using UnityEngine.UI;

//TMP
using TMPro;

[DisallowMultipleComponent]
public class ConfirmFood : MonoBehaviour
{
    //Want Coocking Food Count
    [Header("원하는 음식 조리 갯수")]
    public int wantCookingFood;

    //
    [Header("국밥을 생성하는 Gukbab Generator 스크립트")]
    [SerializeField] private GukbabGenerator gukbapSetting;

    //
    [Header("확정창으로 넘길 때 정보를 불러오기 위한 SetFood 스크립트")]
    [SerializeField] private SetFood setFood;

    //
    [Header("확정 팝업 오브젝트")]
    [SerializeField] private GameObject confirmPopUp;

    //
    [Header("음식 선택 패널 오브젝트")]
    [SerializeField] private GameObject chooseFoodPanel;
    
    //
    [Header("게임 시작 패널 오브젝트")]
    [SerializeField] private GameObject startPanel;

    //
    [Header("확정을 위한 버튼 배열")]
    [SerializeField] private Button[] confirmButtons;

    //
    [Header("현재 클릭한 확정 버튼")]
    [SerializeField] private Button curConfirmButton;

    //
    [Header("음식의 조리 갯수를 표시하기 위한 TMP")]
    [SerializeField] private TextMeshProUGUI countTMP;

    //
    [Header("팝업 가운데 들어가는 음식 이미지")]
    [SerializeField] private Image foodImage;

    //Max Cooking Count
    private int maxCookingFood;

    //Data
    private Data data;
    
    //Category
    private string category;

    //Check Base Food
    [SerializeField] private bool freeFood;

    //Check Start Game
    private bool gameStart;

    private void Start()
    {
        data = DataManager.Instance.data;

        for (int i = 0; i < confirmButtons.Length; i++)
            confirmButtons[i].interactable = true;

        freeFood = true;

        gameStart = false;
    }

    private void Update()
    {
       if(!confirmButtons[0].interactable && !confirmButtons[1].interactable && !confirmButtons[2].interactable && !gameStart)
        {
            gameStart = true;
            startPanel.SetActive(true);
            chooseFoodPanel.SetActive(false);
        }

        if (freeFood) countTMP.text = "무한";
        else countTMP.text = wantCookingFood.ToString();

    }

    //공용 함수 (increase / decrease)
    public void IncreaseCount()
    {
        if (!freeFood)
        {
            wantCookingFood++; //원하는 음식 갯수 증가

            //원하는 음식 갯수가 max값을 넘어가게 된다면 0으로 돌아감
            wantCookingFood %= maxCookingFood + 1;
        }
    }

    public void DecreaseCount()
    {
        if (!freeFood)
        {
            wantCookingFood--;

            if (wantCookingFood < 0)
                wantCookingFood = maxCookingFood;
        }
    }

    //국밥 확정버튼을 클릭했을 경우 창을 띄우고 이미지와 갯수 업데이트
    public void OnConfirmPopUp(string _category)
    {
        confirmPopUp.SetActive(true);

        switch (_category)
        {
            case "Gukbab":
                category = _category;
                maxCookingFood = setFood.gukbabMaxCount;
                foodImage.sprite = setFood.gukbabImage.sprite;

                freeFood = setFood.freeGukbab;

                curConfirmButton = confirmButtons[0];
                break;

            case "Pajeon":
                category = _category;
                maxCookingFood = setFood.pajeonMaxCount;
                foodImage.sprite = setFood.pajeonImage.sprite;

                freeFood = setFood.freePajeon;

                curConfirmButton = confirmButtons[1];
                break;

            case "RiceJuice":
                category = _category;
                maxCookingFood = setFood.riceJuiceMaxCount;
                foodImage.sprite = setFood.riceJuiceImage.sprite;

                freeFood = setFood.freeRiceJuice;

                curConfirmButton = confirmButtons[2];
                break;
        }

        wantCookingFood = maxCookingFood;

    }
    
    //국밥을 확정할 경우 버튼의 Interaction을 false로 변경하고 데이터의 재료를 감소시킴
    public void ConfirmCookFood()
    {
        switch (category)
        {
            case "Gukbab":
                if (!freeFood)
                {
                    setFood.subIngredient(category, wantCookingFood);
                    gukbapSetting.wantGukbabCount = wantCookingFood;
                }
                setFood.ButtonBlocker(category);
                break;

            case "Pajeon":
                if (!freeFood)
                {
                    setFood.subIngredient(category, wantCookingFood);
                }
                setFood.ButtonBlocker(category);
                break;

            case "RiceJuice":
                if (!freeFood)
                {
                    setFood.subIngredient(category, wantCookingFood);
                }
                setFood.ButtonBlocker(category);
                break;
        }
        curConfirmButton.interactable = false;
    }
}

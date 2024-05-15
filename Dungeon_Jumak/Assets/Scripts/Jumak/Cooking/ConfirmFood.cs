using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConfirmFood : MonoBehaviour
{
    public int wantCookingFood;

    [SerializeField]
    private GukbapSetting gukbapSetting;

    [SerializeField]
    private SetFood setFood;
    [SerializeField]
    private GameObject confirmPopUp;

    [SerializeField]
    private GameObject chooseFoodPanel;
    [SerializeField]
    private GameObject startPanel;

    //확정을 위한 버튼 배열
    [SerializeField]
    private Button[] confirmButtons;
    //현재 클릭한 확정 버튼
    [SerializeField]
    private Button curConfirmButton;

    [SerializeField]
    private TextMeshProUGUI countTMP;
    [SerializeField]
    private Image foodImage;

    private int maxCookingFood;

    Data data;
    
    private string category;

    [SerializeField]
    private bool freeFood;

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
                maxCookingFood = setFood.gukbabMaxNum;
                foodImage.sprite = setFood.gukbabImage.sprite;

                freeFood = setFood.freeGukbab;

                curConfirmButton = confirmButtons[0];
                break;

            case "Pajeon":
                category = _category;
                maxCookingFood = setFood.pajeonMaxNum;
                foodImage.sprite = setFood.pajeonImage.sprite;

                freeFood = setFood.freePajeon;

                curConfirmButton = confirmButtons[1];
                break;

            case "RiceJuice":
                category = _category;
                maxCookingFood = setFood.riceJuiceMaxNum;
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
        if (!freeFood)
        {
            switch (category)
            {
                case "Gukbab":
                    setFood.subIngredient("Gukbab", wantCookingFood);
                    gukbapSetting.wantCookingNum = wantCookingFood;
                    break;

                case "Pajeon":
                    setFood.subIngredient("Pajeon", wantCookingFood);
                    break;

                case "RiceJuice":
                    setFood.subIngredient("RiceJuice", wantCookingFood);
                    break;
            }
        }

        curConfirmButton.interactable = false;
    }
}

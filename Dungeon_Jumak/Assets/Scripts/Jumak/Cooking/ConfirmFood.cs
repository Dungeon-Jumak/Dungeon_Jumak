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

    //Ȯ���� ���� ��ư �迭
    [SerializeField]
    private Button[] confirmButtons;
    //���� Ŭ���� Ȯ�� ��ư
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

        if (freeFood) countTMP.text = "����";
        else countTMP.text = wantCookingFood.ToString();

    }

    //���� �Լ� (increase / decrease)
    public void IncreaseCount()
    {
        if (!freeFood)
        {
            wantCookingFood++; //���ϴ� ���� ���� ����

            //���ϴ� ���� ������ max���� �Ѿ�� �ȴٸ� 0���� ���ư�
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

    //���� Ȯ����ư�� Ŭ������ ��� â�� ���� �̹����� ���� ������Ʈ
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
    
    //������ Ȯ���� ��� ��ư�� Interaction�� false�� �����ϰ� �������� ��Ḧ ���ҽ�Ŵ
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

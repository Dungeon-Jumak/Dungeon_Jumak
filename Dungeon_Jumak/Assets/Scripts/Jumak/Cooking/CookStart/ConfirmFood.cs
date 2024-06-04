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
    [Header("���ϴ� ���� ���� ����")]
    public int wantCookingFood;

    //
    [Header("������ �����ϴ� Gukbab Generator ��ũ��Ʈ")]
    [SerializeField] private GukbabGenerator gukbapSetting;

    //
    [Header("Ȯ��â���� �ѱ� �� ������ �ҷ����� ���� SetFood ��ũ��Ʈ")]
    [SerializeField] private SetFood setFood;

    //
    [Header("Ȯ�� �˾� ������Ʈ")]
    [SerializeField] private GameObject confirmPopUp;

    //
    [Header("���� ���� �г� ������Ʈ")]
    [SerializeField] private GameObject chooseFoodPanel;
    
    //
    [Header("���� ���� �г� ������Ʈ")]
    [SerializeField] private GameObject startPanel;

    //
    [Header("Ȯ���� ���� ��ư �迭")]
    [SerializeField] private Button[] confirmButtons;

    //
    [Header("���� Ŭ���� Ȯ�� ��ư")]
    [SerializeField] private Button curConfirmButton;

    //
    [Header("������ ���� ������ ǥ���ϱ� ���� TMP")]
    [SerializeField] private TextMeshProUGUI countTMP;

    //
    [Header("�˾� ��� ���� ���� �̹���")]
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
    
    //������ Ȯ���� ��� ��ư�� Interaction�� false�� �����ϰ� �������� ��Ḧ ���ҽ�Ŵ
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

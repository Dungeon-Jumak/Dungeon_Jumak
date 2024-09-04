using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Furniture : MonoBehaviour
{
    private Data data;
    public ConfirmBuy confirmBuy;

    [Header("변경 버튼 배열")]
    public GameObject[] ChairChangeBtn;
    public GameObject[] TableChangeBtn;
    public GameObject[] BackgroundChangeBtn;

    [Header("적용 버튼 배열")]
    public GameObject[] ChairApplyBtn;
    public GameObject[] TableApplyBtn;
    public GameObject[] BackgroundApplyBtn;

    private int Level;

    void Awake()
    {
        data = DataManager.Instance.data;
    }

    void Start()
    {
        data.chairPrice = new int[] { 100, 250, 650 };
        data.tablePrice = new int[] { 150, 400, 900 };
        data.backgroundPrice = new int[] { 500, 1200, 3000 };
        data.curCoin = 1000000;
        UpdateButtonStates("Chair");
        UpdateButtonStates("Table");
        UpdateButtonStates("Background");
    }

    public void GetFurnitureLevel(int LV)
    {
        Level = LV;
    }

    public void TryBuyFurniture(string type)
    {
        int currentLevel = Level;
        int[] prices = GetPriceArray(type);
        bool[] checkBuyArray = GetCheckBuyArray(type);

        if (currentLevel < prices.Length && data.curCoin >= prices[currentLevel] && !checkBuyArray[currentLevel])
        {
            confirmBuy.Show(
                $"정말로 {type}을(를) 구매하시겠습니까? 가격: {prices[currentLevel]}",
                () => BuyFurniture(type),  // 확인 버튼 클릭 시 구매 수행
                () => { }  // 취소 버튼 클릭 시 아무 작업도 하지 않음
            );
        }
    }

    public void ChangeFurniture(string type, int LV)
    {
        SetFurnitureLevel(type, LV);
        UpdateButtonStates(type);
    }

    private void BuyFurniture(string type)
    {
        int currentLevel = Level;

        if (currentLevel < GetPriceArray(type).Length && data.curCoin >= GetPriceArray(type)[currentLevel])
        {
            if (!GetCheckBuyArray(type)[currentLevel])
            {
                data.curCoin -= GetPriceArray(type)[currentLevel];
                GetCheckBuyArray(type)[currentLevel] = true;
            }

            SetFurnitureLevel(type, currentLevel);
            UpdateButtonStates(type);
        }
    }

    private void UpdateButtonStates(string type)
    {
        GameObject[] changeButtons = GetChangeButtons(type);
        GameObject[] applyButtons = GetApplyButtons(type);
        bool[] checkBuyArray = GetCheckBuyArray(type);
        int currentLevel = GetCurrentLevel(type);

        foreach (var btn in changeButtons) btn.SetActive(false);
        foreach (var btn in applyButtons) btn.SetActive(false);

        for (int i = 0; i < checkBuyArray.Length; i++)
        {
            if (checkBuyArray[i])
            {
                if (currentLevel == i)
                {
                    applyButtons[i].SetActive(true);
                }
                else
                {
                    changeButtons[i].SetActive(true);
                }
            }
        }
    }

    private void SetFurnitureLevel(string type, int LV)
    {
        switch (type)
        {
            case "Chair":
                data.dansangLV = LV;
                break;
            case "Table":
                data.tableLV = LV;
                break;
            case "Background":
                data.houseLV = LV;
                break;
        }
    }

    private int[] GetPriceArray(string type)
    {
        switch (type)
        {
            case "Chair": return data.chairPrice;
            case "Table": return data.tablePrice;
            case "Background": return data.backgroundPrice;
            default: return new int[0];
        }
    }

    private bool[] GetCheckBuyArray(string type)
    {
        switch (type)
        {
            case "Chair": return data.checkBuyChair;
            case "Table": return data.checkBuyTable;
            case "Background": return data.checkBuyBackground;
            default: return new bool[0];
        }
    }

    private GameObject[] GetChangeButtons(string type)
    {
        switch (type)
        {
            case "Chair": return ChairChangeBtn;
            case "Table": return TableChangeBtn;
            case "Background": return BackgroundChangeBtn;
            default: return new GameObject[0];
        }
    }

    private GameObject[] GetApplyButtons(string type)
    {
        switch (type)
        {
            case "Chair": return ChairApplyBtn;
            case "Table": return TableApplyBtn;
            case "Background": return BackgroundApplyBtn;
            default: return new GameObject[0];
        }
    }

    private int GetCurrentLevel(string type)
    {
        switch (type)
        {
            case "Chair": return data.dansangLV;
            case "Table": return data.tableLV;
            case "Background": return data.houseLV;
            default: return -1;
        }
    }
}


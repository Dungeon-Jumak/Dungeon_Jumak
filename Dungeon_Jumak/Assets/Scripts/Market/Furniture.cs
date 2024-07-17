using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Furniture : MonoBehaviour
{
    private Data data;

    [Header("상품 텍스트")]
    public TextMeshProUGUI ChairText;
    public TextMeshProUGUI TableText;
    public TextMeshProUGUI BackgroundText;

    [Header("가격 텍스트")]
    public TextMeshProUGUI ChairPriceText;
    public TextMeshProUGUI TablePriceText;
    public TextMeshProUGUI BackgroundPriceText;

    void Awake()
    {
        data = DataManager.Instance.data;
    }

    void Start()
    {
        data.chairPrice = new int[] { 100, 250, 650 };
        data.tablePrice = new int[] { 150, 400, 900 };
        data.backgroundPrice = new int[] { 500, 1200, 3000 };

        ChairUpdateUI();
        TableUpdateUI();
        BackgroundUpdateUI();
    }

    public void ChairBuySystem()
    {
        if (data.dansangLV < data.chairPrice.Length - 1 && data.curCoin - data.chairPrice[data.dansangLV] >= 0)
        {
            data.curCoin -= data.chairPrice[data.dansangLV];
            data.dansangLV += 1;
            ChairUpdateUI();
        }
        else if (data.dansangLV >= data.chairPrice.Length)
        {
            Debug.Log("MAX");
            ChairUpdateUI();
        }
    }

    public void ChairUpdateUI()
    {
        if (data.dansangLV < data.chairPrice.Length - 1)
        {
            ChairPriceText.text = data.chairPrice[data.dansangLV].ToString() + "전";
        }
        else
        {
            ChairPriceText.text = "MAX";
        }

        switch (data.dansangLV)
        {
            case 0:
                ChairText.text = "낡은 마루";
                break;
            case 1:
                ChairText.text = "평범한 마루";
                break;
            case 2:
                ChairText.text = "고-급 마루";
                break;
            default:
                ChairText.text = "";
                break;
        }
    }

    public void TableBuySystem()
    {
        if (data.tableLV < data.tablePrice.Length - 1 && data.curCoin - data.tablePrice[data.tableLV] >= 0)
        {
            data.curCoin -= data.tablePrice[data.tableLV];
            data.tableLV += 1;
            TableUpdateUI();
        }
        else if (data.tableLV >= data.tablePrice.Length)
        {
            Debug.Log("MAX");
            TableUpdateUI();
        }
    }

    public void TableUpdateUI()
    {
        if (data.tableLV < data.tablePrice.Length - 1)
        {
            TablePriceText.text = data.tablePrice[data.tableLV].ToString() + "전";
        }
        else
        {
            TablePriceText.text = "MAX";
        }

        switch (data.tableLV)
        {
            case 0:
                TableText.text = "낡은 밥상";
                break;
            case 1:
                TableText.text = "평범한 밥상";
                break;
            case 2:
                TableText.text = "고-급 밥상";
                break;
            default:
                TableText.text = "";
                break;
        }
    }

    public void BackgroundBuySystem()
    {
        if (data.houseLV < data.backgroundPrice.Length - 1 && data.curCoin - data.backgroundPrice[data.houseLV] >= 0)
        {
            data.curCoin -= data.backgroundPrice[data.houseLV];
            data.houseLV += 1;
            BackgroundUpdateUI();
        }
        else if (data.houseLV >= data.backgroundPrice.Length)
        {
            Debug.Log("MAX");
            BackgroundUpdateUI();
        }
    }

    public void BackgroundUpdateUI()
    {
        if (data.houseLV < data.backgroundPrice.Length - 1)
        {
            BackgroundPriceText.text = data.backgroundPrice[data.houseLV].ToString() + "전";
        }
        else
        {
            BackgroundPriceText.text = "MAX";
        }

        switch (data.houseLV)
        {
            case 0:
                BackgroundText.text = "초가집";
                break;
            case 1:
                BackgroundText.text = "기와집";
                break;
            case 2:
                BackgroundText.text = "고-급 기와집";
                break;
            default:
                BackgroundText.text = "";
                break;
        }
    }
}

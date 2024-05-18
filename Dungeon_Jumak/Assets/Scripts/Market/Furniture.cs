using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Furniture : MonoBehaviour
{
    private Data data;
    public TextMeshPro chairPriceText;
    public TextMeshPro tablePriceText;
    public TextMeshPro backgroundPriceText;

    void Awake()
    {
        data = DataManager.Instance.data;
    }

    void Start()
    {
        data.curCoin = 1000;
        data.chairPrice = new int[] { 10, 20, 30 };
        data.tablePrice = new int[] { 20, 30, 40 };
        data.backgroundPrice = new int[] { 30, 40, 50 };

        ChairUI();
        TableUI();
        BackgroundUI();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChairBuySystem()
    {
        if (data.dansangLV < data.chairPrice.Length && data.curCoin - data.chairPrice[data.dansangLV] >= 0)
        {
            data.curCoin -= data.chairPrice[data.dansangLV];
            data.dansangLV += 1;
            ChairUI();
        }
        else if (data.dansangLV >= data.chairPrice.Length)
        {
            Debug.Log("MAX");
            ChairUI();
        }
    }

    public void ChairUI()
    {
        if (data.dansangLV < data.chairPrice.Length)
        {
            GameObject.Find("ChairPrice_Text").GetComponent<TextMeshProUGUI>().text = data.chairPrice[data.dansangLV].ToString();
        }
        else
        {
            GameObject.Find("ChairPrice_Text").GetComponent<TextMeshProUGUI>().text = "MAX";
        }

        switch (data.dansangLV)
        {
            case 0:
                GameObject.Find("Chair_Text").GetComponent<TextMeshProUGUI>().text = "평범한 마루";
                break;
            case 1:
                GameObject.Find("Chair_Text").GetComponent<TextMeshProUGUI>().text = "중급 마루";
                break;
            case 2:
                GameObject.Find("Chair_Text").GetComponent<TextMeshProUGUI>().text = "고급 마루";
                break;
            default:
                GameObject.Find("Chair_Text").GetComponent<TextMeshProUGUI>().text = "";
                break;
        }
    }

    public void TableBuySystem()
    {
        if (data.tableLV < data.tablePrice.Length && data.curCoin - data.tablePrice[data.tableLV] >= 0)
        {
            data.curCoin -= data.tablePrice[data.tableLV];
            data.tableLV += 1;
            TableUI();
        }
        else if (data.tableLV >= data.tablePrice.Length)
        {
            Debug.Log("MAX");
            TableUI();
        }
    }

    public void TableUI()
    {
        if (data.tableLV < data.tablePrice.Length)
        {
            GameObject.Find("TablePrice_Text").GetComponent<TextMeshProUGUI>().text = data.tablePrice[data.tableLV].ToString();
        }
        else
        {
            GameObject.Find("TablePrice_Text").GetComponent<TextMeshProUGUI>().text = "MAX";
        }

        switch (data.tableLV)
        {
            case 0:
                GameObject.Find("Table_Text").GetComponent<TextMeshProUGUI>().text = "낡은 상";
                break;
            case 1:
                GameObject.Find("Table_Text").GetComponent<TextMeshProUGUI>().text = "평범한 손님 상";
                break;
            case 2:
                GameObject.Find("Table_Text").GetComponent<TextMeshProUGUI>().text = "고-급 상";
                break;
            default:
                GameObject.Find("Table_Text").GetComponent<TextMeshProUGUI>().text = "";
                break;
        }
    }

    public void BackgroundBuySystem()
    {
        if (data.houseLV < data.backgroundPrice.Length && data.curCoin - data.backgroundPrice[data.houseLV] >= 0)
        {
            data.curCoin -= data.backgroundPrice[data.houseLV];
            data.houseLV += 1;
            BackgroundUI();
        }
        else if (data.houseLV >= data.backgroundPrice.Length)
        {
            Debug.Log("MAX");
            BackgroundUI();
        }
    }

    public void BackgroundUI()
    {
        if (data.houseLV < data.backgroundPrice.Length)
        {
            GameObject.Find("BackgroundPrice_Text").GetComponent<TextMeshProUGUI>().text = data.backgroundPrice[data.houseLV].ToString();
        }
        else
        {
            GameObject.Find("BackgroundPrice_Text").GetComponent<TextMeshProUGUI>().text = "MAX";
        }

        switch (data.houseLV)
        {
            case 0:
                GameObject.Find("Backgr_Text").GetComponent<TextMeshProUGUI>().text = "초가집";
                break;
            case 1:
                GameObject.Find("Backgr_Text").GetComponent<TextMeshProUGUI>().text = "기와집";
                break;
            case 2:
                GameObject.Find("Backgr_Text").GetComponent<TextMeshProUGUI>().text = "삐까뻔쩍 고급 기와집";
                break;
            default:
                GameObject.Find("Backgr_Text").GetComponent<TextMeshProUGUI>().text = "";
                break;
        }
    }
}

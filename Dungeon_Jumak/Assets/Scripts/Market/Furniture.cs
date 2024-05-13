using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Furniture : MonoBehaviour
{
    private Data data;
    public Text chairPriceText;
    public Text tablePriceText;
    public Text backgroundPriceText;


    void Awake()
    {
        data = DataManager.Instance.data;
    }

    void Start()
    {
        data.chairPrice[0] = 10;
        data.chairPrice[1] = 20;
        data.chairPrice[2] = 30;
        data.chairPrice[3] = 40;
        ChairUI();

        data.tablePrice[0] = 20;
        data.tablePrice[1] = 30;
        data.tablePrice[2] = 40;
        data.tablePrice[3] = 50;
        TableUI();

        data.backgroundPrice[0] = 30;
        data.backgroundPrice[1] = 40;
        data.backgroundPrice[2] = 50;
        data.backgroundPrice[3] = 60;
        BackgroundUI();

        data.curCoin = 1000;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChairBuySystem()
    {
        if (data.chairLevel < data.chairPrice.Length && data.curCoin - data.chairPrice[data.chairLevel] >= 0)
        {
            data.curCoin -= data.chairPrice[data.chairLevel];
            data.chairLevel += 1;
            ChairUI();
        }
        else if (data.chairLevel >= data.chairPrice.Length)
        {
            Debug.Log("MAX");
            ChairUI();
        }
    }

    public void ChairUI()
    {
        if (chairPriceText != null)
        {
            if (data.chairLevel < data.chairPrice.Length)
            {
                chairPriceText.text = data.chairPrice[data.chairLevel].ToString();
            }
            else
            {
                chairPriceText.text = "MAX";
            }
        }
        else
        {
            Debug.LogWarning("Chair Price Text is not assigned!");
        }
    }

    public void TableBuySystem()
    {
        if (data.tableLevel < data.tablePrice.Length && data.curCoin - data.tablePrice[data.tableLevel] >= 0)
        {
            data.curCoin -= data.tablePrice[data.tableLevel];
            data.tableLevel += 1;
            TableUI();
        }
        else if (data.tableLevel >= data.tablePrice.Length)
        {
            Debug.Log("MAX");
            TableUI();
        }
    }

    public void TableUI()
    {
        if (tablePriceText != null)
        {
            if (data.tableLevel < data.tablePrice.Length)
            {
                tablePriceText.text = data.tablePrice[data.tableLevel].ToString();
            }
            else
            {
                tablePriceText.text = "MAX";
            }
        }
        else
        {
            Debug.LogWarning("Table Price Text is not assigned!");
        }
    }

    public void BackgroundSystem()
    {
        if (data.backgroundLevel < data.backgroundPrice.Length && data.curCoin - data.backgroundPrice[data.backgroundLevel] >= 0)
        {
            data.curCoin -= data.backgroundPrice[data.backgroundLevel];
            data.backgroundLevel += 1;
            BackgroundUI();
        }
        else if (data.backgroundLevel >= data.backgroundPrice.Length)
        {
            Debug.Log("MAX");
            BackgroundUI();
        }
    }

    public void BackgroundUI()
    {
        if (backgroundPriceText != null)
        {
            if (data.backgroundLevel < data.backgroundPrice.Length)
            {
                backgroundPriceText.text = data.backgroundPrice[data.backgroundLevel].ToString();
            }
            else
            {
                backgroundPriceText.text = "MAX";
            }
        }       
        else
        {
            Debug.LogWarning("background Price Text is not assigned!");
        }
    }
}
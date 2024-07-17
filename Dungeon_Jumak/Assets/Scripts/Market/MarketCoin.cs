using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class MarketCoin : MonoBehaviour
{
    private Data data;

    public TextMeshProUGUI CoinText;

    void Awake()
    {
        data = DataManager.Instance.data;
    }

    void Update()
    {
        CoinText.text = data.curCoin.ToString();
    }
}

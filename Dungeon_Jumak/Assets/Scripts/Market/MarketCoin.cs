using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class MarketCoin : MonoBehaviour
{
    private Data data;

    void Awake()
    {
        data = DataManager.Instance.data;
        //data.curCoin = 100000;
    }

}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MarketCoin : MonoBehaviour
{
    private Data data;

    void Awake()
    {
        data = DataManager.Instance.data;
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        TextMeshProUGUI textMeshPro = GetComponent<TextMeshProUGUI>();
        if (textMeshPro != null)
        {
            textMeshPro.text = data.curCoin.ToString();
        }
        else
        {
            Text text = GetComponent<Text>();
            if (text != null)
            {
                text.text = data.curCoin.ToString();
            }
            else
            {
                Debug.LogError("MarketCoin script is attached to an object without TextMeshPro or Text component!");
            }
        }
    }
}

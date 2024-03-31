using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class JumakScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        //---Define.cs enum ���� ���� Scene ����---//
        SceneType = Define.Scene.Jumak;

        //---�� �ٽ� �ε� �Ǹ� �ٷ� Data�� coin �ε�---//
        GameObject.Find("UI_CoinText").GetComponent<TextMeshProUGUI>().text = DataManager.Instance.data.curCoin.ToString();
    }

    public void Update()
    {
        UpdateCoin();
        UpdateLevel();
    }

    public override void Clear()
    {

    }

    // --- ���� ���� �Լ� --- //
    public void UpdateLevel()
    {
        GameObject.Find("UI_LevelText").GetComponent<TextMeshProUGUI>().text = DataManager.Instance.data.curPlayerLV.ToString();
    }

    // --- ���� ���� --- //
    public void UpdateCoin()
    {
        GameObject.Find("UI_CoinText").GetComponent<TextMeshProUGUI>().text = DataManager.Instance.data.curCoin.ToString();
    }
}

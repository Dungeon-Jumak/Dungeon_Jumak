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

    public override void Clear()
    {

    }
}

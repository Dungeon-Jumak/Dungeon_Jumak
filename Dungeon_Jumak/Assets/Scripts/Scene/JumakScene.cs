using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class JumakScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        //---Define.cs enum 통한 현재 Scene 설정---//
        SceneType = Define.Scene.Jumak;

        //---씬 다시 로드 되면 바로 Data의 coin 로드---//
        GameObject.Find("UI_CoinText").GetComponent<TextMeshProUGUI>().text = DataManager.Instance.data.curCoin.ToString();
    }

    public override void Clear()
    {

    }
}

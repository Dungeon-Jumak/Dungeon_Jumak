using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class WaitingScene : BaseScene
{
    [SerializeField]
    private Data data;

    void Start()
    {
        data = DataManager.Instance.data;
        GameManager.Sound.Play("Wait", Define.Sound.Bgm);
    }

    protected override void Init()
    {
        SceneType = Define.Scene.WaitingScene;
    }

    public override void Clear()
    {
        Debug.Log("Waiting Scene changed!");
    }
}

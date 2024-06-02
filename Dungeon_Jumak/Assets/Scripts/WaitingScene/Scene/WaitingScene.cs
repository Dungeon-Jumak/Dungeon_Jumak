using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class WaitingScene : BaseScene
{
    void Start()
    {
        //GameManager.Sound.Play("Wait", Define.Sound.Bgm);//Play The Bgm Sound in WaitingScene
    }

    //Current Scene Settings
    protected override void Init()
    {
        SceneType = Define.Scene.WaitingScene;
    }

    //Click Sound
    public void ClickSound()
    {
        //GameManager.Sound.Play("PickFood", Define.Sound.Effect);
    }

    //When convert to next scene
    public override void Clear()
    {
        Debug.Log("Waiting Scene changed!");
    }
}

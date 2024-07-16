using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScene : BaseScene
{
    private Data data;

    void Start()
    {
        data = DataManager.Instance.data;//Data.cs
        GameManager.Sound.Play("BGM/[B] Shop", Define.Sound.Bgm, true);
    }

    //Convert to WaitingScene
    public void ConvertScene()
    {
        GameManager.Scene.LoadScene(Define.Scene.WaitingScene);
    }

    //Click Sound
    public void ButtonClickSFX()
    {
        GameManager.Sound.Play("[S] Push Button", Define.Sound.Effect, false);
    }

    //When convert to next scene
    public override void Clear()
    {
        Debug.Log("StartScene Scene changed!");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScene : BaseScene
{
    [SerializeField]
    private Data data;

    void Start()
    {
        data = DataManager.Instance.data;//Data.cs
        GameManager.Sound.Play("BGM/[B] Shop", Define.Sound.Bgm);
        GameManager.Sound.Play("[S] Hit", Define.Sound.Effect, true);
    }

    //Convert to WaitingScene
    public void ConvertScene()
    {
        data.timerStart = true;
        GameManager.Scene.LoadScene(Define.Scene.WaitingScene);
    }

    //Click Sound
    public void ClickSound()
    {
        GameManager.Sound.Pause("[S] Hit", Define.Sound.Effect);
    }

    //When convert to next scene
    public override void Clear()
    {
        Debug.Log("StartScene Scene changed!");
    }
}

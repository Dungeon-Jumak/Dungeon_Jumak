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
        //GameManager.Sound.Play("13 Victory", Define.Sound.Bgm);//Play The Bgm Sound in StartScene
    }

    public void ConvertScene()
    {
        GameManager.Scene.LoadScene(Define.Scene.WaitingScene);
    }

    //Click Sound
    public void ClickSound()
    {
        //GameManager.Sound.Play("PickFood", Define.Sound.Effect);
    }

    //When convert to next scene
    public override void Clear()
    {
        Debug.Log("StartScene Scene changed!");
    }
}

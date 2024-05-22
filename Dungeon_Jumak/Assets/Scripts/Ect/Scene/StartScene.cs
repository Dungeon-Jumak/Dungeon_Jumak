using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScene : BaseScene
{
    [SerializeField]
    private Data data;

    void Start()
    {
        data = DataManager.Instance.data;
        GameManager.Sound.Play("13 Victory", Define.Sound.Bgm);
    }

    public void ConvertScene()
    {
        GameManager.Scene.LoadScene(Define.Scene.WaitingScene);
    }

    public void ClickSound()
    {
        GameManager.Sound.Play("PickFood", Define.Sound.Effect);
    }

    //씬 전환할 때 필요한 기능
    public override void Clear()
    {
        Debug.Log("StartScene Scene changed!");
    }
}

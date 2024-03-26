using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CominScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        //---Define.cs enum 통한 현재 Scene 설정---//
        SceneType = Define.Scene.ComingSoon;
    }

    public override void Clear()
    {

    }

    //---UI 자동화 전이라 임의로 만들어놓은 함수에용---//
    public void cangeToJumakScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Jumak");
    }
}
